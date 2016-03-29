using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class ApproveThirdPartyInvoiceCommandHandler : IDomainServiceCommandHandler<ApproveThirdPartyInvoiceCommand, ThirdPartyInvoiceApprovedEvent>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<ThirdPartyInvoiceModel> thirdPartyInvoiceDomainRuleManager;
        private IDomainRuleManager<ServiceRequestMetadata> metadataDomainRuleManager;
        private IThirdPartyInvoiceApprovalMandateProvider approvalMandateProvider;
        private IThirdPartyInvoiceManager thirdPartyInvoiceManager;

        public ApproveThirdPartyInvoiceCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager, IEventRaiser eventRaiser,
            IServiceCommandRouter serviceCommandRouter, IThirdPartyInvoiceApprovalMandateProvider approvalMandateProvider,
            IDomainRuleManager<ThirdPartyInvoiceModel> thirdPartyInvoiceDomainRuleManager, IDomainRuleManager<ServiceRequestMetadata> metadataDomainRuleManager,
            IThirdPartyInvoiceManager thirdPartyInvoiceManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.eventRaiser = eventRaiser;
            this.thirdPartyInvoiceDomainRuleManager = thirdPartyInvoiceDomainRuleManager;
            this.metadataDomainRuleManager = metadataDomainRuleManager;
            this.approvalMandateProvider = approvalMandateProvider;
            this.thirdPartyInvoiceManager = thirdPartyInvoiceManager;

            thirdPartyInvoiceDomainRuleManager.RegisterRule(new InvoiceMustHaveAtLeastOneLineItemRule());
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new ThirdPartyMustBeCapturedAgainstTheInvoiceRule(thirdPartyInvoiceDataManager));
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new InvoiceNumberMustBeCapturedRule(thirdPartyInvoiceDataManager));
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new InvoiceDateMustBeCapturedRule(thirdPartyInvoiceDataManager));
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new UserWithApproverAboveR60000CapabilityCanApproveAnyInvoicesAmountRule(approvalMandateProvider));
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new UserWithApproverUnderR15000CapabilityCannotApproveInvoicesGreaterThanR15000Rule(approvalMandateProvider));
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new UserWithApproverUnderR30000CapabilityCannotApproveInvoicesGreaterThanR30000Rule(approvalMandateProvider));
            thirdPartyInvoiceDomainRuleManager.RegisterRule(new UserWithApproverUpToR60000CapabilityCannotApproveInvoicesGreaterThanR60000Rule(approvalMandateProvider));
            metadataDomainRuleManager.RegisterRule(new AllCapabilitiesMustBeLinkedToUserRule(thirdPartyInvoiceDataManager));
        }

        public ISystemMessageCollection HandleCommand(ApproveThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var capabilityWithHigherMandate = approvalMandateProvider.GetCapabilityWithHigherMandate(metadata.CurrentUserCapabilities);
            if (string.IsNullOrEmpty(capabilityWithHigherMandate))
            {
                messages.AddMessage(new SystemMessage(string.Format("{0} has no invoice approving capabilities", metadata.UserName), SystemMessageSeverityEnum.Error));
                return messages;
            }
            metadataDomainRuleManager.ExecuteRules(messages, (ServiceRequestMetadata)metadata);
            if (messages.HasErrors)
            {
                return messages;
            }
            var thirdPartyInvoiceModel = thirdPartyInvoiceManager.GetThirdPartyInvoiceModel(command.ThirdPartyInvoiceKey);

            thirdPartyInvoiceModel.ApproverCurrentUserCapabilities = capabilityWithHigherMandate;
            thirdPartyInvoiceDomainRuleManager.ExecuteRules(messages, thirdPartyInvoiceModel);

            if (!messages.HasErrors)
            {
                thirdPartyInvoiceDataManager.UpdateThirdPartyInvoiceStatus(thirdPartyInvoiceModel.ThirdPartyInvoiceKey, InvoiceStatus.Approved);

                string approverUserName = metadata.UserName;
                decimal approvedInvoiceAmount = thirdPartyInvoiceModel.LineItems.Sum(li => li.TotalAmountIncludingVAT);

                var @event = new ThirdPartyInvoiceApprovedEvent(
                        DateTime.Now
                    , thirdPartyInvoiceModel
                    , approvedInvoiceAmount
                    , approverUserName
                    , capabilityWithHigherMandate
                    );
                eventRaiser.RaiseEvent(DateTime.Now, @event, thirdPartyInvoiceModel.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }

            return messages;
        }


    }
}
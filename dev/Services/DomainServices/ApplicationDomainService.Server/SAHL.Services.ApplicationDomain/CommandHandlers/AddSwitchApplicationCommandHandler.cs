using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddSwitchApplicationCommandHandler : IDomainServiceCommandHandler<AddSwitchApplicationCommand, SwitchApplicationAddedEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IApplicationDataManager applicationDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;
        private IApplicationManager applicationManager;
        private IDomainRuleManager<SwitchApplicationModel> domainRuleContext;

        public AddSwitchApplicationCommandHandler(IServiceCommandRouter serviceCommandRouter, IApplicationDataManager applicationDataManager, ILinkedKeyManager linkedKeyManager,
            IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser, IApplicationManager applicationManager, IDomainRuleManager<SwitchApplicationModel> domainRuleContext)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.applicationDataManager = applicationDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
            this.applicationManager = applicationManager;
            this.domainRuleContext = domainRuleContext;

            domainRuleContext.RegisterRule(new SwitchRequestedLoanAmountMustBeAboveMinimumRequiredRule(this.applicationDataManager));
        }

        public ISystemMessageCollection HandleCommand(AddSwitchApplicationCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            domainRuleContext.ExecuteRules(messages, command.SwitchApplicationModel);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var uow = uowFactory.Build())
            {
                // reserve an accountkey
                int reservedAccountKey = applicationDataManager.GetReservedAccountNumber();

                int applicationNumber = applicationManager.SaveApplication(command.SwitchApplicationModel.ApplicationType,
                                                                       command.SwitchApplicationModel.ApplicationStatus,
                                                                       DateTime.Now, command.SwitchApplicationModel.ApplicationSourceKey,
                                                                       reservedAccountKey,
                                                                       SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans,
                                                                       command.SwitchApplicationModel.Reference,
                                                                       command.SwitchApplicationModel.ApplicantCount);

                applicationManager.SaveApplicationMortgageLoan(applicationNumber,
                                                                     MortgageLoanPurpose.Switchloan,
                                                                     command.SwitchApplicationModel.ApplicantCount,
                                                                     null,
                                                                     command.SwitchApplicationModel.EstimatedPropertyValue,
                                                                     null);

                int offerInformationKey = applicationManager.SaveApplicationInformation(DateTime.Now, applicationNumber, OfferInformationType.OriginalOffer, command.SwitchApplicationModel.Product);

                applicationManager.SaveSwitchApplicationInformationVariableLoan(offerInformationKey, command.SwitchApplicationModel.Term, command.SwitchApplicationModel.ExistingLoan,
                    command.SwitchApplicationModel.EstimatedPropertyValue, command.SwitchApplicationModel.CashOut, command.SwitchApplicationModel.LoanAmountNoFees);

                applicationManager.SaveApplicationInformationInterestOnly(offerInformationKey);

                // set the external originator attribute for non sahl origination sources
                if (command.SwitchApplicationModel.OriginationSource != SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans)
                {
                    SetExternalOriginatorAttributeCommand setExternalOriginatorAttributeCommand = new SetExternalOriginatorAttributeCommand(applicationNumber,
                        command.SwitchApplicationModel.OriginationSource);
                    messages = serviceCommandRouter.HandleCommand(setExternalOriginatorAttributeCommand, metadata);
                    if (messages.HasErrors)
                    {
                        uow.Complete();
                        return messages;
                    }
                }

                applicationManager.SaveApplicationInformationQuickCash(offerInformationKey);

                // we need to link the guid to the application
                linkedKeyManager.LinkKeyToGuid(applicationNumber, command.ApplicationId);

                var date = DateTime.Now;
                var switchApplicationAddedEvent = new SwitchApplicationAddedEvent(date, applicationNumber, command.SwitchApplicationModel.ApplicationType,
                    command.SwitchApplicationModel.ApplicationStatus, command.SwitchApplicationModel.ApplicationSourceKey, command.SwitchApplicationModel.OriginationSource
                    , command.SwitchApplicationModel.ExistingLoan, command.SwitchApplicationModel.EstimatedPropertyValue, command.SwitchApplicationModel.Term,
                    command.SwitchApplicationModel.CashOut, command.SwitchApplicationModel.Product);
                eventRaiser.RaiseEvent(date, switchApplicationAddedEvent, applicationNumber, (int)GenericKeyType.Offer, metadata);
                uow.Complete();
            }

            return messages;
        }
    }
}
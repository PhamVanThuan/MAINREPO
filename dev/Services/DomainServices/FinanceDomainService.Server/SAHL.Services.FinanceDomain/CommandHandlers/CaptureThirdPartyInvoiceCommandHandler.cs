using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class CaptureThirdPartyInvoiceCommandHandler : IDomainServiceCommandHandler<CaptureThirdPartyInvoiceCommand, ThirdPartyInvoiceCapturedEvent>
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private IServiceCommandRouter serviceCommandRouter;

        public CaptureThirdPartyInvoiceCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager
            , IEventRaiser eventRaiser, IServiceCommandRouter serviceCommandRouter
            , IUnitOfWorkFactory unitOfWorkFactory, IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.unitOfWorkFactory = unitOfWorkFactory;
            domainRuleManager.RegisterRule(new InvoiceCannotBeAmendedOnceApprovedRule(thirdPartyInvoiceDataManager));
            domainRuleManager.RegisterRule(new InvoiceShouldBeAmendedOnceInitiallyCapturedRule(thirdPartyInvoiceDataManager));
            domainRuleManager.RegisterRule(new InvoiceMustHaveAtLeastOneLineItemRule());
            domainRuleManager.RegisterRule(new InvoiceDateCannotBeInTheFutureRule());
            domainRuleManager.RegisterRule(new InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule(thirdPartyInvoiceDataManager));
            domainRuleManager.RegisterRule(new InvoicePaymentReferenceCannotBeEmptyRule());
        }

        public ISystemMessageCollection HandleCommand(CaptureThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            domainRuleManager.ExecuteRules(messages, command.ThirdPartyInvoiceModel);
            if (!messages.HasErrors)
            {
                using (var uow = unitOfWorkFactory.Build())
                {
                    this.thirdPartyInvoiceDataManager.AmendThirdPartyInvoiceHeader(command.ThirdPartyInvoiceModel);
                    var addThirdPartyInvoiceLineItemsCommand = new AddThirdPartyInvoiceLineItemsCommand(command.ThirdPartyInvoiceModel.LineItems);
                    messages.Aggregate(serviceCommandRouter.HandleCommand(addThirdPartyInvoiceLineItemsCommand, metadata));
                    if (!messages.HasErrors)
                    {
                        this.thirdPartyInvoiceDataManager.AmendInvoiceTotals(command.ThirdPartyInvoiceKey);
                        this.thirdPartyInvoiceDataManager.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Captured);
                        var @event = new ThirdPartyInvoiceCapturedEvent(DateTime.Now, command.ThirdPartyInvoiceModel);
                        eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceModel.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
                        uow.Complete();
                    }
                }
            }
            return messages;
        }
    }
}
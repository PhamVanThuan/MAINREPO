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
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class AmendThirdPartyInvoiceCommandHandler : IDomainServiceCommandHandler<AmendThirdPartyInvoiceCommand, ThirdPartyInvoiceAmendedEvent>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private IThirdPartyInvoiceManager thirdPartyInvoiceManager;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private IServiceCommandRouter serviceCommandRouter;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public AmendThirdPartyInvoiceCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager, IThirdPartyInvoiceManager thirdPartyInvoiceManager, IEventRaiser eventRaiser,
            IServiceCommandRouter serviceCommandRouter, IUnitOfWorkFactory unitOfWorkFactory, IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.thirdPartyInvoiceManager = thirdPartyInvoiceManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.unitOfWorkFactory = unitOfWorkFactory;
            domainRuleManager.RegisterRule(new InvoiceCannotBeAmendedOnceApprovedRule(thirdPartyInvoiceDataManager));
            domainRuleManager.RegisterRule(new InvoiceDateCannotBeInTheFutureRule());
            domainRuleManager.RegisterRule(new InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule(thirdPartyInvoiceDataManager));
            domainRuleManager.RegisterRule(new InvoicePaymentReferenceCannotBeEmptyRule());
            domainRuleManager.RegisterRule(new InvoiceCannotBeAmendedOnceApprovedForPaymentRule(thirdPartyInvoiceDataManager));
        }

        public ISystemMessageCollection HandleCommand(AmendThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var lineItemsToAdd = new List<InvoiceLineItemModel>();
            domainRuleManager.ExecuteRules(messages, command.ThirdPartyInvoiceModel);

            if (!messages.HasErrors)
            {
                using (var uow = unitOfWorkFactory.Build())
                {
                    

                    var originalThirdPartyInvoice = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(command.ThirdPartyInvoiceKey);
                    var existingInvoiceLineItems = thirdPartyInvoiceDataManager.GetInvoiceLineItems(command.ThirdPartyInvoiceKey);

                    if (thirdPartyInvoiceManager.HasThirdPartyInvoiceHeaderChanged(command.ThirdPartyInvoiceModel))
                    {
                        thirdPartyInvoiceDataManager.AmendThirdPartyInvoiceHeader(command.ThirdPartyInvoiceModel);
                    }

                    lineItemsToAdd = command.ThirdPartyInvoiceModel.LineItems.Where(x => x.InvoiceLineItemKey == null).ToList();
                    if (lineItemsToAdd.Count() > 0)
                    {
                        var addInvoiceLineItemsCommand = new AddThirdPartyInvoiceLineItemsCommand(lineItemsToAdd);
                        messages.Aggregate(serviceCommandRouter.HandleCommand(addInvoiceLineItemsCommand, metadata));
                    }


                    var lineItemsToUpdate = thirdPartyInvoiceManager.GetUpdatedInvoicedLineItems(command.ThirdPartyInvoiceModel.LineItems);
                    if (lineItemsToUpdate.Count() > 0)
                    {
                        var amendInvoiceLineItemCommand = new AmendThirdPartyInvoiceLineItemsCommand(lineItemsToUpdate);
                        messages.Aggregate(serviceCommandRouter.HandleCommand(amendInvoiceLineItemCommand, metadata));
                    }

                    var lineItemsToRemove = thirdPartyInvoiceManager.GetRemovedInvoiceLineItems(existingInvoiceLineItems, command.ThirdPartyInvoiceModel.LineItems);
                    if (lineItemsToRemove.Count() > 0)
                    {

                        var removeLineItemsCommand = new RemoveThirdPartyInvoiceLineItemsCommand(lineItemsToRemove);
                        messages.Aggregate(serviceCommandRouter.HandleCommand(removeLineItemsCommand, metadata));
                    }

                    if (!messages.HasErrors)
                    {
                        thirdPartyInvoiceDataManager.AmendInvoiceTotals(command.ThirdPartyInvoiceKey);
                        uow.Complete();
                        var @event = new ThirdPartyInvoiceAmendedEvent(DateTime.Now,originalThirdPartyInvoice, command.ThirdPartyInvoiceModel , lineItemsToAdd, lineItemsToUpdate, lineItemsToRemove);
                        eventRaiser.RaiseEvent(DateTime.Now, @event, command.ThirdPartyInvoiceModel.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
                    }
                }
            }
            return messages;
        }
    }
}
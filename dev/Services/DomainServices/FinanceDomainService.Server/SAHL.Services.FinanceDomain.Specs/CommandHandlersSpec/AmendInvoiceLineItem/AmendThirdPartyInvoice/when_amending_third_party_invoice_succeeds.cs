using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AmmendThirdPartyInvoice
{
    public class when_amending_third_party_invoice_succeeds : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceManager thirdPartyInvoiceDataFilter;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static AmendThirdPartyInvoiceCommand command;
        private static AmendThirdPartyInvoiceCommandHandler handler;
        private static ThirdPartyInvoiceModel newThirdPartyInvoice;
        private static ThirdPartyInvoiceDataModel OldThirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceDataFilter = An<IThirdPartyInvoiceManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            messages = SystemMessageCollection.Empty();
            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(121, 1212, 1, 1234.34M, true) };
            newThirdPartyInvoice = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            OldThirdPartyInvoice = new ThirdPartyInvoiceDataModel("reference", 111210, 112, Guid.NewGuid(), "FF0011", DateTime.Now, null, 100.00M, 14.00M, 114.00M,
                true, DateTime.Now, string.Empty);
            command = new AmendThirdPartyInvoiceCommand(newThirdPartyInvoice);
            handler = new AmendThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, thirdPartyInvoiceDataFilter, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);

            thirdPartyInvoiceDataFilter.WhenToldTo(x => x.HasThirdPartyInvoiceHeaderChanged(Param<ThirdPartyInvoiceModel>
                .Matches(y => y.ThirdPartyInvoiceKey == newThirdPartyInvoice.ThirdPartyInvoiceKey))).Return(true);

            thirdPartyInvoiceDataFilter.WhenToldTo(y => y.GetUpdatedInvoicedLineItems(command.ThirdPartyInvoiceModel.LineItems)).Return(
               new List<InvoiceLineItemModel>());

            thirdPartyInvoiceDataFilter.WhenToldTo(y => y.GetRemovedInvoiceLineItems(Param.IsAny<IEnumerable<InvoiceLineItemDataModel>>(), Param.IsAny<IEnumerable<InvoiceLineItemModel>>()))
               .Return(new InvoiceLineItemDataModel[] { });

            thirdPartyInvoiceDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(newThirdPartyInvoice.ThirdPartyInvoiceKey)).Return(OldThirdPartyInvoice);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_original_invoice_header = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.GetThirdPartyInvoiceByKey(newThirdPartyInvoice.ThirdPartyInvoiceKey));
        };

        private It should_check_if_third_party_invoice_is_updated = () =>
        {
            thirdPartyInvoiceDataFilter.WasToldTo(x => x.HasThirdPartyInvoiceHeaderChanged(Param<ThirdPartyInvoiceModel>
                .Matches(y => y.ThirdPartyInvoiceKey == newThirdPartyInvoice.ThirdPartyInvoiceKey)));
        };

        private It should_amend_the_invoice_header = () =>
        {

            thirdPartyInvoiceDataManager.WasToldTo(x => x.AmendThirdPartyInvoiceHeader(newThirdPartyInvoice));
        };

        private It should_raise_a_third_party_invoice_ammended_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<ThirdPartyInvoiceAmendedEvent>.Matches(
                y => y.AmmendedThirdPartyInvoice.InvoiceNumber == newThirdPartyInvoice.InvoiceNumber
                && y.OriginalThirdPartyInvoice.InvoiceNumber == OldThirdPartyInvoice.InvoiceNumber),
                newThirdPartyInvoice.ThirdPartyInvoiceKey, (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InvoiceNumberMustBeCapturedSpecs
{
    public class when_the_invoice_date_has_been_captured : WithFakes
    {
        private static InvoiceDateMustBeCapturedRule rule;
        private static ISystemMessageCollection messages;
        private static ThirdPartyInvoiceModel model;
        private static ThirdPartyInvoiceDataModel dataModel;
        private static IThirdPartyInvoiceDataManager dataManager;

        private Establish context = () =>
        {
            dataModel = new ThirdPartyInvoiceDataModel(1, "SAHL_Reference", 1, 1, Guid.NewGuid(), "Invoice_Number", DateTime.Now, "clintons@sahomeloans.com", null, null, null
                , true, DateTime.Now, string.Empty);
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(Arg.Any<int>())).Return(dataModel);
            model = new ThirdPartyInvoiceModel(123, Guid.Empty, "invoiceNumber", DateTime.Now, Enumerable.Empty<InvoiceLineItemModel>(), true, string.Empty);
            messages = SystemMessageCollection.Empty();
            rule = new InvoiceDateMustBeCapturedRule(dataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };

        private It should_use_the_third_party_invoice_provided = () =>
        {
            dataManager.Received().GetThirdPartyInvoiceByKey(Arg.Is(model.ThirdPartyInvoiceKey));
        };
    }
}
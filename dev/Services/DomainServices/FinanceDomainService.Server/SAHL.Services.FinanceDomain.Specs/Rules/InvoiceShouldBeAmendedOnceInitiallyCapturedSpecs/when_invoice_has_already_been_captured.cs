using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.ThirdPartyInvoiceShouldBeCapturedOnce
{
    public class when_invoice_has_already_been_captured : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static ISystemMessageCollection messages;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static InvoiceShouldBeAmendedOnceInitiallyCapturedRule thirdPartyInvoiceShouldBeCapturedOnceRule;
        private static int thirdPartyInvoiceKey;
        private static bool capitaliseInvoice;
        private static ISystemMessage errorMessage;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            errorMessage = new SystemMessage("Third party invoice has already been captured", SystemMessageSeverityEnum.Error);
            messages = SystemMessageCollection.Empty();
            Guid thirdPartyId = CombGuid.Instance.Generate();
            DateTime date = DateTime.Now;
            invoiceLineItems = new List<InvoiceLineItemModel>();
            thirdPartyInvoiceKey = 1212;
            capitaliseInvoice = true;
            thirdPartyInvoice = new ThirdPartyInvoiceModel(thirdPartyInvoiceKey, thirdPartyId, "DD0011", date, invoiceLineItems, capitaliseInvoice, string.Empty);
            thirdPartyInvoiceShouldBeCapturedOnceRule = new InvoiceShouldBeAmendedOnceInitiallyCapturedRule(thirdPartyInvoiceDataManager);

            var thirdPartyInvoiceDataModel = new ThirdPartyInvoiceDataModel(
                                                       thirdPartyInvoice.ThirdPartyInvoiceKey
                                                    , "SAHL-2015/04/76"
                                                   , (int)InvoiceStatus.Received
                                                   , 948398
                                                   , thirdPartyInvoice.ThirdPartyId
                                                   , thirdPartyInvoice.InvoiceNumber
                                                   , thirdPartyInvoice.InvoiceDate
                                                   , "attorney@practice.co.za"
                                                   , null
                                                   , null
                                                   , null
                                                   , true
                                                   , DateTime.Now
                                                   , string.Empty
                                                );

            thirdPartyInvoiceDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(thirdPartyInvoiceKey)).Return(thirdPartyInvoiceDataModel);
        };

        private Because of = () =>
        {
            thirdPartyInvoiceShouldBeCapturedOnceRule.ExecuteRule(messages, thirdPartyInvoice);
        };

        private It should_check_if_third_party_invoice_has_been_captured = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.GetThirdPartyInvoiceByKey(thirdPartyInvoiceKey));
        };

        private It should_return_a_third_party_invoice_has_already_been_captured_error_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The invoice has already been initially captured. Please amend the invoice.");
        };
    }
}
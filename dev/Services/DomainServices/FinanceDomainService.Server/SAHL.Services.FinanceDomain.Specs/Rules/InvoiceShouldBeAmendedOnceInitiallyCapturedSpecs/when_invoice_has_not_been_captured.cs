using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Rules.ThirdPartyInvoiceShouldBeCapturedOnce
{
    public class when_invoice_has_not_been_captured : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static ISystemMessageCollection messages;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static InvoiceShouldBeAmendedOnceInitiallyCapturedRule thirdPartyInvoiceShouldBeCapturedOnceRule;
        private static int thirdPartyInvoiceKey;
        private static bool capitaliseInvoice;
        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            messages = SystemMessageCollection.Empty();
            invoiceLineItems = NSubstitute.Substitute.For<List<InvoiceLineItemModel>>();
            thirdPartyInvoiceKey = 1212;
            capitaliseInvoice = true;
            thirdPartyInvoice = new ThirdPartyInvoiceModel(thirdPartyInvoiceKey, Guid.NewGuid(), "DD0011",
                    DateTime.Now, invoiceLineItems, capitaliseInvoice, string.Empty);
            thirdPartyInvoiceShouldBeCapturedOnceRule = new InvoiceShouldBeAmendedOnceInitiallyCapturedRule(thirdPartyInvoiceDataManager);

            var thirdPartyInvoiceDataModel = new ThirdPartyInvoiceDataModel(
                                                      thirdPartyInvoice.ThirdPartyInvoiceKey
                                                   , "SAHL-2015/04/76"
                                                  , (int)InvoiceStatus.Received
                                                  , 948398
                                                  , null
                                                  , null
                                                  , null
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

        private It should_not_return_any_error_message = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
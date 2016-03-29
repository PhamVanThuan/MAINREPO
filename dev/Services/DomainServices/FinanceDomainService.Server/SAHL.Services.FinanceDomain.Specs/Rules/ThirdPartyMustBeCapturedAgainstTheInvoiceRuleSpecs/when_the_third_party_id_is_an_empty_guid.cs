using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.ThirdPartyMustBeCapturedAgainstTheInvoiceRuleSpecs
{
    public class when_the_third_party_id_is_an_empty_guid : WithFakes
    {
        private static ThirdPartyMustBeCapturedAgainstTheInvoiceRule rule;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static ThirdPartyInvoiceDataModel dataModel;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dataModel = new ThirdPartyInvoiceDataModel(1, "SAHLRef", 1, 1408282, Guid.Empty, "invoiceNumber", DateTime.Now, "clintons@sahomeloans.com", null, null, null
                , true, DateTime.Now, string.Empty);
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(123, Guid.Empty, "invoiceNumber", DateTime.Now, Enumerable.Empty<InvoiceLineItemModel>(), true, string.Empty);
            dataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(thirdPartyInvoiceModel.ThirdPartyInvoiceKey))
                .Return(dataModel);
            rule = new ThirdPartyMustBeCapturedAgainstTheInvoiceRule(dataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, thirdPartyInvoiceModel);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The Third Party who sent the invoice has not been captured.");
        };
    }
}
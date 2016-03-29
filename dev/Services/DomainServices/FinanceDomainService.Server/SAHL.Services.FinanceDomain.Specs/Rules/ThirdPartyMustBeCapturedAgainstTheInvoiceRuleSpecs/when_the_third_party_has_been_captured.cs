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
    internal class when_the_third_party_has_been_captured : WithFakes
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
            dataModel = new ThirdPartyInvoiceDataModel(1, "SAHLRef", 1, 1408282, Guid.NewGuid(), "invoiceNumber", DateTime.Now, "clintons@sahomeloans.com", null, null, null,
                true, DateTime.Now, string.Empty);
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(123, Guid.NewGuid(), "invoiceNumber", DateTime.Now, Enumerable.Empty<InvoiceLineItemModel>(), true, string.Empty);
            dataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(thirdPartyInvoiceModel.ThirdPartyInvoiceKey))
                .Return(dataModel);
            rule = new ThirdPartyMustBeCapturedAgainstTheInvoiceRule(dataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, thirdPartyInvoiceModel);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
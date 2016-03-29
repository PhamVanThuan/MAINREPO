using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.InvoiceMustBeApprovedInOrderToBeProcessedForPaymentRuleSpecs
{
    public class when_approved : WithFakes
    {
        private static InvoiceMustBeApprovedInOrderToBeProcessedForPaymentRule rule;
        private static ISystemMessageCollection messages;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceRuleModel ruleModel;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            messages = SystemMessageCollection.Empty();
            rule = new InvoiceMustBeApprovedInOrderToBeProcessedForPaymentRule(thirdPartyInvoiceDataManager);
            ruleModel = new ThirdPartyInvoiceRuleModel(73218);

            var thirdPartyInvoiceDataModel = new ThirdPartyInvoiceDataModel(
                                                      2
                                                    , "ref"
                                                    , (int)InvoiceStatus.Approved
                                                    , 12345
                                                    , Guid.NewGuid()
                                                    , "098876"
                                                    , DateTime.Now
                                                    , "jane@doe.com"
                                                    , 10
                                                    , 1
                                                    , 12
                                                    , true
                                                    , DateTime.Now
                                                    , null
                                              );

            thirdPartyInvoiceDataManager.WhenToldTo(y => y.GetThirdPartyInvoiceByKey(Param.IsAny<int>())).Return(thirdPartyInvoiceDataModel);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
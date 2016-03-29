using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ConfirmInvoicePayment
{
    public class when_instantiating_the_handler : WithCoreFakes
    {
        private static MarkThirdPartyInvoiceAsPaidCommandHandler handler;
        private static IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
     
        private Establish context = () =>
         {
             domainRuleManager = An<IDomainRuleManager<IThirdPartyInvoiceRuleModel>>();
             thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
         };

        private Because of = () =>
         {
             handler = new MarkThirdPartyInvoiceAsPaidCommandHandler(thirdPartyInvoiceDataManager, domainRuleManager, eventRaiser);
         };

        private It should_register_the_rules = () =>
         {
             domainRuleManager.WasToldTo(x => x.RegisterPartialRule<IThirdPartyInvoiceRuleModel>(Param.IsAny<InvoicePaymentShouldBeBeingProcessedRule>()));
         };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using SAHL.Shared.BusinessModel.Calculations;


namespace SAHL.Services.FinancialDomain.Specs.CommandHandlersSpec.PriceNewBusinessApplication
{
    public class when_registering_rules : WithCoreFakes
    {
        private static PriceNewBusinessApplicationCommandHandler handler;
        private static IFinancialDataManager financialDataManager;
        private static IDomainRuleManager<IApplicationModel> domainRuleContext;
        private static IApplicationCalculator applicationCalculator;
        private static IFinancialManager financialManager;
        private static ILoanCalculations functionsUtils;

        private Establish context = () =>
            {
                financialDataManager = An<IFinancialDataManager>();
                domainRuleContext = An<IDomainRuleManager<IApplicationModel>>();
                applicationCalculator = An<IApplicationCalculator>();
                financialManager = An<IFinancialManager>();
                functionsUtils = An<ILoanCalculations>();
            };

        private Because of = () =>
            {
                handler = new PriceNewBusinessApplicationCommandHandler(financialDataManager, unitOfWorkFactory,
                    eventRaiser, applicationCalculator, domainRuleContext, financialManager, functionsUtils);
            };

        private It should_register_ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule_rule = () =>
            {
                domainRuleContext.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule>()));
            };

        private It should_register_ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule_rule = () =>
            {
                domainRuleContext.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule>()));
            };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedSalariedEmploymentToClient
{
    public class when_registering_rules_for_handler : WithCoreFakes
    {
        private static AddUnconfirmedSalariedEmploymentToClientCommandHandler handler;
        private static IEmploymentDataManager employmentDataManager;
        private static IClientDataManager clientDataManager;
        private static IDomainRuleManager<SalariedEmploymentModel> domainRuleContext;

        private Establish context = () =>
        {
            domainRuleContext = An<IDomainRuleManager<SalariedEmploymentModel>>();
            employmentDataManager = An<IEmploymentDataManager>();
            clientDataManager = An<IClientDataManager>();
        };

        private Because of = () =>
        {
            handler = new AddUnconfirmedSalariedEmploymentToClientCommandHandler(employmentDataManager, clientDataManager, domainRuleContext
                                                                               , unitOfWorkFactory, eventRaiser);
        };

        private It should_register_the_minimum_data_rule_without_any_context = () =>
        {
            domainRuleContext.WasToldTo
                (x => x.RegisterRule(Param.IsAny<EmploymentMinimumDataRequiredRule<SalariedEmploymentModel>>()));
        };

        private It should_register_the_basic_income_required_rule_without_any_context = () =>
        {
            domainRuleContext.WasToldTo
                (x => x.RegisterRule(Param.IsAny<BasicIncomeIsRequiredRule<SalariedEmploymentModel>>()));
        };

        private It should_register_the_start_date_cannot_be_prior_to_today_rule_without_any_context = () =>
        {
            domainRuleContext.WasToldTo
                (x => x.RegisterRule(Param.IsAny<EmploymentStartDateMustBeBeforeTodayRule<SalariedEmploymentModel>>()));
        };

        private It should_register_the_employee_deductions_rule = () =>
        {
            domainRuleContext.WasToldTo
                (x => x.RegisterPartialRule<IEmployeeDeductions>(Param.IsAny<EmployeeDeductionsCanOnlyContainOneOfEachTypeRule>()));
        };
    }
}
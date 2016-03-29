using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedSalaryDeductionEmploymentToClient
{
    public class when_registering_rules_for_handler : WithCoreFakes
    {
        private static AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler handler;
        private static IClientDataManager clientDataManager;
        private static IEmploymentDataManager employmentDataManager;
        private static IDomainRuleManager<SalaryDeductionEmploymentModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<SalaryDeductionEmploymentModel>>();
            employmentDataManager = An<IEmploymentDataManager>(); 
            clientDataManager = An<IClientDataManager>();
        };

        private Because of = () =>
        {
            handler = new AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler(employmentDataManager, clientDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser);
        };

        private It should_register_the_housing_allowance_mandatory_for_salary_deduction_rule_for_SAHL = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<HousingAllowanceIsRequiredForSalaryDeductionRule>(), OriginationSource.SAHomeLoans));
        };

        private It should_register_the_housing_allowance_mandatory_for_salary_deduction_rule_for_Capitec = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<HousingAllowanceIsRequiredForSalaryDeductionRule>(), OriginationSource.Capitec));
        };

        private It should_not_register_the_housing_allowance_mandatory_for_salary_deduction_rule_for_Comcorp = () =>
        {
            domainRuleManager.WasNotToldTo(x => x.RegisterRuleForContext(Param.IsAny<HousingAllowanceIsRequiredForSalaryDeductionRule>(), OriginationSource.Comcorp));
        };

        private It should_register_the_minimum_data_rule_without_any_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<EmploymentMinimumDataRequiredRule<SalaryDeductionEmploymentModel>>()));
        };

        private It should_register_the_basic_income_required_rule_without_any_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<BasicIncomeIsRequiredRule<SalaryDeductionEmploymentModel>>()));
        };

        private It should_register_the_start_date_cannot_be_prior_to_today_rule_without_any_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<EmploymentStartDateMustBeBeforeTodayRule<SalaryDeductionEmploymentModel>>()));
        };

        private It should_register_the_employee_deduction_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterPartialRule<IEmployeeDeductions>(Param.IsAny<EmployeeDeductionsCanOnlyContainOneOfEachTypeRule>()));
        };
    }
}
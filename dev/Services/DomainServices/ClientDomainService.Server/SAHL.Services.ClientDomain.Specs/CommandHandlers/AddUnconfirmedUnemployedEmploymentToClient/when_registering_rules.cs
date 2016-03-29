using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;
using NSubstitute;
using SAHL.Services.ClientDomain.Rules;
using Machine.Fakes;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedUnemployedEmploymentToClient
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddUnconfirmedUnemployedEmploymentToClientCommandHandler handler;
        private static IEmploymentDataManager employmentDataManager;
        private static IDomainRuleManager<UnemployedEmploymentModel> domainRuleManager;

        private Establish context = () =>
        {
            employmentDataManager = An<IEmploymentDataManager>();
            domainRuleManager = An<IDomainRuleManager<UnemployedEmploymentModel>>();
        };

        private Because of = () =>
        {
            handler = new AddUnconfirmedUnemployedEmploymentToClientCommandHandler(employmentDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser);
        };

        private It should_register_the_minimum_employment_data_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<EmploymentMinimumDataRequiredRule<UnemployedEmploymentModel>>());
        };

        private It should_register_the_basic_income_required_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<BasicIncomeIsRequiredRule<UnemployedEmploymentModel>>());
        };

        private It should_register_the_employment_start_date_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<EmploymentStartDateMustBeBeforeTodayRule<UnemployedEmploymentModel>>());
        };

        private It should_register_only_three_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<IDomainRule<UnemployedEmploymentModel>>())).Times(3);
        };
    }
}
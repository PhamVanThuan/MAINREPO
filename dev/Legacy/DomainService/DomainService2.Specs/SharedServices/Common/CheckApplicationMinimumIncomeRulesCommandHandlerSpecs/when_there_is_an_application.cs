using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CheckApplicationMinimumIncomeRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckApplicationMinimumIncomeRulesCommandHandler))]
    public class when_there_is_an_application : RuleSetDomainServiceSpec<CheckApplicationMinimumIncomeRulesCommand, CheckApplicationMinimumIncomeRulesCommandHandler>
    {
        Establish context = () =>
        {
            var application = An<IApplication>();
            var applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckApplicationMinimumIncomeRulesCommand(Param<int>.IsAnything, Param<bool>.IsAnything);
            handler = new CheckApplicationMinimumIncomeRulesCommandHandler(commandHandler, applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldNotBeNull();
            command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
        };
    }
}
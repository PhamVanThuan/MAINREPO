using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.CheckApplicationMinimumIncomeRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckApplicationMinimumIncomeRulesCommandHandler))]
    public class when_there_is_no_application : RuleSetDomainServiceSpec<CheckApplicationMinimumIncomeRulesCommand, CheckApplicationMinimumIncomeRulesCommandHandler>
    {
        Establish context = () =>
        {
            var ignoreWarnings = false;
            messages = An<IDomainMessageCollection>();
            IApplication application = null;

            var applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckApplicationMinimumIncomeRulesCommand(Param<int>.IsAnything, ignoreWarnings);
            handler = new CheckApplicationMinimumIncomeRulesCommandHandler(commandHandler, applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It the_rule_should_fail = () =>
        {
            command.Result.ShouldBeFalse();
        };

        It should_contain_a_null_rule_parameter = () =>
        {
            command.RuleParameters.ShouldContain(new object[] { null });
        };
    }
}
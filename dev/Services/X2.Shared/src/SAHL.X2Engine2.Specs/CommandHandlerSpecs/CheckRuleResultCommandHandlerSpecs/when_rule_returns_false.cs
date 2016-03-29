using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CheckRuleResultCommandHandlerSpecs
{
    public class when_rule_returns_false : WithFakes
    {
        private static AutoMocker<CheckRuleResultCommandHandler> autoMocker;
        static CheckRuleResultCommand command = new CheckRuleResultCommand(false, "ruleName");
        static Exception exception;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
            {
                 autoMocker = new NSubstituteAutoMocker<CheckRuleResultCommandHandler>();
            };
        Because of =()=>
        {
            exception = Catch.Exception(() => { autoMocker.ClassUnderTest.HandleCommand(command, metadata); });

        };
        It should_throw_a_rulecommandexception = () =>
            {
                exception.ShouldBe(typeof(RuleCommandException));
            };

    }
}


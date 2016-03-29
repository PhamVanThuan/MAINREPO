using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckChangeTermStageTranistion
{
    [Subject(typeof(CheckChangeTermStageTransition))]
    public class when_Change_Term_Stage_Transition_was_not_passed_an_account : RulesBaseWithFakes<CheckChangeTermStageTransition>
    {
        static object[] param = new object[] { "not an account" };
        static IStageDefinitionRepository stageDefinitionRepository;

        static Exception exception;

        Establish context = () =>
        {
            stageDefinitionRepository = An<IStageDefinitionRepository>();

            businessRule = new CheckChangeTermStageTransition(stageDefinitionRepository);
            RulesBaseWithFakes<CheckChangeTermStageTransition>.startrule.Invoke();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => businessRule.ExecuteRule(messages, param));
        };

        It should_throw_ArgumentException = () =>
        {
            exception.ShouldBeOfType<ArgumentException>();
            exception.Message.ShouldEqual("The CheckChangeTermStageTransition rule expects a Personal Loan Account.");
        };
    }
}
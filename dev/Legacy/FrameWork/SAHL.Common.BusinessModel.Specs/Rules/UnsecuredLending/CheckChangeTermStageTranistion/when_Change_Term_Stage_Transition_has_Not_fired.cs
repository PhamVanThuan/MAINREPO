using System.Collections.Generic;
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
    public class when_Change_Term_Stage_Transition_has_Not_fired : RulesBaseWithFakes<CheckChangeTermStageTransition>
    {
        static object[] param = null;
        static IStageDefinitionRepository stageDefinitionRepository;
        static int? result = null;

        Establish context = () =>
        {
            int accountKey = Param.IsAny<int>();
            param = new object[] { accountKey };

            IList<IStageTransition> transitions = new List<IStageTransition>() { };
            List<int> stageDefinition = new List<int>();

            stageDefinition.Add((int)StageDefinitionStageDefinitionGroups.PersonalLoanChangeTerm);
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            stageDefinitionRepository.WhenToldTo(x => x.GetStageTransitionList(accountKey, (int)GenericKeyTypes.ParentAccount, stageDefinition)).Return(transitions);

            businessRule = new CheckChangeTermStageTransition(stageDefinitionRepository);
            RulesBaseWithFakes<CheckChangeTermStageTransition>.startrule.Invoke();
        };

        Because of = () =>
        {
            result = businessRule.ExecuteRule(messages, param);
        };

        It should_ = () =>
        {
            result.ShouldEqual(0);
            messages.Count.ShouldEqual(1);
        };
    }
}
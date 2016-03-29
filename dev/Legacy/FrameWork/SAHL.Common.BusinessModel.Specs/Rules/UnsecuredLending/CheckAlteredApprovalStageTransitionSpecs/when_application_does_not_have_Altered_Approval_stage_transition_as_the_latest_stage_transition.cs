using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckAlteredApprovalStageTransitionSpecs
{
    [Subject(typeof(CheckAlteredApprovalStageTransition))]
    public class when_application_does_not_have_Altered_Approval_stage_transition_as_the_latest_stage_transition : RulesBaseWithFakes<CheckAlteredApprovalStageTransition>
    {
        private static IStageDefinitionRepository stageDefinitionRepository;
        private static IStageTransition nonAlteredApprovalStageTransition;

        private Establish context = () =>
        {
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            int applicationKey = Param.IsAny<int>();
            parameters = new object[] { applicationKey };

            nonAlteredApprovalStageTransition = An<IStageTransition>();
            nonAlteredApprovalStageTransition.WhenToldTo(st => st.StageDefinitionStageDefinitionGroup.Key).Return((int)StageDefinitionStageDefinitionGroups.PersonalLoanChangeTerm);
            IList<IStageTransition> transitions = new List<IStageTransition>() { nonAlteredApprovalStageTransition };

            stageDefinitionRepository.WhenToldTo(x => x.GetStageTransitionsByGenericKey(applicationKey, (int)StageDefinitionGroups.PersonalLoan)).Return(transitions);
            var results = stageDefinitionRepository.GetStageTransitionsByGenericKey(applicationKey, (int)StageDefinitionGroups.PersonalLoan);

            businessRule = new CheckAlteredApprovalStageTransition(stageDefinitionRepository);
            RulesBaseWithFakes<CheckAlteredApprovalStageTransition>.startrule.Invoke();
        };

        private Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        private It should_not_add_any_warnings = () =>
        {
            messages.HasWarningMessages.ShouldBeFalse();
        };
    }
}

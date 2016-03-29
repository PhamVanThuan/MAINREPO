using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Globals;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckAlteredApprovalStageTransitionSpecs
{
    [Subject(typeof(CheckAlteredApprovalStageTransition))]
    public class when_application_has_Altered_Approval_stage_transition_as_latest_stage_transition : RulesBaseWithFakes<CheckAlteredApprovalStageTransition>
    {
        private static IStageDefinitionRepository stageDefinitionRepository;
        private static IStageTransition alteredApprovalStageTransition;

        private Establish context = () =>
        {
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            int applicationKey = Param.IsAny<int>();
            parameters = new object[] { applicationKey };

            alteredApprovalStageTransition = An<IStageTransition>();
            alteredApprovalStageTransition.WhenToldTo(st => st.StageDefinitionStageDefinitionGroup.Key).Return((int)StageDefinitionStageDefinitionGroups.PersonalLoanAlteredApproval);
            IList<IStageTransition> transitions = new List<IStageTransition>() { alteredApprovalStageTransition };

            stageDefinitionRepository.WhenToldTo(x => x.GetStageTransitionsByGenericKey(applicationKey, (int)StageDefinitionGroups.PersonalLoan)).Return(transitions);
            var results = stageDefinitionRepository.GetStageTransitionsByGenericKey(applicationKey, (int)StageDefinitionGroups.PersonalLoan);

            businessRule = new CheckAlteredApprovalStageTransition(stageDefinitionRepository);
            RulesBaseWithFakes<CheckAlteredApprovalStageTransition>.startrule.Invoke();
        };

        private Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        private It should_add_altered_approval_warning = () =>
        {
            messages.HasWarningMessages.ShouldBeTrue();
        };
    }
}
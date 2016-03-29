using System.Collections.Generic;
using System.Linq;
using xmlElements = SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class PrePublishChecker
    {
        public const string CommonStateChecks = "CommonStateChecks";
        public const string StateMappingChecks = "StateMappingChecks";
        public const string DuplicateActivityPriorityChecks = "DuplicateActivityPriorityChecks";

        public IEnumerable<PublisherError> CheckProcess(xmlElements.Process newProcess)
        {
            List<PublisherError> errors = new List<PublisherError>();
            // perform any process checks

            // perform any workflow checks and duplicate activity priorities
            foreach (xmlElements.Workflow w in newProcess.Workflows)
            {
                this.CheckForUnappliedCommonStates(newProcess, w, errors);
                this.CheckForDuplicateActivityPriorities(newProcess, w, errors);
            }

            return errors;
        }

        private void CheckForUnappliedCommonStates(xmlElements.Process process, xmlElements.Workflow workflowToCheck, List<PublisherError> errors)
        {
            IEnumerable<xmlElements.CommonState> commonStatesWithNoAppliesTo = workflowToCheck.States.OfType<xmlElements.CommonState>().Where(x => x.AppliesTo.Count == 0);
            foreach (xmlElements.CommonState state in commonStatesWithNoAppliesTo)
            {
                errors.Add(new PublisherError(process.Name, workflowToCheck.Name, string.Format("CommonState {0} has not been applied to any states.", state.Name), CommonStateChecks));
            }
        }

        private void CheckForDuplicateActivityPriorities(xmlElements.Process process, xmlElements.Workflow workflow, List<PublisherError> errors)
        {
            var groupedActivityByState = workflow.Activities
                    .GroupBy(x => x.FromStateNode);

            var groupedByPriority = groupedActivityByState.Select(g => new { Key = g.Key, PriorityGroup = g.GroupBy(x => x.Priority) })
                    .Where(a => a.PriorityGroup.Count() > 1);

            IEnumerable<xmlElements.AbstractState> statesWithDuplicatePriorities = groupedByPriority.Select(x => x.Key);

            foreach (xmlElements.AbstractState stateWithDuplicatePriority in statesWithDuplicatePriorities)
            {
                errors.Add(new PublisherError(process.Name, workflow.Name, "State {0} has activities with duplicate priorities.", DuplicateActivityPriorityChecks));
            }
        }
    }
}
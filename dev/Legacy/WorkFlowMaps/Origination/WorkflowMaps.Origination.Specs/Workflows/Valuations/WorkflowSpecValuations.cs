using Origination;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Valuations.Specs
{
    public class WorkflowSpecValuations : WorkflowSpec<X2Valuations, IX2Valuations_Data>
    {
        public WorkflowSpecValuations()
        {
            workflow = new X2Valuations();
            workflowData = new X2Valuations_Data();
        }
    }
}
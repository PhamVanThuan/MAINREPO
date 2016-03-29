using Life;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Life.Specs
{
    public class WorkflowSpecLife : WorkflowSpec<X2LifeOrigination, IX2LifeOrigination_Data>
    {
        public WorkflowSpecLife()
        {
            workflow = new X2LifeOrigination();
            workflowData = new X2LifeOrigination_Data();
        }
    }
}
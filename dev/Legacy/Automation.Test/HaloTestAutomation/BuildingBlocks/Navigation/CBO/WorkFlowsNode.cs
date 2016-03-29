using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class WorkFlowsNode : MenuControls
    {
        public void WorkFlows(TestBrowser b)
        {
            base.tabTasks.Click();
            base.linkWorkFlows.Click();
            //close any open nodes
            b.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(b);
        }

        public void BatchReassign(TestBrowser b)
        {
            base.linkBatchReassign.Click();
        }
    }
}
using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class ApplicationMemoNode : ApplicationMemoNodeControls
    {
        public void ApplicationMemo()
        {
            base.ApplicationMemo.Click();
        }

        public void ApplicationMemoSummary(NodeTypeEnum n)
        {
            base.ApplicationMemoSummary.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Add:
                    base.AddApplicationMemo.Click();
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateApplicationMemo.Click();
                    break;
            }
        }
    }
}
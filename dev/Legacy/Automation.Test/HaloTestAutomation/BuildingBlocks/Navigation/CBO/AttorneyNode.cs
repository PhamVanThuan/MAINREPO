using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class AttorneyNode : AttorneyNodeControls
    {
        public void AttorneySummary(NodeTypeEnum n)
        {
            base.Attorney.Click();
            switch (n)
            {
                case NodeTypeEnum.Update:
                    base.UpdateAttorney.Click();
                    break;
            }
        }
    }
}
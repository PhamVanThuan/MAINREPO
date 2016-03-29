using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class PropertiesNode : PropertiesNodeControls
    {
        public void PropertySummary(NodeTypeEnum n)
        {
            base.PropertySummary.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Add:
                    base.CaptureProperty.Click();
                    break;
            }
        }

        public void Properties()
        {
            base.Properties.Click();
        }
    }
}
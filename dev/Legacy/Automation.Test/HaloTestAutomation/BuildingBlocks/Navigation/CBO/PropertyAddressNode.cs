using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class PropertyAddressNode : PropertiesNodeControls
    {
        private static IPropertyService propertyService;

        static PropertyAddressNode()
        {
            propertyService = ServiceLocator.Instance.GetService<IPropertyService>();
        }

        public void PropertyAddress(int OfferKey)
        {
            //this creates a new connection to the db
            //running the method from SQLQuerying.DataHelper, retrieving the results into a QueryResults
            var property = propertyService.GetProperty(offerkey: OfferKey);
            base.PropertyAddress(property.FormattedPropertyAddress).Click();
        }

        public void UpdatePropertyDetails(NodeTypeEnum n)
        {
            base.PropertyDetails.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdatePropertyDetails.Click();
                    break;
            }
        }

        public void UpdateInspectionContactDetails(NodeTypeEnum n)
        {
            base.PropertyDetails.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateInspectionContactDetails.Click();
                    break;
            }
        }

        public void UpdateDeedsOfficeDetails(NodeTypeEnum n)
        {
            base.PropertyDetails.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateDeedsOfficeDetails.Click();
                    break;
            }
        }

        public void UpdatePropertyAddress(NodeTypeEnum n)
        {
            base.PropertyDetails.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdatePropertyAddress.Click();
                    break;
            }
        }

        public void Valuations(NodeTypeEnum n)
        {
            switch (n)
            {
                case NodeTypeEnum.View:
                    base.Valuations.Click();
                    break;
            }
        }

        public void HomeOwnersCover(NodeTypeEnum n)
        {
            switch (n)
            {
                case NodeTypeEnum.View:
                    base.HomeOwnersCover.Click();
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateHOCDetails.Click();
                    break;
            }
        }
    }
}
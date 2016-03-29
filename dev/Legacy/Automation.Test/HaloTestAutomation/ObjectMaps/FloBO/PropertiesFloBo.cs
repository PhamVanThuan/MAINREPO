using ObjectMaps.NavigationControls;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class PropertiesNodeControls : BaseNavigation
    {
        [FindBy(Text = "Properties")]
        protected Link Properties { get; set; }

        [FindBy(Text = "Property Summary")]
        protected Link PropertySummary { get; set; }

        [FindBy(Text = "Capture Property")]
        protected Link CaptureProperty { get; set; }

        protected Link PropertyAddress(string PropertyAddress)
        {
            //PropertyAddress = PropertyAddress.Replace("/", "//");
            PropertyAddress = PropertyAddress.Replace("(", @"\(");
            PropertyAddress = PropertyAddress.Replace(")", @"\)");

            return base.Document.Link(Find.ByText(new Regex(@"^[\x20-\x7E]*" + PropertyAddress + "$")));
        }

        [FindBy(Text = "Property Details")]
        protected Link PropertyDetails { get; set; }

        [FindBy(Text = "Update Property Details")]
        protected Link UpdatePropertyDetails { get; set; }

        [FindBy(Text = "Update Inspection Contact Details")]
        protected Link UpdateInspectionContactDetails { get; set; }

        [FindBy(Text = "Update Deeds Office Details")]
        protected Link UpdateDeedsOfficeDetails { get; set; }

        [FindBy(Text = "Update Property Address")]
        protected Link UpdatePropertyAddress { get; set; }

        [FindBy(Text = "Valuations")]
        protected Link Valuations { get; set; }

        [FindBy(Text = "Home Owners Cover")]
        protected Link HomeOwnersCover { get; set; }

        [FindBy(Text = "Update HOC Details")]
        protected Link UpdateHOCDetails { get; set; }
    }
}
using ObjectMaps.NavigationControls;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public abstract class SellersNodeControls : BaseNavigation
    {
        protected Link SellerName(string LegalEntityName)
        {
            return base.Document.Link(Find.ByText(new Regex(@"^[\x20-\x7E]*" + LegalEntityName + "$")));
        }

        [FindBy(Text = "Sellers")]
        protected Link Sellers { get; set; }

        [FindBy(Text = "Seller Summary")]
        protected Link SellerSummary { get; set; }

        [FindBy(Text = "Add Seller")]
        protected Link AddSeller { get; set; }

        [FindBy(Text = "Remove Seller")]
        protected Link RemoveSeller { get; set; }

        [FindBy(Text = "Seller Details")]
        protected Link SellerDetails { get; set; }

        [FindBy(Text = "Update Seller Details")]
        protected Link UpdateSellerDetails { get; set; }
    }
}
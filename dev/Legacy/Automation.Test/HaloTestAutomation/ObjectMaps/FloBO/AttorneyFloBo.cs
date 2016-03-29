using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class AttorneyNodeControls : BaseNavigation
    {
        [FindBy(Text = "Attorney")]
        protected Link Attorney { get; set; }

        [FindBy(Text = "Update Attorney")]
        protected Link UpdateAttorney { get; set; }
    }
}
using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public abstract class ExternalLifeNodeControls : BaseNavigation
    {
        [FindBy(Title = "Life")]
        protected Link Life { get; set; }

        [FindBy(Title = "Life Policy Summary")]
        protected Link LifePolicySummary { get; set; }

        [FindBy(Title = "Update External Life Policy")]
        protected Link UpdateExternalLifePolicy { get; set; }
    }
}
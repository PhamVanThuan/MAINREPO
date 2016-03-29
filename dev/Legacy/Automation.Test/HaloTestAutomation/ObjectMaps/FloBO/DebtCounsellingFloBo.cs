using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public abstract class DebtCounsellingFloBo : BaseNavigation
    {
        [FindBy(Text = "Date Summary")]
        protected Link DateSummary { get; set; }

        [FindBy(Text = "Debt Counselling Summary")]
        protected Link DebtCounsellingSummary { get; set; }
    }
}
using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public class ExternalRolesNodeControls : BaseNavigation
    {
        [FindBy(Text = "External Roles")]
        public Link ExternalRoles;

        [FindBy(Text = "Manage Attorney")]
        public Link ManageAttorney;

        [FindBy(Text = "Manage Debt Counsellor")]
        public Link ManageDebtCounsellor;

        [FindBy(Text = "Manage PDA")]
        public Link ManagePDA;
    }
}
using ObjectMaps.FloBO;

namespace BuildingBlocks.Navigation.FLOBO.DebtCounselling
{
    /// <summary>
    /// Contains navigation blocks for the external roles FloBo node in the debt counselling workflow.
    /// </summary>
    public class ExternalRolesNode : ExternalRolesNodeControls
    {
        /// <summary>
        /// Navigates to the Manage Attorney Node
        /// </summary>
        public void ManageAttorney()
        {
            base.ExternalRoles.Click();
            base.ManageAttorney.Click();
        }

        /// <summary>
        /// Navigates to the Manage Debt Counsellor Node
        /// </summary>
        public void ManageDebtCounsellor()
        {
            base.ExternalRoles.Click();
            base.ManageDebtCounsellor.Click();
        }

        /// <summary>
        /// Navigates to the Manage PDA Node
        /// </summary>
        public void ManagePDA()
        {
            base.ExternalRoles.Click();
            base.ManagePDA.Click();
        }

        /// <summary>
        /// Navigates to the External Roles Node
        /// </summary>
        public void ExternalRoles()
        {
            base.ExternalRoles.Click();
        }
    }
}
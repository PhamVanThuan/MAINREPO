using NUnit.Framework;
using ObjectMaps;

namespace BuildingBlocks.Navigation
{
    public class MenuNode : CboControls.Menu
    {
        public void LegalEntityMenu()
        {
            base.linkLegalEntity.Click();
        }

        public void CreateCAP2Offer()
        {
            base.linkCreateCAP2Offer.Click();
        }

        public void CreateLifeLead()
        {
            base.CreateLifeLeads.Click();
        }

        /// <summary>
        /// Navigates to the 'Menu' tab
        /// </summary>
        public void Menu()
        {
            base.tabMenu.WaitUntilExists();
            if (base.tabMenu.Exists)
                base.tabMenu.Click();
            else
                Assert.That(base.tabMenu.Exists, "Menu does not exist");
        }

        /// <summary>
        /// Closes all open loan nodes on the Loan Servicing menu.  Use in the Setup or Teardown methods to cleanup all open
        /// loan nodes
        /// </summary>
        public void CloseLoanNodesCBO()
        {
            base.tabMenu.Click();

            while (base.imageCollectionTreeImage.Count > 0)
            {
                base.imageCollectionTreeImage[0].Click();
            }
        }

        public void Calculators()
        {
            base.linkCalculators.Click();
        }

        public void LossControlNode()
        {
            base.linkLossControl.Click();
        }

        public void AdministrationNode()
        {
            base.linkAdministration.Click();
        }

        public void CreateCase()
        {
            base.linkCreateCase.Click();
        }

        public void DebtCounselling()
        {
            base.linkDebtCounselling.Click();
        }

        public void DebtCounsellorMaintenance()
        {
            base.linkDebtCounsellorMaintenance.Click();
        }

        public void PDAMaintenance()
        {
            base.linkPDAMaintenance.Click();
        }

        internal void PersonalLoanMenu()
        {
            base.linkPersonalLoan.Click();
        }

        public void DisabilityClaimMenu()
        {
            base.linkDisabilityClaims.Click();
        }
    }
}
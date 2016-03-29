using ObjectMaps.FloboControls;
using WatiN.Core;

namespace BuildingBlocks.Navigation
{
    public class NavigationHelper : MenuControls
    {
        /// <summary>
        /// Closes all open loan nodes on the FloBo menu.  Use in the Setup or Teardown methods to cleanup all open
        /// loan nodes
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        public void CloseLoanNodesFLOBO(TestBrowser browser)
        {
            if (browser.Frames.Exists(Find.Any))
            {
                base.tabTasks.Click();
                while (base.imageCollectionTreeImage.Count > 0)
                {
                    base.imageCollectionTreeImage[0].Click();
                }
            }
        }

        /// <summary>
        /// Select a Worklist by UserState Name
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        /// <param name="_workflow">The Worklist, identified by UserState name</param>
        public void SelectWorklist(string _workflow)
        {
            base.linkWorklist(_workflow).Click();
        }

        //Calculators
        public void gotoApplicationWizardCalculator(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().Menu();
            browser.Navigate<MenuNode>().Calculators();
            base.linkApplicationWizard.Click();
        }

        public void gotoLeadCaptureCalculator(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().Menu();
            browser.Navigate<MenuNode>().Calculators();
            base.linkLeadCapture.Click();
        }

        public void gotoApplicationCalculator(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().Menu();
            browser.Navigate<MenuNode>().Calculators();
            base.linkApplicationCalculator.Click();
        }

        /// <summary>
        /// Navigates to the 'Menu' tab
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        public void Menu(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().Menu();
        }

        public void Menu()
        {
            base.tabMenu.Click();
        }

        /// <summary>
        /// Navigates to the 'Task' tab
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        public void Task()
        {
            base.tabTasks.Click();
        }

        public void LegalEntityMenu(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().LegalEntityMenu();
            browser.Navigate<Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        public void PersonalLoansMenu(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().PersonalLoanMenu();
            browser.Navigate<Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        public void DisabilityClaimMenu(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().DisabilityClaimMenu();
        }

        public void CreateCAP2Offer(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().CreateCAP2Offer();
        }

        public void CreateLifeLeads(TestBrowser browser)
        {
            browser.Navigate<MenuNode>().CreateLifeLead();
        }
    }
}
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.Cap;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;

namespace CAP2Tests
{
    /// <summary>
    /// A Helper class used for miscellaneous CAP 2 methods
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Loads a CAP 2 offer into the FLOBO using the workflow super search
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="workflowState">Workflow State</param>
        /// <param name="browser">IE TestBrowser</param>
        internal static void LoadCAP2Offer(int accountKey, string workflowState, TestBrowser browser)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            if (workflowState == WorkflowStates.CAP2OffersWF.CreditApproval)
            {
                browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(browser, accountKey.ToString(), new string[] { workflowState }, "CAP2", true);
            }
            else
            {
                browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(browser, accountKey.ToString(), new string[] { workflowState }, "CAP2", true);
            }
        }

        /// <summary>
        /// Creates a case but will stop at a certain point in order to check if warnings exist.
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="browser">IE TestBrowser</param>
        internal static void CreateCaseCheckForWarnings(int accountKey, TestBrowser browser)
        {
            //ensure that you can select the Create CAP 2 Offer action
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CreateCAP2Offer();
            browser.Page<CAPCreateSearch>().CreateCAP2OfferCheckWarnings(accountKey);
        }

        internal static void CreateCAPCase(TestBrowser browser, int accountKey)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CreateCAP2Offer();
            browser.Page<CAPCreateSearch>().CreateCAP2Offer(accountKey);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            //assert the offer has been created
            CAP2Assertions.AssertCAP2OfferCreated(accountKey);
            //assert the CAP status
            CAP2Assertions.AssertStatus(accountKey, CAPStatus.Open);
            //assert the X2 case is created
            CAP2Assertions.AssertX2InstanceCreated(accountKey);
            //assert the X2 state
            CAP2Assertions.AssertX2State(accountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer);
        }
    }
}
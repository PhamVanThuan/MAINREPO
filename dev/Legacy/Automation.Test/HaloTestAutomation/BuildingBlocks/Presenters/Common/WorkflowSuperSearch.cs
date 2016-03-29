using BuildingBlocks.Navigation;
using Common;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Threading;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class WorkflowSuperSearch : WorkflowSuperSearchControls
    {
        /// <summary>
        /// Performs a workflow search and loads the case into the FLOBO
        /// </summary>
        /// <param name="uniqueIdentifier">OfferKey/AccountKey</param>
        public void Search(int uniqueIdentifier)
        {
            base.txtApplicationNo.TypeText(uniqueIdentifier.ToString());
            base.btnSearch.Click();
            base.gridSelectOffer(uniqueIdentifier.ToString()).Click();
            base.gridSelectOffer(uniqueIdentifier.ToString()).DoubleClick();
        }

        /// <summary>
        /// Performs a workflow search and loads the case into the FLOBO, overloads the method in order to allow the test
        /// to specify the state of the offer that you want to load into the FLOBO.
        /// </summary>
        /// <param name="browser">IE TestBrowser Object</param>
        /// <param name="offerKey">OfferKey/AccountKey</param>
        /// <param name="uniqueIdentifier">Workflow State</param>
        public void Search(TestBrowser browser, int offerKey, string uniqueIdentifier)
        {
            browser.Navigate<NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Navigate<NavigationHelper>().Task();
            browser.Navigate<WorkFlowsNode>().WorkFlows(browser);
            base.txtApplicationNo.TypeText(offerKey.ToString());
            base.btnSearch.Click();
            base.gridSelectOffer(uniqueIdentifier).Click();
            base.gridSelectOffer(uniqueIdentifier).DoubleClick();
        }

        /// <summary>
        /// This allows a test to search for a case using the workflow filters on the workflow super search screen.
        /// </summary>
        /// <param name="workflow">Workflow Name</param>
        /// <param name="state">State in the workflow where the case is</param>
        /// <param name="allCases">Search for all cases</param>
        /// <param name="myCases">Search for my cases</param>
        public void SearchForAllCasesByWorkflowAndState(string workflow, string state, bool allCases, bool myCases)
        {
            base.imgWorkflowFilters.Click();
            Thread.Sleep(2000);
            if (allCases)
                base.ddlSearchIn.Option("All Cases").Select();
            if (myCases)
                base.ddlSearchIn.Option("My Cases").Select();
            Thread.Sleep(2000);
            base.ddlWorkflows.Option(workflow).Select();
            base.ddlStates.Option(state).WaitUntilExists();
            base.ddlStates.Option(state).Select();
            base.imgAddArrow.Click();
            //find the org structure check box and select it
            if (base.Document.Frames[0].CheckBoxes.Count > 0)
            {
                foreach (var checkbox in base.Document.Frames[0].CheckBoxes)
                {
                    if (!checkbox.Checked)
                    {
                        checkbox.Checked = true;
                    }
                }
            }
            base.btnSearch.Click();
        }

        /// <summary>
        /// Finds the first grid record of a particular type and then loads it into the CBO. When used in conjunction
        /// with the SearchByWorkflowAndState() method we can pick up an application type at a particular state.
        /// </summary>
        /// <param name="AppType">Application Type</param>
        public void AddApplicationByApplicationType(string AppType)
        {
            if (base.dxgvPagerBottomPanel.Exists)
                for (int i = 0; i < base.dxpPageNumber.Count; i++)
                {
                    bool result = base.gridOfferExists(AppType);
                    if (result)
                    {
                        base.gridSelectAppType(AppType).Click();
                        base.gridSelectAppType(AppType).DoubleClick();
                        break;
                    }
                    if (base.Next.Src != GlobalConfiguration.HaloURL + "/Themes/Orange/Web/pNextDisabled.png") base.Next.Click();
                }
            else
            {
                base.gridSelectAppType(AppType).Click();
                base.gridSelectAppType(AppType).DoubleClick();
            }
        }

        //NOTE: Removed application type parameter, as it was not being used by this method!!?
        public void WorkflowSearch(int applicationNumber)
        {
            base.txtApplicationNo.TypeText(applicationNumber.ToString());
            base.btnSearch.Click();
            base.gridSelectOffer(applicationNumber.ToString()).Click();
            base.gridSelectOffer(applicationNumber.ToString()).DoubleClick();
        }

        /// <summary>
        /// Searches for a particular case using the Application Type
        /// </summary>
        /// <param name="browser">TestBrowser</param>
        /// <param name="searchString">Search String i.e. OfferKey, AccountKey</param>
        /// <param name="uniqueIdentifiers">Normally a workflow state</param>
        /// <param name="applicationType">Application Type Dropdown value to select</param>
        public void SearchByUniqueIdentifierAndApplicationType(TestBrowser browser, string searchString, string[] uniqueIdentifiers, string applicationType,
            bool allCases = false)
        {
            browser.Navigate<Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Navigate<NavigationHelper>().Task();
            browser.Navigate<WorkFlowsNode>().WorkFlows(browser);
            if (allCases)
                base.ddlSearchIn.Option("All Cases").Select();

            base.txtApplicationNo.Value = searchString;
            base.ddlAppTypes.Option(applicationType).Select();
            base.btnSearch.Click();

            //Check that at least one out of all the uniqueIdentifiers exist and select the first available one
            bool atleastOneExist = false;
            foreach (string uniqueId in uniqueIdentifiers)
                if (base.gridOfferExists(uniqueId))
                {
                    atleastOneExist = true;
                    base.gridSelectOffer(uniqueId).Click();
                    base.gridSelectOffer(uniqueId).DoubleClick();
                    return;
                }
            Assert.That(atleastOneExist, "The expected workflow case was not returned in the worklist search results.");
        }
    }
}
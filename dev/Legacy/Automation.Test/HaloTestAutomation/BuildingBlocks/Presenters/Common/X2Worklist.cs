using BuildingBlocks.Navigation;
using Common;
using ObjectMaps.Pages;
using System;
using System.Linq;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class X2Worklist : X2WorklistControls
    {
        /// <summary>
        /// Selects the first offer from the specified Worklist
        /// </summary>
        /// <param name="TestBrowser">Watin.Core.IE object</param>
        /// <param name="workflow">The Worklist to search in, identified by UserState name</param>
        /// <param name="ExcludeOffers">List of offers to exclude</param>
        /// <returns>Returns the OfferKey of the selected offer</returns>
        public string SelectFirstOfferFromWorklist(TestBrowser TestBrowser, string workflow, params string[] ExcludeOffers)
        {
            string OfferKey = string.Empty;
            TestBrowser.Navigate<NavigationHelper>().SelectWorklist(workflow);
            if (base.dxgvPagerBottomPanel.Exists)
            {
                for (int i = 0; i < base.dxpPageNumber.Count; i++)
                {
                    for (int j = 1; j < base.SearchGridRows.Count && OfferKey == ""; j++)
                    {
                        if (!ExcludeOffers.Contains<string>(base.SearchGridRowCells(j)[0].Text.Trim()))
                        {
                            OfferKey = base.SearchGridRowCells(j)[0].Text.Trim();
                            base.SearchGridRows[j].Click();
                        }
                    }

                    if (!string.IsNullOrEmpty(OfferKey)) break;
                    else if (base.Next.Src != GlobalConfiguration.HaloURL + "/Themes/Orange/Web/pNextDisabled.png") base.Next.Click();
                }
            }
            else
            {
                for (int j = 1; j < base.SearchGridRows.Count; j++)
                {
                    if (!ExcludeOffers.Contains<string>(base.SearchGridRowCells(j)[0].Text.Trim()))
                    {
                        OfferKey = base.SearchGridRowCells(j)[0].Text.Trim();
                        base.SearchGridRows[j].Click();
                    }
                    if (!string.IsNullOrEmpty(OfferKey)) break;
                }
            }
            return OfferKey;
        }

        /// <summary>
        /// Searches the specified Worklist for an Application by OfferKey and selects the offer if found.  Includes
        /// a contingency timer for cases where the application has not made it into the new worklist because of server
        /// side processing.
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        /// <param name="workflow">The Worklist to search in, identified by UserState name</param>
        /// <param name="offerkey">OfferKey to serch for</param>
        /// <param name="timeout">The time to wait before timing out if the OfferKey cannot be found on the Worklist</param>
        public void SelectOfferFromWorklist(TestBrowser browser, string workflow, int offerkey, int timeout)
        {
            var timer = new SimpleTimer(TimeSpan.FromSeconds(timeout));
            browser.Navigate<NavigationHelper>().Task();

            //Contingency for cases where the application has not made it into the new worklist because of server side processing.
            while (!timer.Elapsed)
            {
                browser.Navigate<NavigationHelper>().SelectWorklist(workflow);
                if (base.gridSelectOffer(offerkey).Exists)
                {
                    base.gridSelectOffer(offerkey).Click();
                    base.btnSelect.Click();
                    break;
                }
            }
        }

        /// <summary>
        /// Selects the first offer from the specified Worklist
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        /// <param name="workflow">The Worklist to search in, identified by UserState name</param>
        /// <returns>Returns the OfferKey of the selected offer</returns>
        public int SelectFirstOfferFromWorklist(TestBrowser browser, string workflow)
        {
            browser.Navigate<NavigationHelper>().Task();
            browser.Navigate<NavigationHelper>().SelectWorklist(workflow);
            base.gridSelectFirstOffer().Click();
            string offerkey = base.gridSelectFirstOffer().TableCells[1].Text;
            base.btnSelect.Click();
            return int.Parse(offerkey);
        }

        /// <summary>
        /// Searches the specified Worklist for an Application by OfferKey and selects the offer if found
        /// </summary>
        /// <param name="browser">Watin.Core.IE object</param>
        /// <param name="workflow">The Worklist to search in, identified by UserState name</param>
        /// <param name="offerkey">OfferKey to serch for</param>
        public void SelectCaseFromWorklist(TestBrowser browser, string workflow, int generickey)
        {
            browser.Navigate<NavigationHelper>().Task();
            browser.Navigate<NavigationHelper>().SelectWorklist(workflow);

            bool result = false;
            //page through the worklist
            if (base.dxgvPagerBottomPanel.Exists)
            {
                while (!base.DisabledNext.Exists)
                {
                    result = base.gridOfferExists(generickey);
                    if (result)
                    {
                        base.gridSelectOffer(generickey).Click();
                        break;
                    }
                    if (base.Next.Src != GlobalConfiguration.HaloURL + "/Themes/Orange/Web/pNextDisabled.png")
                    {
                        base.Next.Click();
                    }
                }
                //check again on the last page
                result = base.gridOfferExists(generickey);
                if (result)
                {
                    base.gridSelectOffer(generickey).Click();
                }
            }
            else
            {
                base.gridSelectOffer(generickey).Click();
            }
            base.btnSelect.Click();
        }
    }
}
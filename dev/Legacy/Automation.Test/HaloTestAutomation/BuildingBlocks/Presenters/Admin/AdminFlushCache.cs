using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;
using System.Threading;

namespace BuildingBlocks.Presenters.Admin
{
    public class AdminFlushCache : AdminFlushCacheControls
    {
        private readonly IWatiNService watinService;
        private readonly ICommonService commonService;

        public AdminFlushCache()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Reload an individual lookup
        /// </summary>
        /// <param name="lookupName">Lookup to be reloaded</param>
        public void ClearLookup(string lookupName)
        {
            base.LookupDropdown.Option(lookupName).Select();
            base.btnClearLookup.Click();
        }

        /// <summary>
        /// Clear all lookups
        /// </summary>
        public void ClearAllLookups()
        {
            watinService.HandleConfirmationPopup(base.btnClearAllLookups);
        }

        /// <summary>
        /// Clear all UI Statements
        /// </summary>
        public void ClearUIStatements()
        {
            base.btnClearUIStatements.Click();
        }

        /// <summary>
        /// Clear the OrgStructure and X2 Cache
        /// </summary>
        public void ClearX2Cache()
        {
            base.btnClearX2Cache.Click();
        }

        /// <summary>
        /// Refreshes the caches via the Admin Screen
        /// </summary>
        /// <param name="browser">IE TestBrowser</param>
        /// <param name="allLookups">TRUE = Clear All Lookups</param>
        /// <param name="uiStatements">TRUE = Clear All uiStatements</param>
        /// <param name="x2Cache">TRUE = Clear X2 Cache</param>
        /// <param name="singleUser">TRUE = Clear a single user only</param>
        /// <param name="userName">ADUserName of user to clear. Set to NULL if not needed </param>
        /// <param name="singleLookup">TRUE = Clear single lookup </param>
        /// <param name="lookupName">Lookup name to clear. Set to NULL if not needed </param>
        public void RefreshCache(TestBrowser browser, bool allLookups, bool uiStatements, bool x2Cache, bool singleUser, string userName,
                                        bool singleLookup, string lookupName)
        {
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().AdministrationNode();
            browser.Navigate<AdministrationActions>().FlushCache();
            if (allLookups)
                browser.Page<AdminFlushCache>().ClearAllLookups();
            if (uiStatements)
                browser.Page<AdminFlushCache>().ClearUIStatements();
            if (x2Cache)
                browser.Page<AdminFlushCache>().ClearX2Cache();
            if (singleLookup && !string.IsNullOrEmpty(lookupName))
                browser.Page<AdminFlushCache>().ClearLookup(lookupName);
            Thread.Sleep(1500);
        }

        /// <summary>
        /// Update a rule parameter and then clear the lookup cache to reload the rule cache
        /// </summary>
        /// <param name="browser">IE TestBrowser</param>
        /// <param name="ruleName">Rule Name</param>
        /// <param name="ruleParameter">Rule SqlParameter</param>
        /// <param name="newParameterValue">New Value</param>
        public void UpdateRuleParameterAndRefreshCache(TestBrowser browser, string ruleName, string ruleParameter, string newParameterValue)
        {
            commonService.UpdateRuleParameter(ruleName, ruleParameter, newParameterValue);
            RefreshCache(browser, true, false, false, false, null, false, null);
        }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DataAccess;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class AdminFlushCache : SAHLCommonBasePresenter<IFlushCache>
    {

        private ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AdminFlushCache(IFlushCache view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.LookupButtonClicked += new KeyChangedEventHandler(_view_LookupButtonClicked);
            _view.LookupAllButtonClicked += new EventHandler(_view_LookupAllButtonClicked);
            _view.UserAccessButtonClicked += new KeyChangedEventHandler(_view_UserAccessButtonClicked);
            _view.UIStatementButtonClicked += new EventHandler(_view_UIStatementButtonClicked);
			_view.OrgStructureButtonClicked += new EventHandler(_view_OrgStructureButtonClicked);
            _view.RuleItemButtonClicked += _view_RuleItemButtonClicked;
        }

        

        #region Event Handlers

        private void _view_LookupAllButtonClicked(object sender, EventArgs e)
        {
            _lookupRepo.ResetAll();

            IX2Service x2Service = ServiceFactory.GetService<IX2Service>(); 
            x2Service.ClearLookups();

            _view.SetMessage("All lookups have been cleared from the cache", false);
        }

        private void _view_LookupButtonClicked(object sender, KeyChangedEventArgs e)
        {
            string lookupVal = e.Key.ToString();

            LookupKeys lookUp = (LookupKeys)Enum.Parse(typeof(LookupKeys), lookupVal);
            _lookupRepo.ResetLookup(lookUp);

            IX2Service x2Service = ServiceFactory.GetService<IX2Service>(); 
            x2Service.ClearLookup(lookupVal);

            _view.SetMessage(String.Format("The lookup '{0}' has been cleared from the cache.", lookupVal), false);
        }

        private void _view_UserAccessButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            string adUserName = e.Key.ToString();
            if (SAHLPrincipalCache.RemovePrincipalCache(adUserName))
                _view.SetMessage(String.Format("User '{0}' removed from cache.", adUserName), false);
            else
                _view.SetMessage(String.Format("User '{0}' does not exist in the cache.", adUserName), true);
        }

        private void _view_UIStatementButtonClicked(object sender, EventArgs e)
        {
            UIStatementRepository.ClearCache();

            IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            if (x2Service.ClearUIStatements())
                _view.SetMessage("UIStatements removed from cache.", false);
            else
                _view.SetMessage("UIStatements could not be removed from the cache - please review the X2 logs.", true);

        }

		private void _view_OrgStructureButtonClicked(object sender, EventArgs e)
		{
			IX2Service x2Service = ServiceFactory.GetService<IX2Service>();

			if (x2Service.ClearMetaCache())
				_view.SetMessage(String.Format("The X2 Cache has been cleared."), false);
			else
				_view.SetMessage(String.Format("Error reloading the org structure."), true);
		}


        public void _view_RuleItemButtonClicked(object sender, EventArgs e)
        {
            this._lookupRepo.ClearRuleCache();
            IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            x2Service.ClearRuleCache();
        }

        #endregion


    }
}

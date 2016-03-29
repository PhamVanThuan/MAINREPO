using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;

using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// Called when an existing legal entity is added. (usually via AJAX).
    /// </summary>
    public class LegalEntityDetailsSellerAddExisting : LegalEntityDetailsUpdateBase
    {
        //private int _applicationKey;
        private IApplication _application;

        public LegalEntityDetailsSellerAddExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            LoadLegalEntityFromGlobalCache();

            _application = null;
            if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
            {
                IApplication cachedApplication = GlobalCacheData[ViewConstants.CreateApplication] as IApplication;
                _application = base.ApplicationRepository.GetApplicationByKey(cachedApplication.Key);
            }

            // bind the Seller role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.Seller);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.Seller]));
            _view.BindRoleTypes(RoleTypes, String.Empty);

            BindLegalEntity();

            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);
        }

        void _view_OnReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.UpdateRoleTypeVisible = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                // Populate the marketing options ...
                PopulateMarketingOptions();

                // only validate fields applicable to sellers.
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntitySellers);

                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                if (_application != null)
                {
                    // add the seller role
                    _application.AddRole(base.SelectedRoleTypeKey, base.LegalEntity);

                    // save the application
                    base.ApplicationRepository.SaveApplication(_application);
                }

                txn.VoteCommit();

                //add the node here
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                base.OnSubmitButtonClicked(sender, e);

            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex 
                //if view is valid
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }
            }
        }
    }
}

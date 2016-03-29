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
    /// </summary>
    public class LegalEntityDetailsAddRelateExisting : LegalEntityDetailsUpdateBase
    {
        //private int _applicationKey;
        //private IApplication _application;

        public LegalEntityDetailsAddRelateExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            LoadLegalEntityFromGlobalCache();

            _view.IncomeContributorVisible = false;
            _view.SelectedIncomeContributor = false; 

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

            _view.UpdateRoleTypeVisible = false;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                
                // Populate the marketing options ...
                PopulateMarketingOptions();

                // If legalentity has no 'non-lead' roles, only validate fields applicable to lead applicants/suretors.
                if (base.LegalEntityRepository.HasNonLeadRoles(base.LegalEntity) == false)
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                // Save the legalentity
                base.LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                if (!_view.IsValid)
                    throw new Exception();

                ts.VoteCommit();

                // Add the saved LE to the Global Cache
                base.PersistGlobalCacheVariables();

                Navigator.Navigate("Submit");
            }
            catch (Exception)
            {
                ts.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex 
                //if view is valid
                try
                {
                    ts.Dispose();
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

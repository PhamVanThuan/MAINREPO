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
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.CacheData;


namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsSellerAdd : LegalEntityDetailsAddBase
    {
        private IApplication _application;
        private IApplicationRepository _applicationRepository;
        private int _applicationKey;
        private CBOMenuNode cboMenuNode;

        public LegalEntityDetailsSellerAdd(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) 
                return;

            cboMenuNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = cboMenuNode.GenericKey;

            // Get the Application from the repository
            _application = _applicationRepository.GetApplicationByKey(_applicationKey);

            // bind the Seller role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.Seller);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.Seller]));

            _view.BindRoleTypes(RoleTypes, String.Empty);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage) 
                return;

            _view.AddRoleTypeVisible = true;

            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Save";
        }

        protected override void ReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.ReBindLegalEntity(sender, e);

            if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
                GlobalCacheData.Remove(ViewConstants.CreateApplication);

            GlobalCacheData.Add(ViewConstants.CreateApplication, _application, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {
            // Create a blank LE populate it and save it
            base.LegalEntity = LegalEntityRepository.GetEmptyLegalEntity((LegalEntityTypes)View.SelectedLegalEntityType);

            base.LegalEntity.IntroductionDate = DateTime.Now;

            // Get the details from the screen
            _view.PopulateLegalEntityDetailsForAdd(base.LegalEntity);

            // Populate the marketing options ...
            PopulateMarketingOptions();

            TransactionScope txn = new TransactionScope();

            try
            {
                // only validate fields applicable to sellers.
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntitySellers);

                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                if (_application != null)
                {
                    // add the seller role
                    _application.AddRole(base.SelectedRoleTypeKey, base.LegalEntity);

                    // save the application
                    _applicationRepository.SaveApplication(_application);
                }

                txn.VoteCommit();

                //add the node here
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                Navigator.Navigate("Submit");
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

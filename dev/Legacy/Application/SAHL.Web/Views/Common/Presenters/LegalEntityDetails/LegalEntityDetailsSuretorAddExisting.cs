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
    public class LegalEntityDetailsSuretorAddExisting : LegalEntityDetailsUpdateBase
    {
        private IAccount _account; 
        private IAccountRepository _accountRepository;


        public LegalEntityDetailsSuretorAddExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            LoadLegalEntityFromGlobalCache();

            _account = null;
            int accountKey = -1;
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
            {
                accountKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedAccountKey]);
            }
            else
            {
                CBOMenuNode cboMenuNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                accountKey = cboMenuNode.GenericKey;
            }
            
            // Get the Account from the repository
            _account = _accountRepository.GetAccountByKey(accountKey);

            // bind the Suretor role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.RoleTypes.Suretor);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.RoleTypes.BindableDictionary[roleTypeKey]));
            _view.BindRoleTypes(RoleTypes, String.Empty);

            BindLegalEntity();

            _view.OnReBindLegalEntity += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindLegalEntity);
        }

        void _view_OnReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            GlobalCacheData.Remove(ViewConstants.SelectedAccountKey);
            GlobalCacheData.Add(ViewConstants.SelectedAccountKey, _account.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.UpdateRoleTypeVisible = true;

            _view.SubmitButtonText = "Add Suretor"; 
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

                // add the suretor role
                // Get a new role row, populate it and save it.
                IRole role = LegalEntityRepository.GetEmptyRole();
                role.Account = _account;
                role.LegalEntity = base.LegalEntity;
                role.RoleType = LookupRepository.RoleTypes.ObjectDictionary[Convert.ToString((int)RoleTypes.Suretor)];
                role.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                role.StatusChangeDate = System.DateTime.Now;            

                base.LegalEntity.Roles.Add(_view.Messages, role);

                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, true);

                // The base will attempt to navigate, so save first
                base.OnSubmitButtonClicked(sender, e);

                txn.VoteCommit();
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

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
    public class LegalEntityDetailsSuretorAdd : LegalEntityDetailsAddBase
    {
        private IAccount _account;
        private IAccountRepository _accountRepository;
        private int _genericKey;
        private CBOMenuNode cboMenuNode;

        public LegalEntityDetailsSuretorAdd(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            cboMenuNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _genericKey = cboMenuNode.GenericKey;

            // Get the Account from the repository
            _account = _accountRepository.GetAccountByKey(_genericKey);

            // bind the Suretor role type only
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.RoleTypes.Suretor);
            RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.RoleTypes.BindableDictionary[roleTypeKey]));

            _view.BindRoleTypes(RoleTypes, String.Empty);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.AddRoleTypeVisible = true;

            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Add Suretor"; // "Save";
        }

        protected override void ReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.ReBindLegalEntity(sender, e);

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                GlobalCacheData.Remove(ViewConstants.SelectedAccountKey);

            GlobalCacheData.Add(ViewConstants.SelectedAccountKey, _account.Key, new List<ICacheObjectLifeTime>());

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
                // add the suretor role
                // Get a new role row, populate it and save it.
                IRole role = LegalEntityRepository.GetEmptyRole();
                role.Account = _account;
                role.LegalEntity = base.LegalEntity;
                role.RoleType = LookupRepository.RoleTypes.ObjectDictionary[Convert.ToString((int)RoleTypes.Suretor)];
                role.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                role.StatusChangeDate = System.DateTime.Now;

                base.LegalEntity.Roles.Add(_view.Messages, role);

                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                Navigator.Navigate("Submit");

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

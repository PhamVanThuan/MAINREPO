using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using Castle.ActiveRecord;


namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityEnableUpdateSuretorAdd : LegalEntityEnableUpdateBase
    {
        private IRole _role;
        private IAccount _account;

        public LegalEntityEnableUpdateSuretorAdd(ILegalEntityEnableUpdate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // Get the selected legal legal entity from the Global Cache.
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                LegalEntity = GlobalCacheData[ViewConstants.LegalEntity] as ILegalEntity;
            else
                throw new Exception(ResourceService.GetString(ResourceConstants.GlobalCacheNullElementException));

            // Get the parent account for the selected node.
            CBOMenuNode cboCurrentMenuNodeAny = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            CBOMenuNode cboCurrentMenuNodeAccount = cboCurrentMenuNodeAny.GetParentNodeByType(GenericKeyTypes.Account) as CBOMenuNode;

            if (_account == null && cboCurrentMenuNodeAccount != null)
                _account = AccountRepository.GetAccountByKey((int)cboCurrentMenuNodeAccount.GenericKey);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 


            _view.BindLabelQuestion(String.Format( ResourceService.GetString(ResourceConstants.LegalEntityEnableSuretorAddMessage), LegalEntity.GetLegalName(LegalNameFormat.Full)));
            _view.BindLabelMessage(String.Empty);

            _view.BindCancelButtonText("No");
            _view.BindSubmitButtonText("Yes");
        }

        protected override void OnSubmitButtonClick(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                // if the legalentity already exists as a suretor, update the status to active
                int suretorRoleKey = -1;
                foreach (IRole role in _account.Roles)
                {
                    if (role.LegalEntity.Key == LegalEntity.Key && role.RoleType.Key == (int)RoleTypes.Suretor)
                    {
                        suretorRoleKey = role.Key;
                        break;
                    }
                }
                if (suretorRoleKey > 0)
                {
                    // get the existing role record
                    _role = base.AccountRepository.GetRoleByKey(suretorRoleKey);
                    // activate and save
                    base.LegalEntityRepository.ActivateRole(_role);
                }
                else
                {
                    // Get a new role and populate
                    _role = LegalEntityRepository.GetEmptyRole();
                    _role.Account = _account;
                    _role.LegalEntity = LegalEntity;
                    _role.RoleType = LookupRepository.RoleTypes.ObjectDictionary[Convert.ToString((int)RoleTypes.Suretor)];
                    _role.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                    _role.StatusChangeDate = System.DateTime.Now;
                    // save new suretor role record
                    base.LegalEntityRepository.SaveRole(_role);
                }

                ts.VoteCommit();

                // navigate 
                base.OnSubmitButtonClick(sender, e);
            }
            catch (Exception)
            {
                ts.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }
        }
    }
}

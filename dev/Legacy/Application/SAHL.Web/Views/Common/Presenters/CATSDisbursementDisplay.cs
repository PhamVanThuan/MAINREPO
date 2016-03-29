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
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CATSDisbursementDisplay : SAHLCommonBasePresenter<ICATSDisbursement>
    {
        private IDisbursementRepository _disbursementRepository;
        private IList<IDisbursement> _unpostedDisbursements;
        private IAccount _account;
        private List<IBankAccount> _bankAccountList;
        bool _showControls;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CATSDisbursementDisplay(ICATSDisbursement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _disbursementRepository = RepositoryFactory.GetRepository<IDisbursementRepository>();
            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            switch (cboNode.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    _account = accRepository.GetAccountByKey(cboNode.GenericKey);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    _account = accRepository.GetAccountByApplicationKey(cboNode.GenericKey);
                    break;
                case (int)GenericKeyTypes.ParentAccount:
                    {
                        _account = accRepository.GetAccountByKey(cboNode.GenericKey);
                        break;
                    }
                default:
                    break;
            }

            _showControls = false;
            if (_account != null)
            {
                _showControls = true;
                _view.DisbursementGridPostBackType = GridPostBackType.None;
                BindBankAccounts();
                BindControlData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.DisplayControlsVisible = _showControls;
            _view.DisbursementGridVisible = true;
            _view.AddControlsVisible = false;
            _view.RollbackControlsVisible = false;
            _view.CancelButtonVisible = false;
            _view.SaveButtonVisible = false;
            _view.SubmitButtonVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindControlData()
        {
            IReadOnlyEventList<IDisbursement> savedDisbursements = _disbursementRepository.GetDisbursmentsByParentAccountKeyAndStatus(_account.Key, Convert.ToInt32(DisbursementStatuses.Pending));
            _unpostedDisbursements = new List<IDisbursement>();
            for (int savedIndex = 0; savedIndex < savedDisbursements.Count; savedIndex++)
            {
                _unpostedDisbursements.Add(savedDisbursements[savedIndex]);
            }
            if (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0)
            {
                _view.BindGridDisbursements(_unpostedDisbursements);
                _view.DisbursementTypeLabelText = _unpostedDisbursements[0].DisbursementTransactionType.Description;

                double totalDisbursements = 0;
                for (int i = 0; i < _unpostedDisbursements.Count; i++)
                {
                    if (_unpostedDisbursements[i].Amount.HasValue)
                        totalDisbursements = totalDisbursements + _unpostedDisbursements[i].Amount.Value;
                }

                _view.TotalAmountLabelText = totalDisbursements.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindBankAccounts()
        {
            _bankAccountList = new List<IBankAccount>();
            for (int i = 0; i < _account.Roles.Count; i++)
            {
                if (_account.Roles[i].GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active) &&
                    (_account.Roles[i].RoleType.Key == Convert.ToInt32(RoleTypes.MainApplicant) || _account.Roles[i].RoleType.Key == Convert.ToInt32(RoleTypes.Suretor)))
                {
                    ILegalEntity legalEntity = _account.Roles[i].LegalEntity;
                    for (int bankAccountIndex = 0; bankAccountIndex < legalEntity.LegalEntityBankAccounts.Count; bankAccountIndex++)
                    {
                        ILegalEntityBankAccount leBankAccount = legalEntity.LegalEntityBankAccounts[bankAccountIndex];
                        if (leBankAccount.GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active))
                        {
                            _bankAccountList.Add(leBankAccount.BankAccount);
                        }
                    }
                }
            }

            _view.SetBankAccounts(_bankAccountList);
            _view.BindBankAccounts();
        }
    }
}

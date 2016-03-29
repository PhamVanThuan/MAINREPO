using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CATSDisbursementDelete : SAHLCommonBasePresenter<ICATSDisbursement>
    {
        private IDisbursementRepository _disbursementRepository;
        private IList<IDisbursement> _unpostedDisbursements;
        private IAccount _account;
        private List<IBankAccount> _bankAccountList;
        bool _allowDelete;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CATSDisbursementDelete(ICATSDisbursement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

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

            _view.DisplayControlsVisible = _allowDelete;
            _view.DisbursementGridVisible = true;
            _view.AddControlsVisible = false;
            _view.RollbackControlsVisible = false;
            _view.CancelButtonVisible = true;
            _view.SaveButtonVisible = false;
            _view.SubmitButtonVisible = _allowDelete;

            _view.SubmitButtonText = "Delete";

            if (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0)
                _view.SubmitButtonEnabled = true;
            else
                _view.SubmitButtonEnabled = false;

            //setup confirmation messages
            _view.DisplayDeleteConfirmationMessage = true;
            _view.DeleteConfirmationMessage = "Are you sure you would like to delete all the CATS Disbursement transactions?";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _disbursementRepository = RepositoryFactory.GetRepository<IDisbursementRepository>();

            _view.OnCancelButtonClicked += (_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            if (cboNode != null)
                switch (cboNode.GenericKeyTypeKey)
                {
                    case (int)GenericKeyTypes.Account:
                        _account = accRepository.GetAccountByKey(cboNode.GenericKey);
                        break;
                    case (int)GenericKeyTypes.Offer:
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

            _allowDelete = false;
            if (_account != null)
            {
                _allowDelete = true;
                _view.DisbursementGridPostBackType = GridPostBackType.None;
                BindControlData();
            }
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
                BindBankAccounts();
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

                if (_view.DisbursementTypeSelectedValue == Convert.ToInt32(5 /*DisbursementTransactionTypes.Readvance*/) ||
                    (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0 && _unpostedDisbursements[0].DisbursementTransactionType.Key == 5))
                {
                    IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                    IControl control = ctrlRepo.GetControlByDescription("SAHLValuationBankAccount");
                    if (_account.OriginationSource.Key == (int)OriginationSources.RCS)
                        control = ctrlRepo.GetControlByDescription("RCSValuationBankAccount");

                    if (control != null && control.ControlNumeric.HasValue)
                    {
                        IBankAccountRepository bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
                        IBankAccount bankAcc = bankRepo.GetBankAccountByKey(Convert.ToInt32(control.ControlNumeric.Value));
                        _bankAccountList.Add(bankAcc);
                    }
                }
            }

            _view.SetBankAccounts(_bankAccountList);
            _view.BindBankAccounts();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {

                TransactionScope txn = new TransactionScope();

                try
                {
                    for (int i = 0; i < _unpostedDisbursements.Count; i++)
                    {
                        _disbursementRepository.DeleteDisbursement(_unpostedDisbursements[i]);
                    }
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
                    txn.Dispose();
                }

                if (_view.IsValid)
                    _view.Navigator.Navigate("Cancel");
        }
    }
}

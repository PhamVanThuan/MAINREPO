using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CATSDisbursementRollback : SAHLCommonBasePresenter<ICATSDisbursement>
    {
        private IAccount _account;
        private List<IBankAccount> _bankAccountList;
        private DataTable _loanTransactions;
        IDisbursementRepository _disbursementRepo;
        ILoanTransactionRepository _loanTransactionRepo;

        private DataTable _disbursementTrans;
        private bool _allowRollback;
        DataTable disbursementTrans;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CATSDisbursementRollback(ICATSDisbursement view, SAHLCommonBaseController controller)
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

            _view.DisplayControlsVisible = false;
            _view.DisbursementGridVisible = false;
            _view.AddControlsVisible = false;
            _view.RollbackControlsVisible = true; // _allowRollback;
            _view.CancelButtonVisible = true;
            _view.SaveButtonVisible = false;
            _view.SubmitButtonVisible = _allowRollback;

            _view.SubmitButtonText = "Rollback";

            //setup confirmation messages
            _view.DisplaySubmitConfirmationMessage = true;
            _view.SubmitConfirmationMessage = "Are you sure you would like to rollback all the CATS Disbursement transactions and the associated financial transaction?";
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

            _view.OnCancelButtonClicked += (_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
            _view.OnLoanTransactionGridSelectedIndexChanged += (_view_LoanTransactionGridSelectedIndexChanged);
            _view.SubmitButtonEnabled = false;

            _disbursementRepo = RepositoryFactory.GetRepository<IDisbursementRepository>();
            _loanTransactionRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
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

            _allowRollback = false;
            if (_account != null)
            {
                _loanTransactions = _disbursementRepo.GetDisbursementLoanTransactions(_account.Key, _view.CurrentPrincipal);
                if (_loanTransactions != null && _loanTransactions.Rows.Count > 0)
                {
                    _allowRollback = true;
                    _view.BindLoanTransactions(_loanTransactions);
                    BindBankAccounts();

                    if (!_view.IsPostBack)
                    {
                        disbursementTrans = _disbursementRepo.GetDisbursementRollbackTransactions(_account.Key, new int[] { Convert.ToInt32(_loanTransactions.Rows[0]["LoanTransactionNumber"]) });
                        _view.BindDisbursementTransactions(disbursementTrans);

                        if (Convert.ToInt32(_loanTransactions.Rows[0]["CanRollBack"]) == 1)
                            _view.SubmitButtonEnabled = true;
                    }
                }
            }
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
            int selectedIndex = _view.LoanTransactionSelectedIndex;

            if (selectedIndex >= 0 && _loanTransactions.Rows.Count > selectedIndex)
            {
                int loanTransactionNumber = Convert.ToInt32(_loanTransactions.Rows[selectedIndex]["LoanTransactionNumber"]);

                IReadOnlyEventList<IDisbursement> list = _disbursementRepo.GetDisbursementByLoanTransactionNumber(loanTransactionNumber);
                //execute disbursement rollback rule
                IRuleService rs = ServiceFactory.GetService<IRuleService>();
                rs.ExecuteRule(_view.Messages, "CATSDisbursementRollback", list);
                if (!_view.IsValid)
                    return;

                TransactionScope txns = new TransactionScope();
                try
                {
                    // Mark the Disbursment as Rolled Back
                    //IDisbursement disbursement = _disbursementRepo.GetDisbursementByKey(Convert.ToInt32(1));
                    

                    ILookupRepository _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                    IDisbursementStatus rolledBack = _lookupRepository.DisbursementStatuses.ObjectDictionary[Convert.ToInt32(DisbursementStatuses.RolledBack).ToString()];
                    double totalDisbursement = 0;

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].DisbursementStatus = rolledBack;
                        totalDisbursement += list[i].Amount.HasValue ? list[i].Amount.Value : 0;
                    }

                    ExclusionSets.Add(RuleExclusionSets.RollbackDisbursements);

                    _disbursementRepo.SaveDisbursement(list, totalDisbursement);

                    ExclusionSets.Remove(RuleExclusionSets.RollbackDisbursements);

                    txns.VoteCommit();

                }
                catch (Exception)
                {
                    txns.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txns.Dispose();
                }

                if (_view.IsValid)
                {
                    //if (loanTransactionNumber == 140 || loanTransactionNumber == 141)
                    //{
                    txns = new TransactionScope();

                    try
                    {
                        _loanTransactionRepo.pLoanProcessRollbackTran(loanTransactionNumber, _view.CurrentPrincipal);

                        IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();

                        mlRepo.InstallmentChange(_account.Key, _view.CurrentPrincipal.Identity.Name);

                        txns.VoteCommit();
                    }
                    catch (Exception)
                    {
                        txns.VoteRollBack();
                        if (_view.IsValid)
                            throw;
                    }
                    finally
                    {
                        txns.Dispose();
                    }

                    _view.Navigator.Navigate("Cancel");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_LoanTransactionGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                int selectedIndex = Convert.ToInt32(e.Key);
                if (selectedIndex != -1)
                {
                    IDisbursementRepository disbursementRepo = RepositoryFactory.GetRepository<IDisbursementRepository>();
                    _disbursementTrans = disbursementRepo.GetDisbursementRollbackTransactions(_account.Key, new int[] { Convert.ToInt32(_loanTransactions.Rows[selectedIndex]["LoanTransactionNumber"]) });

                    _view.BindDisbursementTransactions(_disbursementTrans);

                    if (Convert.ToInt32(_loanTransactions.Rows[selectedIndex]["CanRollBack"]) == 1)
                        _view.SubmitButtonEnabled = true;
                }
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

            _view.SetBankAccounts(_bankAccountList);
            _view.BindBankAccounts();
        }
    }
}

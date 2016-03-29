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
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;

using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters
{

    /// <summary>
    /// 
    /// </summary>
    public class CATSDisbursementAddNoPost : SAHLCommonBasePresenter<ICATSDisbursement>
    {
        private IDisbursementRepository _disbursementRepository;
        private IList<IDisbursement> _unpostedDisbursements;
        private IAccount _account;
        private IReadOnlyEventList<IDisbursementTransactionType> _disbursementTypeList;
        private List<IBankAccount> _bankAccountList;
        private IBankAccountRepository _bankRepo;

        private string _cachedDataUnpostedDisbursementKey = "UnPostedDisbursements";
        private string _cachedDataDisbursementSavedKey = "NeedToSaveDisbursements";

        bool saveDisbursements;
        bool _capReadvance;
        double _capReadvanceAmount;
        bool _allowUpdate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CATSDisbursementAddNoPost(ICATSDisbursement view, SAHLCommonBaseController controller)
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
            _view.DisbursementGridVisible = true;
            _view.RollbackControlsVisible = false;
            _view.CancelButtonVisible = true;

            _view.SubmitButtonVisible = false;

            _view.DisbursementTypeLableVisible = false;

            _view.AddControlsVisible = _allowUpdate;
            _view.SaveButtonVisible = _allowUpdate;
            _view.SubmitButtonVisible = _allowUpdate;

            if (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0)
            {
                _view.DisbursementTypeEnabled = false;
                _view.DeleteButtonEnabled = true;
                _view.DisbursementTypeSelectedValue = _unpostedDisbursements[0].DisbursementTransactionType.Key;
            }
            else
            {
                _view.DeleteButtonEnabled = false;
                _view.SaveButtonVisible = false;
            }

            if (PrivateCacheData.ContainsKey(_cachedDataDisbursementSavedKey))
            {
                saveDisbursements = !Convert.ToBoolean(PrivateCacheData[_cachedDataDisbursementSavedKey]);
                _view.SaveButtonEnabled = saveDisbursements;
            }

            if (!_view.IsPostBack && (_unpostedDisbursements == null || _unpostedDisbursements.Count == 0))
            {
                if (_view.DisbursementTypeSelectedValue < 0)
                {
                    _view.SaveButtonVisible = false;
                    _view.DeleteButtonEnabled = false;
                    _view.SubmitButtonVisible = false;
                    _view.AddButtonEnabled = false;
                }
            }
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

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnAddDisbursementClicked += new EventHandler(_view_OnAddDisbursementClicked);
            _view.OnDeleteDisbursementClicked += new KeyChangedEventHandler(_view_OnDeleteDisbursementClicked);
            _view.OnSaveButtonClicked += new EventHandler(_view_OnSaveButtonClicked);
            _view.OnLoanTransactionGridSelectedIndexChanged += new KeyChangedEventHandler(_view_LoanTransactionGridSelectedIndexChanged);
            _view.OnDisbursementTypeSelectedIndexChanged += new EventHandler(_view_OnDisbursementTypeSelectedIndexChanged);

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            switch (cboNode.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    _account = accRepository.GetAccountByKey(cboNode.GenericKey);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    _account = accRepository.GetAccountByApplicationKey(cboNode.GenericKey);
                    break;
                default:
                    break;
            }

            _allowUpdate = false;

            if (_account != null)
            {
                _allowUpdate = true;
                BindDisbursementTypes();
                BindBankAccounts();
                _view.DisbursementGridPostBackType = GridPostBackType.NoneWithClientSelect;
                BindGridAndSetControlVisiblility();
            }

            if (PrivateCacheData.ContainsKey("SelectedDisbursementTypeKey"))
                _view.DisbursementTypeSelectedValue = Convert.ToInt32(PrivateCacheData["SelectedDisbursementTypeKey"]);

        }

        /// <summary>
        /// 
        /// </summary>
        private void BindGridAndSetControlVisiblility()
        {
            if (!_view.IsPostBack)
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
                    PrivateCacheData[_cachedDataUnpostedDisbursementKey] = _unpostedDisbursements;
                    PrivateCacheData[_cachedDataDisbursementSavedKey] = true;

                    double totalDisbursements = 0;
                    for (int i = 0; i < _unpostedDisbursements.Count; i++)
                    {
                        if (_unpostedDisbursements[i].Amount.HasValue)
                            totalDisbursements = totalDisbursements + _unpostedDisbursements[i].Amount.Value;
                    }
                    _view.DisbursementTotalAmount = totalDisbursements;
                }
                else
                    PrivateCacheData[_cachedDataDisbursementSavedKey] = false;
            }
            else
            {
                if (PrivateCacheData.ContainsKey(_cachedDataUnpostedDisbursementKey))
                    _unpostedDisbursements = (IList<IDisbursement>)PrivateCacheData[_cachedDataUnpostedDisbursementKey];
                else
                    _unpostedDisbursements = new List<IDisbursement>();

                _capReadvance = false;
                if (_view.DisbursementTypeSelectedValue != -1)
                {
                    for (int i = 0; i < _disbursementTypeList.Count; i++)
                    {
                        if (_view.DisbursementTypeSelectedValue == _disbursementTypeList[i].Key)
                        {
                            if (_disbursementTypeList[i].TransactionTypeNumber == 141)
                            {
                                _capReadvance = true;
                            }
                        }
                    }
                }

                BindBankAccounts();
                _view.BindGridDisbursements(_unpostedDisbursements);

                if (_capReadvance == true)
                {
                    _capReadvanceAmount = 0;
                    ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
                    IList<ICapApplication> capOffer = capRepo.GetCapOfferByAccountKeyAndStatus(_account.Key, Convert.ToInt32(CapStatuses.ReadvanceRequired));
                    if (capOffer != null && capOffer.Count > 0)
                    {
                        for (int i = 0; i < capOffer[0].CapApplicationDetails.Count; i++)
                        {
                            if (capOffer[0].CapApplicationDetails[i].CapStatus.Key == Convert.ToInt32(CapStatuses.ReadvanceRequired))
                            {
                                _capReadvanceAmount = capOffer[0].CapApplicationDetails[i].Fee;
                            }
                        }
                    }
                    else
                    {
                        if (_view.Messages.ErrorMessages.Count < 1)
                            _view.Messages.Add(new Error("No Cap Offers Awaiting Readvance Found !.", "No Cap Offers Awaiting Readvance Found !."));
                        _view.AddButtonEnabled = false;

                    }
                    _view.DisbursementTotalAmount = _capReadvanceAmount;
                    _view.DisbursementAmount = _capReadvanceAmount;
                    _view.AddControlsEnabled = false;
                }
                else
                    _view.AddControlsEnabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDisbursementTypes()
        {
            _disbursementTypeList = _disbursementRepository.GetDisbursementTransactionTypes(_view.CurrentPrincipal);
            _view.BindDisbursementTypes(_disbursementTypeList);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindBankAccounts()
        {
            _bankAccountList = new List<IBankAccount>();
            IBankAccount bankAcc = null;

            int disbursementType;

            if (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0)
                disbursementType = _unpostedDisbursements[0].DisbursementTransactionType.Key;
            else
                disbursementType = _view.DisbursementTypeSelectedValue;

            if (disbursementType != (int)DisbursementTransactionTypes.CAP2ReAdvance)
            {
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
                                if (!BankAccountExists(_bankAccountList, leBankAccount.BankAccount))
                                    _bankAccountList.Add(leBankAccount.BankAccount);
                            }
                        }
                    }
                    if (disbursementType > 0)
                    {
                        if ((disbursementType == (int)DisbursementTransactionTypes.ReAdvance) ||
                            (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0 && _unpostedDisbursements[0].DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.ReAdvance))
                        {
                            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                            IControl control = controlRepo.GetControlByDescription("SAHLValuationBankAccount");
                            if (_account.OriginationSource.Key == (int)OriginationSources.RCS)
                                control = controlRepo.GetControlByDescription("RCSValuationBankAccount");

                            if (control != null && control.ControlNumeric.HasValue)
                            {
                                IBankAccountRepository bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
                                bankAcc = bankRepo.GetBankAccountByKey(Convert.ToInt32(control.ControlNumeric.Value));
                                if (!BankAccountExists(_bankAccountList, bankAcc))
                                    _bankAccountList.Add(bankAcc);
                            }
                        }
                    }
                }
            }
            else
            {
                _capReadvance = true;
                IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl control = controlRepo.GetControlByDescription("CAP2BankAccountKey");

                if (control != null && control.ControlNumeric.HasValue)
                {
                    IBankAccountRepository bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    bankAcc = bankRepo.GetBankAccountByKey(Convert.ToInt32(control.ControlNumeric.Value));
                    if (!BankAccountExists(_bankAccountList, bankAcc))
                    {
                        _bankAccountList.Add(bankAcc);
                    }
                }
            }


            _view.SetBankAccounts(_bankAccountList);
            _view.BindBankAccounts();
            if (_capReadvance && bankAcc != null)
                _view.SelectedBankAccount = bankAcc.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccountList"></param>
        /// <param name="compareAccount"></param>
        /// <returns></returns>
        private static bool BankAccountExists(List<IBankAccount> bankAccountList, IBankAccount compareAccount)
        {
            for (int i = 0; i < bankAccountList.Count; i++)
            {
                if (bankAccountList[i].Key == compareAccount.Key)
                    return true;
            }
            return false;
        }


        #region Event Handlers

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
        void _view_OnAddDisbursementClicked(object sender, EventArgs e)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IDisbursement newDisbursement = _disbursementRepository.CreateEmptyDisbursement();

            if (_view.SelectedBankAccount > 0)
            {
                if (_capReadvance == false)
                {
                    IBankAccountRepository bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    IBankAccount bankAccount = bankRepo.GetBankAccountByKey(Convert.ToInt32(_view.SelectedBankAccount));
                    IACBBranch branch = BankRepo.GetACBBranchByKey(bankAccount.ACBBranch.Key);

                    newDisbursement.ACBBank = branch.ACBBank;
                    newDisbursement.ACBBranch = branch;
                    newDisbursement.ACBType = lookupRepo.BankAccountTypes.ObjectDictionary[bankAccount.ACBType.Key.ToString()];
                    newDisbursement.AccountName = bankAccount.AccountName;
                    newDisbursement.AccountNumber = bankAccount.AccountNumber;

                    newDisbursement.Account = _account;

                    if (_unpostedDisbursements.Count > 0)
                    {
                        newDisbursement.DisbursementTransactionType = _unpostedDisbursements[0].DisbursementTransactionType;
                    }
                    else
                    {
                        int disbursementTypeKey = _view.DisbursementTypeSelectedValue;
                        if (disbursementTypeKey != -1)
                        {
                            for (int typeIndex = 0; typeIndex < _disbursementTypeList.Count; typeIndex++)
                            {
                                if (_disbursementTypeList[typeIndex].Key == disbursementTypeKey)
                                {
                                    IDisbursementTransactionType disbursementType = _disbursementTypeList[typeIndex];

                                    newDisbursement.DisbursementTransactionType = disbursementType;
                                    break;
                                }
                            }
                        }
                    }

                    newDisbursement.Amount = _view.DisbursementAmount;
                    newDisbursement.PreparedDate = DateTime.Today;
                    newDisbursement.DisbursementStatus = lookupRepo.DisbursementStatuses.ObjectDictionary[Convert.ToInt32(DisbursementStatuses.Pending).ToString()];
                }
                else
                {

                    if (_bankAccountList.Count > 0)
                    {
                        IBankAccount bankAccount = _bankAccountList[0];
                        IACBBranch branch = BankRepo.GetACBBranchByKey(bankAccount.ACBBranch.Key);

                        newDisbursement.ACBBank = branch.ACBBank;
                        newDisbursement.ACBBranch = branch;
                        newDisbursement.ACBType = lookupRepo.BankAccountTypes.ObjectDictionary[bankAccount.ACBType.Key.ToString()];
                        newDisbursement.AccountName = "CAP2 Readvance";
                        newDisbursement.AccountNumber = bankAccount.AccountNumber;

                    }

                    newDisbursement.Account = _account;

                    if (_unpostedDisbursements.Count > 0)
                    {
                        newDisbursement.DisbursementTransactionType = _unpostedDisbursements[0].DisbursementTransactionType;
                    }
                    else
                    {
                        int disbursementTypeKey = _view.DisbursementTypeSelectedValue;
                        if (disbursementTypeKey != -1)
                        {
                            for (int typeIndex = 0; typeIndex < _disbursementTypeList.Count; typeIndex++)
                            {
                                if (_disbursementTypeList[typeIndex].Key == disbursementTypeKey)
                                {
                                    IDisbursementTransactionType disbursementType = _disbursementTypeList[typeIndex];

                                    newDisbursement.DisbursementTransactionType = disbursementType;
                                    break;
                                }
                            }
                        }
                    }

                    newDisbursement.Amount = _capReadvanceAmount;
                    newDisbursement.PreparedDate = DateTime.Today;
                    newDisbursement.DisbursementStatus = lookupRepo.DisbursementStatuses.ObjectDictionary[Convert.ToInt32(DisbursementStatuses.Pending).ToString()];

                }
                _unpostedDisbursements.Add(newDisbursement);

                PrivateCacheData[_cachedDataUnpostedDisbursementKey] = _unpostedDisbursements;
                PrivateCacheData[_cachedDataDisbursementSavedKey] = false;

                PrivateCacheData.Remove("SelectedDisbursementTypeKey");
                PrivateCacheData.Add("SelectedDisbursementTypeKey", _unpostedDisbursements[0].DisbursementTransactionType.Key);

                BindGridAndSetControlVisiblility();
                _view.DisbursementAmount = 0; // reset disbursement amount
            }
            else
                _view.Messages.Add(new Error("Please select Bank Account.", "Please select Bank Account."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnDeleteDisbursementClicked(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                int gridIndex = Convert.ToInt32(e.Key);

                if (PrivateCacheData.ContainsKey(_cachedDataDisbursementSavedKey) && Convert.ToBoolean(PrivateCacheData[_cachedDataDisbursementSavedKey]) == false)
                    _unpostedDisbursements.RemoveAt(gridIndex);
                else
                {
                    if ((PrivateCacheData.ContainsKey(_cachedDataDisbursementSavedKey) && Convert.ToBoolean(PrivateCacheData[_cachedDataDisbursementSavedKey]) == true) && _unpostedDisbursements.Count != 1)
                        _unpostedDisbursements.RemoveAt(gridIndex);
                    else
                        _view.Messages.Add(new Error("You may not delete all transactions. Please use the Delete Disbursement option.", "You may not delete all transactions. Please use the Delete Disbursement option."));
                }
                PrivateCacheData[_cachedDataUnpostedDisbursementKey] = _unpostedDisbursements;
                PrivateCacheData[_cachedDataDisbursementSavedKey] = false;

                BindGridAndSetControlVisiblility();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSaveButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(_unpostedDisbursements);
                _disbursementRepository.SaveDisbursement(disburseList, _view.DisbursementTotalAmount);
                if (_view.IsValid)
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
            {
                PrivateCacheData[_cachedDataDisbursementSavedKey] = true;
                BindGridAndSetControlVisiblility();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_LoanTransactionGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnDisbursementTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            PrivateCacheData.Remove(_cachedDataUnpostedDisbursementKey);
            BindGridAndSetControlVisiblility();
        }


        #endregion

        private IBankAccountRepository BankRepo
        {
            get
            {
                if (_bankRepo == null)
                    _bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();

                return _bankRepo;
            }
        }

    }
}

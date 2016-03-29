using System;
using System.Linq;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class CATSDisbursementAdd : SAHLCommonBasePresenter<ICATSDisbursement>
    {
        private IDisbursementRepository _disbursementRepository;
        private ILoanTransactionRepository _ltRepository;
        private IList<IDisbursement> _unpostedDisbursements;
        private IAccount _account;
        private IReadOnlyEventList<IDisbursementTransactionType> _disbursementTypeList;
        private List<IBankAccount> _bankAccountList;
        private IApplicationRepository _appRepo;
        private ILookupRepository _lookupRepo;
        private IBankAccountRepository _bankRepo;

        bool saveDisbursements;
        bool _capReadvance;
        double _capReadvanceAmount;
        bool _allowUpdate;
        IApplication _app;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CATSDisbursementAdd(ICATSDisbursement view, SAHLCommonBaseController controller)
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

            _view.SubmitButtonText = "Post";

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

            if (PrivateCacheData.ContainsKey("NeedToSaveDisbursements"))
            {
                saveDisbursements = !Convert.ToBoolean(PrivateCacheData["NeedToSaveDisbursements"]);
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

            //setup confirmation messages
            if (PrivateCacheData.ContainsKey("ReadvanceAdded"))
            {
                _view.DisplayCancelConfirmationMessage = true;
                _view.CancelConfirmationMessage = "If you cancel without saving, the CATS Disbursement Transactions captured above will be lost. Are you sure you want to proceed ? ";
            }
            _view.DisplaySubmitConfirmationMessage = true;

            _view.DisplayDeleteConfirmationMessage = true;
            _view.DeleteConfirmationMessage = "Are you sure you would like to delete the CATS Disbursement transaction?";

            _view.DisplaySaveConfirmationMessage = true;

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
            _ltRepository = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
            _view.OnCancelButtonClicked += (_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
            _view.OnAddDisbursementClicked += (_view_OnAddDisbursementClicked);
            _view.OnDeleteDisbursementClicked += (_view_OnDeleteDisbursementClicked);
            _view.OnSaveButtonClicked += (_view_OnSaveButtonClicked);
            _view.OnLoanTransactionGridSelectedIndexChanged += (_view_LoanTransactionGridSelectedIndexChanged);
            _view.OnDisbursementTypeSelectedIndexChanged += (_view_OnDisbursementTypeSelectedIndexChanged);

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            if (cboNode != null)
                switch (cboNode.GenericKeyTypeKey)
                {
                    case (int)GenericKeyTypes.Account:
                    case (int)GenericKeyTypes.ParentAccount:
                        _account = accRepository.GetAccountByKey(cboNode.GenericKey);
                        break;
                    case (int)GenericKeyTypes.Offer:
                        _app = AppRepo.GetApplicationByKey(cboNode.GenericKey);
                        _account = accRepository.GetAccountByApplicationKey(_app.Key);
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
            }

            if (PrivateCacheData.ContainsKey("SelectedDisbursementTypeKey"))
                _view.DisbursementTypeSelectedValue = Convert.ToInt32(PrivateCacheData["SelectedDisbursementTypeKey"]);

            if (PrivateCacheData.ContainsKey("UnPostedDisbursements"))
                _unpostedDisbursements = (IList<IDisbursement>)PrivateCacheData["UnPostedDisbursements"];
            else
            {
                IReadOnlyEventList<IDisbursement> pendingDisbursements = _disbursementRepository.GetDisbursmentsByParentAccountKeyAndStatus(_account.Key, Convert.ToInt32(DisbursementStatuses.Pending));
                _unpostedDisbursements = new List<IDisbursement>();

                for (int i = 0; i < pendingDisbursements.Count; i++)
                {
                    _unpostedDisbursements.Add(pendingDisbursements[i]);
                }
            }

            if (_account != null)
            {
                _view.DisbursementGridPostBackType = GridPostBackType.NoneWithClientSelect;
                BindGridAndSetControlVisiblility();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void BindGridAndSetControlVisiblility()
        {
            if (!_view.IsPostBack)
            {
                if (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0)
                {
                    BindBankAccounts();
                    _view.BindGridDisbursements(_unpostedDisbursements);
                    PrivateCacheData["NeedToSaveDisbursements"] = true;

                    double totalDisbursements = 0;
                    for (int i = 0; i < _unpostedDisbursements.Count; i++)
                    {
                        if (_unpostedDisbursements[i].Amount.HasValue)
                            totalDisbursements = totalDisbursements + _unpostedDisbursements[i].Amount.Value;
                    }
                    _view.DisbursementTotalAmount = totalDisbursements;
                }
                else
                    PrivateCacheData["NeedToSaveDisbursements"] = false;
            }
            else
            {
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

                if (_capReadvance)
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

            _disbursementTypeList = _disbursementRepository.GetDisbursementTransactionTypes(_view.CurrentPrincipal);

            string totalamount = _view.DisbursementTotalAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            string selectedvalue = "";
            if ((_disbursementTypeList.Count > 0) && (_view.DisbursementTypeSelectedValue >= 0))
                for (int i = 0; i < _disbursementTypeList.Count; i++)
                    if (_disbursementTypeList[i].Key == _view.DisbursementTypeSelectedValue)
                    {
                        selectedvalue = _disbursementTypeList[i].Description;
                        break;
                    }
            _view.SaveConfirmationMessage = "Are you sure you would like to create the financial transaction " + selectedvalue + " for " + totalamount + " ?";
            _view.SubmitConfirmationMessage = "Are you sure you would like to create the financial transaction " + selectedvalue + " for " + totalamount + " ?";
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

            if (_account.Product.Key == (int)Products.PersonalLoan)
            {
                // Personal loans we can only show the bankaccount that is the DO bank account
                IFinancialService fs = _account.GetFinancialServiceByType(FinancialServiceTypes.PersonalLoan);
                if (fs.CurrentBankAccount != null && fs.CurrentBankAccount.BankAccount != null)
                    _bankAccountList.Add(fs.CurrentBankAccount.BankAccount);
                else
                    _view.Messages.Add(new DomainMessage("No Debit Order Bank Account setup for this account", "No Debit Order Bank Account setup for this account"));
            }
            else if (disbursementType != (int)DisbursementTransactionTypes.CAP2ReAdvance)
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
        private static bool BankAccountExists(IList<IBankAccount> bankAccountList, IBankAccount compareAccount)
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
        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (_view.IsValid)
                _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        private void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_unpostedDisbursements != null && _unpostedDisbursements.Count > 0)
            {
                if (_view.IsValid)
                {
                    #region Setup Parameters
                    List<IDisbursement> disbursementLst = new List<IDisbursement>();
                    //effective date must always have a 0 timestamp
                    DateTime effectiveDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    string DisbursementReference = _view.DisbursementReference;
                    IFinancialService fs = null;
                    switch (_account.Product.Key)
                    {
                        case (int)Products.VariableLoan:
                        case (int)Products.VariFixLoan:
                        case (int)Products.SuperLo:
                        case (int)Products.DefendingDiscountRate:
                        case (int)Products.NewVariableLoan:
                        case (int)Products.Edge:
                            {
                                fs = _account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan);
                                break;
                            }
                        case (int)Products.PersonalLoan:
                            {
                                fs = _account.GetFinancialServiceByType(FinancialServiceTypes.PersonalLoan);
                                break;
                            }
                        default:
                            {
                                throw new ArgumentException(string.Format("Unsupported Product: {0} - AccountKey: {1}", _account.Product.Key, _account.Key));
                            }
                    }

                    if (fs == null)
                        throw new Exception("No variable financial service record found.");
                    #endregion

                    TransactionScope txns = null;

                    using (txns = new TransactionScope(OnDispose.Rollback))
                    {
                        try
                        {
                            //Set Disbursement status and validate
                            foreach (IDisbursement disb in _unpostedDisbursements)
                            {
                                //need to get the saved disbursements from the DB to avoid session errors or reattach
                                disbursementLst.Add(_disbursementRepository.GetDisbursementByKey(disb.Key));
                            }
                            SetDisbursementsStatus(disbursementLst, DisbursementStatuses.Pending);

                            #region Setup Disbursement specific Parameters

                            if (_capReadvance)
                                DisbursementReference = "CAP2-" + disbursementLst[0].Key.ToString();

                            int ttNumber = disbursementLst.Select(x => x.DisbursementTransactionType.TransactionTypeNumber).FirstOrDefault().Value;

                            #endregion

                            if (!_view.IsValid) //Return if there are validation errors
                                return;

                            //pProcessTran: Sngle Financial Transaction for total disbursement amount
                            _ltRepository.pProcessTran(fs.Key, ttNumber, effectiveDate, _view.DisbursementTotalAmount, DisbursementReference, _view.CurrentPrincipal.Identity.Name);

                            if (!_view.IsValid) //Return if there are validation errors
                                return;

                            //Insert Disbursement Trnasaction for each disbursement
                            foreach (IDisbursement disb in disbursementLst)
                            {
                                //Post Transaction
                                _disbursementRepository.PostDisbursementTransaction(disb.Key,
                                    effectiveDate,
                                    DisbursementReference,
                                    _view.CurrentPrincipal.Identity.Name);
                            }

                            if (!_view.IsValid) //Return if there are validation errors
                                return;

                            //Instalment change
                            if (ttNumber == (int)TransactionTypes.Readvance || ttNumber == (int)TransactionTypes.ReadvanceCAP)
                            {
                                IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                                mlRepo.InstallmentChange(_account.Key, _view.CurrentPrincipal.Identity.Name, DisbursementReference);
                            }

                            if (_view.IsValid)
                            {
                                txns.VoteCommit();
                                _view.Navigator.Navigate("Cancel");
                            }

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

        ///<summary>
        ///</summary>
        ///<param name="whenTimeIsOver"></param>
        ///<returns></returns>
        private bool IsTimeOver(TimeSpan whenTimeIsOver)
        {
            return DateTime.Now.TimeOfDay > whenTimeIsOver;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="disbursementLst"></param>
        /// <param name="disbStatus"></param>
        private void SetDisbursementsStatus(List<IDisbursement> disbursementLst, DisbursementStatuses disbStatus)
        {
            foreach (IDisbursement disb in disbursementLst)
            {
                disb.DisbursementStatus = LookupRepo.DisbursementStatuses.ObjectDictionary[Convert.ToInt32(disbStatus).ToString()];
            }

            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(disbursementLst);
            _disbursementRepository.SaveDisbursement(disburseList, _view.DisbursementTotalAmount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnAddDisbursementClicked(object sender, EventArgs e)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            IDisbursement newDisbursement = _disbursementRepository.CreateEmptyDisbursement();

            if ((_view.SelectedBankAccount > 0) && (_view.IsValid))
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

                if (_account.Product.Key == (int)Products.PersonalLoan && ValidatePersonalLoanDisbursement(_view.DisbursementTypeSelectedValue) == false)
                    return;

                _unpostedDisbursements.Add(newDisbursement);

                PrivateCacheData["UnPostedDisbursements"] = _unpostedDisbursements;
                PrivateCacheData["NeedToSaveDisbursements"] = false;

                PrivateCacheData.Remove("SelectedDisbursementTypeKey");
                PrivateCacheData.Add("SelectedDisbursementTypeKey", _unpostedDisbursements[0].DisbursementTransactionType.Key);

                BindGridAndSetControlVisiblility();
                _view.DisbursementAmount = 0; // reset disbursement amount

                // Used for Javascript button click validation
                PrivateCacheData.Remove("ReadvanceAdded");
                PrivateCacheData.Add("ReadvanceAdded", true);
            }
            else
                _view.Messages.Add(new Error("Please select Bank Account.", "Please select Bank Account."));

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnDeleteDisbursementClicked(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                int gridIndex = Convert.ToInt32(e.Key);

                if (PrivateCacheData.ContainsKey("NeedToSaveDisbursements") && Convert.ToBoolean(PrivateCacheData["NeedToSaveDisbursements"]) == false)
                    _unpostedDisbursements.RemoveAt(gridIndex);
                else
                {
                    if ((PrivateCacheData.ContainsKey("NeedToSaveDisbursements") && Convert.ToBoolean(PrivateCacheData["NeedToSaveDisbursements"])) && _unpostedDisbursements.Count != 1)
                        _unpostedDisbursements.RemoveAt(gridIndex);
                    else
                        _view.Messages.Add(new Error("You may not delete all transactions. Please use the Delete Disbursement option.", "You may not delete all transactions. Please use the Delete Disbursement option."));
                }

                PrivateCacheData["UnPostedDisbursements"] = _unpostedDisbursements;
                PrivateCacheData["NeedToSaveDisbursements"] = false;

                BindGridAndSetControlVisiblility();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnSaveButtonClicked(object sender, EventArgs e)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            TransactionScope txn = new TransactionScope();

            try
            {
                // Custom Validation
                IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(_unpostedDisbursements);
                svc.ExecuteRule(spc.DomainMessages, "CATSDisbursementDebitOrderSuspendedCapReAdvance", disburseList);

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
                PrivateCacheData["NeedToSaveDisbursements"] = true;
                BindGridAndSetControlVisiblility();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _view_LoanTransactionGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnDisbursementTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            PrivateCacheData.Remove("UnPostedDisbursements");
            BindGridAndSetControlVisiblility();
        }

        #endregion Event Handlers

        private IApplicationRepository AppRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        private ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private IBankAccountRepository BankRepo
        {
            get
            {
                if (_bankRepo == null)
                    _bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();

                return _bankRepo;
            }
        }

        private bool ValidatePersonalLoanDisbursement(int disbursementTransactionTypeKey)
        {
            bool valid = true;
            string msg = string.Empty;

            //Check Disbursement Cut-Off Time
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            if (svc.ExecuteRule(_view.Messages, "PersonalLoanDisbursementCutOffTimeCheck", null) == 0)
                valid = false;

            IFinancialService fsPersonalLoan = _account.GetFinancialServiceByType(FinancialServiceTypes.PersonalLoan);

            if (disbursementTransactionTypeKey == (int)SAHL.Common.Globals.DisbursementTransactionTypes.Refund)
            {
                // check that  - Current Balance + Amt To Disburse cannot be greater than the initial balance
                if (fsPersonalLoan.Balance.Amount + _view.DisbursementTotalAmount > fsPersonalLoan.Balance.LoanBalance.InitialBalance)
                {
                    msg = "The total disbursement amount (" + _view.DisbursementTotalAmount.ToString(SAHL.Common.Constants.CurrencyFormat) + ") + current balance ("
                        + fsPersonalLoan.Balance.Amount.ToString(SAHL.Common.Constants.CurrencyFormat)
                        + ") cannot be greater than the initial balance ("
                        + fsPersonalLoan.Balance.LoanBalance.InitialBalance.ToString(SAHL.Common.Constants.CurrencyFormat) + ")";
                    _view.Messages.Add(new Error(msg, msg));
                    valid = false;
                }
            }

            return valid;
        }
    }
}
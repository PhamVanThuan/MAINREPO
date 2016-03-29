using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Caching;

namespace SAHL.Web.Views.Common.Presenters.SettlementBanking
{
    public class SettlementBankingDetailsUpdate : SettlementBankingDetailsBase
    {
        public SettlementBankingDetailsUpdate(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BankAccountGridEnabled = true;
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelClicked += (_view_OnCancelClicked);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
            _view.ControlsVisible = true;
            _view.ShowStatus = false;
            _view.SubmitButtonText = "Update";
            _view.SearchButtonVisible = true;
            _view.AccountTypeBondOnly = false;
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            if (!_view.IsPostBack)
            {
                _view.BindBankAccountControls(lookups.Banks, lookups.BankAccountTypes, lookups.GeneralStatuses);
                _view.SetControlsToFirstAccount = true;
                SetupPrivatecache();
            }
            else
            {
                _view.BindBankAccountControls(lookups.Banks, lookups.BankAccountTypes, lookups.GeneralStatuses);
                _view.SetControlsToFirstAccount = true;

                _view.GETSETddlBank = (int)PrivateCacheData["BANK"];
                _view.GETSETtxtBranch = (string)PrivateCacheData["BRANCHCODE"];
                _view.GETSETddlAccountType = (int)PrivateCacheData["ACCOUNTTYPE"];
                _view.GETSETtxtAccountNumber = (string)PrivateCacheData["ACCOUNTNUMBER"];
                _view.GETSETtxtAccountName = (string)PrivateCacheData["ACCOUNTNAME"];
                // _view.SetControlsToSearchValues = true;

            }

            if (GlobalCacheData.ContainsKey("BANK"))
            {
                _view.BankValue = GlobalCacheData["BANK"] as string;
                _view.BranchCodeValue = GlobalCacheData["BRANCHCODE"] as string;
                _view.AccountTypeValue = GlobalCacheData["ACCOUNTTYPE"] as string;
                _view.AccountNumberValue = GlobalCacheData["ACCOUNTNUMBER"] as string;
                _view.AccountNameValue = GlobalCacheData["ACCOUNTNAME"] as string;
                _view.SetControlsToSearchValues = true;
                GlobalCacheData.Clear();
            }


        }

        private void SetupPrivatecache()
        {
            PrivateCacheData.Clear();
            PrivateCacheData.Add("BANK", _view.GETSETddlBank);
            PrivateCacheData.Add("BRANCHCODE", _view.GETSETtxtBranch);
            PrivateCacheData.Add("ACCOUNTTYPE", _view.GETSETddlAccountType);
            PrivateCacheData.Add("ACCOUNTNUMBER", _view.GETSETtxtAccountNumber);
            PrivateCacheData.Add("ACCOUNTNAME", _view.GETSETtxtAccountName);
        }

 
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            SetupPrivatecache();
            TransactionScope ts = new TransactionScope();
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                if (View.SelectedBankAccount != null)
                {
                    IBankAccount ba = _view.SelectedBankAccount;
                    if (_view.BranchKey.Length != 0)
                    {
                        IACBBranch branch = BAR.GetACBBranchByKey(_view.BranchKey);
                        ba.ACBBranch = branch;
                    }
                    else
                    {
                        ba.ACBBranch = null;
                    }
                    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    for (int x = 0; x < LR.BankAccountTypes.Count; x++)
                    {
                        if (LR.BankAccountTypes[x].Key == Convert.ToInt32(_view.AccountTypeValue))
                        //if (LR.BankAccountTypes[x].Key == (int)SAHL.Common.Globals.ACBTypes.Bond)
                        {
                            ba.ACBType = LR.BankAccountTypes[x];
                            break;
                        }
                    }
                    //const int BondKey = (int)SAHL.Common.Globals.ACBTypes.Bond;
                    //ba.ACBType = LR.BankAccountTypes.ObjectDictionary[BondKey.ToString()];
                    
                    ba.AccountName = _view.AccountName;
                    ba.AccountNumber = _view.AccountNumber;
                    ba.ChangeDate = DateTime.Today;
                    ba.UserID = spc.GetADUser(_view.CurrentPrincipal).ADUserName;
                    IBankAccountRepository BankRep = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    BankRep.SaveBankAccount(ba);
                }
                else //Add
                {
                    IBankAccount ba = BAR.GetEmptyBankAccount();
                    if (_view.BranchKey.Length != 0)
                    {
                        IACBBranch branch = BAR.GetACBBranchByKey(_view.BranchKey);
                        ba.ACBBranch = branch;
                    }
                    else
                    {
                        ba.ACBBranch = null;
                    }
                    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    for (int x = 0; x < LR.BankAccountTypes.Count; x++)
                    {
                        if (LR.BankAccountTypes[x].Key == Convert.ToInt32(_view.AccountTypeValue))
                        //if (LR.BankAccountTypes[x].Key == (int)SAHL.Common.Globals.ACBTypes.Bond)
                        {
                            ba.ACBType = LR.BankAccountTypes[x];
                            break;
                        }
                    }

                    ba.AccountName = _view.AccountName;
                    ba.AccountNumber = _view.AccountNumber;
                    ba.ChangeDate = DateTime.Today;
                    ba.UserID = spc.GetADUser(_view.CurrentPrincipal).ADUserName;
                    IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplicationExpense ae = AR.GetEmptyApplicationExpense();
                    ISupportsVariableLoanApplicationInformation vlInfo = null;
                    if (Application.GetLatestApplicationInformation() != null)
                    {
                        vlInfo = Application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                    }
                    BAR.SaveBankAccount(ba);

                    ae.Application = Application;

                    const int expenseTypeKey = (int)SAHL.Common.Globals.ExpenseTypes.Existingmortgageamount;
                    ae.ExpenseType = LR.ExpenseTypes.ObjectDictionary[expenseTypeKey.ToString()];

                    if (vlInfo != null)
                    {
                        ae.TotalOutstandingAmount = vlInfo.VariableLoanInformation.ExistingLoan.HasValue ? vlInfo.VariableLoanInformation.ExistingLoan.Value : 0;
                    }

                    Application.ApplicationExpenses.Add(_view.Messages, ae);

                    IApplicationDebtSettlement ads = AR.GetEmptyApplicationDebtSettlement();

                    if (vlInfo != null)
                    {
                        ads.CapitalAmount = vlInfo.VariableLoanInformation.ExistingLoan.HasValue ? vlInfo.VariableLoanInformation.ExistingLoan.Value : 0;
                    }

                    ads.BankAccount = ba;

                    const int disbursementType = (int)SAHL.Common.Globals.DisbursementTypes.Registrationdisbursement;
                    ads.DisbursementType = LR.DisbursementTypes.ObjectDictionary[disbursementType.ToString()];

                    ads.InterestStartDate = DateTime.Today;
                    ads.SettlementDate = DateTime.Today;

                    AR.SaveApplicationDebtSettlement(ads);
                    ae.ApplicationDebtSettlements.Add(_view.Messages, ads);
                    AR.SaveApplication(Application);
                }

                ts.VoteCommit();
                GlobalCacheData.Clear();
                _view.Navigator.Navigate("SettlementBankingDetailsDisplay");
            }
            catch (Exception)
            {
                ts.VoteRollBack();

                _view.SetControlsToFirstAccount = false;

                _view.GETSETddlBank = (int)PrivateCacheData["BANK"];
                _view.GETSETtxtBranch = (string)PrivateCacheData["BRANCHCODE"];
                _view.GETSETddlAccountType = (int)PrivateCacheData["ACCOUNTTYPE"];
                _view.GETSETtxtAccountNumber = (string)PrivateCacheData["ACCOUNTNUMBER"];
                _view.GETSETtxtAccountName = (string)PrivateCacheData["ACCOUNTNAME"];

                _view.SetControlsToSearchValues = true;

                //if (!_view.IsValid)
                //{
                //    throw;
                //}
                //else
                //{
                //    // IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();

                //    _view.SetControlsToFirstAccount = false;

                //    _view.BankValue = _view.BankKey;
                //    _view.BranchCodeValue = _view.BranchCode;
                //    _view.AccountTypeValue = _view.AccountType;
                //    _view.AccountNumberValue = _view.AccountNumber;
                //    _view.AccountNameValue = _view.AccountName;
                //    _view.SetControlsToSearchValues = true;

                //}
            }
            finally
            {
                ts.Dispose();
            }

        }

        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

    }
}

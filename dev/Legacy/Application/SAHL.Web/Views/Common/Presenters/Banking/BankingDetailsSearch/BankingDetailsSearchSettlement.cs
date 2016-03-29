using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common.Presenters.Banking.BankingDetailsSearch
{
    public class BankingDetailsSearchSettlement : BankingDetailsSearchBase
    {
        public BankingDetailsSearchSettlement(IBankingDetailsSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnUseButtonClicked += _view_OnUseButtonClicked;
            _view.OnSearchGridSelectedIndexChanged += _view_OnSearchGridSelectedIndexChanged;


        }

        void _view_OnSearchGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            PrivateCacheData.Remove("ValuationsIndex");
            PrivateCacheData.Add("ValuationsIndex", View.BankDetailsSearchGridItemIndex);

        }

        void _view_OnUseButtonClicked(object sender, KeyChangedEventArgs e)
        {  
            TransactionScope ts = new TransactionScope();
            try
            {
                KeyChangedEventArgs arg = e;
                int bankAccountKey = int.Parse(arg.Key.ToString());
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                IBankAccount ba = BAR.GetBankAccountByKey(bankAccountKey);
                IApplication _app = null;
                ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                {
                    int appKey = int.Parse(GlobalCacheData[ViewConstants.ApplicationKey].ToString());
                    _app = AR.GetApplicationByKey(appKey);
                }

                IApplicationExpense ae = AR.GetEmptyApplicationExpense();
                IApplicationInformationVariableLoan vlInfo = null;
                if (_app.GetLatestApplicationInformation() != null)
                {
                    vlInfo = AR.GetApplicationInformationVariableLoan(_app.GetLatestApplicationInformation().Key);
                }

                ae.Application = _app;

                int expenseTypeKey = (int)SAHL.Common.Globals.ExpenseTypes.Existingmortgageamount;
                ae.ExpenseType = LR.ExpenseTypes.ObjectDictionary[expenseTypeKey.ToString()];

                if (GlobalCacheData.ContainsKey("ACCOUNTREFERENCE"))
                    ae.ExpenseReference = (string)GlobalCacheData["ACCOUNTREFERENCE"];

                if (vlInfo != null)
                {
                    ae.TotalOutstandingAmount = vlInfo.ExistingLoan.HasValue ? vlInfo.ExistingLoan.Value : 0;
                }


                _app.ApplicationExpenses.Add(_view.Messages, ae);

                IApplicationDebtSettlement ads = AR.GetEmptyApplicationDebtSettlement();

                if (vlInfo != null)
                {
                    ads.CapitalAmount = vlInfo.ExistingLoan.HasValue ? vlInfo.ExistingLoan.Value : 0;
                }

                ba.ChangeDate = DateTime.Today;
                ba.UserID = _view.CurrentPrincipal.Identity.Name;
                ads.BankAccount = ba;

                //todo: set this reference a globals entry (remove hardcoded key);
                ads.DisbursementType = LR.DisbursementTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.DisbursementTypes.Registrationdisbursement)];
                //todo : check if it is ok to set the following values
                ads.InterestStartDate = DateTime.Today;
                ads.SettlementDate = DateTime.Today;

                AR.SaveApplicationDebtSettlement(ads);
                ae.ApplicationDebtSettlements.Add(_view.Messages, ads);
                AR.SaveApplication(_app);

                ts.VoteCommit();
                _view.Navigator.Navigate("SettlementBankingDetailsDisplay");
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                {
                    throw;
                }
            }
            finally
            {
                ts.Dispose();
            }







            //GlobalCacheData["BANK"] = ba.ACBBranch.ACBBank.Key;
            //GlobalCacheData["BRANCHCODE"] = ba.ACBBranch.Key + " - " +  ba.ACBBranch.ACBBranchDescription;
            //GlobalCacheData["ACCOUNTNAME"] = ba.AccountName;
            //GlobalCacheData["ACCOUNTTYPE"] = ba.ACBType.Key;
            //GlobalCacheData["ACCOUNTNUMBER"] = ba.AccountNumber;

            //_view.Navigator.Navigate("SettlementBankingDetailsDisplay");

        }

    }
}

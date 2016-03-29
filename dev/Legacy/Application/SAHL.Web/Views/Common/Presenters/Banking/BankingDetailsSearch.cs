using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;
using SAHL.Common.Caching;

namespace SAHL.Web.Views.Common.Presenters.Banking
{
    /// <summary>
    /// 
    /// </summary>
    public class BankingDetailsSearch : SAHLCommonBasePresenter<IBankingDetailsSearch>
    {
        const string foundBankAccounts = "FOUNDBANKACCOUNTS";      
        const string callingView = "CALLINGVIEW";
        const string nextView = "NEXTVIEW";
        const string legalEntity = "LEGALENTITY";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BankingDetailsSearch(IBankingDetailsSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {             
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {            
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnUseButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnUseButtonClicked);
            _view.OnSearchGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnSearchGridSelectedIndexChanged);

                        IBankAccountSearchCriteria criteria = null;

            if(GlobalCacheData.ContainsKey(foundBankAccounts))
            {
                criteria = GlobalCacheData[foundBankAccounts] as IBankAccountSearchCriteria;
            }

            IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
            IEventList<ILegalEntityBankAccount> lstFoundAcccounts = BAR.SearchLegalEntityBankAccounts(criteria, 100);

            List<BankingDetailsSearchGridRowItem> lstItems = new List<BankingDetailsSearchGridRowItem>();


            for (int x = 0; x < lstFoundAcccounts.Count; x++)
            {
                BankingDetailsSearchGridRowItem itm = new BankingDetailsSearchGridRowItem();
                itm.BankAccountKey = lstFoundAcccounts[x].BankAccount.Key;
                itm.ACBBankDescription = lstFoundAcccounts[x].BankAccount.ACBBranch.ACBBank.ACBBankDescription;
                itm.ACBBranchDescription = lstFoundAcccounts[x].BankAccount.ACBBranch.ACBBranchDescription;
                itm.ACBBranchKey = lstFoundAcccounts[x].BankAccount.ACBBranch.Key;
                itm.AccountName = lstFoundAcccounts[x].BankAccount.AccountName;
                itm.AccountNumber = lstFoundAcccounts[x].BankAccount.AccountNumber;
                itm.BankAccount = lstFoundAcccounts[x].BankAccount;                
                itm.LegalEntityKey = lstFoundAcccounts[x].LegalEntity.Key;
                if (lstFoundAcccounts[x].LegalEntity.DisplayName != null)
                    itm.LegalEntityName = lstFoundAcccounts[x].LegalEntity.DisplayName;

                if (lstFoundAcccounts[x].LegalEntity.LegalNumber != null)
                    itm.LegalEntityNumber = lstFoundAcccounts[x].LegalEntity.LegalNumber;
                if (lstFoundAcccounts[x].LegalEntity.LegalEntityStatus != null)
                    itm.LegalEntityStatus = lstFoundAcccounts[x].LegalEntity.LegalEntityStatus.Description;
                lstItems.Add(itm);
            }



            IEventList<IBankAccount> lstAccounts = BAR.SearchNonLegalEntityBankAccounts(criteria, 100);

            for (int x = 0; x < lstAccounts.Count; x++)
            {
                BankingDetailsSearchGridRowItem itm = new BankingDetailsSearchGridRowItem();
                itm.BankAccount = lstAccounts[x];
                itm.BankAccountKey = lstAccounts[x].Key;
                itm.ACBBranchKey = lstAccounts[x].ACBBranch.Key;
                itm.LegalEntityName = lstAccounts[x].AccountName;
                itm.AccountName = lstAccounts[x].AccountName;
                itm.AccountNumber = lstAccounts[x].AccountNumber;
                itm.ACBBankDescription = lstAccounts[x].ACBBranch.ACBBank.ACBBankDescription;
                lstItems.Add(itm);
            }


            _view.BindSearchGrid(lstItems);
        }

        /// <summary>
        /// Handles the view's OnCancelButtonClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                _view.Navigator.Navigate(GlobalCacheData[ViewConstants.NavigateTo].ToString());
            else
                _view.Navigator.Navigate("Cancel");

        }

        /// <summary>
        /// Handles the view's OnUseButtonClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        void _view_OnUseButtonClicked(object sender, object key)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                KeyChangedEventArgs arg = key as KeyChangedEventArgs;
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILegalEntity le = null;
                if (GlobalCacheData.ContainsKey("LEGALENTITY")) //LEGAL ENTITY BANK ACCCOUNT SEARCH
                {
                    le = GlobalCacheData["LEGALENTITY"] as ILegalEntity;
                    
                    // TODO: this is a temp fix - should the LE even be in the cache - can't we 
                    // just get it from the CBO? - not reloading it causes problems
                    le = LER.GetLegalEntityByKey(le.Key);

                    ILegalEntityBankAccount leBA = LER.GetEmptyLegalEntityBankAccount();
                    int bankAccountKey = int.Parse(arg.Key.ToString());
                    IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    IBankAccount ba = BAR.GetBankAccountByKey(bankAccountKey);
                    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    leBA.GeneralStatus = LR.GeneralStatuses.ObjectDictionary[((int)GeneralStatuses.Active).ToString()];
                    leBA.BankAccount = ba;
                    leBA.LegalEntity = le;
                    le.LegalEntityBankAccounts.Add(_view.Messages, leBA);
                    ExclusionSets.Add(RuleExclusionSets.BankingDetailsViewLegalEntity);
                    LER.SaveLegalEntity(le, false);
                    ExclusionSets.Remove(RuleExclusionSets.BankingDetailsViewLegalEntity);

                    ts.VoteCommit();
                    _view.Navigator.Navigate("Use");            
                }
                else // SETTLEMENT BANKING DETAILS SEARCH
                {
                    IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication application = null;
                    if (GlobalCacheData.ContainsKey("APPLICATION"))
                    {
                        int bankAccountKey = int.Parse(arg.Key.ToString());
                        IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                        IBankAccount ba = BAR.GetBankAccountByKey(bankAccountKey);
                        
                        application = GlobalCacheData["APPLICATION"] as IApplication;                        
                        IApplicationExpense ae = null;
                        int expenseTypeKey = (int)SAHL.Common.Globals.ExpenseTypes.Existingmortgageamount;
                        ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                        foreach (IApplicationExpense aExpense in application.ApplicationExpenses)
                        {
                            aExpense.ExpenseType = LR.ExpenseTypes.ObjectDictionary[expenseTypeKey.ToString()];
                            ae = aExpense;
                            break;
                        }
                        if (ae != null) // Already exists - Update
                        {
                            IApplicationDebtSettlement ads = null;
                            if (ae.ApplicationDebtSettlements.Count > 0)
                            {
                                ads = ae.ApplicationDebtSettlements[0];
                            }
                            else
                            {
                                ads = AR.GetEmptyApplicationDebtSettlement();
                            }

                            IApplicationInformationVariableLoan vlInfo = null;


                            if (application.ApplicationInformations.ObjectDictionary != null)
                            {
                                if (application.GetLatestApplicationInformation() != null)
                                {
                                    vlInfo = AR.GetApplicationInformationVariableLoan(application.GetLatestApplicationInformation().Key);
                                }
                            }
                            if (vlInfo != null)
                            {
                                ads.CapitalAmount = vlInfo.ExistingLoan.Value;
                            }

                            ba.ChangeDate = DateTime.Today;
                            ba.UserID = spc.GetADUser(_view.CurrentPrincipal).ADUserName;
                            ads.BankAccount = ba;

                            //todo: set this reference a globals entry (remove hardcoded key);
                            int disbursementType = (int)SAHL.Common.Globals.DisbursementTypes.Registrationdisbursement;
                            ads.DisbursementType = LR.DisbursementTypes.ObjectDictionary[disbursementType.ToString()];
                            //todo : check if it is ok to set the following values
                            ads.InterestStartDate = DateTime.Today;
                            ads.SettlementDate = DateTime.Today;

                            AR.SaveApplicationDebtSettlement(ads);                            
                            ae.ApplicationDebtSettlements.Add(_view.Messages, ads);
                          
                            AR.SaveApplication(application);

                        }
                        else // need to add
                        {
                            
                            IApplicationExpense ae2 = AR.GetEmptyApplicationExpense();
                            IApplicationInformationVariableLoan vlInfo = null;
                            if (application.ApplicationInformations.ObjectDictionary != null)
                            {
                                if (application.GetLatestApplicationInformation() != null)
                                {
                                    vlInfo = AR.GetApplicationInformationVariableLoan(application.GetLatestApplicationInformation().Key);
                                }
                            }
                          
                            ae2.Application = application;

                            int expenseTypeKey2 = (int)SAHL.Common.Globals.ExpenseTypes.Existingmortgageamount;
                            ae2.ExpenseType = LR.ExpenseTypes.ObjectDictionary[expenseTypeKey2.ToString()];

                            if (vlInfo != null)
                            {
                                ae2.TotalOutstandingAmount = vlInfo.ExistingLoan.Value;
                            }

                            application.ApplicationExpenses.Add(_view.Messages, ae2);

                            IApplicationDebtSettlement ads = AR.GetEmptyApplicationDebtSettlement();


                            if (vlInfo != null)
                            {
                                ads.CapitalAmount = vlInfo.ExistingLoan.Value;
                            }
                            ba.ChangeDate = DateTime.Today;
                            ba.UserID = spc.GetADUser(_view.CurrentPrincipal).ADUserName;
                            ads.BankAccount = ba;

                            //todo: set this reference a globals entry (remove hardcoded key);
                            int disbursementType = (int)SAHL.Common.Globals.DisbursementTypes.Registrationdisbursement;
                            ads.DisbursementType = LR.DisbursementTypes.ObjectDictionary[disbursementType.ToString()]; 
                            //todo : check if it is ok to set the following values
                            ads.InterestStartDate = DateTime.Today;
                            ads.SettlementDate = DateTime.Today;

                            AR.SaveApplicationDebtSettlement(ads);
                            ae2.ApplicationDebtSettlements.Add(_view.Messages, ads);
                            AR.SaveApplication(application);
                        }
                    }
                    ts.VoteCommit();
                    _view.Navigator.Navigate("SettlementBankingDetailsDisplay");            
                }                           
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
        }


        /// <summary>
        /// Handles the view's OnSearchGridSelectedIndexChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        void _view_OnSearchGridSelectedIndexChanged(object sender, object key)
        {
          
        }
    }
}

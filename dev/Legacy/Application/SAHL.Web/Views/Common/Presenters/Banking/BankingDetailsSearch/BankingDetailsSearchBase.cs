using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;

namespace SAHL.Web.Views.Common.Presenters.Banking
{
    /// <summary>
    /// 
    /// </summary>
    public class BankingDetailsSearchBase : SAHLCommonBasePresenter<IBankingDetailsSearch>
    {
        const string foundBankAccounts = "FOUNDBANKACCOUNTS";      
        //const string callingView = "CALLINGVIEW";
        //const string nextView = "NEXTVIEW";
        //const string legalEntity = "LEGALENTITY";
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BankingDetailsSearchBase(IBankingDetailsSearch view, SAHLCommonBaseController controller)
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



            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
            //view.OnSearchGridSelectedIndexChanged += _view_OnSearchGridSelectedIndexChanged;

            
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

        
    }
}

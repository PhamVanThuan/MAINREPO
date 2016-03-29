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
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using System.Diagnostics.CodeAnalysis;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.Banking
{
    public class BankingDetailsDelete : BankingDetailsBase
    {
        public BankingDetailsDelete(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BankAccountGridEnabled = true;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelClicked += new EventHandler(_view_CancelClicked);
            _view.OnSearchBankAccountClicked += new EventHandler(_view_SearchBankAccountClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_SubmitButtonClicked);        
           
            _view.SetControlsToFirstAccount = false;
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.ControlsVisible = false;
            _view.ShowButtons = true;
            _view.ShowStatus = false;
            _view.SubmitButtonText = "Delete"; 
            if (base.BankAccounts.Count > 0)
            {
                _view.SubmitButtonEnabled = true;
                             
            }
            else
             {
                _view.SubmitButtonEnabled = false;   
                
            };



           
            _view.SearchButtonVisible = false;                     
        }

        /// <summary>
        /// Hooks the View's UpdateBankAccountClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                int bankAccountKey = _view.SelectedLegalEntityBankAccountKey;

                ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                LER.DeleteLegalEntityBankAccount(bankAccountKey, _view.CurrentPrincipal);
                ts.VoteCommit();
                _view.Navigator.Navigate("Submit");
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


        /// <summary>
        /// Hooks the View's SearchBankAccountClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_SearchBankAccountClicked(object sender, EventArgs e)
        {
            // string bankName = _view.BankName;
            string branchCode = _view.BranchCode;
            string accountName = _view.AccountName;
            string accountType = _view.AccountType;
            string accountNumber = _view.AccountNumber;


            IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();

            IBankAccountSearchCriteria criteria = new BankAccountSearchCriteria();
            criteria.ACBBranchKey = branchCode;
            criteria.ACBTypeKey = int.Parse(accountType);
            criteria.AccountName = accountName;
            criteria.AccountNumber = accountNumber;

            IEventList<ILegalEntityBankAccount> lstFoundAcccounts = BAR.SearchLegalEntityBankAccounts(criteria, 100);

            //TODO: Add Real Life Time Object
            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();

            GlobalCacheData.Clear();
            if (GlobalCacheData.ContainsKey(foundBankAccounts))
                GlobalCacheData[foundBankAccounts] = lstFoundAcccounts;
            else
            {
                GlobalCacheData.Add(foundBankAccounts, lstFoundAcccounts, LifeTimes);
            }

            if (GlobalCacheData.ContainsKey(callingView))
                GlobalCacheData[callingView] = "BankingDetails";
            else
            {
                GlobalCacheData.Add(callingView, "BankingDetails", LifeTimes);
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                GlobalCacheData[ViewConstants.LegalEntity] = _legalEntity;
            else
            {
                GlobalCacheData.Add(ViewConstants.LegalEntity, _legalEntity, LifeTimes);
            }


            //navigate to BankingDetailsSearch
            _view.Navigator.Navigate("Search");

        }

        /// <summary>
        ///  Hooks the View's DeleteBankAccountClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="IBankAccount"></param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void _view_DeleteBankAccountClicked(object sender, object IBankAccount)
        {
            //IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
            //todo Call delete bankaccount delete method
        }

        /// <summary>
        /// Hooks the View's CancelClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_CancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }        
    }
}


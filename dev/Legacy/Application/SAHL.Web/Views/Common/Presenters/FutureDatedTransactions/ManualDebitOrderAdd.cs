using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Presenters.Banking;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using SAHL.Common.UI;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.ManualDebitOrder
{

    /// <summary>
    /// 
    /// </summary>
    public class ManualDebitOrderAdd : ManualDebitOrderBase
    {

        IFinancialService _financialService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public ManualDebitOrderAdd(IManualDebitOrder View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
            _node = CBOService.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSet.CBONODESET) as CBOMenuNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);

            // IAccountRepository AR = RepositoryFactory.GetRepository<IAccountRepository>();
            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            _financialService = FSR.GetFinancialServiceByKey(10052374);
            IAccount account = _financialService.Account;// AR.GetAccountByFinancialServiceKey(_10052374);
            //IFinancialService fs = account.GetFinancialServiceByType(SAHL.Common.Globals.FinancialServiceTypes.VariableLoan);
            //IReadOnlyEventList<IFinancialService> fsList = account.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.VariableLoan, new SAHL.Common.Globals.AccountStatuses[] { SAHL.Common.Globals.AccountStatuses.Open });
            //IFinancialService fs = fsList[0];
            IEventList<IFinancialServiceRecurringTransaction> recurringTransactions = _financialService.FinancialServiceRecurringTransactions;
            if (recurringTransactions.Count > 0)
            {
                _view.GridPostbackType = GridPostBackType.SingleClick;
                _view.BindOrdersToGrid(recurringTransactions);

                EventList<ILegalEntityBankAccount> bankAccounts = new EventList<ILegalEntityBankAccount>();


                int[] roleTypes = new int[3] { 1, 2, 3 };

                IReadOnlyEventList<ILegalEntity> lstLegalEntities = account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);


                for (int x = 0; x < lstLegalEntities.Count; x++)
                {
                    for (int y = 0; y < lstLegalEntities[x].LegalEntityBankAccounts.Count; y++)
                    {
                        bankAccounts.Add(_view.Messages, lstLegalEntities[x].LegalEntityBankAccounts[y]);
                    }
                }
                _view.LegalEntityBankAccounts = bankAccounts;
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
                                                            
            _view.SubmitButtonText = "Add";
            _view.SubmitButtonAccessKey = "A";            
            _view.ShowButtons = true;
            _view.ArrearBalanceRowVisible = false;
            _view.ShowLabels = false;
            _view.ControlsVisible = true;
           
        }

        void _view_OnGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {

        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            //todo: set start date for object - what to use as start date?
            TransactionScope ts = new TransactionScope();
            try
            {
                IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                IFinancialServiceRecurringTransaction recurringTransaction = FSR.GetEmptyFinancialServiceRecurringTransaction();               
                recurringTransaction.FinancialService = _financialService;
                recurringTransaction.Amount = _view.Amount;
                recurringTransaction.InsertDate = DateTime.Today;
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                IBankAccount ba = BAR.GetBankAccountByKey(_view.SelectedBankAccountKey);
                recurringTransaction.BankAccount = ba;
                recurringTransaction.Notes = _view.Note;
                recurringTransaction.Reference = _view.Reference;
                recurringTransaction.UserName = _view.CurrentPrincipal.ADUser.ADUserName;
                ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                for(int x=0;x<LR.TransactionTypes.Count;x++)
                {
                    if(LR.TransactionTypes[x].Key == LR.TransactionTypes[x].Key)
                    {
                        recurringTransaction.TransactionType = LR.TransactionTypes[x];
                        break;
                    }
                }
                FSR.SaveFinancialServiceRecurringTransaction(recurringTransaction);               
                ts.VoteCommit();
                //todo: setup view to navigate to.
                //_view.Navigator.Navigate("");
            }
            catch (DomainValidationException)
            {
                //bubble error messages to view               
                ts.VoteRollBack();
            }
            catch (Exception)
            {
                ts.VoteRollBack();
            }
            finally
            {
                ts.Dispose();
            }
            
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ManualDebitOrder");
        }

    }
}

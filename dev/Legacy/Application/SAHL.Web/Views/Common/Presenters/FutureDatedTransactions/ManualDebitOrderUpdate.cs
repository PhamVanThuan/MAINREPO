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
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.ManualDebitOrder
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualDebitOrderUpdate : ManualDebitOrderBase
    {
        IEventList<IFinancialServiceRecurringTransaction> _recurringTransactions; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public ManualDebitOrderUpdate(IManualDebitOrder View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
            {
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

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);

            // IAccountRepository AR = RepositoryFactory.GetRepository<IAccountRepository>();
            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

            IFinancialService fs = FSR.GetFinancialServiceByKey(10052374);

            IAccount account = fs.Account;// AR.GetAccountByFinancialServiceKey(_10052374);

            //IReadOnlyEventList<IFinancialService> fsList = account.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.VariableLoan, new SAHL.Common.Globals.AccountStatuses[] { SAHL.Common.Globals.AccountStatuses.Open });
            //IFinancialService fs = fsList[0];
            _recurringTransactions= fs.FinancialServiceRecurringTransactions;
            if (_recurringTransactions.Count > 0)
            {
                _view.SubmitButtonText = "Update";
                _view.SubmitButtonAccessKey = "U";
                _view.GridPostbackType = GridPostBackType.SingleClick;
                _view.BindOrdersToGrid(_recurringTransactions);

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
                _view.ShowLabels = false;
                _view.SetControlsToFirstGridItem = true;
                
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

            
            _view.ShowButtons = true;
            _view.ArrearBalanceRowVisible = false;
            _view.ControlsVisible = true;         
        }

        void _view_OnGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {

        }

        void _view_OnSubmitButtonClicked(object sender, KeyChangedEventArgs e)
        {
            int transactionKey = int.Parse(e.Key.ToString());
            TransactionScope ts = new TransactionScope();
            try
            {
                IFinancialServiceRecurringTransaction recurringTransaction = null;
                IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                for (int x = 0; x < _recurringTransactions.Count; x++)
                {
                    if (_recurringTransactions[x].Key == transactionKey)
                    {
                        recurringTransaction = _recurringTransactions[x];
                        break;
                    }
                }
                if (recurringTransaction != null)
                {
                    recurringTransaction.Amount = _view.Amount;
                    recurringTransaction.InsertDate = DateTime.Today;
                    IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    IBankAccount ba = BAR.GetBankAccountByKey(_view.SelectedBankAccountKey);
                    recurringTransaction.BankAccount = ba;
                    recurringTransaction.Notes = _view.Note;
                    recurringTransaction.Reference = _view.Reference;
                    recurringTransaction.UserName = _view.CurrentPrincipal.ADUser.ADUserName;
                    FSR.SaveFinancialServiceRecurringTransaction(recurringTransaction);
                  
                    ts.VoteCommit();
                }
                else
                {
                    //todo: handle no recurring transaction to update error;
                }
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

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
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.ManualDebitOrder
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualDebitOrderDelete : ManualDebitOrderBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public ManualDebitOrderDelete(IManualDebitOrder View, SAHLCommonBaseController Controller)
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

            base.OnViewInitialised(sender, e);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);

            //IAccountRepository AR = RepositoryFactory.GetRepository<IAccountRepository>();
            //IAccount account = AR.GetAccountByFinancialServiceKey(_10052374);

            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFinancialService fs = FSR.GetFinancialServiceByKey(10052374);// account.GetFinancialServiceByType(SAHL.Common.Globals.FinancialServiceTypes.VariableLoan);

            IEventList<IFinancialServiceRecurringTransaction> recurringTransactions = fs.FinancialServiceRecurringTransactions;
            if (recurringTransactions.Count > 0)
            {
                _view.GridPostbackType = GridPostBackType.SingleClick;
                _view.BindOrdersToGrid(recurringTransactions);

                EventList<ILegalEntityBankAccount> bankAccounts = new EventList<ILegalEntityBankAccount>();


                int[] roleTypes = new int[3] { 1, 2, 3 };

                IReadOnlyEventList<ILegalEntity> lstLegalEntities = fs.Account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);


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

            _view.SubmitButtonText = "Delete";
            _view.SubmitButtonAccessKey = "D";

            _view.ShowButtons = true;
            _view.ControlsVisible = false;
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
                IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                FSR.DeleteFinancialServiceRecurringTransaction(transactionKey);                   
                ts.VoteCommit();
                //todo : navigate
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

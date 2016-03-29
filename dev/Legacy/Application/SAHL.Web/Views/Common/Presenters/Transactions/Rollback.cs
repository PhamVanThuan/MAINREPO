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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common.Presenters.Transactions
{
    public class Rollback : Base
    {
        private bool isArrear;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Rollback(ITransaction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnRollbackButtonClicked += new EventHandler(_view_OnRollbackButtonClicked);
            _view.OnTransactionTypeSelectedIndexChanged += new EventHandler(_view_OnTransactionTypeSelectedIndexChanged);
            
            _view.ButtonCancel = ButtonStatus.Display.Hidden;
            _view.ButtonRollbackConfirm = ButtonStatus.Display.Hidden;

            _view.GridViewPostBackType = GridPostBackType.NoneWithClientSelect;

            _view.ShowRollbackTransactions = false;
            _view.ShowTransactions = true;
        }

        protected void _view_OnTransactionTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            _view.TransactionDescriptionSelectedValue = "All";

            //get the list of displayed transaction types
            GetTransactionsAndTypes(isArrear);
            _view.BindTransactions(_transactions, isArrear);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            //IAccount acc = AccRepo.GetAccountByKey(_accountKey);

            _view.FinancialTransactionsOnly = true;
            // list each fs in the dropdown
            GetTransactionsAndTypes(isArrear);
            _view.BindTransactions(_transactions, isArrear);
        }

        /// <summary>
        /// OnView Loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);

            if (!_view.ShouldRunPage)
                return;

            //get the list of displayed transaction types
            GetTransactionsAndTypes(isArrear);
            _view.BindTransactions(_transactions, isArrear);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnRollbackButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey("RollbackTransactionNumber"))
            {
                GlobalCacheData.Remove("RollbackTransactionNumber");
            }
            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add("RollbackTransactionNumber", _view.RollbackTransactionNumber, LifeTimes);

            Navigator.Navigate("TransactionRollbackConfirm");
        }

    }
}

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

namespace SAHL.Web.Views.Common.Presenters.Transactions
{
    public class ViewArrearApplication : Base
    {
        private bool isArrear = true;
        private int _applicationkey;
        protected IAccount _acc;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ViewArrearApplication(ITransaction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnTransactionTypeSelectedIndexChanged += new EventHandler(_view_OnTransactionTypeSelectedIndexChanged);

            _view.ButtonCancel = ButtonStatus.Display.Hidden;
            _view.ButtonRollback = ButtonStatus.Display.Hidden;
            _view.ButtonRollbackConfirm = ButtonStatus.Display.Hidden;

            _view.ShowRollbackTransactions = false;
            _view.ShowTransactions = true;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _applicationkey = Convert.ToInt32(_cboNode.GenericKey);
            _acc = AccRepo.GetAccountByApplicationKey(_applicationkey);

            if (_acc != null)
            {
                _accountKey = _acc.Key;

                GetTransactionsAndTypes(isArrear);
                _view.BindTransactions(_transactions, isArrear);
            }
            else
                _view.NoTransactions = true;
        }

        /// <summary>
        /// OnView Loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);

            if (!_view.ShouldRunPage) return;

            if (_acc != null)
            {
                //get the list of displayed transaction types
                GetTransactionsAndTypes(isArrear);
                _view.BindTransactions(_transactions, isArrear);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnTransactionTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            _view.TransactionDescriptionSelectedValue = "All";

            //get the list of displayed transaction types
            GetTransactionsAndTypes(isArrear);
            _view.BindTransactions(_transactions, isArrear);
        }
    }
}

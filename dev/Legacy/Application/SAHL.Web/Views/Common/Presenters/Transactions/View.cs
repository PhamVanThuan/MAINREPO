using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.Transactions
{
    public class View : Base
    {
        private bool isArrear;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public View(ITransaction view, SAHLCommonBaseController controller)
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

            _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            //IAccount acc = AccRepo.GetAccountByKey(_accountKey);

            //get the list of displayed transaction types
            //GetTransactionsAndTypes(false, true, isArrear);
            //_view.BindTransactionDescriptionDropDown(_transactionTypeDescriptions);

            //Default Transaction Type to Financial
            if (!View.IsPostBack)
                _view.TransactionDisplayTypeSelectedValue = (int)TransactionDisplayType.Financial;

            GetTransactionsAndTypes(isArrear);
            _view.BindTransactions(_transactions, isArrear);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);

            if (!_view.ShouldRunPage)
                return;
            //get the list of displayed transaction types
            //GetTransactionsAndTypes(isArrear);
            //_view.BindTransactions(_transactions, isArrear);
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

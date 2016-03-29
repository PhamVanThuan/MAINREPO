using System;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Transactions
{
    public class RollbackArrearConfirm : Base 
    {
        private bool isArrear = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RollbackArrearConfirm(ITransaction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnRollbackConfirmButtonClicked += new EventHandler(_view_OnRollbackConfirmButtonClicked);

            _view.ButtonRollback = ButtonStatus.Display.Hidden;
            _view.ShowRollbackTransactions = true;
            _view.ShowTransactions = false;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            //get the list of displayed transaction types
            GetRollbackTransactions(isArrear);
            _view.BindRollbackTransactions(_transactions, isArrear);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("TransactionArrearRollback");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnRollbackConfirmButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                LTRepo.RollbackTransaction(_accountKey, (int)GlobalCacheData["RollbackTransactionNumber"], _view.CurrentPrincipal.Identity.Name, isArrear);

                if (_view.IsValid)
                {
                    if (GlobalCacheData.ContainsKey("RollbackTransactionNumber"))
                        GlobalCacheData.Remove("RollbackTransactionNumber");
                }

                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();

                if (_view.IsValid)
                    Navigator.Navigate("TransactionArrearRollback");
            }
        }
    }
}

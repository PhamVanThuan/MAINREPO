using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters.Banking;

namespace SAHL.Web.Views.Common.Presenters.FutureDatedTransactions
{
    /// <summary>
    ///
    /// </summary>
    public class FutureDatedTransactionsAdd : FutureDatedTransactionsBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FutureDatedTransactionsAdd(IFutureDatedTransactions view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.GridPostbackType = GridPostBackType.None;
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnAddButtonClicked += new EventHandler(_view_OnAddButtonClicked);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.ButtonAddVisible = true;
            _view.ShowButtons = true;
            _view.ArrearBalanceRowVisible = true;
            _view.ArrearBalance = AccountArrearsBalance;
            _view.ShowLabels = false;
            _view.ControlsVisible = true;
            _view.AccountKey = base.FinancialService.Account.Key.ToString();

            // default the reference to the account number when adding
            if (!_view.IsPostBack)
                _view.Reference = _view.AccountKey;
        }

        private void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            IManualDebitOrder manualDebitOrder = ManualDebitOrderRepository.GetEmptyManualDebitOrder();
            if (base.SaveRecurringTransaction(manualDebitOrder))
            {
                _view.Navigator.Navigate("ManualDebitOrderDisplay");
                _view.ShouldRunPage = false;
            }
        }

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ManualDebitOrderDisplay");
        }
    }
}
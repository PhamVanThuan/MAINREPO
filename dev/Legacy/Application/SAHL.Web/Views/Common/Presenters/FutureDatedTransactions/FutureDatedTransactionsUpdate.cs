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
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
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
    public class FutureDatedTransactionsUpdate : FutureDatedTransactionsBase
    {
        private SAHLPrincipalCache spc;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FutureDatedTransactionsUpdate(IFutureDatedTransactions view, SAHLCommonBaseController controller)
            : base(view, controller)
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

            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnUpdateButtonClicked += new KeyChangedEventHandler(_view_OnUpdateButtonClicked);
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

            if (_view.IsPostBack) //need to reload fs to avoid lazy load errors
                base.FinancialService = null;

            int recordCount = base.FinancialService.ManualDebitOrders.Count;

            _view.ArrearBalanceRowVisible = false;

            _view.ShowButtons = (recordCount > 0);
            _view.ControlsVisible = _view.ShowButtons;
            _view.ShowLabels = false;
            _view.ArrearBalanceRowVisible = true;
            _view.ArrearBalance = this.AccountArrearsBalance;
            _view.ButtonUpdateVisible = (recordCount > 0);
            _view.AccountKey = base.FinancialService.Account.Key.ToString();
        }

        private void _view_OnUpdateButtonClicked(object sender, KeyChangedEventArgs e)
        {
            int transactionKey = int.Parse(e.Key.ToString());
            IManualDebitOrder rt = ManualDebitOrderRepository.GetManualDebitOrderByKey(transactionKey);

            //If the Current user is in the Debt Counselling User group then exclude the 6 month rule
            spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svcRule = ServiceFactory.GetService<IRuleService>();

            int result = svcRule.ExecuteRule(spc.DomainMessages, "DebtCounsellingDeleteDebitOrder", rt, _view.CurrentPrincipal);
            if (result == 1)
            {
                int debtconselling = svcRule.ExecuteRule(spc.DomainMessages, "IsDebtCounsellingUser", _view.CurrentPrincipal);

                if (debtconselling == 1)
                {
                    this.ExclusionSets.Add(RuleExclusionSets.FinancialServiceRecurringTransaction);
                }

                if (base.SaveRecurringTransaction(rt))
                {
                    _view.Navigator.Navigate("ManualDebitOrderDisplay");
                    _view.ShouldRunPage = false;
                }
                this.ExclusionSets.Remove(RuleExclusionSets.FinancialServiceRecurringTransaction);
            }
        }

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ManualDebitOrderDisplay");
        }
    }
}
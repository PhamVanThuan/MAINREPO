using System;
using System.Collections.Generic;
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
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.FutureDatedTransactions
{
    /// <summary>
    ///
    /// </summary>
    public class FutureDatedTransactionsDelete : FutureDatedTransactionsBase
    {
        private int canDelete;
        private SAHLPrincipalCache spc;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FutureDatedTransactionsDelete(IFutureDatedTransactions view, SAHLCommonBaseController controller)
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
            _view.OnDeleteButtonClicked += new KeyChangedEventHandler(_view_OnDeleteButtonClicked);
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

            _view.ButtonDeleteVisible = true;
            _view.ShowButtons = true;
            _view.ControlsVisible = false;

            _view.DeleteButtonText = "Delete";
            _view.DeleteButtonOnClientClick = "return confirm('Are you sure you want to delete the selected item?')";
        }

        private void _view_OnDeleteButtonClicked(object sender, KeyChangedEventArgs e)
        {
            spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            spc.IgnoreWarnings = false;

            int transactionKey = int.Parse(e.Key.ToString());
            if (transactionKey != 0)
            {
                IManualDebitOrder manDebitOrder = ManualDebitOrderRepository.GetManualDebitOrderByKey(transactionKey);
                if (manDebitOrder != null & manDebitOrder.TransactionType.Key == (short)TransactionTypes.ManualDebitOrderPayment)
                {
                    IRuleService svcRule = ServiceFactory.GetService<IRuleService>();

                    // Ensure that the Debit Order can not be deleted by a non Debt Couselling User
                    canDelete = svcRule.ExecuteRule(_view.Messages, "DebtCounsellingDeleteDebitOrder", manDebitOrder, _view.CurrentPrincipal);
                    this.ExclusionSets.Add(RuleExclusionSets.FinancialServiceRecurringTransaction);
                    if (canDelete == 1)
                    {
                        TransactionScope ts = new TransactionScope();
                        try
                        {
                            ManualDebitOrderRepository.CancelManualDebitOrder(manDebitOrder);
                            ts.VoteCommit();
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
                        if (_view.Messages.Count == 0)
                        {
                            _view.Navigator.Navigate("ManualDebitOrderDisplay");
                        }
                    }
                }
                else
                {
                    spc.DomainMessages.Add(new Error("Unable to retrieve Manual Debit Order", ""));
                    _view.Navigator.Navigate("ManualDebitOrderDisplay");
                }
            }
            else
            {
                spc.DomainMessages.Add(new Error("Unable to retrieve Manual Debit Order", ""));
                _view.Navigator.Navigate("ManualDebitOrderDisplay");
            }
        }

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ManualDebitOrderDisplay");
        }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    public class QuickCashThirdPartyPayments : QuickCashDetailsBase
    {
        int selectedGridIndex;
        IEventList<IApplicationExpense> ApplicationThirdPartyExpenses;

        public QuickCashThirdPartyPayments(IQuickCashDetails View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            selectedGridIndex = 0;
            _view.ShowApprovedPanel = true;
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.ShowThirdPartyPaymentsGrid = true;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(true);
            _view.ShowButtons = true;
            _view.SetSubmitButtonText = "Next";

            _view.onGridQuickCashPaymentSelectedIndexChanged += new KeyChangedEventHandler(_view_onGridQuickCashPaymentSelectedIndexChanged);
            _view.OnSubmitButtonClicked+=new EventHandler(_view_OnSubmitButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            FilterApplicationExpensesByPaymentType(quickCashApplicationInformation.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex].ApplicationExpenses);
            _view.BindThirdPartyPaymentsGrid(ApplicationThirdPartyExpenses);
        }

        private void FilterApplicationExpensesByPaymentType(IEventList<IApplicationExpense> appExpenses)
        {
            ApplicationThirdPartyExpenses = new EventList<IApplicationExpense>();
            for (int i = 0; i < appExpenses.Count; i++)
            {
                if (appExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                    ApplicationThirdPartyExpenses.Add(_view.Messages, appExpenses[i]);
            }
        }

        protected void _view_onGridQuickCashPaymentSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            selectedGridIndex = Convert.ToInt32(e.Key);
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.QuickCashDetailKey);

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add(ViewConstants.QuickCashDetailKey, _view.GetSetSelectedGridItem, LifeTimes);

            _view.Navigator.Navigate("QuickCashThirdPartyPaymentsAdd");
        }
    }
}

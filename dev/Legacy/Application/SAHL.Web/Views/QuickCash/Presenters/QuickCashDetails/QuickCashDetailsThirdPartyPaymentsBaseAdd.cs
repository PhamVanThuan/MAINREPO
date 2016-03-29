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
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    public class QuickCashDetailsThirdPartyPaymentsBaseAdd : QuickCashDetailsBase
    {
        int selectedGridIndex;
        IEventList<IApplicationExpense> ApplicationThirdPartyExpenses;

        public QuickCashDetailsThirdPartyPaymentsBaseAdd(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
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
            _view.SetThirdParyPaymentsGridHeaderText = "Third Party Payments";

            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
            _view.onGridQuickCashPaymentSelectedIndexChanged += new KeyChangedEventHandler(_view_onGridQuickCashPaymentSelectedIndexChanged);
            _view.OnSubmitButtonClicked+=new EventHandler(_view_OnSubmitButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            FilterApplicationExpensesByPaymentType(appInfoQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex].ApplicationExpenses);

            if (appInfoQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.RegularPayment)
                _view.BindThirdPartyPaymentsGrid(ApplicationThirdPartyExpenses, false);
            else
                _view.ShowUpdateButton = false;
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
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

            if (_view.Messages.ErrorMessages.Count == 0)
                _view.Navigator.Navigate("QuickCashThirdPartyPaymentsExtendedAdd");
        }
    }
}

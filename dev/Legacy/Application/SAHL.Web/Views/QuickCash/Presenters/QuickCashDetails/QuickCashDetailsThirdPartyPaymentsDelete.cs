using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using System;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    public class QuickCashDetailsThirdPartyPaymentsDelete : QuickCashDetailsBase
    {
        int selectedGridIndex;
        IEventList<IApplicationExpense> ApplicationThirdPartyExpenses;
        int selectedThirdPartyGridIndex;

        public QuickCashDetailsThirdPartyPaymentsDelete(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            selectedGridIndex = 0;
            selectedThirdPartyGridIndex = 0;

            _view.ShowApprovedPanel = true;
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.ShowThirdPartyPaymentsGrid = true;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(true);
            _view.ShowButtons = true;
            _view.SetSubmitButtonText = "Delete";
            _view.SetThirdParyPaymentsGridHeaderText = "Third Party Payments";

            GetSelectedIndexFromCache();

            FilterApplicationExpensesByPaymentType(appInfoQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex].ApplicationExpenses);
            _view.BindThirdPartyPaymentsGrid(ApplicationThirdPartyExpenses, true);

            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
            _view.onGridQuickCashPaymentSelectedIndexChanged += new KeyChangedEventHandler(_view_onGridQuickCashPaymentSelectedIndexChanged);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.ongridThirdPartyPaymentSelectedIndexChanged+=new KeyChangedEventHandler(_view_ongridThirdPartyPaymentSelectedIndexChanged);
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
        }

        private void GetSelectedIndexFromCache()
        {
            if (PrivateCacheData.ContainsKey("SelectedPaymentsGridIndex"))
                selectedGridIndex = Convert.ToInt32(PrivateCacheData["SelectedPaymentsGridIndex"]);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            GetSelectedIndexFromCache();

            FilterApplicationExpensesByPaymentType(appInfoQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex].ApplicationExpenses);
           
            if (ApplicationThirdPartyExpenses != null && ApplicationThirdPartyExpenses.Count > 0)
                 _view.BindThirdPartyPaymentsGrid(ApplicationThirdPartyExpenses,true);
            else
                 _view.ShowUpdateButton = false;
        }

      
        private void FilterApplicationExpensesByPaymentType(IEventList<IApplicationExpense> appExpenses)
        {
            ApplicationThirdPartyExpenses = new EventList<IApplicationExpense>();
            for (int i = 0; i < appExpenses.Count; i++)
            {
                if (appExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                    ApplicationThirdPartyExpenses.Add(_view.Messages, appExpenses[i]);
            }

            PrivateCacheData.Remove("ApplicationExpenses");
            PrivateCacheData.Add("ApplicationExpenses", ApplicationThirdPartyExpenses);
        }

        protected void _view_onGridQuickCashPaymentSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            selectedGridIndex = Convert.ToInt32(e.Key);

            PrivateCacheData.Remove("SelectedPaymentsGridIndex");
            PrivateCacheData.Add("SelectedPaymentsGridIndex", selectedGridIndex);
        }

        protected void _view_ongridThirdPartyPaymentSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            selectedThirdPartyGridIndex = Convert.ToInt32(e.Key);

            PrivateCacheData.Remove("selectedThirdPartyGridIndex");
            PrivateCacheData.Add("selectedThirdPartyGridIndex", selectedThirdPartyGridIndex);
        }
        
        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            GetSelectedIndexFromCache();

            if (PrivateCacheData.ContainsKey("selectedThirdPartyGridIndex"))
                selectedThirdPartyGridIndex = Convert.ToInt32(PrivateCacheData["selectedThirdPartyGridIndex"]);

            IEventList<IApplicationExpense> appExpenses = null;

            if (PrivateCacheData.ContainsKey("ApplicationExpenses"))
                appExpenses = PrivateCacheData["ApplicationExpenses"] as IEventList<IApplicationExpense>;

            IApplicationInformationQuickCashDetail appInformationQCDetail = appInfoQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex];

            IApplicationExpense applicationExpense = appExpenses[selectedThirdPartyGridIndex];
            
            TransactionScope txn = new TransactionScope();

            try
            {
                appInformationQCDetail.ApplicationExpenses.Remove(_view.Messages,applicationExpense);
                qcRepo.SaveApplicationInformationQuickCashDetail(appInformationQCDetail);
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
            }

        }
    }
}

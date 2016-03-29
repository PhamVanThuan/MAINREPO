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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.QuickCash.Presenters
{
    public class QuickCashThirdPartyPaymentsUpdate : SAHLCommonBasePresenter<IQuickCashThirdPartyPayments>
    {
        IQuickCashRepository qcRepo;
        IApplicationInformationQuickCashDetail quickCashDetail;
        IEventList<IApplicationExpense> ApplicationThirdPartyExpenses;
        ILookupRepository lookups;
        int selectedGridIndex;
        double amtToClient;

        public QuickCashThirdPartyPaymentsUpdate(IQuickCashThirdPartyPayments view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            int offerInfoQuickCashDetailKey = 0;
            if (GlobalCacheData.ContainsKey(ViewConstants.QuickCashDetailKey))
                offerInfoQuickCashDetailKey = Convert.ToInt32(GlobalCacheData[ViewConstants.QuickCashDetailKey]);

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            qcRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
            quickCashDetail = qcRepo.GetApplicationInformationQuickCashDetailByKey(offerInfoQuickCashDetailKey);

            selectedGridIndex = 0;

            IEventList<IExpenseType> expenseTypes = FilterExpenseTypeListByPaymentType();
            _view.BindExpenseTypes(expenseTypes);
            FilterApplicationExpensesByPaymentType(quickCashDetail.ApplicationExpenses);
            _view.BindQuickCashDetails(Convert.ToDouble(quickCashDetail.RequestedAmount), amtToClient);
            _view.BindThirdPartyPaymentsGrid(ApplicationThirdPartyExpenses,true);

            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.onGridQuickCashPaymentSelectedIndexChanged+=new KeyChangedEventHandler(_view_onGridQuickCashPaymentSelectedIndexChanged);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            _view.BindThirdPartyPaymentDetails(ApplicationThirdPartyExpenses[selectedGridIndex]);
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
        }

        private IEventList<IApplicationExpense> FilterApplicationExpensesByPaymentType(IEventList<IApplicationExpense> appExpenses)
        {
            ApplicationThirdPartyExpenses = new EventList<IApplicationExpense>();
            for (int i = 0; i < appExpenses.Count; i++)
            {
                if (appExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                    ApplicationThirdPartyExpenses.Add(_view.Messages, appExpenses[i]);
                if (appExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.QuickCashPayment)
                    amtToClient = appExpenses[i].TotalOutstandingAmount;
            }

            return ApplicationThirdPartyExpenses;
        }

        private IEventList<IExpenseType> FilterExpenseTypeListByPaymentType()
        {
            IEventList<IExpenseType> expenseTypes = new EventList<IExpenseType>();

            for (int i = 0; i < lookups.ExpenseTypes.Count; i++)
            {
                if (lookups.ExpenseTypes[i].PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                    expenseTypes.Add(_view.Messages, lookups.ExpenseTypes[i]);
            }
            return expenseTypes;
        }

        protected void _view_onGridQuickCashPaymentSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            selectedGridIndex = Convert.ToInt32(e.Key);
            
            PrivateCacheData.Remove("SelectedGridIndex");
            PrivateCacheData.Add("SelectedGridIndex", selectedGridIndex);
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (PrivateCacheData.ContainsKey("SelectedGridIndex"))
                selectedGridIndex = Convert.ToInt32(PrivateCacheData["SelectedGridIndex"]);

            double previousAmount = ApplicationThirdPartyExpenses[selectedGridIndex].TotalOutstandingAmount;

            ApplicationThirdPartyExpenses[selectedGridIndex] = _view.GetUpdatedApplicationExpense(ApplicationThirdPartyExpenses[selectedGridIndex]);

            double diffBetweenNewAmtAndPrev = previousAmount - ApplicationThirdPartyExpenses[selectedGridIndex].TotalOutstandingAmount;

            for (int i = 0; i < quickCashDetail.ApplicationExpenses.Count; i++)
            {
                if (quickCashDetail.ApplicationExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.QuickCashPayment)
                {
                    quickCashDetail.ApplicationExpenses[i].TotalOutstandingAmount += diffBetweenNewAmtAndPrev;
                    if (quickCashDetail.ApplicationExpenses[i].TotalOutstandingAmount < 0)
                        quickCashDetail.ApplicationExpenses[i].TotalOutstandingAmount = 0;
                    break;
                }
            }

            // Need to call ValidateEntity as these rules will not be called cos we are not changing the Parent object, 
            // just updating one of it's children
            quickCashDetail.ValidateEntity();

            if (!_view.IsValid)
                return;

            TransactionScope txn = new TransactionScope();

            try
            {
                qcRepo.SaveApplicationInformationQuickCashDetail(quickCashDetail);
                txn.VoteCommit();

                _view.Navigator.Navigate(_view.ViewName);

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

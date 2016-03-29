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

namespace SAHL.Web.Views.QuickCash.Presenters
{
    public class QuickCashThirdPartyPaymentsAdd : SAHLCommonBasePresenter<IQuickCashThirdPartyPayments>
    {
        IQuickCashRepository qcRepo;
        IApplicationInformationQuickCashDetail quickCashDetail;
        IEventList<IApplicationExpense> ApplicationThirdPartyExpenses;
        ILookupRepository lookups;
        double amtToClient;

        public QuickCashThirdPartyPaymentsAdd(IQuickCashThirdPartyPayments view, SAHLCommonBaseController controller)
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

            IEventList<IExpenseType> expenseTypes = FilterExpenseTypeListByPaymentType();
            _view.BindExpenseTypes(expenseTypes);
            FilterApplicationExpensesByPaymentType(quickCashDetail.ApplicationExpenses);
            _view.BindQuickCashDetails(Convert.ToDouble(quickCashDetail.RequestedAmount), amtToClient);
            _view.BindThirdPartyPaymentsGrid(ApplicationThirdPartyExpenses,false);

            _view.OnSubmitButtonClicked+=new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
              
        }

        private IEventList<IApplicationExpense> FilterApplicationExpensesByPaymentType(IEventList<IApplicationExpense> appExpenses)
        {
            ApplicationThirdPartyExpenses = new EventList<IApplicationExpense>();
            for (int i = 0; i < appExpenses.Count; i++)
            {
                if (appExpenses[i].ExpenseType != null && appExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                    ApplicationThirdPartyExpenses.Add(_view.Messages, appExpenses[i]);
                if (appExpenses[i].ExpenseType != null && appExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.QuickCashPayment)
                    amtToClient = appExpenses[i].TotalOutstandingAmount;
            }

            return ApplicationThirdPartyExpenses;
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationExpense appExpense = appRepo.GetEmptyApplicationExpense();

            appExpense = _view.GetUpdatedApplicationExpense(appExpense);
            appExpense.Application = quickCashDetail.OfferInformationQuickCash.ApplicationInformation.Application;
            appExpense.MonthlyPayment = 0;
            appExpense.ToBeSettled = true;
            
            appExpense.ValidateEntity();

            quickCashDetail.ApplicationExpenses.Add(_view.Messages,appExpense);

            // Reduce amount to client by the Third Party Payment
            for (int i = 0; i < quickCashDetail.ApplicationExpenses.Count; i++)
            {
                if (quickCashDetail.ApplicationExpenses[i].ExpenseType != null && quickCashDetail.ApplicationExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.QuickCashPayment)
                {
                    quickCashDetail.ApplicationExpenses[i].TotalOutstandingAmount -= appExpense.TotalOutstandingAmount;
                    if (quickCashDetail.ApplicationExpenses[i].TotalOutstandingAmount < 0)
                        quickCashDetail.ApplicationExpenses[i].TotalOutstandingAmount = 0;
                    quickCashDetail.ApplicationExpenses[i].ApplicationDebtSettlements[0].SettlementAmount -= appExpense.TotalOutstandingAmount;
                }
            }
            // We need this to fire prior to saving the expense. Expense has to be saved before the Detail as the domain 
            // throws errors when done the other way around.
            quickCashDetail.ValidateEntity();
            if (!_view.IsValid)
                return;

            TransactionScope txn = new TransactionScope();

            try
            {
                appRepo.SaveApplicationExpense(appExpense);
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
    }
}

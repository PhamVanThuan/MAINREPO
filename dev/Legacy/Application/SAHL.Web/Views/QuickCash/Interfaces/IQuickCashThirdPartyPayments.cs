using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.QuickCash.Interfaces
{
    public interface IQuickCashThirdPartyPayments : IViewBase
    {
        void BindQuickCashDetails(double amtAvailable, double amtToClient);

        void BindExpenseTypes(IEventList<IExpenseType> expenseType);

        void BindThirdPartyPaymentsGrid(IEventList<IApplicationExpense> applicationExpenses , bool autoPostBack);

        IApplicationExpense GetUpdatedApplicationExpense(IApplicationExpense appExpense);

        void BindThirdPartyPaymentDetails(IApplicationExpense appThirdPartyExpense);

        event EventHandler OnSubmitButtonClicked;

        event KeyChangedEventHandler onGridQuickCashPaymentSelectedIndexChanged;

        event EventHandler OnCancelButtonClicked;

    }
}

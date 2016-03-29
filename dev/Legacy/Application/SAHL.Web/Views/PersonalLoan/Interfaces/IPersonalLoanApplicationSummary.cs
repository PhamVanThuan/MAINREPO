using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanApplicationSummary : IViewBase
    {
        event EventHandler OnCancelButtonClicked;

        event EventHandler OnTransitionHistoryClicked;

        bool ShowCancelButton { set; }

        bool ShowHistoryButton { set; }

        void BindApplicationSummary(IApplication application, IApplicationInformationPersonalLoan applicationInformation, IWorkflowRole consultant, IControl monthlyFee);
    }
}
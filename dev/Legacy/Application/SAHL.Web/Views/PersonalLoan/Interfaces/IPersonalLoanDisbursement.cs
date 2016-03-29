using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanDisbursement : IViewBase
    {
        event EventHandler<EventArgs> ConfirmClick;

        event EventHandler<EventArgs> CancelClick;

        void BindApplicationInformation(IApplicationInformationPersonalLoan applicationInformationPersonalLoan);

        void BindBankAccountInformation(IBankAccount bankAccount);
    }
}
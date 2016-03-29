using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Web.UI;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanCalculator : IViewBase
    {
        string SetTextOnCreateApplicationButton
        {
            set;
        }

        double LoanAmount
        {
            get;
        }

        int? Term
        {
            get;
        }

        bool CreditLifePolicy
        {
            get;
            set;
        }

        event EventHandler OnCalculateButtonClicked;

        event EventHandler OnCancelButtonClicked;

        event EventHandler<PersonalLoanOptionSelectedEventArgs> OnCreateApplicationButtonClicked;

        void BindResult(IResult result);

        void BindApplication(IApplicationInformationPersonalLoan applicationInformationPersonalLoan);
    }

    public class PersonalLoanOptionSelectedEventArgs : EventArgs
    {
        public int SelectedOptionKey { get; set; }

        public PersonalLoanOptionSelectedEventArgs(int selectedOptionKey)
        {
            this.SelectedOptionKey = selectedOptionKey;
        }
    }
}
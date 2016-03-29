using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class PersonalLoanApplicationSummary : SAHLCommonBaseView, IPersonalLoanApplicationSummary
    {
        private bool _showCancelButton;
        private bool _showHistoryButton;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            btnCancel.Visible = _showCancelButton;
            btnTransitionHistory.Visible = _showHistoryButton;
        }

        public void BindApplicationSummary(IApplication application, IApplicationInformationPersonalLoan applicationInformation, IWorkflowRole consultant, IControl monthlyFee)
        {
            lblApplicationNumber.Text = application.Key.ToString();
			lblAccountNumber.Text = application.ReservedAccount.Key.ToString();
            lblMonthlyServiceFee.Text = monthlyFee.ControlNumeric.Value.ToString(SAHL.Common.Constants.CurrencyFormat);

            foreach (IApplicationExpense applicationExpense in application.ApplicationExpenses)
            {
                switch (applicationExpense.ExpenseType.Key)
                {
                    case (int)SAHL.Common.Globals.ExpenseTypes.PersonalLoanInitiationFee:
                        lblInitiationFee.Text = applicationExpense.TotalOutstandingAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                        break;
                    default:
                        break;
                }
            }

            if (applicationInformation == null)
                return;

            lblApplicationType.Text = applicationInformation.ApplicationInformation.Product.Description;
            lblLoanAmount.Text = applicationInformation.LoanAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLoanTerm.Text = applicationInformation.Term.ToString();

            lblInterestRate.Text = (applicationInformation.MarketRate.Value + applicationInformation.Margin.Value).ToString(SAHL.Common.Constants.RateFormat);
            lblLinkRate.Text = applicationInformation.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
            lblInstallment.Text = applicationInformation.MonthlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCreditLifePremium.Text = applicationInformation.LifePremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCurrentPersonalLoansConsultant.Text = consultant.LegalEntity.DisplayName;

            //
            lblTotalInstalment.Text = (monthlyFee.ControlNumeric.Value + applicationInformation.MonthlyInstalment + applicationInformation.LifePremium).ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Navigate to the transition history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTransitionHistory_Click(object sender, EventArgs e)
        {
            OnTransitionHistoryClicked(sender, e);
        }

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnTransitionHistoryClicked;


        public bool ShowCancelButton
        {
            set 
            {
                _showCancelButton = value;
            }
        }

        public bool ShowHistoryButton
        {
            set 
            {
                _showHistoryButton = value;
            }
        }
    }
}
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
	public partial class PersonalLoanDisbursement : SAHLCommonBaseView, IPersonalLoanDisbursement
    {
		public event EventHandler<EventArgs> ConfirmClick;
		public event EventHandler<EventArgs> CancelClick;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		public void BindApplicationInformation(IApplicationInformationPersonalLoan applicationInformationPersonalLoan)
		{
			lblDisbursementAmount.Text = applicationInformationPersonalLoan.LoanAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
		}

		public void BindBankAccountInformation(IBankAccount bankAccount)
		{
			lblBank.Text = bankAccount.ACBBranch.ACBBank.ACBBankDescription;
			lblBranch.Text = bankAccount.ACBBranch.ACBBranchDescription;
			lblAccountType.Text = bankAccount.ACBType.ACBTypeDescription;
			lblAccountName.Text = bankAccount.AccountName;
			lblAccountNumber.Text = bankAccount.AccountNumber;
		}

		protected void OnConfirmClick(object sender, EventArgs e)
		{
			if(ConfirmClick != null)
			{
				ConfirmClick(this, new EventArgs());
			}
		}

		protected void OnCancelClick(object sender, EventArgs e)
		{
			if (CancelClick != null)
			{
				CancelClick(this, new EventArgs());
			}
		}
	}
}
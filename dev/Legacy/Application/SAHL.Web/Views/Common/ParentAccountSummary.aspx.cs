using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Globals;
using System.Linq;

namespace SAHL.Web.Views.Common
{
	public partial class ParentAccountSummary : SAHLCommonBaseView, IParentAccountSummary
	{
		private bool _interestOnlyPanelVisible;
		private bool _arearsRowVisible;
		private bool _subsidyPanelVisible;

		private enum SummaryGridColumnPositions
		{
			AccountKey = 0,
			Product = 1,
			AccountStatus = 2,
			OpenDate = 3,
			CloseDate = 4,
			Amount = 5
		}

		private enum FinancialAdjustmentGridColumnPositions
		{
			AccountKey = 0,
			FinancialAdjustmentSource = 1,
			FinancialAdjustmentType = 2,
			FromDate = 3,
			Term = 4,
			Value = 5,
			Status = 6

		}

		#region IProductSummary Members

		public bool InterestOnlyPanelVisible
		{
			get { return _interestOnlyPanelVisible; }
			set { _interestOnlyPanelVisible = value; }
		}

		public bool ArearsRowVisible
		{
			get { return _arearsRowVisible; }
			set { _arearsRowVisible = value; }
		}

		public bool SubsidyPanelVisible
		{
			get { return _subsidyPanelVisible; }
			set { _subsidyPanelVisible = value; }
		}

		public string MaturityOrExpiryDateLabel
		{
			get { return lblMaturityOrExpiryDate.Text; }
			set { lblMaturityOrExpiryDate.Text = value; }
		}

		public void BindSummaryGrid(IList<SAHL.Common.BusinessModel.Interfaces.IAccount> accounts)
		{
			SummaryGrid.AddGridBoundColumn("", "Account Number", Unit.Percentage(15), HorizontalAlign.Left, true);
			SummaryGrid.AddGridBoundColumn("", "Product", Unit.Percentage(30), HorizontalAlign.Left, true);
			SummaryGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Center, true);
			SummaryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Open Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
			SummaryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Close Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
			SummaryGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(15), HorizontalAlign.Right, true);

			SummaryGrid.DataSource = accounts;
			SummaryGrid.DataBind();
		}

		public void BindInstalmentDetails(IAccount account, double subsidyStopOrderAmount)
		{
			// instalment fields
			TotalLoanInstallment.Text = account.InstallmentSummary.TotalLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
			TotalShortTermLoanInstallment.Text = account.InstallmentSummary.TotalShortTermLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
			TotalPremiums.Text = account.InstallmentSummary.TotalPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
			TotalAmountDue.Text = account.InstallmentSummary.TotalAmountDue.ToString(SAHL.Common.Constants.CurrencyFormat);
			FixedDebitOrderAmount.Text = account.FixedPayment.ToString(SAHL.Common.Constants.CurrencyFormat);
			MonthsInArrears.Text = account.InstallmentSummary.MonthsInArrears.ToString("0.00##") + " Months";
			ServiceFee.Text = account.InstallmentSummary.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);

			// subsidy fields
			SubsidyStopOrderAmount.Text = subsidyStopOrderAmount.ToString(SAHL.Common.Constants.CurrencyFormat);

			// interest only fields
			if (_interestOnlyPanelVisible)
			{
				lblAmortisingInstallment.Text = account.InstallmentSummary.AmortisingInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);

				//Get the MortgageLoan
				IMortgageLoan mortgageLoan = (IMortgageLoan)account.FinancialServices[0];

				var edgeAdjustment = mortgageLoan.FinancialAdjustments.FirstOrDefault(x => x.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.Edge &&
										 x.FinancialAdjustmentType.Key == (int)FinancialAdjustmentTypes.InterestOnly &&
										 x.FinancialAdjustmentStatus.Key == Convert.ToInt32(SAHL.Common.Globals.FinancialAdjustmentStatuses.Active));

				var interestOnlyAdjustment = mortgageLoan.FinancialAdjustments.FirstOrDefault(x => x.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.InterestOnly &&
														 x.FinancialAdjustmentType.Key == (int)FinancialAdjustmentTypes.InterestOnly &&
														 x.FinancialAdjustmentStatus.Key == Convert.ToInt32(SAHL.Common.Globals.FinancialAdjustmentStatuses.Active));
				if (edgeAdjustment != null)
				{
					lblMaturityOrExpiryDate.Text = "Expiry Date";
					if (edgeAdjustment.EndDate != null)
						lblMaturityDate.Text = Convert.ToDateTime(edgeAdjustment.EndDate).ToString(SAHL.Common.Constants.DateFormat);
				}
				if (interestOnlyAdjustment != null)
				{
					lblMaturityDate.Text = Convert.ToDateTime(mortgageLoan.InterestOnly.MaturityDate).ToString(SAHL.Common.Constants.DateFormat);
				}
			}
		}

		public void BindFinancialAdjustmentGrid(IList<IFinancialAdjustment> financialAdjustments)
		{

			FinancialAdjustmentGrid.AddGridBoundColumn("", "Account", Unit.Percentage(8), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.AddGridBoundColumn("", "Financial Adjustment", Unit.Percentage(20), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.AddGridBoundColumn("", "Financial Adjustment Type", Unit.Percentage(20), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
			FinancialAdjustmentGrid.AddGridBoundColumn("", "Term", Unit.Percentage(5), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.AddGridBoundColumn("", "Value", Unit.Percentage(10), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.AddGridBoundColumn("", "Status", Unit.Percentage(11), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.DataSource = financialAdjustments;
			FinancialAdjustmentGrid.DataBind();
		}

		/// <summary>
		/// Used to check if an account status is eligible for inclusion when displaying amounts.
		/// </summary>
		/// <param name="accountStatus"></param>
		/// <returns></returns>
		private static bool IsAccountStatusValid(IAccountStatus accountStatus)
		{
			switch ((AccountStatuses)accountStatus.Key)
			{
				case AccountStatuses.Application:
				case AccountStatuses.Open:
					return true;
				default:
					return false;
			}
		}

		#endregion

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			pnlInterestOnly.Visible = _interestOnlyPanelVisible;
			ArrearsRow.Visible = _arearsRowVisible;
			SubsidyPanel.Visible = _subsidyPanelVisible;

		}

		protected void SummaryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.DataItem != null)
			{
				IAccount account = e.Row.DataItem as IAccount;

				if (account != null)
				{
					e.Row.Cells[(int)SummaryGridColumnPositions.AccountKey].Text = account.Key.ToString();
					e.Row.Cells[(int)SummaryGridColumnPositions.AccountStatus].Text = account.AccountStatus.Description;
					e.Row.Cells[(int)SummaryGridColumnPositions.OpenDate].Text = account.OpenDate.HasValue ? account.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
					e.Row.Cells[(int)SummaryGridColumnPositions.CloseDate].Text = account.CloseDate.HasValue ? account.CloseDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";

					if (account is IRegent)
					{
						e.Row.Cells[(int)SummaryGridColumnPositions.Product].Text = SAHL.Common.Constants.RegentDescription;
						e.Row.Cells[(int)SummaryGridColumnPositions.Amount].Text = account.FixedPayment.ToString();
					}
					else
					{
						e.Row.Cells[(int)SummaryGridColumnPositions.Product].Text = account.Product.Description;

						double amount = 0D;
						if (IsAccountStatusValid(account.AccountStatus))
						{
							if (account is IAccountLifePolicy)
							{
								IAccountLifePolicy alp = account as IAccountLifePolicy;
								amount = alp.LifePolicy != null ? alp.LifePolicy.MonthlyPremium : 0;
							}
							else if (account is IAccountHOC)
							{
								IAccountHOC hocAccount = account as IAccountHOC;
								amount = hocAccount != null ? hocAccount.MonthlyPremium : 0;
							}
							else
							{
								foreach (IFinancialService fs in account.FinancialServices)
								{
									if (IsAccountStatusValid(fs.AccountStatus))
									{
										amount += fs.Payment;
									}
								}
							}
						}
						e.Row.Cells[(int)SummaryGridColumnPositions.Amount].Text = amount.ToString();
					}
				}
			}
		}

		protected void FinancialAdjustmentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{

			if (e.Row.DataItem != null)
			{
				// Get the FinancialAdjustment Row
				IFinancialAdjustment financialAdjustment = e.Row.DataItem as IFinancialAdjustment;

				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.AccountKey].Text = financialAdjustment.FinancialService.Account.Key.ToString();
				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentSource].Text = financialAdjustment.FinancialAdjustmentSource.Description;
				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentType].Text = financialAdjustment.FinancialAdjustmentType.Description;
				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FromDate].Text = financialAdjustment.FromDate.HasValue ? financialAdjustment.FromDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Term].Text = financialAdjustment.Term.ToString();
				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Value].Text = financialAdjustment.ToString();
				e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Status].Text = financialAdjustment.FinancialAdjustmentStatus.Description;


                switch (financialAdjustment.FinancialAdjustmentStatus.Key)
                {
                    case (int)FinancialAdjustmentStatuses.Active:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:green;");
                        break;
                    case (int)FinancialAdjustmentStatuses.Inactive :
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:#e66426;");
                        break;
                    case (int)FinancialAdjustmentStatuses.Canceled :
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:red;");
                        break;
                    default:
                        break;
                }
			}
		}
	}
}

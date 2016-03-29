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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Authentication;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Microsoft.SqlServer.Server;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common
{
	public partial class FixedDebitOrderSummary : SAHLCommonBaseView, IFixedDebitOrderSummary
	{
		private ILookupRepository _lookUps;

		private enum SummaryGridColumnPositions
		{
			AccountKey = 0,
			Product = 1,
			AccountStatus = 2,
			OpenDate = 3,
			CloseDate = 4,
			Amount = 5
		}

		/// <summary>
		/// Oninitialise - Load LookUps Repository
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (!ShouldRunPage) return;
			_lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
		}

		/// <summary>
		/// Sets visibility of Buttons
		/// </summary>
		public bool ShowButtons
		{
			set
			{
				ButtonRow.Visible = value;
			}
		}

		/// <summary>
		/// Gets/sets the visibility of the submit button.
		/// </summary>
		public bool SubmitButtonVisible
		{
			get
			{
				return SubmitButton.Visible;
			}
			set
			{
				SubmitButton.Visible = value;
			}
		}

		/// <summary>
		/// Select First Row on Load of Page only in the case of Delete.
		/// In the case of Update, no item must be selected and updateable values must default to that of
		/// Account
		/// </summary>
		public bool selectedFirstRow
		{
			set
			{
				//FutureOrderGrid.SelectFirstRow = value;
				if (!Page.IsPostBack && value == true)
					FutureOrderGrid.SelectedIndex = 0;
				if (!Page.IsPostBack && value == false)
					FutureOrderGrid.SelectedIndex = -1;
			}

		}
		/// <summary>
		/// Sets visibility of Updateable Controls
		/// </summary>
		public bool ShowUpdateableControl
		{
			set
			{
				AddRow.Visible = value;
			}
		}
		/// <summary>
		/// Set GridPostBack - single click in the case of Update and Delete
		/// </summary>
		public void SetGridPostBack()
		{
			FutureOrderGrid.PostBackType = GridPostBackType.SingleClick;
		}
		/// <summary>
		/// Set View for Update
		/// </summary>
		public void SetControlForUpdate()
		{
			SubmitButton.Text = "Update";
			SubmitButton.AccessKey = "U";

		}
		/// <summary>
		/// Set View for Update
		/// </summary>
		public void SetControlForDelete()
		{
			SubmitButton.Text = "Delete";
			SubmitButton.AccessKey = "D";
			SubmitButton.Attributes["onclick"] = "if(!confirm('Are you sure you want to delete the Fixed Debit Order?')) return false";
		}

		/// <summary>
		/// Sets visibility of Interest Only Panel
		/// </summary>
		public bool ShowInterestOnly
		{
			set
			{
				rowInterestOnly.Visible = value;
			}
		}
		/// <summary>
		/// Bind FutureDated Grid
		/// </summary>
		/// <param name="futureDatedChangeLst"></param>
		public void BindFutureDatedDOGrid(IList<IFutureDatedChange> futureDatedChangeLst)
		{
			FutureOrderGrid.Columns.Clear();
			FutureOrderGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false); //FutureDatedChangeKey
			FutureOrderGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false); //FutureDatedChangeDetailKey
			FutureOrderGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(30), HorizontalAlign.Center, true);
			FutureOrderGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Fixed Debit Order Amount", false, Unit.Percentage(30), HorizontalAlign.Right, true);
			FutureOrderGrid.AddGridBoundColumn("", "Changed By", Unit.Percentage(20), HorizontalAlign.Left, true);
			FutureOrderGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Date Changed", false, Unit.Percentage(20), HorizontalAlign.Center, true);

			FutureOrderGrid.DataSource = futureDatedChangeLst;
			FutureOrderGrid.DataBind();
		}

		/// <summary>
		/// Bind Interest Only Data
		/// </summary>
		/// <param name="totalAmortisingInstallment"></param>
		public void BindInterestOnlyData(double totalAmortisingInstallment)
		{
			lblAmotisingInstallment.Text = totalAmortisingInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="account">The mortgage loan account</param>
		public void BindFixedDebitOrderData(IAccount account)
		{
			TotalAmountDue.Text = account.InstallmentSummary.TotalAmountDue.ToString(SAHL.Common.Constants.CurrencyFormat);

			FixedDebitOrderAmount.Text = account.FixedPayment.ToString(SAHL.Common.Constants.CurrencyFormat);
			foreach (IFinancialService fs in account.FinancialServices)
			{
				IMortgageLoan ml = fs as IMortgageLoan;
				if (ml != null && ml.AccountStatus.Key == account.AccountStatus.Key)
				{
					foreach (IFinancialServiceBankAccount fsba in ml.FinancialServiceBankAccounts)
					{
						if (fsba.GeneralStatus.Key == (int)GeneralStatuses.Active)
						{
							DebitOrderDay.Text = fsba.DebitOrderDay.ToString();
							return;
						}
					}
				}
			}
		}
		/// <summary>
		/// RowDataBound event for Future Dates Grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void BindFutureDatedDOGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			TableCellCollection cells = e.Row.Cells;

			IFutureDatedChange fdc = e.Row.DataItem as IFutureDatedChange;

			if (e.Row.DataItem != null)
			{
				cells[0].Text = fdc.Key.ToString();
				cells[1].Text = fdc.FutureDatedChangeDetails[0].Key.ToString(); // Using the first detail as there should be a 1 to 1 mapping as it is in Halo or the grid will be incorrect.
				cells[2].Text = Convert.ToDateTime(fdc.EffectiveDate).ToString(SAHL.Common.Constants.DateFormat);
				cells[3].Text = fdc.FutureDatedChangeDetails[0].Value;
				cells[4].Text = fdc.UserID;
				cells[5].Text = Convert.ToDateTime(fdc.ChangeDate).ToString(SAHL.Common.Constants.DateFormat);
			}
		}
		/// <summary>
		/// Bind Account Summary Grid
		/// </summary>
		/// <param name="accountLst"></param>
		public void BindAccountSummaryGrid(IEventList<IAccount> accountLst)
		{
			AccountsGrid.AddGridBoundColumn("", "Account Number", Unit.Percentage(15), HorizontalAlign.Left, true);
			AccountsGrid.AddGridBoundColumn("", "Product", Unit.Percentage(30), HorizontalAlign.Left, true);
			AccountsGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Center, true);
			AccountsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Open Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
			AccountsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Close Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
			AccountsGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(15), HorizontalAlign.Right, true);

			AccountsGrid.DataSource = accountLst;
			AccountsGrid.DataBind();

		}
		/// <summary>
		/// RowDataBound event for Account Summary Grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void AccountsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
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
							if (account is IAccountHOC)
							{
								amount += (account as IAccountHOC).MonthlyPremium;
							}
							else if (account is IAccountLifePolicy)
							{
								var lifePolicyAccount = (account as IAccountLifePolicy);
								if(lifePolicyAccount.LifePolicy != null)
								{
									amount += lifePolicyAccount.LifePolicy.MonthlyPremium;
								}
							}
							else
							{
								foreach (IFinancialService fs in account.FinancialServices)
								{
									if (IsAccountStatusValid(fs.AccountStatus))
										amount += fs.Payment;
								}
							}
						}
						e.Row.Cells[(int)SummaryGridColumnPositions.Amount].Text = amount.ToString();
					}
				}
			}
		}
		/// <summary>
		/// Click event for Cancel button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			if (CancelButtonClicked != null)
				CancelButtonClicked(sender, new KeyChangedEventArgs(e));

		}
		/// <summary>
		/// Click event for Submit button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			if (UpdateButtonClicked != null)
				UpdateButtonClicked(sender, new KeyChangedEventArgs(FutureOrderGrid.SelectedIndex));

			if (DeleteButtonClicked != null)
				DeleteButtonClicked(sender, new KeyChangedEventArgs(FutureOrderGrid.SelectedIndex));
		}
		/// <summary>
		/// Selected Index Change on Future Dated Grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void FutureOrderGrid_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FutureOrderGrid.SelectedIndex >= 0)
			{
				OnFutureOrderGridSelectedIndexChanged(sender, new KeyChangedEventArgs(FutureOrderGrid.SelectedIndex));
			}
		}
		/// <summary>
		/// Bind Updateable fields according to value selected on Grid
		/// </summary>
		/// <param name="futureDatedChange"></param>
		public void BindUpdateableControlsData(IFutureDatedChange futureDatedChange)
		{
			FixedDebitOrderAmountUpdate.Text = makeSafeNumber(futureDatedChange.FutureDatedChangeDetails[0].Value.ToString());
			EffectiveDateUpdate.Date = Convert.ToDateTime(getDateFromDDMMYYYY(futureDatedChange.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat)));
		}
		/// <summary>
		/// Returns Updated Record with values entered on screen
		/// </summary>
		/// <param name="futureDatedChange"></param>
		/// <returns></returns>
		public IFutureDatedChange GetUpdatedFDChange(IFutureDatedChange futureDatedChange)
		{
			if (EffectiveDateUpdate.DateIsValid)
				futureDatedChange.EffectiveDate = EffectiveDateUpdate.Date.Value;
			futureDatedChange.FutureDatedChangeDetails[0].Value = string.IsNullOrEmpty(FixedDebitOrderAmountUpdate.Text) ? Convert.ToString(0.00) : FixedDebitOrderAmountUpdate.Text.Trim();
			return futureDatedChange;
		}


		/// <summary>
		/// Returns the updated FixedPayment from screen
		/// </summary>
		public double UpdatedFixedDebitOrderAmount
		{
			get
			{
				return string.IsNullOrEmpty(FixedDebitOrderAmountUpdate.Text) ? 0.00 : Convert.ToDouble(FixedDebitOrderAmountUpdate.Text);
			}
		}


		/// <summary>
		/// Get Captured FutureDatedChange record for Add
		/// </summary>
		/// <param name="futureDatedChange"></param>
		/// <returns></returns>
		public IFutureDatedChange GetCapturedFutureDatedChange(IFutureDatedChange futureDatedChange)
		{
			if (EffectiveDateUpdate.DateIsValid)
				futureDatedChange.EffectiveDate = Convert.ToDateTime(EffectiveDateUpdate.Date.Value);
			futureDatedChange.UserID = this.CurrentPrincipal.Identity.Name;
			futureDatedChange.ChangeDate = DateTime.Today;
			futureDatedChange.FutureDatedChangeType = _lookUps.FutureDatedChangeTypes[0];
			futureDatedChange.InsertDate = DateTime.Now;
			futureDatedChange.NotificationRequired = false;

			return futureDatedChange;
		}

		/// <summary>
		/// Get Captured FutureDatedChangeDetail record for Add
		/// </summary>
		/// <param name="futureDatedChangeDetail"></param>
		/// <returns></returns>
		public IFutureDatedChangeDetail GetCapturedFutureDatedChangeDetail(IFutureDatedChangeDetail futureDatedChangeDetail)
		{
			futureDatedChangeDetail.UserID = this.CurrentPrincipal.Identity.Name;
			futureDatedChangeDetail.Value = string.IsNullOrEmpty(FixedDebitOrderAmountUpdate.Text) ? Convert.ToString(0.00) : FixedDebitOrderAmountUpdate.Text.Trim();
			futureDatedChangeDetail.TableName = "Account";
			futureDatedChangeDetail.ColumnName = "FixedPayment";
			futureDatedChangeDetail.Action = Convert.ToChar("U");
			futureDatedChangeDetail.ChangeDate = DateTime.Now;

			return futureDatedChangeDetail;
		}

		/// <summary>
		/// Returns effective date captured on screen
		/// </summary>
		public DateTime? GetEffectiveDateCaptured
		{
			get { return EffectiveDateUpdate.Date; }
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

		/// <summary>
		/// set default data on Load of View
		/// </summary>
		/// <param name="account"></param>
		public void SetUpInitialDataOnView(IAccount account)
		{
			if (FutureOrderGrid.SelectedIndex == -1)
			{
				FixedDebitOrderAmountUpdate.Text = makeSafeNumber(account.FixedPayment.ToString());
				EffectiveDateUpdate.Date = DateTime.Now;
			}
		}

		private static DateTime getDateFromDDMMYYYY(string date)
		{
			int d = int.Parse(date.Substring(0, 2));
			int m = int.Parse(date.Substring(3, 2));
			int y = int.Parse(date.Substring(6, 4));
			return new DateTime(y, m, d);
		}

		private static string makeSafeNumber(string num)
		{
			if (num.IndexOf('.', 0) == 0)
				num = "0" + num;

			num = num.Replace(" ", "");
			num = num.Replace(",", "");
			num = num.Replace("R", "");

			return num;
		}

		/// <summary>
		/// Event handler for Selected Index Change on Future Grid
		/// </summary>
		public event KeyChangedEventHandler OnFutureOrderGridSelectedIndexChanged;
		/// <summary>
		/// Event handler for Update Button Clicked
		/// </summary>
		public event KeyChangedEventHandler UpdateButtonClicked;
		/// <summary>
		/// Event handler for Delete Button Clicked
		/// </summary>
		public event KeyChangedEventHandler DeleteButtonClicked;
		/// <summary>
		/// Event handler for Cancel Button Clicked
		/// </summary>
		public event KeyChangedEventHandler CancelButtonClicked;

	}
}

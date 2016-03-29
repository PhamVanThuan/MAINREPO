using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Web.Views.Migrate.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.Web.UI.Events;
using System.Data;

namespace SAHL.Web.Views.Migrate
{
	public partial class Proposal : SAHLCommonBaseView, SAHL.Web.Views.Migrate.Interfaces.IProposal
	{
		private IDictionary<int, string> proposalStatusses;
		private IDictionary<int, string> inclusiveChoices;
		private IDictionary<int, string> yesNoChoices;
		private IDictionary<int, string> marketRates;
		private DataTable approvalUsers;

		public IDictionary<int, string> ProposalStatusses
		{
			get
			{
				return proposalStatusses;
			}
			set
			{
				proposalStatusses = value;
				BindProposalStatusses(proposalStatusses);
			}
		}
		public IDictionary<int, string> InclusiveChoices
		{
			get
			{
				return inclusiveChoices;
			}
			set
			{
				inclusiveChoices = value;
				BindInclusiveChoices(inclusiveChoices);
			}
		}
		public IDictionary<int, string> YesNoChoices
		{
			get
			{
				return yesNoChoices;
			}
			set
			{
				yesNoChoices = value;
				BindAcceptedProposal(yesNoChoices);
			}
		}
		public IDictionary<int, string> MarketRates
		{
			get
			{
				return marketRates;
			}
			set
			{
				marketRates = value;
				BindMarketRates(marketRates);
			}
		}
		public DataTable ApprovalUsers
		{
			get
			{
				return approvalUsers;
			}
			set
			{
				approvalUsers = value;
				BindApprovalUsers(approvalUsers);
			}
		}

		public event EventHandler<EventArgs> BackClick;
		public event EventHandler<EventArgs> AddClick;
		public event EventHandler<KeyChangedEventArgs> RemoveClick;
		public event EventHandler<EventArgs> FinishClick;

		public event EventHandler<EventArgs> HOCOrLifeChanged;

		public int AccountKey { get; set; }
        public bool SaveProposal { get; set; }
		public decimal TotalInstalmentAmount { get; set; }
		public bool HOCInclusive 
		{
			get { return cmbHOCInclusive.SelectedValue == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false; }
			set { cmbHOCInclusive.SelectedValue = value ? "1" : "0"; }
		}
		public bool LifeInclusive 
		{ 
			get { return cmbLifeInclusive.SelectedValue == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false; }
			set { cmbLifeInclusive.SelectedValue = value ? "1" : "0"; } 
		}

		public decimal HOCInstalment { get; set; }
		public decimal LifeInstalment { get; set; }

        private bool _gridCreated;

		/// <summary>
		/// On Load
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
            lblAccountKey.Text = AccountKey.ToString();
            gridProposalItems.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(OnHtmlDataCellPrepared);
			base.OnLoad(e);
		}

		/// <summary>
		/// On Pre Render
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			btnRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove the selected proposal item ?');");
			base.OnPreRender(e);
		}

		/// <summary>
		/// Bind Inclusive Choices
		/// </summary>
		/// <param name="inclusiveChoices"></param>
		private void BindInclusiveChoices(IDictionary<int, string> inclusiveChoices)
		{
			cmbHOCInclusive.DataSource = inclusiveChoices;
			cmbHOCInclusive.DataBind();

			cmbLifeInclusive.DataSource = inclusiveChoices;
			cmbLifeInclusive.DataBind();
		}

		/// <summary>
		/// Bind Proposal Statusses
		/// </summary>
		/// <param name="proposalStatusses"></param>
		private void BindProposalStatusses(IDictionary<int, string> proposalStatusses)
		{
			cmbProposalStatus.DataSource = proposalStatusses;
			cmbProposalStatus.DataBind();
		}

		/// <summary>
		/// Bind Accepted Statusses
		/// </summary>
		/// <param name="yesNoChoices"></param>
		private void BindAcceptedProposal(IDictionary<int, string> yesNoChoices)
		{
			cmbAcceptedProposal.DataSource = yesNoChoices;
			cmbAcceptedProposal.DataBind();
		}

		/// <summary>
		/// Bind Market Rates
		/// </summary>
		/// <param name="marketRates"></param>
		public void BindMarketRates(IDictionary<int, string> marketRates)
		{
			cmbMarketRate.DataSource = marketRates;
			cmbMarketRate.DataBind();

			cmbMarketRate.SelectedValue = "99";
			cmbMarketRate.Enabled = false;
		}

		/// <summary>
		/// Bind Approval Users
		/// </summary>
		/// <param name="approvalUsers"></param>
		public void BindApprovalUsers(DataTable approvalUsers)
		{
			cmbApprovalUsers.DataSource = approvalUsers;
			cmbApprovalUsers.DataTextField = "Description";
			cmbApprovalUsers.DataValueField = "Key";
			cmbApprovalUsers.DataBind();
		}

		/// <summary>
		/// Bind Proposal
		/// </summary>
		/// <param name="proposal"></param>
		public void BindProposal(IMigrationDebtCounsellingProposal proposal)
		{
			if(proposal.ProposalDate != DateTime.MinValue)
				dateProposalDate.Date = proposal.ProposalDate;
			cmbProposalStatus.SelectedValue = proposal.ProposalStatusKey.ToString();
			cmbAcceptedProposal.SelectedValue = proposal.AcceptedProposal ? "1" : "0";
			cmbHOCInclusive.SelectedValue = proposal.HOCInclusive ? "1" : "0";
			cmbLifeInclusive.SelectedValue = proposal.LifeInclusive ? "1" : "0";

			//Debt Counselling Case stuff
			cmbApprovalUsers.SelectedValue = proposal.DebtCounselling.ApprovalUserKey.ToString();
			dateReview.Date = proposal.DebtCounselling.ReviewDate;
			dateApprovalDate.Date = proposal.DebtCounselling.ApprovalDate;
			//txtApprovalAmount.Amount = (double?)proposal.DebtCounselling.ApprovalAmount;

			BindProposalItems(proposal.DebtCounsellingProposalItems);
		}

		/// <summary>
		/// Bind Proposal Items
		/// </summary>
		/// <param name="proposalItems"></param>
		private void BindProposalItems(IEventList<IMigrationDebtCounsellingProposalItem> proposalItems)
		{
            SetupGrid();

			gridProposalItems.SortBy(gridProposalItems.Columns["StartDate"], DevExpress.Data.ColumnSortOrder.Ascending);

			gridProposalItems.DataSource = proposalItems;
			gridProposalItems.DataBind();

            //lazy hack, cause this is temp stuff.....
            DateTime dtISD = DateTime.Now.AddYears(-100);
            int stIPeriod = 0;
            if (proposalItems.Count > 0)
            {
                foreach (IMigrationDebtCounsellingProposalItem mPI in proposalItems)
                {
                    if (mPI.EndDate > dtISD)
                    {
                        dtISD = mPI.EndDate.AddDays(1);
                        stIPeriod = mPI.EndPeriod;
                    }
                }
                //set the Proposal start date for the view
                dateStartDate.Date = dtISD;
            }
            //begin at the next period
            txtStartPeriod.Text = (stIPeriod + 1).ToString();
            if (stIPeriod != 0)
                dateStartDate.Enabled = false;
            else
            {
                dateStartDate.Date = null;
                dateStartDate.Enabled = true;
            }
		}

        private void AddGridColumn(string fieldName, string caption, Unit width, GridFormatType formatType, string format, HorizontalAlign align, GridViewColumnFixedStyle fixedStyle, bool visible)
        {
            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = fieldName;
            col.Caption = caption;

            col.Width = width;
            col.FixedStyle = fixedStyle;
            col.Format = formatType;
            if (!String.IsNullOrEmpty(format))
                col.FormatString = format;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            gridProposalItems.Columns.Add(col);
        }

        private void SetupGrid()
        {
            if (!_gridCreated)
            {
                //clear columns
                gridProposalItems.Columns.Clear();

                gridProposalItems.KeyFieldName = "Key";
                gridProposalItems.SettingsText.EmptyDataRow = "No Details";

                // key column
                AddGridColumn("Key", "", Unit.Pixel(10), GridFormatType.GridNumber, "", HorizontalAlign.Left, GridViewColumnFixedStyle.None, false);

                AddGridColumn("StartDate", "Start Date", Unit.Pixel(100), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("EndDate", "End Captured", Unit.Pixel(100), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("StartPeriod", "Start Period", Unit.Pixel(100), GridFormatType.GridString, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("EndPeriod", "End Period", Unit.Pixel(100), GridFormatType.GridString, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Market Rate", Unit.Pixel(100), GridFormatType.GridString, null, HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("InterestRate", "Interest Rate", Unit.Pixel(100), GridFormatType.GridCurrency, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("Amount", "Amount", Unit.Pixel(100), GridFormatType.GridCurrency, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("AdditionalAmount", "Add Amount", Unit.Pixel(100), GridFormatType.GridCurrency, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("CreateDate", "Creation Date", Unit.Pixel(100), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("InstalmentPercentage", "Instalment %", Unit.Pixel(100), GridFormatType.GridCurrency, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("AnnualEscalation", "Annual Escalation", Unit.Pixel(120), GridFormatType.GridCurrency, SAHL.Common.Constants.RateFormat, HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);

                _gridCreated = true;
            }
        }

		/// <summary>
		/// On Html Data Cell Prepared
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnHtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (e != null)
			{
				switch (e.DataColumn.Caption)
				{
					case "Market Rate":
						e.Cell.Text = "Fixed";
						break;
				}
			}
		}

		/// <summary>
		/// Save Proposal
		/// </summary>
		/// <param name="proposal"></param>
		public void GetProposalFromView(IMigrationDebtCounsellingProposal proposal)
		{
			proposal.ProposalDate = dateProposalDate.Date.Value;
			proposal.ProposalStatusKey = Convert.ToInt32(cmbProposalStatus.SelectedValue);
			proposal.AcceptedProposal = int.Parse(cmbAcceptedProposal.SelectedValue) == 1 ? true : false;
			proposal.HOCInclusive = int.Parse(cmbHOCInclusive.SelectedValue) == 1 ? true : false;
			proposal.LifeInclusive = int.Parse(cmbLifeInclusive.SelectedValue) == 1 ? true : false;

			//Debt Counselling Case stuff
			proposal.DebtCounselling.ApprovalDate = dateApprovalDate.Date;
			proposal.DebtCounselling.ReviewDate= dateReview.Date;
			//proposal.DebtCounselling.ApprovalAmount = (decimal?)txtApprovalAmount.Amount;

			if (cmbApprovalUsers.SelectedValue != "-select-")
			{
				proposal.DebtCounselling.ApprovalUserKey = Convert.ToInt32(cmbApprovalUsers.SelectedValue);
			}
		}

		/// <summary>
		/// Save Proposal Item
		/// </summary>
		/// <param name="proposalItem"></param>
		public void GetProposalItemFromView(IMigrationDebtCounsellingProposalItem proposalItem)
		{
			proposalItem.AdditionalAmount = !String.IsNullOrEmpty(txtAdditionalAmount.Text) ? Convert.ToDecimal(txtAdditionalAmount.Text) : 0;
			proposalItem.Amount = (decimal)(txtAmount.Amount ?? 0);
			proposalItem.AnnualEscalation = (decimal)(txtAnnualEscalation.Amount.HasValue ? txtAnnualEscalation.Amount.Value / 100.0 : 0);
			proposalItem.CreateDate = DateTime.Now;
			proposalItem.EndDate = dateEndDate.Date.Value;
			proposalItem.InterestRate = (decimal)(txtInterestRate.Amount.HasValue ? txtInterestRate.Amount.Value / 100.0 : 0);
			//proposalItem.MarketRateKey = Convert.ToInt32(cmbMarketRate.SelectedValue);
			proposalItem.StartDate = dateStartDate.Date.Value;
            proposalItem.StartPeriod = Convert.ToInt16(txtStartPeriod.Text);
            proposalItem.EndPeriod = Convert.ToInt16(txtEndPeriod.Text);
		}

		/// <summary>
		/// Add Proposal Item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void OnAddProposalItemClick(object sender, EventArgs args)
		{
			if (AddClick != null)
			{
				AddClick(this, args);
			}
		}

		/// <summary>
		/// Remove Proposal Item Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void OnRemoveProposalItemClick(object sender, EventArgs args)
		{
			// get the selected ProposalItemKey
			KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalItems.SelectedKeyValue);

			if (RemoveClick != null)
				RemoveClick(sender, keyChangedEventArgs);
		}

		/// <summary>
		/// Back Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnBackClick(object sender, EventArgs e)
		{
			if (BackClick != null)
			{
				BackClick(sender, e);
			}
		}

		/// <summary>
		/// Finish Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnFinishClick(object sender, EventArgs e)
		{
			if (FinishClick != null)
			{
				FinishClick(sender, e);
			}
		}

		/// <summary>
		/// On HOC or Life Changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnHOCOrLifeChanged(object sender, EventArgs e)
		{
			HOCInclusive = cmbHOCInclusive.SelectedValue == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false;
			LifeInclusive = cmbLifeInclusive.SelectedValue == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false;
			if (HOCOrLifeChanged != null)
			{
				HOCOrLifeChanged(sender, e);
			}
		}

		/// <summary>
		/// Validate Proposal
		/// </summary>
		/// <returns></returns>
		public bool ValidateProposal()
		{
			bool isValid = true;
            SaveProposal = true;

            //all entries are empty, allow continue...
            if (!dateProposalDate.Date.HasValue
                && !dateReview.Date.HasValue
                && !dateApprovalDate.Date.HasValue
                && cmbProposalStatus.SelectedValue == "-select-"
                && (
                    cmbAcceptedProposal.SelectedValue == "-select-"
                    || cmbAcceptedProposal.SelectedValue == "0"
                    )
                && (
                    cmbHOCInclusive.SelectedValue == "-select-"
                    || cmbHOCInclusive.SelectedValue == "0"
                    )
                && (
                    cmbLifeInclusive.SelectedValue == "-select-"
                    || cmbLifeInclusive.SelectedValue == "0"
                    )
                && cmbApprovalUsers.SelectedValue == "-select-")
            {
                SaveProposal = false;
                return isValid;
            }


			if (!dateProposalDate.Date.HasValue)
			{
				Messages.Add(new Error("The Proposal Date is Mandatory", "The Proposal Date is Mandatory"));
				isValid = false;
			}
			if (cmbProposalStatus.SelectedValue == "-select-")
			{
				Messages.Add(new DomainMessage("Please select a Proposal Status", "Please select a Proposal Status"));
				isValid = false;
            }
			else if (String.Compare(cmbProposalStatus.SelectedValue, ((int)SAHL.Common.Globals.ProposalStatuses.Active).ToString(), true) != 0
                && String.Compare(cmbAcceptedProposal.SelectedValue, "1", true) == 0) //1, "Yes"
            {
                Messages.Add(new DomainMessage("Only Active Proposals can be accepted", "Only Active Proposals can be accepted"));
                isValid = false;
            }
			if (cmbAcceptedProposal.SelectedValue == "-select-")
			{
				Messages.Add(new DomainMessage("Please indicate if the Proposal is Accepted", "Please indicate if the Proposal is Accepted"));
				isValid = false;
			}
			if (cmbHOCInclusive.SelectedValue == "-select-")
			{
				Messages.Add(new DomainMessage("Please select whether HOC is Inclusive", "Please select whether HOC is Inclusive"));
				isValid = false;
			}
			if (cmbLifeInclusive.SelectedValue == "-select-")
			{
				Messages.Add(new DomainMessage("Please select whether Life is Inclusive", "Please select whether Life is Inclusives"));
				isValid = false;
			}
			return isValid;
		}

		/// <summary>
		/// Validate Proposal Item
		/// </summary>
		/// <returns></returns>
		public bool ValidateProposalItem()
		{
			bool isValid = true;
			if (dateStartDate.Date == null)
			{
				Messages.Add(new DomainMessage("Please Select a Start Date", "Please Select A Start Date"));
				isValid = false;
			}
			if (dateEndDate.Date == null)
			{
				Messages.Add(new DomainMessage("Please Select an End Date", "Please Select An End Date"));
				isValid = false;
			}
			if (String.IsNullOrEmpty(txtInterestRate.Text) || Convert.ToDecimal(txtInterestRate.Text) < 0)
			{
				Messages.Add(new DomainMessage("The Interest Rate is Mandatory", "The Interest Rate is Mandatory"));
				isValid = false;
			}
			if (String.IsNullOrEmpty(txtAmount.Text) || Convert.ToDecimal(txtAmount.Text) < 0)
			{
				Messages.Add(new DomainMessage("The Amount is Mandatory", "The Amount is Mandatory"));
				isValid = false;
			}
			if (String.IsNullOrEmpty(txtAnnualEscalation.Text) || Convert.ToDecimal(txtAnnualEscalation.Text) < 0)
			{
				Messages.Add(new DomainMessage("The Annual Escalation is Mandatory", "The Annual Escalation is Mandatory"));
				isValid = false;
			}
			return isValid;
		}

		/// <summary>
		/// Reset Input Fields
		/// </summary>
		public void ResetInputFields()
		{
			dateStartDate.Text = String.Empty;
			dateEndDate.Text = String.Empty;
			txtInterestRate.Text = String.Empty;
			txtAmount.Text = String.Empty;
			txtAdditionalAmount.Text = String.Empty;
			txtAnnualEscalation.Text = "0";
            txtStartPeriod.Text = String.Empty;
            txtEndPeriod.Text = String.Empty;
		}
	}
}
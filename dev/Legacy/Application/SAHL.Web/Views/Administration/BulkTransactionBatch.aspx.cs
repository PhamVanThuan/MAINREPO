using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Controls;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration
{
	public partial class Views_BulkTransactionBatch : SAHLCommonBaseView,IBulkTransactionBatch
	{
		private ListItem li;

		private string presenterState;
		private DataView batchTransactionDT;

		/// <summary>
		/// OnInitialise Event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (!ShouldRunPage) return;
		}

		/// <summary>
		/// Enable Next and Prev buttons, when grid has more than 1 page of data
		/// </summary>
		public bool SetButtonsNextAndPrevious
		{
			set
			{
				NextButton.Enabled = value;
				PreviousButton.Enabled = value;
			}
		}

		/// <summary>
		/// Enable Post Button
		/// </summary>
		public bool SetUpdateButtons
		{
			set
			{
				PostButton.Enabled = value;
			}
		}

		/// <summary>
		/// Set controls for Update
		/// </summary>
		public bool SetAddUpdateControls
		{
			set
			{
				currTransactionAmount.Enabled = value;
				RemoveButton.Enabled = value;
				UpdateButton.Enabled = value;
			}
		}

		/// <summary>
		/// Disable DropDowns - in Add Mode
		/// </summary>
		public bool DisableAllDropDownsForAdd
		{
			set
			{
				SubsidyType.Enabled = value;
				ParentSubsidyProvider.Enabled = value;
				TransactionType.Enabled = value;
				dtEffectiveDate.Enabled = value;
			}

		}

		/// <summary>
		/// Gets/sets the captured transaction amount.
		/// </summary>
		public double? TransactionAmount
		{
			get
			{
				return currTransactionAmount.Amount;
			}
			set
			{
				currTransactionAmount.Amount = value;
			}
		}

		/// <summary>
		/// Restore Drop Down Values which were being overwritten by PostBack
		/// </summary>
		/// <param name="subsidyProviderKeySelected"></param>
		/// <param name="transactionTypeSelected"></param>
		/// <param name="subsidyProviderTypeKey"></param>
		/// <param name="effectiveDate"></param>
		public void RestoreDropDownValues(int subsidyProviderKeySelected, int transactionTypeSelected, int subsidyProviderTypeKey, DateTime effectiveDate)
		{
			SubsidyType.SelectedValue = subsidyProviderTypeKey.ToString();
			ParentSubsidyProvider.SelectedValue = subsidyProviderKeySelected.ToString();
			TransactionType.SelectedValue = transactionTypeSelected.ToString();
			dtEffectiveDate.Date = effectiveDate;
		}

		/// <summary>
		/// Set MessageBox once batch has been saved
		/// </summary>
		/// <param name="batchNumber"></param>
		public void SetMesssageBoxForAdd(string batchNumber)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("alert('The Batch has been saved and the batch number is " + batchNumber + "');");
			Page.ClientScript.RegisterStartupScript(GetType(), "MessageDisplay", sb.ToString(), true);
		}

		/// <summary>
		/// Bind Transaction Types
		/// </summary>
		/// <param name="transactionTypes"></param>
		public void BindTransactionTypes(IReadOnlyEventList<ITransactionType> transactionTypes)
		{
			TransactionType.DataSource = transactionTypes;
			TransactionType.DataTextField = "Description";
			TransactionType.DataValueField = "Key";
			TransactionType.DataBind();
			TransactionType.Items.Insert(0, new ListItem("- Please Select -", "-select-"));
		}

		/// <summary>
		/// Bind Batch Status Drop Down
		/// </summary>
		/// <param name="batchStatus"></param>
		public void BindBatchStatus(IEventList<IBulkBatchStatus> batchStatus)
		{
			BatchStatus.DataSource = batchStatus;
			BatchStatus.DataTextField = "Description";
			BatchStatus.DataValueField = "Key";
			BatchStatus.DataBind();
			BatchStatus.Items.Insert(0, new ListItem("- Please Select -", "-select-"));
	        
			li = BatchStatus.Items.FindByValue("3"); // Remove Ready for Processing
			if (li != null)
				BatchStatus.Items.Remove(li);
			li = BatchStatus.Items.FindByValue("4"); // Remove Processing Started
			if (li != null)
				BatchStatus.Items.Remove(li);
		}

		/// <summary>
		/// Bind Batch Status Drop Down for Update Mode
		/// </summary>
		/// <param name="batchStatus"></param>
		public void BindBatchStatusForUpdate(IEventList<IBulkBatchStatus> batchStatus)
		{
			BatchStatus.DataSource = batchStatus;
			BatchStatus.DataTextField = "Description";
			BatchStatus.DataValueField = "Key";
			BatchStatus.DataBind();
			BatchStatus.Items.Insert(0, new ListItem("- Please Select -", "-select-"));

			li = BatchStatus.Items.FindByValue("2"); // Remove Sucessfull
			if (li != null)
				BatchStatus.Items.Remove(li);
			li = BatchStatus.Items.FindByValue("3"); // Remove Ready for Processing
			if (li != null)
				BatchStatus.Items.Remove(li);
			li = BatchStatus.Items.FindByValue("4"); // Remove Processing Started
			if (li != null)
				BatchStatus.Items.Remove(li);
		}

		/// <summary>
		/// Bind Batch Status drop down for Add Mode
		/// </summary>
		public void BindStatusDropDownforAdd()
		{
			li = BatchStatus.Items.FindByValue("2"); // Remove Sucessfull
			if (li != null)
				BatchStatus.Items.Remove(li);   
		}

		/// <summary>
		/// Bind BulkGrid
		/// </summary>
		/// <param name="action"></param>
		/// <param name="pageNumber"></param>
		/// <param name="totalPages"></param>
		/// <param name="totalNumber"></param>
		/// <param name="batchTransaction"></param>
		public void BindBulkGridDisplay(string action, int pageNumber, int totalPages, int totalNumber, DataView batchTransaction)
		{
			presenterState = action;
			batchTransactionDT = batchTransaction;

			if (batchTransaction == null)
				BulkGrid.NullDataSetMessage = "There are no records to Display";

			BulkGrid.Columns.Clear();
			BulkGrid.AddGridBoundColumn("BatchTransactionKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
			BulkGrid.AddGridBoundColumn("LegalEntityKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
			BulkGrid.AddGridBoundColumn("BatchTransactionStatusKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
	        
			BulkGrid.AddGridBoundColumn("AccountKey", "Account Number", Unit.Percentage(30), HorizontalAlign.Left, true);
			BulkGrid.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(40), HorizontalAlign.Left, true);
			BulkGrid.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Transaction Amount", false, Unit.Percentage(30), HorizontalAlign.Right, true);
	          
			BulkGrid.DataSource = batchTransaction;
			BulkGrid.DataBind();

			RowCount.Text = "Total Count: " + totalNumber;

			PageIndicator.Text = "Page " + pageNumber + " of " + totalPages;

			PreviousButton.Enabled = pageNumber != 1;

			NextButton.Enabled = pageNumber != totalPages;
		}

		/// <summary>
		/// Bind BulkGrid for Add Mode
		/// </summary>
		/// <param name="action"></param>
		/// <param name="selectedGridIndex"></param>
		/// <param name="pageNumber"></param>
		/// <param name="totalPages"></param>
		/// <param name="totalNumber"></param>
		/// <param name="batchTransaction"></param>
		public void BindBulkGridAdd(string action, int selectedGridIndex , int pageNumber, int totalPages, int totalNumber, DataView batchTransaction)
		{
			batchTransactionDT = batchTransaction;
			presenterState = action;
	        
			if (batchTransaction == null)
				BulkGrid.NullDataSetMessage = "There are no records to Display";

			BulkGrid.Columns.Clear();
			BulkGrid.AddGridBoundColumn("BatchTransactionKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
			BulkGrid.AddGridBoundColumn("LegalEntityKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
			BulkGrid.AddGridBoundColumn("BatchTransactionStatusKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);

			BulkGrid.AddGridBoundColumn("AccountKey", "Account Number", Unit.Percentage(8), HorizontalAlign.Left, true);
			BulkGrid.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(21), HorizontalAlign.Left, true);
            
            //#11144 these columns must not be visible
            //BulkGrid.AddGridBoundColumn("FixedDebit", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Fixed Debit", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            //BulkGrid.AddGridBoundColumn("Installment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            //BulkGrid.AddGridBoundColumn("Premium", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Premiums", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            BulkGrid.AddGridBoundColumn("FixedDebit", "Fixed Debit", Unit.Percentage(14), HorizontalAlign.Right, false);
            BulkGrid.AddGridBoundColumn("Installment", "Instalment", Unit.Percentage(14), HorizontalAlign.Right, false);
            BulkGrid.AddGridBoundColumn("Premium", "Premiums", Unit.Percentage(14), HorizontalAlign.Right, false);
            
            
            BulkGrid.AddGridBoundColumn("StopOrderAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Stop Order Amount", false, Unit.Percentage(14), HorizontalAlign.Right, true);
			BulkGrid.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Transaction Amount", false, Unit.Percentage(16), HorizontalAlign.Right, true);

			BulkGrid.DataSource = batchTransaction;

			if (selectedGridIndex > 0)
			{
				BulkGrid.SelectedIndex = selectedGridIndex;
				BulkGrid.SelectedRowStyle.BackColor = System.Drawing.Color.DarkBlue;
				BulkGrid.SelectFirstRow = false;
			}
			else
			{
				BulkGrid.SelectedIndex = 0;
				BulkGrid.SelectFirstRow = true;
			}

			BulkGrid.DataBind();

			RowCount.Text = "Total Count: " + totalNumber;

			PageIndicator.Text = "Page " + pageNumber + " of " + totalPages;

			PreviousButton.Enabled = pageNumber != 1;

			NextButton.Enabled = pageNumber != totalPages;     
		}

		/// <summary>
		/// BulkGrid RowDataBound event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void BulkGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			// TableCellCollection cells = e.Row.Cells;

			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				switch (e.Row.Cells[2].Text)
				{
					case "1":
						e.Row.Cells[3].Style["color"] = "#000000";
						e.Row.Cells[3].Style["background-color"] = "#FFFF66";
						e.Row.Cells[4].Style["color"] = "#000000";
						e.Row.Cells[4].Style["background-color"] = "#FFFF66";
						break;
					case "2":
						e.Row.Cells[3].Style["color"] = "#FFFFFF";
						e.Row.Cells[3].Style["background-color"] = "#AA0000";
						e.Row.Cells[4].Style["color"] = "#FFFFFF";
						e.Row.Cells[4].Style["background-color"] = "#AA0000";
						break;
					case "3":
						e.Row.Cells[3].Style["color"] = "#FFFFFF";
						e.Row.Cells[3].Style["background-color"] = "#00AA00";
						e.Row.Cells[4].Style["color"] = "#FFFFFF";
						e.Row.Cells[4].Style["background-color"] = "#00AA00";
						break;
					case "4":
						e.Row.Cells[3].Style["color"] = "#FFFFFF";
						e.Row.Cells[3].Style["background-color"] = "#0000AA";
						e.Row.Cells[4].Style["color"] = "#FFFFFF";
						e.Row.Cells[4].Style["background-color"] = "#0000AA";
						break;
				}
			}

			if (e.Row.RowType == DataControlRowType.Footer)
			{
				int countSum = 0;
				double fixedSum = 0.0d;
				double installSum = 0.0d;
				double premSum = 0.0d;
				double amountSum = 0.0d;
				double stopOrderSum = 0.0d;
	            
				for (int ii = 0; ii < batchTransactionDT.Count; ii++)
				{
					if ((int)batchTransactionDT[ii]["BatchTransactionStatusKey"] != 4)
					{
						if (presenterState == "Add" || presenterState == "Update")
						{
							if (batchTransactionDT[ii]["FixedDebit"].ToString().Length > 0)
								fixedSum += (double)batchTransactionDT[ii]["FixedDebit"];
							if (batchTransactionDT[ii]["Installment"].ToString().Length > 0)
								installSum += (double)batchTransactionDT[ii]["Installment"];
							if (batchTransactionDT[ii]["Premium"].ToString().Length > 0)
								premSum += (double)batchTransactionDT[ii]["Premium"];
							if (batchTransactionDT[ii]["Amount"].ToString().Length > 0)
								amountSum += (double)batchTransactionDT[ii]["Amount"];
							if (batchTransactionDT[ii]["StopOrderAmount"].ToString().Length > 0)
								stopOrderSum += (double)batchTransactionDT[ii]["StopOrderAmount"];
							countSum++;
						}
						else
						if (presenterState == "Display" || presenterState == "Delete")
						{
							if (batchTransactionDT[ii]["Amount"].ToString().Length > 0)
								amountSum += (double)batchTransactionDT[ii]["Amount"];
							countSum++;
						}
					}
				}
	   
				// this is the count per number of recs on grid at that time - not total count
				e.Row.Cells[3].Text = "Total Count";

				e.Row.Cells[4].Text = countSum.ToString();

				switch (presenterState)
				{
					case "Display":
					case "Delete":
						e.Row.Cells[5].Text = amountSum.ToString(SAHL.Common.Constants.CurrencyFormat);
						e.Row.Cells[5].Style["text-align"] = "right";
						break;
					case "Add":
					case "Update":
						e.Row.Cells[5].Text = fixedSum.ToString(SAHL.Common.Constants.CurrencyFormat);
						e.Row.Cells[5].Style["text-align"] = "right";
						e.Row.Cells[6].Text = installSum.ToString(SAHL.Common.Constants.CurrencyFormat);
						e.Row.Cells[6].Style["text-align"] = "right";
						e.Row.Cells[7].Text = premSum.ToString(SAHL.Common.Constants.CurrencyFormat);
						e.Row.Cells[7].Style["text-align"] = "right";
						e.Row.Cells[8].Text = stopOrderSum.ToString(SAHL.Common.Constants.CurrencyFormat);
						e.Row.Cells[8].Style["text-align"] = "right";
						e.Row.Cells[9].Text = amountSum.ToString(SAHL.Common.Constants.CurrencyFormat);
						e.Row.Cells[9].Style["text-align"] = "right";
						break;
				}
			}
		}

		/// <summary>
		/// Bind Batch Number drop down
		/// </summary>
		/// <param name="bulkBatch"></param>
		public void BindBatchNumber (IList<IBulkBatch> bulkBatch)
		{
				BatchNumber.DataSource = bulkBatch;
				BatchNumber.DataTextField = "Description";
				BatchNumber.DataValueField = "Key";
				BatchNumber.DataBind();

				BatchNumber.Items.Insert(0, new ListItem("- Please Select -", "-select-"));
		}

		/// <summary>
		/// Bind BatchStatus drop down
		/// </summary>
		/// <param name="batchTransactionStatus"></param>
		public void BindTransactionStatus(IEventList<IBatchTransactionStatus> batchTransactionStatus)
		{
			TransactionStatus.DataSource = batchTransactionStatus;
			TransactionStatus.DataTextField = "Description";
			TransactionStatus.DataValueField = "Key";
			TransactionStatus.DataBind();
			TransactionStatus.Items.Insert(0, new ListItem("All", "-1"));
		}

		/// <summary>
		///  Set controls for Display
		/// </summary>
		public void SetControlsForDisplay()
		{
			OtherPanel.Visible = false;
			CancelButton.Visible = false;
			SaveButton.Visible = false;
			PostButton.Visible = false;
			DeleteButton.Visible = false;

			TransactionType.Visible = false;
			dtEffectiveDate.Visible = false;
			SubsidyTypeRow.Visible = false;
			ParentSubsidyProviderRow.Visible = false;

			BulkGrid.GridHeight = Unit.Pixel(290);
		}

		/// <summary>
		/// Set controls for Add
		/// </summary>
		public void SetControlsForAdd()
		{
			DeleteButton.Visible = false;
			SubsidyProviderTitle.Visible = false;
			BatchStatusRow.Visible = false;
			BatchNumberRow.Visible = false;
			SubsidyProviderDisplay.Visible = false;
			TransactionTypeDisplay.Visible = false;
			EffectiveDateDisplay.Visible = false;
			TransactionStatusRow.Visible = false;

			BulkGrid.GridHeight = Unit.Pixel(200);
		}

		/// <summary>
		/// Set controls for Update
		/// </summary>
		public void SetControlsForUpdate()
		{
			DeleteButton.Visible = false;
			TransactionType.Visible = false;

			EffectiveDateDisplay.Visible = false;
			dtEffectiveDate.Visible = true;
			SubsidyTypeRow.Visible = false;
			ParentSubsidyProviderRow.Visible = false;
			TransactionStatusRow.Visible = false;

			BulkGrid.GridHeight = Unit.Pixel(210);
		}
	    
		/// <summary>
		/// Set controls for Delete
		/// </summary>
		public void SetControlsForDelete()
		{
			OtherPanel.Visible = false;
			CancelButton.Visible = true;
			SaveButton.Visible = false;
			PostButton.Visible = false;

			BatchStatusRow.Visible = false;
			TransactionType.Visible = false;
			dtEffectiveDate.Visible = false;
			SubsidyTypeRow.Visible = false;
			ParentSubsidyProviderRow.Visible = false;
			TransactionStatusRow.Visible = false;

			DeleteButton.Attributes["onclick"] = "if(!confirm('Are you sure you would like to delete batch " + hiddenBox.Text + "?')) return false";
		}

		/// <summary>
		/// Bind the BatchFields for Display mode, based on BulkBatch Selected
		/// </summary>
		/// <param name="bulkBatch"></param>
		/// <param name="subsidyProvider"></param>
		/// <param name="transactionType"></param>
		public void BindBatchFields(IBulkBatch bulkBatch, string subsidyProvider, string transactionType)
		{
			hiddenBox.Text = bulkBatch.Description;
			EffectiveDateDisplay.Text = bulkBatch.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
			dtEffectiveDate.Text = bulkBatch.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
			SubsidyProviderDisplay.Text = subsidyProvider;
			TransactionTypeDisplay.Text = transactionType;
		}

		/// <summary>
		/// Set Postback Type on BulkGrid
		/// </summary>
		public void SetPostBackType()
		{
			BulkGrid.PostBackType = GridPostBackType.SingleClick;
		}

		/// <summary>
		/// Bind Subsidy Types
		/// </summary>
		/// <param name="subsidyProviderType"></param>
		public void BindSubsidyTypes(IEventList<ISubsidyProviderType> subsidyProviderType)
		{
			List<ISubsidyProviderType> sortedList = new List<ISubsidyProviderType>(subsidyProviderType);
			sortedList.Sort(
			  delegate(ISubsidyProviderType spt1, ISubsidyProviderType spt2)
			  {
				  return spt1.Description.CompareTo(spt2.Description);
			  });

			SubsidyType.Items.Clear();
			SubsidyType.DataSource = sortedList;
			SubsidyType.DataTextField = "Description";
			SubsidyType.DataValueField = "Key";
			SubsidyType.DataBind();
			SubsidyType.VerifyPleaseSelect();
		}

		/// <summary>
		/// Bind Parent Subsidy Drop Down
		/// </summary>
		/// <param name="subsidyProvider"></param>
		public void BindParentSubsidyProvider(IList<SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider> subsidyProvider)
		{
			List<Int32BindableObject> sortedList = new List<Int32BindableObject>();
			if (subsidyProvider != null)
			{
				foreach (SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider sp in subsidyProvider)
				{
					sortedList.Add(new Int32BindableObject(sp.Key, sp.LegalEntity.DisplayName.Trim()));
				}
				sortedList.Sort(
				  delegate(Int32BindableObject obj1, Int32BindableObject obj2)
				  {
					  return obj1.Object.ToString().CompareTo(obj2.Object.ToString());
				  });
			}
			ParentSubsidyProvider.Items.Clear();
			ParentSubsidyProvider.DataSource = sortedList;
			ParentSubsidyProvider.DataBind();
			ParentSubsidyProvider.VerifyPleaseSelect();
		}

		/// <summary>
		/// Set selected index on Grid and change RemoveButton Text based on status of Grid Item
		/// </summary>
		/// <param name="selectedGridIndex"></param>
		public void SetSelectedIndexOnGrid(int selectedGridIndex)
		{
			if (selectedGridIndex >= 0 && BulkGrid.Rows.Count > 0)
			{
				BulkGrid.SelectedIndex = selectedGridIndex;
				if (selectedGridIndex < BulkGrid.Rows.Count)
				{
					string num = BulkGrid.SelectedRow.Cells[9].Text.Trim();
					num = num.Replace(" ", "").Replace("R", "");
					if (num.Length == 0)
						num = "0";
					currTransactionAmount.Amount = Convert.ToDouble(num);
					RemoveButton.Text = BulkGrid.SelectedRow.Cells[2].Text.Equals("4") ? "Reinstate" : "Remove";
				}
			}
		}

		/// <summary>
		/// Selected Index Change event of BatchStatus
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void BatchStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (OnBatchStatusSelectedIndexChange != null)
				OnBatchStatusSelectedIndexChange(Page, new KeyChangedEventArgs(BatchStatus.SelectedIndex));
		}

		/// <summary>
		/// Selected Index Change event of BatchNumber
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void BatchNumber_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Since this event is not always called on Onit, we need to get the selected value off the forms collection
			if (Request.Form[BatchNumber.UniqueID] != null)
			  BatchNumber.SelectedValue = Request.Form[BatchNumber.UniqueID];

			if (OnBatchNumberSelectedIndexChange != null)
				OnBatchNumberSelectedIndexChange(Page, new KeyChangedEventArgs(BatchNumber.SelectedIndex));
		}

		/// <summary>
		/// Selected Index Change event of Subsidy Type
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SubsidyType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Request.Form[SubsidyType.UniqueID] != null)
				SubsidyType.SelectedValue = Request.Form[SubsidyType.UniqueID];

			if (OnSubsidyTypeSelectedIndexChange != null)
				OnSubsidyTypeSelectedIndexChange(Page, new KeyChangedEventArgs(SubsidyType.SelectedValue));
		}

		/// <summary>
		/// Selected Index Change event of Transaction Status
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TransactionStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Since this event is not always called on Onit, we need to get the selected value off the forms collection
			if (Request.Form[TransactionStatus.UniqueID] != null)
				TransactionStatus.SelectedValue = Request.Form[TransactionStatus.UniqueID];

			if (OnTransactionStatusSelectedIndexChange != null)
				OnTransactionStatusSelectedIndexChange(Page, new KeyChangedEventArgs(TransactionStatus.SelectedIndex));
		}

		/// Transaction Type selected index change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TransactionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Request.Form[TransactionType.UniqueID] != null)
				TransactionType.SelectedValue = Request.Form[TransactionType.UniqueID];

			if (OnTransactionTypeSelectedIndexChange != null)
				OnTransactionTypeSelectedIndexChange(Page, new KeyChangedEventArgs(TransactionType.SelectedIndex));
	  
		}

		/// <summary>
		/// Parent subsidy provider - selected index change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ParentSubsidyProvider_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Request.Form[ParentSubsidyProvider.UniqueID] != null)
				ParentSubsidyProvider.SelectedValue = Request.Form[ParentSubsidyProvider.UniqueID];

			if (OnParentSubsidySelectedIndexChange != null)
				OnParentSubsidySelectedIndexChange(Page, new KeyChangedEventArgs(ParentSubsidyProvider.SelectedIndex));      
		}

		/// <summary>
		/// Next button clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void NextButton_Click(object sender, EventArgs e)
		{
			if (onNextButtonClicked != null)
				onNextButtonClicked(Page, e);
		}
		/// <summary>
		/// Previous button clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void PreviousButton_Click(object sender, EventArgs e)
		{
			if (onPrevButtonClicked != null)
				onPrevButtonClicked(Page, e);
		}

		/// <summary>
		/// Delete button clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			if (Request.Form[BatchNumber.UniqueID] != null)
				BatchNumber.SelectedValue = Request.Form[BatchNumber.UniqueID];

			if (BatchNumber.SelectedIndex > 0)
			{
				if (onDeleteButtonClicked != null)
					onDeleteButtonClicked(Page, new KeyChangedEventArgs(BatchNumber.SelectedIndex));
			}
			else
			{
				ValBatch.Visible = true;
				ValBatch.ErrorMessage = "Please select a batch Number";
			}
		}

		/// <summary>
		/// Find button clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void FindButton_Click(object sender, EventArgs e)
		{
			string accKey = AccountNumber.Text;

			if (accKey.Length > 0)
			{
				if (onFindButtonClicked != null)
					onFindButtonClicked(Page, new KeyChangedEventArgs(accKey));
			}
			else
				SetErrorMessage("Please enter an account to find");
		}

		/// <summary>
		/// Set Error Message
		/// </summary>
		/// <param name="errorMessage"></param>
		public void SetErrorMessage(string errorMessage)
		{
			this.Messages.Add(new Error(errorMessage, errorMessage));
		}
		/// <summary>
		/// Set BatchPost Error Message
		/// </summary>
		/// <param name="errorMessage"></param>
		public void SetBatchPostErrorMessage(string errorMessage)
		{
			this.Messages.Add(new Error(errorMessage, errorMessage));
		}

		/// <summary>
		/// Get Effective Date 
		/// </summary>
		public DateTime? EffectiveDate
		{
			get
			{
				if (dtEffectiveDate.DateIsValid && dtEffectiveDate.Date.HasValue)
					return dtEffectiveDate.Date.Value;
				else
					return null;
			}
			set
			{
				dtEffectiveDate.Date = value;
			}
		}

		/// <summary>
		/// BulkGrid Selected Index Change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void BulkGrid_SelectedIndexChanged(object sender, EventArgs e)
		{
			int gridSelectedGridIndex = 0;
			if (Request.Form["__EVENTTARGET"] != null)
			{
				if (Request.Form["__EVENTTARGET"].Equals(BulkGrid.UniqueID))
				{
					string[] arg = Request.Form["__EVENTARGUMENT"].Split('$');
					if (arg[0].Equals("Select"))
						gridSelectedGridIndex = int.Parse(arg[1]);
				}
			}
			if (OnBulkGridSelectedIndexChange != null)
				OnBulkGridSelectedIndexChange(Page, new KeyChangedEventArgs(gridSelectedGridIndex));      
		}

		/// <summary>
		/// Update button clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void UpdateButton_Click(object sender, EventArgs e)
		{
			if (OnUpdateButtonClicked != null)
				OnUpdateButtonClicked(Page, e);
		}

		/// <summary>
		/// Save Button Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SaveButton_Click(object sender, EventArgs e)
		{
			if (OnSaveButtonClicked != null)
				OnSaveButtonClicked(Page, e);
		}

		/// <summary>
		/// Post Button Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void PostButton_Click(object sender, EventArgs e)
		{
			if (OnPostButtonClicked != null)
				OnPostButtonClicked(Page, e);
		}

		/// <summary>
		/// Cancel Button Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			if (OnCancelButtonClicked != null)
				OnCancelButtonClicked(Page, e);
		}

		/// <summary>
		/// Remove Button Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RemoveButton_Click(object sender, EventArgs e)
		{
			if (OnRemoveButtonClicked != null)
				OnRemoveButtonClicked(Page, e);
		}

		/// <summary>
		/// Hidden Button Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void HiddenButton_Click(object sender, EventArgs e)
		{

		}

		#region EventHandlers
		/// <summary>
		/// SelectedIndexChange event on BatchStatus DropDown
		/// </summary>
		public event KeyChangedEventHandler OnBatchStatusSelectedIndexChange;
		/// <summary>
			/// SelectedIndexChange event on BatchNumber DropDown
		/// </summary>
		public event KeyChangedEventHandler OnBatchNumberSelectedIndexChange;
		/// <summary>
			/// SelectedIndexChange event on Subsidy Type DropDown
		/// </summary>
		public event KeyChangedEventHandler OnSubsidyTypeSelectedIndexChange;
		/// <summary>
		/// Change of Transaction Type
		/// </summary>
		public event KeyChangedEventHandler OnTransactionStatusSelectedIndexChange;
		/// <summary>
		/// Parent Susbidy selected index change
		/// </summary>
		public event KeyChangedEventHandler OnParentSubsidySelectedIndexChange;
		/// <summary>
		/// Next Button Clicked event
		/// </summary>
		public event EventHandler onNextButtonClicked;
		/// <summary>
		/// Previous Button Clicked event
		/// </summary>
		public event EventHandler onPrevButtonClicked;
		/// <summary>
		/// Save button Click event
		/// </summary>
		public event EventHandler OnSaveButtonClicked;
		/// <summary>
		/// Post Button click event
		/// </summary>
		public event EventHandler OnPostButtonClicked;
		/// <summary>
		/// Cancel Button Click event
		/// </summary>
		public event EventHandler OnCancelButtonClicked;
		/// <summary>
		/// Remove Button Click event
		/// </summary>
		public event EventHandler OnRemoveButtonClicked;
		/// <summary>
		/// Delete button Click event
		/// </summary>
		public event KeyChangedEventHandler onDeleteButtonClicked;
		/// <summary>
		/// Find button Click event
		/// </summary>
		public event KeyChangedEventHandler onFindButtonClicked;
		/// <summary>
		/// Update button Click event
		/// </summary>
		public event EventHandler OnUpdateButtonClicked;
		/// <summary>
		/// BulkGrid Selected Index Change
		/// </summary>
		public event KeyChangedEventHandler OnBulkGridSelectedIndexChange;
		/// <summary>
		/// TransactionType selected Index change
		/// </summary>
		public event KeyChangedEventHandler OnTransactionTypeSelectedIndexChange;

		#endregion
	}
}

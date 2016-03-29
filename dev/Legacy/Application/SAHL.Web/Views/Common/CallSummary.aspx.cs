using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CallSummary : SAHLCommonBaseView,ICallSummary
    {

        #region Private Members
		int _legalEntityKey;
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool AddOrUpdateButtonClicked
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form[btnAddOrUpdate.UniqueID]))
                    return false;

                    return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CallSummaryGridSelectFirstRow
        {
            set
            {
                CallSummaryGrid.SelectFirstRow = value;
                CallSummaryGrid.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CallSummaryGridSelectedIndex
        {
            get
            {
                return CallSummaryGrid.SelectedIndexInternal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool StatusDropDownVisible
        {
            set { 
                ddlStatus.Visible  = value;
                StatusTitle.Visible = value;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public bool AddOrUpdateButtonEnabled
		{
			set
			{
				btnAddOrUpdate.Enabled = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set
            {
                btnSubmit.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubmitButtonOnclickAttribute
        {
            set
            {
                 btnSubmit.Attributes["onclick"] = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool AccountDropDownVisible
        {
            set
            {
                ddlAccountNumber.Visible = value;
                AccountNumberTitle.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedAccountNumber
        {
            get
            {
                if (Request.Form[ddlAccountNumber.UniqueID] != null && Request.Form[ddlAccountNumber.UniqueID] != "-select-")
                    return Request.Form[ddlAccountNumber.UniqueID];

                    return "";
            }
            set
            {
                ddlAccountNumber.SelectedValue = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedCategory
        {
            get
            {
                if (Request.Form[ddlCategory.UniqueID] != null && Request.Form[ddlCategory.UniqueID] != "-select-")
                    return Request.Form[ddlCategory.UniqueID];

                    return "";
            }
            set
            {
                ddlCategory.SelectedValue = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedStatus
        {
            get
            {
                if (Request.Form[ddlStatus.UniqueID] != null && Request.Form[ddlStatus.UniqueID] != "-select-")
                    return Request.Form[ddlStatus.UniqueID];

                    return "";
            }
            set
            {
                ddlStatus.SelectedValue = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string SelectedMemoType
        {
            get
            {
                if (Request.Form[ddlQueryType.UniqueID] != null && Request.Form[ddlQueryType.UniqueID] != "-select-")
                    return Request.Form[ddlQueryType.UniqueID];
                
                if (!string.IsNullOrEmpty(ddlQueryType.SelectedValue) && ddlQueryType.SelectedValue != "-select-")
                    return ddlQueryType.SelectedValue;

                    return "";
            }
            set
            {
                ddlQueryType.SelectedValue = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedConsultant
        {
            get
            {
                if (Request.Form[ddlConsultant.UniqueID] != null && Request.Form[ddlConsultant.UniqueID] != "-select-")
                    return Convert.ToInt32 ( Request.Form[ddlConsultant.UniqueID] );
               
                if (!string.IsNullOrEmpty(ddlConsultant.SelectedValue) && ddlConsultant.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlConsultant.SelectedValue);

                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DetailDescription
        {
            get
            {
                if (String.IsNullOrEmpty(Request.Form[txtDetailDescription.UniqueID]))
                    return "";

                    return Request.Form[txtDetailDescription.UniqueID];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShortDescription
        {
            get
            {
                if (String.IsNullOrEmpty(Request.Form[txtShortDescription.UniqueID]))
                    return "";

                    return Request.Form[txtShortDescription.UniqueID];
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public int SetLegalEntityKey
		{
			set
			{
				_legalEntityKey = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public void SetDefaultDates()
        {
            datReminderDate.Date = DateTime.Today;
            DateTime _date = DateTime.Today;
            datExpiryDate.Date = _date.AddDays(1);
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (OnAddUpdateClicked != null)
            {
                OnAddUpdateClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CallSummaryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IHelpDeskQuery helpDeskQuery = e.Row.DataItem as IHelpDeskQuery;

                if (helpDeskQuery != null)
                {
                    if (helpDeskQuery.Memo.GenericKeyType.Key == 1)
                        e.Row.Cells[2].Text = helpDeskQuery.Memo.GenericKey.ToString();
                
                    e.Row.Cells[4].Text = helpDeskQuery.Memo.ADUser.ADUserName;

                    e.Row.Cells[5].Text = helpDeskQuery.Memo.GeneralStatus.Key == (int)GeneralStatuses.Active ? MemoStatus.UnResolved.ToString() : MemoStatus.Resolved.ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CallSummaryGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(CallSummaryGrid.SelectedIndex);
            if (OnGridSelectedIndexChanged != null)
            {
                OnGridSelectedIndexChanged(sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelClicked  != null)
            {
                OnCancelClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitClicked  != null)
            {
                OnSubmitClicked(sender, e);
            }
        }

        #endregion

        #region ICallSummary Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnAddUpdateClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnGridSelectedIndexChanged;

        #endregion

        #region ICallSummary Methods

        /// <summary>
        /// 
        /// </summary>
        public void BindStatusDropDown()
        {
            Dictionary<string, string> statusDict = new Dictionary<string, string>();
            statusDict.Add(((int)MemoStatus.UnResolved).ToString(),MemoStatus.UnResolved.ToString());
            statusDict.Add(((int)MemoStatus.Resolved).ToString(), MemoStatus.Resolved.ToString());

            ddlStatus.DataSource = statusDict;
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataTextField = "Value";
            ddlStatus.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskCategoryList"></param>
		public void BindCategoryDropDown(IList<IHelpDeskCategory> helpDeskCategoryList)
        {
            ddlCategory.DataSource = helpDeskCategoryList;
            ddlCategory.DataValueField = "Key";
            ddlCategory.DataTextField = "Description";
            ddlCategory.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountList"></param>
        public void BindAccountDropDown(IList<IAccount> accountList)
        {
            ddlAccountNumber.DataSource = accountList;
            ddlAccountNumber.DataValueField = "Key";
            ddlAccountNumber.DataTextField = "Key";
            ddlAccountNumber.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryTypeList"></param>
        public void BindQueryTypeDropDown(IList<IGenericKeyType> queryTypeList)
        {
            ddlQueryType.DataSource = queryTypeList;
            ddlQueryType.DataValueField = "Key";
            ddlQueryType.DataTextField = "Description";
            ddlQueryType.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpdeskConsultants"></param>
        public void BindConsultantDropDown(IList<IADUser> helpdeskConsultants)
        {
            ddlConsultant.DataSource = helpdeskConsultants;
            ddlConsultant.DataValueField = "Key";
            ddlConsultant.DataTextField = "ADUserName";
            ddlConsultant.DataBind();

        }

        public void ResetControls()
        {
            ddlCategory.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlQueryType.SelectedIndex = 0;
            ddlAccountNumber.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetupControlsForDisplay()
        {
            lblDetailDescription.Visible = true;
            txtDetailDescription.Visible = false;

            ButtonRow.Visible = false;

            CallSummaryGrid.PostBackType = GridPostBackType.SingleClick;

            //ShortDescription
            txtShortDescription.Visible = false;
            lblShortDescription.Visible = true;

            //Category
            lblCategory.Visible = true;
            ddlCategory.Visible = false;

            //Status
            ddlStatus.Visible = false;
            StatusTitle.Visible = false;

            //Expirydate
            datExpiryDate.Visible = false;
            ExpiryDateTitle.Visible = false;

            //ReminderDate
            datReminderDate.Visible = false;
            ReminderDateTitle.Visible = false;

            //Query Type
            ddlQueryType.Visible = false;
            lblQueryType.Visible = true;

            //Account Number
            ddlAccountNumber.Visible = false;
            AccountNumberTitle.Visible = false;

            btnAddOrUpdate.Visible = false;
            pnlConsultant.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
		public void SetupControlsForAddOrUpdate(bool adding, bool routing)
        {
            lblDetailDescription.Visible = false;
            txtDetailDescription.Visible = true;

            CallSummaryGrid.PostBackType = GridPostBackType.SingleClick;

            ddlQueryType.Enabled = adding;

            ButtonRow.Visible = true;

            //ShortDescription
            txtShortDescription.Visible = true;
            lblShortDescription.Visible = false;

            //Category
            lblCategory.Visible = false;
            ddlCategory.Visible = true;

            //Status
            ddlStatus.Visible = true;

			if (routing)
			{
				//Expirydate
				datExpiryDate.Date = DateTime.Now;
				datExpiryDate.Enabled = false;

				//ReminderDate
				datReminderDate.Date = DateTime.Now;
				datReminderDate.Enabled = false;
			}
			else
			{
				//Expirydate
				datExpiryDate.Visible = true;

				//ReminderDate
				datReminderDate.Visible = true;
			}

            //Query Type
            ddlQueryType.Visible = true;
            lblQueryType.Visible = false;

            //Account Number
            ddlAccountNumber.Visible = true;

            btnAddOrUpdate.Visible = true;

            btnAddOrUpdate.Text = adding ? "Add" : "Update";

            pnlConsultant.Visible = false;

        }

        /// <summary>
        /// 
        /// </summary>
        public void SetupControlsForRouting()
        {
            datExpiryDate.Visible = false;

            //ReminderDate
            datReminderDate.Visible = false;

            ddlStatus.Visible = false;

            pnlConsultant.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryList"></param>
        public void BindGrid(IReadOnlyEventList<IHelpDeskQuery> queryList)
        {
            CallSummaryGrid.Columns.Clear();
            CallSummaryGrid.DataSource = queryList;
            CallSummaryGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            CallSummaryGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(52), HorizontalAlign.Left, true);
            CallSummaryGrid.AddGridBoundColumn("", "Account Number", Unit.Percentage(12), HorizontalAlign.Left, true);
            CallSummaryGrid.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
            CallSummaryGrid.AddGridBoundColumn("", "Allocated To", Unit.Percentage(12), HorizontalAlign.Left, true);
            CallSummaryGrid.AddGridBoundColumn("", "Status", Unit.Percentage(12), HorizontalAlign.Left, true);
            CallSummaryGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskQuery"></param>
        public void BindLabels(IHelpDeskQuery helpDeskQuery)
        {
            lblDetailDescription.Text = helpDeskQuery.Memo.Description;
            lblQueryType.Text = helpDeskQuery.Memo.GenericKeyType.Description;

            lblShortDescription.Text = helpDeskQuery.Description;
            lblCategory.Text = helpDeskQuery.HelpDeskCategory.Description;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearUpdateControls()
        {
            txtDetailDescription.Text = "";
            txtShortDescription.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskQuery"></param>
        public void BindUpdateControls(IHelpDeskQuery helpDeskQuery)
        {
            txtDetailDescription.Text = helpDeskQuery.Memo.Description;

            //ShortDescription
            txtShortDescription.Text = helpDeskQuery.Description;

            //Category
            ddlCategory.SelectedValue = helpDeskQuery.HelpDeskCategory.Key.ToString();

            //Status
            ddlStatus.SelectedValue = helpDeskQuery.Memo.GeneralStatus.Key == (int)GeneralStatuses.Active ? ((int)MemoStatus.UnResolved).ToString() : ((int)MemoStatus.Resolved).ToString();

            //Expirydate
            if (helpDeskQuery.Memo.ExpiryDate != null)
                datExpiryDate.Date = helpDeskQuery.Memo.ExpiryDate;

            //ReminderDate
            if (helpDeskQuery.Memo.ReminderDate != null)
                datReminderDate.Date = helpDeskQuery.Memo.ReminderDate;

            //Query Type
            ddlQueryType.SelectedValue = helpDeskQuery.Memo.GenericKeyType.Key.ToString();

            //Account Number
            ddlAccountNumber.SelectedValue = helpDeskQuery.Memo.GenericKey.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskQuery"></param>
        /// <param name="adding"></param>
        public void PopulateHeldeskRecord(IHelpDeskQuery helpDeskQuery, bool adding)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
            SAHL.Common.BusinessModel.Interfaces.IMemo memo;
            if (helpDeskQuery.Memo == null)
            {
                memo = memoRepo.CreateMemo();
                helpDeskQuery.Memo = memo;
            }
            else
            {
                memo = helpDeskQuery.Memo;
            }
            if (!string.IsNullOrEmpty(Request.Form[ddlQueryType.UniqueID]))
            {
                IGenericKeyType genericKeyType = lookupRepo.GenericKeyType.ObjectDictionary[Request.Form[ddlQueryType.UniqueID]];
                memo.GenericKeyType = genericKeyType;
            }

            if (memo.GenericKeyType.Description == "Account")
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlAccountNumber.UniqueID]))
                {
                    memo.GenericKey = Convert.ToInt32(Request.Form[ddlAccountNumber.UniqueID]);
                }
            }
            else
            {
                memo.GenericKey = _legalEntityKey;
            }
            memo.ADUser = RepositoryFactory.GetRepository<ISecurityRepository>().GetADUserByPrincipal(CurrentPrincipal);
            if (!string.IsNullOrEmpty(Request.Form[txtDetailDescription.UniqueID]))
            {
                memo.Description = Request.Form[txtDetailDescription.UniqueID];
            }
            if (datExpiryDate.DateIsValid)
            {
                memo.ExpiryDate = Convert.ToDateTime(datExpiryDate.Date);
            }
            memo.InsertedDate = DateTime.Now;
            if (datReminderDate.DateIsValid)
            {
                memo.ReminderDate = Convert.ToDateTime(datReminderDate.Date);
            }

            if (!string.IsNullOrEmpty(Request.Form[ddlStatus.UniqueID]))
            {
                memo.GeneralStatus = Request.Form[ddlStatus.UniqueID] == ((int)MemoStatus.UnResolved).ToString() ? lookupRepo.GeneralStatuses[GeneralStatuses.Active] : lookupRepo.GeneralStatuses[GeneralStatuses.Inactive]; 
            }

            if (!string.IsNullOrEmpty(Request.Form[ddlCategory.UniqueID]))
            {
                helpDeskQuery.HelpDeskCategory = lookupRepo.HelpDeskCategories.ObjectDictionary[Request.Form[ddlCategory.UniqueID]]; 
            }
            if (adding)
                helpDeskQuery.InsertDate = DateTime.Now;

            if (memo.GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Inactive))
            {
                helpDeskQuery.ResolvedDate = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(Request.Form[txtShortDescription.UniqueID]))
            {
                helpDeskQuery.Description = Request.Form[txtShortDescription.UniqueID];
            }
        }

        #endregion
    }
}
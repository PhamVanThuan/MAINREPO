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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;



namespace SAHL.Web.Views.Common
{
    public partial class ConditionsUpdate : SAHLCommonBaseView,IConditionsUpdate
    {

        //private enum ViewMode
        //{
        //    View = 1,
        //    Add,
        //    Update,
        //    Route
        //};

        //private ViewMode m_ViewMode;
        //private CallSummaryController m_MyController = null;
        //private bool m_RoutingOn;
        //private int m_ConsultantKey;



        //const string LEGALENTITYDESC = "Legal Entity";
        //const string ACCOUNTDESC = "Account";
        //const string LEGALENTITYFLAG = "L";
        //const string ACCOUNTFLAG = "A";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            // add event handlers
            btnRestoreString.Click += new EventHandler(btnRestoreString_Click);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);


            //if (!ShouldRunPage())
            //    return;


            //ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
            //if (viewSettings.CustomAttributes.Count > 0)
            //{
            //    System.Xml.XmlNode PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");
            //    if (PageStateNode != null)
            //    {
            //        switch (PageStateNode.Value)
            //        {
            //            case "view": m_ViewMode = ViewMode.View; break;
            //            case "add": m_ViewMode = ViewMode.Add; break;
            //            case "route": m_ViewMode = ViewMode.Route; break;
            //            default: throw new Exception("Unknown page state");
            //        }


            //        // odd behavious required on the update/add mode.
            //        // the grid click is enabled in the Add screen and if clicked
            //        // the screen goes to update mode.
            //        // can only be reset by going to the view mode.
            //        if (m_ViewMode == ViewMode.Route)
            //        {
            //            m_ViewMode = ViewMode.Add;
            //            m_RoutingOn = true;
            //        }
            //        else
            //        {
            //            m_RoutingOn = false;
            //        }


            //        // if we are changing from page to page except for the 
            //        // add to update mode transition we want to reset counters
            //        if (m_ViewMode == ViewMode.Add)
            //        {
            //            if ((m_MyController.m_LastViewState == "route" &&
            //                PageStateNode.Value == "add") ||
            //            (m_MyController.m_LastViewState == "add" &&
            //                PageStateNode.Value == "route"))
            //            {
            //                m_MyController.m_GridClickCount = 0;
            //                m_MyController.m_AddOrUpdateCount = 0;
            //            }
            //        }
            //        m_MyController.m_LastViewState = PageStateNode.Value;

            //        if (m_ViewMode == ViewMode.View)
            //        {
            //            m_MyController.m_GridClickCount = 0;
            //            m_MyController.m_AddOrUpdateCount = 0;
            //        }


            //        // this does the transition from add to update if we have had clicks
            //        if (m_ViewMode == ViewMode.Add && m_MyController.m_GridClickCount > 0)
            //            m_ViewMode = ViewMode.Update;


            //    }
            //    m_ConsultantKey = -1;
            //    PageStateNode = viewSettings.CustomAttributes.GetNamedItem("ConsultantKey");
            //    if (PageStateNode != null)
            //        m_ConsultantKey = int.Parse(PageStateNode.Value);
            //    else
            //        throw new Exception("Consultant key not found");


            //}

            //// Grid Postback?
            //if (Request.Form["__EVENTTARGET"] != null)
            //{
            //    if (Request.Form["__EVENTTARGET"].Equals(CallSummaryGrid.UniqueID))
            //    {
            //        string[] arg = Request.Form["__EVENTARGUMENT"].Split('$');
            //        if (arg[0].Equals("Select"))
            //        {
            //            m_MyController.m_SelectedGridIndex = int.Parse(arg[1]);
            //            if (m_ViewMode == ViewMode.Add)
            //            {
            //                m_MyController.m_GridClickCount++;
            //                m_ViewMode = ViewMode.Update;
            //            }
            //        }
            //    }
            //}

            //PopulateLookups();

            //// get then details from the CBO and the data
            //CBO cbo = m_CBONavigator.DataSource;
            //CBO.WorkFlowInstanceInfoRow r = m_CBONavigator.GetRelatedWorkflowMenuRow(m_CBONavigator.SelectedItem);
            //if (r != null)
            //{
            //    long InstanceID = r.InstanceID;
            //    string sLegalEntityKey = r.GenericKey;
            //    int LegalEntityKey = int.Parse(sLegalEntityKey);
            //    m_MyController.RefreshCaseData(LegalEntityKey, InstanceID, base.GetClientMetrics());
            //}
            //else
            //{
            //}

            //if (m_RoutingOn)
            //{
            //    m_MyController.RefreshConsultantData(m_ConsultantKey, base.GetClientMetrics());
            //}


            //ClientSideSetup();
            //BindControls();

            //if (m_RoutingOn)
            //{
            //    BindConsultant();
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!ShouldRunPage())
            //    return;

            //if (!Page.IsPostBack && m_ViewMode == ViewMode.Add || m_ViewMode == ViewMode.Update)
            //    X2LockSelectedInstance();

            //BIndAccountNumber();
            //BindGrid();
            //BindValues();


        }


        /// <summary>
        /// 
        /// </summary>
        protected void BindConsultant()
        {
            //foreach (SAHL.Common.Datasets.CBO.UserOrganisationStructureRow r in m_MyController.m_ConsultantsDT)
            //{
            //    if (SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName() != r.ADUserName)
            //    {
            //        int index = r.ADUserName.IndexOf('\\');
            //        string text = r.ADUserName.Substring(index != -1 ? index + 1 : 0);
            //        string value = r.ADUserName;
            //        ListItem l = new ListItem(text, value);
            //        ddlConsultant.Items.Add(l);
            //    }

            //    //ddlConsultant.DataSource = m_Controller.m_ConsultantsDT;
            //    //ddlConsultant.DataTextField = m_Controller.m_ConsultantsDT.ADUserNameColumn.ToString();
            //    //ddlConsultant.DataValueField = m_Controller.m_ConsultantsDT.ADUserNameColumn.ToString();
            //    //ddlConsultant.DataBind();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindValues()
        {
            //switch (m_ViewMode)
            //{

            //    case ViewMode.Add:

            //        if (!IsPostBack)
            //        {
            //            if (datReminderDate.Date == new DateTime(1900, 1, 1))
            //                datReminderDate.Date = DateTime.Now;
            //            if (datExpiryDate.Date == new DateTime(1900, 1, 1))
            //                datExpiryDate.Date = DateTime.Now.AddDays(1.0);
            //        }


            //        //datReminderDate.Date = DateTime.Now;
            //        //datExpiryDate.Date = DateTime.Now.AddDays(1.0);
            //        datReminderDate.Enabled = true;
            //        datExpiryDate.Enabled = true;


            //        break;

            //    case ViewMode.View:
            //        if (m_MyController.m_SelectedGridIndex == -1 && m_MyController.m_CallSummaryDT.Rows.Count > 0)
            //        {
            //            m_MyController.m_SelectedGridIndex = 0;
            //            CallSummaryGrid.SelectedIndex = 0;
            //        }

            //        if (m_MyController.m_SelectedGridIndex != -1 && m_MyController.m_CallSummaryDT.Rows.Count > 0)
            //        {
            //            HelpDeskView.CallSummaryRow r = m_MyController.m_CallSummaryDT[m_MyController.m_SelectedGridIndex];
            //            lblShortDescription.Text = r.Description;
            //            lblCategory.Text = m_MyController.m_HelpDeskCategoryDT.FindByHelpDeskCategoryKey(r.HelpDeskCategoryKey).Description;
            //            lblQueryType.Text = r.QueryType == ACCOUNTFLAG ? ACCOUNTDESC : LEGALENTITYDESC;
            //            lblDetailDescription.Text = r.Memo;

            //        }
            //        else
            //        {
            //            lblShortDescription.Text = "";
            //            lblCategory.Text = "-";
            //            lblQueryType.Text = "-";
            //            lblDetailDescription.Text = "";
            //        }
            //        break;
            //    case ViewMode.Update:
            //        string tmp = Request.Form[btnAddOrUpdate.UniqueID];
            //        if (Request.Form[btnAddOrUpdate.UniqueID] == null && m_MyController.m_SelectedGridIndex != -1)
            //        {
            //            //short description from the query tabel
            //            HelpDesk.HelpDeskQueryRow r = m_MyController.m_HelpDeskQueryDT[m_MyController.m_SelectedGridIndex];
            //            txtShortDescription.Text = r.Description;

            //            //The category and status
            //            ddlCategory.SelectedValue = r.HelpDeskCategoryKey.ToString();
            //            ddlStatus.SelectedValue = r.AccountMemoStatusKey.ToString();


            //            //detail description from either the Account ot LegalEntity memo table
            //            if (r.QueryType == ACCOUNTFLAG)
            //            {
            //                Account.AccountMemoRow memo = null;
            //                if (!r.IsAccountMemoKeyNull())
            //                    memo = m_MyController.m_AccountMemoDT.FindByAccountMemoKey(r.AccountMemoKey);

            //                //The status 
            //                ddlQueryType.SelectedValue = ACCOUNTDESC;
            //                if (memo != null)
            //                {
            //                    txtDetailDescription.Text = memo.Memo;
            //                    datExpiryDate.Date = memo.ExpiryDate;
            //                    datReminderDate.Date = memo.ReminderDate;
            //                    datExpiryDate.Enabled = true;
            //                    datReminderDate.Enabled = true;
            //                }
            //                else
            //                {
            //                    txtDetailDescription.Text = "";
            //                    datExpiryDate.ClearDate();
            //                    datReminderDate.ClearDate();

            //                    datExpiryDate.Enabled = false;
            //                    datReminderDate.Enabled = false;
            //                }

            //            }
            //            else if (r.QueryType == LEGALENTITYFLAG)
            //            {
            //                LegalEntity.LegalEntityMemoRow memo = null;
            //                if (!r.IsLegalEntityMemoKeyNull())
            //                    memo = m_MyController.m_LegalEntityMemoDT.FindByLegalEntityMemoKey(r.LegalEntityMemoKey);

            //                //The status 
            //                ddlQueryType.SelectedValue = LEGALENTITYDESC;
            //                if (memo != null)
            //                {
            //                    txtDetailDescription.Text = memo.Memo;
            //                    datExpiryDate.Date = memo.ExpiryDate;
            //                    datReminderDate.Date = memo.ReminderDate;
            //                    datExpiryDate.Enabled = true;
            //                    datReminderDate.Enabled = true;
            //                }
            //                else
            //                {
            //                    txtDetailDescription.Text = "";
            //                    datExpiryDate.ClearDate();
            //                    datReminderDate.ClearDate();

            //                    datExpiryDate.Enabled = false;
            //                    datReminderDate.Enabled = false;
            //                }
            //            }
            //            else
            //            {
            //                throw new Exception("CallSummary: Unknown/Invalid QueryType");
            //            }



            //        }
            //        break;
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindGrid()
        {
            //if (m_ViewMode == ViewMode.View)
            //{
            //    CallSummaryGrid.DataSource = m_MyController.m_CallSummaryDT;
            //    CallSummaryGrid.AddGridBoundColumn("HelpDeskQueryKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            //    CallSummaryGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(52), HorizontalAlign.Left, true);
            //    CallSummaryGrid.AddGridBoundColumn("AccountKey", "Account Number", Unit.Percentage(12), HorizontalAlign.Left, true);
            //    CallSummaryGrid.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
            //    CallSummaryGrid.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
            //    CallSummaryGrid.AddGridBoundColumn("UserID", "Allocated To", Unit.Percentage(12), HorizontalAlign.Left, true);
            //    CallSummaryGrid.AddGridBoundColumn("AccountMemoStatus", "Status", Unit.Percentage(12), HorizontalAlign.Left, true);
            //    CallSummaryGrid.DataBind();


            //}
            //else
            //{
            //    CallSummaryGrid.DataSource = m_MyController.m_HelpDeskQueryDT;
            //    CallSummaryGrid.AddGridBoundColumn("HelpDeskQueryKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            //    CallSummaryGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(52), HorizontalAlign.Left, true);
            //    CallSummaryGrid.AddGridBoundColumn("AccountMemoKey", "Account Number", Unit.Percentage(12), HorizontalAlign.Left, true);
            //    CallSummaryGrid.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
            //    CallSummaryGrid.AddGridBoundColumn("UserID", "Allocated To", Unit.Percentage(12), HorizontalAlign.Left, true);
            //    CallSummaryGrid.AddGridBoundColumn("AccountMemoStatusKey", "Status", Unit.Percentage(12), HorizontalAlign.Left, true);
            //    CallSummaryGrid.DataBind();
            //}


            //if (m_MyController.m_CallSummaryDT == null || m_MyController.m_CallSummaryDT.Count < 1)
            //    m_MyController.m_SelectedGridIndex = 1;

            //if (m_MyController.m_CallSummaryDT.Count <= m_MyController.m_SelectedGridIndex)
            //    m_MyController.m_SelectedGridIndex = 0;

            //CallSummaryGrid.SelectedIndex = m_MyController.m_SelectedGridIndex;



        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindCategory()
        {
            //DataView dv = new DataView(m_MyController.m_HelpDeskCategoryDT, "", "Description ASC", DataViewRowState.CurrentRows);

            //ddlCategory.DataSource = dv;
            //ddlCategory.DataTextField = m_MyController.m_HelpDeskCategoryDT.DescriptionColumn.ToString();
            //ddlCategory.DataValueField = m_MyController.m_HelpDeskCategoryDT.HelpDeskCategoryKeyColumn.ToString();
            //ddlCategory.DataBind();


            ///*
            //ddlCategory.DataSource = m_Controller.m_HelpDeskCategoryDT;
            //ddlCategory.DataTextField = m_Controller.m_HelpDeskCategoryDT.DescriptionColumn.ToString();
            //ddlCategory.DataValueField = m_Controller.m_HelpDeskCategoryDT.HelpDeskCategoryKeyColumn.ToString();
            //ddlCategory.DataBind();
            //*/
            //if (m_ViewMode == ViewMode.Add || m_ViewMode == ViewMode.Route)
            //    ddlCategory.VerifyPleaseSelect();

        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindStatus()
        {
            //ddlStatus.DataSource = m_MyController.Lookups.AccountMemoStatus;
            //ddlStatus.DataTextField = m_MyController.Lookups.AccountMemoStatus.DescriptionColumn.ToString();
            //ddlStatus.DataValueField = m_MyController.Lookups.AccountMemoStatus.AccountMemoStatusKeyColumn.ToString();
            //ddlStatus.DataBind();
            //if (m_ViewMode == ViewMode.Add || m_ViewMode == ViewMode.Route)
            //    ddlStatus.VerifyPleaseSelect();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindQueryType()
        {
            //ddlQueryType.Items.Add(LEGALENTITYDESC);
            //ddlQueryType.Items.Add(ACCOUNTDESC);
            //if (m_ViewMode == ViewMode.Add || m_ViewMode == ViewMode.Route)
            //    ddlQueryType.VerifyPleaseSelect();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void BIndAccountNumber()
        {

            //string CurrentValue = Request.Form[ddlAccountNumber.UniqueID];
            //ddlAccountNumber.Visible = false;
            //titAccountNumber.Visible = false;

            ////HelpDesk.HelpDeskQueryRow r = m_Controller.m_HelpDeskQueryDT[m_Controller.m_SelectedGridIndex];
            //if (ddlQueryType.SelectedValue == ACCOUNTDESC)
            //{
            //    ddlAccountNumber.Visible = true;
            //    titAccountNumber.Visible = true;
            //    ddlAccountNumber.DataSource = m_MyController.m_AccountDT;
            //    ddlAccountNumber.DataTextField = m_MyController.m_AccountDT.AccountKeyColumn.ToString();
            //    ddlAccountNumber.DataValueField = m_MyController.m_AccountDT.AccountKeyColumn.ToString();
            //    ddlAccountNumber.DataBind();
            //    if (m_ViewMode == ViewMode.Add || m_ViewMode == ViewMode.Route)
            //        ddlAccountNumber.VerifyPleaseSelect();
            //}

            //if (!string.IsNullOrEmpty(CurrentValue))
            //    ddlAccountNumber.SelectedValue = CurrentValue;

        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindControls()
        {
            //
            //switch (m_ViewMode)
            //{
            //    case ViewMode.View:
            //        //nuthin
            //        break;
            //    case ViewMode.Add:
            //    case ViewMode.Update:
            //        BindCategory();
            //        BindStatus();
            //        BindQueryType();
            //        break;
            //}
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected void ClientSideSetup()
        {
            //switch (m_ViewMode)
            //{
            //    case ViewMode.View:

            //        lblDetailDescription.Visible = true;
            //        //pnlDetailDescription.Visible = true;
            //        txtDetailDescription.Visible = false;
            //        valDetailDescription.Visible = false;

            //        ButtonRow.Visible = false;

            //        CallSummaryGrid.PostBackType = GridPostBackType.SingleClick;


            //        //ShortDescription
            //        titShortDescription.Visible = true;
            //        txtShortDescription.Visible = false;
            //        valShortDescription.Visible = false;
            //        lblShortDescription.Visible = true;

            //        //Category
            //        titCategory.Visible = true;
            //        lblCategory.Visible = true;
            //        ddlCategory.Visible = false;

            //        //Status
            //        titStatus.Visible = false;
            //        ddlStatus.Visible = false;
            //        valStatus.Visible = false;

            //        //Expirydate
            //        titExpiryDate.Visible = false;
            //        //txtExpiryDate.Visible = false;
            //        valExpiryDate.Visible = false;
            //        datExpiryDate.Visible = false;

            //        //ReminderDate
            //        titReminderDate.Visible = false;
            //        //txtReminderDate.Visible = false;
            //        valReminderDate.Visible = false;
            //        datReminderDate.Visible = false;

            //        //Query Type
            //        titQueryType.Visible = true;
            //        ddlQueryType.Visible = false;
            //        lblQueryType.Visible = true;

            //        //Account Number
            //        titAccountNumber.Visible = false;
            //        ddlAccountNumber.Visible = false;

            //        btnAddOrUpdate.Visible = false;

            //        pnlConsultant.Visible = false;

            //        break;
            //    case ViewMode.Update:
            //    case ViewMode.Add:

            //        lblDetailDescription.Visible = false;
            //        //                pnlDetailDescription.Visible = false;
            //        txtDetailDescription.Visible = true;
            //        valDetailDescription.Visible = true;


            //        CallSummaryGrid.PostBackType = GridPostBackType.SingleClick;
            //        if (m_ViewMode == ViewMode.Add)
            //        {
            //            CallSummaryGrid.SelectFirstRow = false;
            //            m_MyController.m_SelectedGridIndex = -1;
            //            CallSummaryGrid.SelectedIndex = -1;
            //            ddlQueryType.Enabled = true;
            //        }
            //        else
            //        {
            //            CallSummaryGrid.SelectFirstRow = true;
            //            ddlQueryType.Enabled = false;
            //        }

            //        ButtonRow.Visible = true;

            //        //ShortDescription
            //        titShortDescription.Visible = true;
            //        txtShortDescription.Visible = true;
            //        valShortDescription.Visible = true;
            //        lblShortDescription.Visible = false;

            //        //Category
            //        titCategory.Visible = true;
            //        lblCategory.Visible = false;
            //        ddlCategory.Visible = true;

            //        //Status
            //        titStatus.Visible = true;
            //        ddlStatus.Visible = true;
            //        valStatus.Visible = true;


            //        //Expirydate
            //        titExpiryDate.Visible = true;
            //        //txtExpiryDate.Visible = true;
            //        valExpiryDate.Visible = true;
            //        datExpiryDate.Visible = true;

            //        //ReminderDate
            //        titReminderDate.Visible = true;
            //        //txtReminderDate.Visible = true;
            //        valReminderDate.Visible = true;
            //        datReminderDate.Visible = true;

            //        //Query Type
            //        titQueryType.Visible = true;
            //        ddlQueryType.Visible = true;
            //        lblQueryType.Visible = false;

            //        //Account Number
            //        titAccountNumber.Visible = true;
            //        ddlAccountNumber.Visible = true;

            //        btnAddOrUpdate.Visible = true;

            //        if (m_ViewMode == ViewMode.Add)
            //            btnAddOrUpdate.Text = "Add";
            //        else
            //            btnAddOrUpdate.Text = "Update";


            //        btnSubmit.Enabled = false;
            //        if (m_MyController.m_AddOrUpdateCount > 0)
            //            btnSubmit.Enabled = true;

            //        if (m_RoutingOn && btnSubmit.Enabled && m_MyController.UnResolvedCount() > 0)
            //        {
            //            btnSubmit.Attributes["onclick"] = "if(!confirmMessage('There are unresolved help desk queries against the workflow instance. These will be updated to resolved. Would you like to proceed.?')) return false";
            //        }
            //        if (!m_RoutingOn && btnSubmit.Enabled && m_MyController.UnResolvedCount() > 0)
            //        {
            //            btnSubmit.Attributes["onclick"] = "if(!confirmMessage('There are unresolved help desk queries against the workflow instance. If you proceed, the workflow instance will remain on your work list. Would you like to proceed.?')) return false";
            //        }


            //        if (m_RoutingOn)
            //        {

            //            titExpiryDate.Visible = false;
            //            valExpiryDate.Visible = false;
            //            datExpiryDate.Visible = false;

            //            //ReminderDate
            //            titReminderDate.Visible = false;
            //            valReminderDate.Visible = false;
            //            datReminderDate.Visible = false;

            //            titStatus.Visible = false;
            //            ddlStatus.Visible = false;
            //            valStatus.Visible = false;

            //            pnlConsultant.Visible = false;
            //            if (m_MyController.m_AddOrUpdateCount > 0)
            //                pnlConsultant.Visible = true;

            //        }
            //        else
            //        {
            //            pnlConsultant.Visible = false;

            //        }



            //        break;


            //}


        }


        //protected bool ValidateAddOrUpdate()
        //{

            //bool BadDates = false;
            //if (valExpiryDate.Visible && (datExpiryDate.DateString == null ||
            //    datExpiryDate.DateString == "" ||
            //    datExpiryDate.DateString.Equals("01/01/1900") ||
            //    Request.Form[datExpiryDate.UniqueID + "YY"].IndexOf("Y") != -1 ||
            //    Request.Form[datExpiryDate.UniqueID + "MM"].IndexOf("M") != -1 ||
            //    Request.Form[datExpiryDate.UniqueID + "DD"].IndexOf("D") != -1
            //    ))
            //{
            //    valExpiryDate.IsValid = false;
            //    valExpiryDate.ErrorMessage = "Please capture the Expiry Date";
            //    BadDates = true;

            //}

            //if (valReminderDate.Visible && (datReminderDate.DateString == null ||
            //    datReminderDate.DateString == "" ||
            //    datReminderDate.DateString.Equals("01/01/1900") ||
            //    Request.Form[datReminderDate.UniqueID + "YY"].IndexOf("Y") != -1 ||
            //    Request.Form[datReminderDate.UniqueID + "MM"].IndexOf("M") != -1 ||
            //    Request.Form[datReminderDate.UniqueID + "DD"].IndexOf("D") != -1
            //    ))
            //{
            //    valReminderDate.IsValid = false;
            //    valReminderDate.ErrorMessage = "Please capture the Reminder Date";
            //    BadDates = true;
            //}

            //if (valDetailDescription.Visible == true && txtDetailDescription.Text == "")
            //{
            //    valDetailDescription.IsValid = false;
            //    valDetailDescription.ErrorMessage = "Please enter a description";
            //    BadDates = true;
            //}



            //if (BadDates)
            //    return false;

            //return true;






            ////if( string.IsNullOrEmpty(datExpiryDate.DateString)   || datExpiryDate.DefaultDate == datExpiryDate.Date )
            ////    valExpiryDate.IsValid = false;

            ////if( string.IsNullOrEmpty(datReminderDate.DateString) || datReminderDate.DefaultDate == datReminderDate.Date )
            ////   valReminderDate.IsValid = false;
        //}

        //protected void PopulateForAdd()
        //{
            //HelpDesk.HelpDeskQueryRow h = m_MyController.m_HelpDeskQueryDT.NewHelpDeskQueryRow();
            //Account.AccountMemoRow a = m_MyController.m_AccountMemoDT.NewAccountMemoRow();
            //LegalEntity.LegalEntityMemoRow l = m_MyController.m_LegalEntityMemoDT.NewLegalEntityMemoRow();

            //h.HelpDeskQueryKey = -1;
            //h.LegalEntityKey = m_MyController.m_LegalEntityKey;
            //h.Description = txtShortDescription.Text;
            //h.AccountMemoStatusKey = m_RoutingOn ? (int)SAHL.Datasets.AccountMemoStatus.Resolved : int.Parse(ddlStatus.SelectedValue);
            //h.QueryType = ddlQueryType.SelectedValue == ACCOUNTDESC ? ACCOUNTFLAG : LEGALENTITYFLAG;
            //h.InsertDate = DateTime.Now;
            //h.ChangeDate = DateTime.Now;
            //h.UserID = SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName();
            //h.InstanceID = m_MyController.m_InstanceID;
            //h.HelpDeskCategoryKey = int.Parse(ddlCategory.SelectedValue);

            //h.SetResolvedDateNull();
            //if (h.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Resolved)
            //    h.ResolvedDate = DateTime.Now;

            //m_MyController.m_HelpDeskQueryDT.AddHelpDeskQueryRow(h);

            //if (ddlQueryType.SelectedValue == LEGALENTITYDESC)
            //{
            //    l.LegalEntityMemoKey = -1;
            //    l.LegalEntityKey = m_MyController.m_LegalEntityKey;
            //    l.InsertDate = DateTime.Now;
            //    l.ReminderDate = m_RoutingOn ? DateTime.Now : datReminderDate.Date;
            //    l.ExpiryDate = m_RoutingOn ? DateTime.Now : datExpiryDate.Date;
            //    l.Memo = txtDetailDescription.Text;
            //    l.UserID = SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName();
            //    l.AccountMemoStatusKey = m_RoutingOn ? (int)SAHL.Datasets.AccountMemoStatus.Resolved : int.Parse(ddlStatus.SelectedValue);

            //    m_MyController.m_LegalEntityMemoDT.AddLegalEntityMemoRow(l);

            //}
            //else if (ddlQueryType.SelectedValue == ACCOUNTDESC)
            //{
            //    a.AccountMemoKey = -1;
            //    a.AccountKey = int.Parse(ddlAccountNumber.SelectedValue);
            //    a.InsertDate = DateTime.Now;
            //    a.ReminderDate = m_RoutingOn ? DateTime.Now : datReminderDate.Date;
            //    a.ExpiryDate = m_RoutingOn ? DateTime.Now : datExpiryDate.Date;
            //    a.Memo = txtDetailDescription.Text;
            //    a.UserID = SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName();
            //    a.AccountMemoStatusKey = m_RoutingOn ? (int)SAHL.Datasets.AccountMemoStatus.Resolved : int.Parse(ddlStatus.SelectedValue);

            //    m_MyController.m_AccountMemoDT.AddAccountMemoRow(a);

            //}
            //else
            //{
            //    throw new Exception("Invalid Value for ddlQueryType.SelectedValue");
            //}
        //}

        //protected void PopulateForUpdate()
        //{
            //HelpDesk.HelpDeskQueryRow h = m_MyController.m_HelpDeskQueryDT[m_MyController.m_SelectedGridIndex];
            //Account.AccountMemoRow a = null;
            //LegalEntity.LegalEntityMemoRow l = null;

            //if (h.QueryType == LEGALENTITYFLAG)
            //{
            //    l = m_MyController.m_LegalEntityMemoDT.FindByLegalEntityMemoKey(h.LegalEntityMemoKey);
            //    if (l == null)
            //        throw new Exception("PopulateForUpdate: Invalid LE key");

            //}
            //else if (h.QueryType == ACCOUNTFLAG)
            //{
            //    a = m_MyController.m_AccountMemoDT.FindByAccountMemoKey(h.AccountMemoKey);
            //    if (a == null)
            //        throw new Exception("PopulateForUpdate: Invalid account key");
            //}
            //else
            //{
            //    throw new Exception("PopulateForUpdate: Ivalid data");
            //}

            //h.Description = txtShortDescription.Text;
            //h.AccountMemoStatusKey = m_RoutingOn ? (int)SAHL.Datasets.AccountMemoStatus.Resolved : int.Parse(ddlStatus.SelectedValue);
            //h.UserID = SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName();
            //h.HelpDeskCategoryKey = int.Parse(ddlCategory.SelectedValue);
            //h.ChangeDate = DateTime.Now;

            //h.SetResolvedDateNull();
            //if (h.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Resolved)
            //    h.ResolvedDate = DateTime.Now;

            //if (l != null)
            //{
            //    l.ReminderDate = m_RoutingOn ? DateTime.Now : datReminderDate.Date;
            //    l.ExpiryDate = m_RoutingOn ? DateTime.Now : datExpiryDate.Date;
            //    l.Memo = txtDetailDescription.Text;
            //    l.UserID = SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName();
            //    l.AccountMemoStatusKey = m_RoutingOn ? (int)SAHL.Datasets.AccountMemoStatus.Resolved : int.Parse(ddlStatus.SelectedValue);


            //}
            //else
            //{
            //    a.ReminderDate = m_RoutingOn ? DateTime.Now : datReminderDate.Date;
            //    a.ExpiryDate = m_RoutingOn ? DateTime.Now : datExpiryDate.Date;
            //    a.Memo = txtDetailDescription.Text;
            //    a.UserID = SAHL.Common.Authentication.Authenticator.GetFullWindowsUserName();
            //    a.AccountMemoStatusKey = m_RoutingOn ? (int)SAHL.Datasets.AccountMemoStatus.Resolved : int.Parse(ddlStatus.SelectedValue);
            //}
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {


            //if (!ValidateAddOrUpdate() || !Page.IsValid)
            //    return;


            //switch (m_ViewMode)
            //{
            //    // case ViewMode.Route:
            //    case ViewMode.Add:
            //        {
            //            //if (m_viewMode == ViewMode.Add)
            //            PopulateForAdd();
            //            //else if (m_viewMode == ViewMode.Route)
            //            //    PopulateForRoute();


            //            HelpDeskRules rule = new HelpDeskRules();
            //            DataSet ds = new DataSet();

            //            if (m_MyController.m_HelpDeskQueryDT.DataSet != null)
            //                m_MyController.m_HelpDeskQueryDT.DataSet.Tables.Clear();
            //            if (m_MyController.m_LegalEntityMemoDT.DataSet != null)
            //                m_MyController.m_LegalEntityMemoDT.DataSet.Tables.Clear();
            //            if (m_MyController.m_AccountMemoDT.DataSet != null)
            //                m_MyController.m_AccountMemoDT.DataSet.Tables.Clear();


            //            ds.Tables.Add(m_MyController.m_HelpDeskQueryDT);
            //            ds.Tables.Add(m_MyController.m_LegalEntityMemoDT);
            //            ds.Tables.Add(m_MyController.m_AccountMemoDT);

            //            if (rule.Validate(RulesBase.ValidationBasis.ModeIsAdd, ds))
            //            {
            //                m_MyController.AddHelpDeskQuery(base.GetClientMetrics());
            //                m_MyController.ClearCaseData();

            //                m_MyController.m_AddOrUpdateCount++;
            //                m_MyController.Navigator.Navigate("Add");

            //            }
            //            else
            //            {
            //                m_MyController.m_HelpDeskQueryDT.RejectChanges();
            //                m_MyController.m_LegalEntityMemoDT.RejectChanges();
            //                m_MyController.m_AccountMemoDT.RejectChanges();
            //                valAddOrUpdate.ErrorMessage = rule.ErrorList;
            //                valAddOrUpdate.IsValid = false;
            //                return;
            //            }
            //        }
            //        break;
            //    case ViewMode.Update:
            //        {

            //            PopulateForUpdate();


            //            HelpDeskRules rule = new HelpDeskRules();
            //            DataSet ds = new DataSet();

            //            if (m_MyController.m_HelpDeskQueryDT.DataSet != null)
            //                m_MyController.m_HelpDeskQueryDT.DataSet.Tables.Clear();
            //            if (m_MyController.m_LegalEntityMemoDT.DataSet != null)
            //                m_MyController.m_LegalEntityMemoDT.DataSet.Tables.Clear();
            //            if (m_MyController.m_AccountMemoDT.DataSet != null)
            //                m_MyController.m_AccountMemoDT.DataSet.Tables.Clear();

            //            ds.Tables.Add(m_MyController.m_HelpDeskQueryDT);
            //            ds.Tables.Add(m_MyController.m_LegalEntityMemoDT);
            //            ds.Tables.Add(m_MyController.m_AccountMemoDT);


            //            if (rule.Validate(RulesBase.ValidationBasis.ModeIsUpdate, ds))
            //            {
            //                m_MyController.UpdateHelpDeskQuery(base.GetClientMetrics());
            //                m_MyController.ClearCaseData();

            //                // we have succesfully done n update and must go back to
            //                // the add mode on page load.
            //                // the submit button must be switched on
            //                m_MyController.m_AddOrUpdateCount++;
            //                m_MyController.m_GridClickCount = 0;
            //                if (m_ViewMode == ViewMode.Update)
            //                    m_MyController.Navigator.Navigate("Add");
            //                else if (m_ViewMode == ViewMode.Route)
            //                    m_MyController.Navigator.Navigate("Route");
            //            }
            //            else
            //            {
            //                m_MyController.m_HelpDeskQueryDT.RejectChanges();
            //                m_MyController.m_LegalEntityMemoDT.RejectChanges();
            //                m_MyController.m_AccountMemoDT.RejectChanges();
            //                valAddOrUpdate.ErrorMessage = rule.ErrorList;
            //                valAddOrUpdate.IsValid = false;
            //                return;
            //            }
            //            break;
            //        }
            //    default:
            //        throw new Exception("btnAddOrUpdate_Click: invalid page state");
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CallSummaryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (m_ViewMode == ViewMode.View)
            //{
            //}
            //else
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        e.Row.Cells[5].Text = m_MyController.Lookups.AccountMemoStatus.FindByAccountMemoStatusKey(int.Parse(e.Row.Cells[5].Text)).Description;

            //        if (!string.IsNullOrEmpty(e.Row.Cells[2].Text) && e.Row.Cells[2].Text != "&nbsp;")
            //        {
            //            SAHL.Datasets.Account.AccountMemoRow row = m_MyController.m_AccountMemoDT.FindByAccountMemoKey(int.Parse(e.Row.Cells[2].Text));
            //            if (row != null)
            //                e.Row.Cells[2].Text = row.AccountKey.ToString();


            //        }
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (m_RoutingOn && (string.IsNullOrEmpty(ddlConsultant.SelectedValue) || ddlConsultant.SelectedValue == "-select-"))
            //{
            //    valConsultant.IsValid = false;
            //    valConsultant.ErrorMessage = "Please select a consultant";
            //    return;
            //}


            //if (m_RoutingOn)
            //{
            //    if (m_MyController.UnResolvedCount() > 0)
            //    {
            //        m_MyController.ResolveAllRelatedQueries(base.GetClientMetrics());
            //        m_MyController.ClearCaseData();
            //    }
            //    Dictionary<string, string> dict = new Dictionary<string, string>();
            //    dict.Add("CurrentConsultant", ddlConsultant.SelectedValue);
            //    X2SubmitLockedActivityAndRefresh(dict); // page unload is going to do this anyhow.
            //}
            //else
            //{
            //    if (m_MyController.UnResolvedCount() > 0)
            //    {
            //        X2CancelLockedActivityAndRemove();
            //    }
            //    else
            //    {
            //        Dictionary<string, string> dict = new Dictionary<string, string>();
            //        dict.Add("CurrentConsultant", ddlConsultant.SelectedValue);
            //        X2SubmitLockedActivityAndRefresh(dict); // page unload is going to do this anyhow.
            //    }
            //}
            //m_MyController.Navigator.Navigate("Submit");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRestoreString_Click(object sender, EventArgs e)
        {
            if (btnRestoreStringClicked != null)
                btnRestoreStringClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAddClicked != null)
                btnAddClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClicked != null)
                btnUpdateClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        /// <summary>
        /// Implements <see cref="IConditionsUpdate.btnRestoreStringClicked"/>.
        /// </summary>
        public event EventHandler btnRestoreStringClicked;
        /// <summary>
        /// Implements <see cref="IConditionsUpdate.btnAddClicked"/>.
        /// </summary>
        public event EventHandler btnAddClicked;
        /// <summary>
        /// Implements <see cref="IConditionsUpdate.btnUpdateClicked"/>.
        /// </summary>
        public event EventHandler btnUpdateClicked;
        /// <summary>
        /// Implements <see cref="IConditionsUpdate.btnCancelClicked"/>.
        /// </summary>
        public event EventHandler btnCancelClicked;
 
    }



}
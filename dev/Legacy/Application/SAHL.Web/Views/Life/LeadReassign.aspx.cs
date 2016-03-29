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
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;


namespace SAHL.Web.Views.Life
{
    public partial class LeadReassign : SAHLCommonBaseView, ILeadReassign
    {
        private string _pageHeading;
        private string _accountHeading;
        private string _parentAccountHeading;
        private bool _displayParentAccount;
        private string _selectedReassignADUserName;
        private bool _allowLeadReassign;
        private bool _adminUser;
        private IList<int> _selectedApplicationKeys;
        private IEventList<SAHL.Common.BusinessModel.Interfaces.IApplication> _searchResults;
        private ILifeRepository lifeRepo;
        private IX2Repository x2Repo;

        private string _searchAccountKey;
        private OfferStatuses _searchApplicationStatus;
        private string _searchConsultant;
        private string _searchClientName;
        private bool _displayConfirmationPanel;

        private enum GridColumnPositions
        {
            Select = 0,
            ApplicationKey = 1,
            AccountKey = 2,
            LoanAccountKey = 3,
            Name = 4,
            ApplicationStatus = 5,
            AccountStatus = 6,
            LoanAccountStatus = 7,
            Consultant = 8
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
               return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;

            RegisterClientJavascript();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;

            btnReassignLeads.Visible = _allowLeadReassign;
            lblConsultant.Visible = _allowLeadReassign;
            ddlConsultant.Visible = _allowLeadReassign;

            lblHeading1.Text = _pageHeading;
            lblAccountNumber.Text = _accountHeading;

            if (_displayConfirmationPanel)
            {
                pnlConfirmation.Visible = true;
                pnlReassign.Visible = false;
            }
            else
            {
                pnlConfirmation.Visible = false;
                pnlReassign.Visible = true;
            }
        }

        #region ILeadReassign Members

        /// <summary>
        /// 
        /// </summary>
        public string PageHeading
        {
            get { return _pageHeading; }
            set { _pageHeading = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccountHeading
        {
            get { return _accountHeading; }
            set { _accountHeading = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisplayParentAccount
        {
            get { return _displayParentAccount; }
            set { _displayParentAccount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParentAccountHeading
        {
            get { return _parentAccountHeading; }
            set { _parentAccountHeading = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SearchAccountKey
        {
            get { return _searchAccountKey; }
            set { _searchAccountKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public OfferStatuses SearchApplicationStatus
        {
            get { return _searchApplicationStatus; }
            set { _searchApplicationStatus = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SearchConsultant
        {
            get { return _searchConsultant; }
            set { _searchConsultant = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SearchClientName
        {
            get { return _searchClientName; }
            set { _searchClientName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SelectedReassignADUserName
        {
            get { return _selectedReassignADUserName; }
            set { _selectedReassignADUserName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<int> SelectedApplicationKeys
        {
            get { return _selectedApplicationKeys; }
            set { _selectedApplicationKeys = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxSearchResults
        {
            get { return Int32.Parse(selMaxResults.SelectedValue); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowLeadReassign
        {
            get { return _allowLeadReassign; }
            set { _allowLeadReassign = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisplayConfirmationPanel
        {
            get { return _displayConfirmationPanel; }
            set { _displayConfirmationPanel = value; }
        }
	
        /// <summary>
        /// 
        /// </summary>
        public IEventList<SAHL.Common.BusinessModel.Interfaces.IApplication> SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AdminUser
        {
            get { return _adminUser; }
            set { _adminUser = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnReassignButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnConfirmationButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstApplicationStatuses"></param>
        public void BindApplicationStatuses(IEventList<IApplicationStatus> lstApplicationStatuses)
        {
            foreach (IApplicationStatus status in lstApplicationStatuses)
            {
                if (status.Key==(int)SAHL.Common.Globals.OfferStatuses.Open || status.Key==(int)SAHL.Common.Globals.OfferStatuses.NTU)
                    ddlApplicationStatus.Items.Add(new ListItem(status.Description, status.Key.ToString()));
            }

            ddlApplicationStatus.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstConsultantsSearch">list of consultants to search for</param>
        /// <param name="lstConsultantsReassign">list of consultants to reassign to</param>
        public void BindConsultants(IList<IADUser> lstConsultantsSearch, IList<IADUser> lstConsultantsReassign)
        {
            // Bind the consultants to search for
            if (_adminUser)
                ddlSearchConsultant.Items.Add(new ListItem("All", "All"));

            foreach (IADUser aduser in lstConsultantsSearch)
            {
                ddlSearchConsultant.Items.Add(new ListItem(aduser.LegalEntity.FirstNames + " " + aduser.LegalEntity.Surname, aduser.ADUserName));
            }

            ddlSearchConsultant.DataBind();

            // Bind the consultants to reassign to
            foreach (IADUser aduser in lstConsultantsReassign)
            {
                ddlConsultant.Items.Add(new ListItem(aduser.LegalEntity.FirstNames + " " + aduser.LegalEntity.Surname, aduser.ADUserName));
            }

            ddlConsultant.DataBind();


        }

        /// <summary>
        /// 
        /// </summary>
        public void BindSearchResults()
        {
            SearchGrid.Columns.Clear();

            // setup the columns
            SearchGrid.AddCheckBoxColumn("chkSelect", "", true, Unit.Percentage(1), HorizontalAlign.Center, true);
            SearchGrid.AddGridBoundColumn("", "ApplicationKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            SearchGrid.AddGridBoundColumn("", _accountHeading, Unit.Percentage(1), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", _parentAccountHeading, Unit.Percentage(1), HorizontalAlign.Left, _displayParentAccount);
            SearchGrid.AddGridBoundColumn("", "Account Legal Name", Unit.Percentage(30), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Application Status", Unit.Percentage(0), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Account Status", Unit.Percentage(0), HorizontalAlign.Left, false);
            SearchGrid.AddGridBoundColumn("", "Loan Account Status", Unit.Percentage(0), HorizontalAlign.Left, false);
            SearchGrid.AddGridBoundColumn("", "Consultant", Unit.Percentage(15), HorizontalAlign.Left, true);

            SearchGrid.DataSource = _searchResults;
            SearchGrid.DataBind();

            if (_searchResults.Count > 0)
                _allowLeadReassign = true;
            else
                _allowLeadReassign = false;

            divMaxResultsError.Visible = (_searchResults.Count > MaxSearchResults);
            lblMaxCount.Text = MaxSearchResults.ToString();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _searchAccountKey = txtAccountNumber.Text;
            if (Convert.ToInt32(ddlApplicationStatus.SelectedValue) == (int)SAHL.Common.Globals.OfferStatuses.Open)
                _searchApplicationStatus = OfferStatuses.Open;
            else
                _searchApplicationStatus = OfferStatuses.NTU;
            _searchConsultant = ddlSearchConsultant.SelectedItem.Value;
            _searchClientName = txtClientName.Text;
           
            OnSearchButtonClicked(sender, e);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReassignLeads_Click(object sender, EventArgs e)
        {
            _allowLeadReassign = true;

            // Get the selected consultant
            if (ddlConsultant.SelectedItem.Value != "-select-")
                _selectedReassignADUserName = Convert.ToString(ddlConsultant.SelectedItem.Value);

            // Get the selected rows via the checkboxes from the GridView control
            _selectedApplicationKeys = new List<int>();
            for (int i = 0; i < SearchGrid.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)SearchGrid.Rows[i].FindControl("chkSelect");
                object o = Page.Request.Form[cb.UniqueID];
                bool isChecked = (o == null) ? false : true;
                if (isChecked)
                {
                    int applicationKey = Convert.ToInt32(SearchGrid.Rows[i].Cells[(int)GridColumnPositions.ApplicationKey].Text);
                    _selectedApplicationKeys.Add(applicationKey);
                }
            }

            lblConfirmationMessage1.Text = "Selected application(s) have been reassigned to : " + _selectedReassignADUserName;
            lblConfirmationMessage2.Text = "Please press the button below to return to the reassign screen.";

            OnReassignButtonClicked(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            OnConfirmationButtonClicked(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (lifeRepo == null)
                    lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                if (x2Repo == null)
                    x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

                bool bEnableSelect = true;

                // Get the Life Application 
                IApplication application = e.Row.DataItem as IApplication;
                // get the life account
                IAccountLifePolicy accountLifePolicy = application.Account as IAccountLifePolicy;
                // get the loan account
                IMortgageLoanAccount mortgageLoanAccount = accountLifePolicy.ParentMortgageLoan as IMortgageLoanAccount;

                // Get the workflow data
                IInstance instance = x2Repo.GetInstanceForGenericKey(application.Key, SAHL.Common.Constants.WorkFlowName.LifeOrigination, SAHL.Common.Constants.WorkFlowProcessName.LifeOrigination);

                // Application Key
                e.Row.Cells[(int)GridColumnPositions.ApplicationKey].Text = application.Key.ToString();

                // Application Account Key
                e.Row.Cells[(int)GridColumnPositions.AccountKey].Text = application.ReservedAccount.Key.ToString();

                // Loan Account Number
                e.Row.Cells[(int)GridColumnPositions.LoanAccountKey].Text = mortgageLoanAccount != null ? mortgageLoanAccount.Key.ToString() : "";
                
                // Account Name
                e.Row.Cells[(int)GridColumnPositions.Name].Text = application.GetLegalName(LegalNameFormat.Full);

                // Application Status
                if (application.ApplicationStatus.Key==(int)SAHL.Common.Globals.OfferStatuses.NTU &&application.Account.AccountStatus.Key==(int)SAHL.Common.Globals.AccountStatuses.Closed)
                    e.Row.Cells[(int)GridColumnPositions.ApplicationStatus].Text = "NTU Archive";
                else
                    e.Row.Cells[(int)GridColumnPositions.ApplicationStatus].Text = application.ApplicationStatus.Description;

                // Life Account Status
                e.Row.Cells[(int)GridColumnPositions.AccountStatus].Text = application.Account.AccountStatus.Description;

                // Loan Account Status
                e.Row.Cells[(int)GridColumnPositions.LoanAccountStatus].Text = mortgageLoanAccount != null ? mortgageLoanAccount.AccountStatus.Description : "";

                switch (application.ApplicationStatus.Key)
	            {
                    case (int)SAHL.Common.Globals.OfferStatuses.Open :
                        bEnableSelect=true;
                        break;
                    case (int)SAHL.Common.Globals.OfferStatuses.NTU :
                        bEnableSelect=true;
                        if (application.Account.AccountStatus.Key==(int)SAHL.Common.Globals.AccountStatuses.Closed)
                        {
                            bEnableSelect = false;
                            e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Application Status is : NTU Archive";
                        }
                        break;
		            default:
                        bEnableSelect=false;
                        e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Application Status is : " + application.ApplicationStatus.Description;
                        break;
	            }

                // Consultant
                //e.Row.Cells[(int)GridColumnPositions.Consultant].Text = application.Consultant == null ? "Unknown" : application.Consultant.ADUserName;
                if (instance.WorkLists.Count > 1)
                {
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = "Multiple";
                    string consultants = "";
                    int cnt = 0;
                    foreach (IWorkList worklist in instance.WorkLists)
                    {
                        if (cnt==0)
                            consultants += worklist.ADUserName;
                        else
                            consultants += "," + worklist.ADUserName;
                        cnt++;
                    }
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Application exists on multiple worklists : " + consultants;
                    bEnableSelect = false;
                }
                else if (instance.WorkLists.Count == 0)
                {
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = "None";
                    bEnableSelect = false;
                }
                else
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = instance.WorkLists[0].ADUserName == null ? "Unknown" : instance.WorkLists[0].ADUserName;

                // if there is no mortgage loan the disable - this should never happen but for bad data
                if (mortgageLoanAccount == null)
                {
                    bEnableSelect = false;
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "There is no Mortgage Loan Account";
                }

                e.Row.Cells[(int)GridColumnPositions.Select].Enabled = bEnableSelect;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ValidateSearch(object source, ServerValidateEventArgs args)
        {
            if (txtAccountNumber.Text.Trim().Length > 0 ||
                 txtClientName.Text.Trim().Length > 0 ||
                 ddlApplicationStatus.SelectedIndex > -1 ||
                 ddlSearchConsultant.SelectedIndex > -1)
            {
                return;
            }
            args.IsValid = false;
        }

        private void RegisterClientJavascript()
        {
            string sFormName = "window." + this.Form.Name;

            StringBuilder sbJavascript = new StringBuilder();

            #region ClearScreen
            sbJavascript = new StringBuilder();
            sbJavascript.AppendLine("function ClearScreen ()");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtAccountNumber.ClientID + "').disabled = false;");
            sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtAccountNumber.ClientID + "').value = '';");
            sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtClientName.ClientID + "').value = '';");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ClearScreen"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ClearScreen", sbJavascript.ToString(), true);
            #endregion
        }



    }
}

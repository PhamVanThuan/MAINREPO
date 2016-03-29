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
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Life
{
    public partial class LeadCreate : SAHLCommonBaseView, ILeadCreate
    {
        private string _selectedConsultant;
        private bool _allowLeadCreate;
        private bool _adminUser;
        private IList<int> _selectedLoanAccountKeys;
        private IEventList<SAHL.Common.BusinessModel.Interfaces.IAccount> _searchResults;
        private ILifeRepository lifeRepo;
        private IControlRepository ctrlRepo;
        private string _searchAccountKey;
        private string _searchFirstNames;
        private string _searchSurname;
        private bool _searchExcludeClosedLoans;
        private IRuleService _svc;

        private enum GridColumnPositions
        {
            Select = 0,
            AccountKey = 1,
            AccountType = 2,
            Name = 3,
            AccountStatus = 4,
            RelatedAccountKey = 5,
            RelatedAccountStatus = 6,
            Consultant = 7
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

            btnCreateLeads.Visible = _allowLeadCreate;
            lblConsultant.Visible = _allowLeadCreate;
            ddlConsultant.Visible = _allowLeadCreate;
        }

        #region ILeadCreate Members

        public string SearchAccountKey
        {
            get { return _searchAccountKey; }
            set { _searchAccountKey = value; }
        }

        public string SearchFirstNames
        {
            get { return _searchFirstNames; }
            set { _searchFirstNames = value; }
        }

        public string SearchSurname
        {
            get { return _searchSurname; }
            set { _searchSurname = value; }
        }

        public bool SearchExcludeClosedLoans
        {
            get { return _searchExcludeClosedLoans; }
            set { _searchExcludeClosedLoans = value; }
        }
	

        /// <summary>
        /// 
        /// </summary>
        public string SelectedConsultant
        {
            get { return _selectedConsultant; }
            set { _selectedConsultant = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<int> SelectedLoanAccountKeys
        {
            get { return _selectedLoanAccountKeys; }
            set { _selectedLoanAccountKeys = value; }
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
        public bool AllowLeadCreate
        {
            get { return _allowLeadCreate; }
            set { _allowLeadCreate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<SAHL.Common.BusinessModel.Interfaces.IAccount> SearchResults
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
        public event EventHandler OnCreateButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstConsultants"></param>
        public void BindConsultants(IList<IADUser> lstConsultants)
        {
            foreach (IADUser aduser in lstConsultants)
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
            SearchGrid.AddCheckBoxColumn("chkSelect", "", true, Unit.Percentage(5), HorizontalAlign.Center, true);
            SearchGrid.AddGridBoundColumn("", "Loan Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Type", Unit.Percentage(10), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Account Legal Name", Unit.Percentage(30), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Loan Status", Unit.Percentage(15), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Policy Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Policy Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Consultant", Unit.Percentage(30), HorizontalAlign.Left, true);

            SearchGrid.DataSource = _searchResults;
            SearchGrid.DataBind();

            if (_searchResults.Count > 0) 
                _allowLeadCreate = true;
            else
                _allowLeadCreate = false;

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
            _searchFirstNames = txtFirstnames.Text;
            _searchSurname = txtSurname.Text;
            _searchExcludeClosedLoans = false;

            OnSearchButtonClicked(sender, e);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateLeads_Click(object sender, EventArgs e)
        {
            _allowLeadCreate = true;

            // Get the selected consultant
            if (ddlConsultant.SelectedItem.Value != "-select-")
                _selectedConsultant = Convert.ToString(ddlConsultant.SelectedItem.Value);

            // Get the selected rows via the checkboxes from the GridView control
            _selectedLoanAccountKeys = new List<int>();
            for (int i = 0; i < SearchGrid.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)SearchGrid.Rows[i].FindControl("chkSelect");
                object o = Page.Request.Form[cb.UniqueID];
                bool isChecked = (o == null) ? false : true;
                if (isChecked)
                {
                    int loanAccountKey = Convert.ToInt32(SearchGrid.Rows[i].Cells[(int)GridColumnPositions.AccountKey].Text);
                    _selectedLoanAccountKeys.Add(loanAccountKey);
                }
            }

            OnCreateButtonClicked(sender, e);
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
        protected void SearchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (lifeRepo==null)
                    lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

                if (_svc == null)
                    _svc = ServiceFactory.GetService<IRuleService>();

                IDomainMessageCollection tempMessages = new DomainMessageCollection();
                bool bEnableSelect = true;

                // Get the Mortgage Loan Account Row
                IMortgageLoanAccount mortgageLoanAccount = e.Row.DataItem as IMortgageLoanAccount;


                _svc.ExecuteRule(tempMessages, "LifeApplicationCheckMonthsInArrears", mortgageLoanAccount);
                if (tempMessages.Count > 0)
                {
                    bEnableSelect = false;
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = tempMessages[0].Message;
                }

                // Loan Account Key
                e.Row.Cells[(int)GridColumnPositions.AccountKey].Text = mortgageLoanAccount.Key.ToString();

                // Product
                e.Row.Cells[(int)GridColumnPositions.AccountType].Text = mortgageLoanAccount.Product.Description;

                // Account Status
                e.Row.Cells[(int)GridColumnPositions.AccountStatus].Text = mortgageLoanAccount.AccountStatus.Description;

                // Account Name
                e.Row.Cells[(int)GridColumnPositions.Name].Text = mortgageLoanAccount.GetLegalName(LegalNameFormat.InitialsOnly);
                
                if (mortgageLoanAccount.LifePolicyAccount != null)
                {
                    IApplicationLife currentLifeApplication = mortgageLoanAccount.LifePolicyAccount.CurrentLifeApplication;

                    // Policy Number
                    e.Row.Cells[(int)GridColumnPositions.RelatedAccountKey].Text = mortgageLoanAccount.LifePolicyAccount.Key.ToString();

                    // Consultant
                    string consultant = "";
                    if (currentLifeApplication != null)
                    {
                        IADUser aduser = currentLifeApplication.Consultant;
                        consultant = aduser == null ? "Unknown" : aduser.ADUserName;
                    }
                    e.Row.Cells[(int)GridColumnPositions.Consultant].Text = consultant;

                    // Status
                    e.Row.Cells[(int)GridColumnPositions.RelatedAccountStatus].Text = mortgageLoanAccount.LifePolicyAccount.LifePolicy != null ? mortgageLoanAccount.LifePolicyAccount.LifePolicy.LifePolicyStatus.Description : (currentLifeApplication != null ? "Prospect" : "");

                    if (currentLifeApplication != null)
                    {
                        switch (currentLifeApplication.ApplicationStatus.Key)
                        {
                            case (int)SAHL.Common.Globals.OfferStatuses.Open:
                                bEnableSelect = false;
                                e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "An open life application already exists";
                                break;
                            case (int)SAHL.Common.Globals.OfferStatuses.Accepted:
                                if (currentLifeApplication.Account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open)
                                {
                                    bEnableSelect = false;
                                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "An open/accepted life policy already exists";
                                }
                                break;
                            case (int)SAHL.Common.Globals.OfferStatuses.NTU:
                            case (int)SAHL.Common.Globals.OfferStatuses.Declined:
                                if (mortgageLoanAccount.LifePolicyAccount.AccountStatus.Key != (int)SAHL.Common.Globals.AccountStatuses.Closed)
                                {
                                    bEnableSelect = false;
                                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Application NTU'd but not archived";
                                }
                                else
                                    e.Row.Cells[(int)GridColumnPositions.RelatedAccountStatus].Text = "NTU Archive";

                                break;
                        }

                        if (!_adminUser && this.CurrentPrincipal.Identity.Name.ToLower() != (currentLifeApplication.Consultant != null ? currentLifeApplication.Consultant.ADUserName.ToLower():""))
                            bEnableSelect = false;
                    }
                }

                switch (mortgageLoanAccount.AccountStatus.Key)
                {
                    case (int)SAHL.Common.Globals.AccountStatuses.Closed:
                        bEnableSelect = false;
                        e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Loan is Closed";
                        break;
                    case (int)SAHL.Common.Globals.AccountStatuses.Application:
                    case (int)SAHL.Common.Globals.AccountStatuses.ApplicationpriortoInstructAttorney:                      
                        // check the application to see if the attorney has been instruction
                        bool attorneyInstructed = false;
                        if (mortgageLoanAccount.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Application)
                        {
                            if (mortgageLoanAccount.CurrentMortgageLoanApplication != null)
                            {
                                IApplicationInformation applicationInformation = mortgageLoanAccount.CurrentMortgageLoanApplication.GetLatestApplicationInformation();
                                if (applicationInformation != null)
                                {
                                    if (applicationInformation.ApplicationInformationType.Key == (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer)
                                        attorneyInstructed = true;
                                }
                            }
                        }
                        if (attorneyInstructed == false)
                        {
                            bEnableSelect = false;
                            e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Loan has an Application Prior to Instruct Attorney";
                        }
                        break;
                    case (int)SAHL.Common.Globals.AccountStatuses.Locked:
                    case (int)SAHL.Common.Globals.AccountStatuses.Dormant:
                        bEnableSelect = false;
                        e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Loan is Locked/Dormant";
                        break;
                    default:
                        if (ctrlRepo == null)
                            ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                        int lifeAssuredMaxAge = Convert.ToInt32(ctrlRepo.GetControlByDescription("LifeAssuredMaxAge").ControlNumeric);
                        int maxPoliciesAllowed = Convert.ToInt32(ctrlRepo.GetControlByDescription("LifeGroupExposureMaxPolicies").ControlNumeric);

                        // Check the Qualifying ages & Group Exposure of the Applicants / Suretors
                        bool accountOverExposed = true, allApplicantsOverMaxAge = true;
                        foreach (IRole role in mortgageLoanAccount.Roles)
                        {
                            if (role.RoleType.Key == (int)SAHL.Common.Globals.RoleTypes.MainApplicant || role.RoleType.Key == (int)SAHL.Common.Globals.RoleTypes.Suretor)
                            {
                                if (lifeRepo.IsLifeOverExposed(role.LegalEntity) == false)
                                    accountOverExposed = false;
                                
                                if (role.LegalEntity is ILegalEntityNaturalPerson)
                                {
                                    ILegalEntityNaturalPerson np = role.LegalEntity as ILegalEntityNaturalPerson;
                                    if (np.AgeNextBirthday.HasValue && np.AgeNextBirthday <= lifeAssuredMaxAge)
                                        allApplicantsOverMaxAge = false;
                                }
                            }                           
                        }

                        // If there are no Main Applicants under 65 then disable checkbox
                        if (allApplicantsOverMaxAge == true && bEnableSelect)
                        {
                            bEnableSelect = false;
                            e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "No applicants under " + lifeAssuredMaxAge;
                        }
                        // If all the Main Applicants / Suretors are over exposed then disable checkbox
                        if (accountOverExposed == true && bEnableSelect)
                        {
                            bEnableSelect = false;
                            e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "All applicants already have " + maxPoliciesAllowed + " life policies";
                        }

                        break;
                }

                // The rule below checks both the Account and related Legal Entities
                //// Check if the Loan is under Debt Councelling
                //if (mortgageLoanAccount.UnderDebtCounselling == true && bEnableSelect)
                //{
                //    bEnableSelect = false;
                //    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Loan is under Debt Counselling";
                //}

                // Check if the Legal Entities related to the Account are Under Debt Counselling
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                IDomainMessageCollection dmc = new DomainMessageCollection();
                svc.ExecuteRule(dmc, "LifeApplicationCreateDebtCounselling", mortgageLoanAccount);
                if (dmc.Count > 0)
                {
                    bEnableSelect = false;
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Legal Entities on Loan are Under Debt Counselling";
                }
                
                // Check if the Loan has a Cancellation Registered (251 Detail Type )
                if (mortgageLoanAccount.CancellationRegistered == true && bEnableSelect)
                {
                    bEnableSelect = false;
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Loan has a Cancellation Registered";
                }

                // Check for Regent -- allow consultant to create leads from these 29/01/2008 HD# 38221
                if (lifeRepo.IsRegentLoan(mortgageLoanAccount.Key) == true && bEnableSelect)
                {
                    e.Row.Cells[(int)GridColumnPositions.AccountStatus].Text = "Regent";
                    //bEnableSelect = false;
                    e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "Regent Policy";
                }

                // Check For External Originators -- allow consultant to create leads from these 29/01/2008 HD# 38221
                if (bEnableSelect)
                {
                    switch (mortgageLoanAccount.OriginationSource.Key)
                    {
                        case (int)SAHL.Common.Globals.OriginationSources.Blakes:
                        case (int)SAHL.Common.Globals.OriginationSources.Imperial:
                            //if (String.IsNullOrEmpty(e.Row.Cells[(int)GridColumnPositions.RelatedAccountKey].Text))
                            //    e.Row.Cells[(int)GridColumnPositions.RelatedAccountKey].Text = mortgageLoanAccount.OriginationSource.Description;
                            //bEnableSelect = false;
                            e.Row.Cells[(int)GridColumnPositions.AccountStatus].Text = mortgageLoanAccount.OriginationSource.Description;
                            e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = mortgageLoanAccount.OriginationSource.Description + " Loan";
                            break;
                        case (int)SAHL.Common.Globals.OriginationSources.RCS:
                            if (mortgageLoanAccount.SecuredMortgageLoan == null)
                            {
                                bEnableSelect = false;
                                e.Row.Cells[(int)GridColumnPositions.Select].ToolTip = "RCS Loan has not been registered (no mortgageloan account exists).";
                            }
                            break;
                        default:
                            break;
                    }
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
            if ( txtAccountNumber.Text.Trim().Length > 0 ||
                 txtSurname.Text.Trim().Length > 0 ||
                 txtFirstnames.Text.Trim().Length > 0)
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
            sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtSurname.ClientID + "').value = '';");
            sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtFirstnames.ClientID + "').value = '';");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ClearScreen"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ClearScreen", sbJavascript.ToString(), true);
            #endregion
        }



    }
}

using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// View displaying subsidy details.
    /// </summary>
    public partial class SubsidyDetails : SAHLCommonBaseView, ISubsidyDetails
    {
        private bool _readOnly;
        private bool _showButtons;
        private int _employmentStatusKey;
        private int _subsidyKey = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
            RegisterWebService(ServiceConstants.Employment);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            btnBack.Click += new EventHandler(btnBack_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            lblAccountKey.Visible = ReadOnly;
            lblSubsidyProvider.Visible = ReadOnly;
            lblStopOrderAmt.Visible = ReadOnly;
            lblSalaryNumber.Visible = ReadOnly;
            lblNotch.Visible = ReadOnly;
            lblPayPoint.Visible = ReadOnly;
            lblRank.Visible = ReadOnly;
            lblGEPFMember.Visible = ReadOnly;

            btnSave.Visible = _showButtons;
            btnBack.Visible = _showButtons;

            spanAccountKey.Visible = !ReadOnly;
            txtSubsidyProvider.Visible = !ReadOnly;
            currStopOrder.Visible = !ReadOnly;
            txtSalaryNumber.Visible = !ReadOnly;
            txtRank.Visible = !ReadOnly;
            txtPaypoint.Visible = !ReadOnly;
            txtNotch.Visible = !ReadOnly;
            chkGEPFMember.Visible = !ReadOnly;
            chkGEPFMember.Enabled = false;

            if (_subsidyKey != -1)
            {
                IEmploymentRepository empRep = RepositoryFactory.GetRepository<IEmploymentRepository>();
                ISubsidy subsidy = empRep.GetSubsidyByKey(_subsidyKey);

                // show status label in update and in display modes
                // if the Employment Status is 'Previous' then show the Subsidy Status as 'Inactive'
                // because this is what it is going to be set to
                string subsidyStatusDesc = subsidy.GeneralStatus.Description;
                if (_employmentStatusKey == (int)SAHL.Common.Globals.EmploymentStatuses.Previous)
                    subsidyStatusDesc = Enum.GetName(typeof(SAHL.Common.Globals.GeneralStatuses), SAHL.Common.Globals.GeneralStatuses.Inactive);

                lblStatus.Text = subsidyStatusDesc;

                if (ReadOnly)
                {
                    if (subsidy.Account != null)
                        lblAccountKey.Text = subsidy.Account.Key.ToString();
                    else if (subsidy.Application != null)
                        lblAccountKey.Text = subsidy.Application.Key.ToString();

                    string displayName = subsidy.SubsidyProvider.GEPFAffiliate ? string.Format("{0} (GEPF)", subsidy.SubsidyProvider.LegalEntity.DisplayName) : subsidy.SubsidyProvider.LegalEntity.DisplayName;

                    lblSubsidyProvider.Text = displayName;
                    lblStopOrderAmt.Text = subsidy.StopOrderAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblSalaryNumber.Text = subsidy.SalaryNumber;
                    lblNotch.Text = subsidy.Notch;
                    lblPayPoint.Text = subsidy.Paypoint;
                    lblRank.Text = subsidy.Rank;

                    if (subsidy.SubsidyProvider.GEPFAffiliate)
                    {
                        lblGEPFMember.Text = subsidy.GEPFMember ? "Yes" : "No";
                    }
                    else
                    {
                        lblGEPFMember.Text = "n/a";
                    }
                }
                else
                {
                    if (subsidy.Account != null)
                        ddlAccountKey.SelectedValue = "A_" + subsidy.Account.Key.ToString();
                    else if (subsidy.Application != null)
                        ddlAccountKey.SelectedValue = "O_" + subsidy.Application.Key.ToString();

                    if (acSubsidyProvider.SelectedValue.Length == 0 && subsidy.SubsidyProvider != null)
                    {
                        string displayName = subsidy.SubsidyProvider.GEPFAffiliate ? string.Format("{0} (GEPF)", subsidy.SubsidyProvider.LegalEntity.DisplayName) : subsidy.SubsidyProvider.LegalEntity.DisplayName;

                        txtSubsidyProvider.Text = displayName;
                        acSubsidyProvider.SelectedValue = subsidy.SubsidyProvider.Key.ToString();
                    }
                    currStopOrder.Amount = subsidy.StopOrderAmount;
                    txtSalaryNumber.Text = subsidy.SalaryNumber;
                    txtRank.Text = subsidy.Rank;
                    txtPaypoint.Text = subsidy.Paypoint;
                    txtNotch.Text = subsidy.Notch;

                    if (subsidy.SubsidyProvider.GEPFAffiliate)
                    {
                        chkGEPFMember.Enabled = true;
                        chkGEPFMember.Checked = subsidy.GEPFMember;
                    }
                    else
                    {
                        chkGEPFMember.Checked = false;
                        chkGEPFMember.Enabled = false;
                    }
                }
            }
        }

        #region ISubsidyDetails Members

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.BackButtonClicked"/>.
        /// </summary>
        public event EventHandler BackButtonClicked;

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.CancelButtonClicked"/>.
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.SubsidySelected"/>.
        /// </summary>
        public event KeyChangedEventHandler SubsidySelected;

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.SaveButtonClicked"/>.
        /// </summary>
        public event EventHandler SaveButtonClicked;

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.GetCapturedSubsidy()"/>.
        /// </summary>
        public ISubsidy GetCapturedSubsidy()
        {
            IEmploymentRepository empRep = RepositoryFactory.GetRepository<IEmploymentRepository>();
            ISubsidy subsidy = empRep.GetEmptySubsidy();
            return GetCapturedSubsidy(subsidy);
        }

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.GetCapturedSubsidy(ISubsidy)"/>.
        /// </summary>
        /// <param name="subsidy"></param>
        /// <returns></returns>
        public ISubsidy GetCapturedSubsidy(ISubsidy subsidy)
        {
            IEmploymentRepository empRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

            // use the drop down list account key to try and retrieve an account - if it's null then this means
            // it's still in origination and we must grab an application
            if (ddlAccountKey.SelectedIndex > 0)
            {
                string key = ddlAccountKey.SelectedValue;
                if (key.StartsWith("A_"))
                {
                    key = key.Substring(2);
                    //reset the application subsidy value
                    if (subsidy.Application != null)
                        subsidy.Application = null;

                    // update/set the details of the AccountSubsidy object
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IAccount account = accRepo.GetAccountByKey(Int32.Parse(key));
                    subsidy.Account = account;
                }
                else
                {
                    key = key.Substring(2);
                    //reset the account subsidy value
                    subsidy.Account = null;

                    // update/set the details of the AccountSubsidy object
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication application = appRepo.GetApplicationByKey(Int32.Parse(key));
                    subsidy.Application = application;
                }
            }
            subsidy.Notch = txtNotch.Text;
            subsidy.Paypoint = txtPaypoint.Text;
            subsidy.Rank = txtRank.Text;
            subsidy.SalaryNumber = txtSalaryNumber.Text;
            if (currStopOrder.Amount.HasValue)
                subsidy.StopOrderAmount = currStopOrder.Amount.Value;
            else
                subsidy.StopOrderAmount = 0D;

            if (acSubsidyProvider.SelectedValue.Length > 0)
                subsidy.SubsidyProvider = empRepo.GetSubsidyProviderByKey(Int32.Parse(acSubsidyProvider.SelectedValue));
            else
                subsidy.SubsidyProvider = null;

            if (subsidy.SubsidyProvider == null ||
                !subsidy.SubsidyProvider.GEPFAffiliate)
            {
                subsidy.GEPFMember = false;
            }
            else
            {
                subsidy.GEPFMember = chkGEPFMember.Checked;
            }
            
            return subsidy;
        }

        /// <summary>
        /// Set subsidy dataset
        /// </summary>
        public void SetSubsidy(int subsidyKey)
        {
            _subsidyKey = subsidyKey;
        }

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.ReadOnly"/>
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }

        /// <summary>
        /// Show Status control
        /// </summary>
        public bool ShowStatus
        {
            set 
            { 
                divStatus.Visible = value; 
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ShowButtons
        {
            set 
            { 
                _showButtons = value; 
            }
        }

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.GridVisible"/>.
        /// </summary>
        public bool GridVisible
        {
            get
            {
                return pnlGrid.Visible;
            }
            set
            {
                pnlGrid.Visible = value;
            }
        }

        public int EmploymentStatusKey
        {
            get 
            { 
                return _employmentStatusKey; 
            }
            set 
            { 
                _employmentStatusKey = value; 
            }
        }

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.SaveButtonText"/>.
        /// </summary>
        public string SaveButtonText
        {
            get
            {
                return btnSave.Text;
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                btnSave.Text = value;
            }
        }

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.BindAccounts"/>.
        /// </summary>
        public void BindAccounts(IEventList<IAccount> accounts, IEventList<IApplication> applications)
        {
            ddlAccountKey.Items.Clear();
            ddlAccountKey.VerifyPleaseSelect();
            List<int> accountKeys = new List<int>();
            foreach (IAccount account in accounts)
            {
                if (accountKeys.Contains(account.Key))
                    continue;

                string accountKey = account.Key.ToString();
                ddlAccountKey.Items.Add(new ListItem(accountKey, "A_" + accountKey));
                accountKeys.Add(account.Key);
            }
            foreach (IApplication application in applications)
            {
                int accountKey = application.ReservedAccount.Key;
                if (accountKeys.Contains(accountKey))
                    continue;

                string applicationKey = application.Key.ToString();
                ddlAccountKey.Items.Add(new ListItem(accountKey.ToString() + " *", "O_" + applicationKey));
                accountKeys.Add(accountKey);
            }
        }

        /// <summary>
        /// Implements <see cref="ISubsidyDetails.BindSubsidies"/>.
        /// </summary>
        public void BindSubsidies(IList<ISubsidy> subsidies)
        {
            grdSubsidy.AutoGenerateColumns = false;
            grdSubsidy.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdSubsidy.AddGridBoundColumn("", "Subsidy Provider", Unit.Percentage(30), HorizontalAlign.Left, true);
            grdSubsidy.AddGridBoundColumn("", "Rank", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdSubsidy.AddGridBoundColumn("", "Salary Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdSubsidy.AddGridBoundColumn("", "PayPoint", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdSubsidy.AddGridBoundColumn("", "Notch", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdSubsidy.AddGridBoundColumn("", "Stop Order Amount", Unit.Percentage(30), HorizontalAlign.Left, true);

            grdSubsidy.DataSource = subsidies;
            grdSubsidy.DataBind();
        }

        public void grdSubsidy_RowDataBound(Object s, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            ISubsidy subsidy = e.Row.DataItem as ISubsidy;
            if (e.Row.DataItem != null)
            {
                cells[0].Text = subsidy.Key.ToString();
                cells[1].Text = subsidy.SubsidyProvider.LegalEntity.DisplayName.ToString();
                cells[2].Text = subsidy.Rank == null ? " " : subsidy.Rank.ToString();
                cells[3].Text = subsidy.SalaryNumber == null ? "  " : subsidy.SalaryNumber.ToString();
                cells[4].Text = subsidy.Paypoint == null ? " " : subsidy.Paypoint.ToString();
                cells[5].Text = subsidy.Notch == null ? " " : subsidy.Notch.ToString();
                cells[6].Text = subsidy.StopOrderAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        #endregion ISubsidyDetails Members

        #region Event Handlers

        protected void grdSubsidy_SelectedIndexChanged(object sender, EventArgs e)
        {
            int subsidyKey = Convert.ToInt32(grdSubsidy.SelectedRow.Cells[0].Text);
            this.SetSubsidy(subsidyKey);

            KeyChangedEventArgs eventArgs = new KeyChangedEventArgs(subsidyKey);
            if (SubsidySelected != null)
                SubsidySelected(sender, eventArgs);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveButtonClicked != null)
                SaveButtonClicked(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (BackButtonClicked != null)
                BackButtonClicked(sender, e);
        }

        #endregion Event Handlers
    }
}
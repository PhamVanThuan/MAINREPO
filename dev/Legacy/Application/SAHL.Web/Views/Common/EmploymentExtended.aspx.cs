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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Security;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common 
{
    public partial class EmploymentExtended : SAHLCommonBaseView, IEmploymentExtended
    {

        private bool _confirmedIncomeEnabled;
        private bool _confirmedIncomeReadOnly;
        private bool _confirmedIncomeVisible;
        private bool _monthlyIncomeReadOnly;
        private bool _monthlyIncomeEnabled;
        private bool _confirmedDetailsEnabled;
        private bool _chkVerificationProcessReadOnly;
        private bool _verificationProcessPanelEnabled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            btnBack.Click += new EventHandler(btnBack_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            pnlConfirmed.Authenticate += new SAHLSecurityControlEventHandler(pnlConfirmed_Authenticate);

        }

        public void pnlConfirmed_Authenticate(object source, SAHLSecurityControlEventArgs e)
        {
            if (!SecurityHelper.CheckSecurity(pnlConfirmed.SecurityTag, this))
            {
                e.Cancel = false;
                DisableTextFields(pnlConfirmed);
                pnlConfirmed.Enabled = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            if (MonthlyIncomeReadOnly)
                DisableTextFields(pnlMonthly);

            if (!MonthlyIncomeEnabled)
                pnlMonthly.Enabled = false;

            if (ConfirmedIncomeReadOnly)
                DisableTextFields(pnlConfirmedValues);

            if (!ConfirmedIncomeEnabled)
                pnlConfirmedValues.Enabled = false;

            if (!ConfirmedDetailsEnabled)
                pnlConfirmedDetails.Enabled = false;

            if (!VerificationProcessPanelEnabled)
                chkVerificationProcessPanel.Enabled = false;
        }

        #region IEmploymentExtended Members

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.BackButtonClicked"/>.
        /// </summary>
        public event EventHandler BackButtonClicked;

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.CancelButtonClicked"/>.
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.SaveButtonClicked"/>.
        /// </summary>
        public event EventHandler SaveButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool PanelConfirmedEnabled
        {
            get
            {
                if (!SecurityHelper.CheckSecurity(pnlConfirmed.SecurityTag, this))
                        return false;

                return pnlConfirmed.Enabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool VerificationProcessPanelEnabled
        {
            set { _verificationProcessPanelEnabled = value; }
            get { return _verificationProcessPanelEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmedDetailsEnabled
        {
            get { return _confirmedDetailsEnabled; }
            set { _confirmedDetailsEnabled = value; }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.ConfirmedIncomeEnabled"/>.
        /// </summary>
        public bool ConfirmedIncomeEnabled
        {
            get
            {
                return _confirmedIncomeEnabled;
            }
            set
            {
                _confirmedIncomeEnabled = value;
            }
        }
        /// <summary>
        /// Implements <see cref="IEmploymentExtended.ConfirmedIncomeReadOnly"/>.
        /// </summary>
        public bool ConfirmedIncomeReadOnly
        {
            get
            {
                return _confirmedIncomeReadOnly;
            }
            set
            {
                _confirmedIncomeReadOnly = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.ConfirmedIncomeVisible"/>.
        /// </summary>
        public bool ConfirmedIncomeVisible
        {
            get
            {
                return _confirmedIncomeVisible;
            }
            set
            {
                _confirmedIncomeVisible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.MonthlyIncomeEnabled"/>.
        /// </summary>
        public bool MonthlyIncomeEnabled
        {
            get
            {
                return _monthlyIncomeEnabled;
            }
            set
            {
                _monthlyIncomeEnabled = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.MonthlyIncomeReadOnly"/>.
        /// </summary>
        public bool MonthlyIncomeReadOnly
        {
            get
            {
                return _monthlyIncomeReadOnly;
            }
            set
            {
                _monthlyIncomeReadOnly = value;
            }
        }

        /// <summary>   
        /// Implements <see cref="IEmploymentExtended.BackButtonVisible"/>.
        /// </summary>
        public bool BackButtonVisible
        {
            get
            {
                return btnBack.Visible;
            }
            set
            {
                btnBack.Visible = value;
            }
        }

        /// <summary>   
        /// Implements <see cref="IEmploymentExtended.SaveButtonText"/>.
        /// </summary>
        public string SaveButtonText
        {
            get
            {
                return btnSave.Text;
            }
            set
            {
                btnSave.Text = value;
            }
        }

        /// <summary>   
        /// Implements <see cref="IEmploymentExtended.SaveButtonVisible"/>.
        /// </summary>
        public bool SaveButtonVisible
        {
            get
            {
                return btnSave.Visible;
            }
            set
            {
                btnSave.Visible = value;
            }
        }

        private void DisableTextFields(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                SAHLTextBox tb = c as SAHLTextBox;
                if (tb == null)
                    DisableTextFields(c);
                else
                    tb.ReadOnly = true;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.GetExtendedDetails"/>.
        /// </summary>
        public void GetExtendedDetails(IEmployment employment)
        {
            if (!MonthlyIncomeReadOnly && MonthlyIncomeEnabled)
            {
                employment.BasicIncome = txtMonthBasicIncome.Amount;
                employment.ExtendedEmployment.Commission = txtMonthCommission.Amount;
                employment.ExtendedEmployment.Overtime = txtMonthOvertime.Amount;
                employment.ExtendedEmployment.Shift = txtMonthShift.Amount;
                employment.ExtendedEmployment.Performance = txtMonthPerformance.Amount;
                employment.ExtendedEmployment.Allowances = txtMonthAllowances.Amount;
                employment.ExtendedEmployment.PAYE = txtMonthPAYE.Amount;
                employment.ExtendedEmployment.UIF = txtMonthUIF.Amount;
                employment.ExtendedEmployment.PensionProvident = txtMonthPension.Amount;
                employment.ExtendedEmployment.MedicalAid = txtMonthMedicalAid.Amount;
            }

            // We don't want to read values of the view if the Panel is disabled due to security reasons
            if (PanelConfirmedEnabled)
            {
                if (!ConfirmedIncomeReadOnly && ConfirmedIncomeEnabled)
                {
                    employment.ConfirmedBasicIncome = txtConfBasicIncome.Amount;
                    employment.ExtendedEmployment.ConfirmedCommission = txtConfCommission.Amount;
                    employment.ExtendedEmployment.ConfirmedOvertime = txtConfOvertime.Amount;
                    employment.ExtendedEmployment.ConfirmedShift = txtConfShift.Amount;
                    employment.ExtendedEmployment.ConfirmedPerformance = txtConfPerformance.Amount;
                    employment.ExtendedEmployment.ConfirmedAllowances = txtConfAllowances.Amount;
                    employment.ExtendedEmployment.ConfirmedPAYE = txtConfPAYE.Amount;
                    employment.ExtendedEmployment.ConfirmedUIF = txtConfUIF.Amount;
                    employment.ExtendedEmployment.ConfirmedPensionProvident = txtConfPension.Amount;
                    employment.ExtendedEmployment.ConfirmedMedicalAid = txtConfMedicalAid.Amount;
                }

                // Update Confirmation Details
                if (ConfirmedDetailsEnabled)
                {
                    employment.ConfirmedBy = CurrentPrincipal.Identity.Name;
                    employment.ConfirmedDate = DateTime.Now;
                    employment.ContactPerson = txtContactPerson.Text;
                    employment.ContactPhoneCode = spPhoneNumber.Code;
                    employment.ContactPhoneNumber = spPhoneNumber.Number;
                    employment.Department = txtDepartment.Text;
                    if (!string.IsNullOrEmpty(txtSalaryPayDay.Text))
                        employment.SalaryPaymentDay = Convert.ToInt32(txtSalaryPayDay.Text);
                    else
                        employment.SalaryPaymentDay = null;

                    ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                    if (!string.IsNullOrEmpty(ddlConfirmationSource.SelectedValue) && ddlConfirmationSource.SelectedValue != "-select-")
                    {
                        IEmploymentConfirmationSource employmentConfirmationSource = lookupRepository.EmploymentConfirmationSources.ObjectDictionary[ddlConfirmationSource.SelectedValue];
                        employment.EmploymentConfirmationSource = employmentConfirmationSource;
                    }
                    else
                        employment.EmploymentConfirmationSource = null;

                    string UnionMembership = ddlUnionMembership.SelectedValue;
                    int Key = 0;
                    if (int.TryParse(UnionMembership, out Key))
                    {
                        if (Key == 1)
                        {
                            employment.UnionMember = true;
                        }
                        else if (Key == 2)
                        {
                            employment.UnionMember = false;
                        }
                        else
                        {
                            employment.UnionMember = null;
                        }
                    }
                    else
                    {
                        employment.UnionMember = null;
                    }
                }
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentExtended.SetEmployment(IEmployment)"/>.
        /// </summary>
        public void SetEmployment(IEmployment employment)
        {
            //IExtendedEmployment ext = employment.ExtendedEmployment;
            // Monthly
            txtMonthBasicIncome.Amount = employment.BasicIncome;
 
            // Confirmation Values
            txtConfBasicIncome.Amount = employment.ConfirmedBasicIncome;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employment"></param>
        public void SetExtendedEmployment(IEmployment employment)
        {
            IExtendedEmployment ext = employment.ExtendedEmployment;
            if (ext != null)
            {
                // Monthly
                txtMonthCommission.Amount = ext.Commission;
                txtMonthOvertime.Amount = ext.Overtime;
                txtMonthShift.Amount = ext.Shift;
                txtMonthPerformance.Amount = ext.Performance;
                txtMonthAllowances.Amount = ext.Allowances;
                txtMonthGrossIncome.Amount = employment.MonthlyIncome;
                txtMonthDeductions.Amount = ext.Deductions;
                txtMonthPAYE.Amount = ext.PAYE;
                txtMonthUIF.Amount = ext.UIF;
                txtMonthPension.Amount = ext.PensionProvident;
                txtMonthMedicalAid.Amount = ext.MedicalAid;
                txtMonthNetIncome.Amount = ext.NetIncome;
                // Confirmation Values
                txtConfCommission.Amount = ext.ConfirmedCommission;
                txtConfOvertime.Amount = ext.ConfirmedOvertime;
                txtConfShift.Amount = ext.ConfirmedShift;
                txtConfPerformance.Amount = ext.ConfirmedPerformance;
                txtConfAllowances.Amount = ext.ConfirmedAllowances;
                txtConfGrossIncome.Amount = employment.ConfirmedIncome;
                txtConfDeductions.Amount = ext.ConfirmedDeductions;
                txtConfPAYE.Amount = ext.ConfirmedPAYE;
                txtConfUIF.Amount = ext.ConfirmedUIF;
                txtConfPension.Amount = ext.ConfirmedPensionProvident;
                txtConfMedicalAid.Amount = ext.ConfirmedMedicalAid;
                txtConfNetIncome.Amount = ext.ConfirmedNetIncome;
                txtSalaryPayDay.Text = ext.SalaryPayDay.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employment"></param>
        public void SetConfirmationDisplay(IEmployment employment)
        {
            lblConfirmedBy.Text = string.IsNullOrEmpty(employment.ConfirmedBy) ? "-" : employment.ConfirmedBy;
            lblConfirmedDate.Text = employment.ConfirmedDate.HasValue ? employment.ConfirmedDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblContactPerson.Text = string.IsNullOrEmpty(employment.ContactPerson) ? "-" : employment.ContactPerson;
            lblPhoneNumber.Text = employment.ContactPhoneCode + "-" + employment.ContactPhoneNumber;
            lblDepartment.Text = employment.Department;
            lblConfirmationSource.Text = employment.EmploymentConfirmationSource != null ? employment.EmploymentConfirmationSource.Description : "-";
            lblSalaryPayDay.Text = employment.SalaryPaymentDay != null ? employment.SalaryPaymentDay.ToString() : "-";
            if (employment.UnionMember == null) lblUnionMemberShip.Text = "-";
            if (employment.UnionMember == true) lblUnionMemberShip.Text = "Yes";
            if (employment.UnionMember == false) lblUnionMemberShip.Text = "No";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employment"></param>
        public void SetConfirmationEdit(IEmployment employment)
        {
            lblConfirmedBy.Text = string.IsNullOrEmpty(employment.ConfirmedBy) ? "-" : employment.ConfirmedBy;
            lblConfirmedDate.Text = employment.ConfirmedDate.HasValue ? employment.ConfirmedDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            txtContactPerson.Text = employment.ContactPerson;
            spPhoneNumber.Code = employment.ContactPhoneCode;
            spPhoneNumber.Number = employment.ContactPhoneNumber;
            txtDepartment.Text = employment.Department;
            txtSalaryPayDay.Text = (employment.SalaryPaymentDay == null) ? "" : employment.SalaryPaymentDay.ToString();
            if (employment.EmploymentConfirmationSource != null)
                ddlConfirmationSource.SelectedValue = employment.EmploymentConfirmationSource.Key.ToString();

            if (employment.UnionMember == null)
            {
                ddlUnionMembership.SelectedValue = "0";
                lblUnionMemberShip.Text = "Unknown";
            }
            if (employment.UnionMember == true)
            {
                ddlUnionMembership.SelectedValue = "1";
                lblUnionMemberShip.Text = "Yes";
            }
            if (employment.UnionMember == false)
            {
                ddlUnionMembership.SelectedValue = "2";
                lblUnionMemberShip.Text = "No";
            }
        }

        #endregion

        #region Event handlers

        void btnBack_Click(object sender, EventArgs e)
        {
            if (BackButtonClicked != null)
                BackButtonClicked(sender, e);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveButtonClicked != null)
                SaveButtonClicked(sender, e);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        #endregion

        #region Confirmation Details Section

        // SetUp
        public bool ContactPersonReadOnly
        {
            set
            {
                lblContactPerson.Visible = value;
                txtContactPerson.Visible = !value;
            }
        }

        public bool PhoneNumberReadOnly
        {
            set
            {
                lblPhoneNumber.Visible = value;
                spPhoneNumber.Visible = !value;
            }
        }

        public bool DepartmentReadOnly
        {
            set
            {
                lblDepartment.Visible = value;
                txtDepartment.Visible = !value;
            }
        }

        public bool SalaryPayDayReadOnly
        {
            set
            {
                lblSalaryPayDay.Visible = value;
                txtSalaryPayDay.Visible = !value;
            }
        }

        public bool UnionMemberReadOnly
        {
            set
            {
                lblUnionMemberShip.Visible = value;
                ddlUnionMembership.Visible = !value;
            }
        }

        public bool ConfirmationSourceReadOnly
        {
            set
            {
                lblConfirmationSource.Visible = value;
                ddlConfirmationSource.Visible = !value;
            }
        }

        public bool VerificationProcessReadOnly
        {
            set
            {
                _chkVerificationProcessReadOnly = !value;
            }
        }

        // Bind
        public void BindVerificationProcessList(DataTable dtVerificationProcess)
        {
            chkVerificationProcessList.DataSource = dtVerificationProcess;
            chkVerificationProcessList.DataValueField = "EmploymentVerificationProcessTypeKey";
            chkVerificationProcessList.DataTextField = "Description";
            chkVerificationProcessList.DataBind();
        }

        public void BindConfirmationSourceList(IEventList<IEmploymentConfirmationSource> confirmLst)
        {
            ddlConfirmationSource.DataSource = confirmLst;
            ddlConfirmationSource.DataValueField = "Key";
            ddlConfirmationSource.DataTextField = "Description";
            ddlConfirmationSource.DataBind();
        }

        public void BindUniomMemberShipList()
        {
            ddlUnionMembership.Items.Add(new ListItem("-Please Select-", "0"));
            ddlUnionMembership.Items.Add(new ListItem("Yes", "1"));
            ddlUnionMembership.Items.Add(new ListItem("No", "2"));
        }

        // Events
        protected void chkVerificationProcessList_DataBound(object sender, EventArgs e)
        {
            DataTable dtVerificationProcess = (DataTable)chkVerificationProcessList.DataSource;
            int irow = 0;

            foreach (DataRow dr in dtVerificationProcess.Rows)
            {
                chkVerificationProcessList.Items[irow].Enabled = _chkVerificationProcessReadOnly;
                chkVerificationProcessList.Items[irow].Selected = Convert.ToBoolean(dr["SELECTED"]);
                irow++;
            }
        }

        public List<int> GetVerificationProcessList
        {
            get
            {
                List<int> lstVerification = new List<int>();

                foreach (ListItem item in chkVerificationProcessList.Items)
                {
                    if (item.Selected)
                        lstVerification.Add(Convert.ToInt32(item.Value));
                }
                return lstVerification;
            }
        }

        #endregion

    }
}

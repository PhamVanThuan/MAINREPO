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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// Maintains a list of Applicants (Origination)
    /// </summary>
    public partial class Applicants : SAHLCommonBaseView, IApplicants
    {
        #region IApplicants Interface Private Members

        private bool _buttonsVisible = true;

        private bool _applicantDetailsNaturalPersonVisible = true;
        private bool _applicantDetailsCompanyVisible = true;
        private bool _applicantDetailsVisible = true;

        private bool _removeButtonEnabled = true;
        private bool _addButtonEnabled = true;
        private bool _cancelButtonEnabled = true;

        private int _selectedLegalEntityKey;
        private int _selectedApplicationRoleTypeKey;
        private int _selectedApplicationRoleKey;
        private string _gridHeading;
        private string _infoColumnDescription;

        private bool _linkedDebtCounsellingAccountsWarningMessageVisible = false;
        private string _linkedDebtCounsellingAccountsWarningMessage = string.Empty;

        #endregion

        #region Private Variables
        private enum GridColumns
        {
            LegalEntityKey = 0,
            LegalEntityName = 1,
            IDCompanyNumber = 2,
            ApplicationRoleTypeKey = 3,
            Role = 4,
            RoleKey = 5,
            IncomeContributor = 6
        }
        #endregion

        #region IApplicants Members

        /// <summary>
        /// <see cref="IApplicants.BindOfferApplicantsGrid"/>
        /// </summary>
        /// <param name="ApplicationRoles"></param>
        public void BindOfferApplicantsGrid(IList<IApplicationRole> ApplicationRoles)
        {
            BindGrid(ApplicationRoles);

        }

        /// <summary>
        /// <see cref="IApplicants.BindAccountApplicantsGrid"/>
        /// </summary>
        /// <param name="AccountRoles"></param>
        public void BindAccountApplicantsGrid(IList<IRole> AccountRoles)
        {
            BindGrid(AccountRoles);
        }

        /// <summary>
        /// <see cref="IApplicants.BindLegalEntityDetails"/>
        /// </summary>
        /// <param name="LegalEntity"></param>
        public void BindLegalEntityDetails(ILegalEntity LegalEntity)
        {
            string telFaxFormat = "({0}) {1}";

            switch ((LegalEntityTypes)LegalEntity.LegalEntityType.Key)
            {
                case LegalEntityTypes.NaturalPerson:
                    // Basic Details
                    ILegalEntityNaturalPerson legalEntityNaturalPerson = LegalEntity as ILegalEntityNaturalPerson;

                    lblNaturalLegalEntityName.Text = legalEntityNaturalPerson.DisplayName == null ? "-" : legalEntityNaturalPerson.DisplayName;
                    lblNaturalPreferredName.Text = legalEntityNaturalPerson.PreferredName == null ? "-" : legalEntityNaturalPerson.PreferredName;
                    lblNaturalIDNumber.Text = legalEntityNaturalPerson.IDNumber == null ? "-" : legalEntityNaturalPerson.IDNumber;
                    lblNaturalGender.Text = legalEntityNaturalPerson.Gender == null ? "-" : legalEntityNaturalPerson.Gender.Description;
                    lblNaturalDateOfBirth.Text = legalEntityNaturalPerson.DateOfBirth.HasValue == false ? "-" : legalEntityNaturalPerson.DateOfBirth.Value.ToString(SAHL.Common.Constants.DateFormat);
                    lblNaturalMaritalStatus.Text = legalEntityNaturalPerson.MaritalStatus == null ? "-" : legalEntityNaturalPerson.MaritalStatus.Description;
                    lblNaturalLegalEntityStatus.Text = legalEntityNaturalPerson.LegalEntityStatus == null ? "-" : legalEntityNaturalPerson.LegalEntityStatus.Description;

                    // Contact Details
                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityNaturalPerson.HomePhoneCode == null ? "-" : legalEntityNaturalPerson.HomePhoneCode,
                                                    legalEntityNaturalPerson.HomePhoneNumber == null ? "-" : legalEntityNaturalPerson.HomePhoneNumber);
                    lblNaturalHomePhone.Text = telFaxFormat;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityNaturalPerson.WorkPhoneCode == null ? "-" : legalEntityNaturalPerson.WorkPhoneCode,
                                                    legalEntityNaturalPerson.WorkPhoneNumber == null ? "-" : legalEntityNaturalPerson.WorkPhoneNumber);
                    lblNaturalWorkPhone.Text = telFaxFormat;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityNaturalPerson.FaxCode == null ? "-" : legalEntityNaturalPerson.FaxCode,
                                                    legalEntityNaturalPerson.FaxNumber == null ? "-" : legalEntityNaturalPerson.FaxNumber);
                    lblNaturalFaxNumber.Text = telFaxFormat;

                    lblNaturalCellphone.Text = legalEntityNaturalPerson.CellPhoneNumber == null ? "-" : legalEntityNaturalPerson.CellPhoneNumber;
                    lblNaturalEmailAddress.Text = legalEntityNaturalPerson.EmailAddress == null ? "-" : legalEntityNaturalPerson.EmailAddress;


                    break;
                case LegalEntityTypes.CloseCorporation:

                    ILegalEntityCloseCorporation legalEntityCloseCorporation = LegalEntity as ILegalEntityCloseCorporation;

                    lblCompanyName.Text = legalEntityCloseCorporation.RegisteredName == null ? "-" : legalEntityCloseCorporation.RegisteredName;
                    lblCompanyTradingName.Text = legalEntityCloseCorporation.TradingName == null ? "-" : legalEntityCloseCorporation.TradingName;
                    lblCompanyRegistrationNumber.Text = legalEntityCloseCorporation.RegistrationNumber == null ? "-" : legalEntityCloseCorporation.RegistrationNumber;
                    lblCompanyLegalEntityStatus.Text = legalEntityCloseCorporation.LegalEntityStatus == null ? "-" : legalEntityCloseCorporation.LegalEntityStatus.Description;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityCloseCorporation.WorkPhoneCode == null ? "-" : legalEntityCloseCorporation.WorkPhoneCode,
                                                    legalEntityCloseCorporation.WorkPhoneNumber == null ? "-" : legalEntityCloseCorporation.WorkPhoneNumber);
                    lblCompanyWorkPhone.Text = telFaxFormat;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityCloseCorporation.FaxCode == null ? "-" : legalEntityCloseCorporation.FaxCode,
                                                    legalEntityCloseCorporation.FaxNumber == null ? "-" : legalEntityCloseCorporation.FaxNumber);
                    lblCompanyFaxNumber.Text = telFaxFormat;

                    lblCompanyCellphone.Text = legalEntityCloseCorporation.CellPhoneNumber == null ? "-" : legalEntityCloseCorporation.CellPhoneNumber;
                    lblCompanyEmailAddress.Text = legalEntityCloseCorporation.EmailAddress == null ? "-" : legalEntityCloseCorporation.EmailAddress;

                    break;
                case LegalEntityTypes.Company:

                    ILegalEntityCompany legalEntityCompany = LegalEntity as ILegalEntityCompany;

                    lblCompanyName.Text = legalEntityCompany.RegisteredName == null ? "-" : legalEntityCompany.RegisteredName;
                    lblCompanyTradingName.Text = legalEntityCompany.TradingName == null ? "-" : legalEntityCompany.TradingName;
                    lblCompanyRegistrationNumber.Text = legalEntityCompany.RegistrationNumber == null ? "-" : legalEntityCompany.RegistrationNumber;
                    lblCompanyLegalEntityStatus.Text = legalEntityCompany.LegalEntityStatus == null ? "-" : legalEntityCompany.LegalEntityStatus.Description;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityCompany.WorkPhoneCode == null ? "-" : legalEntityCompany.WorkPhoneCode,
                                                    legalEntityCompany.WorkPhoneNumber == null ? "-" : legalEntityCompany.WorkPhoneNumber);
                    lblCompanyWorkPhone.Text = telFaxFormat;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityCompany.FaxCode == null ? "-" : legalEntityCompany.FaxCode,
                                                    legalEntityCompany.FaxNumber == null ? "-" : legalEntityCompany.FaxNumber);
                    lblCompanyFaxNumber.Text = telFaxFormat;

                    lblCompanyCellphone.Text = legalEntityCompany.CellPhoneNumber == null ? "-" : legalEntityCompany.CellPhoneNumber;
                    lblCompanyEmailAddress.Text = legalEntityCompany.EmailAddress == null ? "-" : legalEntityCompany.EmailAddress;

                    break;
                case LegalEntityTypes.Trust:
                    ILegalEntityTrust legalEntityTrust = LegalEntity as ILegalEntityTrust;

                    lblCompanyName.Text = legalEntityTrust.RegisteredName == null ? "-" : legalEntityTrust.RegisteredName;
                    lblCompanyTradingName.Text = legalEntityTrust.TradingName == null ? "-" : legalEntityTrust.TradingName;
                    lblCompanyRegistrationNumber.Text = legalEntityTrust.RegistrationNumber == null ? "-" : legalEntityTrust.RegistrationNumber;
                    lblCompanyLegalEntityStatus.Text = legalEntityTrust.LegalEntityStatus == null ? "-" : legalEntityTrust.LegalEntityStatus.Description;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityTrust.WorkPhoneCode == null ? "-" : legalEntityTrust.WorkPhoneCode,
                                                    legalEntityTrust.WorkPhoneNumber == null ? "-" : legalEntityTrust.WorkPhoneNumber);
                    lblCompanyWorkPhone.Text = telFaxFormat;

                    telFaxFormat = "({0}) {1}";
                    telFaxFormat = String.Format(telFaxFormat,
                                                    legalEntityTrust.FaxCode == null ? "-" : legalEntityTrust.FaxCode,
                                                    legalEntityTrust.FaxNumber == null ? "-" : legalEntityTrust.FaxNumber);
                    lblCompanyFaxNumber.Text = telFaxFormat;

                    lblCompanyCellphone.Text = legalEntityTrust.CellPhoneNumber == null ? "-" : legalEntityTrust.CellPhoneNumber;
                    lblCompanyEmailAddress.Text = legalEntityTrust.EmailAddress == null ? "-" : legalEntityTrust.EmailAddress;

                    break;
                default:
                    throw new Exception("Legal entity type not supported.");
                    break;
            }
        }

        public int SelectedLegalEntityKey
        {
            get { return _selectedLegalEntityKey; }
            set { _selectedLegalEntityKey = value; }
        }

        public int SelectedApplicationRoleTypeKey
        {
            get { return _selectedApplicationRoleTypeKey; }
            set { _selectedApplicationRoleTypeKey = value; }
        }

        public int SelectedApplicationRoleKey
        {
            get { return _selectedApplicationRoleKey; }
            set { _selectedApplicationRoleKey = value; }
        }
        /// <summary>
        /// <see cref="IApplicants.ApplicantDetailsVisible"/>
        /// </summary>
        public bool ApplicantDetailsVisible
        {
            set { _applicantDetailsVisible = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.ApplicantDetailsCompanyVisible"/>
        /// </summary>
        public bool ApplicantDetailsCompanyVisible
        {
            set { _applicantDetailsCompanyVisible = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.ApplicantDetailsNaturalPersonVisible"/>
        /// </summary>
        public bool ApplicantDetailsNaturalPersonVisible
        {
            set { _applicantDetailsNaturalPersonVisible = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.ButtonsVisible"/>
        /// </summary>
        public bool ButtonsVisible
        {
            set { _buttonsVisible = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.RemoveButtonEnabled"/>
        /// </summary>
        public bool RemoveButtonEnabled
        {
            set { _removeButtonEnabled = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.AddButtonEnabled"/>
        /// </summary>
        public bool AddButtonEnabled
        {
            set { _addButtonEnabled = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.CancelButtonEnabled"/>
        /// </summary>
        public bool CancelButtonEnabled
        {
            set { _cancelButtonEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string GridHeading
        {
            set { _gridHeading = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.LinkedDebtCounsellingAccountsWarningMessageVisible"/>
        /// </summary>
        public bool LinkedDebtCounsellingAccountsWarningMessageVisible
        {
            set { _linkedDebtCounsellingAccountsWarningMessageVisible = value; }
        }

        /// <summary>
        /// Warning message if the legal entity is linked to other debt counselling accounts
        /// </summary>
        public string LinkedDebtCounsellingAccountsWarningMessage
        {
            set { _linkedDebtCounsellingAccountsWarningMessage = value; }
        }

        /// <summary>
        /// The heading for the Account Information Column
        /// </summary>
        public string InformationColumnDescription
        {
            set { _infoColumnDescription = value; }
        }

        /// <summary>
        /// <see cref="IApplicants.OnGridLegalEntitySelected"/>
        /// </summary>
        public event KeyChangedEventHandler OnGridLegalEntitySelected;

        /// <summary>
        /// <see cref="IApplicants.OnAddButtonClicked"/>
        /// </summary>
        public event KeyChangedEventHandler OnAddButtonClicked;

        /// <summary>
        /// <see cref="IApplicants.OnRemoveButtonClicked"/>
        /// </summary>
        public event KeyChangedEventHandler OnRemoveButtonClicked;

        /// <summary>
        /// <see cref="IApplicants.OnCancelButtonClicked"/>
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Raised before the page renders - used to finalize controls status.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (base.ShouldRunPage)
            {

                //Get the Selected Legal Entity and Application Role Type 
                if (grdApplicants.Rows.Count > 0 && grdApplicants.SelectedRow != null)
                {
                    _selectedLegalEntityKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.LegalEntityKey].Text);
                    _selectedApplicationRoleTypeKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.ApplicationRoleTypeKey].Text);
                    _selectedApplicationRoleKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.RoleKey].Text);
                }
            }
            base.OnPreRender(e);

            if (!base.ShouldRunPage)
                return;

            trButtons.Visible = _buttonsVisible;
            trApplicantsDetails.Visible = _applicantDetailsVisible;
            tblCompanyContactDetails.Visible = _applicantDetailsCompanyVisible;
            tblCompanyLegalEntityDetails.Visible = _applicantDetailsCompanyVisible;
            tblNaturalLegalEntityDetails.Visible = _applicantDetailsNaturalPersonVisible;
            tblNaturalContactDetails.Visible = _applicantDetailsNaturalPersonVisible;
            btnCancel.Enabled = _cancelButtonEnabled;
            btnRemove.Enabled = _removeButtonEnabled;
            btnAdd.Enabled = _addButtonEnabled;


            if (!String.IsNullOrEmpty(_infoColumnDescription) && _infoColumnDescription == "Under Debt Counselling" && _buttonsVisible)
            {
                btnRemove.Visible = grdApplicants.SelectedRow.Cells[(int)GridColumns.IncomeContributor].Text == "Yes" ? true : false;
                btnAdd.Visible = grdApplicants.SelectedRow.Cells[(int)GridColumns.IncomeContributor].Text == "Yes" ? false : true;

                trLinkedDebtCounsellingAccountsWarningMessage.Visible = _linkedDebtCounsellingAccountsWarningMessageVisible;
                if (_linkedDebtCounsellingAccountsWarningMessageVisible && btnRemove.Visible)
                    lblLinkedDebtCounsellingAccountsWarningMessage.Text = _linkedDebtCounsellingAccountsWarningMessage;
            }
        }

        #endregion

        private void BindGrid(IList<IApplicationRole> ApplicationRoles)
        {
            grdApplicants.HeaderCaption = _gridHeading;
            grdApplicants.EmptyDataSetMessage = "There are no " + _gridHeading + " defined for this Application.";

            if (ApplicationRoles.Count <= 0)
                btnRemove.Visible = false;

            grdApplicants.Columns.Clear();
            grdApplicants.AddGridBoundColumn("", "LegalEntityKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdApplicants.AddGridBoundColumn("", "Legal Entity Name", Unit.Percentage(35), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "ID/Company Number", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "ApplicationRoleTypeKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdApplicants.AddGridBoundColumn("", "Role", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "RoleKey", Unit.Percentage(0), HorizontalAlign.Left, false); 
            grdApplicants.AddGridBoundColumn("", "Income Contributor", Unit.Percentage(15), HorizontalAlign.Left, true);

            grdApplicants.DataSource = ApplicationRoles;
            grdApplicants.DataBind();
        }

        private void BindGrid(IList<IRole> accRoles)
        {
            grdApplicants.HeaderCaption = _gridHeading;
            grdApplicants.EmptyDataSetMessage = "There are no " + _gridHeading + " defined for this Application.";

            if (accRoles.Count <= 0)
                btnRemove.Visible = false;

            grdApplicants.Columns.Clear();
            grdApplicants.AddGridBoundColumn("", "LegalEntityKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdApplicants.AddGridBoundColumn("", "Legal Entity Name", Unit.Percentage(35), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "ID/Company Number", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "ApplicationRoleTypeKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdApplicants.AddGridBoundColumn("", "Role", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "RoleKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            if (!String.IsNullOrEmpty(_infoColumnDescription))
                grdApplicants.AddGridBoundColumn("", _infoColumnDescription, Unit.Percentage(15), HorizontalAlign.Left, true);

            grdApplicants.DataSource = accRoles;
            grdApplicants.DataBind();
        }

        /// <summary>
        /// Raised for every role bound. Used for formatting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;
            IApplicationRole applicationRole = e.Row.DataItem as IApplicationRole;
            IRole accountRole = e.Row.DataItem as IRole;
            string idCompanyNumber = null;

            #region Offer

            if (applicationRole != null)
            {
                cells[(int)GridColumns.LegalEntityKey].Text = applicationRole.LegalEntity.Key.ToString();
                cells[(int)GridColumns.LegalEntityName].Text = applicationRole.LegalEntity.DisplayName;
                cells[(int)GridColumns.ApplicationRoleTypeKey].Text = applicationRole.ApplicationRoleType.Key.ToString();
                cells[(int)GridColumns.Role].Text = applicationRole.ApplicationRoleType.Description;
                cells[(int)GridColumns.RoleKey].Text = applicationRole.Key.ToString();

                bool incomeContributor = false;
                foreach (IApplicationRoleAttribute aroleAttribute in applicationRole.ApplicationRoleAttributes)
                {
                    if (aroleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                    {
                        incomeContributor = true;
                        break;
                    }
                }
                cells[(int)GridColumns.IncomeContributor].Text = incomeContributor == true ? "Yes" : "No";

                switch ((LegalEntityTypes)applicationRole.LegalEntity.LegalEntityType.Key)
                {
                    case LegalEntityTypes.NaturalPerson:
                        idCompanyNumber = ((ILegalEntityNaturalPerson)applicationRole.LegalEntity).IDNumber;
                        break;

                    case LegalEntityTypes.CloseCorporation:
                        idCompanyNumber = ((ILegalEntityCloseCorporation)applicationRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Company:
                        idCompanyNumber = ((ILegalEntityCompany)applicationRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Trust:
                        idCompanyNumber = ((ILegalEntityTrust)applicationRole.LegalEntity).RegistrationNumber;
                        break;

                    default:
                        break;
                }
                cells[(int)GridColumns.IDCompanyNumber].Text = idCompanyNumber;
            }

            #endregion

            #region Account

            if (accountRole != null)
            {
                cells[(int)GridColumns.LegalEntityKey].Text = accountRole.LegalEntity.Key.ToString();
                cells[(int)GridColumns.LegalEntityName].Text = accountRole.LegalEntity.DisplayName;
                cells[(int)GridColumns.ApplicationRoleTypeKey].Text = accountRole.RoleType.Key.ToString();
                cells[(int)GridColumns.Role].Text = accountRole.RoleType.Description;
                cells[(int)GridColumns.RoleKey].Text = accountRole.Key.ToString();

                if (!String.IsNullOrEmpty(_infoColumnDescription))
                {
                    switch (_infoColumnDescription)
                    {
                        case "Under Debt Counselling":
                            cells[(int)GridColumns.IncomeContributor].Text = accountRole.UnderDebtCounselling(false) == true ? "Yes" : "No";
                            break;
                        default:
                            break;
                    }
                }

                switch ((LegalEntityTypes)accountRole.LegalEntity.LegalEntityType.Key)
                {
                    case LegalEntityTypes.NaturalPerson:
                        idCompanyNumber = ((ILegalEntityNaturalPerson)accountRole.LegalEntity).IDNumber;
                        break;

                    case LegalEntityTypes.CloseCorporation:
                        idCompanyNumber = ((ILegalEntityCloseCorporation)accountRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Company:
                        idCompanyNumber = ((ILegalEntityCompany)accountRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Trust:
                        idCompanyNumber = ((ILegalEntityTrust)accountRole.LegalEntity).RegistrationNumber;
                        break;

                    default:
                        break;
                }
                cells[(int)GridColumns.IDCompanyNumber].Text = idCompanyNumber;
            }

            #endregion
        }

        /// <summary>
        /// Raised when the Cancel button is clicked. Bubble up the event for presenters to handle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Raised when the Remove button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            _selectedLegalEntityKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.LegalEntityKey].Text);
            _selectedApplicationRoleTypeKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.ApplicationRoleTypeKey].Text);
            _selectedApplicationRoleKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.RoleKey].Text);

            OnRemoveButtonClicked(sender, new KeyChangedEventArgs(grdApplicants.SelectedIndex));
        }

        /// <summary>
        /// Raised when the Add button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            _selectedLegalEntityKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.LegalEntityKey].Text);

            OnAddButtonClicked(sender, new KeyChangedEventArgs(grdApplicants.SelectedIndex));
        }

        /// <summary>
        /// Raised when teh user selects a new ro on the grid. Bubble up the event for presenters to handle (passing the index).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicants_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedLegalEntityKey = Convert.ToInt32(grdApplicants.SelectedRow.Cells[(int)GridColumns.LegalEntityKey].Text);
            OnGridLegalEntitySelected(sender, new KeyChangedEventArgs(grdApplicants.SelectedIndex));
        }
    }
}

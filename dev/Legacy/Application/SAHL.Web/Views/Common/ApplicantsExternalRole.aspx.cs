using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    public partial class ApplicantsExternalRole : SAHLCommonBaseView, IApplicantsExternalRole
    {
        #region IApplicants Interface Private Members

        private bool _applicantDetailsNaturalPersonVisible = true;
        private bool _applicantDetailsCompanyVisible = true;
        private bool _applicantDetailsVisible = true;

        private int _selectedLegalEntityKey;
        private int _selectedApplicationRoleTypeKey;
        private string _gridHeading;
        private string _infoColumnDescription;

        #endregion

        #region Private Variables
        private enum GridColumns
        {
            LegalEntityKey = 0,
            LegalEntityName = 1,
            IDCompanyNumber = 2,
            ApplicationRoleTypeKey = 3,
            Role = 4,
            IncomeContributor = 5
        }
        #endregion

        #region IApplicants Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalRoles"></param>
        public void BindExternalRoleApplicantsGrid(IList<IExternalRole> externalRoles)
        {
            grdApplicants.HeaderCaption = _gridHeading;
            grdApplicants.EmptyDataSetMessage = "There are no " + _gridHeading + " defined for this Application.";

            grdApplicants.Columns.Clear();
            grdApplicants.AddGridBoundColumn("", "LegalEntityKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdApplicants.AddGridBoundColumn("", "Legal Entity Name", Unit.Percentage(35), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "ID/Company Number", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdApplicants.AddGridBoundColumn("", "ApplicationRoleTypeKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdApplicants.AddGridBoundColumn("", "Role", Unit.Percentage(15), HorizontalAlign.Left, true);
            if (!String.IsNullOrEmpty(_infoColumnDescription))
                grdApplicants.AddGridBoundColumn("", _infoColumnDescription, Unit.Percentage(15), HorizontalAlign.Left, true);

            grdApplicants.DataSource = externalRoles;
            grdApplicants.DataBind();
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
        /// 
        /// </summary>
        public string GridHeading
        {
            set { _gridHeading = value; }
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
                }
            }
            base.OnPreRender(e);

            if (!base.ShouldRunPage)
                return;

            
            trApplicantsDetails.Visible = _applicantDetailsVisible;
            tblCompanyContactDetails.Visible = _applicantDetailsCompanyVisible;
            tblCompanyLegalEntityDetails.Visible = _applicantDetailsCompanyVisible;
            tblNaturalLegalEntityDetails.Visible = _applicantDetailsNaturalPersonVisible;
            tblNaturalContactDetails.Visible = _applicantDetailsNaturalPersonVisible;

        }

        #endregion


        /// <summary>
        /// Raised for every role bound. Used for formatting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IExternalRole externalRole = e.Row.DataItem as IExternalRole;

            string idCompanyNumber = null;

            if (externalRole != null)
            {
                cells[(int)GridColumns.LegalEntityKey].Text = externalRole.LegalEntity.Key.ToString();
                cells[(int)GridColumns.LegalEntityName].Text = externalRole.LegalEntity.DisplayName;
                cells[(int)GridColumns.ApplicationRoleTypeKey].Text = externalRole.ExternalRoleType.Key.ToString();
                cells[(int)GridColumns.Role].Text = externalRole.ExternalRoleType.Description;


                switch ((LegalEntityTypes)externalRole.LegalEntity.LegalEntityType.Key)
                {
                    case LegalEntityTypes.NaturalPerson:
                        idCompanyNumber = ((ILegalEntityNaturalPerson)externalRole.LegalEntity).IDNumber;
                        break;

                    case LegalEntityTypes.CloseCorporation:
                        idCompanyNumber = ((ILegalEntityCloseCorporation)externalRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Company:
                        idCompanyNumber = ((ILegalEntityCompany)externalRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Trust:
                        idCompanyNumber = ((ILegalEntityTrust)externalRole.LegalEntity).RegistrationNumber;
                        break;

                    default:
                        break;
                }
                cells[(int)GridColumns.IDCompanyNumber].Text = idCompanyNumber;

            }

        }


        /// <summary>
        /// Raised when the user selects a new row on the grid. Bubble up the event for presenters to handle (passing the index).
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
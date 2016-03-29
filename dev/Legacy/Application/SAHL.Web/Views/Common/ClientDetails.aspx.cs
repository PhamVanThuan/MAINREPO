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
using SAHL.Common; 
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common
{
    public partial class ClientDetails : SAHLCommonBaseView, IClientDetails
    {

        #region Private members
        private bool _naturalPersonPanelVisible = true;
        private bool _companyPanelVisible = true;
        private string _addressDescription = String.Empty;
        private string m_Temp;
        #endregion

        #region Protected Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            LENaturalPersonTable.Visible = _naturalPersonPanelVisible;
            pnlContactDetailsPerson.Visible = _naturalPersonPanelVisible;
            pnlContactDetailsCompany.Visible = _companyPanelVisible;
            LECompanyTable.Visible = _companyPanelVisible;
            pnlLEAddress.GroupingText = _addressDescription;
        }
        #endregion

        #region Private Methods

        private void BindNaturalPerson(ILegalEntityNaturalPerson LegalEntity)
        {
            lblNLEIntroductionDate.Text = LegalEntity.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblNLEName.Text = String.Format("{0} {1} {2}",
                                                LegalEntity.Salutation == null ? "" : LegalEntity.Salutation.Description,
                                                LegalEntity.FirstNames == null ? "" : LegalEntity.FirstNames,
                                                LegalEntity.Surname == null ? "" : LegalEntity.Surname);

            if (!(LegalEntity.PreferredName == null || LegalEntity.PreferredName.Trim().Length == 0))
                lblNLEPreferredName.Text = LegalEntity.PreferredName;

            if (!(LegalEntity.IDNumber == null || LegalEntity.IDNumber.Trim().Length == 0))
                lblNLEIDNumber.Text = LegalEntity.IDNumber;

            if (!(LegalEntity.PassportNumber == null || LegalEntity.PassportNumber.Trim().Length == 0))
                lblNLEPassPortNumber.Text = LegalEntity.PassportNumber;

            lblNLEDateOfBirth.Text = !LegalEntity.DateOfBirth.HasValue ? "-" : LegalEntity.DateOfBirth.Value.ToString(SAHL.Common.Constants.DateFormat);

            lblNLEGender.Text = LegalEntity.Gender == null ? "-" : LegalEntity.Gender.Description;

            lblNLEMaritalStatus.Text = LegalEntity.MaritalStatus == null ? "-" : LegalEntity.MaritalStatus.Description;

            lblNLEStatus.Text = LegalEntity.LegalEntityStatus == null ? "-" : LegalEntity.LegalEntityStatus.Description;

            // BindContactInfo(LegalEntity);
            m_Temp = "({0}) {1}";
            m_Temp = String.Format(m_Temp, LegalEntity.HomePhoneCode == null ? "-" : LegalEntity.HomePhoneCode.Trim(), LegalEntity.HomePhoneNumber == null ? "-" : LegalEntity.HomePhoneNumber.Trim());
            if (LegalEntity.HomePhoneNumber == null && LegalEntity.HomePhoneCode == null)
                m_Temp = "-";
            lblNLEHomePhone.Text = m_Temp;

            m_Temp = "({0}) {1}";
            m_Temp = String.Format(m_Temp, LegalEntity.WorkPhoneCode == null ? "-" : LegalEntity.WorkPhoneCode.Trim(), LegalEntity.WorkPhoneNumber == null ? "-" : LegalEntity.WorkPhoneNumber.Trim());
            if (LegalEntity.WorkPhoneNumber == null && LegalEntity.WorkPhoneCode == null)
                m_Temp = "-";
            lblNLEWorkPhone.Text = m_Temp;

            m_Temp = "({0}) {1}";
            m_Temp = String.Format(m_Temp, LegalEntity.FaxCode == null ? "-" : LegalEntity.FaxCode.Trim(), LegalEntity.FaxNumber == null ? "-" : LegalEntity.FaxNumber.Trim());
            if (LegalEntity.FaxCode == null && LegalEntity.FaxNumber == null)
                m_Temp = "-";
            lblNLEFaxNumber.Text = m_Temp;

            lblNLECellphone.Text = String.IsNullOrEmpty(LegalEntity.CellPhoneNumber) ? "-" : LegalEntity.CellPhoneNumber.Trim();

            lblNLEEmailAddress.Text = String.IsNullOrEmpty(LegalEntity.EmailAddress) ? "-" : LegalEntity.EmailAddress.Trim();

        }

        [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength", Justification = "Incorrectly raised.")]
        private void BindCompany(ILegalEntity LegalEntity)
        {
            string registeredName = String.Empty;
            string tradingName = String.Empty;
            string registrationNumber = String.Empty;
            string faxCode = String.Empty;
            string faxNumber = String.Empty;
            string workPhoneCode = String.Empty;
            string workPhoneNumber = String.Empty;
            //string cellPhoneNumber = String.Empty;

            string errorMessage = "Unknown legal entity type.";

            switch ((SAHL.Common.Globals.LegalEntityTypes)LegalEntity.LegalEntityType.Key)
            {
                case SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                    ILegalEntityCloseCorporation legalEntityCloseCorporation = (ILegalEntityCloseCorporation)LegalEntity;

                    registeredName = String.IsNullOrEmpty(legalEntityCloseCorporation.RegisteredName) ? "-" : legalEntityCloseCorporation.RegisteredName;
                    tradingName = String.IsNullOrEmpty(legalEntityCloseCorporation.TradingName) ? "-" : legalEntityCloseCorporation.TradingName;
                    registrationNumber = String.IsNullOrEmpty(legalEntityCloseCorporation.RegistrationNumber) ? "-" : legalEntityCloseCorporation.RegistrationNumber;
                    workPhoneCode = String.IsNullOrEmpty(legalEntityCloseCorporation.WorkPhoneCode) ? "-" : legalEntityCloseCorporation.WorkPhoneCode;
                    workPhoneNumber = String.IsNullOrEmpty(legalEntityCloseCorporation.WorkPhoneNumber) ? "-" : legalEntityCloseCorporation.WorkPhoneNumber;
                    faxCode = String.IsNullOrEmpty(legalEntityCloseCorporation.FaxCode) ? "-" : legalEntityCloseCorporation.FaxCode;
                    faxNumber = String.IsNullOrEmpty(legalEntityCloseCorporation.FaxNumber) ? "-" : legalEntityCloseCorporation.FaxNumber;

                    break;

                case SAHL.Common.Globals.LegalEntityTypes.Company:
                    ILegalEntityCompany legalEntityCompany = (ILegalEntityCompany)LegalEntity;

                    registeredName = String.IsNullOrEmpty(legalEntityCompany.RegisteredName) ? "-" : legalEntityCompany.RegisteredName;
                    tradingName = String.IsNullOrEmpty(legalEntityCompany.TradingName) ? "-" : legalEntityCompany.TradingName;
                    registrationNumber = String.IsNullOrEmpty(legalEntityCompany.RegistrationNumber) ? "-" : legalEntityCompany.RegistrationNumber;
                    workPhoneCode = String.IsNullOrEmpty(legalEntityCompany.WorkPhoneCode) ? "-" : legalEntityCompany.WorkPhoneCode;
                    workPhoneNumber = String.IsNullOrEmpty(legalEntityCompany.WorkPhoneNumber) ? "-" : legalEntityCompany.WorkPhoneNumber;
                    faxCode = String.IsNullOrEmpty(legalEntityCompany.FaxCode) ? "-" : legalEntityCompany.FaxCode;
                    faxNumber = String.IsNullOrEmpty(legalEntityCompany.FaxNumber) ? "-" : legalEntityCompany.FaxNumber;

                    break;

                case SAHL.Common.Globals.LegalEntityTypes.Trust:
                    ILegalEntityTrust legalEntityTrust  = (ILegalEntityTrust)LegalEntity;

                    registeredName = String.IsNullOrEmpty(legalEntityTrust.RegisteredName) ? "-" : legalEntityTrust.RegisteredName;
                    tradingName = String.IsNullOrEmpty(legalEntityTrust.TradingName) ? "-" : legalEntityTrust.TradingName;
                    registrationNumber = String.IsNullOrEmpty(legalEntityTrust.RegistrationNumber) ? "-" : legalEntityTrust.RegistrationNumber;
                    workPhoneCode = String.IsNullOrEmpty(legalEntityTrust.WorkPhoneCode) ? "-" : legalEntityTrust.WorkPhoneCode;
                    workPhoneNumber= String.IsNullOrEmpty(legalEntityTrust.WorkPhoneNumber) ? "-" : legalEntityTrust.WorkPhoneNumber;
                    faxCode = String.IsNullOrEmpty(legalEntityTrust.FaxCode) ? "-" : legalEntityTrust.FaxCode;
                    faxNumber = String.IsNullOrEmpty(legalEntityTrust.FaxNumber) ? "-" : legalEntityTrust.FaxNumber;

                    break;

                case SAHL.Common.Globals.LegalEntityTypes.Unknown:
                    throw new Exception(errorMessage);

                default:
                    throw new Exception(errorMessage);

            }

            lblCLEIntroductionDate.Text = LegalEntity.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblCLECompanyName.Text = registeredName;
            lblCLETradingName.Text = tradingName;
            lblCLERegistrationNumber.Text = registrationNumber;
            lblCLEStatus.Text = LegalEntity.LegalEntityStatus == null ? "-" : LegalEntity.LegalEntityStatus.Description;
            lblCLECellPhone.Text = String.IsNullOrEmpty(LegalEntity.CellPhoneNumber) ? "-" : LegalEntity.CellPhoneNumber.Trim();
            lblCLEEmailAddress.Text = String.IsNullOrEmpty(LegalEntity.EmailAddress) ? "-" : LegalEntity.EmailAddress.Trim();

            m_Temp = "({0}) {1}";
            m_Temp = String.Format(m_Temp, workPhoneCode.Trim(), workPhoneNumber.Trim());
            if (workPhoneNumber == "-" && workPhoneCode == "-")
                m_Temp = "-";
            lblCLEWorkPhone.Text = m_Temp;

            m_Temp = "({0}) {1}";
            m_Temp = String.Format(m_Temp, faxCode.Trim(), faxNumber.Trim());
            if (LegalEntity.FaxCode == "-" && LegalEntity.FaxNumber == "-")
                m_Temp = "-";
            lblCLEFaxNumber.Text = m_Temp;

        }

        #endregion

        #region IClientDetails Members

        public bool NaturalPersonPanelVisible
        {
            set { _naturalPersonPanelVisible = value; }
        }

        public bool CompanyPanelVisible
        {
            set { _companyPanelVisible = value; }
        }

        public void BindLegalEntityNaturalPerson(ILegalEntityNaturalPerson LegalEntity)
        {
            BindNaturalPerson(LegalEntity);
        }

        public void BindLegalEntityCompany(ILegalEntity LegalEntity)
        {
            BindCompany(LegalEntity);
        }

        public void BindAddressDescription(string AddressDescription)
        {
            _addressDescription = AddressDescription;
        }

        public void BindAddresses(StringCollection LegalEntityAddress)
        {
            lblAddressLine1.Text = LegalEntityAddress.Count > 0 ? LegalEntityAddress[0] : "-";
            lblAddressLine2.Text = LegalEntityAddress.Count > 1 ? LegalEntityAddress[1] : "-";
            lblAddressLine3.Text = LegalEntityAddress.Count > 2 ? LegalEntityAddress[2] : "-";
            lblAddressLine4.Text = LegalEntityAddress.Count > 3 ? LegalEntityAddress[3] : "-";
            lblAddressLine5.Text = LegalEntityAddress.Count > 4 ? LegalEntityAddress[4] : "-";
        }

        #endregion

    }
}
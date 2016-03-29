using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class LegalEntityDetails : SAHLCommonBaseView, ILegalEntityDetails
    {
        #region ILegalEntityDetails Private Members

        private bool _panelAddVisible;
        private bool _panelCompanyAddVisible;
        private bool _updateControlsVisible;
        private bool _lockedUpdateControlsVisible;
        private bool _marketingOptionsEnabled = true;
        private bool _limitedUpdate;
        private bool _nonContactDetailsDisabled;
        private bool _panelCompanyDisplayVisible;
        private bool _panelNaturalPersonDisplayVisible;
        private bool _panelNaturalPersonAddVisible;
        private bool _panelMarketingOptionPanelVisible = true;
        private bool _cancelButtonVisible;
        private bool _submitButtonVisible;
        private bool _applicantsUpdateWithActiveFinancialServices;
        private bool _applicantAddExistingRoleVisible;
        private bool _addRoleTypeVisible = true;
        private bool _legalEntityTypeEnabled = true;
        private bool _insurableInterestUpdateVisible;
        private bool _insurableInterestDisplayVisible;
        private bool _displayRoleTypeVisible;
        private bool _updateRoleTypeVisible;
        private int _applicantRoleTypeKey;
        private bool _incomeContributorVisible;
        private bool _selectedIncomeContributor;
        private bool _selectAllMarketingOptions;
        private bool _disableAjaxFunctionality;
        private ILookupRepository _lookupRepository;
        private ICommonRepository _commonRepo;
        private bool _UpdateMyDetails;
        private bool _RoleTypeVisible = true;
        private bool _CitizenTypeVisible = true;
        private bool _DateOfBirthVisible = true;
        private bool _PassportNumberVisible = true;
        private bool _TaxNumberVisible = true;
        private bool _GenderVisible = true;
        private bool _MaritalStatusVisible = true;
        private bool _StatusVisible = true;
        private bool _PopulationGroupVisible = true;
        private bool _InsurableInterestVisible = true;

        #endregion ILegalEntityDetails Private Members

        #region Constants

        private const string DEFAULTDROPDOWNITEM = "-select-";

        #endregion Constants

        #region Protected Functions Section

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            _commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            acIDNumberUpdate.ItemSelected += acIDNumberUpdate_ItemSelected;
            acNatAddIDNumber.ItemSelected += acNatAddIDNumber_ItemSelected;

            acPassportNumberUpdate.ItemSelected += acPassportNumberUpdate_ItemSelected;
            acNatAddPassportNumber.ItemSelected += acNatAddPassportNumber_ItemSelected;

            acCOAddRegistrationNumber.ItemSelected += acCOAddRegistrationNumber_ItemSelected;
            acCORegistrationNumberUpdate.ItemSelected += acCORegistrationNumberUpdate_ItemSelected;

            if (_disableAjaxFunctionality)
            {
                acIDNumberUpdate.Visible = false;
                acPassportNumberUpdate.Visible = false;
                acCORegistrationNumberUpdate.Visible = false;
            }
        }

        private void acIDNumberUpdate_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
        }

        private void acPassportNumberUpdate_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
        }

        private void acCORegistrationNumberUpdate_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
        }

        private void acNatAddIDNumber_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
        }

        private void acNatAddPassportNumber_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
        }

        private void acCOAddRegistrationNumber_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ShouldRunPage)
                RegisterWebService(ServiceConstants.LegalEntity);
        }

        #endregion Protected Functions Section

        #region private methods

        private void SwitchUpdateAdd(bool p_UpdateTrue)
        {
            txtUpdPreferredName.Visible = p_UpdateTrue;
            txtUpdPassportNumber.Visible = p_UpdateTrue;
            txtUpdTaxNumber.Visible = p_UpdateTrue;
            ddlUpdPopulationGroup.Visible = p_UpdateTrue;
            ddlEducation.Visible = p_UpdateTrue;
            ddlUpdHomeLanguage.Visible = p_UpdateTrue;
            ddlUpdDocumentLanguage.Visible = p_UpdateTrue;
            ddlUpdStatus.Visible = p_UpdateTrue;
            // ddlUpdResidenceStatus.Visible = p_UpdateTrue;
            ddlNatUpdInsurableInterest.Visible = p_UpdateTrue; // Craig Fraser 25/10/2006
            // ddlRoleTypeUpdate.Visible = p_UpdateTrue;
            txtUpdDateOfBirth.Visible = p_UpdateTrue;
            txtIDNumberUpdate.Visible = p_UpdateTrue;

            txtCOUpdTradingName.Visible = p_UpdateTrue;
            txtCOUpdTaxNumber.Visible = p_UpdateTrue;
            ddlCOUpdDocumentLanguage.Visible = p_UpdateTrue;
            ddlCOUpdStatus.Visible = p_UpdateTrue;

            CORegistrationNumberUpdate.Visible = p_UpdateTrue;
            lblCORegistrationNumber.Visible = !p_UpdateTrue;

            lblPreferredName.Visible = !p_UpdateTrue;
            lblHomePhone.Visible = !p_UpdateTrue;
            lblWorkPhone.Visible = !p_UpdateTrue;
            lblFaxNumber.Visible = !p_UpdateTrue;
            lblCellphoneNumber.Visible = !p_UpdateTrue;
            lblEmailAddress.Visible = !p_UpdateTrue;
            lblIDNumber.Visible = !p_UpdateTrue;
            lblDateofBirth.Visible = !p_UpdateTrue;
            lblPassportNumber.Visible = !p_UpdateTrue;
            lblTaxNumber.Visible = !p_UpdateTrue;
            lblHomeLanguage.Visible = !p_UpdateTrue;
            lblDocumentLanguage.Visible = !p_UpdateTrue;
            lblStatus.Visible = !p_UpdateTrue;
            // Field removed as per user request
            // lblResidenceStatus.Visible = !p_UpdateTrue;
            lblPopulationGroup.Visible = !p_UpdateTrue;
            lblEducation.Visible = !p_UpdateTrue;
            lblInsurableInterest.Visible = !p_UpdateTrue; // Craig Fraser 25/10/2006
            // Field removed as per user request
            // lblRoleTypeUpdate.Visible = !p_UpdateTrue;

            lblCOTradingName.Visible = !p_UpdateTrue;
            lblCOTaxNumber.Visible = !p_UpdateTrue;
            lblCOStatus.Visible = !p_UpdateTrue;
            lblCODocumentLanguage.Visible = !p_UpdateTrue;

            lblNatIncomeContributor.Visible = !p_UpdateTrue;
            lblCoIncomeContributor.Visible = !p_UpdateTrue;
            chkCoUpdIncomeContributor.Visible = p_UpdateTrue;
            chkNatUpdIncomeContributor.Visible = p_UpdateTrue;

            ddlSalutationUpdate.Visible = p_UpdateTrue;
            lblSalutation.Visible = !p_UpdateTrue;

            txtHomePhoneCode.Visible = p_UpdateTrue;
            txtHomePhoneNumber.Visible = p_UpdateTrue;
            txtWorkPhoneCode.Visible = p_UpdateTrue;
            txtWorkPhoneNumber.Visible = p_UpdateTrue;
            txtFaxCode.Visible = p_UpdateTrue;
            txtFaxNumber.Visible = p_UpdateTrue;
            txtCellPhoneNumber.Visible = p_UpdateTrue;
            txtEmailAddress.Visible = p_UpdateTrue;

            lblHomePhone.Visible = !p_UpdateTrue;
            lblWorkPhone.Visible = !p_UpdateTrue;
            lblFaxNumber.Visible = !p_UpdateTrue;
            lblCellphoneNumber.Visible = !p_UpdateTrue;
            lblEmailAddress.Visible = !p_UpdateTrue;
        }

        #endregion private methods

        #region Private Helper Functions

        private static void PopulateDropDown(SAHLDropDownList DropDownList, IDictionary<string, string> IDataItems, string DefaultValue)
        {
            PopulateDropDown(DropDownList, IDataItems, DefaultValue, true);
        }

        private static void PopulateDropDown(SAHLDropDownList DropDownList, IDictionary<string, string> IDataItems, string DefaultValue, bool pleaseSelect)
        {
            DropDownList.DataSource = IDataItems;
            DropDownList.DataBind();
            if (pleaseSelect)
                DropDownList.VerifyPleaseSelect();

            // Set the default value if supplied
            if (!String.IsNullOrEmpty(DefaultValue))
                DropDownList.SelectedValue = DefaultValue;
        }

        #endregion Private Helper Functions

        #region ILegalEntityDetails Members

        public event EventHandler onCancelButtonClicked;

        public event EventHandler onSubmitButtonClicked;

        public void BindLegalEntityTypes(IDictionary<string, string> LegalEntityType, string DefaultValue)
        {
            PopulateDropDown(ddlLegalEntityTypes, LegalEntityType, DefaultValue);
            PopulateDropDown(COLegalEntityTypeUpdate, LegalEntityType, DefaultValue);
        }

        public void BindRoleTypes(IDictionary<string, string> RoleType, string DefaultValue)
        {
            PopulateDropDown(ddlRoleTypeAdd, RoleType, DefaultValue, false);

            PopulateDropDown(ddlRoleTypeUpdate, RoleType, DefaultValue, false);

            PopulateDropDown(ddlCoRoleTypeUpdate, RoleType, DefaultValue, false);
        }

        public void BindSalutation(IDictionary<string, string> Salutation, string DefaultValue)
        {
            PopulateDropDown(ddlSalutationUpdate, Salutation, DefaultValue);
            PopulateDropDown(ddlNatAddSalutation, Salutation, DefaultValue);
        }

        public void BindGender(IDictionary<string, string> Gender, string DefaultValue)
        {
            PopulateDropDown(ddlGenderUpdate, Gender, DefaultValue);
            PopulateDropDown(ddlNatAddGender, Gender, DefaultValue);
        }

        public void BindMaritalStatus(IDictionary<string, string> MaritalStatus, string DefaultValue)
        {
            PopulateDropDown(ddlNatAddMaritalStatus, MaritalStatus, DefaultValue);
            PopulateDropDown(ddlMaritalStatusUpdate, MaritalStatus, DefaultValue);
        }

        public void BindPopulationGroup(IDictionary<string, string> PopulationGroup, string DefaultValue)
        {
            PopulateDropDown(ddlUpdPopulationGroup, PopulationGroup, DefaultValue);
            PopulateDropDown(ddlNatAddpopulationGroup, PopulationGroup, DefaultValue);
        }

        public void BindEducation(IDictionary<string, string> Education, string DefaultValue)
        {
            PopulateDropDown(ddlEducation, Education, DefaultValue);
            PopulateDropDown(ddlNatAddEducation, Education, DefaultValue);
        }

        public void BindCitizenType(IDictionary<string, string> CitizenType, string DefaultValue)
        {
            PopulateDropDown(ddlNatAddCitizenshipType, CitizenType, DefaultValue);
            PopulateDropDown(ddlCitizenshipTypeUpdate, CitizenType, DefaultValue);
        }

        public void BindHomeLanguage(ICollection<ILanguageReadOnly> HomeLanguage, string DefaultValue)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (ILanguageReadOnly l in HomeLanguage)
                dict.Add(l.Key.ToString(), l.Description);

            PopulateDropDown(ddlUpdHomeLanguage, dict, DefaultValue);
            PopulateDropDown(ddlNatAddHomeLanguage, dict, DefaultValue);
        }

        public void BindDocumentLanguage(ICollection<ILanguageReadOnly> DocumentLanguage, string DefaultValue)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (ILanguageReadOnly l in DocumentLanguage)
                dict.Add(l.Key.ToString(), l.Description);

            PopulateDropDown(ddlUpdDocumentLanguage, dict, DefaultValue);
            PopulateDropDown(ddlCOUpdDocumentLanguage, dict, DefaultValue);
            PopulateDropDown(ddlNatAddDocumentLanguage, dict, DefaultValue);
            PopulateDropDown(ddlCOAddDocumentlanguage, dict, DefaultValue);
        }

        public void BindLegalEntityStatus(IDictionary<string, string> LegalEntityStatus, string DefaultValue)
        {
            PopulateDropDown(ddlUpdStatus, LegalEntityStatus, DefaultValue);
            PopulateDropDown(ddlCOUpdStatus, LegalEntityStatus, DefaultValue);
            PopulateDropDown(ddlNatAddStatus, LegalEntityStatus, DefaultValue);
            PopulateDropDown(ddlCOAddStatus, LegalEntityStatus, DefaultValue);
        }

        public void BindResidenceStatus(IDictionary<string, string> ResidenceStatus, string DefaultValue)
        {
            // Field removed as per user request
            // PopulateDropDown(ddlUpdResidenceStatus, ResidenceStatus, DefaultValue);
            // PopulateDropDown(ddlNatResidenceStatus, ResidenceStatus, DefaultValue);
        }

        public void BindInsurableInterestType(IDictionary<string, string> InsurableInterestType, string DefaultValue)
        {
            PopulateDropDown(ddlNatUpdInsurableInterest, InsurableInterestType, DefaultValue);
            PopulateDropDown(ddlNatAddInsurableInterest, InsurableInterestType, DefaultValue);
        }

        public void BindRegistrationNumberLabel(string RegistrationNumber)
        {
            lblCORegistrationNumber.Text = RegistrationNumber;
        }

        public void BindLegalEntityReadOnlyNaturalPerson(ILegalEntityNaturalPerson LegalEntity)
        {
            PopulateLegalEntityNaturalPersonForUpdate(LegalEntity);
        }

        public void BindLegalEntityReadOnlyCompany(ILegalEntity LegalEntity)
        {
            PopulateLegalEntityCompanyForUpdate(LegalEntity);
        }

        public void BindLegalEntityUpdatableNaturalPerson(ILegalEntityNaturalPerson LegalEntity)
        {
            PopulateLegalEntityNaturalPersonForUpdate(LegalEntity);
        }

        public void BindLegalEntityUpdatableCompany(ILegalEntity LegalEntity)
        {
            PopulateLegalEntityCompanyForUpdate(LegalEntity);
        }

        public void BindIntroductionDate(DateTime IntroductionDate)
        {
            lblNatIntroductionDate.Text = IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblCOAddIntroductionDate.Text = IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
        }

        public bool PanelAddVisible
        {
            set { _panelAddVisible = value; }
        }

        public bool PanelCompanyAddVisible
        {
            set { _panelCompanyAddVisible = value; }
        }

        public bool UpdateControlsVisible
        {
            set { _updateControlsVisible = value; }
        }

        public bool LockedUpdateControlsVisible
        {
            get { return _lockedUpdateControlsVisible; }
            set { _lockedUpdateControlsVisible = value; }
        }

        public bool PanelNaturalPersonAddVisible
        {
            set { _panelNaturalPersonAddVisible = value; }
        }

        public bool PanelNaturalPersonDisplayVisible
        {
            set { _panelNaturalPersonDisplayVisible = value; }
        }

        public bool PanelCompanyDisplayVisible
        {
            set { _panelCompanyDisplayVisible = value; }
        }

        public bool PanelMarketingOptionPanelVisible
        {
            set { _panelMarketingOptionPanelVisible = value; }
        }

        public bool NonContactDetailsDisabled
        {
            set { _nonContactDetailsDisabled = value; }
        }

        public void BindInsurableInterestReadonly(string InsurableInterest)
        {
            lblInsurableInterest.Text = String.IsNullOrEmpty(InsurableInterest) ? "-" : InsurableInterest;
        }

        public event KeyChangedEventHandler onLegalEntityTypeChange;

        public string SubmitButtonText
        {
            set { btnSubmitButton.Text = value; }
        }

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible = value; }
        }

        public bool SubmitButtonVisible
        {
            set { _submitButtonVisible = value; }
        }

        public bool SetUpdateMyDetails
        {
            set { _UpdateMyDetails = value; }
        }

        public bool RoleTypeVisible
        {
            set { _RoleTypeVisible = value; }
        }

        public bool CitizenTypeVisible
        {
            set { _CitizenTypeVisible = value; }
        }

        public bool DateOfBirthVisible
        {
            set { _DateOfBirthVisible = value; }
        }

        public bool PassportNumberVisible
        {
            set { _PassportNumberVisible = value; }
        }

        public bool TaxNumberVisible
        {
            set { _TaxNumberVisible = value; }
        }

        public bool GenderVisible
        {
            set { _GenderVisible = value; }
        }

        public bool MaritalStatusVisible
        {
            set { _MaritalStatusVisible = value; }
        }

        public bool StatusVisible
        {
            set { _StatusVisible = value; }
        }

        public bool PopulationGroupVisible
        {
            set { _PopulationGroupVisible = value; }
        }

        public bool InsurableInterestVisible
        {
            set { _InsurableInterestVisible = value; }
        }

        /// <summary>
        /// Populates the screen update controls with the legalentity details (company,cc,trust)
        /// </summary>
        /// <param name="LegalEntity"></param>
        private void PopulateLegalEntityCompanyForUpdate(ILegalEntity LegalEntity)
        {
            string telFormat;

            chkCoUpdIncomeContributor.Checked = _selectedIncomeContributor;
            //lblCoIncomeContributor.Text = _selectedIncomeContributor ? "Yes" : "No";

            if (_applicantRoleTypeKey > 0)
                ddlCoRoleTypeUpdate.SelectedValue = _applicantRoleTypeKey.ToString();

            switch ((LegalEntityTypes)LegalEntity.LegalEntityType.Key)
            {
                case LegalEntityTypes.CloseCorporation:

                    ILegalEntityCloseCorporation leCloseCorporation = LegalEntity as ILegalEntityCloseCorporation;

                    if (leCloseCorporation != null)
                    {
                        COLegalEntityTypeUpdate.SelectedValue = leCloseCorporation.LegalEntityType.Key.ToString();
                        lblCOLegalEntityType.Text = leCloseCorporation.LegalEntityType == null ? "-" : leCloseCorporation.LegalEntityType.Description;

                        lblCOIntroductionDate.Text = leCloseCorporation.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
                        COIntroductionDateUpdate.Text = leCloseCorporation.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                        ddlCOUpdStatus.SelectedValue = leCloseCorporation.LegalEntityStatus == null ? DEFAULTDROPDOWNITEM : leCloseCorporation.LegalEntityStatus.Key.ToString();
                        lblCOStatus.Text = leCloseCorporation.LegalEntityStatus == null ? "-" : leCloseCorporation.LegalEntityStatus.Description;

                        lblCORegisteredName.Text = leCloseCorporation.RegisteredName ?? "-";
                        CORegisteredNameUpdate.Text = leCloseCorporation.RegisteredName ?? String.Empty;

                        lblCOTradingName.Text = leCloseCorporation.TradingName ?? "-";
                        txtCOUpdTradingName.Text = leCloseCorporation.TradingName ?? String.Empty;

                        lblCORegistrationNumber.Text = leCloseCorporation.RegistrationNumber ?? "-";
                        CORegistrationNumberUpdate.Text = leCloseCorporation.RegistrationNumber ?? String.Empty;

                        txtCOUpdTaxNumber.Text = leCloseCorporation.TaxNumber ?? String.Empty;
                        lblCOTaxNumber.Text = leCloseCorporation.TaxNumber ?? "-";

                        ddlCOUpdDocumentLanguage.SelectedValue = leCloseCorporation.DocumentLanguage == null ? DEFAULTDROPDOWNITEM : leCloseCorporation.DocumentLanguage.Key.ToString();
                        lblCODocumentLanguage.Text = leCloseCorporation.DocumentLanguage == null ? "-" : leCloseCorporation.DocumentLanguage.Description;
                    }

                    telFormat = "({0}) {1}";
                    if (leCloseCorporation != null)
                        telFormat = String.Format(telFormat, leCloseCorporation.WorkPhoneCode ?? "-", leCloseCorporation.WorkPhoneNumber ?? "-");
                    lblWorkPhone.Text = telFormat;
                    if (leCloseCorporation != null)
                    {
                        txtWorkPhoneCode.Text = leCloseCorporation.WorkPhoneCode ?? String.Empty;
                        txtWorkPhoneNumber.Text = leCloseCorporation.WorkPhoneNumber ?? String.Empty;
                    }

                    telFormat = "({0}) {1}";
                    telFormat = String.Format(telFormat, leCloseCorporation.FaxCode ?? "-", leCloseCorporation.FaxNumber ?? "-");
                    lblFaxNumber.Text = telFormat;
                    txtFaxCode.Text = leCloseCorporation.FaxCode ?? String.Empty;
                    txtFaxNumber.Text = leCloseCorporation.FaxNumber ?? String.Empty;

                    txtCellPhoneNumber.Text = leCloseCorporation.CellPhoneNumber ?? String.Empty;
                    lblCellphoneNumber.Text = leCloseCorporation.CellPhoneNumber ?? "-";

                    txtEmailAddress.Text = leCloseCorporation.EmailAddress ?? String.Empty;
                    lblEmailAddress.Text = leCloseCorporation.EmailAddress ?? "-";

                    break;

                case LegalEntityTypes.Trust:

                    ILegalEntityTrust leTrust = LegalEntity as ILegalEntityTrust;

                    if (leTrust != null)
                    {
                        COLegalEntityTypeUpdate.SelectedValue = leTrust.LegalEntityType.Key.ToString();
                        lblCOLegalEntityType.Text = leTrust.LegalEntityType == null ? "-" : leTrust.LegalEntityType.Description;

                        lblCOIntroductionDate.Text = leTrust.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
                        COIntroductionDateUpdate.Text = leTrust.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                        ddlCOUpdStatus.SelectedValue = leTrust.LegalEntityStatus == null ? DEFAULTDROPDOWNITEM : leTrust.LegalEntityStatus.Key.ToString();
                        lblCOStatus.Text = leTrust.LegalEntityStatus == null ? "-" : leTrust.LegalEntityStatus.Description;

                        lblCORegisteredName.Text = leTrust.RegisteredName ?? "-";
                        CORegisteredNameUpdate.Text = leTrust.RegisteredName ?? String.Empty;

                        lblCOTradingName.Text = leTrust.TradingName ?? "-";
                        txtCOUpdTradingName.Text = leTrust.TradingName ?? String.Empty;

                        lblCORegistrationNumber.Text = leTrust.RegistrationNumber ?? "-";
                        CORegistrationNumberUpdate.Text = leTrust.RegistrationNumber ?? String.Empty;

                        txtCOUpdTaxNumber.Text = leTrust.TaxNumber ?? String.Empty;
                        lblCOTaxNumber.Text = leTrust.TaxNumber ?? "-";

                        ddlCOUpdDocumentLanguage.SelectedValue = leTrust.DocumentLanguage == null ? DEFAULTDROPDOWNITEM : leTrust.DocumentLanguage.Key.ToString();
                        lblCODocumentLanguage.Text = leTrust.DocumentLanguage == null ? "-" : leTrust.DocumentLanguage.Description;
                    }

                    telFormat = "({0}) {1}";
                    if (leTrust != null)
                        telFormat = String.Format(telFormat, leTrust.WorkPhoneCode ?? "-", leTrust.WorkPhoneNumber ?? "-");
                    lblWorkPhone.Text = telFormat;
                    if (leTrust != null)
                    {
                        txtWorkPhoneCode.Text = leTrust.WorkPhoneCode ?? String.Empty;
                        txtWorkPhoneNumber.Text = leTrust.WorkPhoneNumber ?? String.Empty;
                    }

                    telFormat = "({0}) {1}";
                    telFormat = String.Format(telFormat, leTrust.FaxCode ?? "-", leTrust.FaxNumber ?? "-");
                    lblFaxNumber.Text = telFormat;
                    txtFaxCode.Text = leTrust.FaxCode ?? String.Empty;
                    txtFaxNumber.Text = leTrust.FaxNumber ?? String.Empty;

                    txtCellPhoneNumber.Text = leTrust.CellPhoneNumber ?? String.Empty;
                    lblCellphoneNumber.Text = leTrust.CellPhoneNumber ?? "-";

                    txtEmailAddress.Text = leTrust.EmailAddress ?? String.Empty;
                    lblEmailAddress.Text = leTrust.EmailAddress ?? "-";

                    break;

                case LegalEntityTypes.Company:

                    ILegalEntityCompany leCompany = LegalEntity as ILegalEntityCompany;

                    if (leCompany != null)
                    {
                        COLegalEntityTypeUpdate.SelectedValue = leCompany.LegalEntityType.Key.ToString();
                        lblCOLegalEntityType.Text = leCompany.LegalEntityType == null ? "-" : leCompany.LegalEntityType.Description;

                        lblCOIntroductionDate.Text = leCompany.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
                        COIntroductionDateUpdate.Text = leCompany.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                        ddlCOUpdStatus.SelectedValue = leCompany.LegalEntityStatus == null ? DEFAULTDROPDOWNITEM : leCompany.LegalEntityStatus.Key.ToString();
                        lblCOStatus.Text = leCompany.LegalEntityStatus == null ? "-" : leCompany.LegalEntityStatus.Description;

                        lblCORegisteredName.Text = leCompany.RegisteredName ?? "-";
                        CORegisteredNameUpdate.Text = leCompany.RegisteredName ?? String.Empty;

                        lblCOTradingName.Text = leCompany.TradingName ?? "-";
                        txtCOUpdTradingName.Text = leCompany.TradingName ?? String.Empty;

                        lblCORegistrationNumber.Text = leCompany.RegistrationNumber ?? "-";
                        CORegistrationNumberUpdate.Text = leCompany.RegistrationNumber ?? String.Empty;

                        txtCOUpdTaxNumber.Text = leCompany.TaxNumber ?? String.Empty;
                        lblCOTaxNumber.Text = leCompany.TaxNumber ?? "-";

                        ddlCOUpdDocumentLanguage.SelectedValue = leCompany.DocumentLanguage == null ? DEFAULTDROPDOWNITEM : leCompany.DocumentLanguage.Key.ToString();
                        lblCODocumentLanguage.Text = leCompany.DocumentLanguage == null ? "-" : leCompany.DocumentLanguage.Description;
                    }

                    telFormat = "({0}) {1}";
                    if (leCompany != null)
                        telFormat = String.Format(telFormat, leCompany.WorkPhoneCode ?? "-", leCompany.WorkPhoneNumber ?? "-");
                    lblWorkPhone.Text = telFormat;
                    if (leCompany != null)
                    {
                        txtWorkPhoneCode.Text = leCompany.WorkPhoneCode ?? String.Empty;
                        txtWorkPhoneNumber.Text = leCompany.WorkPhoneNumber ?? String.Empty;
                    }

                    telFormat = "({0}) {1}";
                    telFormat = String.Format(telFormat, leCompany.FaxCode ?? "-", leCompany.FaxNumber ?? "-");
                    lblFaxNumber.Text = telFormat;
                    txtFaxCode.Text = leCompany.FaxCode ?? String.Empty;
                    txtFaxNumber.Text = leCompany.FaxNumber ?? String.Empty;

                    txtCellPhoneNumber.Text = leCompany.CellPhoneNumber ?? String.Empty;
                    lblCellphoneNumber.Text = leCompany.CellPhoneNumber ?? "-";

                    txtEmailAddress.Text = leCompany.EmailAddress ?? String.Empty;
                    lblEmailAddress.Text = leCompany.EmailAddress ?? "-";

                    break;
            }

            foreach (ILegalEntityMarketingOption legalEntityMarketingOption in LegalEntity.LegalEntityMarketingOptions)
            {
                MarketingOptionExcludedListBox.Items.FindByValue(legalEntityMarketingOption.MarketingOption.Key.ToString()).Selected = false;
            }
        }

        /// <summary>
        /// Populates the screen update controls with the legalentity details (natural person)
        /// </summary>
        /// <param name="LegalEntity"></param>
        private void PopulateLegalEntityNaturalPersonForUpdate(ILegalEntity LegalEntity)
        {
            chkNatUpdIncomeContributor.Checked = _selectedIncomeContributor;
            //lblNatIncomeContributor.Text = _selectedIncomeContributor ? "Yes" : "No";

            ILegalEntityNaturalPerson LegalEntityNaturalPerson = (ILegalEntityNaturalPerson)LegalEntity;
            //string m_LegalEntityName = "{0} {1} {2}";

            string m_FirstNames = LegalEntityNaturalPerson.FirstNames ?? "-";
            //string m_Surname = LegalEntityNaturalPerson.Surname ?? "-";
            string m_Salutation = LegalEntityNaturalPerson.Salutation == null ? "-" : LegalEntityNaturalPerson.Salutation.Description;
            //m_LegalEntityName = String.Format(m_LegalEntityName, m_Salutation == "-" ? "" : m_Salutation, m_FirstNames, m_Surname);

            #region ReadOnly Controls

            lblIntroductionDate.Text = LegalEntityNaturalPerson.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            //lblLegalEntityName.Text = m_LegalEntityName;
            lblSalutation.Text = m_Salutation;
            lblInitials.Text = LegalEntityNaturalPerson.Initials ?? "-";
            lblFirstNames.Text = m_FirstNames;
            lblSurname.Text = LegalEntityNaturalPerson.Surname ?? "-";
            lblPreferredName.Text = LegalEntityNaturalPerson.PreferredName ?? "-";
            lblGender.Text = LegalEntityNaturalPerson.Gender == null ? "-" : LegalEntityNaturalPerson.Gender.Description;
            lblMaritalStatus.Text = LegalEntityNaturalPerson.MaritalStatus == null ? "-" : LegalEntityNaturalPerson.MaritalStatus.Description;
            lblPopulationGroup.Text = LegalEntityNaturalPerson.PopulationGroup == null ? "-" : LegalEntityNaturalPerson.PopulationGroup.Description;
            lblEducation.Text = LegalEntityNaturalPerson.Education == null ? "-" : LegalEntityNaturalPerson.Education.Description;

            string telFaxFormat = "({0}) {1}";
            telFaxFormat = String.Format(telFaxFormat,
                                            LegalEntityNaturalPerson.HomePhoneCode ?? "-",
                                            LegalEntityNaturalPerson.HomePhoneNumber ?? "-");
            lblHomePhone.Text = telFaxFormat;

            telFaxFormat = "({0}) {1}";
            telFaxFormat = String.Format(telFaxFormat,
                                            LegalEntityNaturalPerson.WorkPhoneCode ?? "-",
                                            LegalEntityNaturalPerson.WorkPhoneNumber ?? "-");
            lblWorkPhone.Text = telFaxFormat;

            telFaxFormat = "({0}) {1}";
            telFaxFormat = String.Format(telFaxFormat,
                                            LegalEntityNaturalPerson.FaxCode ?? "-",
                                            LegalEntityNaturalPerson.FaxNumber ?? "-");
            lblFaxNumber.Text = telFaxFormat;

            lblCellphoneNumber.Text = LegalEntityNaturalPerson.CellPhoneNumber ?? "-";
            lblEmailAddress.Text = LegalEntityNaturalPerson.EmailAddress ?? "-";

            lblCitizenshipType.Text = LegalEntityNaturalPerson.CitizenType == null ? "-" : LegalEntityNaturalPerson.CitizenType.Description;
            lblIDNumber.Text = LegalEntityNaturalPerson.IDNumber ?? "-";
            lblDateofBirth.Text = !LegalEntityNaturalPerson.DateOfBirth.HasValue ? "-" : LegalEntityNaturalPerson.DateOfBirth.Value.ToString(SAHL.Common.Constants.DateFormat);
            lblPassportNumber.Text = LegalEntityNaturalPerson.PassportNumber ?? "-";
            lblTaxNumber.Text = LegalEntityNaturalPerson.TaxNumber ?? "-";
            lblHomeLanguage.Text = LegalEntityNaturalPerson.HomeLanguage == null ? "-" : LegalEntityNaturalPerson.HomeLanguage.Description;
            lblDocumentLanguage.Text = LegalEntityNaturalPerson.DocumentLanguage == null ? "-" : LegalEntityNaturalPerson.DocumentLanguage.Description;
            lblStatus.Text = LegalEntityNaturalPerson.LegalEntityStatus == null ? "-" : LegalEntityNaturalPerson.LegalEntityStatus.Description;
            lblType.Text = LegalEntityNaturalPerson.LegalEntityType == null ? "-" : LegalEntityNaturalPerson.LegalEntityType.Description;
            // lblResidenceStatus.Text = LegalEntityNaturalPerson.IsResidenceStatusKeyNull() ? "-" : m_Lookups.ResidenceStatus.FindByResidenceStatusKey(LegalEntityNaturalPerson.ResidenceStatusKey).Description;

            #endregion ReadOnly Controls

            #region Updateable Controls

            txtInitialsUpdate.Text = LegalEntityNaturalPerson.Initials ?? String.Empty;
            txtFirstNamesUpdate.Text = m_FirstNames;
            txtSurnameUpdate.Text = LegalEntityNaturalPerson.Surname ?? String.Empty;
            txtUpdPreferredName.Text = LegalEntityNaturalPerson.PreferredName ?? String.Empty;

            if (_applicantRoleTypeKey > 0)
                ddlRoleTypeUpdate.SelectedValue = _applicantRoleTypeKey.ToString();

            if (LegalEntityNaturalPerson.Salutation != null)
                ddlSalutationUpdate.SelectedValue = LegalEntityNaturalPerson.Salutation.Key.ToString();

            if (LegalEntityNaturalPerson.Gender != null)
                ddlGenderUpdate.SelectedValue = LegalEntityNaturalPerson.Gender.Key.ToString();

            if (LegalEntityNaturalPerson.MaritalStatus != null)
                ddlMaritalStatusUpdate.SelectedValue = LegalEntityNaturalPerson.MaritalStatus.Key.ToString();

            if (LegalEntityNaturalPerson.CitizenType != null)
                ddlCitizenshipTypeUpdate.SelectedValue = LegalEntityNaturalPerson.CitizenType.Key.ToString();

            if (LegalEntityNaturalPerson.PopulationGroup != null)
                ddlUpdPopulationGroup.SelectedValue = LegalEntityNaturalPerson.PopulationGroup.Key.ToString();

            if (LegalEntityNaturalPerson.Education != null)
                ddlEducation.SelectedValue = LegalEntityNaturalPerson.Education.Key.ToString();

            if (LegalEntityNaturalPerson.HomeLanguage != null)
                ddlUpdHomeLanguage.SelectedValue = LegalEntityNaturalPerson.HomeLanguage.Key.ToString();

            if (LegalEntityNaturalPerson.DocumentLanguage != null)
                ddlUpdDocumentLanguage.SelectedValue = LegalEntityNaturalPerson.DocumentLanguage.Key.ToString();

            // Field removed as per user request
            // if (LegalEntityNaturalPerson.ResidenceStatus != null)
            // ddlUpdResidenceStatus.SelectedValue = LegalEntityNaturalPerson.ResidenceStatus.Key.ToString();

            if (LegalEntityNaturalPerson.LegalEntityStatus != null)
                ddlUpdStatus.SelectedValue = LegalEntityNaturalPerson.LegalEntityStatus.Key.ToString();

            txtHomePhoneCode.Text = LegalEntityNaturalPerson.HomePhoneCode ?? String.Empty;
            txtHomePhoneNumber.Text = LegalEntityNaturalPerson.HomePhoneNumber ?? String.Empty;
            txtWorkPhoneCode.Text = LegalEntityNaturalPerson.WorkPhoneCode ?? String.Empty;
            txtWorkPhoneNumber.Text = LegalEntityNaturalPerson.WorkPhoneNumber ?? String.Empty;
            txtFaxCode.Text = LegalEntityNaturalPerson.FaxCode ?? String.Empty;
            txtFaxNumber.Text = LegalEntityNaturalPerson.FaxNumber ?? String.Empty;
            txtCellPhoneNumber.Text = LegalEntityNaturalPerson.CellPhoneNumber ?? String.Empty;
            txtEmailAddress.Text = LegalEntityNaturalPerson.EmailAddress ?? String.Empty;
            txtUpdDateOfBirth.Date = !LegalEntityNaturalPerson.DateOfBirth.HasValue ? new DateTime?() : LegalEntityNaturalPerson.DateOfBirth.Value;
            txtUpdPassportNumber.Text = LegalEntityNaturalPerson.PassportNumber ?? String.Empty;
            txtUpdTaxNumber.Text = LegalEntityNaturalPerson.TaxNumber ?? String.Empty;
            txtIDNumberUpdate.Text = LegalEntityNaturalPerson.IDNumber ?? String.Empty;

            // Bind the Marketing Options
            foreach (ILegalEntityMarketingOption legalEntityMarketingOption in LegalEntityNaturalPerson.LegalEntityMarketingOptions)
            {
                MarketingOptionExcludedListBox.Items.FindByValue(legalEntityMarketingOption.MarketingOption.Key.ToString()).Selected = false;
            }

            #endregion Updateable Controls
        }

        // To be called on prerender
        private void HelperSetLockedUpdateControlsVisible()
        {
            #region Natural Person

            //ddlSalutationUpdate.Visible = true;
            //lblSalutation.Visible = false;

            txtInitialsUpdate.Visible = true;
            lblInitials.Visible = false;

            txtFirstNamesUpdate.Visible = true;
            lblFirstNames.Visible = false;

            txtSurnameUpdate.Visible = true;
            lblSurname.Visible = false;

            ddlGenderUpdate.Visible = true;
            lblGender.Visible = false;

            ddlMaritalStatusUpdate.Visible = true;
            lblMaritalStatus.Visible = false;

            ddlCitizenshipTypeUpdate.Visible = true;
            lblCitizenshipType.Visible = false;

            if (_updateRoleTypeVisible)
            {
                tdNatRoleType.Visible = true;
                ddlRoleTypeUpdate.Visible = true;

                tdCoRoleType.Visible = true;
                ddlCoRoleTypeUpdate.Visible = true;
            }
            else
            {
                ddlRoleTypeUpdate.Visible = false;
                ddlCoRoleTypeUpdate.Visible = false;
            }

            #endregion Natural Person

            #region Company

            COIntroductionDateUpdate.Visible = true;
            lblCOIntroductionDate.Visible = false;

            CORegisteredNameUpdate.Visible = true;
            lblCORegisteredName.Visible = false;

            COLegalEntityTypeUpdate.Visible = true;
            lblCOLegalEntityType.Visible = false;

            #endregion Company
        }

        // To be called on prerender
        private void HelperSetLockedUpdateControlsInvisible()
        {
            txtUpdDateOfBirth.Visible = false;
            txtIDNumberUpdate.Visible = false;
            txtInitialsUpdate.Visible = false;
            txtFirstNamesUpdate.Visible = false;
            txtSurnameUpdate.Visible = false;
            ddlGenderUpdate.Visible = false;
            ddlMaritalStatusUpdate.Visible = false;
            ddlCitizenshipTypeUpdate.Visible = false;

            ddlRoleTypeUpdate.Visible = false;
            ddlCoRoleTypeUpdate.Visible = false;

            CORegistrationNumberUpdate.Visible = false;
            lblCORegistrationNumber.Visible = true;

            //ddlSalutationUpdate.Visible = false;
            //lblSalutation.Visible = true;

            lblCitizenshipType.Visible = true;
            lblInitials.Visible = true;
            lblFirstNames.Visible = true;
            lblSurname.Visible = true;
            lblGender.Visible = true;
            lblMaritalStatus.Visible = true;
            lblIDNumber.Visible = true;
            lblDateofBirth.Visible = true;
        }

        // To be called on prerender
        private void HelperApplicantAddExistingRoleVisible()
        {
            txtUpdDateOfBirth.Enabled = false;

            // Field removed as per user request
            //ddlRoleTypeUpdate.Visible = true;
            //lblRoleTypeUpdate.Visible = true;
            //RoleTypeText.Visible = true;
        }

        // Call this on Prerender
        private void HelperApplicantsUpdateWithActiveFinancialServices()
        {
            // Field removed as per user request
            // ddlRoleTypeUpdate.Visible = true;
            // lblRoleTypeUpdate.Visible = true;
            // RoleTypeText.Visible = true;
            txtUpdDateOfBirth.Enabled = false;
        }

        // Call this on Prerender
        private void HelperDisabledNonContactDetails()
        {
            txtUpdPreferredName.ReadOnly = true;
            txtUpdDateOfBirth.Enabled = false;
            txtUpdPassportNumber.ReadOnly = true;
            txtUpdTaxNumber.ReadOnly = true;
            ddlUpdStatus.Enabled = false;

            if (ddlUpdPopulationGroup.SelectedIndex > 0)
                ddlUpdPopulationGroup.Enabled = false;

            if (ddlEducation.SelectedIndex > 0)
                ddlEducation.Enabled = false;

            if (ddlUpdHomeLanguage.SelectedIndex > 0)
                ddlUpdHomeLanguage.Enabled = false;

            if (ddlUpdDocumentLanguage.SelectedIndex > 0)
                ddlUpdDocumentLanguage.Enabled = false;
        }

        private void HelperUpdateMyDetails()
        {
            tdRole.InnerText = "";
            ddlRoleTypeUpdate.Visible = _RoleTypeVisible;
            lblRoleTypeDisplay.Visible = _RoleTypeVisible;

            tdCitizenshipType.InnerText = "";
            ddlCitizenshipTypeUpdate.Visible = _CitizenTypeVisible;
            lblCitizenshipType.Visible = _CitizenTypeVisible;

            tdDateOfBirth.InnerText = "";
            txtUpdDateOfBirth.Visible = _DateOfBirthVisible;
            lblDateofBirth.Visible = _DateOfBirthVisible;

            tdPassportNumber.InnerText = "";
            txtUpdPassportNumber.Visible = _PassportNumberVisible;
            lblPassportNumber.Visible = _PassportNumberVisible;

            tdTaxNumber.InnerText = "";
            txtUpdTaxNumber.Visible = _TaxNumberVisible;
            lblTaxNumber.Visible = _TaxNumberVisible;

            tdGender.InnerText = "";
            ddlGenderUpdate.Visible = _GenderVisible;
            lblGender.Visible = _GenderVisible;

            tdMaritalStatus.InnerText = "";
            ddlMaritalStatusUpdate.Visible = _MaritalStatusVisible;
            lblMaritalStatus.Visible = _MaritalStatusVisible;

            tdStatus.InnerText = "";
            ddlUpdStatus.Visible = _StatusVisible;
            lblStatus.Visible = _StatusVisible;

            tdPopulationGroup.InnerText = "";
            ddlUpdPopulationGroup.Visible = _PopulationGroupVisible;
            lblPopulationGroup.Visible = _PopulationGroupVisible;

            UpdateInsurableInterestLabel.Visible = _InsurableInterestVisible;
            lblInsurableInterest.Visible = _InsurableInterestVisible;
            ddlNatUpdInsurableInterest.Visible = _InsurableInterestVisible;
        }

        public bool LimitedUpdate
        {
            set { _limitedUpdate = value; }
        }

        public bool ApplicantsUpdateWithActiveFinancialServices
        {
            set { _applicantsUpdateWithActiveFinancialServices = value; }
        }

        public bool ApplicantAddExistingRoleVisible
        {
            set { _applicantAddExistingRoleVisible = value; }
        }

        /// <summary>
        /// Populates the LegalEntity object with details from the screen - Update mode
        /// </summary>
        /// <param name="legalEntity"></param>
        public void PopulateLegalEntityDetailsForUpdate(ILegalEntity legalEntity)
        {
            legalEntity.UserID = SAHLPrincipal.GetCurrent().Identity.Name;

            LegalEntityTypes m_LegalEntityType = (LegalEntityTypes)legalEntity.LegalEntityType.Key;

            switch (m_LegalEntityType)
            {
                case LegalEntityTypes.NaturalPerson:

                    ILegalEntityNaturalPerson leNaturalPerson = legalEntity as ILegalEntityNaturalPerson;

                    if (leNaturalPerson != null)
                    {
                        leNaturalPerson.Salutation = ddlSalutationUpdate.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.Salutations.ObjectDictionary[ddlSalutationUpdate.SelectedValue] : null;
                        leNaturalPerson.Gender = ddlGenderUpdate.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.Genders.ObjectDictionary[ddlGenderUpdate.SelectedValue] : null;
                        leNaturalPerson.MaritalStatus = ddlMaritalStatusUpdate.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.MaritalStatuses.ObjectDictionary[ddlMaritalStatusUpdate.SelectedValue] : null;
                        leNaturalPerson.PopulationGroup = ddlUpdPopulationGroup.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.PopulationGroups.ObjectDictionary[ddlUpdPopulationGroup.SelectedValue] : null;
                        leNaturalPerson.Education = ddlEducation.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.Educations.ObjectDictionary[ddlEducation.SelectedValue] : null;
                        leNaturalPerson.CitizenType = ddlCitizenshipTypeUpdate.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.CitizenTypes.ObjectDictionary[ddlCitizenshipTypeUpdate.SelectedValue] : null;

                        // load up the languages
                        leNaturalPerson.HomeLanguage = GetLanguageFromList(ddlUpdHomeLanguage);
                        leNaturalPerson.DocumentLanguage = GetLanguageFromList(ddlUpdDocumentLanguage);

                        leNaturalPerson.LegalEntityStatus = ddlUpdStatus.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlUpdStatus.SelectedValue] : null;

                        // Removed to sync with branch
                        // leNaturalPerson.ResidenceStatus.Key = Convert.ToInt32(ddlNatResidenceStatus.SelectedValue);

                        leNaturalPerson.Initials = txtInitialsUpdate.Text;
                        leNaturalPerson.FirstNames = txtFirstNamesUpdate.Text;
                        leNaturalPerson.Surname = txtSurnameUpdate.Text;
                        leNaturalPerson.PreferredName = txtUpdPreferredName.Text;

                        leNaturalPerson.IDNumber = txtIDNumberUpdate.Text;
                        leNaturalPerson.PassportNumber = txtUpdPassportNumber.Text;

                        //if (!String.IsNullOrEmpty(txtIDNumberUpdate.Text))
                        //{
                        //    leNaturalPerson.IDNumber = txtIDNumberUpdate.Text;
                        //}
                        //Nazir J => 2008-07-14
                        //leNaturalPerson.SetIDNumber(txtIDNumberUpdate.Text);

                        //if (!String.IsNullOrEmpty(txtUpdPassportNumber.Text))
                        //{
                        //    //Nazir J => 2008-07-14
                        //    //leNaturalPerson.SetPassportNumber(txtUpdPassportNumber.Text, (CitizenTypes)Convert.ToInt32(ddlCitizenshipTypeUpdate.SelectedValue));
                        //    leNaturalPerson.PassportNumber = txtUpdPassportNumber.Text;
                        //}

                        leNaturalPerson.DateOfBirth = txtUpdDateOfBirth.Date;
                        leNaturalPerson.TaxNumber = txtUpdTaxNumber.Text;
                        leNaturalPerson.HomePhoneCode = txtHomePhoneCode.Text;
                        leNaturalPerson.HomePhoneNumber = txtHomePhoneNumber.Text;
                        leNaturalPerson.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leNaturalPerson.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leNaturalPerson.FaxCode = txtFaxCode.Text;
                        leNaturalPerson.FaxNumber = txtFaxNumber.Text;
                        leNaturalPerson.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leNaturalPerson.EmailAddress = txtEmailAddress.Text;
                    }

                    _selectedIncomeContributor = chkNatUpdIncomeContributor.Checked;

                    break;

                case LegalEntityTypes.CloseCorporation:
                    ILegalEntityCloseCorporation leCloseCorporation = legalEntity as ILegalEntityCloseCorporation;

                    if (leCloseCorporation != null)
                    {
                        leCloseCorporation.DocumentLanguage = GetLanguageFromList(ddlCOUpdDocumentLanguage);
                        leCloseCorporation.LegalEntityStatus = ddlCOUpdStatus.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlCOUpdStatus.SelectedValue] : null;

                        leCloseCorporation.RegisteredName = CORegisteredNameUpdate.Text;
                        leCloseCorporation.TradingName = txtCOUpdTradingName.Text;
                        leCloseCorporation.RegistrationNumber = CORegistrationNumberUpdate.Text;
                        leCloseCorporation.TaxNumber = txtCOUpdTaxNumber.Text;
                        leCloseCorporation.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leCloseCorporation.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leCloseCorporation.FaxCode = txtFaxCode.Text;
                        leCloseCorporation.FaxNumber = txtFaxNumber.Text;
                        leCloseCorporation.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leCloseCorporation.EmailAddress = txtEmailAddress.Text;
                    }

                    _selectedIncomeContributor = chkCoUpdIncomeContributor.Checked;

                    break;

                case LegalEntityTypes.Trust:

                    ILegalEntityTrust leTrust = legalEntity as ILegalEntityTrust;

                    if (leTrust != null)
                    {
                        leTrust.DocumentLanguage = GetLanguageFromList(ddlCOUpdDocumentLanguage);
                        leTrust.LegalEntityStatus = ddlCOUpdStatus.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlCOUpdStatus.SelectedValue] : null;

                        leTrust.RegisteredName = CORegisteredNameUpdate.Text;
                        leTrust.TradingName = txtCOUpdTradingName.Text;
                        leTrust.RegistrationNumber = CORegistrationNumberUpdate.Text;
                        leTrust.TaxNumber = txtCOUpdTaxNumber.Text;
                        leTrust.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leTrust.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leTrust.FaxCode = txtFaxCode.Text;
                        leTrust.FaxNumber = txtFaxNumber.Text;
                        leTrust.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leTrust.EmailAddress = txtEmailAddress.Text;
                    }

                    _selectedIncomeContributor = chkCoUpdIncomeContributor.Checked;

                    break;

                case LegalEntityTypes.Company:

                    ILegalEntityCompany leCompany = legalEntity as ILegalEntityCompany;

                    if (leCompany != null)
                    {
                        leCompany.DocumentLanguage = GetLanguageFromList(ddlCOUpdDocumentLanguage);
                        leCompany.LegalEntityStatus = ddlCOUpdStatus.SelectedValue != DEFAULTDROPDOWNITEM ? _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlCOUpdStatus.SelectedValue] : null;

                        leCompany.RegisteredName = CORegisteredNameUpdate.Text;
                        leCompany.TradingName = txtCOUpdTradingName.Text;
                        leCompany.RegistrationNumber = CORegistrationNumberUpdate.Text;
                        leCompany.TaxNumber = txtCOUpdTaxNumber.Text;
                        leCompany.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leCompany.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leCompany.FaxCode = txtFaxCode.Text;
                        leCompany.FaxNumber = txtFaxNumber.Text;
                        leCompany.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leCompany.EmailAddress = txtEmailAddress.Text;
                    }

                    _selectedIncomeContributor = chkCoUpdIncomeContributor.Checked;

                    break;
            }
        }

        /// <summary>
        /// Populates the LegalEntity object with details from the screen - Add Mode
        /// </summary>
        /// <param name="legalEntity"></param>
        public void PopulateLegalEntityDetailsForAdd(ILegalEntity legalEntity)
        {
            legalEntity.UserID = SAHLPrincipal.GetCurrent().Identity.Name;

            LegalEntityTypes m_LegalEntityType = (LegalEntityTypes)legalEntity.LegalEntityType.Key;

            _selectedIncomeContributor = chkAddIncomeContributor.Checked;

            switch (m_LegalEntityType)
            {
                case LegalEntityTypes.NaturalPerson:

                    ILegalEntityNaturalPerson leNaturalPerson = legalEntity as ILegalEntityNaturalPerson;
                    if (leNaturalPerson != null)
                    {
                        if (ddlNatAddSalutation.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.Salutation = _lookupRepository.Salutations.ObjectDictionary[ddlNatAddSalutation.SelectedValue];
                        if (ddlNatAddGender.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.Gender = _lookupRepository.Genders.ObjectDictionary[ddlNatAddGender.SelectedValue];
                        if (ddlNatAddMaritalStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.MaritalStatus = _lookupRepository.MaritalStatuses.ObjectDictionary[ddlNatAddMaritalStatus.SelectedValue];
                        if (ddlNatAddpopulationGroup.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.PopulationGroup = _lookupRepository.PopulationGroups.ObjectDictionary[ddlNatAddpopulationGroup.SelectedValue];
                        if (ddlNatAddEducation.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.Education = _lookupRepository.Educations.ObjectDictionary[ddlNatAddEducation.SelectedValue];
                        if (ddlNatAddCitizenshipType.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.CitizenType = _lookupRepository.CitizenTypes.ObjectDictionary[ddlNatAddCitizenshipType.SelectedValue];
                        leNaturalPerson.HomeLanguage = GetLanguageFromList(ddlNatAddHomeLanguage);
                        leNaturalPerson.DocumentLanguage = GetLanguageFromList(ddlNatAddDocumentLanguage);
                        if (ddlNatAddStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.LegalEntityStatus = _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlNatAddStatus.SelectedValue];

                        // Removed to sync with branch
                        // leNaturalPerson.ResidenceStatus.Key = Convert.ToInt32(ddlNatResidenceStatus.SelectedValue);

                        leNaturalPerson.Initials = txtNatAddInitials.Text;
                        leNaturalPerson.FirstNames = txtNatAddFirstNames.Text;
                        leNaturalPerson.Surname = txtNatAddSurname.Text;
                        leNaturalPerson.PreferredName = txtNatAddPreferredName.Text;

                        if (!String.IsNullOrEmpty(txtNatAddIDNumber.Text))
                        {
                            //Nazir J => 2008-07-14
                            //leNaturalPerson.SetIDNumber(txtNatAddIDNumber.Text);
                            leNaturalPerson.IDNumber = txtNatAddIDNumber.Text;
                        }

                        if (!String.IsNullOrEmpty(txtNatAddPassportNumber.Text))
                        {
                            //Nazir J => 2008-07-14
                            //leNaturalPerson.SetPassportNumber(txtNatAddPassportNumber.Text, (CitizenTypes)Convert.ToInt32(ddlNatAddCitizenshipType.SelectedValue));
                            leNaturalPerson.PassportNumber = txtNatAddPassportNumber.Text;
                        }

                        leNaturalPerson.DateOfBirth = txtNatAddDateOfBirth.Date;
                        leNaturalPerson.TaxNumber = txtNatAddTaxNumber.Text;
                        leNaturalPerson.HomePhoneCode = txtHomePhoneCode.Text;
                        leNaturalPerson.HomePhoneNumber = txtHomePhoneNumber.Text;
                        leNaturalPerson.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leNaturalPerson.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leNaturalPerson.FaxCode = txtFaxCode.Text;
                        leNaturalPerson.FaxNumber = txtFaxNumber.Text;
                        leNaturalPerson.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leNaturalPerson.EmailAddress = txtEmailAddress.Text;
                    }
                    break;

                case LegalEntityTypes.CloseCorporation:

                    ILegalEntityCloseCorporation leCloseCorporation = legalEntity as ILegalEntityCloseCorporation;
                    if (leCloseCorporation != null)
                    {
                        leCloseCorporation.DocumentLanguage = GetLanguageFromList(ddlCOAddDocumentlanguage);
                        if (ddlCOAddStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leCloseCorporation.LegalEntityStatus = _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlCOAddStatus.SelectedValue]; leCloseCorporation.RegisteredName = txtCOAddCompanyName.Text;
                        leCloseCorporation.TradingName = txtCOAddTradingName.Text;
                        leCloseCorporation.RegistrationNumber = txtCOAddRegistrationNumber.Text;
                        leCloseCorporation.TaxNumber = txtCOAddTaxNumber.Text;
                        leCloseCorporation.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leCloseCorporation.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leCloseCorporation.FaxCode = txtFaxCode.Text;
                        leCloseCorporation.FaxNumber = txtFaxNumber.Text;
                        leCloseCorporation.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leCloseCorporation.EmailAddress = txtEmailAddress.Text;
                    }
                    break;

                case LegalEntityTypes.Trust:

                    ILegalEntityTrust leTrust = legalEntity as ILegalEntityTrust;
                    if (leTrust != null)
                    {
                        leTrust.DocumentLanguage = GetLanguageFromList(ddlCOAddDocumentlanguage);
                        if (ddlCOAddStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leTrust.LegalEntityStatus = _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlCOAddStatus.SelectedValue];
                        leTrust.RegisteredName = txtCOAddCompanyName.Text;
                        leTrust.TradingName = txtCOAddTradingName.Text;
                        leTrust.RegistrationNumber = txtCOAddRegistrationNumber.Text;
                        leTrust.TaxNumber = txtCOAddTaxNumber.Text;
                        leTrust.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leTrust.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leTrust.FaxCode = txtFaxCode.Text;
                        leTrust.FaxNumber = txtFaxNumber.Text;
                        leTrust.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leTrust.EmailAddress = txtEmailAddress.Text;
                    }
                    break;

                case LegalEntityTypes.Company:

                    ILegalEntityCompany leCompany = legalEntity as ILegalEntityCompany;
                    if (leCompany != null)
                    {
                        leCompany.DocumentLanguage = GetLanguageFromList(ddlCOAddDocumentlanguage);
                        if (ddlCOAddStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leCompany.LegalEntityStatus = _lookupRepository.LegalEntityStatuses.ObjectDictionary[ddlCOAddStatus.SelectedValue];
                        leCompany.RegisteredName = txtCOAddCompanyName.Text;
                        leCompany.TradingName = txtCOAddTradingName.Text;
                        leCompany.RegistrationNumber = txtCOAddRegistrationNumber.Text;
                        leCompany.TaxNumber = txtCOAddTaxNumber.Text;
                        leCompany.WorkPhoneCode = txtWorkPhoneCode.Text;
                        leCompany.WorkPhoneNumber = txtWorkPhoneNumber.Text;
                        leCompany.FaxCode = txtFaxCode.Text;
                        leCompany.FaxNumber = txtFaxNumber.Text;
                        leCompany.CellPhoneNumber = txtCellPhoneNumber.Text;
                        leCompany.EmailAddress = txtEmailAddress.Text;
                    }
                    break;
            }
        }

        public int SelectedLegalEntityType
        {
            get
            {
                int ret = -1;

                if (ddlLegalEntityTypes.SelectedValue.Length > 0 && !ddlLegalEntityTypes.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
                {
                    ret = Convert.ToInt32(ddlLegalEntityTypes.SelectedValue);
                }
                return ret;
            }
        }

        public int SelectedCOLegalEntityTypeUpdate
        {
            get
            {
                int ret = -1;

                if (COLegalEntityTypeUpdate.SelectedValue.Length > 0 && !COLegalEntityTypeUpdate.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
                {
                    ret = Convert.ToInt32(COLegalEntityTypeUpdate.SelectedValue);
                }
                return ret;
            }
        }

        //public int SelectedRoleType
        //{
        //    get
        //    {
        //        int ret = -1;
        //        if (_panelAddVisible)
        //        {
        //            if (ddlRoleTypeAdd.SelectedValue.Length > 0 && !ddlRoleTypeAdd.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
        //                ret = Convert.ToInt32(ddlRoleTypeAdd.SelectedValue);
        //        }

        //        else
        //        {
        //            if (_panelNaturalPersonDisplayVisible)
        //            {
        //                if (ddlRoleTypeUpdate.SelectedValue.Length > 0 && !ddlRoleTypeUpdate.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
        //                    ret = Convert.ToInt32(ddlRoleTypeUpdate.SelectedValue);
        //            }
        //            else
        //            {
        //                if (ddlCoRoleTypeUpdate.SelectedValue.Length > 0 && !ddlCoRoleTypeUpdate.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
        //                    ret = Convert.ToInt32(ddlCoRoleTypeUpdate.SelectedValue);
        //            }
        //        }
        //        return ret;
        //    }
        //}

        public bool SelectAllMarketingOptionsExcluded
        {
            set { _selectAllMarketingOptions = value; }
        }

        public int SelectedRoleTypAdd
        {
            get
            {
                int ret = -1;
                if (ddlRoleTypeAdd.SelectedValue.Length > 0 && !ddlRoleTypeAdd.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
                    ret = Convert.ToInt32(ddlRoleTypeAdd.SelectedValue);
                return ret;
            }
        }

        public int SelectedRoleTypeUpdateNaturalPerson
        {
            get
            {
                int ret = -1;
                if (ddlRoleTypeUpdate.SelectedValue.Length > 0 && !ddlRoleTypeUpdate.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
                    ret = Convert.ToInt32(ddlRoleTypeUpdate.SelectedValue);
                return ret;
            }
        }

        public int SelectedRoleTypeUpdateCompany
        {
            get
            {
                int ret = -1;
                if (ddlCoRoleTypeUpdate.SelectedValue.Length > 0 && !ddlCoRoleTypeUpdate.SelectedValue.Equals(DEFAULTDROPDOWNITEM))
                    ret = Convert.ToInt32(ddlCoRoleTypeUpdate.SelectedValue);
                return ret;
            }
        }

        public int SelectedInsurableInterestTypeAdd
        {
            get
            {
                int ret = -1;
                if (ddlNatAddInsurableInterest.SelectedIndex > 0)
                    ret = Convert.ToInt32(ddlNatAddInsurableInterest.SelectedValue);
                return ret;
            }
        }

        public int SelectedInsurableInterestTypeUpdate
        {
            get
            {
                int ret = -1;
                if (ddlNatUpdInsurableInterest.SelectedIndex > 0)
                    ret = Convert.ToInt32(ddlNatUpdInsurableInterest.SelectedValue);
                return ret;
            }
        }

        public event KeyChangedEventHandler OnReBindLegalEntity;

        public void BindMarketingOptons(IDictionary<int, string> marketingoptions)
        {
            foreach (KeyValuePair<int, string> MarketingOption in marketingoptions)
            {
                MarketingOptionExcludedListBox.Items.Add(new ListItem(MarketingOption.Value, MarketingOption.Key.ToString()));
            }
            if (_selectAllMarketingOptions)
            {
                foreach (ListItem i in MarketingOptionExcludedListBox.Items)
                    i.Selected = true;
            }
        }

        public bool MarketingOptionsEnabled
        {
            set { _marketingOptionsEnabled = value; }
        }

        public ListItemCollection MarketingOptionsExcluded
        {
            get { return MarketingOptionExcludedListBox.Items; }
        }

        public bool AddRoleTypeVisible
        {
            set { _addRoleTypeVisible = value; }
        }

        public bool LegalEntityTypeEnabled
        {
            set { _legalEntityTypeEnabled = value; }
        }

        public bool DisableAjaxFunctionality
        {
            set { _disableAjaxFunctionality = value; }
        }

        #endregion ILegalEntityDetails Members

        #region Protected Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SwitchUpdateAdd(_updateControlsVisible);

            if (_panelAddVisible)
                chkAddIncomeContributor.Checked = _selectedIncomeContributor;

            pnlAdd.Visible = _panelAddVisible;
            pnlCompanyAdd.Visible = _panelCompanyAddVisible;
            pnlCompanyDisplay.Visible = _panelCompanyDisplayVisible;
            pnlNaturalPersonDisplay.Visible = _panelNaturalPersonDisplayVisible;
            pnlNaturalPersonAdd.Visible = _panelNaturalPersonAddVisible;
            MarketingOptionPanel.Visible = _panelMarketingOptionPanelVisible;

            // hide the home phone table row if a company
            if (pnlCompanyAdd.Visible || pnlCompanyDisplay.Visible)
                trHomePhone.Visible = false;

            ddlNatAddInsurableInterest.Visible = _insurableInterestUpdateVisible;
            ddlNatUpdInsurableInterest.Visible = _insurableInterestUpdateVisible;
            lblInsurableInterest.Visible = _insurableInterestDisplayVisible;
            UpdateInsurableInterestLabel.Visible = _insurableInterestUpdateVisible || _insurableInterestDisplayVisible;

            InsurableInterestLabel.Visible = _insurableInterestDisplayVisible || _insurableInterestUpdateVisible;

            tdNatRoleType.Visible = _displayRoleTypeVisible;
            lblRoleTypeDisplay.Visible = _displayRoleTypeVisible;

            tdCoRoleType.Visible = _displayRoleTypeVisible;
            lblCoRoleTypeDisplay.Visible = _displayRoleTypeVisible;

            if (_applicantRoleTypeKey > 0 && _displayRoleTypeVisible)
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                lblRoleTypeDisplay.Text = _lookupRepository.ApplicationRoleTypes[_applicantRoleTypeKey];
                lblCoRoleTypeDisplay.Text = _lookupRepository.ApplicationRoleTypes[_applicantRoleTypeKey];
            }

            trNatUpdIncomeContributor.Visible = _incomeContributorVisible;
            trCoUpdIncomeContributor.Visible = _incomeContributorVisible;
            trAddIncomeContributor.Visible = _incomeContributorVisible;

            lblCoIncomeContributor.Text = _selectedIncomeContributor ? "Yes" : "No";
            lblNatIncomeContributor.Text = _selectedIncomeContributor ? "Yes" : "No";

            if (_nonContactDetailsDisabled)
                HelperDisabledNonContactDetails();

            if (_applicantsUpdateWithActiveFinancialServices)
                HelperApplicantsUpdateWithActiveFinancialServices();

            if (_applicantAddExistingRoleVisible)
                HelperApplicantAddExistingRoleVisible();

            if (_lockedUpdateControlsVisible)
                HelperSetLockedUpdateControlsVisible();
            else
                HelperSetLockedUpdateControlsInvisible();

            MarketingOptionExcludedListBox.Enabled = _marketingOptionsEnabled;

            btnCancelButton.Visible = _cancelButtonVisible;
            btnSubmitButton.Visible = _submitButtonVisible;

            if (_limitedUpdate)
            {
                txtIDNumberUpdate.Visible = false;
                lblIDNumber.Visible = true;

                txtUpdDateOfBirth.Visible = false;
                lblDateofBirth.Visible = true;

                //CORegistrationNumberUpdate.Visible = false;
                //lblCORegistrationNumber.Visible = true;
            }

            ddlLegalEntityTypes.Enabled = _legalEntityTypeEnabled;

            ddlRoleTypeAdd.Visible = _addRoleTypeVisible;
            tdRole.Visible = _addRoleTypeVisible;

            tdNatRoleType.Visible = _updateRoleTypeVisible || _displayRoleTypeVisible ? true : false;
            ddlRoleTypeUpdate.Visible = _updateRoleTypeVisible;

            tdCoRoleType.Visible = _updateRoleTypeVisible || _displayRoleTypeVisible ? true : false;
            ddlCoRoleTypeUpdate.Visible = _updateRoleTypeVisible;

            if (_panelCompanyAddVisible || _panelCompanyDisplayVisible)
            {
                string selectedCOLegalEntityType = "-select-";
                if (_panelCompanyAddVisible)
                    selectedCOLegalEntityType = ddlLegalEntityTypes.SelectedValue;
                else if (_panelCompanyDisplayVisible)
                    selectedCOLegalEntityType = COLegalEntityTypeUpdate.SelectedValue;

                if (selectedCOLegalEntityType != "-select-" && !String.IsNullOrEmpty(selectedCOLegalEntityType))
                {
                    LegalEntityTypes legalEntityType = (LegalEntityTypes)Convert.ToInt32(selectedCOLegalEntityType);
                    switch (legalEntityType)
                    {
                        case LegalEntityTypes.Company:
                            RegNumberLabel.InnerText = "Registration Number";
                            RegNumberLabelUpdate.InnerText = "Registration Number";
                            break;

                        case LegalEntityTypes.Trust:
                            RegNumberLabel.InnerText = "Trust Number";
                            RegNumberLabelUpdate.InnerText = "Trust Number";
                            break;

                        case LegalEntityTypes.CloseCorporation:
                            RegNumberLabel.InnerText = "CK Number";
                            RegNumberLabelUpdate.InnerText = "CK Number";
                            break;

                        default:
                            break;
                    }
                }
            }

            if (_UpdateMyDetails)
                HelperUpdateMyDetails();
        }

        protected void ddlLegalEntityTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(ddlLegalEntityTypes.SelectedIndex);

            onLegalEntityTypeChange(sender, keyChangedEventArgs);
        }

        protected void btnCancelButton_Click(object sender, EventArgs e)
        {
            onCancelButtonClicked(sender, e);
        }

        protected void btnSubmitButton_Click(object sender, EventArgs e)
        {
            onSubmitButtonClicked(sender, e);
        }

        #endregion Protected Methods

        #region ILegalEntityDetails Members

        public bool InsurableInterestUpdateVisible
        {
            set { _insurableInterestUpdateVisible = value; }
        }

        public bool InsurableInterestDisplayVisible
        {
            set { _insurableInterestDisplayVisible = value; }
        }

        //public bool RoleLeadMainApplicantOnly
        //{
        //    set { _roleLeadMainApplicantOnly = value; }
        //}

        public bool DisplayRoleTypeVisible
        {
            set { _displayRoleTypeVisible = value; }
        }

        public bool UpdateRoleTypeVisible
        {
            set { _updateRoleTypeVisible = value; }
        }

        public int ApplicantRoleTypeKey
        {
            get { return _applicantRoleTypeKey; }
            set { _applicantRoleTypeKey = value; }
        }

        public bool IncomeContributorVisible
        {
            set { _incomeContributorVisible = value; }
        }

        public bool SelectedIncomeContributor
        {
            get
            {
                return _selectedIncomeContributor;
            }
            set
            {
                _selectedIncomeContributor = value;
            }
        }

        #endregion ILegalEntityDetails Members

        /// <summary>
        /// Gets a language provided from a key in a drop-down list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <returns></returns>
        private ILanguage GetLanguageFromList(DropDownList ddl)
        {
            ILanguage lang = null;
            if (ddl.SelectedValue != DEFAULTDROPDOWNITEM)
                lang = _commonRepo.GetLanguageByKey(Int32.Parse(ddl.SelectedValue));
            return lang;
        }
    }
}
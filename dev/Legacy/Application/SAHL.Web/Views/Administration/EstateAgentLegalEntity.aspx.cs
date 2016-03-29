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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Administration
    {
    public partial class EstateAgentLegalEntity : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IEstateAgentLegalEntity
        {
        #region Constants
        const string DEFAULTDROPDOWNITEM = "-select-";
        #endregion

        protected void Page_Load (object sender, EventArgs e)
            {
            if (!ShouldRunPage) return;

            RegisterWebService(ServiceConstants.LegalEntity);
            }

        protected override void OnInit (EventArgs e)
            {
            base.OnInit(e);
            if (!ShouldRunPage) return;
            }

        protected override void OnLoad (EventArgs e)
            {
            base.OnLoad(e);
            if (!ShouldRunPage) return;

            acNatAddIDNumber.ItemSelected += AJAX_ItemSelected;
            acCORegistrationNumberUpdate.ItemSelected += AJAX_ItemSelected;


            }

        protected override void OnPreRender (EventArgs e)
            {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            if (_disableAjaxFunctionality)
                {
                acNatAddIDNumber.Visible = false;
                acCORegistrationNumberUpdate.Visible = false;
                }
            }

        //public void SetUpControlForViewOnly()
        //{
        //    pnlContDet.Controls.Add(tblContactDetails);
        //    pnlAddDet.Controls.Add(lblAddress);
        //    pnlAddressLabel.Visible = false;
        //    apLegalEntityAddDetails.Visible = false;
        //    pnlInfo.Visible = false;
        //    pnlContactDetails.Visible = false;
        //    leSummaryDetails.Visible = true;
        //    pnlLEViewHeader.Visible = true;
        //    tdMain.Controls.Add(pnlLegalEntityDetail);
        //    tdMain.Controls.Add(leSummaryDetails);
        //    apLegalEntityDetails.Visible = false;
        //}

        public void SetUpAddressReadOnly ()
            {
            apLegalEntityAddDetails.Visible = false;
            pnlInfo.Visible = false;
            pnlAddressGrid.Visible = true;
            }

        public void SetupDisplay (bool ReadOnly)
            {
            //view controls
            //-- LE Details
            lblLEType.Visible = ReadOnly;
            lblRole.Visible = ReadOnly;
            lblIDNumber.Visible = ReadOnly;
            lblSalutation.Visible = ReadOnly;
            lblInitials.Visible = ReadOnly;
            lblFirstNames.Visible = ReadOnly;
            lblSurname.Visible = ReadOnly;
            lblPreferredName.Visible = ReadOnly;
            lblGender.Visible = ReadOnly;
            lblStatus.Visible = ReadOnly;
            //lblOrganisationTypeDisplay.Visible = readOnly;
            lblCOIntroductionDate.Visible = ReadOnly;
            lblCORegisteredName.Visible = ReadOnly;
            lblCOTradingName.Visible = ReadOnly;
            lblCORegistrationNumber.Visible = ReadOnly;
            lblCOStatus.Visible = ReadOnly;
            lblHomePhone.Visible = ReadOnly;
            lblWorkPhone.Visible = ReadOnly;
            lblFaxNumber.Visible = ReadOnly;
            lblCellphoneNumber.Visible = ReadOnly;
            lblEmailAddress.Visible = ReadOnly;
            //-- LE Address
            lblAddressType.Visible = ReadOnly;
            pnlAddressLabel.Visible = ReadOnly;
            lblAddressFormat.Visible = ReadOnly;
            lblEffectiveDate.Visible = ReadOnly;
            lblAddress.Visible = ReadOnly;

            //update controls
            //-- LE Details
            //btnSubmitButton.Visible = !readOnly;
            ddlLegalEntityTypes.Visible = !ReadOnly;
            ddlOSDescriptionTypeAdd.Visible = !ReadOnly;
            txtNatAddIDNumber.Visible = !ReadOnly;
            ddlNatAddSalutation.Visible = !ReadOnly;
            txtNatAddInitials.Visible = !ReadOnly;
            txtNatAddFirstNames.Visible = !ReadOnly;
            txtNatAddSurname.Visible = !ReadOnly;
            txtNatAddPreferredName.Visible = !ReadOnly;
            ddlNatAddGender.Visible = !ReadOnly;
            ddlNatAddStatus.Visible = !ReadOnly;
            ddlOrganisationType.Visible = !ReadOnly;
            COIntroductionDateUpdate.Visible = !ReadOnly;
            CORegisteredNameUpdate.Visible = !ReadOnly;
            txtCOUpdTradingName.Visible = !ReadOnly;
            // Robin
            CORegistrationNumberUpdate.Visible = !ReadOnly;
            ddlCOStatus.Visible = !ReadOnly;
            txtHomePhoneCode.Visible = !ReadOnly;
            txtHomePhoneNumber.Visible = !ReadOnly;
            txtWorkPhoneCode.Visible = !ReadOnly;
            txtWorkPhoneNumber.Visible = !ReadOnly;
            txtFaxCode.Visible = !ReadOnly;
            txtFaxNumber.Visible = !ReadOnly;
            txtCellPhoneNumber.Visible = !ReadOnly;
            txtEmailAddress.Visible = !ReadOnly;
            //-- LE Address
            ddlAddressFormat.Visible = !ReadOnly;
            ddlAddressType.Visible = !ReadOnly;
            dtEffectiveDate.Visible = !ReadOnly;
            addressDetails.Visible = !ReadOnly;

            if (ddlLegalEntityTypes.SelectedIndex == 0)
                ddlLegalEntityTypes.SelectedIndex = 2;

            }

        void AJAX_ItemSelected (object sender, KeyChangedEventArgs e)
            {
            if (OnReBindLegalEntity != null)
                OnReBindLegalEntity(sender, e);
            }

        #region Private Helper Functions

        private static void PopulateDropDown (SAHLDropDownList DropDownList, IDictionary<string, string> IDataItems, string DefaultValue)
            {
            PopulateDropDown(DropDownList, IDataItems, DefaultValue, true);
            }
        private static void PopulateDropDown (SAHLDropDownList DropDownList, IDictionary<string, string> IDataItems, string DefaultValue, bool pleaseSelect)
            {
            DropDownList.DataSource = IDataItems;
            DropDownList.DataBind();
            if (pleaseSelect)
                DropDownList.VerifyPleaseSelect();

            // Set the default value if supplied
            if (!String.IsNullOrEmpty(DefaultValue))
                DropDownList.SelectedValue = DefaultValue;
            }

        /// <summary>
        /// Populates the screen update controls with the legalentity details (company,cc,trust) 
        /// </summary>
        /// <param name="LegalEntity"></param>
        private void BindLegalEntityCompany (ILegalEntity LegalEntity)
            {
            string telFormat;

            switch ((LegalEntityTypes)LegalEntity.LegalEntityType.Key)
                {
                case LegalEntityTypes.CloseCorporation:

                    ILegalEntityCloseCorporation leCloseCorporation = LegalEntity as ILegalEntityCloseCorporation;

                    if (leCloseCorporation != null)
                        {
                        ddlLegalEntityTypes.SelectedValue = leCloseCorporation.LegalEntityType.Key.ToString();
                        lblLEType.Text = leCloseCorporation.LegalEntityType == null ? "-" : leCloseCorporation.LegalEntityType.Description;

                        //ddlOrganisationType.SelectedValue = "";
                        //lblOrganisationTypeDisplay.Text = "Not done yet";

                        lblCOIntroductionDate.Text = leCloseCorporation.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
                        COIntroductionDateUpdate.Text = leCloseCorporation.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                        ddlCOStatus.SelectedValue = leCloseCorporation.LegalEntityStatus == null ? DEFAULTDROPDOWNITEM : leCloseCorporation.LegalEntityStatus.Key.ToString();
                        lblCOStatus.Text = leCloseCorporation.LegalEntityStatus == null ? "-" : leCloseCorporation.LegalEntityStatus.Description;

                        lblCORegisteredName.Text = leCloseCorporation.RegisteredName ?? "-";
                        CORegisteredNameUpdate.Text = leCloseCorporation.RegisteredName ?? String.Empty;

                        lblCOTradingName.Text = leCloseCorporation.TradingName ?? "-";
                        txtCOUpdTradingName.Text = leCloseCorporation.TradingName ?? String.Empty;

                        lblCORegistrationNumber.Text = leCloseCorporation.RegistrationNumber ?? "-";
                        CORegistrationNumberUpdate.Text = leCloseCorporation.RegistrationNumber ?? String.Empty;
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
                        ddlLegalEntityTypes.SelectedValue = leTrust.LegalEntityType.Key.ToString();
                        lblLEType.Text = leTrust.LegalEntityType == null ? "-" : leTrust.LegalEntityType.Description;

                        lblCOIntroductionDate.Text = leTrust.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
                        COIntroductionDateUpdate.Text = leTrust.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                        ddlCOStatus.SelectedValue = leTrust.LegalEntityStatus == null ? DEFAULTDROPDOWNITEM : leTrust.LegalEntityStatus.Key.ToString();
                        lblCOStatus.Text = leTrust.LegalEntityStatus == null ? "-" : leTrust.LegalEntityStatus.Description;

                        lblCORegisteredName.Text = leTrust.RegisteredName ?? "-";
                        CORegisteredNameUpdate.Text = leTrust.RegisteredName ?? String.Empty;

                        lblCOTradingName.Text = leTrust.TradingName ?? "-";
                        txtCOUpdTradingName.Text = leTrust.TradingName ?? String.Empty;

                        lblCORegistrationNumber.Text = leTrust.RegistrationNumber ?? "-";
                        CORegistrationNumberUpdate.Text = leTrust.RegistrationNumber ?? String.Empty;
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
                        ddlLegalEntityTypes.SelectedValue = leCompany.LegalEntityType.Key.ToString();
                        lblLEType.Text = leCompany.LegalEntityType == null ? "-" : leCompany.LegalEntityType.Description;

                        lblCOIntroductionDate.Text = leCompany.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
                        COIntroductionDateUpdate.Text = leCompany.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                        ddlCOStatus.SelectedValue = leCompany.LegalEntityStatus == null ? DEFAULTDROPDOWNITEM : leCompany.LegalEntityStatus.Key.ToString();
                        lblCOStatus.Text = leCompany.LegalEntityStatus == null ? "-" : leCompany.LegalEntityStatus.Description;

                        lblCORegisteredName.Text = leCompany.RegisteredName ?? "-";
                        CORegisteredNameUpdate.Text = leCompany.RegisteredName ?? String.Empty;

                        lblCOTradingName.Text = leCompany.TradingName ?? "-";
                        txtCOUpdTradingName.Text = leCompany.TradingName ?? String.Empty;

                        lblCORegistrationNumber.Text = leCompany.RegistrationNumber ?? "-";
                        CORegistrationNumberUpdate.Text = leCompany.RegistrationNumber ?? String.Empty;
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

            BindLegalEntityAddress(LegalEntity);
            }

        /// <summary>
        /// Populates the screen update controls with the legalentity details (natural person) 
        /// </summary>
        /// <param name="LegalEntity"></param>
        private void BindLegalEntityNaturalPerson (ILegalEntity LegalEntity)
            {
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = (ILegalEntityNaturalPerson)LegalEntity;

            string m_FirstNames = LegalEntityNaturalPerson.FirstNames ?? "-";
            string m_Salutation = LegalEntityNaturalPerson.Salutation == null ? "-" : LegalEntityNaturalPerson.Salutation.Description;

            #region ReadOnly Controls
            lblNatIntroductionDate.Text = LegalEntityNaturalPerson.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            //lblLegalEntityName.Text = m_LegalEntityName;
            lblSalutation.Text = m_Salutation;
            lblInitials.Text = LegalEntityNaturalPerson.Initials ?? "-";
            lblFirstNames.Text = m_FirstNames;
            lblSurname.Text = LegalEntityNaturalPerson.Surname ?? "-";
            lblPreferredName.Text = LegalEntityNaturalPerson.PreferredName ?? "-";
            lblGender.Text = LegalEntityNaturalPerson.Gender == null ? "-" : LegalEntityNaturalPerson.Gender.Description;
            lblIDNumber.Text = LegalEntityNaturalPerson.IDNumber ?? "-";
            lblStatus.Text = LegalEntityNaturalPerson.LegalEntityStatus == null ? "-" : LegalEntityNaturalPerson.LegalEntityStatus.Description;
            lblLEType.Text = LegalEntityNaturalPerson.LegalEntityType == null ? "-" : LegalEntityNaturalPerson.LegalEntityType.Description;


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



            #endregion

            #region Updateable Controls

            txtNatAddInitials.Text = LegalEntityNaturalPerson.Initials ?? String.Empty;
            txtNatAddFirstNames.Text = m_FirstNames;
            txtNatAddSurname.Text = LegalEntityNaturalPerson.Surname ?? String.Empty;
            txtNatAddPreferredName.Text = LegalEntityNaturalPerson.PreferredName ?? String.Empty;

            if (LegalEntityNaturalPerson.Salutation != null)
                ddlNatAddSalutation.SelectedValue = LegalEntityNaturalPerson.Salutation.Key.ToString();

            if (LegalEntityNaturalPerson.Gender != null)
                ddlNatAddGender.SelectedValue = LegalEntityNaturalPerson.Gender.Key.ToString();

            if (LegalEntityNaturalPerson.LegalEntityStatus != null)
                ddlNatAddStatus.SelectedValue = LegalEntityNaturalPerson.LegalEntityStatus.Key.ToString();

            txtNatAddIDNumber.Text = LegalEntityNaturalPerson.IDNumber ?? String.Empty;

            txtHomePhoneCode.Text = LegalEntityNaturalPerson.HomePhoneCode ?? String.Empty;
            txtHomePhoneNumber.Text = LegalEntityNaturalPerson.HomePhoneNumber ?? String.Empty;
            txtWorkPhoneCode.Text = LegalEntityNaturalPerson.WorkPhoneCode ?? String.Empty;
            txtWorkPhoneNumber.Text = LegalEntityNaturalPerson.WorkPhoneNumber ?? String.Empty;
            txtFaxCode.Text = LegalEntityNaturalPerson.FaxCode ?? String.Empty;
            txtFaxNumber.Text = LegalEntityNaturalPerson.FaxNumber ?? String.Empty;
            txtCellPhoneNumber.Text = LegalEntityNaturalPerson.CellPhoneNumber ?? String.Empty;
            txtEmailAddress.Text = LegalEntityNaturalPerson.EmailAddress ?? String.Empty;


            #endregion

            BindLegalEntityAddress(LegalEntity);
            }

        #endregion

        #region Events

        public event EventHandler onCancelButtonClicked;

        public event EventHandler onSubmitButtonClicked;

        public event KeyChangedEventHandler OnAddressTypeSelectedIndexChanged;

        public event KeyChangedEventHandler OnAddressFormatSelectedIndexChanged;

        public event KeyChangedEventHandler SelectAddressButtonClicked;

        #endregion

        #region Protected Methods

        protected void ddlLegalEntityTypes_SelectedIndexChanged (object sender, EventArgs e)
            {
            //KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(ddlLegalEntityTypes.SelectedIndex);
            //onLegalEntityTypeChange(sender, keyChangedEventArgs);

            // Make sure that thee is a legal entity type before using the AJAX registration number control
            if (ddlLegalEntityTypes.SelectedIndex > 0)
                CORegistrationNumberUpdate.Visible = true;
            else
                CORegistrationNumberUpdate.Visible = false;
            }



        protected void btnCancelButton_Click (object sender, EventArgs e)
            {
            onCancelButtonClicked(sender, e);
            }

        protected void btnSubmitButton_Click (object sender, EventArgs e)
            {
            if (onSubmitButtonClicked != null)
                onSubmitButtonClicked(sender, e);
            }

        protected void ddlAddressType_SelectedIndexChanged (object sender, EventArgs e)
            {
            if (OnAddressTypeSelectedIndexChanged != null)
                {
                OnAddressTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAddressType.SelectedValue));
                }
            }

        protected void ddlAddressFormat_SelectedIndexChanged (object sender, EventArgs e)
            {
            if (OnAddressFormatSelectedIndexChanged != null)
                OnAddressFormatSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAddressFormat.SelectedValue));
            }

        protected void SelectAddress_Click (object sender, EventArgs e)
            {
            string addressKey = Request.Form["addressKey"];

            if (SelectAddressButtonClicked != null)
                if (!String.IsNullOrEmpty(addressKey))
                    SelectAddressButtonClicked(this, new KeyChangedEventArgs(Int32.Parse(addressKey)));
            }

        #endregion

        #region IEstateAgentLegalEntity Members

        public bool AddressFormatVisible
            {
            set
                {
                ddlAddressFormat.Visible = value;
                lblAddressFormat.Visible = !value;
                }
            }

        public bool AddressTypeVisible
            {
            set
                {
                ddlAddressType.Visible = value;
                lblAddressType.Visible = !value;
                }
            }

        public bool LegalEntityTypeReadOnly
            {
            set
                {
                ddlLegalEntityTypes.Visible = !value;
                lblLEType.Visible = value;
                }
            }

        public bool OSDescriptionTypeAddReadOnly
            {
            set
                {
                ddlOSDescriptionTypeAdd.Visible = !value;
                lblRole.Visible = value;
                }
            }

        public bool OrganisationTypeReadOnly
            {
            set
                {
                ddlOrganisationType.Visible = !value;
                lblOrganisationTypeDisplay.Visible = value;
                }
            }

        public bool SubmitButtonVisible
            {
            set
                {
                btnSubmitButton.Visible = value;
                }
            }

        public int AddressFormatSelectedIndex
            {
            set
                {
                ddlAddressFormat.SelectedIndex = value;
                }
            }

        public AddressFormats GetSetAddressFormat
            {
            get
                {
                return addressDetails.AddressFormat;
                }

            set
                {
                addressDetails.AddressFormat = value;
                }
            }

        public int GetAddressTypeSelectedValue
            {
            get
                {
                if (!string.IsNullOrEmpty(Request.Form[ddlAddressType.UniqueID]))
                    return Convert.ToInt32(Request.Form[ddlAddressType.UniqueID]);
                else if (!string.IsNullOrEmpty(ddlAddressType.SelectedValue))
                    return Convert.ToInt32(ddlAddressType.SelectedValue);
                else
                    return -1;

                //if (!string.IsNullOrEmpty(ddlAddressType.SelectedValue))
                //    return Convert.ToInt32(ddlAddressType.SelectedValue);
                //else
                //    return -1;
                }
            }

        public void BindAddressTypeDropDown (IDictionary<int, string> addressTypes)
            {
            ddlAddressType.DataSource = addressTypes;
            ddlAddressType.DataTextField = "Value";
            ddlAddressType.DataValueField = "Key";
            ddlAddressType.DataBind();
            }

        public int GetAddressFormatSelectedValue
            {
            get
                {
                //if (!string.IsNullOrEmpty(Request.Form[ddlAddressFormat.UniqueID]))
                //    return Convert.ToInt32(Request.Form[ddlAddressFormat.UniqueID]);
                //else if (!string.IsNullOrEmpty(ddlAddressFormat.SelectedValue))
                //    return Convert.ToInt32(ddlAddressFormat.SelectedValue);
                //else
                //    return -1;

                if (!string.IsNullOrEmpty(ddlAddressFormat.SelectedValue))
                    return Convert.ToInt32(ddlAddressFormat.SelectedValue);
                else
                    return -1;
                }
            set
                {
                ddlAddressFormat.SelectedValue = value.ToString();
                }
            }

        public void BindAddressFormatDropDown (IDictionary<int, string> addressFormatList)
            {
            ddlAddressFormat.DataSource = addressFormatList;
            ddlAddressFormat.DataTextField = "Value";
            ddlAddressFormat.DataValueField = "Key";
            ddlAddressFormat.DataBind();
            }

        public DateTime GetSetEffectiveDate
            {
            get
                {
                if (dtEffectiveDate.DateIsValid && dtEffectiveDate.Date.HasValue)
                    return dtEffectiveDate.Date.Value;
                else
                    return DateTime.Today;
                }

            set
                {
                dtEffectiveDate.Date = value;
                }
            }

        public void BindLegalEntityAddress (ILegalEntity legalEntity)
            {
            if (legalEntity.LegalEntityAddresses != null && legalEntity.LegalEntityAddresses.Count > 0)
                {
                ILegalEntityAddress leAddress = legalEntity.LegalEntityAddresses[0];

                BindAddressGrid(legalEntity.LegalEntityAddresses);

                // Get the latest address that has been added to the Legal Entity
                foreach (ILegalEntityAddress leAdd in legalEntity.LegalEntityAddresses)
                    {
                    if (leAdd.Key > leAddress.Key)
                        leAddress = leAdd;
                    }

                pnlAddDet.GroupingText += string.IsNullOrEmpty(leAddress.AddressType.Description) == true ? "" : leAddress.AddressType.Description;
                lblAddressType.Text = string.IsNullOrEmpty(leAddress.AddressType.Description) == true ? "" : leAddress.AddressType.Description;
                ddlAddressType.SelectedValue = System.Convert.IsDBNull(leAddress.AddressType.Key) == true ? "" : leAddress.AddressType.Key.ToString();

                lblEffectiveDate.Text = leAddress.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                dtEffectiveDate.Date = leAddress.EffectiveDate;

                BindAddressDetails(leAddress.Address);
                }
            }

        public void BindAddressDetails (IAddress Address)
            {
            addressDetails.AddressFormat = (AddressFormats)Address.AddressFormat.Key;
            addressDetails.SetAddress(Address);

            lblAddressFormat.Text = string.IsNullOrEmpty(Address.AddressFormat.Description) == true ? "" : Address.AddressFormat.Description;
            ddlAddressFormat.SelectedValue = Address.AddressFormat.Key.ToString();

            lblAddress.Text = Address.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);
            }

        /// <summary>
        /// Populates the LegalEntity object with details from the screen
        /// </summary>
        /// <param name="legalEntity"></param>
        public void PopulateLegalEntityDetails (ILegalEntity legalEntity)
            {
            legalEntity.UserID = SAHLPrincipal.GetCurrent().Identity.Name;

            LegalEntityTypes m_LegalEntityType = (LegalEntityTypes)legalEntity.LegalEntityType.Key;

            switch (m_LegalEntityType)
                {
                case LegalEntityTypes.NaturalPerson:

                    ILegalEntityNaturalPerson leNaturalPerson = legalEntity as ILegalEntityNaturalPerson;
                    if (leNaturalPerson != null)
                        {
                        if (ddlNatAddSalutation.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.Salutation = LKRepo.Salutations.ObjectDictionary[ddlNatAddSalutation.SelectedValue];
                        if (ddlNatAddGender.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.Gender = LKRepo.Genders.ObjectDictionary[ddlNatAddGender.SelectedValue];
                        if (leNaturalPerson.DocumentLanguage == null)
                            leNaturalPerson.DocumentLanguage = GetDefaultLanguage;

                        if (ddlNatAddStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leNaturalPerson.LegalEntityStatus = LKRepo.LegalEntityStatuses.ObjectDictionary[ddlNatAddStatus.SelectedValue];

                        leNaturalPerson.Initials = txtNatAddInitials.Text;
                        leNaturalPerson.FirstNames = txtNatAddFirstNames.Text;
                        leNaturalPerson.Surname = txtNatAddSurname.Text;
                        leNaturalPerson.PreferredName = txtNatAddPreferredName.Text;

                        if (!String.IsNullOrEmpty(txtNatAddIDNumber.Text))
                            {
                            leNaturalPerson.IDNumber = txtNatAddIDNumber.Text;
                            }

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
                        if (ddlCOStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leCloseCorporation.LegalEntityStatus = LKRepo.LegalEntityStatuses.ObjectDictionary[ddlCOStatus.SelectedValue];
                        if (leCloseCorporation.DocumentLanguage == null)
                            leCloseCorporation.DocumentLanguage = GetDefaultLanguage;
                        leCloseCorporation.RegisteredName = CORegisteredNameUpdate.Text;
                        leCloseCorporation.TradingName = txtCOUpdTradingName.Text;
                        leCloseCorporation.RegistrationNumber = CORegistrationNumberUpdate.Text;
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
                        if (ddlCOStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leTrust.LegalEntityStatus = LKRepo.LegalEntityStatuses.ObjectDictionary[ddlCOStatus.SelectedValue];
                        if (leTrust.DocumentLanguage == null)
                            leTrust.DocumentLanguage = GetDefaultLanguage;
                        leTrust.RegisteredName = CORegisteredNameUpdate.Text;
                        leTrust.TradingName = txtCOUpdTradingName.Text;
                        leTrust.RegistrationNumber = CORegistrationNumberUpdate.Text;
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
                        if (ddlCOStatus.SelectedValue != DEFAULTDROPDOWNITEM)
                            leCompany.LegalEntityStatus = LKRepo.LegalEntityStatuses.ObjectDictionary[ddlCOStatus.SelectedValue];
                        if (leCompany.DocumentLanguage == null)
                            leCompany.DocumentLanguage = GetDefaultLanguage;
                        leCompany.RegisteredName = CORegisteredNameUpdate.Text;
                        leCompany.TradingName = txtCOUpdTradingName.Text;
                        leCompany.RegistrationNumber = CORegistrationNumberUpdate.Text;
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

        public void BindLegalEntityTypes (IDictionary<string, string> LegalEntityType, string DefaultValue)
            {
            PopulateDropDown(ddlLegalEntityTypes, LegalEntityType, DefaultValue);
            }

        public void BindSalutation (IDictionary<string, string> Salutation, string DefaultValue)
            {
            PopulateDropDown(ddlNatAddSalutation, Salutation, DefaultValue);
            }

        public void BindGender (IDictionary<string, string> Gender, string DefaultValue)
            {
            PopulateDropDown(ddlNatAddGender, Gender, DefaultValue);
            }

        public void BindLegalEntityStatus (IDictionary<string, string> LegalEntityStatus, string DefaultValue)
            {
            PopulateDropDown(ddlNatAddStatus, LegalEntityStatus, DefaultValue);
            PopulateDropDown(ddlCOStatus, LegalEntityStatus, DefaultValue);
            }

        public void BindIntroductionDate (DateTime IntroductionDate)
            {
            lblNatIntroductionDate.Text = IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblCOIntroductionDate.Text = IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);
            }

        public void BindLegalEntity (ILegalEntity le, string OrgStructureDescription, string OrganisationTypeDescription)
            {
            if (le.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
                BindLegalEntityNaturalPerson(le);
            else
                BindLegalEntityCompany(le);

            if (!String.IsNullOrEmpty(OrgStructureDescription))
                lblRole.Text = OrgStructureDescription;

            if (!String.IsNullOrEmpty(OrganisationTypeDescription))
                lblOrganisationTypeDisplay.Text = OrganisationTypeDescription;

            }

        public void BindOrgStructureDesctiption (IDictionary<string, string> OrgStructureDescriptions, string DefaultValue)
            {
            PopulateDropDown(ddlOSDescriptionTypeAdd, OrgStructureDescriptions, DefaultValue);
            }

        public void BindOrganisationType (IDictionary<string, string> OrganisationTypes, string DefaultValue)
            {
            PopulateDropDown(ddlOrganisationType, OrganisationTypes, DefaultValue);
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

        public IOrganisationType OrganisationType
            {
            get
                {
                if (SelectedLegalEntityType == (int)LegalEntityTypes.NaturalPerson)
                    return LKRepo.OrganisationTypes.ObjectDictionary[Convert.ToInt32(SAHL.Common.Globals.OrganisationTypes.Designation).ToString()];

                //not an LENP
                if (ddlOrganisationType.SelectedValue == DEFAULTDROPDOWNITEM)
                    return null;

                return LKRepo.OrganisationTypes.ObjectDictionary[ddlOrganisationType.SelectedValue];
                }
            }

        public string OrganisationStructureDescription
            {
            get
                {
                if (SelectedLegalEntityType == (int)LegalEntityTypes.NaturalPerson)
                    return ddlOSDescriptionTypeAdd.SelectedValue;

                //not an LENP
                return CORegisteredNameUpdate.Text;
                }
            }

        public bool PanelNaturalPersonAddVisible
            {
            set { pnlNaturalPersonAdd.Visible = value; }
            }

        public bool PanelCompanyAddVisible
            {
            set { pnlCompanyAdd.Visible = value; }
            }

        public event KeyChangedEventHandler OnReBindLegalEntity;

        public bool SetAddressDetailsVisibility
            {
            set
                {
                addressDetails.Visible = value;
                }
            }

        public IAddress GetCapturedAddress
            {
            get
                {
                return addressDetails.GetCapturedAddress();
                }
            }

        public void ClearAddress ()
            {
            addressDetails.ClearInputValues();
            }

        public int GetAjaxAddressKey
            {
            get
                {
                string addressKey = Request.Form["addressKey"];
                if (!string.IsNullOrEmpty(addressKey) && addressKey.Length > 0)
                    return Convert.ToInt32(addressKey);
                else
                    return -1;
                }
            }

        private bool _disableAjaxFunctionality;
        public bool DisableAjaxFunctionality
            {
            set { _disableAjaxFunctionality = value; }
            }

        public void BindAddressGrid (IEventList<ILegalEntityAddress> leAddresses)
            {
            grdAddress.BindAddressList(leAddresses);
            }

        #endregion

        #region Properties

        public string SubmitButtonText
            {
            set { btnSubmitButton.Text = value; }
            }

        private ILookupRepository _lkRepo;

        private ILookupRepository LKRepo
            {
            get
                {
                if (_lkRepo == null)
                    _lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lkRepo;
                }
            }

        private ICommonRepository _cRepo;

        private ICommonRepository CRepo
            {
            get
                {
                if (_cRepo == null)
                    _cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

                return _cRepo;
                }
            }

        /// <summary>
        /// Gets the default language as english
        /// </summary>
        private ILanguage GetDefaultLanguage
            {
            get { return CRepo.GetLanguageByKey(Convert.ToInt16(Languages.English)); }
            }

        #endregion
        }
    }

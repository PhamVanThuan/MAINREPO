using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration
{
    public partial class Attorney : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IAttorney
    {
        #region Properties

        
        public int GetSet_ddlDeedsOffice
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlDeedsOffice.SelectedValue) && ddlDeedsOffice.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlDeedsOffice.SelectedValue);
                else
                    return -1;
            }
            set
            {
                if (ddlDeedsOffice.Items.Count > 0)
                    ddlDeedsOffice.SelectedValue = value.ToString();
            }
        }

        
        public int GetSet_ddlAttorney
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlAttorney.SelectedValue) && ddlAttorney.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlAttorney.SelectedValue);
                else
                    return -1;
            }
            set
            {
                if (ddlAttorney.Items.Count > 0)
                    ddlAttorney.SelectedValue = value.ToString();
            }
        }

        
        public bool Set_tdAttorneyVisibility
        {
            set
            {
                tdAttorney.Visible = value;
            }
        }

        
        public bool Set_ddlAttorneyVisibility
        {
            set
            {
                ddlAttorney.Visible = value;
            }
        }

     
        public bool Set_pnlAttorneyDetailsVisibility
        {
            set
            {
                pnlAttorneyDetails.Visible = value;
            }
        }

     
        public bool Set_tdAttorneyNumberVisibility
        {
            set
            {
                tdAttorneyNumber.Visible = value;
            }
        }

     
        public bool Set_lblAttorneyNumberVisibility
        {
            set
            {
                lblAttorneyNumber.Visible = value;
            }
        }

       
        public bool Set_tdStatusVisibility
        {
            set
            {
                tdStatus.Visible = value;
            }
        }

     
        public bool Set_lblStatusVisibility
        {
            set
            {
                lblStatus.Visible = value;
            }
        }

       
        public bool Set_ddStatusVisibility
        {
            set
            {
                ddlStatus.Visible = value;
            }
        }

       
        public bool Set_lblAttorneyNameVisibility
        {
            set
            {
                lblAttorneyName.Visible = value;
            }
        }

       
        public bool Set_txtAttorneyNameVisibility
        {
            set
            {
                txtAttorneyName.Visible = value;
            }
        }

      
        public bool Set_lblWorkflowEnabledVisibility
        {
            set
            {
                lblWorkflowEnabled.Visible = value;
            }
        }

        public bool Set_ddlWorkflowEnabledVisibility
        {
            set
            {
                ddlWorkflowEnabled.Visible = value;
            }
        }

     
        public bool Set_lblAttorneyContactVisibility
        {
            set
            {
                lblAttorneyContact.Visible = value;
            }
        }

       
        public bool Set_txtAttorneyContactVisibility
        {
            set
            {
                txtAttorneyContact.Visible = value;
            }
        }

       
        public bool Set_lblAttorneyMandateVisibility
        {
            set
            {
                lblAttorneyMandate.Visible = value;
            }
        }

        public bool Set_txtAttorneyMandateVisibility
        {
            set
            {
                txtAttorneyMandate.Visible = value;
            }
        }

     
        public bool Set_lblPhoneNumberVisibility
        {
            set
            {
                lblPhoneNumberCode.Visible = value;
                lblPhoneNumber.Visible = value;
            }
        }

        public bool Set_txtPhoneNumberVisibility
        {
            set
            {
                txtPhoneNumberCode.Visible = value;
                txtPhoneNumber.Visible = value;
            }
        }

        
        public bool Set_lblRegistrationAttorneyVisibility
        {
            set
            {
                lblRegistrationAttorney.Visible = value;
            }
        }

    
        public bool Set_ddlRegistrationAttorneyVisibility
        {
            set
            {
                ddlRegistrationAttorney.Visible = value;
            }
        }

       
        public bool Set_lblEmailAddressVisibility
        {
            set
            {
                lblEmailAddress.Visible = value;
            }
        }

   
        public bool Set_txtEmailAddressVisibility
        {
            set
            {
                txtEmailAddress.Visible = value;
            }
        }

       
        public bool Set_lblLitigationAttorneyVisibility
        {
            set
            {
                lblLitigationAttorney.Visible = value;
            }
        }

   
        public bool Set_ddlLitigationAttorneyVisibility
        {
            set
            {
                ddlLitigationAttorney.Visible = value;
            }
        }

      
        public bool Set_tdDeedsOfficeChangeVisibility
        {
            set
            {
                tdDeedsOfficeChange.Visible = value;
            }
        }

      
        public bool Set_ddlDeedsOfficeChangeVisibility
        {
            set
            {
                ddlDeedsOfficeChange.Visible = value;
            }
        }

     
        public bool Set_pnlAddressVisibility
        {
            set
            {
                pnlAddress.Visible = value;
            }
        }

        public bool Set_lblAddressTypeVisibility
        {
            set
            {
                lblAddressType.Visible = value;
            }
        }

        public bool Set_ddlAddressTypeVisibility
        {
            set
            {
                ddlAddressType.Visible = value;
            }
        }

      
        public bool Set_lblAddressFormatVisibility
        {
            set
            {
                lblAddressFormat.Visible = value;
            }
        }

      
        public bool Set_ddlAddressFormatVisibility
        {
            set
            {
                ddlAddressFormat.Visible = value;
            }
        }

     
        public bool Set_lblEffectiveDateVisibility
        {
            set
            {
                lblEffectiveDate.Visible = value;
            }
        }

     
        public bool Set_dtEffectiveDateVisibility
        {
            set
            {
                dtEffectiveDate.Visible = value;
            }
        }

        public bool Set_lblAddressVisibility
        {
            set
            {
                lblAddress.Visible = value;
            }
        }

   
        public bool Set_addressDetailsVisibility
        {
            set
            {
                addressDetails.Visible = value;
            }
        }

        
        public int Get_ddlStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                {
                    return Convert.ToInt32(ddlStatus.SelectedValue);
                }
                else
                    return (int)GeneralStatuses.Inactive;
            }
        }

     
        public string Get_txtAttorneyName
        {
            get
            {
                return txtAttorneyName.Text;
            }
        }

        
        public int Get_ddlWorkflowEnabled
        {
            get
            {
                if (ddlWorkflowEnabled.SelectedValue == "Yes")
                    return 1;
                else if (ddlWorkflowEnabled.SelectedValue == "No")
                    return 0;
                else
                    return 0;
            }
        }

        
        public string Get_txtAttorneyContact
        {
            get
            {
                return txtAttorneyContact.Text;
            }
        }

        public double Get_txtAttorneyMandate
        {
            get
            {
                return Convert.ToDouble(string.IsNullOrEmpty(txtAttorneyMandate.Text) == true ? "0" : txtAttorneyMandate.Text);
            }
        }

      
        public string Get_txtPhoneNumberCode
        {
            get
            {
                return txtPhoneNumberCode.Text;
            }
        }

        public string Get_txtPhoneNumber
        {
            get
            {
                return txtPhoneNumber.Text;
            }
        }

     
        public bool Get_ddlRegistrationAttorney
        {
            get
            {
                if (ddlRegistrationAttorney.SelectedValue == "Yes")
                    return true;
                else if (ddlRegistrationAttorney.SelectedValue == "No")
                    return false;
                else
                    return false;
            }
        }

    
        public string Get_txtEmailAddress
        {
            get
            {
                return txtEmailAddress.Text;
            }
        }

        public bool Get_ddlLitigationAttorney
        {
            get
            {
                if (ddlLitigationAttorney.SelectedValue == "Yes")
                    return true;
                else if (ddlLitigationAttorney.SelectedValue == "No")
                    return false;
                else
                    return false;
            }
        }

       
        public int Get_ddlDeedsOfficeChange
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlDeedsOfficeChange.SelectedValue) && ddlDeedsOfficeChange.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlDeedsOfficeChange.SelectedValue);
                else
                    return -1;
            }
        }


        public int Get_ddlAddressTypeSelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlAddressType.SelectedValue))
                    return Convert.ToInt32(ddlAddressType.SelectedValue);
                else
                    return -1;
            }
        }


        public int Get_ddlAddressFormatSelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlAddressFormat.SelectedValue))
                    return Convert.ToInt32(ddlAddressFormat.SelectedValue);
                else
                    return -1;
            }
        }

     
        public DateTime GetSet_dtEffectiveDate
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

 
        public AddressFormats GetSet_AddressFormat
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

       
        public IAddress Get_CapturedAddress
        {
            get
            {
                return addressDetails.GetCapturedAddress();
            }
        }

 
        public bool Set_CancelButtonVisibility
        {
            set
            {
                CancelButton.Visible = value;
            }
        }

        public bool Set_SubmitButtonVisibility
        {
            set
            {
                SubmitButton.Visible = value;
            }
        }

        public bool Set_ContactsButtonVisibility
        {
            set
            {
                btnContacts.Visible = value;
            }
        }

        public DateTime Set_EffectiveDateDefault
        {
            set
            {
                dtEffectiveDate.Date = value;
            }
        }

        #endregion Properties

        #region EventHandlers

        /// <summary>
        /// submit button clicked Event Handler
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Contacts button clicked Event Handler
        /// </summary>
        public event EventHandler OnContactsButton_Clicked;

        /// <summary>
        /// Cancel button clicked Event Handler
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Deeds Office ddl changed Event Handler
        /// </summary>
        public event KeyChangedEventHandler OnDeedsOfficeSelectedIndexChanged;

        /// <summary>
        /// Attorney ddl changed Event Handler
        /// </summary>
        public event KeyChangedEventHandler OnAttorneySelectedIndexChanged;

        /// <summary>
        /// AddressType ddl changed Event Handler
        /// </summary>
        public event KeyChangedEventHandler OnAddressTypeSelectedIndexChanged;

        /// <summary>
        /// AddressFormat ddl changed Event Handler
        /// </summary>
        public event KeyChangedEventHandler OnAddressFormatSelectedIndexChanged;

        /// <summary>
        /// SelectAddress button clicked when user selects address on ajax
        /// </summary>
        public event KeyChangedEventHandler SelectAddressButtonClicked;

        #endregion EventHandlers

        #region Events

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            btnContacts.Authenticate += new SAHLSecurityControlEventHandler(btnContacts_Authenticate);
        }

        protected void btnContacts_Authenticate(object source, SAHLSecurityControlEventArgs e)
        {
            if (LitigationAttorneyAuthenticated)
                btnContacts.Visible = true;
            else
                btnContacts.Visible = false;
        }

        public bool LitigationAttorneyAuthenticated
        {
            get { return SecurityHelper.CheckSecurity(btnContacts.SecurityTag, this); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;
        }

        /// <summary>
        /// Cancel Button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Submit button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        /// Contacts button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ContactsButton_Click(object sender, EventArgs e)
        {
            //set a cache key item for the Attorney, and navigate to the new view
            if (OnContactsButton_Clicked != null)
                OnContactsButton_Clicked(sender, e);
        }

        /// <summary>
        /// Deeds Office ddl changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDeedsOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnDeedsOfficeSelectedIndexChanged != null)
                OnDeedsOfficeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlDeedsOffice.SelectedIndex));
        }

        /// <summary>
        /// Attorney ddl changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAttorney_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnAttorneySelectedIndexChanged != null)
                OnAttorneySelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAttorney.SelectedIndex));
        }

        /// <summary>
        /// AddressType ddl changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAddressType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnAddressTypeSelectedIndexChanged != null)
                OnAddressTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAddressType.SelectedIndex));
        }

        /// <summary>
        /// AddressFormat ddl changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAddressFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnAddressFormatSelectedIndexChanged != null)
                OnAddressFormatSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlAddressFormat.SelectedIndex));
        }

        /// <summary>
        /// SelectAddress button Click event - called when user selects address on address ajax - clicked via Java scrip not by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectAddress_Click(object sender, EventArgs e)
        {
            string addressKey = Request.Form["addressKey"];

            if (SelectAddressButtonClicked != null)
                if (!String.IsNullOrEmpty(addressKey))
                    SelectAddressButtonClicked(this, new KeyChangedEventArgs(Int32.Parse(addressKey)));
        }

        #endregion Events

        #region IAttorney Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="deedsOfficeList"></param>
        public void BindDeedsOfficeDropDown(IEventList<IDeedsOffice> deedsOfficeList)
        {
            ddlDeedsOffice.DataSource = deedsOfficeList;
            ddlDeedsOffice.DataValueField = "Key";
            ddlDeedsOffice.DataTextField = "Description";
            ddlDeedsOffice.DataBind();

            ddlDeedsOfficeChange.DataSource = deedsOfficeList;
            ddlDeedsOfficeChange.DataValueField = "Key";
            ddlDeedsOfficeChange.DataTextField = "Description";
            ddlDeedsOfficeChange.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attorneyList"></param>
        public void BindAttorneyDropDown(IDictionary<int, string> attorneyList)
        {
            ddlAttorney.DataSource = attorneyList;
            ddlAttorney.DataValueField = "Key";
            ddlAttorney.DataTextField = "RegisteredName";
            ddlAttorney.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addressTypes"></param>
        public void BindAddressTypeDropDown(IDictionary<int, string> addressTypes)
        {
            ddlAddressType.DataSource = addressTypes;
            ddlAddressType.DataTextField = "Description";
            ddlAddressType.DataValueField = "Key";
            ddlAddressType.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addressFormatList"></param>
        public void BindAddressFormatDropDown(ISAHLDictionary<int, string> addressFormatList)
        {
            ddlAddressFormat.DataSource = addressFormatList;
            ddlAddressFormat.DataTextField = "Description";
            ddlAddressFormat.DataValueField = "Key";
            ddlAddressFormat.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        public void PopulateStatusDropDown(IList<IGeneralStatus> generalStatus)
        {
            ddlStatus.DataSource = generalStatus;
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        public void PopulateWorkflowEnabledDropDown()
        {
            ddlWorkflowEnabled.Items.Add("Yes");
            ddlWorkflowEnabled.Items.Add("No");
        }

        /// <summary>
        ///
        /// </summary>
        public void PopulateRegistrationAttorneyDropDown()
        {
            ddlRegistrationAttorney.Items.Add("Yes");
            ddlRegistrationAttorney.Items.Add("No");
        }

        /// <summary>
        ///
        /// </summary>
        public void PopulateLitigationAttorneyDropDown()
        {
            ddlLitigationAttorney.Items.Add("Yes");
            ddlLitigationAttorney.Items.Add("No");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attorney"></param>
        public void ClearAttorneyControls()
        {
            lblAttorneyNumber.Text = string.Empty;

            lblStatus.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;

            lblAttorneyName.Text = string.Empty;
            txtAttorneyName.Text = string.Empty;

            lblWorkflowEnabled.Text = string.Empty;
            ddlWorkflowEnabled.SelectedIndex = 0;

            lblAttorneyContact.Text = string.Empty;
            txtAttorneyContact.Text = string.Empty;

            lblAttorneyMandate.Text = string.Empty;
            txtAttorneyMandate.Text = string.Empty;

            lblPhoneNumberCode.Text = string.Empty;
            lblPhoneNumber.Text = string.Empty;
            txtPhoneNumberCode.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;

            lblEmailAddress.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;

            ddlDeedsOfficeChange.SelectedIndex = 0;

            lblAddressType.Text = string.Empty;
            ddlAddressType.SelectedIndex = 0;

            lblAddressFormat.Text = string.Empty;

            lblEffectiveDate.Text = string.Empty;
            dtEffectiveDate.Date = DateTime.Today;

            lblAddress.Text = string.Empty;

            ddlAddressFormat.SelectedIndex = 0;
            ClearAddress();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attorney"></param>
        public void PopulateAttorneyControls(IAttorney attorney)
        {
            Set_ContactsButtonVisibility = true;

            lblAttorneyNumber.Text = attorney.Key.ToString();

            lblStatus.Text = System.Convert.IsDBNull(attorney.GeneralStatus.Key) == true ? "Inactive" : attorney.GeneralStatus.Key == (int)GeneralStatuses.Inactive ? "Inactive" : "Active";
            ddlStatus.SelectedValue = System.Convert.IsDBNull(attorney.GeneralStatus.Key) == true ? Convert.ToString(GeneralStatuses.Inactive) : attorney.GeneralStatus.Key.ToString();

            lblAttorneyName.Text = string.IsNullOrEmpty(attorney.LegalEntity.DisplayName) == true ? "" : attorney.LegalEntity.DisplayName;

            ILegalEntityCompany lec = attorney.LegalEntity as ILegalEntityCompany;
            txtAttorneyName.Text = string.IsNullOrEmpty(lec.RegisteredName) == false ? lec.RegisteredName : (string.IsNullOrEmpty(lec.TradingName) == false ? lec.TradingName : "");

            lblWorkflowEnabled.Text = System.Convert.IsDBNull(attorney.AttorneyWorkFlowEnabled) == true ? "No" : attorney.AttorneyWorkFlowEnabled.Value == 0 ? "No" : "Yes";
            ddlWorkflowEnabled.SelectedValue = System.Convert.IsDBNull(attorney.AttorneyWorkFlowEnabled) == true ? "No" : attorney.AttorneyWorkFlowEnabled.Value == 0 ? "No" : "Yes";

            lblAttorneyContact.Text = string.IsNullOrEmpty(attorney.AttorneyContact) == true ? "" : attorney.AttorneyContact;
            txtAttorneyContact.Text = string.IsNullOrEmpty(attorney.AttorneyContact) == true ? "" : attorney.AttorneyContact;

            lblAttorneyMandate.Text = System.Convert.IsDBNull(attorney.AttorneyMandate) == true ? "" : attorney.AttorneyMandate.ToString();
            txtAttorneyMandate.Text = System.Convert.IsDBNull(attorney.AttorneyMandate) == true ? "" : attorney.AttorneyMandate.ToString();

            lblPhoneNumberCode.Text = string.IsNullOrEmpty(attorney.LegalEntity.WorkPhoneCode) == true ? "" : attorney.LegalEntity.WorkPhoneCode + " ";
            lblPhoneNumber.Text = string.IsNullOrEmpty(attorney.LegalEntity.WorkPhoneNumber) == true ? "" : attorney.LegalEntity.WorkPhoneNumber;
            txtPhoneNumberCode.Text = string.IsNullOrEmpty(attorney.LegalEntity.WorkPhoneCode) == true ? "" : attorney.LegalEntity.WorkPhoneCode + " ";
            txtPhoneNumber.Text = string.IsNullOrEmpty(attorney.LegalEntity.WorkPhoneNumber) == true ? "" : attorney.LegalEntity.WorkPhoneNumber;

            lblEmailAddress.Text = string.IsNullOrEmpty(attorney.LegalEntity.EmailAddress) == true ? "" : attorney.LegalEntity.EmailAddress.ToString();
            txtEmailAddress.Text = string.IsNullOrEmpty(attorney.LegalEntity.EmailAddress) == true ? "" : attorney.LegalEntity.EmailAddress.ToString();

            PopulateRegistrationLitigationControls(System.Convert.IsDBNull(attorney.AttorneyRegistrationInd) == true ? false : attorney.AttorneyRegistrationInd, System.Convert.IsDBNull(attorney.AttorneyLitigationInd.ToString()) == true ? false : attorney.AttorneyLitigationInd);

            ddlDeedsOfficeChange.SelectedValue = System.Convert.IsDBNull(GetSet_ddlDeedsOffice) == true ? "-1" : GetSet_ddlDeedsOffice.ToString();

            if (attorney.LegalEntity.LegalEntityAddresses != null && attorney.LegalEntity.LegalEntityAddresses.Count > 0)
            {
                ILegalEntityAddress leAddress = attorney.LegalEntity.LegalEntityAddresses[0];

                // Get the latest address that has been added to the Legal Entity
                foreach (ILegalEntityAddress leAdd in attorney.LegalEntity.LegalEntityAddresses)
                {
                    if (leAdd.Key > leAddress.Key)
                        leAddress = leAdd;
                }

                lblAddressType.Text = string.IsNullOrEmpty(leAddress.AddressType.Description) == true ? "" : leAddress.AddressType.Description;
                ddlAddressType.SelectedValue = System.Convert.IsDBNull(leAddress.AddressType.Key) == true ? "" : leAddress.AddressType.Key.ToString();

                lblEffectiveDate.Text = leAddress.EffectiveDate.ToString();
                dtEffectiveDate.Date = leAddress.EffectiveDate;

                BindAttorneyAddress(leAddress.Address);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="registrationInd"></param>
        /// <param name="litigationInd"></param>
        public void PopulateRegistrationLitigationControls(bool? registrationInd, bool? litigationInd)
        {
            lblRegistrationAttorney.Text = registrationInd == false ? "No" : "Yes";
            ddlRegistrationAttorney.SelectedValue = registrationInd == false ? "No" : "Yes";

            lblLitigationAttorney.Text = litigationInd == false ? "No" : "Yes";
            ddlLitigationAttorney.SelectedValue = litigationInd == false ? "No" : "Yes";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="address"></param>
        public void BindAttorneyAddress(IAddress address)
        {
            addressDetails.AddressFormat = (AddressFormats)address.AddressFormat.Key;
            addressDetails.SetAddress(address);

            lblAddressFormat.Text = string.IsNullOrEmpty(address.AddressFormat.Description) == true ? "" : address.AddressFormat.Description;
            ddlAddressFormat.SelectedValue = address.AddressFormat.Key.ToString();

            lblAddress.Text = address.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="address"></param>
        public void ClearAddress()
        {
            addressDetails.ClearInputValues();
        }

        #endregion IAttorney Methods
    }
}
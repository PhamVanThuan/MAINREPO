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
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration
{
    public partial class SubsidyProvider : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.ISubsidyProvider
    {
        ILookupRepository _lookups;
        private ListItem li;
        private bool _rebindAddress;
        // private bool _rebindSubsidyDetails;

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;

            RegisterWebService(ServiceConstants.Employment);
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();

        }
        /// <summary>
        /// Oninitialise event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAdd.Click+=new EventHandler(btnAdd_Click);
            acSubsidyProvider.ItemSelected+=new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(acSubsidyProvider_ItemSelected);
            ddlAddressFormat.SelectedIndexChanged += new EventHandler(ddlAddressFormat_SelectedIndexChanged);
        }

        public bool AddressDetailsVisible
        {
            set
            {
                addressDetails.Visible = value;
            }
        }

        public bool SetControlsForDisplay
        {
            set
            {
                ddlSubsidyType.Visible = value;
                txtContactPersonEdit.Visible = value;
                txtPhone.Visible = value;
                txtEmailAddressEdit.Visible = value;
                ddlAddressType.Visible = value;
                ddlAddressFormat.Visible = value;
                dtEffectiveDate.Visible = value;
                addressDetails.Visible = value;
            }
        }
        /// <summary>
        /// Implements <see cref="SetControlsForUpdate"/>.
        /// </summary>
        public bool SetControlsForUpdate 
        { 
            set
            {
                lblSubsidyType.Visible = value;
                lblContactPerson.Visible = value;
                lblPhoneNumber.Visible = value;
                lblEmailAddress.Visible = value;
                lblEffectiveDate.Visible = value;
                lblAddressType.Visible = value;
                lblAddressFormat.Visible = value;
                lblAddress.Visible = value;
            }
        }
        /// <summary>
        /// Implements <see cref="SetButtonVisibility"/>
        /// </summary>
        public bool SetButtonVisibility
        {
            set
            {
                CancelButtonVisible = value;
                SubmitButtonVisible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the ajax call to retrieve subsidy providers is enabled.
        /// </summary>
        public bool SubsidyProviderAjaxEnabled
        {
            get
            {
                return acSubsidyProvider.Visible;
            }
            set
            {
                acSubsidyProvider.Visible = value;
            }
        }

        public bool CancelButtonVisible
        {
            set
            {
                CancelButton.Visible = value;
            }
        }

        public bool SubmitButtonVisible
        {
            set
            {
                SubmitButton.Visible = value;
            }
        }

        public bool pnlAddressVisible
        {
            set
            {
                pnlAddress.Visible = value;

            }
        }
        /// <summary>
        /// Implements <see cref="EnableDisableUpdate"/>
        /// </summary>
        public bool EnableDisableUpdate
        {
            set
            {
                SubmitButton.Visible = value ;
                txtSubsidyProvider.Enabled = value;
            }
        }

        public bool SetRebindAddress
        {
            set
            {
                _rebindAddress = value;
            }
        }

        public void DisableSusbidyTypeUpdate()
        {
            ddlSubsidyType.Visible = false;
            lblSubsidyType.Visible = true; 
        }

        void ddlAddressFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressFormat.SelectedValue != "-select-")
                addressDetails.AddressFormat = (AddressFormats)Int32.Parse(ddlAddressFormat.SelectedValue);
            else
                addressDetails.Visible = false;
        }

        /// <summary>
        /// Implements <see cref="Interfaces.ISubsidyProvider.BindDropDowns"/>
        /// </summary>
        /// <param name="lookups"></param>
        public void BindDropDowns(ILookupRepository lookups)
        {
            ddlSubsidyType.DataSource = lookups.SubsidyProviderTypes;
            ddlSubsidyType.DataTextField = "Description";
            ddlSubsidyType.DataValueField = "Key";
            ddlSubsidyType.DataBind();

            //Since we only dealing with postal addresses
            //ddlAddressFormat.DataSource = lookups.AddressFormats;
            ddlAddressFormat.DataSource = lookups.AddressFormatsByAddressType(AddressTypes.Postal);
            ddlAddressFormat.DataTextField = "Description";
            ddlAddressFormat.DataValueField = "Key";
            ddlAddressFormat.DataBind();

            ddlAddressType.DataSource = lookups.AddressTypes;
            ddlAddressType.DataTextField = "Description";
            ddlAddressType.DataValueField = "Key";
            ddlAddressType.DataBind();

            li = ddlAddressType.Items.FindByValue(((int)AddressTypes.Residential).ToString()); // Remove Residential Address
            if (li != null)
                ddlAddressType.Items.Remove(li);

        }
        /// <summary>
        /// </summary>
        /// <param name="subsidyProvider"></param>
        public void BindSubsidyProviderDetail(SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProvider)
        {
            txtSubsidyProvider.Text = subsidyProvider.LegalEntity.DisplayName;
            lblPersalOrganisationCode.Text = subsidyProvider.PersalOrganisationCode;

            if (Messages.Count == 0)
            {
                lblSubsidyType.Text = subsidyProvider.SubsidyProviderType.Description;
                ddlSubsidyType.SelectedValue = subsidyProvider.SubsidyProviderType.Key.ToString();

                lblContactPerson.Text = subsidyProvider.ContactPerson;
                txtContactPersonEdit.Text = subsidyProvider.ContactPerson;

                lblPhoneNumber.Text = subsidyProvider.LegalEntity.WorkPhoneCode + " - " + subsidyProvider.LegalEntity.WorkPhoneNumber;
                txtPhone.Code = subsidyProvider.LegalEntity.WorkPhoneCode;
                txtPhone.Number = subsidyProvider.LegalEntity.WorkPhoneNumber;

                lblEmailAddress.Text = subsidyProvider.LegalEntity.EmailAddress;
                txtEmailAddressEdit.Text = subsidyProvider.LegalEntity.EmailAddress;
            }

            if (subsidyProvider.LegalEntity.LegalEntityAddresses != null && subsidyProvider.LegalEntity.LegalEntityAddresses.Count > 0)
            {
                ILegalEntityAddress _leAddress = subsidyProvider.LegalEntity.LegalEntityAddresses[0];

                // Get the latest address that has been added to the Legal Entity
                foreach (ILegalEntityAddress _leAdd in subsidyProvider.LegalEntity.LegalEntityAddresses)
                {
                    if (_leAdd.Key > _leAddress.Key)
                        _leAddress = _leAdd;
                }

                lblAddressType.Text = _leAddress.AddressType.Description;
                ddlAddressType.SelectedValue = _leAddress.AddressType.Key.ToString();

                lblAddressFormat.Text = _leAddress.Address.AddressFormat.Description;

                lblEffectiveDate.Text = _leAddress.EffectiveDate.ToString();
                //dtEffectiveDate.Text = _leAddress.EffectiveDate.ToString();
                dtEffectiveDate.Date = _leAddress.EffectiveDate;

                lblAddress.Text = _leAddress.Address.GetFormattedDescription(AddressDelimiters.Comma);

                if (ddlAddressFormat.SelectedIndex > 0 && !_rebindAddress)
                    addressDetails.AddressFormat = (AddressFormats)Int32.Parse(ddlAddressFormat.SelectedValue);
                else
                {
                    addressDetails.AddressFormat = (AddressFormats)Int32.Parse(_leAddress.Address.AddressFormat.Key.ToString());
                    ddlAddressFormat.SelectedValue = _leAddress.Address.AddressFormat.Key.ToString();
                }

                addressDetails.SetAddress(_leAddress.Address);
            }
            else
            {
                // set selected index to default value if no address exists
                if (_rebindAddress)
                    ddlAddressFormat.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Implements <see cref="BindSubsidyProviderAddress"/>
        /// </summary>
        /// <param name="address"></param>
        public void BindSubsidyProviderAddress (IAddress address)
        {
            addressDetails.SetAddress(address);

            addressDetails.AddressFormat = (AddressFormats)Int32.Parse(address.AddressFormat.Key.ToString());
        }

        /// <summary>
        /// <returns></returns>
        /// Implements <see cref="GetCapturedSubsidyProvider"/>
        /// </summary>
        /// <param name="subsidyProvider"></param>
        /// <returns></returns>
        public SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider GetCapturedSubsidyProvider(SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProvider)
        {
            subsidyProvider.ContactPerson = txtContactPersonEdit.Text;
            subsidyProvider.LegalEntity.WorkPhoneNumber = txtPhone.Number;
            subsidyProvider.LegalEntity.WorkPhoneCode = txtPhone.Code;
            subsidyProvider.LegalEntity.EmailAddress = txtEmailAddressEdit.Text;

            // This should not been done especially if there is a new address to be saved
            /*IAddress address = addressDetails.GetCapturedAddress();

            if (subsidyProvider.LegalEntity.LegalEntityAddresses != null && subsidyProvider.LegalEntity.LegalEntityAddresses.Count > 0)
            {
                if (subsidyProvider.LegalEntity.LegalEntityAddresses[0].Address != null)
                {
                    // If the address format is changed
                    if (address.GetType().Name == subsidyProvider.LegalEntity.LegalEntityAddresses[0].Address.GetType().Name)
                        address = subsidyProvider.LegalEntity.LegalEntityAddresses[0].Address;
                    else
                        subsidyProvider.LegalEntity.LegalEntityAddresses[0].Address = address;

                    subsidyProvider.LegalEntity.LegalEntityAddresses[0].Address = addressDetails.GetCapturedAddress(address);
                }
                else
                {
                    address = addressDetails.GetCapturedAddress(address);
                    subsidyProvider.LegalEntity.LegalEntityAddresses[0].Address = address;
                }
            }*/
            return subsidyProvider;
        }
        /// <summary>
        /// Implements <see cref="GetSusbidyProviderForAdd"/>
        /// </summary>
        /// <param name="subsidyProviderNew"></param>
        /// <returns></returns>
        public SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider GetSusbidyProviderForAdd(SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProviderNew)
        {
            if (ddlSubsidyType.SelectedValue != "-select-")
                subsidyProviderNew.SubsidyProviderType = _lookups.SubsidyProviderTypes.ObjectDictionary[ddlSubsidyType.SelectedValue];
            
            subsidyProviderNew.ContactPerson = txtContactPersonEdit.Text;
            
            ILegalEntityRepository leRepo;
            ICommonRepository commRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ILegalEntityCompany le = leRepo.GetEmptyLegalEntity(LegalEntityTypes.Company) as ILegalEntityCompany;

            le.EmailAddress = txtEmailAddressEdit.Text;
            le.WorkPhoneNumber = txtPhone.Number;
            le.WorkPhoneCode = txtPhone.Code;
            le.RegisteredName = txtSubsidyProvider.Text;
            le.IntroductionDate = DateTime.Now;


            le.DocumentLanguage = commRepo.GetLanguageByKey((int)Languages.English);
            le.LegalEntityStatus = _lookups.LegalEntityStatuses.ObjectDictionary[((int)LegalEntityStatuses.Alive).ToString()];
            //le.Education = _lookups.Educations.ObjectDictionary[((int)Educations.Unknown).ToString()];
            //le.HomeLanguage = _lookups.Languages.ObjectDictionary[((int)Languages.English).ToString()];

            subsidyProviderNew.LegalEntity = le;

            return subsidyProviderNew;
        }
        /// <summary>
        /// Implements <see cref="GetCapturedSubsidyProviderAddress"/>
        /// </summary>
        /// <param name="subsidyProviderAddress"></param>
        /// <returns></returns>
        public IAddress GetCapturedSubsidyProviderAddress(IAddress subsidyProviderAddress)
        {
            IAddress address = addressDetails.GetCapturedAddress();

            subsidyProviderAddress  = addressDetails.GetCapturedAddress(address);

            return subsidyProviderAddress;
        }
        /// <summary>
        /// Implements <see cref="GetCapturedAddress"/>
        /// </summary>
        public IAddress GetCapturedAddress
        {
            get
            {
                return addressDetails.GetCapturedAddress();
            }
        }
        /// <summary>
        /// Implements <see cref="SelectedAddressTypeValue"/>
        /// </summary>
        public string SelectedAddressTypeValue
        {
            get
            {
                return ddlAddressType.SelectedValue;
            }
        }

        public string SelectedAddressFormatValue
        {
            get
            {
                return ddlAddressFormat.SelectedValue;
            }
        }

        /// <summary>
        /// Add Click event - called when user selects address on address ajax - not meant to be clicked
        /// on by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string addressKey = Request.Form["addressKey"];

            if (AddAddressButtonClicked != null)
            if (!String.IsNullOrEmpty(addressKey))
                AddAddressButtonClicked(this, new KeyChangedEventArgs(Int32.Parse(addressKey)));
        }

        /// <summary>
        /// Cancel Button Clicked
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

        void acSubsidyProvider_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (OnReBindSubsidyDetails != null)
                OnReBindSubsidyDetails(sender, e);
        }

        /// <summary>
        /// Gets the effective date to save to the DB.
        /// </summary>
        public DateTime EffectiveDate
        {
            get
            {
                if (dtEffectiveDate.DateIsValid && dtEffectiveDate.Date.HasValue)
                    return dtEffectiveDate.Date.Value;
                else
                    return DateTime.Now;
            }
            set
            {
                dtEffectiveDate.Date = value;
            }
        }

        /// <summary>
        /// Set the default address type
        /// </summary>
        public int SetAddressType
        {
            set
            {
                ddlAddressType.SelectedValue = value.ToString();
            }
        }

        #region EventHandlers
        /// <summary>
        /// Rebind subsidy details
        /// </summary>
        public event KeyChangedEventHandler OnReBindSubsidyDetails;
        /// <summary>
        /// add address button clicked when user selects address on ajax
        /// </summary>
        public event KeyChangedEventHandler AddAddressButtonClicked;
        /// <summary>
        /// submit button clicked -Event Handler
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// Cancel button clicked -Event Handler
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        #endregion

    }
}

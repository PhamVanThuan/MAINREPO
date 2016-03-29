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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Web.Controls;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// View displaying address information, and allowing users to add, modify or delete address information.
    /// </summary>
    public partial class Address : SAHLCommonBaseView, IAddressView
    {

        private IAddressRepository _addressRepository;
        private ILegalEntity _legalEntity;
        private bool _populateInputFromGrid = true;
        private ILookupRepository _lookUps = RepositoryFactory.GetRepository<ILookupRepository>();

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            // addgrid row bound event handler BEFORE init to ensure we get the event
            // grdAddress.RowDataBound += new GridViewRowEventHandler(grdAddress_RowDataBound);
            base.OnInit(e);
            if (!ShouldRunPage) return;

            // add event handlers
            ddlAddressFormat.SelectedIndexChanged += new EventHandler(ddlAddressFormat_SelectedIndexChanged);
            ddlAddressType.SelectedIndexChanged += new EventHandler(ddlAddressType_SelectedIndexChanged);
            grdAddress.SelectedIndexChanged += new EventHandler(grdAddress_SelectedIndexChanged);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnAuditTrail.Click += new EventHandler(btnAuditTrail_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);

            // set the address format now if there is a posted value - this can be overridden later in the page life cycle
            string postedAddressFormat = Request.Form[ddlAddressFormat.UniqueID];
            if (!String.IsNullOrEmpty(postedAddressFormat))
                AddressFormat = (AddressFormats)Int32.Parse(postedAddressFormat);

        }

        void grdAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdAddress.SelectedAddressSource != AddressSources.FailedLegalEntityAddress)
            {
                IAddress address = SelectedAddress as IAddress;

                if (address == null)
                    address = (SelectedAddress as ILegalEntityAddress).Address;

                AddressFormat = (AddressFormats)address.AddressFormat.Key;
            }
            if (PopulateInputFromGrid)
                PopulateInputForm();

            if (_legalEntity != null)
            {
                if (grdAddress.SelectedAddress != null)
                {
                    ILegalEntityAddress address = grdAddress.SelectedAddress as ILegalEntityAddress;
                    if (address != null)
                    {
                        if (address.LegalEntity.Key != _legalEntity.Key)
                        {
                            btnAdd.Text = "Add";
                        }
                        else
                        {
                            btnAdd.Text = "Update";
                        }
                    }
                }
            }
            if (GridAddressSelectedIndexChanged != null)
                GridAddressSelectedIndexChanged(sender, e);
        }

        void ddlAddressFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdAddress.SelectedAddressSource != AddressSources.FailedLegalEntityAddress)
            {
                // UpdatePostedAddressFormat();
            }
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            string addressKey = Request.Form["addressKey"];
            if (String.IsNullOrEmpty(addressKey))
            {
                if (UpdateButtonClicked != null)
                    UpdateButtonClicked(sender, e);
            }
            else
            {
                if (ExistingAddressSelected != null)
                    ExistingAddressSelected(sender, new KeyChangedEventArgs(addressKey));
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteButtonClicked != null)
                DeleteButtonClicked(sender, e);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            string addressKey = Request.Form["addressKey"];
            if (String.IsNullOrEmpty(addressKey))
            {
                if (AddButtonClicked != null)
                    AddButtonClicked(sender, e);
            }
            else
            {
                if (ExistingAddressSelected != null)
                    ExistingAddressSelected(sender, new KeyChangedEventArgs(addressKey));
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        void btnAuditTrail_Click(object sender, EventArgs e)
        {
            if (AuditButtonClicked != null)
                AuditButtonClicked(sender, e);
        }


        void ddlAddressType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update the addressformat displayed, as it may have changed as not all 
            // address formats are available for each address type - we just try and set it to the 
            // first available option and exit
            ISAHLDictionary<int, string> addressFormats = _lookUps.AddressFormatsByAddressType((AddressTypes)Int32.Parse(ddlAddressType.SelectedValue));
            if (!addressFormats.ContainsKey(((int)AddressFormat)))
            {
                AddressFormat = (AddressFormats)addressFormats.GetKeyAt(0);
            }

            if (SelectedAddressTypeChanged != null)
                SelectedAddressTypeChanged(this, new KeyChangedEventArgs(Int32.Parse(ddlAddressType.SelectedValue)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            if (AddressFormatReadOnly)
            {
                // if we're in read-only mode, clear the format items so we're not confused by posted values
                // when trying to work out the address format after postbacks
                ddlAddressFormat.Items.Clear();
                ddlAddressFormat.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            // update the address format box here
            string sAddressFormat = ((int)this.AddressFormat).ToString();
            if (ddlAddressFormat.Items.FindByValue(sAddressFormat) != null)
                ddlAddressFormat.SelectedValue = sAddressFormat;

            if (pnlInput.Visible)
            {
                if (!IsPostBack && GridPostBack && _populateInputFromGrid)
                    PopulateInputForm();

                // update read-only labels
                lblAddressFormat.Text = AddressRepository.GetAddressFormatByKey(AddressFormat).Description;
                if (SelectedAddress != null)
                {
                    ILegalEntityAddress leAddress = SelectedAddress as ILegalEntityAddress;
                    if (leAddress != null && PopulateInputFromGrid)
                    {
                        lblAddressType.Text = leAddress.AddressType.Description;
                        return;
                    }
                }
            }
            else
            {
                if (SelectedAddress != null)
                {
                    IAddress address = SelectedAddress as IAddress;
                    if (address != null)
                    {
                        lblDisplayAddress.Text = address.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);
                        return;
                    }
                    ILegalEntityAddress leAddress = SelectedAddress as ILegalEntityAddress;
                    if (leAddress != null)
                    {
                        lblDisplayAddress.Text = leAddress.Address.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);
                        return;
                    }
                    IFailedLegalEntityAddress dirtyAddress = SelectedAddress as IFailedLegalEntityAddress;
                    if (dirtyAddress != null)
                    {
                        if (dirtyAddress.FailedPostalMigration != null)
                            lblDisplayAddress.Text = dirtyAddress.FailedPostalMigration.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);
                        else
                            lblDisplayAddress.Text = dirtyAddress.FailedStreetMigration.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);
                        return;
                    }
                }
            }
            lblTitleText.Visible = !String.IsNullOrEmpty(lblTitleText.Text);

            // if it's not a postback and an effective date hasn't been set it, set it to today's date now
            if (!Page.IsPostBack && !dtEffectiveDate.Date.HasValue)
                dtEffectiveDate.Date = DateTime.Today;
        }

        private void PopulateInputForm()
        {
            // load the legal entity address entity
            switch (grdAddress.SelectedAddressSource)
            {
                case AddressSources.Address:
                    IAddress address = grdAddress.SelectedAddress as IAddress;
                    SetAddress(address);
                    break;
                case AddressSources.FailedLegalEntityAddress:
                    addressDetails.ClearInputValues();
                    break;
                case AddressSources.LegalEntityAddress:
                    ILegalEntityAddress leAddress = grdAddress.SelectedAddress as ILegalEntityAddress;
                    SetAddress(leAddress);
                    break;
            }

        }

        protected void OnCalculate_Click(object sender, EventArgs e)
        {
            BackButtonClicked(sender, e);
        }

        #endregion

        #region IAddress Members

        /// <summary>
        /// Gets the address status of a legal entity address.  
        /// </summary>
        public GeneralStatuses AddressStatus
        {
            get
            {
                return (GeneralStatuses)Int32.Parse(ddlAddressStatus.SelectedValue);
            }
        }

        /// <summary>
        /// Gets the effective date to save to the DB.
        /// </summary>
        public DateTime? EffectiveDate 
        {
            get
            {
                if (dtEffectiveDate.DateIsValid && dtEffectiveDate.Date.HasValue)
                    return dtEffectiveDate.Date;
                else
                    return new DateTime?();
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddressDetailsVisible"/>.
        /// </summary>
        public bool AddressDetailsVisible
        {
            set
            {
                pnlDisplay.Visible = true;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddressFormVisible"/>.
        /// </summary>
        public bool AddressFormVisible
        {
            set
            {
                pnlInput.Visible = value;
            }
        }


        /// <summary>
        /// Implements <see cref="IAddressView.AddressListHeaderText"/>.
        /// </summary>
        public string AddressListHeaderText
        {
            set 
            {
                grdAddress.HeaderCaption = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddressListVisible"/>
        /// </summary>
        public bool AddressListVisible
        {
            set 
            {
                grdAddress.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddressRepository"/>.
        /// </summary>
        public IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return _addressRepository;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddressStatusVisible"/>
        /// </summary>
        public bool AddressStatusVisible
        {
            set
            {
                divAddressStatus.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.LegalEntityInfoVisible"/>
        /// </summary>
        public bool LegalEntityInfoVisible
        {
            set
            {
                divLegalEntityInfo.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddButtonVisible"/>.
        /// </summary>
        public bool AddButtonVisible
        {
            set
            {
                btnAdd.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AuditTrailButtonVisible"/>.
        /// </summary>
        public bool AuditTrailButtonVisible
        {
            set 
            {
                btnAuditTrail.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.CancelButtonVisible"/>.
        /// </summary>
        public bool CancelButtonVisible
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.DeleteButtonVisible"/>.
        /// </summary>
        public bool DeleteButtonVisible
        {
            set
            {
                btnDelete.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.UpdateButtonVisible"/>.
        /// </summary>
        public bool UpdateButtonVisible
        {
            set
            {
                btnUpdate.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.ShowInactive"/>.  
        /// </summary>
        /// <see cref="AddressGrid.ShowInactive"/>
        public bool ShowInactive
        {
            get
            {
                return grdAddress.ShowInactive;
            }
            set
            {
                grdAddress.ShowInactive = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.BindAddressList"/>
        /// </summary>
        /// <param name="addresses"></param>
        public void BindAddressList(IEventList<IAddress> addresses)
        {
            grdAddress.BindAddressList(addresses);
        }

        /// <summary>
        /// Implements <see cref="IAddressView.BindAddressList(IEventList&lt;ILegalEntityAddress&gt;)"/>
        /// </summary>
        /// <param name="addresses"></param>
        public void BindAddressList(IEventList<ILegalEntityAddress> addresses)
        {
           
            grdAddress.BindAddressList(addresses);
        }

        /// <summary>
        /// Implements <see cref="IAddressView.BindAddressList(IEventList&lt;ILegalEntityAddress&gt;,IEventList&lt;IFailedLegalEntityAddress&gt;)"/>
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="failedAddresses"></param>
        public void BindAddressList(IEventList<ILegalEntityAddress> addresses, IEventList<IFailedLegalEntityAddress> failedAddresses)
        {
            grdAddress.BindAddressList(addresses, failedAddresses);
        }

        /// <summary>
        /// See <see cref="IAddressView.BindAddressStatuses"/>
        /// </summary>
        /// <param name="addressStatuses"></param>
        public void BindAddressStatuses(ICollection<IGeneralStatus> addressStatuses)
        {
            ddlAddressStatus.DataTextField = "Description";
            ddlAddressStatus.DataValueField = "Key";
            ddlAddressStatus.DataSource = addressStatuses;
            ddlAddressStatus.DataBind();
        }

        /// <summary>
        /// See <see cref="IAddressView.BindAddressTypes"/>
        /// </summary>
        /// <param name="addressTypes"></param>
        public void BindAddressTypes(IDictionary<int, string> addressTypes)
        {
            ddlAddressType.DataSource = addressTypes;
            ddlAddressType.DataBind();
        }


        /// <summary>
        /// See <see cref="IAddressView.BindAddressFormats"/>
        /// </summary>
        /// <param name="addressFormats"></param>
        public void BindAddressFormats(IDictionary<int, string> addressFormats)
        {
            // reset the SelectedValue on the address formats input, asthe list of values changes 
            // depending on the address type selected
            foreach (int key in addressFormats.Keys)
            {
                ddlAddressFormat.SelectedValue = key.ToString();
                break;
            }
            ddlAddressFormat.DataSource = addressFormats;
            ddlAddressFormat.DataBind();

        }

        /// <summary>
        /// Implements <see cref="IAddressView.GridPostBack"/>
        /// </summary>
        public bool GridPostBack
        {
            get
            {
                return (grdAddress.PostBackType == GridPostBackType.SingleClick);
            }
            set
            {
                grdAddress.PostBackType = (value ? GridPostBackType.SingleClick : GridPostBackType.None);
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.GridSelectedIndex"/>
        /// </summary>
        public int GridSelectedIndex
        {
            get
            {
                return grdAddress.SelectedIndex;
            }
            set
            {
                grdAddress.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.SelectedAddress"/>
        /// </summary>
        public object SelectedAddress
        {
            get
            {
                if (grdAddress.PostBackType == GridPostBackType.None) return null;
                return grdAddress.SelectedAddress;
            }
        }

        public AddressSources SelectedAddressSource
        {
            get
            {
                return grdAddress.SelectedAddressSource;
            }
        }


        /// <summary>
        /// Implements <see cref="IAddressView.SelectedAddressTypeValue"/>
        /// </summary>
        public string SelectedAddressTypeValue
        {
            get
            {
                ILegalEntityAddress leAddress = SelectedAddress as ILegalEntityAddress;
                if (leAddress != null)
                    return leAddress.AddressType.Key.ToString();
                else
                    return ddlAddressType.SelectedValue;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.SelectedAddressTypeChanged"/>
        /// </summary>
        public event KeyChangedEventHandler SelectedAddressTypeChanged;


        /// <summary>
        /// Implements <see cref="IAddressView.GridAddressSelectedIndexChanged"/>
        /// </summary>
        public event EventHandler GridAddressSelectedIndexChanged;

        /// <summary>
        /// Implements <see cref="IAddressView.GridColumnTypeVisible"/>.
        /// </summary>
        public bool GridColumnTypeVisible
        {
            set
            {
                grdAddress.TypeColumnVisible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.GridColumnEffectiveDateVisible"/>.
        /// </summary>
        public bool GridColumnEffectiveDateVisible
        {
            set
            {
                grdAddress.EffectiveDateColumnVisible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.GridColumnStatusVisible"/>.
        /// </summary>
        public bool GridColumnStatusVisible
        {
            set
            {
                grdAddress.StatusColumnVisible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the control should automatically populate the input fields with 
        /// the details of the address selected in the grid.  This will default to true.
        /// </summary>
        public bool PopulateInputFromGrid
        {
            get
            {
                return _populateInputFromGrid;
            }
            set
            {
                _populateInputFromGrid = value;
            }
        }

        /// <summary>
        /// Gets/sets the title text displayed on the control.  If this is a set, a title will 
        /// display between the grid and the input form.
        /// </summary>
        public string TitleText
        {
            get
            {
                return lblTitleText.Text;
            }
            set
            {
                lblTitleText.Text = value;
            }
        }


        /// <summary>
        /// Implements <see cref="IAddressView.ExistingAddressSelected"/>.
        /// </summary>
        public event KeyChangedEventHandler ExistingAddressSelected;

        /// <summary>
        /// Implements <see cref="IAddressView.AuditButtonClicked"/>.
        /// </summary>
        public event EventHandler AuditButtonClicked;

        /// <summary>
        /// Implements <see cref="IAddressView.AddButtonClicked"/>.
        /// </summary>
        public event EventHandler AddButtonClicked;

        /// <summary>
        /// Implements <see cref="IAddressView.BackButtonClicked"/>.
        /// </summary>
        public event EventHandler BackButtonClicked;

        /// <summary>
        /// Implements <see cref="IAddressView.CancelButtonClicked"/>.
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Implements <see cref="IAddressView.DeleteButtonClicked"/>.
        /// </summary>
        public event EventHandler DeleteButtonClicked;

        /// <summary>
        /// Implements <see cref="IAddressView.UpdateButtonClicked"/>.
        /// </summary>
        public event EventHandler UpdateButtonClicked;

        /// <summary>
        /// Implements <see cref="IAddressView.AddressFormatReadOnly"/>.
        /// </summary>
        public bool AddressFormatReadOnly
        {
            get
            {
                return lblAddressFormat.Visible;
            }
            set
            {
                lblAddressFormat.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddressTypeReadOnly"/>.
        /// </summary>
        public bool AddressTypeReadOnly
        {
            set
            {
                ddlAddressType.Style.Add(HtmlTextWriterStyle.Display, "none");
                lblAddressType.Visible = true;
            }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.BackButtonVisible"/>.
        /// </summary>
        public bool BackButtonVisible
        {
            set { btnBack.Visible = value;}
        }

        /// <summary>
        /// 
        /// </summary>
        public AddressFormats AddressFormat
        {
            get { return addressDetails.AddressFormat; }

            set { addressDetails.AddressFormat = value; }
        }


        /// <summary>
        /// Implements <see cref="IAddressView.AddButtonText"/>.
        /// </summary>
        public string AddButtonText
        {
            set { btnAdd.Text = value; }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.UpdateButtonText"/>.
        /// </summary>
        public string UpdateButtonText
        {
            set { btnUpdate.Text = value; }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.GetCapturedAddress()"/>
        /// </summary>
        /// <returns></returns>
        public IAddress GetCapturedAddress()
        {
            return addressDetails.GetCapturedAddress();
        }

        public bool ShowLegalEntityNameOnAddressGrid
        {
            set { grdAddress.LegalEntityColumnVisible = value; }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.LegalEntity"/>
        /// </summary>
        public ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
        }

        /// <summary>
        /// Implements <see cref="IAddressView.AddButtonEnabled"/>
        /// </summary>
        public bool AddButtonEnabled
        {
            set { btnAdd.Enabled = value; }
        }


        /// <summary>
        /// Implements <see cref="IAddressView.GetCapturedAddress(IAddress)"/>
        /// </summary>
        /// <returns></returns>
        public IAddress GetCapturedAddress(IAddress address)
        {
            return addressDetails.GetCapturedAddress(address);
        }

        /// <summary>
        /// Implements <see cref="IAddressView.SetAddress(IAddress)"/>
        /// </summary>
        public void SetAddress(IAddress address)
        {
            ddlAddressFormat.SelectedValue = address.AddressFormat.Key.ToString();
            addressDetails.SetAddress(address);
        }
       

     
        /// <summary>
        /// Implements <see cref="IAddressView.SetAddress(ILegalEntityAddress)"/>
        /// </summary>
        public void SetAddress(ILegalEntityAddress address)
        {
            ddlAddressType.SelectedValue = address.AddressType.Key.ToString();
            dtEffectiveDate.Date = address.EffectiveDate;
            ddlAddressStatus.SelectedValue = address.GeneralStatus.Key.ToString();
            SetAddress(address.Address);
        }

        public AddressTypes AddressType
        {
            set
            {
                lblAddressType.Text = _lookUps.AddressTypes[(int)value];
            }
        }

        public bool ButtonRowVisible
        {
            set { ButtonRow.Visible = value; }
        }

        #endregion


    }
}

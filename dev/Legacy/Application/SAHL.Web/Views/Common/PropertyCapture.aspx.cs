using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Utils;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Common
{
    public partial class PropertyCapture : SAHLCommonBaseView, IPropertyCapture
    {
        public event KeyChangedEventHandler OnSearchButtonClicked;

        public event KeyChangedEventHandler OnPropertiesGridSelectedIndexChanged;

        public event KeyChangedEventHandler OnPropertiesGridDoubleClick;

        public event KeyChangedEventHandler OnExistingAddressSelected;

        public event KeyChangedEventHandler OnNewAddressSelected;

        public event KeyChangedEventHandler OnPageChanged;

        public event KeyChangedEventHandler OnPropertySave;

        public event KeyChangedEventHandler OnSavePropertyData;

        private string _sellerID;
        private string _pageNo;
        private DataRow _selectedPropertyData;
        private int _addressKey = -1;
        private Dictionary<string, string> _pData;
        private bool _hasComcorpOfferPropertyDetails = false;

        public bool ButtonRowVisible
        {
            set { ButtonRow.Visible = value; }
        }

        public string SellerID
        {
            set
            {
                _sellerID = value;
            }
        }

        public string Message
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        public string PageNo
        {
            set
            {
                _pageNo = value;

                if (OnPageChanged != null)
                    OnPageChanged(null, new KeyChangedEventArgs(_pageNo));
            }
        }

        public int PropertyIndex
        {
            set
            {
                PropertiesGrid.SelectedIndex = value;
            }
        }

        public DataRow SelectedPropertyData
        {
            set
            {
                _selectedPropertyData = value;
                SetAddress(_selectedPropertyData);
            }
        }

        public int AddressKey
        {
            set
            {
                _addressKey = value;
            }
        }

        public bool HasComcorpOfferPropertyDetails
        {
            set
            {
                _hasComcorpOfferPropertyDetails = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The collection is data that is cached by the presenter.")]
        public Dictionary<string, string> PropertyData
        {
            set
            {
                _pData = value;

                if (_pData != null)
                {
                    txtPropertyDescription1.Text = _pData["Description1"];
                    txtPropertyDescription2.Text = _pData["Description2"];
                    txtPropertyDescription3.Text = _pData["Description3"];

                    if (Convert.ToInt32(_pData["OccupancyType"]) != -1)
                        ddlOccupancyType.SelectedValue = _pData["OccupancyType"];

                    ddlPropertyType.SelectedValue = _pData["PropertyType"];
                    ddlTitleType.SelectedValue = _pData["TitleType"];
                    txtInspectionContact1.Text = _pData["InspectionContact"];
                    txtInspectionTel1.Text = _pData["InspectionPhone"];
                    txtInspectionContact2.Text = _pData["InspectionContact2"];
                    txtInspectionTel2.Text = _pData["InspectionPhone2"];
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_selectedPropertyData != null)
            {
                txtPropertyDescription1.Text = _selectedPropertyData["PROPERTY_DESCRIPTION_1"] != null ? Convert.ToString(_selectedPropertyData["PROPERTY_DESCRIPTION_1"]) : null;
                txtPropertyDescription2.Text = _selectedPropertyData["PROPERTY_DESCRIPTION_2"] != null ? Convert.ToString(_selectedPropertyData["PROPERTY_DESCRIPTION_2"]) : null;
                txtPropertyDescription3.Text = _selectedPropertyData["PROPERTY_DESCRIPTION_3"] != null ? Convert.ToString(_selectedPropertyData["PROPERTY_DESCRIPTION_3"]) : null;
                txtPropertyDescription1.ReadOnly = !String.IsNullOrEmpty(txtPropertyDescription1.Text);
                txtPropertyDescription2.ReadOnly = !String.IsNullOrEmpty(txtPropertyDescription2.Text);
                txtPropertyDescription3.ReadOnly = !String.IsNullOrEmpty(txtPropertyDescription3.Text);
            }
            else
            {
                txtPropertyDescription1.ReadOnly = false;
                txtPropertyDescription2.ReadOnly = false;
                txtPropertyDescription3.ReadOnly = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetPage(_pageNo);
            txtIdentity.Text = _sellerID;

            if (PropertiesGrid.Rows.Count > 0 && PropertiesGrid.SelectedIndex > -1)
            {
                lblSelectedProperty.Text = PropertiesGrid.Rows[PropertiesGrid.SelectedIndex].Cells[2].Text;
            }
            else
            {
                lblSelectedProperty.Text = "";
            }
        }

        public string SetOccupancyTypeValue
        {
            set
            {
                ddlOccupancyType.SelectedValue = value;
            }
        }

        public void BindPropertyTypes(IDictionary<string, string> propertyTypes)
        {
            ddlPropertyType.DataSource = propertyTypes;
            ddlPropertyType.DataBind();
        }

        public void BindTitleTypes(IDictionary<string, string> titleTypes)
        {
            ddlTitleType.DataSource = titleTypes;
            ddlTitleType.DataBind();
        }

        public void BindOccupancyTypes(IDictionary<string, string> occupancyTypes)
        {
            ddlOccupancyType.DataSource = occupancyTypes;
            ddlOccupancyType.DataBind();
        }

        public void BindPropertiesGrid(DataTable DT)
        {
            PropertiesGrid.AddGridBoundColumn("", "PropID", Unit.Percentage(10), HorizontalAlign.Left, true);
            PropertiesGrid.AddGridBoundColumn("", "Erf", Unit.Percentage(10), HorizontalAlign.Left, true);
            PropertiesGrid.AddGridBoundColumn("", "Address", Unit.Percentage(80), HorizontalAlign.Left, true);

            PropertiesGrid.DataSource = DT;
            PropertiesGrid.DataBind();
        }

        protected void PropertiesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                string streetNo = "";
                string streetName = "";
                string sectional = "";
                string suburb = "";
                string postcode = "";
                string town = "";
                string province = "";

                DataRowView drv = e.Row.DataItem as DataRowView;

                if (!String.IsNullOrEmpty(drv.Row["PROP_ID"].ToString()))
                    e.Row.Cells[0].Text = drv.Row["PROP_ID"].ToString().Trim();

                if (!String.IsNullOrEmpty(drv.Row["ERF"].ToString()))
                    e.Row.Cells[1].Text = drv.Row["ERF"].ToString().Trim();

                //unit no
                if (drv.Row["UNIT"] != null && drv.Row["SECTIONAL_TITLE"] != null)
                {
                    string title = drv.Row["SECTIONAL_TITLE"].ToString().Trim();
                    string unit = drv.Row["UNIT"].ToString().Trim();

                    //sectional title
                    if (!String.IsNullOrEmpty(title))
                        sectional = String.Format("Sectional Unit {0} {1}", unit, title);
                    else if (!String.IsNullOrEmpty(unit))
                        sectional = String.Format("Unit {0},", unit);
                }

                //street number
                if (!String.IsNullOrEmpty(drv.Row["STREET_NUMBER"].ToString()))
                    streetNo = drv.Row["STREET_NUMBER"].ToString().Trim();

                //street name
                if (!String.IsNullOrEmpty(drv.Row["STREET_NAME"].ToString()))
                {
                    streetName = drv.Row["STREET_NAME"].ToString().Trim();

                    //street type
                    if (!String.IsNullOrEmpty(drv.Row["STREET_TYPE"].ToString()))
                        streetName += " " + drv.Row["STREET_TYPE"].ToString().Trim();
                }

                //suburb
                if (!String.IsNullOrEmpty(drv.Row["SUBURB"].ToString()))
                    suburb = drv.Row["SUBURB"].ToString().Trim();

                //post code
                if (!String.IsNullOrEmpty(drv.Row["PO_CODE"].ToString()))
                    postcode = drv.Row["PO_CODE"].ToString().Trim();

                //deedtown
                if (!String.IsNullOrEmpty(drv.Row["DEEDTOWN"].ToString()))
                    town += drv.Row["DEEDTOWN"].ToString().Trim();

                //province
                if (!String.IsNullOrEmpty(drv.Row["PROVINCE"].ToString()))
                    province += drv.Row["PROVINCE"].ToString().Trim();

                e.Row.Cells[2].Text = String.Format("{0} {1} {2}, {3} {4}, {5}, {6}", sectional, streetNo, streetName, suburb, postcode, town, province);
            }
        }

        protected void PropertiesGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelectedProperty.Text = PropertiesGrid.Rows[PropertiesGrid.SelectedIndex].Cells[2].Text;

            if (OnPropertiesGridSelectedIndexChanged != null)
                OnPropertiesGridSelectedIndexChanged(sender, new KeyChangedEventArgs(PropertiesGrid.SelectedIndex));
        }

        protected void PropertiesGrid_GridDoubleClick(object sender, GridSelectEventArgs e)
        {
            btnNext_Click(sender, null);
        }

        public void SetAddress(DataRow row)
        {
            if (row == null)
            {
                if (_addressKey > 0)
                {
                    IAddressRepository repo = RepositoryFactory.GetRepository<IAddressRepository>();
                    IAddress address = repo.GetAddressByKey(_addressKey);
                    addressDetails.SetAddress(address);
                }
                return;
            }

            addressDetails.ClearInputValues();

            this.addressDetails.UnitNumber = Convert.ToString(row["UNIT"]).Trim();
            this.addressDetails.BuildingName = Convert.ToString(row["SECTIONAL_TITLE"]).Trim();
            this.addressDetails.StreetNumber = Convert.ToString(row["STREET_NUMBER"]).Trim();
            string streetName = Convert.ToString(row["STREET_NAME"]).Trim();
            string streetType = Convert.ToString(row["STREET_TYPE"]).Trim();

            if (!String.IsNullOrEmpty(streetName))
            {
                streetName = StringUtils.CapitaliseFirstLetter(streetName);

                if (!String.IsNullOrEmpty(streetType))
                    streetName += " " + StringUtils.CapitaliseFirstLetter(streetType);

                this.addressDetails.StreetName = streetName;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtIdentity.Text))
            {
                lblMessage.Text = "Please enter an ID Number and retry or press 'Next' to capture property manually.";
                return;
            }

            lblSelectedProperty.Text = "";
            PropertiesGrid.SelectedIndex = -1;

            if (OnSearchButtonClicked != null)
                OnSearchButtonClicked(sender, new KeyChangedEventArgs(Page.Request.Form[txtIdentity.UniqueID]));

            if (PropertiesGrid.Rows.Count > 0 && PropertiesGrid.SelectedIndex > -1)
            {
                lblSelectedProperty.Text = PropertiesGrid.Rows[0].Cells[2].Text;
            }
        }

        //select an address
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            string addressKey = Request.Form["addressKey"];

            if (String.IsNullOrEmpty(addressKey)) //use the manually entered data in the address control.
            {
                IAddress address;

                try
                {
                    address = addressDetails.GetCapturedAddress();
                }
                catch (Exception)
                {
                    this.Messages.Add(new Error("Failed to create address object, please ensure all relevant fields are entered (Street Number/Street Name/Province/Suburb/PostCode)", ""));
                    return;
                }

                if (address == null)
                {
                    this.Messages.Add(new Error("Address object is null, are all required fields entered?", ""));
                    return;
                }

                IAddressStreet addStreet = address as IAddressStreet;
                //note: move to presenter
                addStreet.ValidateEntity();

                if (Messages.Count > 0)
                    return;

                if (OnNewAddressSelected != null)
                    OnNewAddressSelected(sender, new KeyChangedEventArgs(address));
            }
            else //use the existing address
            {
                if (OnExistingAddressSelected != null)
                    OnExistingAddressSelected(sender, new KeyChangedEventArgs(addressKey));
            }

            if (_addressKey > -1)
                btnNext.Enabled = true;

            btnNext_Click(sender, e);
        }

        private void SavePropertyData()
        {
            _pData = new Dictionary<string, string>();
            _pData.Add("Description1", String.IsNullOrEmpty(txtPropertyDescription1.Text) ? "" : txtPropertyDescription1.Text.Trim());
            _pData.Add("Description2", String.IsNullOrEmpty(txtPropertyDescription2.Text) ? "" : txtPropertyDescription2.Text.Trim());
            _pData.Add("Description3", String.IsNullOrEmpty(txtPropertyDescription3.Text) ? "" : txtPropertyDescription3.Text.Trim());

            if (ddlOccupancyType.SelectedValue != "-select-")
                _pData.Add("OccupancyType", ddlOccupancyType.SelectedValue);
            else
                _pData.Add("OccupancyType", Convert.ToString(-1));

            _pData.Add("PropertyType", ddlPropertyType.SelectedValue);
            _pData.Add("TitleType", ddlTitleType.SelectedValue);
            _pData.Add("InspectionContact", txtInspectionContact1.Text);
            _pData.Add("InspectionPhone", txtInspectionTel1.Text);
            _pData.Add("InspectionContact2", txtInspectionContact2.Text);
            _pData.Add("InspectionPhone2", txtInspectionTel2.Text);

            if (OnSavePropertyData != null)
                OnSavePropertyData(null, new KeyChangedEventArgs(_pData));
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (this.Messages.Count > 0)
                return;

            if (_pageNo == "Property Search")
            {
                SetPage("Address Capture");

                if (OnPropertiesGridDoubleClick != null)
                    OnPropertiesGridDoubleClick(null, new KeyChangedEventArgs(PropertiesGrid.SelectedIndex));
            }
            else if (_pageNo == "Address Capture")
            {
                SetPage("Property Capture");
            }
            else if (_pageNo == "Property Capture")
            {
                //check fields are captured?
                if (!String.IsNullOrEmpty(txtInspectionContact1.Text) && String.IsNullOrEmpty(txtInspectionContact1.Text.Trim()))
                {
                    this.Messages.Add(new Error("Contact1 may not be white space", ""));
                }

                if (!String.IsNullOrEmpty(txtInspectionTel1.Text) && String.IsNullOrEmpty(txtInspectionTel1.Text.Trim()))
                {
                    this.Messages.Add(new Error("Contact1 Phone may not be white space", ""));
                }

                if (this.Messages.ErrorMessages.Count > 0)
                    return;

                SavePropertyData();

                if (OnPropertySave != null)
                    OnPropertySave(null, new KeyChangedEventArgs(null));
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (_pageNo == "Address Capture")
            {
                SetPage("Property Search");
            }
            else if (_pageNo == "Property Capture")
            {
                SavePropertyData();
                SetPage("Address Capture");
            }
        }

        private void SetPage(string pageNo)
        {
            PageNo = pageNo;

            if (pageNo == "Property Search")
            {
                pnlProperty.Visible = true;
                pnlAddress.Visible = false;
                pnlPropertyDetails.Visible = false;
                btnNext.Text = "Next";

                if (!String.IsNullOrEmpty(_sellerID))
                    txtIdentity.Text = _sellerID;

                string searchClicked = Request.Form["searchClicked"];

                if (!String.IsNullOrEmpty(_sellerID) || searchClicked == "true")
                    btnNext.Enabled = true;
                else
                    btnNext.Enabled = false;

                btnPrev.Visible = false;
                btnPrev.Enabled = false;
                pnlSelectedProperty.Visible = true;

                pnlComcorpOfferPropertyDetails.Visible = _hasComcorpOfferPropertyDetails;
                btnCopySellerIDNo.Visible = _hasComcorpOfferPropertyDetails;
            }
            else if (pageNo == "Address Capture")
            {
                pnlProperty.Visible = false;
                pnlAddress.Visible = true;
                pnlPropertyDetails.Visible = false;
                btnNext.Enabled = false;
                btnPrev.Enabled = true;
                btnPrev.Visible = true;
                btnNext.Text = "Next";

                if (PropertiesGrid.DataSource != null && PropertiesGrid.SelectedIndex > -1)
                    pnlSelectedProperty.Visible = true;
                else
                    pnlSelectedProperty.Visible = false;

                if (_addressKey > -1)
                    btnNext.Enabled = true;

                pnlComcorpOfferPropertyDetails.Visible = _hasComcorpOfferPropertyDetails;
                btnCopySellerIDNo.Visible = false;
            }
            else if (pageNo == "Property Capture")
            {
                pnlProperty.Visible = false;
                pnlAddress.Visible = false;
                pnlPropertyDetails.Visible = true;
                btnNext.Enabled = true;
                btnNext.Text = "Finish";
                btnPrev.Enabled = true;
                btnPrev.Visible = true;
                btnPrev.Attributes["onclick"] = "disableValidators('" + pnlPropertyDetails.ClientID + "');";

                if (PropertiesGrid.DataSource != null && PropertiesGrid.SelectedIndex > -1)
                    pnlSelectedProperty.Visible = true;
                else
                    pnlSelectedProperty.Visible = false;

                if (_selectedPropertyData != null)
                {
                    txtPropertyDescription1.Text = _selectedPropertyData["PROPERTY_DESCRIPTION_1"] != null ? Convert.ToString(_selectedPropertyData["PROPERTY_DESCRIPTION_1"]) : null;
                    txtPropertyDescription2.Text = _selectedPropertyData["PROPERTY_DESCRIPTION_2"] != null ? Convert.ToString(_selectedPropertyData["PROPERTY_DESCRIPTION_2"]) : null;
                    txtPropertyDescription3.Text = _selectedPropertyData["PROPERTY_DESCRIPTION_3"] != null ? Convert.ToString(_selectedPropertyData["PROPERTY_DESCRIPTION_3"]) : null;
                    txtPropertyDescription1.ReadOnly = !String.IsNullOrEmpty(txtPropertyDescription1.Text);
                    txtPropertyDescription2.ReadOnly = !String.IsNullOrEmpty(txtPropertyDescription2.Text);
                    txtPropertyDescription3.ReadOnly = !String.IsNullOrEmpty(txtPropertyDescription3.Text);
                }
                else
                {
                    txtPropertyDescription1.ReadOnly = false;
                    txtPropertyDescription2.ReadOnly = false;
                    txtPropertyDescription3.ReadOnly = false;
                }

                pnlComcorpOfferPropertyDetails.Visible = _hasComcorpOfferPropertyDetails;
                btnCopySellerIDNo.Visible = false;
            }
        }

        protected void btnCopySellerIDNo_Click(object sender, EventArgs e)
        {
            _sellerID = lblSellerIDNo.Text;
        }

        public void BindComcorpOfferPropertyDetail(string pageNo, IComcorpOfferPropertyDetails comcorpOfferPropertyDetails)
        {
            lblSellerIDNo.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.SellerIDNo) ? "-" : comcorpOfferPropertyDetails.SellerIDNo;

            if (pageNo == "Property Search" &&
                !string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.SellerIDNo))
                btnCopySellerIDNo.Visible = true;

            string comcorpPropertyAddress = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.SectionalTitleUnitNo) ? "" : comcorpOfferPropertyDetails.SectionalTitleUnitNo + " ";
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.ComplexName) ? "" : comcorpOfferPropertyDetails.ComplexName + ", ");
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.StreetNo) ? "" : comcorpOfferPropertyDetails.StreetNo + " ");
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.StreetName) ? "" : comcorpOfferPropertyDetails.StreetName + ", ");
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.Suburb) ? "" : comcorpOfferPropertyDetails.Suburb + ", ");
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.City) ? "" : comcorpOfferPropertyDetails.City + ", ");
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.Province) ? "" : comcorpOfferPropertyDetails.Province + ", ");
            comcorpPropertyAddress = comcorpPropertyAddress + (string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.PostalCode) ? "" : comcorpOfferPropertyDetails.PostalCode);
            lblComcorpPropertyAddress.Text = comcorpPropertyAddress;

            lblContactCellphone.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.ContactCellphone) ? "-" : comcorpOfferPropertyDetails.ContactCellphone;
            lblContactName.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.ContactName) ? "-" : comcorpOfferPropertyDetails.ContactName;
            lblNamePropertyRegistered.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.NamePropertyRegistered) ? "-" : comcorpOfferPropertyDetails.NamePropertyRegistered;
            lblOccupancyType.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.SAHLOccupancyType) ? "-" : comcorpOfferPropertyDetails.SAHLOccupancyType;
            lblPropertyType.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.SAHLPropertyType) ? "-" : comcorpOfferPropertyDetails.SAHLPropertyType;
            lblTitleType.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.SAHLTitleType) ? "-" : comcorpOfferPropertyDetails.SAHLTitleType;
            lblStandErfNo.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.StandErfNo) ? "-" : comcorpOfferPropertyDetails.StandErfNo;
            lblPortionNo.Text = string.IsNullOrWhiteSpace(comcorpOfferPropertyDetails.PortionNo) ? "-" : comcorpOfferPropertyDetails.PortionNo;
        }
    }
}
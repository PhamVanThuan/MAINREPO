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
using SAHL.Common.Web.UI.Controls;
using System.Text;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;

namespace SAHL.Web.Views.Common
{
    public partial class PropertyDetails : SAHLCommonBaseView, IPropertyDetails
    {
        private bool _showPropertyGrid;
        private bool _showDeedsTransfersGrid;
        private IProperty _selectedProperty;
        private ILookupRepository _lookupRepo;

        private int _selectedAddressKey;
        private bool _valuesChanged;
        private bool _titleDeedNumbersChanged;

        private int _updatedPropertyTypeKey, _updatedTitleTypeKey, _updatedAreaClassificationKey, _updatedDeedsPropertyTypeKey;
        private string _updatedPropertyDesc1, _updatedPropertyDesc2, _updatedPropertyDesc3;
        private string _updatedContact, _updatedContactNumber, _updatedContact2, _updatedContactNumber2;

        private string _updatedErfNumber, _updatedErfPortionNumber, _updatedErfSuburbDescription, 
                       _updatedErfMetroDescription, _updatedSectionalSchemeName, _updatedSectionalUnitNumber;
        private int _updatedOccupancyTypeKey;

        private int _updatedDeedsOfficeKey;
        private string _updatedBondAccountNumber;
        private string _updatedTitleDeedNumbers;

        private string _origionalBondAccountNumber, _origionalTitleDeedNumbers;
        private int _origionalDeedsOfficeKey;
        private int _colPurchaseDate_Transfers = -1, _colRegistrationDate_Transfers = -1;
        private int _colRegistrationDate_Registrations = -1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;

            PropertyAddressGrid.Visible = _showPropertyGrid;

            #region Set visible fields

            txtInspectionContact.Visible = false;
            txtInspectionNumber.Visible = false;
            txtInspectionContact2.Visible = false;
            txtInspectionNumber2.Visible = false;

            switch (_propertyDetailsUpdateMode)
            {
                case PropertyDetailsUpdateMode.Property:
                    txtErfNumber.Visible = false;
                    txtPortionNumber.Visible = false;
                    txtErfSuburb.Visible = false;
                    txtErfMetroDescription.Visible = false;
                    txtSectionalSchemeName.Visible = false;
                    txtSectionalUnitNumber.Visible = false;
                    txtTitleDeedNumber.Visible = false;

                    btnUpdate.Visible = true;
                    btnCancel.Visible = true;
                    ddlOccupancyType.Visible = true;
                    ddlAreaClassification.Visible = true;
                    ddlDeedsPropertyType.Visible = true;
                    ddlPropertyType.Visible = true;
                    ddlTitleType.Visible = true;

                    trUpdatePropertyDesc1.Visible = true;
                    trUpdatePropertyDesc2.Visible = true;
                    trUpdatePropertyDesc3.Visible = true;

                    trDisplayPropertyDesc1.Visible = false;
                    trDisplayPropertyDesc2.Visible = false;

                    txtBondAccountNumber.Visible = false;
                    BondAccountNumber.Visible = true;
                    ddlDeedsOffice.Visible = false;
                    DeedsOffice.Visible = true;

                    AreaClassification.Visible = false;
                    DeedsPropertyType.Visible = false;
                    PropertyType.Visible = false;
                    OccupancyType.Visible = false;
                    TitleType.Visible = false;
 
                    break;
                case PropertyDetailsUpdateMode.Deeds:
                    ddlDeedsPropertyType.Visible = true;
                    btnUpdate.Visible = true;
                    btnCancel.Visible = true;
                    ddlPropertyType.Visible = true;
                    ddlTitleType.Visible = true;
                    ddlOccupancyType.Visible = true;
                    ddlAreaClassification.Visible = true;
                    TitleDeedNumberPanel.Enabled = true;
                    txtTitleDeedNumber.Visible = true;

                    txtBondAccountNumber.Visible = true;
                    BondAccountNumber.Visible = false;
                    ddlDeedsOffice.Visible = true;
                    DeedsOffice.Visible = false;

                    trUpdatePropertyDesc1.Visible = true;
                    trUpdatePropertyDesc2.Visible = true;
                    trUpdatePropertyDesc3.Visible = true;

                    trDisplayPropertyDesc1.Visible = false;
                    trDisplayPropertyDesc2.Visible = false;

                    txtErfNumber.Visible = true;
                    txtPortionNumber.Visible = true;
                    txtErfSuburb.Visible = true;
                    txtErfMetroDescription.Visible = true;
                    txtSectionalSchemeName.Visible = true;
                    txtSectionalUnitNumber.Visible = true;

                    AreaClassification.Visible = false;
                    DeedsPropertyType.Visible = false;
                    PropertyType.Visible = false;
                    TitleType.Visible = false;
                                      
                    OccupancyType.Visible = false;
                    ErfNumber.Visible = false;
                    PortionNumber.Visible = false;
                    ErfSuburb.Visible = false;
                    ErfMetroDescription.Visible = false;
                    SectionalSchemeName.Visible = false;
                    SectionalUnitNumber.Visible = false;
                    TitleDeedNumberPanel.Visible = false; 
                    break;
                case PropertyDetailsUpdateMode.Display:
                case PropertyDetailsUpdateMode.Contact:
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;

                    txtErfNumber.Visible = false;
                    txtPortionNumber.Visible = false;
                    txtErfSuburb.Visible = false;
                    txtErfMetroDescription.Visible = false;
                    txtSectionalSchemeName.Visible = false;
                    txtSectionalUnitNumber.Visible = false;
                    txtTitleDeedNumber.Visible = false;
                    ddlOccupancyType.Visible = false;
                    ddlAreaClassification.Visible = false;
                    ddlDeedsPropertyType.Visible = false;
                    ddlPropertyType.Visible = false;
                    ddlTitleType.Visible = false;

                    trUpdatePropertyDesc1.Visible = false;
                    trUpdatePropertyDesc2.Visible = false;
                    trUpdatePropertyDesc3.Visible = false;

                    trDisplayPropertyDesc1.Visible = true;
                    trDisplayPropertyDesc2.Visible = true;

                    txtBondAccountNumber.Visible = false;
                    BondAccountNumber.Visible = true;
                    ddlDeedsOffice.Visible = false;
                    DeedsOffice.Visible = true;

                    AreaClassification.Visible = true;
                    DeedsPropertyType.Visible = true;
                    PropertyType.Visible = true;
                    TitleType.Visible = true;
                    
                   if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Contact)
                    {
                        btnUpdate.Visible = true;
                        btnCancel.Visible = true;

                        InspectionContact.Visible = false;
                        InspectionNumber.Visible = false;
                        InspectionContact2.Visible = false;
                        InspectionNumber2.Visible = false;

                        txtInspectionContact.Visible = true;
                        txtInspectionNumber.Visible = true;
                        txtInspectionContact2.Visible = true;
                        txtInspectionNumber2.Visible = true;

                    }
                    break;
                default:
                    break;
            }
            #endregion

            // set transfers accordion visibility
            PropertyOwnersGrid.Visible = _showDeedsTransfersGrid;
            BondRegistrationsGrid.Visible = _showDeedsTransfersGrid;
            accTransfers.Visible = _showDeedsTransfersGrid;

            // add the grids to the accordian
            apTransfers.ContentContainer.Controls.Add(PropertyOwnersGrid);
            apRegistrations.ContentContainer.Controls.Add(BondRegistrationsGrid);
        }

        #region IPropertyDetails Members

        public event EventHandler OnCancelButtonClicked;
        public event EventHandler OnUpdateButtonClicked;
        public event KeyChangedEventHandler OnAddressCleanButtonClicked;
        public event KeyChangedEventHandler OnPropertyAddressGridSelectedIndexChanged;

        private PropertyDetailsUpdateMode _propertyDetailsUpdateMode;

        public bool ButtonRowVisible
        {
            set { ButtonRow.Visible = value; }
        }

        public PropertyDetailsUpdateMode PropertyDetailsUpdateMode
        {
            get { return _propertyDetailsUpdateMode; }
            set { _propertyDetailsUpdateMode = value; }
        }
	
        public bool ShowPropertyGrid
        {
            set { _showPropertyGrid = value; }
        }

        public bool ShowDeedsTransfersGrid
        {
            get { return _showDeedsTransfersGrid; }
            set { _showDeedsTransfersGrid = value; }
        }


        public bool ValuesChanged
        {
            get { return _valuesChanged; }
            set { _valuesChanged = value; }
        }
        public bool TitleDeedNumbersChanged
        {
            get { return _titleDeedNumbersChanged; }
            set { _titleDeedNumbersChanged = value; }
        }

        public IProperty SelectedProperty
        {
            get { return _selectedProperty; }
            set { _selectedProperty = value; }

        }

        public int SelectedAddressKey
        {
            get { return _selectedAddressKey; }
            set { _selectedAddressKey = value; }

        }

        public int UpdatedDeedsOfficeKey
        {
            get { return _updatedDeedsOfficeKey; }
        }

        public string UpdatedBondAccountNumber
        {
            get { return _updatedBondAccountNumber; }
        }

        public string UpdatedTitleDeedNumbers
        {
            get { return _updatedTitleDeedNumbers; }
        }

        public string UpdatedContactName
        {
            get { return _updatedContact; }
        }
        public string UpdatedContactNumber
        {
            get { return _updatedContactNumber; }
        }
        public string UpdatedContactName2
        {
            get { return _updatedContact2; }
        }
        public string UpdatedContactNumber2
        {
            get { return _updatedContactNumber2; }
        }

        public void BindPropertyAddressGrid(IEventList<IAddress> lstPropertyAddresses)
        {
            PropertyAddressGrid.HeaderCaption = "Property Details";
            PropertyAddressGrid.EffectiveDateColumnVisible = false;
            PropertyAddressGrid.FormatColumnVisible = false;
            PropertyAddressGrid.StatusColumnVisible = false;
            PropertyAddressGrid.TypeColumnVisible = false;

            PropertyAddressGrid.GridHeight = 50;

            PropertyAddressGrid.BindAddressList(lstPropertyAddresses);
        }

        public void BindPropertyDetails(IProperty property, string bondAccountNumber, int deedsOfficeKey, string lightStonePropertyID, string adCheckPropertyID, string currentDataProvider)
        {
            if (_lookupRepo == null)
                _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            _selectedProperty = property;

            bool fillDisplayFields = true;

            if (property == null)
                return;

            if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Property || _propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds)
            {
                // set the values of the dropdown lists
                ddlPropertyType.SelectedValue = property.PropertyType == null ? SAHL.Common.Constants.DefaultDropDownItem : property.PropertyType.Key.ToString();
                ddlTitleType.SelectedValue = property.TitleType == null ? SAHL.Common.Constants.DefaultDropDownItem : property.TitleType.Key.ToString();
                ddlOccupancyType.SelectedValue = property.OccupancyType == null ? SAHL.Common.Constants.DefaultDropDownItem : property.OccupancyType.Key.ToString();
                ddlAreaClassification.SelectedValue = property.AreaClassification == null ? SAHL.Common.Constants.DefaultDropDownItem : property.AreaClassification.Key.ToString();
                ddlDeedsPropertyType.SelectedValue = property.DeedsPropertyType == null ? SAHL.Common.Constants.DefaultDropDownItem : property.DeedsPropertyType.Key.ToString();

                txtBondAccountNumber.Text = bondAccountNumber;
                ddlDeedsOffice.SelectedValue = deedsOfficeKey.ToString();

                _origionalDeedsOfficeKey = ddlDeedsOffice.SelectedIndex <= 0 ? 0 : Convert.ToInt32(ddlDeedsOffice.SelectedItem.Value);
                _origionalBondAccountNumber = txtBondAccountNumber.Text;

                // set the values of the update fields
                txtPropertyDescription1.Text = property.PropertyDescription1 == null ? "" : property.PropertyDescription1;
                txtPropertyDescription2.Text = property.PropertyDescription2 == null ? "" : property.PropertyDescription2;
                txtPropertyDescription3.Text = property.PropertyDescription3 == null ? "" : property.PropertyDescription3;

                if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds)
                {
                    txtErfNumber.Text = property.ErfNumber == null ? "" : property.ErfNumber;
                    txtPortionNumber.Text = property.ErfPortionNumber == null ? "" : property.ErfPortionNumber;
                    txtErfSuburb.Text = property.ErfSuburbDescription == null ? "" : property.ErfSuburbDescription;
                    txtErfMetroDescription.Text = property.ErfMetroDescription == null ? "" : property.ErfMetroDescription;
                    txtSectionalSchemeName.Text = property.SectionalSchemeName == null ? "" : property.SectionalSchemeName;
                    txtSectionalUnitNumber.Text = property.SectionalUnitNumber == null ? "" : property.SectionalUnitNumber;
                    _origionalTitleDeedNumbers = GetDeedNumbers(property,false);
                    txtTitleDeedNumber.Text = _origionalTitleDeedNumbers == null ? "" : _origionalTitleDeedNumbers;
                }
            }

            if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Contact && property.PropertyAccessDetails != null)
            {
                txtInspectionContact.Text = property.PropertyAccessDetails.Contact1 == null ? "" : property.PropertyAccessDetails.Contact1;
                txtInspectionContact2.Text = property.PropertyAccessDetails.Contact2 == null ? "" : property.PropertyAccessDetails.Contact2;

                txtInspectionNumber.Text = property.PropertyAccessDetails.Contact1Phone == null ? "" : property.PropertyAccessDetails.Contact1Phone;
                txtInspectionNumber2.Text = property.PropertyAccessDetails.Contact2Phone == null ? "" : property.PropertyAccessDetails.Contact2Phone;

            }

            if (fillDisplayFields)
            {
                PropertyType.Text = property.PropertyType == null ? "-" : property.PropertyType.Description;
                TitleType.Text = property.TitleType == null ? "-" : property.TitleType.Description;
                OccupancyType.Text = property.OccupancyType == null ? "-" : property.OccupancyType.Description;
                AreaClassification.Text = property.AreaClassification == null ? "-" : property.AreaClassification.Description;

                PropertyDescription.Text = property.PropertyDescription1 == null ? "-" : property.PropertyDescription1;
                if (!String.IsNullOrEmpty(property.PropertyDescription2))
                    PropertyDescription.Text += "\r\n" + property.PropertyDescription2;
                if (!String.IsNullOrEmpty(property.PropertyDescription3))
                    PropertyDescription.Text += "\r\n" + property.PropertyDescription3;

                ErfNumber.Text = property.ErfNumber == null || property.ErfNumber.Length == 0 ? "-" : property.ErfNumber;
                PortionNumber.Text = property.ErfPortionNumber == null || property.ErfPortionNumber.Length == 0 ? "-" : property.ErfPortionNumber;
                ErfSuburb.Text = property.ErfSuburbDescription == null || property.ErfSuburbDescription.Length == 0 ? "-" : property.ErfSuburbDescription;
                ErfMetroDescription.Text = property.ErfMetroDescription == null || property.ErfMetroDescription.Length == 0 ? "-" : property.ErfMetroDescription;
                SectionalSchemeName.Text = property.SectionalSchemeName == null || property.SectionalSchemeName.Length == 0 ? "-" : property.SectionalSchemeName;
                SectionalUnitNumber.Text = property.SectionalUnitNumber == null || property.SectionalUnitNumber.Length == 0 ? "-" : property.SectionalUnitNumber;

                TitleDeedNumber.Text = GetDeedNumbers(property, true);
                DeedsPropertyType.Text = property.DeedsPropertyType == null ? "-" : property.DeedsPropertyType.Description;

                BondAccountNumber.Text = String.IsNullOrEmpty(bondAccountNumber) ? "-" : bondAccountNumber;
                DeedsOffice.Text = deedsOfficeKey == 0 ? "-" : _lookupRepo.DeedsOffice.ObjectDictionary[Convert.ToString(deedsOfficeKey)].Description;
                
                LightStonePropertyID.Text = String.IsNullOrEmpty(lightStonePropertyID) ? "-" : lightStonePropertyID;
                AdCheckPropertyID.Text = String.IsNullOrEmpty(adCheckPropertyID) ? "-" : adCheckPropertyID;
                lbCurrentDataProvider.Text = String.IsNullOrEmpty(currentDataProvider) ? "-" : currentDataProvider;

                if (property.PropertyAccessDetails != null)
                {
                    InspectionContact.Text = String.IsNullOrEmpty(property.PropertyAccessDetails.Contact1) ? "-" : property.PropertyAccessDetails.Contact1;
                    InspectionContact2.Text = String.IsNullOrEmpty(property.PropertyAccessDetails.Contact2) ? "-" : property.PropertyAccessDetails.Contact2;

                    InspectionNumber.Text = String.IsNullOrEmpty(property.PropertyAccessDetails.Contact1Phone) ? "-" : property.PropertyAccessDetails.Contact1Phone;
                    InspectionNumber2.Text = String.IsNullOrEmpty(property.PropertyAccessDetails.Contact2Phone) ? "-" : property.PropertyAccessDetails.Contact2Phone;
                }
            }


        }


        public void BindPropertyOwnersGrid(DataTable dtOwnerDetails)
        {
            // setup the columns
            PropertyOwnersGrid.Columns.Clear();

            if (dtOwnerDetails == null)
                return;

            int columnIndex = 0;

            PropertyOwnersGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.PropertyID, "Key", Unit.Percentage(0), HorizontalAlign.Left, false);

            if (dtOwnerDetails.Columns[SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate] != null)
            {
                columnIndex++;
                PropertyOwnersGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate, "Reg. Date", Unit.Percentage(10), HorizontalAlign.Left, true);
                _colRegistrationDate_Transfers = columnIndex;
            }

            if (dtOwnerDetails.Columns[SAHL.Common.Constants.LightStone.TransfersTable.PurchasePrice] != null)
            {
                columnIndex++;
                PropertyOwnersGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.PurchasePrice, SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridCurrency, "Purch. Amt", false, Unit.Percentage(10), HorizontalAlign.Left, true);
            }

            if (dtOwnerDetails.Columns[SAHL.Common.Constants.LightStone.TransfersTable.PurchaseDate] != null)
            {
                columnIndex++;
                PropertyOwnersGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.PurchaseDate, "Purch. Date", Unit.Percentage(10), HorizontalAlign.Left, true);
                _colPurchaseDate_Transfers = columnIndex;
            }

            if (dtOwnerDetails.Columns[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName] != null)
            {
                columnIndex++;
                PropertyOwnersGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.BuyersName, "Owner Name", Unit.Percentage(30), HorizontalAlign.Left, true);
            }
            if (dtOwnerDetails.Columns[SAHL.Common.Constants.LightStone.TransfersTable.SellersName] != null)
            {
                columnIndex++;
                PropertyOwnersGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.SellersName, "Seller Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            }

            PropertyOwnersGrid.DataSource = dtOwnerDetails;
            PropertyOwnersGrid.DataBind();
        }

        public void BindBondRegistrationsGrid(DataTable dtBondRegistrations)
        {
            // setup the columns
            BondRegistrationsGrid.Columns.Clear();

            if (dtBondRegistrations == null)
                return;

            int columnIndex = 0;

            BondRegistrationsGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.PropertyID, "Key", Unit.Percentage(0), HorizontalAlign.Left, false);

            if (dtBondRegistrations.Columns[SAHL.Common.Constants.LightStone.TransfersTable.BondNumber] != null)
            {
                columnIndex++;
                BondRegistrationsGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.BondNumber, "Bond Number", Unit.Percentage(20), HorizontalAlign.Left, true);
            }

            if (dtBondRegistrations.Columns[SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate] != null)
            {
                columnIndex++;
                BondRegistrationsGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate, "Reg. Date", Unit.Percentage(10), HorizontalAlign.Left, true);
                _colRegistrationDate_Registrations = columnIndex;
            }

            if (dtBondRegistrations.Columns[SAHL.Common.Constants.LightStone.TransfersTable.BondBank] != null)
            {
                columnIndex++;
                BondRegistrationsGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.BondBank, "Institution Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            }

            if (dtBondRegistrations.Columns[SAHL.Common.Constants.LightStone.TransfersTable.BondAmount] != null)
            {
                columnIndex++;
                BondRegistrationsGrid.AddGridBoundColumn(SAHL.Common.Constants.LightStone.TransfersTable.BondAmount, SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridCurrency, "Bond Amount", false, Unit.Percentage(15), HorizontalAlign.Left, true);
            }

            BondRegistrationsGrid.DataSource = dtBondRegistrations;
            BondRegistrationsGrid.DataBind();
        }


        #endregion

        protected void PropertyOwnersGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // purchase date
                if (_colPurchaseDate_Transfers > -1)
                    cells[_colPurchaseDate_Transfers].Text = FormatDateForGrid(cells[_colPurchaseDate_Transfers].Text);

                // registration date
                if (_colRegistrationDate_Transfers > -1)
                    cells[_colRegistrationDate_Transfers].Text = FormatDateForGrid(cells[_colRegistrationDate_Transfers].Text);
            }
        }

        protected void BondRegistrationsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // registration date
                if (_colRegistrationDate_Registrations > -1)
                    cells[_colRegistrationDate_Registrations].Text = FormatDateForGrid(cells[_colRegistrationDate_Registrations].Text);
            }
        }
        /// <summary>
        /// we expect the input date string to be 8 chars yyyyMMdd
        /// </summary>
        /// <param name="stringDate"></param>
        /// <returns></returns>
        private static string FormatDateForGrid(string stringDate)
        {
            // if the date string is blank then return space
            if (stringDate == "&nbsp;" || String.IsNullOrEmpty(stringDate))
                return "";

            // if the date string is not 8 chars then return whatever the string value is
            if (stringDate.Length != 8)
                return stringDate;

            // if we have what looks like a valid date then lets format it
            string formattedDate = "";
            string tempDate = Convert.ToInt32(stringDate.Substring(0, 4)) // year
                + "-"
                + Convert.ToInt32(stringDate.Substring(4, 2)) // month
                + "-"
                + Convert.ToInt32(stringDate.Substring(6, 2)); // day

            DateTime dateValue;
            if (DateTime.TryParse(tempDate, out dateValue))
                formattedDate = dateValue.ToString(SAHL.Common.Constants.DateFormat);
            else
                formattedDate = stringDate;

            return formattedDate;
        }

        public void BindDropDownLists()
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            
            ddlOccupancyType.DataSource = lookupRepo.OccupancyTypes.BindableDictionary;
            ddlOccupancyType.DataBind();

            ddlAreaClassification.DataSource = lookupRepo.AreaClassifications.BindableDictionary;
            ddlAreaClassification.DataBind();

            ddlDeedsPropertyType.DataSource = lookupRepo.DeedsPropertyTypes.BindableDictionary;
            ddlDeedsPropertyType.DataBind();

            ddlPropertyType.DataSource = lookupRepo.PropertyTypes.BindableDictionary;
            ddlPropertyType.DataBind();

            ddlTitleType.DataSource = lookupRepo.TitleTypes.BindableDictionary;
            ddlTitleType.DataBind();

            ddlDeedsOffice.DataSource = lookupRepo.DeedsOffice.BindableDictionary;
            ddlDeedsOffice.DataBind();

        }

        private static string GetDeedNumbers(IProperty property, bool withHTMLBreak)
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;
            foreach (IPropertyTitleDeed titleDeed in property.PropertyTitleDeeds)
            {
                index++;
                if (withHTMLBreak==true)
                    sb.Append(titleDeed.TitleDeedNumber + "<br/>");
                else
                {
                    if (index==property.PropertyTitleDeeds.Count)
                        sb.Append(titleDeed.TitleDeedNumber);
                    else
                        sb.AppendLine(titleDeed.TitleDeedNumber);

                }
            }

            return sb.ToString();
        }

        protected void PropertyAddressGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            IAddress address = PropertyAddressGrid.SelectedAddress as IAddress;
            _selectedAddressKey = address.Key;
            OnPropertyAddressGridSelectedIndexChanged(sender, new KeyChangedEventArgs(_selectedAddressKey.ToString()));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void btnAddressClean_Click(object sender, EventArgs e)
        {
            OnAddressCleanButtonClicked(sender, new KeyChangedEventArgs(_selectedAddressKey.ToString()));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            switch (_propertyDetailsUpdateMode)
            {
                case PropertyDetailsUpdateMode.Contact:
                    _updatedContact = Request.Form[txtInspectionContact.UniqueID];
                    _updatedContactNumber = Request.Form[txtInspectionNumber.UniqueID];
                    _updatedContact2 = Request.Form[txtInspectionContact2.UniqueID];
                    _updatedContactNumber2 = Request.Form[txtInspectionNumber2.UniqueID];
                    break;
                case PropertyDetailsUpdateMode.Property:
                case PropertyDetailsUpdateMode.Deeds:
                    _valuesChanged = false;
                    if ((Request.Form[ddlPropertyType.UniqueID] != null) && (Request.Form[ddlPropertyType.UniqueID] != SAHL.Common.Constants.DefaultDropDownItem))
                        _updatedPropertyTypeKey = int.Parse(Request.Form[ddlPropertyType.UniqueID]);
                    if ((Request.Form[ddlTitleType.UniqueID] != null) && (Request.Form[ddlTitleType.UniqueID] != SAHL.Common.Constants.DefaultDropDownItem))
                        _updatedTitleTypeKey = int.Parse(Request.Form[ddlTitleType.UniqueID]);
                    if ((Request.Form[ddlAreaClassification.UniqueID] != null) && (Request.Form[ddlAreaClassification.UniqueID] != SAHL.Common.Constants.DefaultDropDownItem))
                        _updatedAreaClassificationKey = int.Parse(Request.Form[ddlAreaClassification.UniqueID]);
                    if ((Request.Form[ddlDeedsPropertyType.UniqueID] != null) && (Request.Form[ddlDeedsPropertyType.UniqueID] != SAHL.Common.Constants.DefaultDropDownItem))
                        _updatedDeedsPropertyTypeKey = int.Parse(Request.Form[ddlDeedsPropertyType.UniqueID]);
                    if ((Request.Form[ddlOccupancyType.UniqueID] != null) && (Request.Form[ddlOccupancyType.UniqueID] != SAHL.Common.Constants.DefaultDropDownItem))
                        _updatedOccupancyTypeKey = int.Parse(Request.Form[ddlOccupancyType.UniqueID]);

                    _updatedPropertyDesc1 = Request.Form[txtPropertyDescription1.UniqueID].Trim();
                    _updatedPropertyDesc2 = Request.Form[txtPropertyDescription2.UniqueID].Trim();
                    _updatedPropertyDesc3 = Request.Form[txtPropertyDescription3.UniqueID].Trim();

                    if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds)
                    {
                        _updatedErfNumber = Request.Form[txtErfNumber.UniqueID];
                        _updatedErfPortionNumber = Request.Form[txtPortionNumber.UniqueID];
                        _updatedErfSuburbDescription = Request.Form[txtErfSuburb.UniqueID];
                        _updatedErfMetroDescription = Request.Form[txtErfMetroDescription.UniqueID];
                        _updatedSectionalSchemeName = Request.Form[txtSectionalSchemeName.UniqueID];
                        _updatedSectionalUnitNumber = Request.Form[txtSectionalUnitNumber.UniqueID];
                        _updatedTitleDeedNumbers = Request.Form[txtTitleDeedNumber.UniqueID];
                        _updatedBondAccountNumber = Request.Form[txtBondAccountNumber.UniqueID];
                        if ((Request.Form[ddlDeedsOffice.UniqueID] != null) && (Request.Form[ddlDeedsOffice.UniqueID] != SAHL.Common.Constants.DefaultDropDownItem))
                            _updatedDeedsOfficeKey = int.Parse(Request.Form[ddlDeedsOffice.UniqueID]);
                    }
                    else
                        _updatedTitleDeedNumbers = "";

                    if (((_selectedProperty.PropertyType == null ? 0 : _selectedProperty.PropertyType.Key) != _updatedPropertyTypeKey)
                        || ((_selectedProperty.TitleType == null ? 0 : _selectedProperty.TitleType.Key) != _updatedTitleTypeKey)
                        || ((_selectedProperty.AreaClassification == null ? 0 : _selectedProperty.AreaClassification.Key) != _updatedAreaClassificationKey)
                        || ((_selectedProperty.DeedsPropertyType == null ? 0 : _selectedProperty.DeedsPropertyType.Key) != _updatedDeedsPropertyTypeKey)
                        || ((_selectedProperty.OccupancyType == null ? 0 : _selectedProperty.OccupancyType.Key) != _updatedOccupancyTypeKey)
                        || _selectedProperty.PropertyDescription1 != _updatedPropertyDesc1
                        || _selectedProperty.PropertyDescription2 != _updatedPropertyDesc2
                        || _selectedProperty.PropertyDescription3 != _updatedPropertyDesc3
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _origionalBondAccountNumber != _updatedBondAccountNumber)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _origionalDeedsOfficeKey != _updatedDeedsOfficeKey)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _selectedProperty.ErfNumber != _updatedErfNumber)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _selectedProperty.ErfPortionNumber != _updatedErfPortionNumber)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _selectedProperty.ErfSuburbDescription != _updatedErfSuburbDescription)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _selectedProperty.ErfMetroDescription != _updatedErfMetroDescription)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _selectedProperty.SectionalSchemeName != _updatedSectionalSchemeName)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _selectedProperty.SectionalUnitNumber != _updatedSectionalUnitNumber)
                        || (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _origionalTitleDeedNumbers != _updatedTitleDeedNumbers)
                        )
                    {
                        _valuesChanged = true;
                    }

                    _titleDeedNumbersChanged = false;
                    if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds && _origionalTitleDeedNumbers != _updatedTitleDeedNumbers)
                        _titleDeedNumbersChanged = true;

                    if (_valuesChanged)
                    {
                        if (_lookupRepo == null)
                            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                        // populate property object
                        _selectedProperty.PropertyType = _updatedPropertyTypeKey > 0 ? _lookupRepo.PropertyTypes.ObjectDictionary[Convert.ToString(_updatedPropertyTypeKey)] : null;
                        _selectedProperty.TitleType = _updatedTitleTypeKey > 0 ? _lookupRepo.TitleTypes.ObjectDictionary[Convert.ToString(_updatedTitleTypeKey)] : null;
                        _selectedProperty.OccupancyType =_updatedOccupancyTypeKey > 0 ? _lookupRepo.OccupancyTypes.ObjectDictionary[Convert.ToString(_updatedOccupancyTypeKey)] : null;
                        _selectedProperty.AreaClassification = _updatedAreaClassificationKey > 0 ? _lookupRepo.AreaClassifications.ObjectDictionary[Convert.ToString(_updatedAreaClassificationKey)] : null;
                        _selectedProperty.DeedsPropertyType = _updatedDeedsPropertyTypeKey > 0 ? _lookupRepo.DeedsPropertyTypes.ObjectDictionary[Convert.ToString(_updatedDeedsPropertyTypeKey)] : null;
                        _selectedProperty.PropertyDescription1 = _updatedPropertyDesc1;
                        _selectedProperty.PropertyDescription2 = _updatedPropertyDesc2;
                        _selectedProperty.PropertyDescription3 = _updatedPropertyDesc3;
                        _selectedProperty.DataProvider = _lookupRepo.DataProviders.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.DataProviders.SAHL)];

                        if (_propertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds)
                        {
                            _selectedProperty.ErfNumber = _updatedErfNumber;
                            _selectedProperty.ErfPortionNumber = _updatedErfPortionNumber;
                            _selectedProperty.ErfSuburbDescription = _updatedErfSuburbDescription;
                            _selectedProperty.ErfMetroDescription = _updatedErfMetroDescription;
                            _selectedProperty.SectionalSchemeName = _updatedSectionalSchemeName;
                            _selectedProperty.SectionalUnitNumber = _updatedSectionalUnitNumber;
                        }
                    }
                    break;
                default:
                    break;
            }

            OnUpdateButtonClicked(sender, e);
        }
    }
}

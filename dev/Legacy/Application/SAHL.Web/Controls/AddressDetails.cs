using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using AjaxControlToolkit;
using SAHL.Web.AJAX;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;
using System.Collections.ObjectModel;


namespace SAHL.Web.Controls
{
    /// <summary>
    /// Control for adding/updating address information.
    /// <para>
    /// Some notes on usage:
    /// <list type="bullet">
    ///     <item>
    ///         <description>The default input type is Street.  Set the AddressFormat if you wish to enter any other address type.</description>
    ///     </item>
    ///     <item>
    ///         <description>The AJAX search area can be displayed to the right of the input form or below it by using the Orientation property.</description>
    ///     </item>
    ///     <item>
    ///         <description>There is no server-side support for the AJAX search area.  You can allow for client-side support by setting the ClientAddressSelectFunction - you will need to make use of the key this supplies.</description>
    ///     </item>
    /// </list>
    /// </para>
    /// </summary>
    public class AddressDetails : Panel, INamingContainer
    {

        private HtmlTable _table;
        private HtmlTableRow _row;
        private HtmlTableCell _cell;
        private HtmlGenericControl _divControls;
        private Orientation _orientation = Orientation.Horizontal;
        private Panel _pnlPlaceHolder;
        private HiddenField _hidAddressFormat;
        private Unit _widthText = Unit.Pixel(150);
        private Unit _widthInput = Unit.Pixel(230);
        private Unit _widthForm = Unit.Pixel(395);
        private Unit _heightSearch = Unit.Pixel(200);
        private IAddressRepository _addressRepository;
        private ILookupRepository _lookupRepository;
        private string _clientAddressSelectFunction;
        private IAddress _currentAddress;
        private AddressFormats _addressFormat = AddressFormats.Street;


        private HtmlGenericControl _rowUnitNumber;
        private HtmlGenericControl _rowBuilding;
        private HtmlGenericControl _rowStreet;
        private HtmlGenericControl _rowCountry;
        private HtmlGenericControl _rowProvince;
        private HtmlGenericControl _rowSuburb;
        private HtmlGenericControl _rowCity;
        private HtmlGenericControl _rowPostalCode;
        private HtmlGenericControl _rowBoxNumber;
        private HtmlGenericControl _rowPostOffice;
        private HtmlGenericControl _rowPostnetSuiteNumber;
        private HtmlGenericControl _rowPrivateBagNumber;
        private HtmlGenericControl _rowFreeLine1;
        private HtmlGenericControl _rowFreeLine2;
        private HtmlGenericControl _rowFreeLine3;
        private HtmlGenericControl _rowFreeLine4;
        private HtmlGenericControl _rowFreeLine5;
        private HtmlGenericControl _rowClusterBoxNumber;

        private SAHLTextBox _txtUnitNumber;
        private SAHLTextBox _txtBuildingNumber;
        private SAHLTextBox _txtBuildingName;
        private SAHLTextBox _txtStreetNumber;
        private SAHLTextBox _txtStreetName;
        private SAHLDropDownList _ddlCountry;
        private SAHLDropDownList _ddlProvince;
        private CascadingDropDown _cddProvince;
        private SAHLTextBox _txtSuburb;
        private SAHLAutoComplete _acSuburb;
        private SAHLTextBox _txtCity;
        private SAHLTextBox _txtPostalCode;
        private SAHLTextBox _txtBoxNumber;
        private SAHLTextBox _txtPostOffice;
        private SAHLAutoComplete _acPostOffice;
        private SAHLTextBox _txtPostnetSuiteNumber;
        private SAHLTextBox _txtPrivateBagNumber;
        private SAHLTextBox _txtFreeLine1;
        private SAHLTextBox _txtFreeLine2;
        private SAHLTextBox _txtFreeLine3;
        private SAHLTextBox _txtFreeLine4;
        private SAHLTextBox _txtFreeLine5;
        private SAHLTextBox _txtClusterBoxNumber;

        private HtmlGenericControl _divAjaxAddressMatch;

		private bool _searchAddressOnPrerender;

        // Any controls that can be bound to have private values to store the values being set, as we cannot guarantee 
        // that they aren't bound AFTER a value is set - and therefore we lose the value as it doesn't exist in the 
        // bound items
        private string _countryKey;
 
		#region Properties

		public bool SearchAddressOnPrerender
		{
			get
			{
				return _searchAddressOnPrerender;
			}
			set
			{
				_searchAddressOnPrerender = value;
			}
		}
             
		public string UnitNumber
		{
			get { return _txtUnitNumber.Text; }
			set { _txtUnitNumber.Text = value; }
		}

		public string StreetNumber
		{
			get { return _txtStreetNumber.Text; }
			set { _txtStreetNumber.Text = value; }
		}

		public string StreetName
		{
			get { return _txtStreetName.Text; }
			set { _txtStreetName.Text = value; }
		}

		public string BuildingNumber
		{
			get { return _txtBuildingNumber.Text; }
			set { _txtBuildingNumber.Text = value; }
		}

		public string BuildingName
		{
			get { return _txtBuildingName.Text; }
			set { _txtBuildingName.Text = value; }
		}

		public string Province
		{
			get
			{
				if (this._ddlProvince.SelectedIndex < 0)
					return null;

				return this._ddlProvince.SelectedValue;
			}
		}

		public string City
		{
			get { return _txtCity.Text; }
			set { _txtCity.Text = value; }
		}

		public string Suburb
		{
			get { return _txtSuburb.Text; }
			set { _txtSuburb.Text = value; }
		}

		public string PostalCode
		{
			get { return _txtPostalCode.Text; }
			set { _txtPostalCode.Text = value; }
		}

		public string BoxNumber
		{
			get { return _txtBoxNumber.Text; }
			set { _txtBoxNumber.Text = value; }
		}

		public string PostOffice
		{
			get { return _txtPostOffice.Text; }
			set { _txtPostOffice.Text = value; }
		}

		public string PostnetSuiteNumber
		{
			get { return _txtPostnetSuiteNumber.Text; }
			set { _txtPostnetSuiteNumber.Text = value; }
		}

		public string PrivateBagNumber
		{
			get { return _txtPrivateBagNumber.Text; }
			set { _txtPrivateBagNumber.Text = value; }
		}

		public string FreeLine1
		{
			get { return _txtFreeLine1.Text; }
			set { _txtFreeLine1.Text = value; }
		}

		public string FreeLine2
		{
			get { return _txtFreeLine2.Text; }
			set { _txtFreeLine2.Text = value; }
		}


		public string FreeLine3
		{
			get { return _txtFreeLine3.Text; }
			set { _txtFreeLine3.Text = value; }
		}

		public string FreeLine4
		{
			get { return _txtFreeLine4.Text; }
			set { _txtFreeLine4.Text = value; }
		}

		public string FreeLine5
		{
			get { return _txtFreeLine5.Text; }
			set { _txtFreeLine5.Text = value; }
		}

		public string ClusterBoxNumber
		{
			get { return _txtClusterBoxNumber.Text; }
			set { _txtClusterBoxNumber.Text = value; }
		}


            /// <summary>
            /// Gets/sets the address format applied to the control.  This defaults to <see cref="AddressFormats.Street"/>.
            /// </summary>
            public AddressFormats AddressFormat
		{
			get
			{
                return _addressFormat;
			}
			set
			{
                _addressFormat = value;
			}
		}

		/// <summary>
		/// Gets an IAddressRepository instance.
		/// </summary>
		private IAddressRepository AddressRepository
		{
			get
			{
				if (_addressRepository == null)
					_addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
				return _addressRepository;
			}
		}

		/// <summary>
		/// Gets/sets the name of a JavaScript function called when an item in the AJAX list is clicked.
		/// Your method must be in the format <code>addressSelectedFunction(key, description)</code>, and you 
		/// would set this property to "addressSelectedFunction" (i.e. do not supply the parameters).
		/// </summary>
		public string ClientAddressSelectFunction
		{
			get
			{
				return _clientAddressSelectFunction;
			}
			set
			{
				_clientAddressSelectFunction = value;
			}
		}

		/// <summary>
		/// Determines whether we are in design mode (standard DesignMode not reliable).
		/// </summary>
		protected static new bool DesignMode
		{
			get
			{
				return (HttpContext.Current == null);
			}
		}

		/// <summary>
		/// Gets/sets the height of the ajax search results area.  This defaults to 200px.
		/// </summary>
		public Unit HeightSearch
		{
			get
			{
				return _heightSearch;
			}
			set
			{
				_heightSearch = value;
			}
		}

		/// <summary>
		/// Gets a reference to a lookup repository.
		/// </summary>
		private ILookupRepository LookupRepository
		{
			get
			{
				if (_lookupRepository == null)
					_lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
				return _lookupRepository;
			}
		}

		/// <summary>
		/// Gets/sets the orientation of the control components, where Vertical will result in the AJAX search area being displayed below the 
		/// </summary>
		public Orientation Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				_orientation = value;
			}
		}

		/// <summary>
		/// Gets/sets the width of the form area (text and input controls combined).  This defaults to 375 pixels.
		/// </summary>
		public Unit WidthForm
		{
			get
			{
				return _widthForm;
			}
			set
			{
				_widthForm = value;
			}
		}

		/// <summary>
		/// Gets/sets the width of the area around the input fields.  This defaults to 220 pixels.
		/// </summary>
		public Unit WidthInput
		{
			get
			{
				return _widthInput;
			}
			set
			{
				_widthInput = value;
			}
		}

		/// <summary>
		/// Gets/sets the width of the text area.  This defaults to 150 pixels.
		/// </summary>
		public Unit WidthText
		{
			get
			{
				return _widthText;
			}
			set
			{
				_widthText = value;
			}
		}



		#endregion


        #region Life Cycle Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _pnlPlaceHolder = new Panel();
            _pnlPlaceHolder.ID = "PlaceHolder";
            Controls.Add(_pnlPlaceHolder);

            // create the main table, row, and cell (the row/cell(s) containing the search div is dynamically created in OnPreRender 
            // depending on the Orientation property
            _table = new HtmlTable();
            _table.Width = "100%";
            _table.CellPadding = 0;
            _table.CellSpacing = 0;
            _table.Style.Add("float", "left");
            this.Controls.Add(_table);
            _row = new HtmlTableRow();
            _table.Rows.Add(_row);
            _cell = new HtmlTableCell();
            _cell.VAlign = "top";
            _row.Cells.Add(_cell);

            // this control surrounds all the input controls, and is necessary otherwise IE6 pushes all the controls to the bottom of the table cell
            _divControls = new HtmlGenericControl("div");
            _divControls.Style.Add(HtmlTextWriterStyle.Height, "100%");
            _cell.Controls.Add(_divControls);

            #region Input fields                      

            // unit number
            _rowUnitNumber = new HtmlGenericControl("div");
            _txtUnitNumber = new SAHLTextBox();
            _txtUnitNumber.Columns = 4;
            _txtUnitNumber.ID = "txtUnitNumber";
            _rowUnitNumber.Controls.Add(_txtUnitNumber);
            AddInputRow(_rowUnitNumber, "Unit Number", _txtUnitNumber);

            // building number and name
            _rowBuilding = new HtmlGenericControl("div");
            _txtBuildingNumber = new SAHLTextBox();
            _txtBuildingNumber.Columns = 4;
            _txtBuildingNumber.ID = "txtBuildingNumber";
            _rowBuilding.Controls.Add(_txtBuildingNumber);
            _txtBuildingName = new SAHLTextBox();
            _txtBuildingName.Columns = 25;
            _txtBuildingName.ID = "txtBuildingName";
            _txtBuildingName.Style.Add(HtmlTextWriterStyle.MarginLeft, "5px");
            _rowBuilding.Controls.Add(_txtBuildingName);
            AddInputRow(_rowBuilding, "Building No & Name", _txtBuildingNumber, _txtBuildingName);

            // street number and name
            _rowStreet = new HtmlGenericControl("div");
            _txtStreetNumber = new SAHLTextBox();
            _txtStreetNumber.Columns = 4;
            _txtStreetNumber.ID = "txtStreetNumber";
            _rowStreet.Controls.Add(_txtStreetNumber);
            _txtStreetName = new SAHLTextBox();
            _txtStreetName.Columns = 25;
            _txtStreetName.ID = "txtStreetName";
            _txtStreetName.Style.Add(HtmlTextWriterStyle.MarginLeft, "5px");
            _rowStreet.Controls.Add(_txtStreetName);
            AddInputRow(_rowStreet, "Street No & Name", _txtStreetNumber, _txtStreetName);

            // country
            _rowCountry = new HtmlGenericControl("div");
            _ddlCountry = new SAHLDropDownList();
            _ddlCountry.ID = "ddlCountry";
            _ddlCountry.Width = Unit.Pixel(190);
            _ddlCountry.Mandatory = true;
            _ddlCountry.Attributes.Add("onchange", "AddressDetails_countryChanged()");
            _rowCountry.Controls.Add(_ddlCountry);
            AddInputRow(_rowCountry, "Country", _ddlCountry);
            _countryKey = GetDropDownValue(_ddlCountry);
            if (_countryKey == SAHLDropDownList.PleaseSelectValue)
                _countryKey = null;

            // province
            _rowProvince = new HtmlGenericControl("div");
            _ddlProvince = new SAHLDropDownList();
            _ddlProvince.ID = "ddlProvince";
            _ddlProvince.Width = Unit.Pixel(190);
            _ddlProvince.Attributes.Add("onchange", "AddressDetails_provinceChanged(this)");
            _ddlProvince.Mandatory = true;
            _rowProvince.Controls.Add(_ddlProvince);
            _cddProvince = new CascadingDropDown();
            _cddProvince.TargetControlID = _ddlProvince.UniqueID;
            _cddProvince.Category = "Province";
            _cddProvince.PromptText = SAHLDropDownList.PleaseSelectText;
            _cddProvince.LoadingText = "[Loading...]";
            _cddProvince.ServicePath = ServiceConstants.Address;
            _cddProvince.ServiceMethod = "GetProvincesByCountry";
            _cddProvince.ParentControlID = _ddlCountry.UniqueID;
            _rowProvince.Controls.Add(_cddProvince);
            AddInputRow(_rowProvince, "Province", _ddlProvince, _cddProvince);

            // suburb
            _rowSuburb = new HtmlGenericControl("div");
            _txtSuburb = new SAHLTextBox();
            _txtSuburb.ID = "txtSuburb";
            _txtSuburb.Columns = 3;
            _txtSuburb.Width = Unit.Pixel(190);
            _txtSuburb.Mandatory = true;
            _rowSuburb.Controls.Add(_txtSuburb);
            _acSuburb = new SAHLAutoComplete();
            _acSuburb.ID = "acSuburb";
            _acSuburb.MaxRowCount = 7;
            _acSuburb.ClientClickFunction = "AddressDetails_suburbSelected";
            _acSuburb.TargetControlID = _txtSuburb.UniqueID;
            _acSuburb.ServiceMethod = "SAHL.Web.AJAX.Address.GetSuburbsByProvince";
            _acSuburb.ParentControls.Add(new SAHLAutoCompleteParentControl(_ddlProvince.ID));
            _rowSuburb.Controls.Add(_acSuburb);
            AddInputRow(_rowSuburb, "Suburb", _txtSuburb, _acSuburb);

            // city
            _rowCity = new HtmlGenericControl("div");
            _txtCity = new SAHLTextBox();
            _txtCity.ID = "txtCity";
            _txtCity.ReadOnly = true;
            _rowCity.Controls.Add(_txtCity);
            AddInputRow(_rowCity, "City", _txtCity);

            // postal code
            _rowPostalCode = new HtmlGenericControl("div");
            _txtPostalCode = new SAHLTextBox();
            _txtPostalCode.ID = "txtPostalCode";
            _txtPostalCode.ReadOnly = true;
            _rowPostalCode.Controls.Add(_txtPostalCode);
            AddInputRow(_rowPostalCode, "Postal Code", _txtPostalCode);
           
            // box number
            _rowBoxNumber = new HtmlGenericControl("div");
            _txtBoxNumber = new SAHLTextBox();
            _txtBoxNumber.ID = "txtBoxNumber";
            _txtBoxNumber.Mandatory = true;
            _txtBoxNumber.DisplayInputType = InputType.AlphaNumNoSpace;
            _rowBoxNumber.Controls.Add(_txtBoxNumber);
            AddInputRow(_rowBoxNumber, "Box Number", _txtBoxNumber);

            // suburb
            _rowPostOffice = new HtmlGenericControl("div");
            _txtPostOffice = new SAHLTextBox();
            _txtPostOffice.ID = "txtPostOffice";
            _txtPostOffice.Width = Unit.Pixel(190);
            _txtPostOffice.Mandatory = true;
            _rowPostOffice.Controls.Add(_txtPostOffice);
            _acPostOffice = new SAHLAutoComplete();
            _acPostOffice.ID = "acPostOffice";
            _acPostOffice.MaxRowCount = 7;
            _acPostOffice.ClientClickFunction = "AddressDetails_postOfficeSelected";
            _acPostOffice.TargetControlID = _txtPostOffice.UniqueID;
            _acPostOffice.ServiceMethod = "SAHL.Web.AJAX.Address.GetPostOffices";
            _rowPostOffice.Controls.Add(_acPostOffice);
            AddInputRow(_rowPostOffice, "Post Office", _txtPostOffice, _acPostOffice);

            // postnet suite number
            _rowPostnetSuiteNumber = new HtmlGenericControl("div");
            _txtPostnetSuiteNumber = new SAHLTextBox();
            _txtPostnetSuiteNumber.ID = "txtPostnetSuiteNumber";
            _txtPostnetSuiteNumber.DisplayInputType = InputType.AlphaNumNoSpace;
            _txtPostnetSuiteNumber.Mandatory = true;
            _rowPostnetSuiteNumber.Controls.Add(_txtPostnetSuiteNumber);
            AddInputRow(_rowPostnetSuiteNumber, "Postnet Suite Number", _txtPostnetSuiteNumber);

            // private bag
            _rowPrivateBagNumber = new HtmlGenericControl("div");
            _txtPrivateBagNumber = new SAHLTextBox();
            _txtPrivateBagNumber.ID = "txtPrivateBagNumber";
            _txtPrivateBagNumber.DisplayInputType = InputType.AlphaNumNoSpace;
            _txtPrivateBagNumber.Mandatory = true;
            _rowPrivateBagNumber.Controls.Add(_txtPrivateBagNumber);
            AddInputRow(_rowPrivateBagNumber, "Private Bag", _txtPrivateBagNumber);

            // free text fields
            _rowFreeLine1 = new HtmlGenericControl("div");
            _txtFreeLine1 = new SAHLTextBox();
            _txtFreeLine1.ID = "txtFreeLine1";
            _txtFreeLine1.MaxLength = 50;
            _txtFreeLine1.Mandatory = true;
            _rowFreeLine1.Controls.Add(_txtFreeLine1);
            AddInputRow(_rowFreeLine1, "Line 1", _txtFreeLine1);

            _rowFreeLine2 = new HtmlGenericControl("div");
            _txtFreeLine2 = new SAHLTextBox();
            _txtFreeLine2.ID = "txtFreeLine2";
            _txtFreeLine2.MaxLength = 50;
            _rowFreeLine2.Controls.Add(_txtFreeLine2);
            AddInputRow(_rowFreeLine2, "Line 2", _txtFreeLine2);

            _rowFreeLine3 = new HtmlGenericControl("div");
            _txtFreeLine3 = new SAHLTextBox();
            _txtFreeLine3.ID = "txtFreeLine3";
            _txtFreeLine3.MaxLength = 50;
            _rowFreeLine3.Controls.Add(_txtFreeLine3);
            AddInputRow(_rowFreeLine3, "Line 3", _txtFreeLine3);

            _rowFreeLine4 = new HtmlGenericControl("div");
            _txtFreeLine4 = new SAHLTextBox();
            _txtFreeLine4.ID = "txtFreeLine4";
            _txtFreeLine4.MaxLength = 50;
            _rowFreeLine4.Controls.Add(_txtFreeLine4);
            AddInputRow(_rowFreeLine4, "Line 4", _txtFreeLine4);

            _rowFreeLine5 = new HtmlGenericControl("div");
            _txtFreeLine5 = new SAHLTextBox();
            _txtFreeLine5.ID = "txtFreeLine5";
            _txtFreeLine5.MaxLength = 50;
            _rowFreeLine5.Controls.Add(_txtFreeLine5);
            AddInputRow(_rowFreeLine5, "Line 5", _txtFreeLine5);

            // cluster box
            _rowClusterBoxNumber = new HtmlGenericControl("div");
            _txtClusterBoxNumber = new SAHLTextBox();
            _txtClusterBoxNumber.ID = "txtClusterBoxNumber";
            _txtClusterBoxNumber.DisplayInputType = InputType.AlphaNumNoSpace;
            _txtClusterBoxNumber.Mandatory = true;
            _rowClusterBoxNumber.Controls.Add(_txtClusterBoxNumber);
            AddInputRow(_rowClusterBoxNumber, "Cluster Box Number", _txtClusterBoxNumber);

            #endregion


            // hidden input field that keeps track of the address format
            _hidAddressFormat = new HiddenField();
            _hidAddressFormat.ID = "hidAddressFormat";
            Controls.Add(_hidAddressFormat);
            
            if (!DesignMode) // this is for the vs2010 designtime bug
            {
                if (!String.IsNullOrEmpty(Page.Request.Form[_hidAddressFormat.UniqueID]))
                    _addressFormat = (AddressFormats)Int32.Parse(Page.Request.Form[_hidAddressFormat.UniqueID]);
            }

            // search div
            _divAjaxAddressMatch = new HtmlGenericControl("div");
            _divAjaxAddressMatch.ID = "divAjaxAddressMatch";
            _divAjaxAddressMatch.Attributes.Add("class", "borderAll backgroundDark");
            _divAjaxAddressMatch.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
            _divAjaxAddressMatch.Style.Add(HtmlTextWriterStyle.Overflow, "auto");
            _divAjaxAddressMatch.Style.Add(HtmlTextWriterStyle.MarginTop, "5px");
            _divAjaxAddressMatch.Style.Add(HtmlTextWriterStyle.Width, "99%");
            Controls.Add(_divAjaxAddressMatch);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SAHLCommonBaseView view = this.Page as SAHLCommonBaseView;
            view.RegisterWebService(ServiceConstants.Address);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			RegisterCommonScript();
			_cell.Width = WidthForm.ToString();
			_divAjaxAddressMatch.Style.Add(HtmlTextWriterStyle.Height, HeightSearch.ToString());

			// bind lookup data to the controls
			BindCountries();
            _ddlCountry.SelectedValue = _countryKey;

			// if the orientation is horizontal, create a new cell and add the search grid to that, if it is 
			// vertical then add a new row and add it there
			HtmlTableCell cellSearch = new HtmlTableCell();
			if (Orientation == Orientation.Horizontal)
			{
				_row.Cells.Add(cellSearch);
				_cell.Height = _heightSearch.ToString();
			}
			else
			{
				HtmlTableRow row = new HtmlTableRow();
				_table.Rows.Add(row);
				row.Cells.Add(cellSearch);
			}
			cellSearch.Controls.Add(_divAjaxAddressMatch);
			cellSearch.VAlign = "top";

           

			// depending on the address format, make the correct fields visible
			switch (AddressFormat)
			{
				case AddressFormats.Box:
					ShowInputRow(_rowBoxNumber);
					ShowInputRow(_rowPostOffice);
					break;
				case AddressFormats.ClusterBox:
					ShowInputRow(_rowClusterBoxNumber);
					ShowInputRow(_rowPostOffice);
					break;
				case AddressFormats.FreeText:
					ShowInputRow(_rowFreeLine1);
					ShowInputRow(_rowFreeLine2);
					ShowInputRow(_rowFreeLine3);
					ShowInputRow(_rowFreeLine4);
					ShowInputRow(_rowFreeLine5);
					ShowInputRow(_rowCountry);
					break;
				case AddressFormats.PostNetSuite:
					ShowInputRow(_rowPostnetSuiteNumber);
					ShowInputRow(_rowPrivateBagNumber);
					ShowInputRow(_rowPostOffice);
					break;
				case AddressFormats.PrivateBag:
					ShowInputRow(_rowPrivateBagNumber);
					ShowInputRow(_rowPostOffice);
					break;
				case AddressFormats.Street:
					ShowInputRow(_rowUnitNumber);
					ShowInputRow(_rowBuilding);
					ShowInputRow(_rowStreet);
					ShowInputRow(_rowCountry);
					ShowInputRow(_rowProvince);
					ShowInputRow(_rowSuburb);
					ShowInputRow(_rowCity);
					ShowInputRow(_rowPostalCode);

                    // update the values of the city and postal box text boxes
                    if (Page.IsPostBack && !String.IsNullOrEmpty(_acSuburb.SelectedValue))
                    {
                        ISuburb suburb = AddressRepository.GetSuburbByKey(Int32.Parse(_acSuburb.SelectedValue));
                        if (suburb.City != null)
                            _txtCity.Text = suburb.City.Description;
                        if (!String.IsNullOrEmpty(suburb.PostalCode))
                            _txtPostalCode.Text = suburb.PostalCode;
                    }
					break;
				default:
					throw new NotSupportedException();
			}

            _hidAddressFormat.Value = ((int)_addressFormat).ToString();
			_pnlPlaceHolder.Visible = false;

			// suburb input must be disabled if no province selected
            if (!Page.IsPostBack && _currentAddress != null)
            {
                _txtSuburb.Enabled = (!String.IsNullOrEmpty(_currentAddress.RRR_SuburbDescription));
            }
            else
            {
                string province = GetDropDownValue(_ddlProvince);
                _txtSuburb.Enabled = ((province != SAHLDropDownList.PleaseSelectValue && province.Length > 0) || _currentAddress != null);
            }
		}

        #endregion

        #region Methods

        /// <summary>
        /// Adds an input row to the control - this is wrapped up so we can surround the label and input control with some decent formatting.
        /// </summary>
        /// <param name="div"></param>
        /// <param name="text"></param>
        /// <param name="inputControls"></param>
        private void AddInputRow(HtmlGenericControl div, string text, params Control[] inputControls)
        {
            div.Attributes.Add("class", "row");

            HtmlGenericControl label = new HtmlGenericControl("div");
            label.InnerText = text;
            label.Attributes.Add("class", "cellInput TitleText");
            label.Style.Add(HtmlTextWriterStyle.Width, WidthText.ToString());
            div.Controls.Add(label);

            HtmlGenericControl inputDiv = new HtmlGenericControl("div");
            
            inputDiv.Attributes.Add("class", "cellInput");
            inputDiv.Style.Add(HtmlTextWriterStyle.Width, WidthInput.ToString());
            foreach (Control c in inputControls)
            {
                inputDiv.Controls.Add(c);
                
            }
            div.Controls.Add(inputDiv);
            _pnlPlaceHolder.Controls.Add(div);

        }

        /// <summary>
        /// Binds a list of countries to the country drop down box.  This will filter the list of countries 
        /// depending on the address format.
        /// </summary>
        private void BindCountries()
        {
            _ddlCountry.Items.Clear();

            // we need to adjust which countries are displayed depending on what address format is selected - do a 
            // manual add instead of a DataBind() otherwise you end up getting errors as items that are expected 
            // are no longer there between postbacks
            foreach (ICountryReadOnly country in LookupRepository.Countries.Values)
            {
                ListItem listItem = new ListItem(country.Description, country.Key.ToString());
                if (AddressFormat == AddressFormats.FreeText && country.AllowFreeTextFormat)
                    _ddlCountry.Items.Add(listItem);
                else if (AddressFormat != AddressFormats.FreeText && !country.AllowFreeTextFormat)
                    _ddlCountry.Items.Add(listItem);
            }

            if (_ddlCountry.Items.Count > 1)
                _ddlCountry.VerifyPleaseSelect();

            if (_ddlCountry.Items.FindByValue(_countryKey) != null)
                _ddlCountry.SelectedValue = _countryKey;
        }
       

        /// <summary>
        /// Binds a list of provinces to the provinces drop down box.
        /// </summary>
        private void BindProvinces()
        {
            if (AddressFormat == AddressFormats.Street && !String.IsNullOrEmpty(_countryKey))
            {

                IDictionary<int, string> provinces = LookupRepository.ProvincesByCountry(Int32.Parse(_countryKey));

                _ddlProvince.Items.Clear();

                _ddlProvince.PleaseSelectItem = true;
                _ddlProvince.DataValueField = "Key";
                _ddlProvince.DataTextField = "Description";
                _ddlProvince.DataSource = provinces;
                _ddlProvince.DataBind();
                _ddlProvince.SelectedValue = GetDropDownValue(_ddlProvince);
            }

        }

        /// <summary>
        /// Clears the values of all controls.
        /// </summary>
        public void ClearInputValues()
        {
            _txtUnitNumber.Text = String.Empty;
            _txtBuildingNumber.Text = String.Empty;
            _txtBuildingName.Text = String.Empty;
            _txtStreetNumber.Text = String.Empty;
            _txtStreetName.Text = String.Empty;
            _ddlCountry.ClearSelection();
            _countryKey = null;
            _cddProvince.SelectedValue = String.Empty;
            _txtSuburb.Text = String.Empty;
            _acSuburb.SelectedValue = String.Empty;
            _txtCity.Text = String.Empty;
            _txtPostalCode.Text = String.Empty;
            _txtBoxNumber.Text = String.Empty;
            _txtPostOffice.Text = String.Empty;
            _acPostOffice.SelectedValue = String.Empty;
            _txtClusterBoxNumber.Text = String.Empty;
            _txtFreeLine1.Text = String.Empty;
            _txtFreeLine2.Text = String.Empty;
            _txtFreeLine3.Text = String.Empty;
            _txtFreeLine4.Text = String.Empty;
            _txtFreeLine5.Text = String.Empty;
            _txtPostnetSuiteNumber.Text = String.Empty;
            _txtPrivateBagNumber.Text = String.Empty;
        }

        /// <summary>
        /// Gets the address captured in the input form, as a new IAddress object.
        /// </summary>
        /// <returns></returns>
        public IAddress GetCapturedAddress()
        {
            IAddress address;

            switch (AddressFormat)
            {
                case AddressFormats.Box:
                    IAddressBox addressBox = AddressRepository.GetEmptyAddress(typeof(IAddressBox)) as IAddressBox;
                    address = addressBox;
                    break;
                case AddressFormats.ClusterBox:
                    IAddressClusterBox addressCluster = AddressRepository.GetEmptyAddress(typeof(IAddressClusterBox)) as IAddressClusterBox;
                    address = addressCluster;
                    break;
                case AddressFormats.FreeText:
                    IAddressFreeText addressFreeText = AddressRepository.GetEmptyAddress(typeof(IAddressFreeText)) as IAddressFreeText;
                    address = addressFreeText;
                    break;
                case AddressFormats.PostNetSuite:
                    IAddressPostnetSuite addressPostnet = AddressRepository.GetEmptyAddress(typeof(IAddressPostnetSuite)) as IAddressPostnetSuite;
                    address = addressPostnet;
                    break;
                case AddressFormats.PrivateBag:
                    IAddressPrivateBag addressPrivateBag = AddressRepository.GetEmptyAddress(typeof(IAddressPrivateBag)) as IAddressPrivateBag;
                    address = addressPrivateBag;
                    break;
                case AddressFormats.Street:
                    IAddressStreet addressStreet = AddressRepository.GetEmptyAddress(typeof(IAddressStreet)) as IAddressStreet;
                    address = addressStreet;
                    break;
                default:
                    throw new NotSupportedException("Address format " + AddressFormat.ToString() + " is not supported.");
            }

            return GetCapturedAddress(address);
        }
       

        /// <summary>
        /// Populates <c>address</c> with the values captured in the input fields.
        /// </summary>
        /// <returns></returns>
        public IAddress GetCapturedAddress(IAddress address)
        {
            switch (AddressFormat)
            {
                case AddressFormats.Box:
                    IAddressBox addressBox = address as IAddressBox;
                    addressBox.BoxNumber = _txtBoxNumber.Text;
                    if (_acPostOffice.SelectedValue.Length > 0)
                        addressBox.PostOffice = AddressRepository.GetPostOfficeByKey(Int32.Parse(_acPostOffice.SelectedValue));
                    break;
                case AddressFormats.ClusterBox:
                    IAddressClusterBox addressCluster = address as IAddressClusterBox;
                    addressCluster.ClusterBoxNumber = _txtClusterBoxNumber.Text;
                    if (_acPostOffice.SelectedValue.Length > 0)
                        addressCluster.PostOffice = AddressRepository.GetPostOfficeByKey(Int32.Parse(_acPostOffice.SelectedValue));
                    break;
                case AddressFormats.FreeText:
                    IAddressFreeText addressFree = address as IAddressFreeText;
                    addressFree.FreeText1 = _txtFreeLine1.Text;
                    addressFree.FreeText2 = _txtFreeLine2.Text;
                    addressFree.FreeText3 = _txtFreeLine3.Text;
                    addressFree.FreeText4 = _txtFreeLine4.Text;
                    addressFree.FreeText5 = _txtFreeLine5.Text;
                    if (!String.IsNullOrEmpty(_countryKey))
                        addressFree.PostOffice = AddressRepository.GetForeignCountryPostOffice(Int32.Parse(_countryKey));
                    break;
                case AddressFormats.PostNetSuite:
                    IAddressPostnetSuite addressPostnet = address as IAddressPostnetSuite;
                    if (_acPostOffice.SelectedValue.Length > 0)
                        addressPostnet.PostOffice = AddressRepository.GetPostOfficeByKey(Int32.Parse(_acPostOffice.SelectedValue));
                    addressPostnet.PrivateBagNumber = _txtPrivateBagNumber.Text;
                    addressPostnet.SuiteNumber = _txtPostnetSuiteNumber.Text;
                    break;
                case AddressFormats.PrivateBag:
                    IAddressPrivateBag addressPrivateBag = address as IAddressPrivateBag;
                    if (_acPostOffice.SelectedValue.Length > 0)
                        addressPrivateBag.PostOffice = AddressRepository.GetPostOfficeByKey(Int32.Parse(_acPostOffice.SelectedValue));
                    addressPrivateBag.PrivateBagNumber = _txtPrivateBagNumber.Text;
                    break;
                case AddressFormats.Street:
                    IAddressStreet addressStreet = address as IAddressStreet;
                    addressStreet.BuildingName = _txtBuildingName.Text;
                    addressStreet.BuildingNumber = _txtBuildingNumber.Text;
                    addressStreet.StreetName = _txtStreetName.Text;
                    addressStreet.StreetNumber = _txtStreetNumber.Text;
                    if (_acSuburb.SelectedValue.Length > 0)
                        addressStreet.Suburb = AddressRepository.GetSuburbByKey(Int32.Parse(_acSuburb.SelectedValue));
                    addressStreet.UnitNumber = _txtUnitNumber.Text;
                    break;
                default:
                    throw new NotSupportedException("Address format " + AddressFormat.ToString() + " is not supported.");
            }
            
            return address;
        }

        /// <summary>
        /// Helper function to return the selected value of a dropdownlist.
        /// </summary>
        /// <param name="ddl"></param>
        /// <returns></returns>
        private string GetDropDownValue(DropDownList ddl)
        {
            if (!DesignMode) // this is for the vs2010 designtime bug
            {
                string formValue = Page.Request.Form[ddl.UniqueID];
                if (formValue != null && formValue.Length > 0)
                    return formValue;
                else
                    return ddl.SelectedValue;
            }
            else
                return ddl.SelectedValue;

        }

        /// <summary>
        /// Adds JavaScript and CSS required by the control.
        /// </summary>
        private void RegisterCommonScript()
        {
            string key = "AddressDetails";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(key))
            {
                string css = @" <style type=""text/css"">
                                .ajaxSearchItem { width:100%; padding-top: 2px; padding-bottom: 2px; float:left; }
                                </style>";
                Page.ClientScript.RegisterClientScriptBlock(typeof(AddressDetails), "AddressDetailsCss", css);


                string script = @"
                    var AddressDetails_timerAjaxAddress = null;
                    var AddressDetails_ajaxSearchItems = null;
                    
                    function AddressDetails_AjaxItem(element, propertyName, type)
                    {
                        this.elementID = element;
                        this.propertyName = propertyName;
                        this.type = type;
                    }
                    
                    AddressDetails_AjaxItem.prototype = 
                    {
                        getElement : function()
                        {
                            return document.getElementById(this.elementID);
                        },
                        
                        getValue : function()
                        {
                            if (this.type == 'TEXT')
                                return this.getElement().value;
                            else if (this.type == 'AUTOCOMPLETE')
                            {
                                var ac = SAHLAutoComplete_findAutoComplete(this.getElement());
                                return (ac == null ? '' : ac.hiddenInput.value);
                            }
                            else if (this.type == 'DDL_TEXT')
                                return this.getElement().options[this.getElement().selectedIndex].text;
                            else if (this.type == 'DDL')
                                return this.getElement().options[this.getElement().selectedIndex].value;
                        }
                    }

                    function AddressDetails_ajaxItemChanged()
                    {
                        if (AddressDetails_timerAjaxAddress != null) clearTimeout(AddressDetails_timerAjaxAddress);
                        AddressDetails_timerAjaxAddress = setTimeout('AddressDetails_searchAddresses()', 500);
                    }

                    function AddressDetails_countryChanged()
                    {
                        var ddlProvince = document.getElementById('" + _ddlProvince.ClientID + @"');
                        if (ddlProvince == null)
                            AddressDetails_searchAddresses();
                        else
                            AddressDetails_provinceChanged(ddlProvince);
                    }                                      

                    function AddressDetails_postOfficeSelected(objAutoComplete)
                    {
                        AddressDetails_searchAddresses();
                    }    

                    function AddressDetails_provinceChanged(ddlProvince)
                    {
                        var txtSuburb = document.getElementById('" + _txtSuburb.ClientID + @"');
                        var acSuburb = SAHLAutoComplete_findAutoComplete(txtSuburb);
                        var txtCity = document.getElementById('" + _txtCity.ClientID + @"');
                        var txtPostalCode = document.getElementById('" + _txtPostalCode.ClientID + @"');
                        
                        txtSuburb.disabled = (ddlProvince.selectedIndex == 0);
                        if (acSuburb != null) acSuburb.clearSelection();
                        txtCity.value = '';
                        txtPostalCode.value = '';
                        
                        AddressDetails_searchAddresses();
                        
                    }

                    function AddressDetails_suburbSelected(objAutoComplete)
                    {
                        SAHL.Web.AJAX.Address.GetSuburbDetails(objAutoComplete.key, AddressDetails_suburbSelectedCallback);
                    }
                    
                    function AddressDetails_suburbSelectedCallback(result) 
                    {
                        var txtSuburb = document.getElementById('" + _txtSuburb.ClientID + @"');
                        var txtCity = document.getElementById('" + _txtCity.ClientID + @"');
                        var txtPostalCode = document.getElementById('" + _txtPostalCode.ClientID + @"');
                        txtCity.value = result[0];
                        txtPostalCode.value = result[1];
                    }

                    function AddressDetails_updateSuburbAutoComplete(ddlCountryId, ddlProvinceId)
                    {
                        var acSuburb = $find('acSuburb');
                        var ddlCountry = document.getElementById(ddlCountryId);
                        var ddlProvince = document.getElementById(ddlProvinceId);
                        acSuburb.set_contextKey(ddlCountry.options[ddlCountry.selectedIndex].value + '|' + ddlProvince.options[ddlProvince.selectedIndex].value);
                    }

                    function AddressDetails_initAjaxSearchItems()
                    {
                        // add all possible items to the array
                        ajaxSearchItems = new Array(
                            new AddressDetails_AjaxItem('" + _txtBuildingName.ClientID + @"',          'BuildingName',         'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtBuildingNumber.ClientID + @"',        'BuildingNumber',       'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtBoxNumber.ClientID + @"',             'BoxNumber',            'TEXT'),
                            new AddressDetails_AjaxItem('" + _ddlCountry.ClientID + @"',               'Country',              'DDL_TEXT'),
                            new AddressDetails_AjaxItem('" + _ddlProvince.ClientID + @"',              'Province',             'DDL_TEXT'),
                            new AddressDetails_AjaxItem('" + _txtStreetName.ClientID + @"',            'StreetName',           'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtStreetNumber.ClientID + @"',          'StreetNumber',         'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtPostOffice.ClientID + @"',            'PostOfficeKey',        'AUTOCOMPLETE'),
                            new AddressDetails_AjaxItem('" + _txtSuburb.ClientID + @"',                'SuburbKey',            'AUTOCOMPLETE'),
                            new AddressDetails_AjaxItem('" + _txtUnitNumber.ClientID + @"',            'UnitNumber',           'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtPostnetSuiteNumber.ClientID + @"',    'PostnetSuiteNumber',   'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtPrivateBagNumber.ClientID + @"',      'PrivateBagNumber',     'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtFreeLine1.ClientID + @"',             'FreeTextLine1',        'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtFreeLine2.ClientID + @"',             'FreeTextLine2',        'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtFreeLine3.ClientID + @"',             'FreeTextLine3',        'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtFreeLine4.ClientID + @"',             'FreeTextLine4',        'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtFreeLine5.ClientID + @"',             'FreeTextLine5',        'TEXT'),
                            new AddressDetails_AjaxItem('" + _txtClusterBoxNumber.ClientID + @"',      'ClusterBoxNumber',     'TEXT')
                            
                        );
                        
                        // register items for events
                        for (var i=0; i<ajaxSearchItems.length; i++)
                        {
                            var item = ajaxSearchItems[i];
                            var element = item.getElement();
                            if (element != null)
                            {
                                if (item.type == 'TEXT')
                                    registerEvent(element, 'keyup', AddressDetails_ajaxItemChanged);
                                else if (item.type == 'AUTOCOMPLETE')
                                    registerEvent(element, 'blur', AddressDetails_ajaxItemChanged);
                            }
                        }
                    }

                    function AddressDetails_searchAddresses()
                    {
                        var searchValues = new Array();
                        for (var i=0; i<ajaxSearchItems.length; i++)
                        {
                            var ajaxItem = ajaxSearchItems[i];
                            if (ajaxItem.getElement() != null)
                            {
                                searchValues[searchValues.length] = ajaxItem.propertyName + '=' + ajaxItem.getValue();
                            }
                            
                        }
                        SAHL.Web.AJAX.Address.SearchAddresses(" + ((int)AddressFormat).ToString() + @", searchValues, AddressDetails_searchAddressesCallback);
                    }
                    
                    function AddressDetails_searchAddressesCallback(result)
                    {
                        var html = '<div style=""width:100%;float:left;padding:0px;"" class=""titleText borderBottom""><div style=""padding:3px;"">Search Results</div></div>';
                        var cssClass = 'backgroundDark';
                        var divItem = '<div class=""{CSS} ajaxSearchItem""><span style=""padding:2px 2px 2px 2px "">{TEXT}</span></div>';
                        
                        // build up the html to display
                        for (var i=0; i<result.length; i++)
                        {
                            var ajaxItem = result[i];
                            var text = ajaxItem.Text.cleanForJavaScript();
                            var text = '<a href=""#"" onclick=""AddressDetails_selectAddress(' + ajaxItem.Value + ', \'' + text+ '\')"" title=""Select address: ' + text + '"">' + ajaxItem.Text + '</a>';
                            cssClass = (cssClass == 'backgroundDark' ? 'backgroundLight' : 'backgroundDark');
                            html += divItem.replace('{CSS}', cssClass).replace('{TEXT}', text);
                        }
                        
                        if (result.length == 0)
                        {
                            html += divItem.replace('{CSS}', cssClass).replace('{TEXT}', 'No matching addresses.');
                        }
                        
                        var divSearch = document.getElementById('" + _divAjaxAddressMatch.ClientID + @"');
                        divSearch.style.visibility = 'visible';
                        divSearch.innerHTML = html;
                    }

                    function AddressDetails_selectAddress(key, description)
                    {
                        " + (String.IsNullOrEmpty(ClientAddressSelectFunction) ? "" : (ClientAddressSelectFunction + "(key, description);")) + @"
                    }

                    AddressDetails_initAjaxSearchItems();

                ";

				if (_searchAddressOnPrerender)
				{
					script +=  "AddressDetails_searchAddresses();";
				}

                Page.ClientScript.RegisterStartupScript(typeof(AddressDetails), key, script, true);
            }

        }

        /// <summary>
        /// Sets the address information displayed on the control.
        /// </summary>
        /// <param name="address"></param>
        public void SetAddress(IAddress address)
        {

            _currentAddress = address;

            // clear all fields as we are setting new values
            ClearInputValues();

            // street address
            IAddressStreet streetAddress = address as IAddressStreet;
            if (streetAddress != null)
            {
                UnitNumber = streetAddress.UnitNumber;
                BuildingNumber = streetAddress.BuildingNumber;
                BuildingName = streetAddress.BuildingName;
                StreetNumber = streetAddress.StreetNumber;
                StreetName = streetAddress.StreetName;

                if (streetAddress.Suburb != null)
                {
                    _countryKey = streetAddress.Suburb.City.Province.Country.Key.ToString();
                    _cddProvince.SelectedValue = streetAddress.Suburb.City.Province.Key.ToString();
                    Suburb = streetAddress.Suburb.Description;
                    _acSuburb.SelectedValue = streetAddress.Suburb.Key.ToString();
                    City = streetAddress.Suburb.City.Description;
                    PostalCode = streetAddress.Suburb.PostalCode;
                }
                return;
            }

            IAddressBox boxAddress = address as IAddressBox;
            if (boxAddress != null)
            {
                BoxNumber = boxAddress.BoxNumber;
                PostOffice = boxAddress.PostOffice.Description;
                _acPostOffice.SelectedValue = boxAddress.PostOffice.Key.ToString();
                return;
            }

            IAddressClusterBox clusterAddress = address as IAddressClusterBox;
            if (clusterAddress != null)
            {
                ClusterBoxNumber = clusterAddress.ClusterBoxNumber;
                PostOffice = clusterAddress.PostOffice.Description;
                _acPostOffice.SelectedValue = clusterAddress.PostOffice.Key.ToString();
                return;
            }

            IAddressFreeText freeTextAddress = address as IAddressFreeText;
            if (freeTextAddress != null)
            {
                FreeLine1 = freeTextAddress.FreeText1;
                FreeLine2 = freeTextAddress.FreeText2;
                FreeLine3 = freeTextAddress.FreeText3;
                FreeLine4 = freeTextAddress.FreeText4;
                FreeLine5 = freeTextAddress.FreeText5;
                if (freeTextAddress.PostOffice != null && freeTextAddress.PostOffice.City != null && freeTextAddress.PostOffice.City.Province != null 
                    && freeTextAddress.PostOffice.City.Province.Country != null)
                    _countryKey = freeTextAddress.PostOffice.City.Province.Country.Key.ToString();
                return;
            }

            IAddressPostnetSuite postnetAddress = address as IAddressPostnetSuite;
            if (postnetAddress != null)
            {
                PostnetSuiteNumber = postnetAddress.SuiteNumber;
                PrivateBagNumber = postnetAddress.PrivateBagNumber;
                PostOffice = postnetAddress.PostOffice.Description;
                _acPostOffice.SelectedValue = postnetAddress.PostOffice.Key.ToString();
                return;
            }

            IAddressPrivateBag privateBagAddress = address as IAddressPrivateBag;
            if (privateBagAddress != null)
            {
                PrivateBagNumber = privateBagAddress.PrivateBagNumber;
                PostOffice = privateBagAddress.PostOffice.Description;
                _acPostOffice.SelectedValue = privateBagAddress.PostOffice.Key.ToString();
                return;
            }

            // if we get here, there is something wrong with the address
            throw new NotImplementedException("The selected address type has not been catered for by the AddressDetails control.");
        }

        /// <summary>
        /// Sets ddlProvince to a matching string value
        /// </summary>
        /// <param name="Key"></param>
        public void SelectProvinceByKey(string Key)
        {
			_cddProvince.SelectedValue = Key;

			//for (int i = 0; i < this._ddlProvince.Items.Count; i++)
			//{
			//    if (String.Compare(this._ddlProvince.Items[i].Value, province.Trim()) == 0)
			//    {
			//        this._ddlProvince.SelectedIndex = i;
			//        break;
			//    }
			//}
        }

        /// <summary>
        /// Sets ddlCountry to a matching string value
        /// </summary>
        /// <param name="country"></param>
        public void SelectCountryFromText(string country)
        {
            for (int i = 0; i < this._ddlCountry.Items.Count; i++)
            {
                if (String.Compare(this._ddlCountry.Items[i].Value, country.Trim()) == 0)
                {
                    this._ddlCountry.SelectedIndex = i;
                    break;
                }
            }
        }


        /// <summary>
        /// Ensures that an input row is displayed.
        /// </summary>
        /// <param name="div"></param>
        private void ShowInputRow(HtmlGenericControl div)
        {
            _pnlPlaceHolder.Controls.Remove(div);
            _divControls.Controls.Add(div);
        }

        #endregion
    }
}

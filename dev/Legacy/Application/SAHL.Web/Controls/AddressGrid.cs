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
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Controls
{

    /// <summary>
    /// Grid used to display SAHL address information.
    /// </summary>
    public class AddressGrid : SAHLGridView
    {
        #region Private Attributes

        private object _address;
        private bool _showInactive;
        private int _selectedAddressKey = -1;
        private bool _showLegalEntityColumn;

        #endregion

        #region Private Attributes and Enumerations

        /// <summary>
        /// Defines all columns used in the <see cref="AddressGrid"/>.
        /// </summary>
        private enum GridColumns
        {
            Key = 0,            
            AddressGridSource,
            LegalEntityName,
            Type,
            Format,
            Address,
            EffectiveDate,
            Status
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AddressGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "Addresses";
            EmptyDataSetMessage = "No addresses found";
            NullDataSetMessage = EmptyDataSetMessage;
            EmptyDataText = EmptyDataSetMessage;
            PostBackType = GridPostBackType.SingleClick;
            RowStyle.CssClass = "TableRowA";
            GridWidth = Unit.Percentage(100);
            Width = Unit.Percentage(100);
            GridHeight = Unit.Pixel(150);

            if (!DesignMode)
            {
                // add the columns to the grid
                this.AddGridBoundColumn("Key", "Key", Unit.Empty, HorizontalAlign.Left, false);
                this.AddGridBoundColumn("AddressSource", "Address Source", Unit.Empty, HorizontalAlign.Left, false);               
                this.AddGridBoundColumn("LegalEntityName", "Legal Entity", Unit.Empty, HorizontalAlign.Left, true);

                this.AddGridBoundColumn("Type", "Type", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("Format", "Format", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("AddressDescription", "Address Description", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("EffectiveDate", "Effective Date", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("Status", "Status", Unit.Empty, HorizontalAlign.Left, true);
            }

            this.SelectedIndexChanged += new EventHandler(AddressGrid_SelectedIndexChanged);
            this.RowDataBound += new GridViewRowEventHandler(AddressGrid_RowDataBound);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Columns[(int)GridColumns.LegalEntityName].Visible = _showLegalEntityColumn;
        }

        protected void AddressGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!DesignMode && e.Row.RowType == DataControlRowType.DataRow && (AddressSources)Convert.ToInt32(e.Row.Cells[(int)GridColumns.AddressGridSource].Text) == AddressSources.FailedLegalEntityAddress)
                e.Row.Cells[(int)GridColumns.Status].CssClass = "backgroundError";

            if (_selectedAddressKey != -1)
            {
                if (e.Row.Cells[0].Text == _selectedAddressKey.ToString())
                {
                    this.SelectedIndex = e.Row.RowIndex;                    
                }

            }
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
           
            base.OnRowDataBound(e);
          
        }

        void AddressGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            _address = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds a collection of <see cref="ILegalEntityAddress"/> entities to the grid.
        /// </summary>
        /// <param name="addresses"></param>
        public void BindAddressList(IEventList<ILegalEntityAddress> addresses)
        {
            BindAddressList(addresses, null);
        }

      

       
        /// <summary>
        /// Binds a collection of <see cref="ILegalEntityAddress"/> entities to the grid.
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="failedAddresses">Addresses that did not successfully migrate (dirty addresses).</param>
        public void BindAddressList(IEventList<ILegalEntityAddress> addresses, IEventList<IFailedLegalEntityAddress> failedAddresses)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            foreach (ILegalEntityAddress leAddress in addresses)
            {
                if (!ShowInactive && (leAddress.GeneralStatus.Key == (int)GeneralStatuses.Inactive))
                    continue;

                gridEntities.Add(
                    new GridEntity(leAddress.Key,
                        (int)AddressSources.LegalEntityAddress,
                        leAddress.LegalEntity.DisplayName,
                        leAddress.AddressType.Description,
                        leAddress.Address.AddressFormat.Description,
                        leAddress.Address.GetFormattedDescription(AddressDelimiters.Comma),
                        leAddress.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat),
                        leAddress.GeneralStatus.Description
                    )
                );
            }

            // loop through the dirty address if there are any
            if (failedAddresses != null)
            {
                foreach (IFailedLegalEntityAddress dirtyAddress in failedAddresses)
                {
                    IFailedStreetMigration failedStreetAddress = dirtyAddress.FailedStreetMigration;
                    if (failedStreetAddress != null)
                    {
                        gridEntities.Add(
                            new GridEntity(dirtyAddress.Key,
                                (int)AddressSources.FailedLegalEntityAddress,
                                "-",
                                AddressTypes.Residential.ToString(),
                                "Unknown",
                                failedStreetAddress.GetFormattedDescription(AddressDelimiters.Comma),
                                "-",
                                "Dirty"
                            )
                        );
                    }
                    else 
                    {
                        IFailedPostalMigration failedPostalAddress = dirtyAddress.FailedPostalMigration;
                        gridEntities.Add(
                            new GridEntity(dirtyAddress.Key,
                                (int)AddressSources.FailedLegalEntityAddress,
                                "-",
                                AddressTypes.Postal.ToString(),
                                "Unknown",
                                failedPostalAddress.GetFormattedDescription(AddressDelimiters.Comma),
                                "-",
                                "Dirty"
                            )
                        );
                    }

                }
            }

            DataSource = gridEntities;
        
            DataBind();            
        }

        

        /// <summary>
        /// Binds a collection of <see cref="IAddress"/> entities to the grid.
        /// </summary>
        /// <param name="addresses"></param>
        public void BindAddressList(IEventList<IAddress> addresses)
        {
            List<GridEntity> gridEntities = new List<GridEntity>();
            foreach (IAddress address in addresses)
            {
                gridEntities.Add(
                    new GridEntity(address.Key,
                        (int)AddressSources.Address,
                         "-",
                        null,

                        address.AddressFormat.Description,
                        address.GetFormattedDescription(AddressDelimiters.Comma),
                        null,
                        null
                    )
                );
            }
            DataSource = gridEntities;
        
            DataBind();
        }


        #endregion

        #region Properties


        /// <summary>
        /// Gets/sets whether the Address column is visible on the grid.
        /// </summary>
        public bool AddressColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.Address].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Address].Visible = value;
            }
        }

        public bool LegalEntityColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.LegalEntityName].Visible;
            }
            set
            {
                Columns[(int)GridColumns.LegalEntityName].Visible = value;
                _showLegalEntityColumn = value;
            }
        }


        /// <summary>
        /// Gets/sets whether the Effective Date column is visible on the grid.
        /// </summary>
        public bool EffectiveDateColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.EffectiveDate].Visible;
            }
            set
            {
                Columns[(int)GridColumns.EffectiveDate].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Format column is visible on the grid.
        /// </summary>
        public bool FormatColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.Format].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Format].Visible = value;
            }
        }

        /// <summary>
        /// Gets a reference to the currently selected address object.  If no row is selected, a null is returned.
        /// Otherwise, an object of the correct type is returned by the grid - this can be an IAddress, 
        /// ILegalEntityAddress, or IFailedLegalEntityAddress.  This should be used in conjunction with 
        /// <see cref="SelectedAddressSource"/>, particularly in the case of failed legal entity address objects that 
        /// 
        /// 
        /// </summary>
        public object SelectedAddress
        {
            get
            {
                if (_address == null && Rows.Count > 0 && SelectedIndex > -1)
                {
                    IAddressRepository rep = RepositoryFactory.GetRepository<IAddressRepository>();
                    AddressSources addressGridType = (AddressSources)Int32.Parse(Rows[SelectedIndex].Cells[(int)GridColumns.AddressGridSource].Text);
                    int key = Int32.Parse(Rows[SelectedIndex].Cells[(int)GridColumns.Key].Text);

                    switch (addressGridType)
                    {
                        case AddressSources.Address:
                            _address = rep.GetAddressByKey(key);
                            break;
                        case AddressSources.FailedLegalEntityAddress:
                            _address = rep.GetFailedLegalEntityAddressByKey(key);
                            break;
                        case AddressSources.LegalEntityAddress:
                            _address = rep.GetLegalEntityAddressByKey(key);
                            break;
                    }
                }
                return _address;
            }
        }

        /// <summary>
        /// Property to determine the source of the address that is currently selected.
        /// </summary>
        public AddressSources SelectedAddressSource
        {
            get
            {
                if (Rows.Count > 0 && SelectedIndex > -1)
                {
                    return (AddressSources)Int32.Parse(Rows[SelectedIndex].Cells[(int)GridColumns.AddressGridSource].Text);

                }
                return AddressSources.None;
            }
        }

        /// <summary>
        /// Gets/sets whether to show inactive items.  This will only apply if LegalEntityAddress objects 
        /// are bound to the grid.  The default value is <c>false</c>.
        /// </summary>
        /// <remarks>This must be set before the data is bound to the grid.</remarks>
        public bool ShowInactive
        {
            get
            {
                return _showInactive;
            }
            set
            {
                _showInactive = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Status column is visible on the grid.
        /// </summary>
        public bool StatusColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.Status].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Status].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Type column is visible on the grid.
        /// </summary>
        public bool TypeColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.Type].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Type].Visible = value;
            }
        }

        /// <summary>
        /// used by the application wizard to set the index of the grid based on addresskey
        /// </summary>
        public int SelectedAddressKey
        {
            set
            {
                _selectedAddressKey = value;
            }
        }

        #endregion

        #region Overrides
      

        #endregion

        #region Private Classes

        /// <summary>
        /// Internal class to make binding to the grid a little easier as we have to support different data source entities.
        /// </summary>
        private class GridEntity
        {
            private int _key;
            private int _addressSource;
            private string _legalEntityName;
            private string _type;
            private string _format;
            private string _addressDescription;
            private string _effectiveDate;
            private string _status;

            public GridEntity(int key, int addressSource, string legalEntityName,
                string type, string format, string addressDescription, string effectiveDate, string status
                )
            {
                _key = key;
                _addressSource = addressSource;
                _legalEntityName = legalEntityName;
                _type = type;
                _format = format;
                _addressDescription = addressDescription;
                _effectiveDate = effectiveDate;
                _status = status;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public int Key
            {
                get { return _key; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public int AddressSource
            {
                get { return _addressSource; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string LegalEntityName
            {
                get { return _legalEntityName; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Type
            {
                get { return _type; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Format
            {
                get { return _format; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string AddressDescription
            {
                get { return _addressDescription; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string EffectiveDate
            {
                get { return _effectiveDate; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string Status
            {
                get { return _status; }
            }

        }
        #endregion
    }
}

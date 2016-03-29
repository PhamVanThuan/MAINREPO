using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class LegalEntityDomiciliumAddress : SAHLCommonBaseView, ILegalEntityDomiciliumAddress
    {
        /// <summary>
        /// Set Controls for Display
        /// </summary>
        public void SetControlsForDisplay(string groupingText)
        {
            activeDomiciliumAddressRow.Visible = _showActiveDomiciliumAddressRow;
            addressSelectionRow.Visible = false;
            ViewButtons.Visible = false;

            pnlDomiciliumAddressDetails.GroupingText = groupingText;
        }

        /// <summary>
        /// Set View Controls for Update
        /// </summary>
        public void SetControlsForUpdate()
        {
            activeDomiciliumAddressRow.Visible = false;
            addressSelectionRow.Visible = true;
            ViewButtons.Visible = true;
            pnlDomiciliumAddressDetails.Visible = false;
            _showActiveDomiciliumAddressRow = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addressList"></param>
        public void PopulateAddressGrid(List<AddressBindableObject> addressList)
        {
            gvAddresses.KeyFieldName = "AddressKey";
            gvAddresses.AddGridCheckBoxColumn("Select", HorizontalAlign.Center, true);
            gvAddresses.AddGridColumn("AddressKey", "AddressKey", 0, GridFormatType.GridNumber, null, HorizontalAlign.Left, false, true);
            gvAddresses.AddGridColumn("FormattedAddress", "Address", 70, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            gvAddresses.AddGridColumn("DomiciliumAddressTypeKey", "Type", 30, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);

            gvAddresses.DataSource = addressList;
            gvAddresses.DataBind();
        }

        protected void gvAddresses_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Caption == "Type")
            {
                AddressBindableObject abbo = gvAddresses.GetRow(e.VisibleIndex) as AddressBindableObject;

                switch (abbo.DomiciliumAddressTypeKey)
                {
                    case (int)SAHL.Common.Globals.DomiciliumAddressTypes.Active:
                        e.Cell.Text = "Active Domicilium Address";
                        break;

                    case (int)SAHL.Common.Globals.DomiciliumAddressTypes.Property:
                        e.Cell.Text = "Property Address";
                        break;

                    case (int)SAHL.Common.Globals.DomiciliumAddressTypes.LegalEntity:
                        e.Cell.Text = abbo.Address.AddressFormat.Description;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Bind Domicilium address text
        /// </summary>
        /// <param name="address"></param>
        public void BindDomiciliumAddress(string address)
        {
            AddressLine.Text = address.Replace(", ", "\n");
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            // get the selected addresskey and pass it thru
            List<object> selectedAddressKeys = gvAddresses.GetSelectedFieldValues("AddressKey");

            // we can only selecet one address to gets its key from the collection
            SelectedAddressKey = selectedAddressKeys != null && selectedAddressKeys.Count > 0 ? Convert.ToInt32(selectedAddressKeys[0]) : -1;

            if (onSubmitButtonClicked != null)
                onSubmitButtonClicked(sender, e);
        }

        #region EventHandlers

        /// <summary>
        /// Event Handler for Cancel Button
        /// </summary>
        public event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Event Handler for Submit Button
        /// </summary>
        public event EventHandler onSubmitButtonClicked;

        #endregion EventHandlers

        public int SelectedAddressKey
        {
            get;
            set;
        }

        #region #21429 personal loans domicilium

        private bool _showActiveDomiciliumAddressRow = false;

        /// <summary>
        /// Used for personal loans domicilium address to show the active legal entity domicilium address, if one exists
        /// </summary>
        public bool ShowActiveDomiciliumAddressRow
        {
            set
            {
                _showActiveDomiciliumAddressRow = value;
            }
        }

        /// <summary>
        /// Bind Active Domicilium address text
        /// </summary>
        /// <param name="address"></param>
        public void BindActiveDomiciliumAddress(string address)
        {
            ActiveDomiciliumAddress.Text = address.Replace(", ", "\n");
        }

        #endregion #21429 personal loans domicilium
    }
}
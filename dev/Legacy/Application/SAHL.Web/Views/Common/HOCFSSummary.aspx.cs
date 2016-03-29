using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Authentication;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class HOCFSSummary : SAHLCommonBaseView, IHOCFSSummary
    {
        #region Private Structures

        private enum TitleType
        {
            SectionalTitle = 3,
            SectionalTitleWithHOC = 7
        }

        private bool _useDefaultValue;
        private const double _defValuationAmt = 0;
        private double _totalHOCSumInsured;
        private ILookupRepository _lookUps;
        private IValuation _valuation;
        private IHOC lstHOC;
        private IHOCHistoryDetail _hocHistoryDetail;
        private int _selectedHOCInsurer;
        private int _HOCInsurerKey;
        private bool _bindValue = true;
        private bool _isHOCFSSummary;
        private bool _useHOCHistoryDetail;
        private bool _showProRataPremium = true;

        #endregion Private Structures

        #region Protected Functions Section

        /// <summary>
        /// Get looks repository and set validation summary control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;
            _lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        /// <summary>
        /// Update event of HOC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (onUpdateHOCButtonClicked != null)
                onUpdateHOCButtonClicked(sender, e);
        }

        /// <summary>
        /// Event Handler for Cancel Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelButton_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
            {
                onCancelButtonClicked(sender, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IValuation valuation
        {
            set { _valuation = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool UseHOCHistoryDetail
        {
            set
            {
                _useHOCHistoryDetail = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IHOCHistoryDetail HOCHistoryDetail
        {
            set
            {
                _hocHistoryDetail = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ShowProRataPremium
        {
            set
            {
                _showProRataPremium = value;
            }
        }

        /// <summary>
        /// RowDataBound event of Properties Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PropertiesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IProperty property = e.Row.DataItem as IProperty;
            IAddressStreet addressStreet;

            if (e.Row.DataItem != null)
            {
                addressStreet = property.Address as IAddressStreet;
                if (addressStreet != null)
                {
                    cells[0].Text = property.Key.ToString();
                    cells[1].Text = addressStreet.BuildingNumber == null ? " " : addressStreet.BuildingNumber.ToString();
                    cells[2].Text = addressStreet.BuildingName == null ? " " : addressStreet.BuildingName.ToString();
                    cells[3].Text = addressStreet.StreetNumber == null ? " " : addressStreet.StreetNumber.ToString() + " " + addressStreet.StreetName.ToString();
                    cells[4].Text = property.Address.RRR_SuburbDescription.ToString();
                    cells[5].Text = property.Address.RRR_CityDescription.ToString();
                    cells[6].Text = property.Address.RRR_ProvinceDescription.ToString();
                }
            }
        }

        /// <summary>
        /// Selected Index Change event of Properties Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PropertiesGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnPropertiesGridSelectedIndexChanged != null && PropertyGrid.SelectedIndex >= 0)
                OnPropertiesGridSelectedIndexChanged(sender, new KeyChangedEventArgs(PropertyGrid.SelectedIndex));
        }

        #endregion Protected Functions Section

        #region Private Methods

        // marked as static as does not use "this"
        private static double GetNumeric(string p_Currency)
        {
            double m_ret = 0.00;

            p_Currency = p_Currency.Replace("R", "");
            p_Currency = p_Currency.Replace(",", "");
            p_Currency = p_Currency.Replace(" ", "");
            p_Currency = p_Currency.Replace("-", "");

            if (!string.IsNullOrEmpty(p_Currency))
                m_ret = Convert.ToDouble(p_Currency);

            return m_ret;
        }

        #endregion Private Methods

        /// <summary>
        /// Set Controls for Add
        /// </summary>
        public void SetControlsForAdd()
        {
            trCommencementDate.Visible = false;
            trAnniversaryDate.Visible = false;
            trCloseDate.Visible = false;

            ddlStatus.Visible = false;
            tdHOCInsurer.Width = "150px";

            ddlHOCInsurer.Visible = true;
            lblHOCInsurer.Visible = false;

            ddlSubsidenceDescription.Visible = false;
            ddlConstructionDescription.Visible = false;

            lblTotalHOCSumInsured.Visible = false;
            lblHOCPolicyNumber.Visible = false;
        }

        /// <summary>
        /// Bind HOCInsurer DropDown
        /// </summary>
        /// <param name="hocinsurer"></param>
        public void BindHOCInsurer(IEventList<IHOCInsurer> hocinsurer)
        {
            Dictionary<int, string> hocDict = new Dictionary<int, string>();

            foreach (IHOCInsurer hoc in hocinsurer)
            {
                hocDict.Add(hoc.Key, hoc.Description);
            }

            ddlHOCInsurer.DataSource = hocDict;
            ddlHOCInsurer.DataValueField = "Key";
            ddlHOCInsurer.DataTextField = "Value";
            ddlHOCInsurer.DataBind();
        }

        public void BindPropertiesGrid(IEventList<IAddress> lstPropertyAddresses)
        {
            PropertyGrid.HeaderCaption = "Property Details";
            PropertyGrid.EffectiveDateColumnVisible = false;
            PropertyGrid.FormatColumnVisible = false;
            PropertyGrid.StatusColumnVisible = false;
            PropertyGrid.TypeColumnVisible = false;

            PropertyGrid.GridHeight = 100;

            PropertyGrid.BindAddressList(lstPropertyAddresses);
        }

        /// <summary>
        /// Show Display Controls
        /// </summary>
        public bool HOCDetailsDisplay
        {
            set
            {
                lblStatus.Visible = value;
                lblHOCInsurer.Visible = value;
                lblSubsidenceDescription.Visible = value;
                lblConstructionDescription.Visible = value;
                lblTotalHOCSumInsured.Visible = value;
                lblHOCPolicyNumber.Visible = value;
                chkCeded.Enabled = false;
            }
        }

        /// <summary>
        /// Show Update display controls
        /// </summary>
        public bool HOCDetailsUpdateDisplay
        {
            set
            {
                lblCommencementDate.Visible = value;
                lblAnniversaryDate.Visible = value;
                lblCloseDate.Visible = value;
                lblRoofDescription.Visible = value;
                lblThatchValuation.Visible = value;
                lblConventionalValuation.Visible = value;
                lblShingleValuation.Visible = value;
            }
        }

        /// <summary>
        /// Set screen default values for HOC Add
        /// </summary>
        /// <param name="hocstatus"></param>
        /// <param name="hocsubsidence"></param>
        /// <param name="hocroof"></param>
        /// <param name="hocconstruction"></param>
        public void SetDefaultValuesForAdd(IEventList<IHOCStatus> hocstatus, IEventList<IHOCSubsidence> hocsubsidence, IEventList<IHOCRoof> hocroof, IEventList<IHOCConstruction> hocconstruction)
        {
            lblStatus.Text = hocstatus.ObjectDictionary[((int)HocStatuses.Open).ToString()].Description; // Defaults to Open
            lblSubsidenceDescription.Text = hocsubsidence.ObjectDictionary[((int)HOCSubsidences.NotRequired).ToString()].Description;// as per LH, this defaults to Not Required
            lblConstructionDescription.Text = hocconstruction.ObjectDictionary[((int)HOCConstructions.BrickandTile).ToString()].Description;

            if (_valuation != null && _valuation.HOCRoof != null)
                lblRoofDescription.Text = _valuation.HOCRoof.Description;
            else
                lblRoofDescription.Text = hocroof.ObjectDictionary[((int)HOCRoofs.Conventional).ToString()].Description.ToString(); // This must change to a calcution once val objects are exposed on the domain

            if (_valuation != null && _selectedHOCInsurer == (int)HOCInsurers.SAHLHOC)
            {
                lblThatchValuation.Text = Convert.ToDouble(_valuation.HOCThatchAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                lblConventionalValuation.Text = Convert.ToDouble(_valuation.HOCConventionalAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                lblShingleValuation.Text = Convert.ToDouble(_valuation.HOCShingleAmount).ToString(SAHL.Common.Constants.CurrencyFormat);

                if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
                {
                    txtTotalHOCSumInsured.Text = Convert.ToString(Convert.ToDouble(_valuation.HOCThatchAmount) + Convert.ToDouble(_valuation.HOCConventionalAmount) + Convert.ToDouble(_valuation.HOCShingleAmount));
                }
            }
            else if (_valuation == null && _selectedHOCInsurer == (int)HOCInsurers.SAHLHOC)
            {
                lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                // Set Conventional Valuation
                if (_useDefaultValue)
                    lblConventionalValuation.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                else
                    lblConventionalValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                // Set Shingle Valuation
                lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                // Set Total HOC Sum Insured
                if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
                {
                    if (_useDefaultValue)
                        txtTotalHOCSumInsured.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                    else
                        txtTotalHOCSumInsured.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
            else
            {
                lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);

                if (_valuation != null)
                {
                    lblConventionalValuation.Text = (_valuation.HOCConventionalAmount.HasValue ? _valuation.HOCConventionalAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                    lblThatchValuation.Text = (_valuation.HOCThatchAmount.HasValue ? _valuation.HOCThatchAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                    lblShingleValuation.Text = (_valuation.HOCShingleAmount.HasValue ? _valuation.HOCShingleAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                }
                else
                {
                    lblConventionalValuation.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                }

                if (_bindValue)
                    txtTotalHOCSumInsured.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// Shows updateable controls
        /// </summary>
        public bool HOCDetailsUpdate
        {
            set
            {
                ddlStatus.Visible = value;
                ddlHOCInsurer.Visible = value;
                ddlSubsidenceDescription.Visible = value;
                ddlConstructionDescription.Visible = value;
                txtHOCPolicyNumber.Visible = value;
                txtTotalHOCSumInsured.Visible = value;
            }
        }

        /// <summary>
        /// Show Premium panel - dependant on HOC Insurer
        /// </summary>
        public bool HOCPremiumPanelVisible
        {
            set
            {
                pnlPremiums.Visible = value;

                if (value == true)
                {
                    txtHOCPolicyNumber.Enabled = false;
                    txtTotalHOCSumInsured.Enabled = false;
                    chkCeded.Checked = false;
                    chkCeded.Enabled = false;
                }
                else
                {
                    txtHOCPolicyNumber.Enabled = true;
                    txtTotalHOCSumInsured.Enabled = true;
                    chkCeded.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Visibility of submit button
        /// </summary>
        public bool HOCUpdateButtonVisible
        {
            set { btnSubmitButton.Visible = value; }
        }

        /// <summary>
        /// Visibility of Cancel Button
        /// </summary>
        public bool HOCCancelButtonVisible
        {
            set { btnCancelButton.Visible = value; }
        }

        /// <summary>
        /// Selected HOC Insurer Key
        /// </summary>
        public int HOCInsurerValueChange
        {
            set { ddlHOCInsurer.SelectedValue = value.ToString(); }
        }

        /// <summary>
        /// Implements <see cref="IHOCFSSummary.SelectedHOCStatusValue"/>
        /// </summary>
        public string SelectedHOCStatusValue
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlStatus.UniqueID]) && Request.Form[ddlStatus.UniqueID] != "-select-")
                    return (Convert.ToInt32(Request.Form[ddlStatus.UniqueID])).ToString();
                else if (!string.IsNullOrEmpty(ddlStatus.SelectedValue) && ddlStatus.SelectedValue != "-select-")
                    return (Convert.ToString(ddlStatus.SelectedValue));
                else
                    return Convert.ToString(-1);
            }
        }

        /// <summary>
        /// Implements <see cref="IHOCFSSummary.SelectedHOCInsurerValue"/>
        /// </summary>
        public string SelectedHOCInsurerValue
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlHOCInsurer.UniqueID]) && Request.Form[ddlHOCInsurer.UniqueID] != "-select-")
                    return (Convert.ToInt32(Request.Form[ddlHOCInsurer.UniqueID])).ToString();
                else if (!string.IsNullOrEmpty(ddlHOCInsurer.SelectedValue) && ddlHOCInsurer.SelectedValue != "-select-")
                    return (Convert.ToString(ddlHOCInsurer.SelectedValue));
                else
                    return Convert.ToString(-1);
            }
            set
            {
                _selectedHOCInsurer = Convert.ToInt32(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string SetUpdateButtonText
        {
            set { btnSubmitButton.Text = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public double TotalHOCSumInsured
        {
            set { _totalHOCSumInsured = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool UseDefaultValue
        {
            set { _useDefaultValue = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int SetHOCInsurerKey
        {
            set { _HOCInsurerKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool SetBindValue
        {
            set { _bindValue = value; }
        }

        public bool IsHOCFSSummary
        {
            set
            {
                _isHOCFSSummary = value;
            }
        }

        #region Members

        /// <summary>
        /// Populate the HOC Record for HOC Add - Application
        /// </summary>
        /// <param name="valuation"></param>
        /// <param name="HOC"></param>
        /// <returns></returns>
        public IHOC GetCapturedHOCRecordForAdd(IValuation valuation, IHOC HOC)
        {
            //HOC.HOCInsurer = _lookUps.HOCInsurers.ObjectDictionary[ddlHOCInsurer.SelectedValue];
            IHOCRepository _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            HOC.HOCInsurer = _hocRepo.GetHOCInsurerByKey(Convert.ToInt32(ddlHOCInsurer.SelectedValue));
            HOC.CommencementDate = null;
            HOC.HOCConstruction = _lookUps.HOCConstruction.ObjectDictionary[((int)HOCConstructions.BrickandTile).ToString()];// must change to what has been calculated
            if (valuation != null && Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
            {
                HOC.HOCConventionalAmount = Convert.ToDouble(valuation.HOCConventionalAmount);
                HOC.HOCThatchAmount = Convert.ToDouble(valuation.HOCThatchAmount);
                HOC.HOCShingleAmount = Convert.ToDouble(valuation.HOCShingleAmount);
            }
            else if (valuation == null && Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
            {
                if (_useDefaultValue)
                    HOC.HOCConventionalAmount = _totalHOCSumInsured;
                else
                    HOC.HOCConventionalAmount = _defValuationAmt;

                HOC.HOCThatchAmount = _defValuationAmt;
                HOC.HOCShingleAmount = _defValuationAmt;
            }
            else
            {
                if (valuation != null)
                {
                    HOC.HOCConventionalAmount = (valuation.HOCConventionalAmount.HasValue ? Convert.ToDouble(valuation.HOCConventionalAmount) : _defValuationAmt);
                    HOC.HOCThatchAmount = (valuation.HOCThatchAmount.HasValue ? Convert.ToDouble(valuation.HOCThatchAmount) : _defValuationAmt);
                    HOC.HOCShingleAmount = (valuation.HOCShingleAmount.HasValue ? Convert.ToDouble(valuation.HOCShingleAmount) : _defValuationAmt);
                }
                else
                {
                    HOC.HOCConventionalAmount = _totalHOCSumInsured;
                    HOC.HOCThatchAmount = _defValuationAmt;
                    HOC.HOCShingleAmount = _defValuationAmt;
                }
            }

            if (valuation != null && valuation.HOCRoof != null)
                HOC.HOCRoof = valuation.HOCRoof;
            else
                HOC.HOCRoof = _lookUps.HOCRoof.ObjectDictionary[((int)HOCRoofs.Conventional).ToString()]; // must be calculated

            HOC.HOCStatus = _lookUps.HOCStatus.ObjectDictionary[((int)SAHL.Common.Globals.HocStatuses.Open).ToString()]; // Defaults to open
            HOC.HOCSubsidence = _lookUps.HOCSubsidence.ObjectDictionary[((int)HOCSubsidences.NotRequired).ToString()]; // Defaults to Not Required
            HOC.UserID = this.CurrentPrincipal.Identity.Name;
            HOC.ChangeDate = DateTime.Now;

            // commented by eugene 27/11/2007 - should no longer need to set a financialservices type
            //            HOC.FinancialServiceType = _lookUps.FinancialServiceGroups[2].FinancialServiceTypes[0];

            if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
            {
                double totalSumInsured = 0;

                if (_valuation != null && _valuation is IValuationDiscriminatedLightstoneAVM)
                    totalSumInsured = Convert.ToDouble(_valuation.ValuationHOCValue);
                else if (valuation != null)
                    totalSumInsured = Convert.ToDouble(valuation.HOCConventionalAmount) + Convert.ToDouble(valuation.HOCShingleAmount) + Convert.ToDouble(valuation.HOCThatchAmount);
                else if (_useDefaultValue)
                    totalSumInsured = _totalHOCSumInsured;
                else
                    totalSumInsured = _defValuationAmt;

                HOC.SetHOCTotalSumInsured(totalSumInsured);
                HOC.HOCPolicyNumber = "";
            }
            else
            {
                HOC.SetHOCTotalSumInsured(Convert.ToDouble(GetNumeric(txtTotalHOCSumInsured.Text)));
                HOC.HOCPolicyNumber = txtHOCPolicyNumber.Text;
                if (chkCeded.Checked)
                    HOC.Ceded = true;
            }

            return HOC;
        }

        /// <summary>
        /// Get Amended HOC Record (Used for Update of an Open HOC Account
        /// </summary>
        /// <param name="hoc"></param>
        /// <returns></returns>
        ///
        public IHOC GetCapturedHOC(IHOC hoc)
        {
            IHOCRepository _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            if (!_useHOCHistoryDetail)
            {
                if (_valuation != null && Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
                {
                    hoc.HOCConventionalAmount = (_valuation.HOCConventionalAmount.HasValue ? _valuation.HOCConventionalAmount : 0);
                    hoc.HOCThatchAmount = (_valuation.HOCThatchAmount.HasValue ? _valuation.HOCThatchAmount : 0);
                    hoc.HOCShingleAmount = (_valuation.HOCShingleAmount.HasValue ? _valuation.HOCShingleAmount : 0);
                    double _hocTotalSumInsured = ((_valuation.HOCShingleAmount.HasValue ? _valuation.HOCShingleAmount.Value : 0)
                        + (_valuation.HOCConventionalAmount.HasValue ? _valuation.HOCConventionalAmount.Value : 0)
                        + (_valuation.HOCThatchAmount.HasValue ? _valuation.HOCThatchAmount.Value : 0));
                    hoc.SetHOCTotalSumInsured(_hocTotalSumInsured);
                }
                else if (_valuation == null && Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC)
                {
                    hoc.HOCConventionalAmount = hoc.HOCTotalSumInsured;
                    hoc.HOCThatchAmount = _defValuationAmt;
                    hoc.HOCShingleAmount = _defValuationAmt;
                    hoc.SetHOCTotalSumInsured(hoc.HOCConventionalAmount.Value + hoc.HOCThatchAmount.Value + hoc.HOCShingleAmount.Value);
                }
                else
                {
                    if (_valuation != null)
                    {
                        hoc.HOCConventionalAmount = (_valuation.HOCConventionalAmount.HasValue ? Convert.ToDouble(_valuation.HOCConventionalAmount) : _defValuationAmt);
                        hoc.HOCThatchAmount = (_valuation.HOCThatchAmount.HasValue ? Convert.ToDouble(_valuation.HOCThatchAmount) : _defValuationAmt);
                        hoc.HOCShingleAmount = (_valuation.HOCShingleAmount.HasValue ? Convert.ToDouble(_valuation.HOCShingleAmount) : _defValuationAmt);
                    }
                    else
                    {
                        hoc.HOCConventionalAmount = _totalHOCSumInsured;
                        hoc.HOCThatchAmount = _defValuationAmt;
                        hoc.HOCShingleAmount = _defValuationAmt;
                    }
                }
            }
            else
            {
                hoc.HOCConventionalAmount = (_hocHistoryDetail.HOCConventionalAmount.HasValue ? _hocHistoryDetail.HOCConventionalAmount : 0);
                hoc.HOCThatchAmount = (_hocHistoryDetail.HOCThatchAmount.HasValue ? _hocHistoryDetail.HOCThatchAmount : 0);
                hoc.HOCShingleAmount = (_hocHistoryDetail.HOCShingleAmount.HasValue ? _hocHistoryDetail.HOCShingleAmount : 0);
                double _hocTotalSumInsured = ((_hocHistoryDetail.HOCShingleAmount.HasValue ? _hocHistoryDetail.HOCShingleAmount.Value : 0)
                    + (_hocHistoryDetail.HOCConventionalAmount.HasValue ? _hocHistoryDetail.HOCConventionalAmount.Value : 0)
                    + (_hocHistoryDetail.HOCThatchAmount.HasValue ? _hocHistoryDetail.HOCThatchAmount.Value : 0));
                hoc.SetHOCTotalSumInsured(_hocTotalSumInsured);
            }
            // Changing to SAHL Insurer
            if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC && hoc.HOCInsurer.Key != (int)HOCInsurers.SAHLHOC)
            {
                hoc.HOCInsurer = _hocRepo.GetHOCInsurerByKey(Convert.ToInt32(ddlHOCInsurer.SelectedValue));
                hoc.HOCPolicyNumber = hoc.FinancialService.Account.Key.ToString();
                hoc.Ceded = false;
                hoc.CommencementDate = DateTime.Now;
                hoc.CancellationDate = null;
                DateTime nextMonth = DateTime.Now.AddMonths(1);
                nextMonth = nextMonth.AddDays((nextMonth.Day * -1) + 1);
                DateTime anniversaryDate = nextMonth.AddYears(1);
                hoc.AnniversaryDate = anniversaryDate;
            }
            // If remaining with SAHL
            else if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) == (int)HOCInsurers.SAHLHOC && hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC)
            {
                hoc.Ceded = false;
            }
            // Changing from SAHLHOC to Another Company
            else if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) != (int)HOCInsurers.SAHLHOC && hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC)
            {
                hoc.CommencementDate = DateTime.Now;
                hoc.CancellationDate = DateTime.Now;
                hoc.AnniversaryDate = null;
            }
            // Changing from one Non-SAHL Insurer to another Non-SAHL Insurer
            else if ((Convert.ToInt32(ddlHOCInsurer.SelectedValue) != (int)HOCInsurers.SAHLHOC &&
              hoc.HOCInsurer.Key != (int)HOCInsurers.SAHLHOC) &&
                Convert.ToInt32(ddlHOCInsurer.SelectedValue) != hoc.HOCInsurer.Key)
            {
                hoc.CommencementDate = DateTime.Now;
                hoc.CancellationDate = null;
                hoc.AnniversaryDate = null;
            }

            hoc.HOCInsurer = _hocRepo.GetHOCInsurerByKey(Convert.ToInt32(ddlHOCInsurer.SelectedValue));

            if (Convert.ToInt32(ddlStatus.SelectedIndex) != -1) // This is cos in the case of HOC App Update, these fields should not be updated
                hoc.HOCStatus = _lookUps.HOCStatus[Convert.ToInt32(ddlStatus.SelectedIndex)];

            if (Convert.ToInt32(ddlSubsidenceDescription.SelectedIndex) != -1)
                hoc.HOCSubsidence = _lookUps.HOCSubsidence[Convert.ToInt32(ddlSubsidenceDescription.SelectedIndex)];

            if (Convert.ToInt32(ddlConstructionDescription.SelectedIndex) != -1)
                hoc.HOCConstruction = _lookUps.HOCConstruction[Convert.ToInt32(ddlConstructionDescription.SelectedIndex)];

            if (_valuation != null && _valuation.HOCRoof != null && !_useHOCHistoryDetail)
                hoc.HOCRoof = _valuation.HOCRoof;
            else
                hoc.HOCRoof = _lookUps.HOCRoof.ObjectDictionary[((int)HOCRoofs.Conventional).ToString()]; // must be calculated

            hoc.UserID = this.CurrentPrincipal.Identity.Name;
            hoc.ChangeDate = DateTime.Now;

            // if SAHLInsurer not equal SAHL
            if (Convert.ToInt32(ddlHOCInsurer.SelectedValue) != (int)HOCInsurers.SAHLHOC)
            {
                double sHOCTotalSumInsured = GetNumeric(txtTotalHOCSumInsured.Text);
                hoc.SetHOCTotalSumInsured(Convert.ToDouble(sHOCTotalSumInsured));
                hoc.HOCPolicyNumber = txtHOCPolicyNumber.Text;
                if (chkCeded.Checked)
                    hoc.Ceded = true;
                else
                    hoc.Ceded = false;
            }

            return hoc;
        }

        /// <summary>
        /// Bind HOC Summary Data
        /// </summary>
        /// <param name="HOC"></param>
        public void BindHOCSummaryData(IHOC HOC)
        {
            lstHOC = HOC;

            lblCommencementDate.Text = lstHOC.CommencementDate == null ? "-" : Convert.ToDateTime(lstHOC.CommencementDate).ToString(SAHL.Common.Constants.DateFormat);

            lblStatus.Text = lstHOC.HOCStatus.Description == null ? "-" : lstHOC.HOCStatus.Description;
            ddlStatus.SelectedValue = lstHOC.HOCStatus.Key.ToString();

            lblAnniversaryDate.Text = lstHOC.AnniversaryDate == null ? "-" : Convert.ToDateTime(lstHOC.AnniversaryDate).ToString(SAHL.Common.Constants.DateFormat);
            lblCloseDate.Text = lstHOC.CancellationDate == null ? "-" : Convert.ToDateTime(lstHOC.CancellationDate).ToString(SAHL.Common.Constants.DateFormat);

            lblHOCInsurer.Text = lstHOC.HOCInsurer == null ? "-" : lstHOC.HOCInsurer.Description;

            if (_bindValue)
                ddlHOCInsurer.SelectedValue = lstHOC.HOCInsurer.Key.ToString();

            lblSubsidenceDescription.Text = lstHOC.HOCSubsidence == null ? "-" : lstHOC.HOCSubsidence.Description;
            ddlSubsidenceDescription.SelectedValue = lstHOC.HOCSubsidence.Key.ToString();

            lblConstructionDescription.Text = lstHOC.HOCConstruction.Description == null ? "-" : lstHOC.HOCConstruction.Description;
            ddlConstructionDescription.SelectedValue = lstHOC.HOCConstruction.Key.ToString();

            lblHOCPolicyNumber.Text = lstHOC.HOCPolicyNumber == null ? "-" : lstHOC.HOCPolicyNumber;
            if (_bindValue)
                txtHOCPolicyNumber.Text = lstHOC.HOCPolicyNumber == null ? "-" : lstHOC.HOCPolicyNumber;

            if (lstHOC.Ceded == true)
                chkCeded.Checked = true;
            else
                chkCeded.Checked = false;

            if (_isHOCFSSummary)
            {
                lblThatchValuation.Text = (lstHOC.HOCThatchAmount.HasValue ? lstHOC.HOCThatchAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-");
                lblShingleValuation.Text = (lstHOC.HOCShingleAmount.HasValue ? lstHOC.HOCShingleAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-");
                lblConventionalValuation.Text = (lstHOC.HOCConventionalAmount.HasValue ? lstHOC.HOCConventionalAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-");
                lblTotalHOCSumInsured.Text = lstHOC.HOCTotalSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblRoofDescription.Text = lstHOC.HOCRoof.Description == null ? "-" : lstHOC.HOCRoof.Description;
                return;
            }

            if (_valuation != null && _valuation.HOCRoof != null && !_useHOCHistoryDetail)
                lblRoofDescription.Text = _valuation.HOCRoof.Description;
            else
                lblRoofDescription.Text = lstHOC.HOCRoof.Description == null ? "-" : lstHOC.HOCRoof.Description;

            if (_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC && txtTotalHOCSumInsured.ReadOnly == true)
                txtTotalHOCSumInsured.ReadOnly = false;

            if (!_useHOCHistoryDetail)
            {
                if ((_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC && _HOCInsurerKey == (int)HOCInsurers.SAHLHOC)
                    && _valuation == null)
                {
                    lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblConventionalValuation.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblTotalHOCSumInsured.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtTotalHOCSumInsured.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else if ((_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC && _HOCInsurerKey == (int)HOCInsurers.SAHLHOC)
                    && _valuation != null)
                {
                    lblThatchValuation.Text = Convert.ToDouble(_valuation.HOCThatchAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblConventionalValuation.Text = Convert.ToDouble(_valuation.HOCConventionalAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShingleValuation.Text = Convert.ToDouble(_valuation.HOCShingleAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblTotalHOCSumInsured.Text = Convert.ToString(Convert.ToDouble(_valuation.HOCThatchAmount) + Convert.ToDouble(_valuation.HOCConventionalAmount) + Convert.ToDouble(_valuation.HOCShingleAmount));
                    txtTotalHOCSumInsured.Text = Convert.ToString(Convert.ToDouble(_valuation.HOCThatchAmount) + Convert.ToDouble(_valuation.HOCConventionalAmount) + Convert.ToDouble(_valuation.HOCShingleAmount));
                }
                else if ((_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC && _HOCInsurerKey != (int)HOCInsurers.SAHLHOC)
                    && _valuation == null)
                {
                    lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblConventionalValuation.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblTotalHOCSumInsured.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                    txtTotalHOCSumInsured.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else if ((_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC && _HOCInsurerKey != (int)HOCInsurers.SAHLHOC)
                && _valuation != null)
                {
                    lblThatchValuation.Text = Convert.ToDouble(_valuation.HOCThatchAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblConventionalValuation.Text = Convert.ToDouble(_valuation.HOCConventionalAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShingleValuation.Text = Convert.ToDouble(_valuation.HOCShingleAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblTotalHOCSumInsured.Text = Convert.ToString(Convert.ToDouble(_valuation.HOCThatchAmount) + Convert.ToDouble(_valuation.HOCConventionalAmount) + Convert.ToDouble(_valuation.HOCShingleAmount));
                    txtTotalHOCSumInsured.Text = Convert.ToString(Convert.ToDouble(_valuation.HOCThatchAmount) + Convert.ToDouble(_valuation.HOCConventionalAmount) + Convert.ToDouble(_valuation.HOCShingleAmount));
                }
                else if (_HOCInsurerKey == _selectedHOCInsurer)
                {
                    if (_valuation != null)
                    {
                        lblConventionalValuation.Text = (_valuation.HOCConventionalAmount.HasValue ? _valuation.HOCConventionalAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                        lblThatchValuation.Text = (_valuation.HOCThatchAmount.HasValue ? _valuation.HOCThatchAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                        lblShingleValuation.Text = (_valuation.HOCShingleAmount.HasValue ? _valuation.HOCShingleAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                    }
                    else
                    {
                        lblConventionalValuation.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                        lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                        lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    }
                    lblTotalHOCSumInsured.Text = Convert.ToDouble(lstHOC.HOCTotalSumInsured).ToString(SAHL.Common.Constants.CurrencyFormat);
                    if (_bindValue)
                        txtTotalHOCSumInsured.Text = Convert.ToDouble(lstHOC.HOCTotalSumInsured).ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                {
                    lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);

                    if (_valuation != null)
                    {
                        lblConventionalValuation.Text = (_valuation.HOCConventionalAmount.HasValue ? _valuation.HOCConventionalAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                        lblThatchValuation.Text = (_valuation.HOCThatchAmount.HasValue ? _valuation.HOCThatchAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                        lblShingleValuation.Text = (_valuation.HOCShingleAmount.HasValue ? _valuation.HOCShingleAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat));
                    }
                    else
                    {
                        lblConventionalValuation.Text = _totalHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                        lblThatchValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                        lblShingleValuation.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    }

                    lblTotalHOCSumInsured.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);

                    if (_bindValue)
                        txtTotalHOCSumInsured.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
            else
            {
                lblThatchValuation.Text = _hocHistoryDetail.HOCThatchAmount.HasValue ? _hocHistoryDetail.HOCThatchAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblShingleValuation.Text = _hocHistoryDetail.HOCShingleAmount.HasValue ? _hocHistoryDetail.HOCShingleAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblConventionalValuation.Text = _hocHistoryDetail.HOCConventionalAmount.HasValue ? _hocHistoryDetail.HOCConventionalAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                double hhdHOCSumInsured = (_hocHistoryDetail.HOCThatchAmount.HasValue ? _hocHistoryDetail.HOCThatchAmount.Value : _defValuationAmt)
                                            + (_hocHistoryDetail.HOCShingleAmount.HasValue ? _hocHistoryDetail.HOCShingleAmount.Value : _defValuationAmt)
                                            + (_hocHistoryDetail.HOCConventionalAmount.HasValue ? _hocHistoryDetail.HOCConventionalAmount.Value : _defValuationAmt);
                lblTotalHOCSumInsured.Text = hhdHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtTotalHOCSumInsured.Text = hhdHOCSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            if (_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC && txtTotalHOCSumInsured.ReadOnly == false)
                txtTotalHOCSumInsured.ReadOnly = true;
        }

        /// <summary>
        /// Calculates HOC Premims on Change of Insurer to SAHL
        /// </summary>
        public void ShowCalculatedHOCPremiums(string action)
        {
            if (_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC)
            {
                if (action == "HOCFSSummary")
                {
                    lstHOC.CalculatePremium();

                    //lstHOC.HOCInsurer = _lookUps.HOCInsurers.ObjectDictionary[((int)HOCInsurers.SAHLHOC).ToString()];
                    IHOCRepository _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                    lstHOC.HOCInsurer = _hocRepo.GetHOCInsurerByKey((int)HOCInsurers.SAHLHOC);

                    lblThatchPremium.Text = lstHOC.PremiumThatch.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblConventionalPremium.Text = lstHOC.PremiumConventional.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblShinglePremium.Text = lstHOC.PremiumShingle.ToString(SAHL.Common.Constants.CurrencyFormat);

                    lblProRataPremium.Text = Convert.ToDouble(lstHOC.HOCProrataPremium).ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblTotalHOCPremium.Text = Convert.ToDouble(lstHOC.HOCMonthlyPremium).ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                else
                    OnLinePremiumCalc();
            }
        }

        public void OnLinePremiumCalc()
        {
            if (_selectedHOCInsurer == (int)HOCInsurers.SAHLHOC)
            {
                IHOCRepository _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOC _hoc = _hocRepo.CreateEmptyHOC();

                // Only interested in this calculation if SAHL hence we can default it
                _hoc.HOCInsurer = _hocRepo.GetHOCInsurerByKey((int)HOCInsurers.SAHLHOC);

                // It should default in this instance if not selected
                if (!string.IsNullOrEmpty(ddlSubsidenceDescription.SelectedValue))
                    _hoc.HOCSubsidence = _lookUps.HOCSubsidence.ObjectDictionary[Convert.ToInt32(ddlSubsidenceDescription.SelectedValue).ToString()];
                else
                    _hoc.HOCSubsidence = _lookUps.HOCSubsidence.ObjectDictionary[((int)HOCSubsidences.NotRequired).ToString()];

                if (!_useHOCHistoryDetail)
                {
                    if (_valuation != null)
                    {
                        _hoc.HOCThatchAmount = _valuation.HOCThatchAmount;
                        _hoc.HOCShingleAmount = _valuation.HOCShingleAmount;
                        _hoc.HOCConventionalAmount = _valuation.HOCConventionalAmount;
                    }
                    else
                    {
                        _hoc.HOCThatchAmount = _defValuationAmt;
                        _hoc.HOCShingleAmount = _defValuationAmt;
                        _hoc.HOCConventionalAmount = _totalHOCSumInsured;
                    }
                }
                else
                {
                    _hoc.HOCThatchAmount = _hocHistoryDetail.HOCThatchAmount.HasValue ? _hocHistoryDetail.HOCThatchAmount.Value : _defValuationAmt;
                    _hoc.HOCShingleAmount = _hocHistoryDetail.HOCShingleAmount.HasValue ? _hocHistoryDetail.HOCShingleAmount.Value : _defValuationAmt;
                    _hoc.HOCConventionalAmount = _hocHistoryDetail.HOCConventionalAmount.HasValue ? _hocHistoryDetail.HOCConventionalAmount.Value : _defValuationAmt;
                }

                _hoc.CalculatePremium();

                double totalHocPremium = (_hoc.HOCMonthlyPremium.HasValue ? _hoc.HOCMonthlyPremium.Value : 0);

                if (_showProRataPremium)
                    lblProRataPremium.Text = _hoc.HOCProrataPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                else
                    lblProRataPremium.Text = _defValuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);

                lblTotalHOCPremium.Text = totalHocPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblThatchPremium.Text = _hoc.PremiumThatch.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblShinglePremium.Text = _hoc.PremiumShingle.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblConventionalPremium.Text = _hoc.PremiumConventional.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        public void ShowDefaultView(string InfoMessage)
        {
            ButtonRow.Visible = false;
            tblHOC.Visible = false;
            NoRecsLbl.Visible = true;
            NoRecsLbl.Font.Bold = true;
            NoRecsLbl.Text = InfoMessage;
        }

        /// <summary>
        /// Remove the Policy number on screen when the HOC Insurer Changes
        /// </summary>
        public void RemoveHOCPolicyNumber()
        {
            txtHOCPolicyNumber.Text = "";
        }

        /// <summary>
        /// Bind HOC Look Ups
        /// </summary>
        /// <param name="hocInsurer"></param>
        /// <param name="status"></param>
        /// <param name="Subsidence"></param>
        /// <param name="Construction"></param>
        public void BindHOCLookUpControls(IEventList<IHOCInsurer> hocInsurer, IDictionary<string, string> status, IDictionary<string, string> Subsidence, IDictionary<string, string> Construction)
        {
            IDictionary<string, string> _hocDict = new Dictionary<string, string>();
            foreach (IHOCInsurer _hocInsurer in hocInsurer)
            {
                _hocDict.Add(_hocInsurer.Key.ToString(), _hocInsurer.Description);
            }

            //ddlHOCInsurer.DataSource = hocInsurer;
            ddlHOCInsurer.DataSource = _hocDict;
            ddlHOCInsurer.DataValueField = "Key";
            ddlHOCInsurer.DataTextField = "Value";
            ddlHOCInsurer.DataBind();

            ddlStatus.DataSource = status;
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataBind();

            ddlSubsidenceDescription.DataSource = Subsidence;
            ddlSubsidenceDescription.DataValueField = "Key";
            ddlSubsidenceDescription.DataTextField = "Description";
            ddlSubsidenceDescription.DataBind();

            ddlConstructionDescription.DataSource = Construction;
            ddlConstructionDescription.DataValueField = "Key";
            ddlConstructionDescription.DataTextField = "Description";
            ddlConstructionDescription.DataBind();
        }

        /// <summary>
        /// Change of Insurer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlHOCInsurer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnddlHOCInsurerSelectedIndexChanged != null && ddlHOCInsurer.SelectedValue != "-select-")
            {
                OnddlHOCInsurerSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlHOCInsurer.SelectedValue));
            }
        }

        /// <summary>
        /// Cancel Button Clicked event handler
        /// </summary>
        public event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Event handler for update button
        /// </summary>
        public event EventHandler onUpdateHOCButtonClicked;

        /// <summary>
        /// Event handler for HOC Insurer Drop Downm List
        /// </summary>
        public event KeyChangedEventHandler OnddlHOCInsurerSelectedIndexChanged;

        /// <summary>
        /// Change event of Properties Grid
        /// </summary>
        public event KeyChangedEventHandler OnPropertiesGridSelectedIndexChanged;

        #endregion Members
    }
}
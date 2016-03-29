using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationDetails : SAHLCommonBaseView, IValuationManualDetailsView
    {
        #region Private Variables

        private bool _showButtons;
        private bool _showLabels;
        private bool _showNavigationButtons;
        private IEventList<IProperty> _properties;
        private List<ValuationDetailsValuationGridItem> _lstValuations;
        private ListItem li;
        private string presenter;
        private int _valKey;
        private ILookupRepository _lookUps;
        private IPropertyRepository _propertyRepo;

        #endregion

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnddlHOCRoofDescriptionIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnValuationDetailsClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnBackButtonClicked;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            _lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
            _propertyRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            BindPropertyGrid();
            if (_properties.Count > 0)
            {
                BindValuationsGrid(_properties[0]);
                BindLookUps();

                if (!IsPostBack)
                    BindValuationControls();
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            CancelButton.Visible = _showButtons;
            SubmitButton.Visible = _showButtons;
            BackButton.Visible = _showButtons;
            BtnValuationDetailsDisplay.Visible = _showNavigationButtons;

            lblValuer.Visible = _showLabels;
            ddlValuer.Visible = !_showLabels;

            lblValuationDate.Visible = _showLabels;
            dateValuationDate.Visible = !_showLabels;

            lblValuationAmount.Visible = _showLabels;
            txtValuationAmount.Visible = !_showLabels;

            lblHOCThatchAmount.Visible = _showLabels;
            txtHOCThatchAmount.Visible = !_showLabels;

            lblHOCRoofDescription.Visible = _showLabels;
            ddlHOCRoofDescription.Visible = !_showLabels;

            lblHOCConventionalAmount.Visible = _showLabels;
            txtHOCConventionalAmount.Visible = !_showLabels;

            lblMunicipalValuation.Visible = _showLabels;
            txtMunicipalValuation.Visible = !_showLabels;

            SetViewForValuationUpdate();
        }


        #region IValuationDetails Members



        /// <summary>
        /// 
        /// </summary>
        public void HideAllPanels()
        {
            DisplayData.Visible = false;
            ButtonRow.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowButtons
        {
            set { _showButtons = value; }
        }

        public bool ShowNavigationButtons
        {
            set { _showNavigationButtons = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowLabels
        {
            set { _showLabels = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SetHOCThatchAmountForUpdate
        {
            set
            {
                txtHOCThatchAmount.Enabled = value;
            }
        }
        /// <summary>
        ///
        /// </summary>
        public bool SetHOCConventionalAmountForUpdate
        {
            set
            {
                txtHOCConventionalAmount.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetViewForValuationUpdate()
        {
            if (presenter == "Update")
            {
                BackButton.Visible = false;
                SubmitButton.Text = "Update";
                lblHOCValuationAmount.Visible = true;
                txtHOCValuationAmount.Visible = false;
            }

        }
        
        private void BindPropertyGrid()
        {
            IEventList<IAddress> lstPropertyAddresses = new EventList<IAddress>();
            if (_properties != null)
            {
                foreach (IProperty property in _properties)
                {
                    lstPropertyAddresses.Add(this.Messages, property.Address);
                }
            }
            PropertyAddressGrid.HeaderCaption = "Property Details";
            PropertyAddressGrid.EffectiveDateColumnVisible = false;
            PropertyAddressGrid.FormatColumnVisible = false;
            PropertyAddressGrid.StatusColumnVisible = false;
            PropertyAddressGrid.TypeColumnVisible = false;

            PropertyAddressGrid.GridHeight = 50;

            PropertyAddressGrid.BindAddressList( lstPropertyAddresses);
        }

        private void BindValuationsGrid(IProperty property)
        {
            _lstValuations = new List<ValuationDetailsValuationGridItem>();

            for (int x = 0; x < property.Valuations.Count; x++)
            {
                if (property.Valuations[x] is IValuationDiscriminatedSAHLManual)
                {
                    ValuationDetailsValuationGridItem itm = new ValuationDetailsValuationGridItem();
                    itm.Key = property.Valuations[x].Key;

                    double municipalValuation = property.Valuations[x].ValuationMunicipal.HasValue ? property.Valuations[x].ValuationMunicipal.Value : 0;
                    double valuationAmt = property.Valuations[x].ValuationAmount.HasValue ? property.Valuations[x].ValuationAmount.Value : 0;
                    double hocValuation = property.Valuations[x].ValuationHOCValue.HasValue ? property.Valuations[x].ValuationHOCValue.Value : 0;
                    double hocConventionalAmount = property.Valuations[x].HOCConventionalAmount.HasValue ? property.Valuations[x].HOCConventionalAmount.Value : 0;
                    double hocThatchAmount = property.Valuations[x].HOCThatchAmount.HasValue ? property.Valuations[x].HOCThatchAmount.Value : 0;

                    itm.HOCValuation = hocValuation.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.MunicipalValuation = municipalValuation.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.ValuationAmount = valuationAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.ValuationDateDisplay = property.Valuations[x].ValuationDate.ToString(SAHL.Common.Constants.DateFormat);
                    itm.ValuationDate = property.Valuations[x].ValuationDate;
                    itm.Valuator = property.Valuations[x].Valuator;
                    itm.ValuatorDisplayName = property.Valuations[x].Valuator.LegalEntity.DisplayName;
                    
                    if (property.Valuations[x].HOCRoof != null)
                        itm.HOCRoof = property.Valuations[x].HOCRoof;                   

                    itm.HOCConventionalValuation = hocConventionalAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.HOCThatchAmount = hocThatchAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.ChangedBy = String.IsNullOrEmpty(property.Valuations[x].ValuationUserID) ? "" : property.Valuations[x].ValuationUserID;
                    itm.IsActive = property.Valuations[x].IsActive ? "True" : "False";
                    _lstValuations.Add(itm);
                }
            }

            // sort by latest valuation first
            _lstValuations.Sort(delegate(ValuationDetailsValuationGridItem c1, ValuationDetailsValuationGridItem c2)
            { return c2.Key.CompareTo(c1.Key); });


            gridValuations.AutoGenerateColumns = false;
            gridValuations.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridValuations.AddGridBoundColumn("ValuatorDisplayName", "Valuer", Unit.Percentage(30), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("ValuationDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Valuation Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            gridValuations.AddGridBoundColumn("IsActive", "Active", Unit.Percentage(5), HorizontalAlign.Left, true);      
            gridValuations.AddGridBoundColumn("ValuationAmount", "Valuation Amount", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("HOCValuation", "HOC Valuation", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("MunicipalValuation", "Municipal Valuation", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("ChangedBy", "Changed By", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridValuations.DataSource = _lstValuations;

            if (_lstValuations.Count > 0 && presenter == "Update")
            {
                gridValuations.SelectedIndex = 0;
                _valKey = Convert.ToInt32(_lstValuations[0].Key);
            }

            if (presenter == "Add")
            {
                gridValuations.PostBackType = GridPostBackType.None;
                gridValuations.SelectedIndex = -1;
            }
            gridValuations.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IProperty> Properties
        {
            set { _properties = value; }
        }
        /// <summary>
        /// Set Presenter
        /// </summary>
        public string SetPresenter
        {
            set { presenter = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetSelectedValuationKey
        {
            get
            {
                if (gridValuations.Rows.Count > 0)
                    return Convert.ToInt32(gridValuations.SelectedRow.Cells[0].Text);

                return 0;
            }
        }

        #endregion


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (OnBackButtonClicked != null)
                OnBackButtonClicked(sender, e);
        }

        /// <summary>
        /// Get Updated Valuation
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public IValuation GetUpdatedValuation(IValuationDiscriminatedSAHLManual val)
        {
            IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            if (ddlValuer.SelectedValue != "-select-")
                val.Valuator = propRepo.GetValuatorByKey(Convert.ToInt32(ddlValuer.SelectedValue));

            if (ddlHOCRoofDescription.SelectedValue != "-select-")
                val.HOCRoof = _lookUps.HOCRoof.ObjectDictionary[ddlHOCRoofDescription.SelectedValue];

            val.ValuationAmount = Convert.ToDouble(txtValuationAmount.Amount);
            val.ValuationDate = dateValuationDate.Date.GetValueOrDefault();
            val.ValuationMunicipal = Convert.ToDouble(txtMunicipalValuation.Amount);

            double hocConventionalAmount = 0;
            double hocThatchAmount = 0;
            double hocShingleAmount = 0;

            if (ddlHOCRoofDescription.SelectedValue == ((int)HOCRoofs.Conventional).ToString() && txtHOCConventionalAmount.Text.Length > 0)
                hocConventionalAmount = Convert.ToDouble(txtHOCConventionalAmount.Amount);

            if (ddlHOCRoofDescription.SelectedValue == ((int)HOCRoofs.Thatch).ToString() && txtHOCThatchAmount.Text.Length > 0)
                hocThatchAmount = Convert.ToDouble(txtHOCThatchAmount.Amount);

            if (ddlHOCRoofDescription.SelectedValue == ((int)HOCRoofs.Partial).ToString())
            {
                if (txtHOCConventionalAmount.Text.Length > 0)
                    hocConventionalAmount = Convert.ToDouble(txtHOCConventionalAmount.Amount);
                if (txtHOCThatchAmount.Text.Length > 0)
                    hocThatchAmount = Convert.ToDouble(txtHOCThatchAmount.Amount);
            }

            val.HOCConventionalAmount = hocConventionalAmount;
            val.HOCThatchAmount = hocThatchAmount;
            val.HOCShingleAmount = hocShingleAmount;
            val.ValuationHOCValue = hocConventionalAmount + hocThatchAmount;

            val.ValuationUserID = CurrentPrincipal.Identity.Name;
            val.ValuationStatus = _lookUps.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Complete).ToString()];          
            
            return val;
        }

        /// <summary>
        /// Get HOCRoof
        /// </summary>
        public IHOCRoof GetHOCRoof
        {
            get
            {
                int selectedValue = ddlHOCRoofDescription.SelectedValue != "-select-" ? Convert.ToInt32(ddlHOCRoofDescription.SelectedValue) : -1;
                if (selectedValue == -1)
                    return null;
                else
                    return _lookUps.HOCRoof.ObjectDictionary[selectedValue.ToString()];
            }
        }

        /// <summary>
        /// Submit Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, new KeyChangedEventArgs(_valKey));
        }

        protected void ValuationDetailsDisplay_Click(object sender, EventArgs e)
        {
            if (OnValuationDetailsClicked != null)
                OnValuationDetailsClicked(sender, new KeyChangedEventArgs(_valKey));
        }

        protected void gridValuations_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindValuationControls();
        }

        private void BindValuationControls()
        {
            if (gridValuations.SelectedIndex >= 0) // if Add - you do not want to populate upadateable fields
            {
                if (_lstValuations.Count > 0)
                {
                    if (_showLabels) // display mode
                    {
                        for (int x = 0; x < _lstValuations.Count; x++)
                        {
                            if (_lstValuations[x].Key == Convert.ToInt32(gridValuations.SelectedRow.Cells[0].Text))
                            {
                                lblValuer.Text = _lstValuations[x].Valuator.LegalEntity.DisplayName;
                                lblValuationAmount.Text = _lstValuations[x].ValuationAmount;
                                lblValuationDate.Text = _lstValuations[x].ValuationDateDisplay;
                                lblMunicipalValuation.Text = _lstValuations[x].MunicipalValuation;
                                lblHOCRoofDescription.Text = _lstValuations[x].HOCRoof != null ? _lstValuations[x].HOCRoof.Description : "-";
                                lblHOCConventionalAmount.Text = !String.IsNullOrEmpty(_lstValuations[x].HOCConventionalValuation) ? _lstValuations[x].HOCConventionalValuation : "-";
                                lblHOCThatchAmount.Text = !String.IsNullOrEmpty(_lstValuations[x].HOCThatchAmount) ? _lstValuations[x].HOCThatchAmount : "-";
                                lblHOCValuationAmount.Text = !String.IsNullOrEmpty(_lstValuations[x].HOCValuation) ? _lstValuations[x].HOCValuation : "-"; 
                                break;
                            }
                        }
                    }
                    else // update mode
                    {
                        // always force bind to first row
                        int x = 0;
                        if (_lstValuations[x].HOCRoof != null)
                            ddlHOCRoofDescription.SelectedValue = _lstValuations[x].HOCRoof.Key.ToString();

                        SetAmountCaptureFields();

                        ddlValuer.SelectedValue = _lstValuations[x].Valuator.Key.ToString();

                        txtValuationAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].ValuationAmount));
                        dateValuationDate.Date = _lstValuations[x].ValuationDate;
                        txtMunicipalValuation.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].MunicipalValuation));
                        txtHOCConventionalAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].HOCConventionalValuation));
                        txtHOCThatchAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].HOCThatchAmount));
                        txtHOCValuationAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].HOCValuation));
                        lblHOCValuationAmount.Text = _lstValuations[x].HOCValuation;
                    }
                }
            }
        }

        void BindLookUps()
        {
            ddlHOCRoofDescription.DataSource = _lookUps.HOCRoof;
            ddlHOCRoofDescription.DataValueField = "key";
            ddlHOCRoofDescription.DataTextField = "description";
            ddlHOCRoofDescription.DataBind();

            // Remove Shingle from Drop Down
            li = ddlHOCRoofDescription.Items.FindByValue(((int)HOCRoofs.Shingle).ToString());
            if (li != null)
                ddlHOCRoofDescription.Items.Remove(li);

            // Get a list of valuators filtered by origination source
            List<IValuator> valuators = _propertyRepo.GetValuatorsByOriginationSource((int)OriginationSources.SAHomeLoans);

            // loop thru each valuator and add to valuator detail collection
            List<ValuatorDetail> valuatorLst = new List<ValuatorDetail>();
            foreach (IValuator val in valuators)
            {
                string desc = string.Format("{0}", val.LegalEntity.GetLegalName(LegalNameFormat.Full));
                valuatorLst.Add(new ValuatorDetail(val.Key, desc));
            }

            // sort the list of valuators by description
            valuatorLst.Sort(delegate(ValuatorDetail vd1, ValuatorDetail vd2) { return vd1.Description.CompareTo(vd2.Description); });

            // bind valuators dropdown
            ddlValuer.DataSource = valuatorLst;
            ddlValuer.DataValueField = "Key";
            ddlValuer.DataTextField = "Description";
            ddlValuer.DataBind();
        }

        private void SetAmountCaptureFields()
        {
            if (Convert.ToInt32(ddlHOCRoofDescription.SelectedValue) == (int)HOCRoofs.Conventional)
            {
                txtHOCConventionalAmount.Enabled = true;
                txtHOCThatchAmount.Enabled = false;
            }

            if (Convert.ToInt32(ddlHOCRoofDescription.SelectedValue) == (int)HOCRoofs.Thatch)
            {
                txtHOCConventionalAmount.Enabled = false;
                txtHOCThatchAmount.Enabled = true;
            }

            if (Convert.ToInt32(ddlHOCRoofDescription.SelectedValue) == (int)HOCRoofs.Partial)
            {
                txtHOCConventionalAmount.Enabled = true;
                txtHOCThatchAmount.Enabled = true;
            }
        }

        /// <summary>
        /// HOC Roof - change of selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlHOCRoofDescription_SelectedIndexChange(object sender, EventArgs e)
        {
            if (OnddlHOCRoofDescriptionIndexChanged != null)
            {
                int selectedValue = ddlHOCRoofDescription.SelectedValue == "-select-" ? -1 : Convert.ToInt32(ddlHOCRoofDescription.SelectedValue);
                OnddlHOCRoofDescriptionIndexChanged(Page, new KeyChangedEventArgs(selectedValue));
            }
        }

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

     }
    /// <summary>
    /// Grid Item - Valuation details
    /// </summary>
    public class ValuationDetailsValuationGridItem
    {
        private int _key;
        private IValuator _valuator;
        private string _valuatorDisplayName;
        private string _valuationDateDisplay;
        private DateTime _valuationDate;
        private string _valuationAmount;
        private string _hOCValuation;
        private string _municipalValuation;
        private string _changedBy;
        private IHOCRoof _hocRoof;
        private string _hocConventionalValuation;
        private string _hocThatchAmount;
        private string _isActive;

        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        public IValuator Valuator
        {
            get
            {
                return _valuator;
            }
            set
            {
                _valuator = value;
            }
        }

        public string ValuatorDisplayName
        {
            get
            {
                return _valuatorDisplayName;
            }
            set
            {
                _valuatorDisplayName = value;
            }
        }

        public DateTime ValuationDate
        {
            get
            {
                return _valuationDate;
            }
            set
            {
                _valuationDate = value;
            }
        }
        public string ValuationDateDisplay
        {
            get
            {
                return _valuationDateDisplay;
            }
            set
            {
                _valuationDateDisplay = value;
            }
        }
        public string ValuationAmount
        {
            get
            {
                return _valuationAmount;
            }
            set
            {
                _valuationAmount = value;
            }
        }
        public string HOCValuation
        {
            get
            {
                return _hOCValuation;
            }
            set
            {
                _hOCValuation = value;
            }
        }
        public string MunicipalValuation
        {
            get
            {
                return _municipalValuation;
            }
            set
            {
                _municipalValuation = value;
            }
        }
        public string ChangedBy
        {
            get
            {
                return _changedBy;
            }
            set
            {
                _changedBy = value;
            }
        }

        public IHOCRoof HOCRoof
        {
            get
            {
                return _hocRoof;
            }
            set
            {
                _hocRoof = value;
            }
        }

        public string HOCConventionalValuation
        {
            get
            {
                return _hocConventionalValuation;
            }
            set
            {
                _hocConventionalValuation = value;
            }
        }

        public string HOCThatchAmount
        {
            get
            {
                return _hocThatchAmount;
            }
            set
            {
                _hocThatchAmount = value;
            }
        }
        public string IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }


    }

    public class ValuatorDetail
    {
        int _key;
        string _description;

        public ValuatorDetail(int key, string desc)
        {
            _key = key;
            _description = desc;
        }

        public int Key
        {
            get { return _key; }
        }

        public string Description
        {
            get { return _description; }
        }
    }
}
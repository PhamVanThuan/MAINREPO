using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;
 
using SAHL.Common.Web.UI.Controls;
using System.Security.Principal;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.BusinessModel.Repositories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.Utilities.Sorting;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationDetails : SAHLCommonBaseView,IValuationDetails
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

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            _lookUps = RepositoryFactory.GetRepository<ILookupRepository>();

            BindPropertyGrid();
            if (_properties.Count > 0)
            {
                    BindValuationsGrid(_properties[0]);
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
        /// <see cref="IValuationDetails.OnCancelButtonClicked"/>
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// <see cref="IValuationDetails.OnSubmitButtonClicked"/>
        /// </summary>
        public event KeyChangedEventHandler OnSubmitButtonClicked;

        public event KeyChangedEventHandler OnddlHOCRoofDescriptionIndexChanged;

        public event KeyChangedEventHandler OnValuationDetailsClicked;

        public event EventHandler OnBackButtonClicked;

        public void HideAllPanels()
        {
            DisplayData.Visible = false;
            ButtonRow.Visible = false;
        }
        /// <summary>
        /// <see cref="IValuationDetails.ShowButtons"/>
        /// </summary>
        public bool ShowButtons
        {
           set{ _showButtons = value; }
        }

        public bool ShowNavigationButtons
        {
            set { _showNavigationButtons = value; }
        }

        /// <summary>
        /// <see cref="IValuationDetails.ShowLabels"/>
        /// </summary>
        public bool ShowLabels
        {
           set{_showLabels = value ; }
        }
        /// <summary>
        /// <see cref="IValuationDetails.SetHOCThatchAmountForUpdate"/>
        /// </summary>
        public bool SetHOCThatchAmountForUpdate
        {
            set
            {
                txtHOCThatchAmount.Enabled = value;
            }
        }
        /// <summary>
        /// <see cref="IValuationDetails.SetHOCConventionalAmountForUpdate"/>
        /// </summary>
        public bool SetHOCConventionalAmountForUpdate
        {
            set
            {
                txtHOCConventionalAmount.Enabled = value;     
            }
        }

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
            List<ValuationDetailsPropertyGridRowItem> lstItems = new List<ValuationDetailsPropertyGridRowItem>();

            if (_properties != null)
            {
                for (int x = 0; x < _properties.Count; x++)
                {
                    ValuationDetailsPropertyGridRowItem itm = new ValuationDetailsPropertyGridRowItem();
                    itm.Property = _properties[x];
                    IAddressStreet addressStreet = _properties[x].Address as IAddressStreet;
                    if (addressStreet != null)
                    {
                        itm.Building = addressStreet.BuildingNumber + " " + addressStreet.BuildingName;
                        itm.Street = addressStreet.StreetNumber + " " + addressStreet.StreetName;
                    }
                    else
                    {
                        itm.Building = _properties[x].SectionalSchemeName;
                        itm.Street = " ";
                    }

                    itm.City = _properties[x].Address.RRR_CityDescription;
                    itm.Province = _properties[x].Address.RRR_ProvinceDescription;
                    itm.Suburb = _properties[x].Address.RRR_SuburbDescription;
                    itm.Unit = _properties[x].SectionalUnitNumber;
                    lstItems.Add(itm);
                }
            }
            gridProperty.AutoGenerateColumns = false;
            gridProperty.AddGridBoundColumn("Property", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridProperty.AddGridBoundColumn("Unit", "Unit", Unit.Percentage(9), HorizontalAlign.Left, true);
            gridProperty.AddGridBoundColumn("Building", "Building", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridProperty.AddGridBoundColumn("Street", "Street", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridProperty.AddGridBoundColumn("Suburb", "Suburb", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridProperty.AddGridBoundColumn("City", "City", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridProperty.AddGridBoundColumn("Province", "Province", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridProperty.DataSource = lstItems;
            gridProperty.DataBind();      
        }

        private void BindValuationsGrid(IProperty property)
        {
            _lstValuations = new List<ValuationDetailsValuationGridItem>();
            
            for (int x = 0; x < property.Valuations.Count; x++)
            {
                if (property.Valuations[x] is IValuationDiscriminatedSAHLManual)
                {
                    ValuationDetailsValuationGridItem itm = new ValuationDetailsValuationGridItem();
                    itm.Key = property.Valuations[x].Key.ToString();

                    itm.HOCValuation = property.Valuations[x].ValuationHOCValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.MunicipalValuation = double.Parse(property.Valuations[x].ValuationMunicipal.ToString()).ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.ValuationAmount = property.Valuations[x].ValuationAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.ValuationDateDisplay = property.Valuations[x].ValuationDate.ToString(SAHL.Common.Constants.DateFormat);
                    itm.ValuationDate = property.Valuations[x].ValuationDate;
                    itm.Valuator = property.Valuations[x].Valuator;
                    itm.ValuatorDisplayName = property.Valuations[x].Valuator.LegalEntity.DisplayName;

                    if (property.Valuations[x].HOCRoof != null)
                        itm.HOCRoof = property.Valuations[x].HOCRoof;
                    else
                        if (property.Valuations[x].Property.HOC != null && property.Valuations[x].Property.HOC.HOCRoof != null)
                            itm.HOCRoof = property.Valuations[x].Property.HOC.HOCRoof;

                    itm.HOCConventionalValuation = property.Valuations[x].HOCConventionalAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    itm.HOCThatchAmount = property.Valuations[x].HOCThatchAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    _lstValuations.Add(itm);
                }     
            }

            _lstValuations.Sort(delegate(ValuationDetailsValuationGridItem c1, ValuationDetailsValuationGridItem c2)
            { return c2.ValuationDate.CompareTo(c1.ValuationDate); });

            gridValuations.AutoGenerateColumns = false;
            gridValuations.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridValuations.AddGridBoundColumn("ValuatorDisplayName", "Valuer", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("ValuationDate", "Valuation Date", Unit.Percentage(23), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("ValuationAmount", "Valuation Amount", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("HOCValuation", "HOC Valuation", Unit.Percentage(13), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("MunicipalValuation", "Municipal Valuation", Unit.Percentage(13), HorizontalAlign.Left, true);
            gridValuations.AddGridBoundColumn("", "Changed By", Unit.Percentage(23), HorizontalAlign.Left, true);
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
        /// <see cref="IValuationDetails.Properties"/> 
        /// </summary>
        public IEventList<SAHL.Common.BusinessModel.Interfaces.IProperty> Properties
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

        public int GetSelectedValuationKey
        {
            get
            {
                if (gridValuations.Rows.Count > 0)
                    return Convert.ToInt32(gridValuations.SelectedRow.Cells[0].Text);
                else
                    return 0;
            }
        }
        #endregion
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
            if (ddlValuer.SelectedValue != "-select-")
                val.Valuator = _lookUps.Valuators.ObjectDictionary[ddlValuer.SelectedValue];
            
            val.ValuationAmount = Convert.ToDouble(txtValuationAmount.Amount);
            val.ValuationDate = dateValuationDate.Date;
            val.ValuationMunicipal = Convert.ToDouble(txtMunicipalValuation.Amount);
            
            if (ddlHOCRoofDescription.SelectedValue == ((int)HOCRoofs.Conventional).ToString())
                val.HOCConventionalAmount = Convert.ToDouble(txtHOCConventionalAmount.Amount);
            if (ddlHOCRoofDescription.SelectedValue == ((int)HOCRoofs.Thatch).ToString())
                val.HOCThatchAmount = Convert.ToDouble(txtHOCThatchAmount.Amount);
            if (ddlHOCRoofDescription.SelectedValue == ((int)HOCRoofs.Partial).ToString())
               {
                   val.HOCConventionalAmount = Convert.ToDouble(txtHOCConventionalAmount.Amount);
                   val.HOCThatchAmount = Convert.ToDouble(txtHOCThatchAmount.Amount);
               }
            val.ValuationUserID = this.CurrentPrincipal.Identity.Name;
            val.ValuationStatus = _lookUps.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Complete).ToString()];
            
            double hocConventionalAmount = 0;
            double hocThatchAmount = 0;

            if (txtHOCConventionalAmount.Enabled)
                hocConventionalAmount = Convert.ToDouble(txtHOCConventionalAmount.Amount);
            if (txtHOCThatchAmount.Enabled)
                hocThatchAmount = Convert.ToDouble(txtHOCThatchAmount.Amount);
            val.ValuationHOCValue = hocConventionalAmount + hocThatchAmount;
            if (ddlHOCRoofDescription.SelectedValue != "-select-")
                val.HOCRoof = _lookUps.HOCRoof.ObjectDictionary[ddlHOCRoofDescription.SelectedValue];
            return val;
        }
       
        /// <summary>
        /// Get HOCRoof
        /// </summary>
        public IHOCRoof GetHOCRoof
        {
            get
            {
                if (ddlHOCRoofDescription.SelectedValue != "-select-")
                    return _lookUps.HOCRoof.ObjectDictionary[ddlHOCRoofDescription.SelectedValue];
                else
                    return _lookUps.HOCRoof.ObjectDictionary[((int)HOCRoofs.Conventional).ToString()]; // Default since all code paths must return a value
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

        /// <summary>
        /// Bind Valuation Controls
        /// </summary>
        private void BindValuationControls()
        {
            if (gridValuations.SelectedIndex != -1) // if Add - you do not want to populate upadateable fields
            {
                for (int x = 0; x < _lstValuations.Count; x++)
                {
                    if (_lstValuations[x].Key == gridValuations.SelectedRow.Cells[0].Text)
                    {
                        if (_showLabels)
                        {
                            lblValuer.Text = _lstValuations[x].Valuator.LegalEntity.DisplayName;
                            lblValuationAmount.Text = _lstValuations[x].ValuationAmount;
                            lblValuationDate.Text = _lstValuations[x].ValuationDateDisplay;
                            lblMunicipalValuation.Text = _lstValuations[x].MunicipalValuation;
                            lblHOCRoofDescription.Text = _lstValuations[x].HOCRoof.Description;
                            lblHOCConventionalAmount.Text = _lstValuations[x].HOCConventionalValuation;
                            lblHOCThatchAmount.Text = _lstValuations[x].HOCThatchAmount;
                            lblHOCValuationAmount.Text = _lstValuations[x].HOCValuation;
                        }
                        else
                        {
                            // set update control values
                            BindLookUps();

                            for (int y = 0; y < ddlHOCRoofDescription.Items.Count; y++)
                            {
                                if (ddlHOCRoofDescription.Items[y].Value == _lstValuations[x].HOCRoof.Key.ToString())
                                {
                                    ddlHOCRoofDescription.SelectedIndex = y;
                                    SetAmountCaptureFields();
                                    break;
                                }
                            }

                            for (int y = 0; y < ddlValuer.Items.Count; y++)
                            {
                                if (ddlValuer.Items[y].Value == _lstValuations[x].Valuator.Key.ToString())
                                {
                                    ddlValuer.SelectedIndex = y;
                                    break;
                                }
                            }

                            txtValuationAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].ValuationAmount.ToString()));
                            dateValuationDate.Date = _lstValuations[x].ValuationDate;
                            txtMunicipalValuation.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].MunicipalValuation));
                            txtHOCConventionalAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].HOCConventionalValuation));
                            txtHOCThatchAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].HOCThatchAmount));
                            txtHOCValuationAmount.Amount = Convert.ToDouble(GetNumeric(_lstValuations[x].HOCValuation));
                            lblHOCValuationAmount.Text = _lstValuations[x].HOCValuation;
                        }
                        break;
                    }

                }
            }
            else
            {
                BindLookUps();
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

            // Filter Valuator by OS
            IEventList<IValuator> valuators = new EventList<IValuator>();
            for (int i = 0; i < _lookUps.Valuators.Count; i++)
            {
                foreach (IOriginationSource os in _lookUps.Valuators[i].OriginationSources)
                {
                    if (os.Key == (int)OriginationSources.SAHomeLoans)
                        valuators.Add(this.Messages, _lookUps.Valuators[i]);
                }
            }

            ddlValuer.DataSource = valuators;
            ddlValuer.DataValueField = "Key";
            ddlValuer.DataTextField = "";
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
                OnddlHOCRoofDescriptionIndexChanged(this.Page,new KeyChangedEventArgs(ddlHOCRoofDescription.SelectedValue));
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

        /// <summary>
        /// Change of Valuer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlValuer_SelectedIndexChanged(object sender, EventArgs e)
        {
                  
        }
        /// <summary>
        /// Valuer DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlValuer_DataBound(object sender, EventArgs e)
        {
            IPropertyRepository pr = RepositoryFactory.GetRepository<IPropertyRepository>();
            for (int x = 0; x < ddlValuer.Items.Count; x++)
            {
                IValuator v = pr.GetValuatorByKey(int.Parse(ddlValuer.Items[x].Value));
                ddlValuer.Items[x].Text = v.LegalEntity.DisplayName;
            }
        }       
    }
    /// <summary>
    /// Valuation Details item - used by Grid
    /// </summary>
    public class ValuationDetailsPropertyGridRowItem
    {
        private IProperty _property;
        private string _unit;
        private string _building;
        private string _street;
        private string _suburb;
        private string _city;
        private string _province;

        public IProperty Property
        {
            get
            {
                return _property;
            }
            set
            {
                _property = value;
            }
        }

        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }

        public string Building
        {
            get
            {
                return _building;
            }
            set
            {
                _building = value;
            }
        }

        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
            }
        }

        public string Suburb
        {
            get
            {
                return _suburb;
            }
            set
            {
                _suburb = value;
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        public string Province
        {
            get
            {
                return _province;
            }
            set
            {
                _province = value;
            }
        }
    }


    /// <summary>
    /// Grid Item - Valuation details
    /// </summary>
    public class ValuationDetailsValuationGridItem
    {
        private string _key;
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

        public string Key
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

        
    }


}
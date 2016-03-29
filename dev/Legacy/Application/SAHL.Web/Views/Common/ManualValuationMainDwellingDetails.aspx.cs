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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.Common
{
    public partial class ManualValuationMainDwellingDetails : SAHLCommonBaseView,IManualValuationMainDwellingDetails
    {
        private IEventList<IProperty> _properties;
        private IValuationDiscriminatedSAHLManual _valManual;
        bool _showImprovementGrid;
        ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            txtMainBuildingExtent.Attributes["onkeyup"] = "calculateMainbuildingReplacementValue()" + txtMainBuildingExtent.Attributes["onkeyup"];
            txtMainBuildingRate.Attributes["onkeyup"] = "calculateMainbuildingReplacementValue()" + txtMainBuildingRate.Attributes["onkeyup"];

            txtCottageExtent.Attributes["onkeyup"] = "calculateCottageReplacementValue()" + txtCottageExtent.Attributes["onkeyup"];
            txtCottageRate.Attributes["onkeyup"] = "calculateCottageReplacementValue()" + txtCottageRate.Attributes["onkeyup"];

            txtCombinedOutbuildingsExtent.Attributes["onkeyup"] = "calculateOutbuildingReplacementValue()" + txtCombinedOutbuildingsExtent.Attributes["onkeyup"];
            txtCombinedOutbuildingsRate.Attributes["onkeyup"] = "calculateOutbuildingReplacementValue()" + txtCombinedOutbuildingsRate.Attributes["onkeyup"];

            BindPropertyGrid();
            ImprovementsGrid.Visible = _showImprovementGrid;
        }

        /// <summary>
        /// <see cref="IValuationDetails.Properties"/> 
        /// </summary>
        public IEventList<SAHL.Common.BusinessModel.Interfaces.IProperty> Properties
        {
            set { _properties = value; }
        }

        public IValuationDiscriminatedSAHLManual ValuationSAHLManual
        {
            set { _valManual = value; }
        }

        public void ShowPanelsForAdd()
        {
            MainBuildingPanel.Visible = true;
            CottagePanel.Visible = true;
            ValuationEscalation.Visible = true;
            tblValuation.Visible = true;
            ValuationConventionalValue.Visible = false;
            ValuationThatchValue.Visible = false;
            ValuationTotalValue.Visible = false;

            lblMainBuildingClassification.Visible = false;
            lblMainBuildingRoofType.Visible = false;
            lblMainBuildingExtent.Visible = false;
            lblMainBuildingRate.Visible = false;
            lblMainBuildingReplacementValue.Visible = false;

            lblCottageRoofType.Visible = false;
            lblCottageReplacementValue.Visible = false;
            lblCottageExtent.Visible = false;
            lblCottageRate.Visible = false;
                
            lblEscalation.Visible = false;
            lblCombinedThatchValue.Visible = false;

            AddButton.Visible = false;
            AddNewButton.Visible = false;
            BackButton.Visible = false;
        }

        public void ShowPanelsForAddExtended()
        {
            BuildingType.Visible = true;

            CancelButton.Visible = true;
            BackButton.Visible = true;
            AddNewButton.Visible = true;
            SubmitButton.Visible = true;
            AddButton.Visible = true;
        }

        public void ShowPanelForOutBuildingAddUpdate()
        {
            CombinedOutbuildingsPanel.Visible = true;
            lblOutBuildingRoofType.Visible = false;
            lblCombinedOutbuildingsExtent.Visible = false;
            lblCombinedOutbuildingsRate.Visible = false;
            lblCombinedOutbuildingsReplaceValue.Visible = false;
        }

        public void ShowPanelForImprovementAddUpdate()
        {
            pnlImprovement.Visible = true;
            lblImprovementType.Visible = false;
            lblImprovementDate.Visible = false;
        }

        protected void ddlBuildingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBuildingTypeOnSelectedIndexChanged != null)
                ddlBuildingTypeOnSelectedIndexChanged(sender, new KeyChangedEventArgs (ddlBuildingType.SelectedValue));
        }

        public bool ShowImprovementGrid 
        {
            set { _showImprovementGrid = value; }
        }

        public void BindOutBuildingDataFromGrid(int selectedGridItem)
        {
            ImprovementsGrid.SelectedIndex = selectedGridItem;

            ddlBuildingType.SelectedValue = "1";
            ddlBuildingType.Enabled = false;

            ddlCombinedOutbuildingsRoofType.SelectedValue = ImprovementsGrid.SelectedRow.Cells[8].Text;
            txtCombinedOutbuildingsExtent.Text = ImprovementsGrid.SelectedRow.Cells[2].Text;
            txtCombinedOutbuildingsRate.Text = ImprovementsGrid.SelectedRow.Cells[3].Text;
            txtCombinedOutbuildingsReplaceValue.Text = ImprovementsGrid.SelectedRow.Cells[4].Text;

            AddButton.Text = "Update";
            txtHiddenBox.Text = "Update";
        }

        public string GetSelectedFirstItem
        {
            get 
            {
            string selectedGridRowItem;

            if (ImprovementsGrid.SelectedRow.Cells[1].Text == "Outbuilding")
                selectedGridRowItem = "1" + "," + ImprovementsGrid.SelectedIndex.ToString();
            else
                selectedGridRowItem = "2" + "," + ImprovementsGrid.SelectedIndex.ToString();

            return selectedGridRowItem;
            }
        }

        public void BindImprovementDataFromGrid(int selectedGridItem)
        {
            ImprovementsGrid.SelectedIndex = selectedGridItem;

            ddlBuildingType.SelectedValue = "2";
            ddlBuildingType.Enabled = false;

            ddlImprovementType.SelectedValue = ImprovementsGrid.SelectedRow.Cells[9].Text;
            txtImprovementReplacementValue.Text = Convert.ToDouble(ImprovementsGrid.SelectedRow.Cells[4].Text).ToString();
            if (ImprovementsGrid.SelectedRow.Cells[7].Text.Length > 0)
            {
                DateTime date = Convert.ToDateTime(ImprovementsGrid.SelectedRow.Cells[10].Text);
                dteImprovementDate.Date = Convert.ToDateTime(date);
            }
            AddButton.Text = "Update";
            txtHiddenBox.Text = "Update";
        }

        public void ShowUpdateButton()
        {
            AddButton.Visible = true;
            AddButton.Text = "Update";

            SubmitButton.Visible = false;
            BackButton.Visible = true;
        }


        public void ShowBackButton()
        {
            AddButton.Visible = false;
            SubmitButton.Visible = true;
            BackButton.Visible = true;
        }

        protected void ImprovementsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
         string selectedGridRow;

         if (ImprovementsGrid.SelectedRow.Cells[1].Text == "Outbuilding")
            selectedGridRow = "1" + "," + ImprovementsGrid.SelectedIndex.ToString();
         else
            selectedGridRow = "2" + "," + ImprovementsGrid.SelectedIndex.ToString();

        if (OnImprovementsGridSelectedIndexChanged != null)
            OnImprovementsGridSelectedIndexChanged(sender, new KeyChangedEventArgs(selectedGridRow) );
        }

        public void ShowReadOnlyFieldsForDisplay()
        {
            ddlMainBuildingClassification.Visible = false;
            ddlMainBuildingRoofType.Visible = false;
            txtMainBuildingExtent.Visible = false;
            txtMainBuildingRate.Visible = false;
            txtMainBuildingReplaceValue.Visible = false;

            ddlCottageRoofType.Visible = false;
            txtCottageExtent.Visible = false;
            txtCottageRate.Visible = false;
            txtCottageReplaceValue.Visible = false;
           
            ddlCombinedOutbuildingsRoofType.Visible = false;
            txtCombinedOutbuildingsExtent.Visible = false;
            txtCombinedOutbuildingsRate.Visible = false;
            txtCombinedOutbuildingsReplaceValue.Visible = false;
            
            txtCombinedImprovementsReplaceValue.Visible = false;
            ddlCombinedImprovementType.Visible = false;
            txtCombinedThatchValue.Visible = false;

            BuildingType.Visible = false;

            CombinedImprovementType.Visible = false;
            CombinedOutbuildingRoofType.Visible = false;

            BackButton.Visible = false;
            AddButton.Visible = false;
            AddNewButton.Visible = false;

            pnlImprovement.Visible = false;
        }

        public void BindValuationDisplay(IValuationDiscriminatedSAHLManual val)
        {
            lblMainBuildingClassification.Text = val.ValuationClassification == null ? "-" : val.ValuationClassification.Description;

            if (val.ValuationMainBuilding != null && val.ValuationMainBuilding.Extent != null && val.ValuationMainBuilding.Extent > 0)
            {
                    lblMainBuildingRoofType.Text = val.ValuationMainBuilding.ValuationRoofType.Description;
                    lblMainBuildingExtent.Text = Convert.ToDouble(val.ValuationMainBuilding.Extent).ToString();
                    lblMainBuildingRate.Text = val.ValuationMainBuilding.Rate.ToString();
                    lblMainBuildingReplacementValue.Text = Convert.ToDouble((val.ValuationMainBuilding.Extent * val.ValuationMainBuilding.Rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            if (val.ValuationCottage != null  && val.ValuationCottage.Extent != null && val.ValuationCottage.Extent > 0)
            {
                lblCottageRoofType.Text = val.ValuationCottage.ValuationRoofType == null ? "-" : val.ValuationCottage.ValuationRoofType.Description;
                lblCottageExtent.Text = val.ValuationCottage == null ? "-" : val.ValuationCottage.Extent.ToString();
                lblCottageRate.Text = val.ValuationCottage == null ? "-" : val.ValuationCottage.Rate.ToString();
                lblCottageReplacementValue.Text = Convert.ToDouble((val.ValuationCottage.Extent * val.ValuationCottage.Rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            double OutBuildingReplacementValue = 0;
            double OutBuildingExtent = 0;
            double OutBuildRate = 0;

            if (val.ValuationOutBuildings != null && val.ValuationOutBuildings.Count > 0)
            {
                for (int i = 0; i < val.ValuationOutBuildings.Count; i++)
                {
                    OutBuildingReplacementValue += Convert.ToDouble(val.ValuationOutBuildings[i].Rate * val.ValuationOutBuildings[i].Extent);
                    OutBuildingExtent += Convert.ToDouble(val.ValuationOutBuildings[i].Extent);
                    OutBuildRate = Convert.ToDouble(val.ValuationOutBuildings[i].Rate);
                }
                OutBuildRate = OutBuildRate / val.ValuationOutBuildings.Count;
            }

            lblCombinedOutbuildingsReplaceValue.Text = OutBuildingReplacementValue == 0 ? "-" : OutBuildingReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCombinedOutbuildingsRate.Text = OutBuildRate == 0 ? "-" : OutBuildRate.ToString();
            lblCombinedOutbuildingsExtent.Text = OutBuildingExtent == 0 ? "-" : OutBuildingExtent.ToString();

            double ImprovementReplacementValue = 0;

            for (int x = 0; x < val.ValuationImprovements.Count; x++)
            {
                ImprovementReplacementValue += Convert.ToDouble(val.ValuationImprovements[x].ImprovementValue);
            }

            lblCombinedImprovementsReplaceValue.Text = ImprovementReplacementValue == 0 ? "-" : ImprovementReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            TrImprovementDate.Visible = false;
            TrImprovementExt.Visible = false;
            TrImprovementRate.Visible = false;

            txtEscalation.Visible = false;
            lblEscalation.Text = val.ValuationEscalationPercentage.ToString() + " %";

            if (val.ValuationCombinedThatch != null)
            {
                lblCombinedThatchValue.Text = val.ValuationCombinedThatch.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                double ThatchValuePercentageCheck = CalculateThatchValuePercentageCheck(val);
                lblCombinedThatchValuePercCheck.Text = ThatchValuePercentageCheck.ToString();
                double ThatchValuePercentageExtent = CalculateThatchExtentCheck(val);
                lblCombinedThatchPercExtCheck.Text = ThatchValuePercentageExtent.ToString();
            }

            double TotalConventionalValue = CalculateTotalConventionalValue(val);
            lblTotalConventionalValue.Text = TotalConventionalValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            double TotalThatchValue = CalculateTotalThatchValue(val);
            lblTotalThatchValue.Text = TotalThatchValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            double TotalValue = TotalConventionalValue + TotalThatchValue;
            lblTotalValue.Text = TotalValue.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        static double CalculateConventionalValue(IValuationDiscriminatedSAHLManual valManual)
        {
            double totalConventionalValue = 0;
            double mainBuildingValue = 0;
            
            if (valManual.ValuationMainBuilding != null)
                mainBuildingValue = (Convert.ToDouble(valManual.ValuationMainBuilding.Rate) * Convert.ToDouble(valManual.ValuationMainBuilding.Extent));
            
            double cottageValue = 0;
            if (valManual.ValuationCottage != null)
              cottageValue =  Convert.ToDouble(valManual.ValuationCottage.Rate) * Convert.ToDouble(valManual.ValuationCottage.Extent);

            double outBuildingValue = 0;
            double improvementValue = 0;
            double totalThatchValue = CalculateTotalThatchValue(valManual);

            if (valManual.ValuationOutBuildings != null && valManual.ValuationOutBuildings.Count > 0)
            {
                for (int i = 0; i < valManual.ValuationOutBuildings.Count; i++)
                {
                    outBuildingValue += (Convert.ToDouble(valManual.ValuationOutBuildings[i].Rate) * Convert.ToDouble(valManual.ValuationOutBuildings[i].Extent));
                }
            }

            if (valManual.ValuationImprovements != null && valManual.ValuationImprovements.Count > 0)
            {
                for (int y = 0; y < valManual.ValuationImprovements.Count; y++)
                {
                    improvementValue += (Convert.ToDouble(valManual.ValuationImprovements[y].ImprovementValue));
                }
            }
            double sum = mainBuildingValue + cottageValue + outBuildingValue + improvementValue;
            
            if (sum > 0)
                totalConventionalValue = (sum - totalThatchValue);
            else
                totalConventionalValue = sum;

            return totalConventionalValue;
        }

        static double CalculateTotalThatchValue(IValuationDiscriminatedSAHLManual valManual)
        {
            double thatchValue = 0;
            double combinedThatchValue = 0;

            if (valManual.ValuationCombinedThatch != null)
                combinedThatchValue = valManual.ValuationCombinedThatch.Value;

            thatchValue = Convert.ToDouble(combinedThatchValue * (1 + (valManual.ValuationEscalationPercentage/100)));
            return thatchValue;
        }

        static double CalculateTotalConventionalValue(IValuationDiscriminatedSAHLManual valManual)
        {
                double conventionalValue = CalculateConventionalValue(valManual);
                double totalConventionalValue = Convert.ToDouble(conventionalValue * (1 + (valManual.ValuationEscalationPercentage/100)));
                return totalConventionalValue;
        }

        static double CalculateThatchValuePercentageCheck(IValuationDiscriminatedSAHLManual valManual)
        {
            double combinedThatchValue = CalculateTotalThatchValue(valManual);
            double mainBuildingReplaceValue = 0;
            
            if (valManual.ValuationMainBuilding != null)
                mainBuildingReplaceValue = Convert.ToDouble(valManual.ValuationMainBuilding.Rate * valManual.ValuationMainBuilding.Extent);
            double cottageReplaceValue = 0.0;
            
            if (valManual.ValuationCottage != null)
               cottageReplaceValue =  Convert.ToDouble(valManual.ValuationCottage.Rate * valManual.ValuationCottage.Extent);

            double outbuildingReplaceValue = 0;
            double improvementReplaceValue = 0;

            if (valManual.ValuationOutBuildings != null && valManual.ValuationOutBuildings.Count > 0)
            {
                for (int i = 0; i < valManual.ValuationOutBuildings.Count; i++)
                {
                    outbuildingReplaceValue += (Convert.ToDouble(valManual.ValuationOutBuildings[i].Rate) * Convert.ToDouble(valManual.ValuationOutBuildings[i].Extent));
                }
            }

            if (valManual.ValuationImprovements != null && valManual.ValuationImprovements.Count > 0)
            {
                for (int y = 0; y < valManual.ValuationImprovements.Count; y++)
                {
                    improvementReplaceValue += (Convert.ToDouble(valManual.ValuationImprovements[y].ImprovementValue));
                }
            }

            double totalReplaceValue = mainBuildingReplaceValue + cottageReplaceValue + outbuildingReplaceValue + improvementReplaceValue;
            double thatchValuePercentageCheck = combinedThatchValue / totalReplaceValue;
            return thatchValuePercentageCheck;
        }

        static double CalculateThatchExtentCheck(IValuationDiscriminatedSAHLManual valManual)
        {
            double cottageExtent = 0.0;

            if (valManual.ValuationCottage != null)
                cottageExtent = Convert.ToDouble(valManual.ValuationCottage.Extent);

            double extent = Convert.ToDouble(valManual.ValuationMainBuilding.Extent + cottageExtent);

            if (valManual.ValuationOutBuildings != null && valManual.ValuationOutBuildings.Count > 0)
            {
                for (int i = 0; i < valManual.ValuationOutBuildings.Count; i++)
                {
                    extent += Convert.ToDouble(valManual.ValuationOutBuildings[i].Extent);
                }
            }

            double thatchValuePercentage = CalculateThatchValuePercentageCheck(valManual);
            double thatchExtentCheck = extent * thatchValuePercentage;
            return thatchExtentCheck;
        }

        public void BindUpdateableFields(IValuationDiscriminatedSAHLManual val)
        {
            
            if (val.ValuationClassification != null)
                ddlMainBuildingClassification.SelectedValue = val.ValuationClassification.Key.ToString();
           
            if (val.ValuationMainBuilding != null)
            {
                ddlMainBuildingRoofType.SelectedValue = val.ValuationMainBuilding.ValuationRoofType.Key.ToString();
                txtMainBuildingExtent.Text = val.ValuationMainBuilding.Extent.ToString();
                txtMainBuildingRate.Text = val.ValuationMainBuilding.Rate.ToString();
                txtMainBuildingReplaceValue.Text = Convert.ToDouble((val.ValuationMainBuilding.Extent * val.ValuationMainBuilding.Rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            if (val.ValuationCottage != null)
            {
                if (val.ValuationCottage.ValuationRoofType != null)
                    ddlCottageRoofType.SelectedValue = val.ValuationCottage.ValuationRoofType.Key.ToString();
                txtCottageExtent.Text = val.ValuationCottage.Extent.ToString();
                txtCottageRate.Text = val.ValuationCottage.Rate.ToString();
                txtCottageReplaceValue.Text = Convert.ToDouble((val.ValuationCottage.Extent * val.ValuationCottage.Rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            txtEscalation.Text = val.ValuationEscalationPercentage.ToString();
        }
     
        public void ShowValuationOutBuildingDisplayDetails(IValuationOutbuilding valOutBuilding)
        {
            CombinedOutbuildingsPanel.Visible = true;
            BuildingType.Visible = true;

            txtCombinedOutbuildingsRate.Visible = false;
            txtCombinedOutbuildingsExtent.Visible = false;

            ddlBuildingType.SelectedValue = "1";
            ddlBuildingType.Enabled = false;
            lblOutBuildingRoofType.Text = valOutBuilding.ValuationRoofType.Description;
            ddlCombinedOutbuildingsRoofType.Visible = false;

            lblCombinedOutbuildingsExtent.Text = valOutBuilding.Extent.ToString();
            lblCombinedOutbuildingsRate.Text = valOutBuilding.Rate.ToString();
            txtCombinedOutbuildingsReplaceValue.Text = Convert.ToDouble((valOutBuilding.Extent * valOutBuilding.Rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCombinedOutbuildingsReplaceValue.Visible = false;
       }

        public void ShowValuationImprovementDisplayDetails(IValuationImprovement valImprovements)
        {
            pnlImprovement.Visible = true;
            BuildingType.Visible = true;

            ddlBuildingType.SelectedValue = "2";
            ddlBuildingType.Enabled = false;

            ddlImprovementType.Visible = false;
            dteImprovementDate.Visible = false;

            lblImprovementDate.Text = Convert.ToDateTime(valImprovements.ImprovementDate).ToString(SAHL.Common.Constants.DateFormat);
            lblImprovementType.Text = valImprovements.ValuationImprovementType.Description;
            txtImprovementReplacementValue.Text = Convert.ToDouble(valImprovements.ImprovementValue).ToString(SAHL.Common.Constants.CurrencyFormat);
            txtImprovementReplacementValue.Enabled = false;
        }

        public void HideAllPanels()
        {
            MainBuildingPanel.Visible = false;
            CottagePanel.Visible = false;
            CombinedThatchPanel.Visible = false;
            tblValuation.Visible = false;
            CombinedImprovementsPanel.Visible = false;
            pnlImprovement.Visible = false;
            CombinedOutbuildingsPanel.Visible = false;
            BuildingType.Visible = false;
        }

        private void BindPropertyGrid()
        {
            PropertyGrid.AutoGenerateColumns = false;
            PropertyGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            PropertyGrid.AddGridBoundColumn("", "Unit", Unit.Percentage(9), HorizontalAlign.Left, true);
            PropertyGrid.AddGridBoundColumn("", "Building", Unit.Percentage(18), HorizontalAlign.Left, true);
            PropertyGrid.AddGridBoundColumn("", "Street", Unit.Percentage(18), HorizontalAlign.Left, true);
            PropertyGrid.AddGridBoundColumn("", "Suburb", Unit.Percentage(18), HorizontalAlign.Left, true);
            PropertyGrid.AddGridBoundColumn("", "City", Unit.Percentage(18), HorizontalAlign.Left, true);
            PropertyGrid.AddGridBoundColumn("", "Province", Unit.Percentage(10), HorizontalAlign.Left, true);
            PropertyGrid.DataSource = _properties;
            PropertyGrid.DataBind();
        }

        public void bindGridImprovements()
        {
            List<ValuationExtendedDetailsPropertyGridRowItem> lstItems = new List<ValuationExtendedDetailsPropertyGridRowItem>();

            for (int x = 0; x < _valManual.ValuationImprovements.Count; x++)
            {
                ValuationExtendedDetailsPropertyGridRowItem itm = new ValuationExtendedDetailsPropertyGridRowItem();

                if (_valManual.ValuationImprovements[x].Key > 0)
                    itm.Key = _valManual.ValuationImprovements[x].Key.ToString();
                else
                    itm.Key = x.ToString();

                itm.buildingType = "Improvement";
                itm.extent = "";
                itm.rate = "";
                itm.replacementValue = Convert.ToDouble(_valManual.ValuationImprovements[x].ImprovementValue).ToString();
                itm.roofType = "";
                itm.ImprovementType = _valManual.ValuationImprovements[x].ValuationImprovementType.Description;
                itm.ImprovementDate = _valManual.ValuationImprovements[x].ImprovementDate.ToString();
                itm.RoofTypeKey = "0";
                itm.ImprovementTypeKey = _valManual.ValuationImprovements[x].ValuationImprovementType.Key.ToString();
                itm.ImprovementDateDateTime = Convert.ToDateTime(_valManual.ValuationImprovements[x].ImprovementDate);
                lstItems.Add(itm);
            }

            for (int k = 0; k < _valManual.ValuationOutBuildings.Count; k++)
            {
                ValuationExtendedDetailsPropertyGridRowItem itm = new ValuationExtendedDetailsPropertyGridRowItem();

                if (_valManual.ValuationOutBuildings[k].Key > 0)
                    itm.Key = _valManual.ValuationOutBuildings[k].Key.ToString();
                else
                    itm.Key = k.ToString();

                itm.buildingType = "Outbuilding";
                itm.extent = _valManual.ValuationOutBuildings[k].Extent.ToString();
                itm.rate = _valManual.ValuationOutBuildings[k].Rate.ToString();
                itm.replacementValue = Convert.ToDouble(_valManual.ValuationOutBuildings[k].Rate * _valManual.ValuationOutBuildings[k].Extent).ToString();
                if (_valManual.ValuationOutBuildings[k].ValuationRoofType != null)
                    itm.roofType = _valManual.ValuationOutBuildings[k].ValuationRoofType.Description;
                else
                    itm.roofType = "";
                itm.ImprovementType = "";
                itm.ImprovementDate = "";
                if (_valManual.ValuationOutBuildings[k].ValuationRoofType != null)
                    itm.RoofTypeKey = _valManual.ValuationOutBuildings[k].ValuationRoofType.Key.ToString();
                itm.ImprovementTypeKey = "0";
                itm.ImprovementDateDateTime = DateTime.Now;
                lstItems.Add(itm);
            }
            ImprovementsGrid.Columns.Clear();
            ImprovementsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(14), HorizontalAlign.Left, false);
            ImprovementsGrid.AddGridBoundColumn("buildingType", "Building Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ImprovementsGrid.AddGridBoundColumn("extent", SAHL.Common.Constants.NumberFormat, GridFormatType.GridNumber, "Extent (sq meter)", false, Unit.Percentage(14), HorizontalAlign.Center, true);
            ImprovementsGrid.AddGridBoundColumn("rate", SAHL.Common.Constants.NumberFormat, GridFormatType.GridString, "Rate (R/sq meter)", false, Unit.Percentage(14), HorizontalAlign.Center, true);
            ImprovementsGrid.AddGridBoundColumn("replacementValue", SAHL.Common.Constants.NumberFormat, GridFormatType.GridString, "Replacement Value", false, Unit.Percentage(16), HorizontalAlign.Center, true);
            ImprovementsGrid.AddGridBoundColumn("roofType", "Roof Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ImprovementsGrid.AddGridBoundColumn("ImprovementType", "Improvement Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ImprovementsGrid.AddGridBoundColumn("ImprovementDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Improvement Date", false, Unit.Percentage(14), HorizontalAlign.Center, true);
            ImprovementsGrid.AddGridBoundColumn("RoofTypeKey", "", Unit.Percentage(14), HorizontalAlign.Left, false);
            ImprovementsGrid.AddGridBoundColumn("ImprovementTypeKey", "", Unit.Percentage(14), HorizontalAlign.Left, false);
            ImprovementsGrid.AddGridBoundColumn("ImprovementDateDateTime", "", Unit.Percentage(14), HorizontalAlign.Left, false);

            ImprovementsGrid.DataSource = lstItems;

            if (!this.IsPostBack && lstItems.Count > 0)
                ImprovementsGrid.SelectFirstRow = true;

            ImprovementsGrid.DataBind();
        }

        public string getSelectedFirstRowGrid
        {
            get
            {
                string selectedGridItem;

                if (ImprovementsGrid.SelectedRow.Cells[1].Text == "Outbuilding")
                    selectedGridItem = "1" + "," + ImprovementsGrid.SelectedRow.Cells[0].Text;
                else
                    selectedGridItem = "2" + "," + ImprovementsGrid.SelectedRow.Cells[0].Text;
                
                return selectedGridItem;
            }
        }

        public void ShowButtonsForImprovements()
        {
            AddButton.Visible = false;
            AddNewButton.Visible = false;
            SubmitButton.Visible = false;
        }

        public void SetButtonsForFirstImprovementAdd(int numberRecs)
        {
            if (numberRecs > 0)
            {
                AddNewButton.Enabled = true;
                AddNewButton.Text = "Add New Entry";
                AddButton.Text = "Update";
                ddlBuildingType.Enabled = false;
            }
            else
            {
                AddNewButton.Enabled = false;
                AddNewButton.Text = "Cancel Add";
                AddButton.Text = "Add";
            }
        }

        public void SetupViewForAddNewEntry()
        {
            ddlCombinedOutbuildingsRoofType.SelectedIndex = 0;
            txtCombinedOutbuildingsExtent.Text = "";
            txtCombinedOutbuildingsRate.Text = "";
            txtCombinedOutbuildingsReplaceValue.Text = "";

            ddlImprovementType.SelectedIndex = 0;
            txtImprovementReplacementValue.Text = "";
            dteImprovementDate.Date = null;
            ddlBuildingType.Enabled = true;
            
            AddNewButton.Text = "Cancel Add";
            AddButton.Text = "Add";
            txtHiddenBox.Text = "Add";
        }

        protected void gridProperty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        TableCellCollection cells = e.Row.Cells;

            IProperty property = e.Row.DataItem as IProperty;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = property.Key.ToString();
                IAddressStreet addressStreet = property.Address as IAddressStreet;
                if (addressStreet != null)
                {
                    cells[1].Text = addressStreet.UnitNumber;
                    cells[2].Text = addressStreet.BuildingNumber + " " + addressStreet.BuildingName;
                    cells[3].Text = addressStreet.StreetNumber + " " + addressStreet.StreetName;
                }
                cells[4].Text = property.Address.RRR_SuburbDescription;
                cells[5].Text = property.Address.RRR_CityDescription;
                cells[6].Text = property.Address.RRR_ProvinceDescription;
            }    
         }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (OnBackButtonClicked != null)
                OnBackButtonClicked(sender, e);  
        }

        protected void Cancel_ButtonClick(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void Next_ButtonClick(object sender, EventArgs e)
        {
            if (OnNextButtonClicked != null)
                OnNextButtonClicked(sender, e);
        }

        protected void Add_ButtonClicked(object sender, EventArgs e)
        {
            string action = txtHiddenBox.Text;
            txtHiddenBox.Text = "";

            if (OnAddButtonClicked != null)
                OnAddButtonClicked(sender, new KeyChangedEventArgs(action));
        }

        protected void AddNew_ButtonClick(object sender, EventArgs e)
        {
            if (OnAddNewButtonClicked != null)
                OnAddNewButtonClicked(sender,e );
        }

        public event KeyChangedEventHandler OnImprovementsGridSelectedIndexChanged;

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnNextButtonClicked;

        public event EventHandler OnBackButtonClicked;

        public event KeyChangedEventHandler ddlBuildingTypeOnSelectedIndexChanged;

        public event KeyChangedEventHandler OnAddButtonClicked;

        public event EventHandler OnAddNewButtonClicked;

        public void BindCottageRoofTypes(IEventList<IValuationRoofType> hocRoof)
        {
            ddlCottageRoofType.DataTextField = "Description";
            ddlCottageRoofType.DataValueField = "Key";
            ddlCottageRoofType.DataSource = hocRoof;
            ddlCottageRoofType.DataBind();
        }

        public void BindMainBuildingRoofType(IEventList<IValuationRoofType> hocRoof)
        {
            ddlMainBuildingRoofType.DataTextField = "Description";
            ddlMainBuildingRoofType.DataValueField = "Key";
            ddlMainBuildingRoofType.DataSource = hocRoof;
            ddlMainBuildingRoofType.DataBind();
        }

        public void BindBuildingClassification( IEventList<IValuationClassification> valuationClassification)
        {
            ddlMainBuildingClassification.DataTextField = "Description";
            ddlMainBuildingClassification.DataValueField = "Key";
            ddlMainBuildingClassification.DataSource = valuationClassification;
            ddlMainBuildingClassification.DataBind();
        }

        public void BindOutBuildingRoofType(IEventList<IValuationRoofType> valuationRoofTypes)
        {
            ddlCombinedOutbuildingsRoofType.DataTextField = "Description";
            ddlCombinedOutbuildingsRoofType.DataValueField = "Key";
            ddlCombinedOutbuildingsRoofType.DataSource = valuationRoofTypes;
            ddlCombinedOutbuildingsRoofType.DataBind();
        }

        public void BindImprovementType(IEventList<IValuationImprovementType> valImprovementTypes)
        {
            ddlImprovementType.DataTextField = "Description";
            ddlImprovementType.DataValueField = "Key";
            ddlImprovementType.DataSource = valImprovementTypes;
            ddlImprovementType.DataBind();

        }

        public IValuationMainBuilding GetValuationMainBuilding(IValuationMainBuilding valMainBuilding)
        {
         //   ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            if (ddlMainBuildingRoofType.SelectedValue != "-select-")
                valMainBuilding.ValuationRoofType = lookups.ValuationRoofTypes.ObjectDictionary[ddlMainBuildingRoofType.SelectedValue];
            if (txtMainBuildingExtent.Text.Length > 0 )
                valMainBuilding.Extent = Convert.ToDouble(txtMainBuildingExtent.Text);
            if (txtMainBuildingRate.Text.Length > 0)
                valMainBuilding.Rate = Convert.ToDouble(txtMainBuildingRate.Text);
        
            return valMainBuilding;
        }

        public IValuationCottage GetValuationCottage(IValuationCottage valCottage)
        {
           ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

           if (ddlCottageRoofType.SelectedValue == "-select-")
               valCottage = null;
           else
           {
               if (ddlCottageRoofType.SelectedValue != "-select-")
                   valCottage.ValuationRoofType = lookups.ValuationRoofTypes.ObjectDictionary[ddlCottageRoofType.SelectedValue];
               if (txtCottageExtent.Text.Length > 0)
                   valCottage.Extent = Convert.ToDouble(txtCottageExtent.Text);
               if (txtCottageRate.Text.Length > 0)
                   valCottage.Rate = Convert.ToDouble(txtCottageRate.Text);
           }
           return valCottage;
        }

        public IValuationDiscriminatedSAHLManual GetValuationManual(IValuationDiscriminatedSAHLManual valSAHLManual)
        {
           ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

           if (ddlMainBuildingClassification.SelectedValue != "-select-")
              valSAHLManual.ValuationClassification = lookups.ValuationClassification.ObjectDictionary[ddlMainBuildingClassification.SelectedValue];
           if (txtEscalation.Text.Length > 0)
              valSAHLManual.ValuationEscalationPercentage = Convert.ToDouble(txtEscalation.Text);
            valSAHLManual.ValuationUserID = this.CurrentPrincipal.Identity.Name;
            return valSAHLManual;
        }

        public IValuationImprovement GetValuationImprovement(IValuationImprovement valImprovement)
        {
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            if (ddlImprovementType.SelectedValue != "-select-")
                valImprovement.ValuationImprovementType = lookups.ValuationImprovementType.ObjectDictionary[ddlImprovementType.SelectedValue];

                valImprovement.ImprovementValue = Convert.ToDouble(txtImprovementReplacementValue.Text);
                valImprovement.ImprovementDate = dteImprovementDate.Date;
            return valImprovement;
        }

        public IValuationOutbuilding GetCapturedOutbuilding(IValuationOutbuilding valOutbuilding)
        {
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            if (ddlCombinedOutbuildingsRoofType.SelectedValue != "-select-")
                valOutbuilding.ValuationRoofType = lookups.ValuationRoofTypes.ObjectDictionary[ddlCombinedOutbuildingsRoofType.SelectedValue];
                if (txtCombinedOutbuildingsExtent.Text.Length > 0)
                    valOutbuilding.Extent = Convert.ToDouble(txtCombinedOutbuildingsExtent.Text);
                if (txtCombinedOutbuildingsRate.Text.Length > 0)
                    valOutbuilding.Rate = Convert.ToDouble(txtCombinedOutbuildingsRate.Text);
            
            return valOutbuilding;
        }

        class ValuationExtendedDetailsPropertyGridRowItem
        {
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _key;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _buildingType;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _extent;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _rate;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _replacementValue;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _roofType;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _ImprovementType;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _ImprovementDate;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _roofTypeKey;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private string _improvementTypeKey;
            [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private DateTime _improvementDte;

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string buildingType
            {
                get
                {
                    return _buildingType;   
                }
                set
                {
                    _buildingType = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string extent
            {
                get
                {
                    return _extent;
                }
                set
                {
                    _extent = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string rate
            {
                get
                {
                    return _rate;
                }
                set
                {
                    _rate = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string replacementValue
            {
                get
                {
                    return _replacementValue;
                }
                set
                {
                    _replacementValue = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string roofType
            {
                get
                {
                    return _roofType;
                }
                set
                {
                    _roofType = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string ImprovementType
            {
                get
                {
                    return _ImprovementType;
                }
                set
                {
                    _ImprovementType = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string ImprovementDate
            {
                get
                {
                    return _ImprovementDate;
                }
                set
                {
                    _ImprovementDate = value;
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string RoofTypeKey
            {
                get
                {
                    return _roofTypeKey;
                }
                set
                {
                    _roofTypeKey = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public string ImprovementTypeKey
            {
                get
                {
                    return _improvementTypeKey;
                }
                set
                {
                    _improvementTypeKey = value;
                }
            }
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public DateTime ImprovementDateDateTime
            {
                get
                {
                    return _improvementDte;
                }
                set
                {
                    _improvementDte = value;
                }
            }
        }

    }
}
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    m_MyController = base.Controller as PropertyController;

        //    if (!ShouldRunPage())
        //        return;

        //    m_MyController.m_AddingNew = false;
        //    ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
        //    if (viewSettings.CustomAttributes.Count > 0)
        //    {
        //        System.Xml.XmlNode PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");
        //        if (PageStateNode != null)
        //        {
        //            switch (PageStateNode.Value)
        //            {
        //                case "displayaddition":
        //                    m_PageState = PageState.DisplayAddition;
        //                    break;
        //                case "addmain":
        //                    m_PageState = PageState.AddMain;
        //                    break;
        //                case "addaddition":
        //                    m_MyController.m_AddingNew = true;
        //                    m_PageState = PageState.UpdateAddition;
        //                    break;
        //                case "updatemain":
        //                    m_PageState = PageState.UpdateMain;
        //                    break;
        //                case "updateaddition":
        //                    m_PageState = PageState.UpdateAddition;
        //                    break;
        //            }
        //        }
        //    }

        //    PopulateLookups();


        //    int accKey = int.Parse(m_CBONavigator.SelectedItem.GenericKey);
        //    if (!IsPostBack)
        //    {
        //        if (m_PageState == PageState.AddMain || m_PageState == PageState.AddAddition || m_PageState == PageState.UpdateAddition)
        //        {
        //            m_MyController.GetPropertyAndAddressByAccountKey(accKey, base.GetClientMetrics());

        //            if (m_PageState == PageState.AddMain && m_MyController.m_ResetDS == true)
        //            {
        //                m_MyController.m_ValuationExtDS = null;
        //                m_MyController.m_ValuationsImprovements = null;
        //                m_MyController.m_AddOnUpdate = false;
        //            }
        //        }
        //        else if (m_PageState == PageState.UpdateMain)
        //        {
        //            if (m_MyController.m_ValuationExtDS != null)
        //            {
        //                m_MyController.GetPropertyAndAddressByAccountKey(accKey, base.GetClientMetrics());
        //            }
        //            else
        //            {
        //                if (m_MyController.m_CrossControllerPropertyKey != -1 && m_MyController.m_ExtentValuationKey != -1)
        //                {
        //                    m_MyController.GetPropertyExtentDetails(base.GetClientMetrics());
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (m_MyController.m_CrossControllerPropertyKey != -1 && m_MyController.m_ExtentValuationKey != -1)
        //            {
        //                m_MyController.GetPropertyExtentDetails(base.GetClientMetrics());
        //            }
        //        }

        //    }

        //    CombinedOutbuildingsPanel.Visible = false;
        //    CombinedImprovementsPanel.Visible = false;
        //    bindDropDowns();
        //    bindGridProperty();
        //    bindDisplay();
        //    m_MyController.m_ResetDS = true;
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage())
        //        return;

        //    switch (m_PageState)
        //    {
        //        case PageState.DisplayAddition:
        //            txtCottageExtent.Visible = false;
        //            txtCottageRate.Visible = false;
        //            txtMainBuildingExtent.Visible = false;
        //            txtMainBuildingRate.Visible = false;
        //            ddlCottageRoofType.Visible = false;
        //            ddlMainBuildingClassification.Visible = false;
        //            ddlMainBuildingRoofType.Visible = false;
        //            ddlImprovementType.Visible = false;

        //            //txtCombinedImprovementsExtent.Visible = false;
        //            //txtCombinedImprovementsRate.Visible = false;
        //            txtCombinedOutbuildingsExtent.Visible = false;
        //            txtCombinedOutbuildingsRate.Visible = false;
        //            ddlCombinedOutbuildingsRoofType.Visible = false;
        //            //DateofImprovement.Visible = false;
        //            txtCombinedThatchValue.Visible = false;
        //            txtValuationEscalationPercentage.Visible = false;

        //            ExtentTable.Visible = false;
        //            AdditionsTable.Visible = true;
        //            BackButton.Visible = true;
        //            AddButton.Visible = false;
        //            SubmitButton.Visible = false;
        //            ImprovementsGrid.Visible = true;

        //            AddNewButton.Visible = false;
        //            break;

        //        case PageState.AddMain:
        //        case PageState.UpdateMain:
        //            lblCottageExtent.Visible = false;
        //            lblCottageRate.Visible = false;
        //            lblMainBuildingExtent.Visible = false;
        //            lblMainBuildingRate.Visible = false;
        //            lblCottageRoofType.Visible = false;
        //            lblMainBuildingClassification.Visible = false;
        //            lblMainBuildingRoofType.Visible = false;
        //            lblImprovementType.Visible = false;

        //            //lblCombinedImprovementsExtent.Visible = false;
        //            //lblCombinedImprovementsRate.Visible = false;
        //            lblCombinedOutbuildingsExtent.Visible = false;
        //            lblCombinedOutbuildingsRate.Visible = false;
        //            lblCombinedOutbuildingsRoofType.Visible = false;
        //            //lblDateofImprovement.Visible = false;
        //            lblCombinedThatchValue.Visible = false;
        //            lblValuationEscalationPercentage.Visible = false;

        //            ExtentTable.Visible = true;
        //            AdditionsTable.Visible = false;
        //            BackButton.Visible = false;
        //            AddButton.Visible = false;
        //            ImprovementsGrid.Visible = false;

        //            AddNewButton.Visible = false;
        //            break;

        //        case PageState.AddAddition:
        //        case PageState.UpdateAddition:
        //            lblCottageExtent.Visible = false;
        //            lblCottageRate.Visible = false;
        //            lblMainBuildingExtent.Visible = false;
        //            lblMainBuildingRate.Visible = false;
        //            lblCottageRoofType.Visible = false;
        //            lblMainBuildingClassification.Visible = false;
        //            lblMainBuildingRoofType.Visible = false;

        //            //lblCombinedImprovementsExtent.Visible = false;
        //            //lblCombinedImprovementsRate.Visible = false;
        //            lblCombinedOutbuildingsExtent.Visible = false;
        //            lblCombinedOutbuildingsRate.Visible = false;
        //            lblCombinedOutbuildingsRoofType.Visible = false;
        //            //lblDateofImprovement.Visible = false;
        //            lblCombinedThatchValue.Visible = false;
        //            lblValuationEscalationPercentage.Visible = false;
        //            lblImprovementType.Visible = false;

        //            ExtentTable.Visible = false;
        //            AdditionsTable.Visible = true;
        //            BackButton.Visible = true;
        //            AddButton.Visible = true;
        //            if (m_PageState == PageState.UpdateAddition && !m_MyController.m_AddOnUpdate)
        //                AddButton.Text = "Update";
        //            else
        //                AddButton.Text = "Add";
        //            ImprovementsGrid.Visible = true;

        //            AddNewButton.Visible = true;
        //            break;

        //    }

        //    if (m_PageState == PageState.AddMain)
        //        if (m_MyController.AddressDS._Address.Rows.Count == 0)
        //            SubmitButton.Enabled = false;
        //}

        //private void bindGridProperty()
        //{
        //    PropertyGrid.AddGridBoundColumn("UnitNumber", "Unit", Unit.Percentage(6), HorizontalAlign.Left, true);
        //    PropertyGrid.AddGridBoundColumn("BuildingNumber", "Building", Unit.Percentage(20), HorizontalAlign.Left, true);
        //    PropertyGrid.AddGridBoundColumn("BuildingName", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    PropertyGrid.AddGridBoundColumn("StreetNumber", "Street", Unit.Percentage(20), HorizontalAlign.Left, true);
        //    PropertyGrid.AddGridBoundColumn("StreetName", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    PropertyGrid.AddGridBoundColumn("RRR_SuburbDescription", "Suburb", Unit.Percentage(18), HorizontalAlign.Left, true);
        //    PropertyGrid.AddGridBoundColumn("RRR_CityDescription", "City", Unit.Percentage(18), HorizontalAlign.Left, true);
        //    PropertyGrid.AddGridBoundColumn("RRR_ProvinceDescription", "Province", Unit.Percentage(18), HorizontalAlign.Left, true);
        //    PropertyGrid.AddGridBoundColumn("", "PropertyKey", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    PropertyGrid.AddGridBoundColumn("AddressKey", "AddressKey", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    PropertyGrid.AddGridBoundColumn("", "FinKey", Unit.Percentage(0), HorizontalAlign.Left, false);

        //    PropertyGrid.DataSource = m_MyController.AddressDS._Address;
        //    PropertyGrid.DataBind();
        //}


        //private void bindGridImprovements()
        //{
        //    m_MyController.GetValuationGridData();
        //    ImprovementsGrid.Columns.Clear();
        //    ImprovementsGrid.AddGridBoundColumn("BuildingType", "Building Type", Unit.Percentage(14), HorizontalAlign.Left, true);
        //    ImprovementsGrid.AddGridBoundColumn("Extent", Constants.NumberFormat, GridFormatType.GridNumber, "Extent (sq meter)", false, Unit.Percentage(14), HorizontalAlign.Center, true);
        //    ImprovementsGrid.AddGridBoundColumn("Rate", Constants.CurrencyFormat, GridFormatType.GridString, "Rate (R/sq meter)", false, Unit.Percentage(14), HorizontalAlign.Center, true);
        //    ImprovementsGrid.AddGridBoundColumn("ReplacementValue", Constants.CurrencyFormat, GridFormatType.GridString, "Replacement Value", false, Unit.Percentage(16), HorizontalAlign.Center, true);
        //    ImprovementsGrid.AddGridBoundColumn("RoofType", "Roof Type", Unit.Percentage(14), HorizontalAlign.Left, true);
        //    ImprovementsGrid.AddGridBoundColumn("ImprovementDescription", "Improvement Type", Unit.Percentage(14), HorizontalAlign.Left, true);
        //    ImprovementsGrid.AddGridBoundColumn("ImprovementDate", Constants.DateFormat, GridFormatType.GridDate, "Improvement Date", false, Unit.Percentage(14), HorizontalAlign.Center, true);

        //    ImprovementsGrid.DataSource = m_MyController.m_ValuationsImprovements;
        //    ImprovementsGrid.DataBind();
        //}

        //private void bindDisplay()
        //{
        //    switch (m_PageState)
        //    {
        //        case PageState.DisplayAddition:
        //            if (m_MyController.m_ValuationExtDS != null)
        //            {
        //                bindGridImprovements();

        //                int selectedIndex = -1;
        //                selectedIndex = ImprovementsGrid.SelectedIndexInternal;
        //                if (m_MyController.m_ValuationsImprovements == null || selectedIndex >= m_MyController.m_ValuationsImprovements.Count)
        //                    selectedIndex = -1;

        //                if (selectedIndex != -1)
        //                {
        //                    if (m_MyController.m_ValuationsImprovements[selectedIndex].BuildingType == "Outbuilding")
        //                    {
        //                        ddlBuildingType.SelectedValue = "Outbuilding";
        //                        CombinedOutbuildingsPanel.Visible = true;
        //                        CombinedImprovementsPanel.Visible = false;

        //                        lblCombinedOutbuildingsRoofType.Text = m_MyController.m_ValuationsImprovements[selectedIndex].RoofType;
        //                        lblCombinedOutbuildingsExtent.Text = m_MyController.m_ValuationsImprovements[selectedIndex].Extent.ToString(SAHL.Common.Constants.NumberFormat);
        //                        lblCombinedOutbuildingsRate.Text = m_MyController.m_ValuationsImprovements[selectedIndex].Rate.ToString(SAHL.Common.Constants.CurrencyFormat);
        //                        lblCombinedOutbuildingsReplaceValue.Text = m_MyController.m_ValuationsImprovements[selectedIndex].ReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
        //                    }
        //                    else
        //                    {
        //                        ddlBuildingType.SelectedValue = "Improvement";
        //                        ddlBuildingType.Enabled = false;
        //                        CombinedOutbuildingsPanel.Visible = false;
        //                        CombinedImprovementsPanel.Visible = true;

        //                        lblImprovementType.Text = m_MyController.m_ValuationsImprovements[selectedIndex].ImprovementDescription;
        //                        lblCombinedImprovementsReplaceValue.Text = m_MyController.m_ValuationsImprovements[selectedIndex].ReplacementValue.ToString(SAHL.Common.Constants.NumberFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    CombinedOutbuildingsPanel.Visible = false;
        //                    CombinedImprovementsPanel.Visible = false;
        //                }
        //                ddlBuildingType.Enabled = false;
        //            }
        //            break;

        //        case PageState.AddMain:
        //        case PageState.UpdateMain:
        //            {
        //                if (m_MyController.m_ValuationExtDS == null)
        //                {
        //                    m_MyController.m_ValuationExtDS = new Valuation();
        //                    m_MyController.m_ValuationsImprovements = null;
        //                }

        //                double mainbuildingReplaceValue = 0;
        //                double mainbuildingExtent = 0;
        //                double mainbuildingRate = 0;
        //                bool recordExists = true;
        //                if (m_MyController.m_ValuationExtDS.ValuationMainBuilding == null || m_MyController.m_ValuationExtDS.ValuationMainBuilding.Count == 0)
        //                {
        //                    recordExists = false;
        //                }
        //                if ((Request.Form[txtMainBuildingExtent.UniqueID] == null || Request.Form[txtMainBuildingRate.UniqueID] == ""))
        //                {
        //                    if (recordExists && !m_MyController.m_ValuationExtDS.ValuationMainBuilding[0].IsExtentNull())
        //                        mainbuildingExtent = m_MyController.m_ValuationExtDS.ValuationMainBuilding[0].Extent;
        //                }
        //                else
        //                    mainbuildingExtent = Convert.ToDouble(Request.Form[txtMainBuildingExtent.UniqueID]);
        //                txtMainBuildingExtent.Text = (mainbuildingExtent != 0) ? mainbuildingExtent.ToString(SAHL.Common.Constants.NumberFormat) : "";


        //                if (Request.Form[ddlMainBuildingRoofType.UniqueID] == null || Request.Form[ddlMainBuildingRoofType.UniqueID] == "-select-")
        //                {
        //                    if (recordExists && !m_MyController.m_ValuationExtDS.ValuationMainBuilding[0].IsValuationRoofTypeKeyNull())
        //                        ddlMainBuildingRoofType.SelectedValue = m_MyController.m_ValuationExtDS.ValuationMainBuilding[0].ValuationRoofTypeKey.ToString();
        //                }
        //                else
        //                    ddlMainBuildingRoofType.SelectedValue = Request.Form[ddlMainBuildingRoofType.UniqueID];

        //                if (Request.Form[txtMainBuildingRate.UniqueID] == null || Request.Form[txtMainBuildingRate.UniqueID] == "")
        //                {
        //                    if (recordExists && !m_MyController.m_ValuationExtDS.ValuationMainBuilding[0].IsRateNull())
        //                        mainbuildingRate = m_MyController.m_ValuationExtDS.ValuationMainBuilding[0].Rate;
        //                }
        //                else
        //                    mainbuildingRate = Convert.ToDouble(Request.Form[txtMainBuildingRate.UniqueID]);
        //                txtMainBuildingRate.Text = (mainbuildingRate != 0) ? mainbuildingRate.ToString(SAHL.Common.Constants.NumberFormat) : "";


        //                mainbuildingReplaceValue = mainbuildingExtent * mainbuildingRate;
        //                lblMainBuildingReplaceValue.Text = mainbuildingReplaceValue.ToString(SAHL.Common.Constants.CurrencyFormat);


        //                double cottageReplaceValue = 0;
        //                double cottageExtent = 0;
        //                double cottageRate = 0;
        //                recordExists = true;
        //                if (m_MyController.m_ValuationExtDS.ValuationCottage == null || m_MyController.m_ValuationExtDS.ValuationCottage.Count == 0 || m_MyController.m_ValuationExtDS.ValuationCottage[0].RowState == DataRowState.Deleted)
        //                {
        //                    recordExists = false;
        //                }
        //                if (Request.Form[txtCottageExtent.UniqueID] == null || Request.Form[txtCottageExtent.UniqueID] == "")
        //                {
        //                    if (recordExists && m_MyController.m_ValuationExtDS.ValuationCottage[0].RowState != DataRowState.Deleted && !m_MyController.m_ValuationExtDS.ValuationCottage[0].IsExtentNull())
        //                    {
        //                        cottageExtent = m_MyController.m_ValuationExtDS.ValuationCottage[0].Extent;
        //                    }
        //                }
        //                else
        //                    cottageExtent = Convert.ToDouble(Request.Form[txtCottageExtent.UniqueID]);
        //                txtCottageExtent.Text = (cottageExtent != 0) ? cottageExtent.ToString(SAHL.Common.Constants.NumberFormat) : "";

        //                if (Request.Form[txtCottageRate.UniqueID] == null || Request.Form[txtCottageRate.UniqueID] == "")
        //                {
        //                    if (recordExists && m_MyController.m_ValuationExtDS.ValuationCottage[0].RowState != DataRowState.Deleted && !m_MyController.m_ValuationExtDS.ValuationCottage[0].IsRateNull())
        //                    {
        //                        cottageRate = m_MyController.m_ValuationExtDS.ValuationCottage[0].Rate;
        //                    }
        //                }
        //                else
        //                    cottageRate = Convert.ToDouble(Request.Form[txtCottageRate.UniqueID]);
        //                txtCottageRate.Text = (cottageRate != 0) ? cottageRate.ToString(SAHL.Common.Constants.NumberFormat) : "";

        //                if (Request.Form[ddlCottageRoofType.UniqueID] == null || Request.Form[ddlCottageRoofType.UniqueID] == "-select-")
        //                {
        //                    if (recordExists && m_MyController.m_ValuationExtDS.ValuationCottage[0].RowState != DataRowState.Deleted && !m_MyController.m_ValuationExtDS.ValuationCottage[0].IsValuationRoofTypeKeyNull())
        //                        ddlCottageRoofType.SelectedValue = m_MyController.m_ValuationExtDS.ValuationCottage[0].ValuationRoofTypeKey.ToString();
        //                }
        //                else
        //                    ddlCottageRoofType.SelectedValue = Request.Form[ddlCottageRoofType.UniqueID];
        //                cottageReplaceValue = cottageExtent * cottageRate;
        //                lblCottageReplaceValue.Text = cottageReplaceValue.ToString(SAHL.Common.Constants.CurrencyFormat);


        //                double combinedThatchValue = 0;
        //                recordExists = true;
        //                if (m_MyController.m_ValuationExtDS.ValuationCombinedThatch == null || m_MyController.m_ValuationExtDS.ValuationCombinedThatch.Count == 0)
        //                {
        //                    recordExists = false;
        //                }
        //                if (Request.Form[txtCombinedThatchValue.UniqueID] == null || Request.Form[txtCombinedThatchValue.UniqueID] == "")
        //                {
        //                    if (recordExists && m_MyController.m_ValuationExtDS.ValuationCombinedThatch[0].RowState != DataRowState.Deleted && !m_MyController.m_ValuationExtDS.ValuationCombinedThatch[0].IsValueNull())
        //                        combinedThatchValue = m_MyController.m_ValuationExtDS.ValuationCombinedThatch[0].Value;
        //                }
        //                else
        //                    combinedThatchValue = Convert.ToDouble(Request.Form[txtCombinedThatchValue.UniqueID]);
        //                txtCombinedThatchValue.Text = (combinedThatchValue != 0) ? combinedThatchValue.ToString(SAHL.Common.Constants.NumberFormat) : "";


        //                if (m_PageState == PageState.AddMain)
        //                {
        //                    if (Request.Form[txtValuationEscalationPercentage.UniqueID] != null && Request.Form[txtValuationEscalationPercentage.UniqueID] != "")
        //                    {
        //                        m_MyController.m_ExtentValuationEscalationPercentage = Convert.ToDouble(Request.Form[txtValuationEscalationPercentage.UniqueID]);
        //                        txtValuationEscalationPercentage.Text = Request.Form[txtValuationEscalationPercentage.UniqueID];
        //                    }
        //                    else if (m_MyController.m_ExtentValuationEscalationPercentage != -1)
        //                    {
        //                        txtValuationEscalationPercentage.Text = m_MyController.m_ExtentValuationEscalationPercentage.ToString(Constants.NumberFormat);
        //                    }
        //                    else
        //                    {
        //                        txtValuationEscalationPercentage.Text = "20.00";
        //                    }

        //                    if (Request.Form[ddlMainBuildingClassification.UniqueID] != null && Request.Form[ddlMainBuildingClassification.UniqueID] != "-select-")
        //                    {
        //                        m_MyController.m_ExtentValuationClassification = Convert.ToInt32(Request.Form[ddlMainBuildingClassification.UniqueID]);
        //                        ddlMainBuildingClassification.SelectedValue = Request.Form[ddlMainBuildingClassification.UniqueID];
        //                    }
        //                    else if (m_MyController.m_ExtentValuationClassification != -1)
        //                    {
        //                        ddlMainBuildingClassification.SelectedValue = m_MyController.m_ExtentValuationClassification.ToString();
        //                    }
        //                }
        //                else
        //                    if (m_PageState == PageState.UpdateMain)
        //                    {
        //                        recordExists = true;
        //                        if (m_MyController.m_ValuationExtDS._Valuation == null || m_MyController.m_ValuationExtDS._Valuation.Count == 0)
        //                        {
        //                            recordExists = false;
        //                        }
        //                        if (Request.Form[ddlMainBuildingClassification.UniqueID] == null || Request.Form[ddlMainBuildingClassification.UniqueID] == "-select-")
        //                        {
        //                            if (recordExists && !m_MyController.m_ValuationExtDS._Valuation[0].IsValuationClassificationKeyNull())
        //                                ddlMainBuildingClassification.SelectedValue = m_MyController.m_ValuationExtDS._Valuation[0].ValuationClassificationKey.ToString();
        //                        }
        //                        else
        //                            ddlMainBuildingClassification.SelectedValue = Request.Form[ddlMainBuildingRoofType.UniqueID];


        //                        if (Request.Form[txtValuationEscalationPercentage.UniqueID] == null || Request.Form[txtValuationEscalationPercentage.UniqueID] == "")
        //                        {
        //                            if (recordExists && !m_MyController.m_ValuationExtDS._Valuation[0].IsValuationEscalationPercentageNull())
        //                                txtValuationEscalationPercentage.Text = m_MyController.m_ValuationExtDS._Valuation[0].ValuationEscalationPercentage.ToString(Constants.NumberFormat);
        //                            else
        //                                txtValuationEscalationPercentage.Text = "20.00";
        //                        }
        //                        else
        //                            txtValuationEscalationPercentage.Text = Request.Form[txtValuationEscalationPercentage.UniqueID];
        //                    }
        //            }
        //            break;


        //        case PageState.AddAddition:
        //        case PageState.UpdateAddition:
        //            {

        //                bindGridImprovements();
        //                AddNewButton.Enabled = true;
        //                bool updating = false;
        //                if (m_PageState == PageState.UpdateAddition)
        //                {
        //                    if (ImprovementsGrid.SelectedIndexInternal < m_MyController.m_ValuationsImprovements.Count && !m_MyController.m_AddOnUpdate)
        //                    {
        //                        updating = true;
        //                        AddButton.Text = "Update";
        //                        AddNewButton.Text = "Add New Entry";
        //                    }
        //                    else
        //                    {
        //                        //AddButton.Enabled = false;
        //                        m_MyController.m_AddOnUpdate = true;
        //                        AddButton.Text = "Add";
        //                        AddNewButton.Text = "Cancel Add";
        //                        ddlBuildingType.Enabled = true;
        //                        if (m_MyController.m_ValuationsImprovements.Count == 0)
        //                            AddNewButton.Enabled = false;
        //                    }
        //                }

        //                string buildingtype = "";
        //                if (updating == true && !m_MyController.m_AddOnUpdate)
        //                {
        //                    ddlBuildingType.SelectedValue = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].BuildingType;
        //                    buildingtype = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].BuildingType;
        //                }
        //                else
        //                {
        //                    if (Request.Form[ddlBuildingType.UniqueID] != null)
        //                    {
        //                        buildingtype = Request.Form[ddlBuildingType.UniqueID];
        //                        ddlBuildingType.SelectedValue = Request.Form[ddlBuildingType.UniqueID];
        //                    }
        //                }
        //                if (buildingtype == "Outbuilding")
        //                {
        //                    CombinedOutbuildingsPanel.Visible = true;
        //                    CombinedImprovementsPanel.Visible = false;
        //                }
        //                else if (buildingtype == "Improvement")
        //                {
        //                    CombinedOutbuildingsPanel.Visible = false;
        //                    CombinedImprovementsPanel.Visible = true;
        //                }
        //                else
        //                {
        //                    if (m_PageState == PageState.AddAddition || m_MyController.m_AddOnUpdate)
        //                    {
        //                        ddlBuildingType.SelectedValue = "Outbuilding";
        //                        CombinedOutbuildingsPanel.Visible = true;
        //                        CombinedImprovementsPanel.Visible = false;
        //                    }
        //                    else
        //                    {
        //                        CombinedOutbuildingsPanel.Visible = false;
        //                        CombinedImprovementsPanel.Visible = false;
        //                    }
        //                }

        //                if (updating == true)
        //                {
        //                    ddlBuildingType.Enabled = false;
        //                    if (updating == true && buildingtype == "Outbuilding")
        //                    {

        //                        ddlBuildingType.SelectedValue = "Outbuilding";
        //                        CombinedOutbuildingsPanel.Visible = true;
        //                        CombinedImprovementsPanel.Visible = false;

        //                        if (Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID] != null && Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID] != "-select-")
        //                            ddlCombinedOutbuildingsRoofType.SelectedValue = Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID];
        //                        else
        //                            ddlCombinedOutbuildingsRoofType.SelectedValue = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].RoofTypeKey.ToString();

        //                        double combinedOutbuildingsExtent = 0;
        //                        if (Request.Form[txtCombinedOutbuildingsExtent.UniqueID] != null && Request.Form[txtCombinedOutbuildingsExtent.UniqueID] != "")
        //                        {
        //                            txtCombinedOutbuildingsExtent.Text = Request.Form[txtCombinedOutbuildingsExtent.UniqueID];
        //                            combinedOutbuildingsExtent = Convert.ToDouble(Request.Form[txtCombinedOutbuildingsExtent.UniqueID]);
        //                        }
        //                        else
        //                        {
        //                            txtCombinedOutbuildingsExtent.Text = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].Extent.ToString(Constants.NumberFormat);
        //                            combinedOutbuildingsExtent = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].Extent;
        //                        }

        //                        double combinedOutbuildingsRate = 0;
        //                        if (Request.Form[txtCombinedOutbuildingsRate.UniqueID] != null && Request.Form[txtCombinedOutbuildingsRate.UniqueID] != "")
        //                        {
        //                            txtCombinedOutbuildingsRate.Text = Request.Form[txtCombinedOutbuildingsRate.UniqueID];
        //                            combinedOutbuildingsRate = Convert.ToDouble(Request.Form[txtCombinedOutbuildingsRate.UniqueID]);
        //                        }
        //                        else
        //                        {
        //                            txtCombinedOutbuildingsRate.Text = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].Rate.ToString(Constants.NumberFormat);
        //                            combinedOutbuildingsRate = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].Rate;
        //                        }

        //                        lblCombinedOutbuildingsReplaceValue.Text = ((double)(combinedOutbuildingsRate * combinedOutbuildingsExtent)).ToString(SAHL.Common.Constants.CurrencyFormat);
        //                    }
        //                    else
        //                    {

        //                        ddlBuildingType.SelectedValue = "Improvement";

        //                        CombinedOutbuildingsPanel.Visible = false;
        //                        CombinedImprovementsPanel.Visible = true;
        //                        lblCombinedImprovementsReplaceValue.Enabled = true;

        //                        //if (Request.Form[ddlImprovementType.UniqueID] != null && Request.Form[ddlImprovementType.UniqueID] != "-select-")
        //                        //    ddlImprovementType.SelectedValue = Request.Form[ddlImprovementType.UniqueID];
        //                        //else
        //                        ddlImprovementType.SelectedValue = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].ImprovementType.ToString();

        //                        lblCombinedImprovementsReplaceValue.Text = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].ReplacementValue.ToString(SAHL.Common.Constants.NumberFormat);


        //                        //double combinedImprovementsReplaceValue = 0;
        //                        //if (Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] != null && Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] != "")
        //                        //{
        //                        //    lblCombinedImprovementsReplaceValue.Text = Request.Form[lblCombinedImprovementsReplaceValue.UniqueID];
        //                        //    combinedImprovementsReplaceValue = Convert.ToDouble(Request.Form[lblCombinedImprovementsReplaceValue.UniqueID]);
        //                        //}
        //                        //else
        //                        //{
        //                        //    if (!m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].IsImprovementValueNull())
        //                        //    {
        //                        //        lblCombinedImprovementsReplaceValue.Text = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].ImprovementValue.ToString(Constants.NumberFormat);
        //                        //        combinedImprovementsReplaceValue = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].ImprovementValue;
        //                        //    }
        //                        //}

        //                        //if (ddlImprovementType.SelectedValue == "6")
        //                        //{
        //                        //    lblCombinedImprovementsReplaceValue.Text = ((double)(combinedImprovementsReplaceValue)).ToString(SAHL.Common.Constants.NumberFormat);
        //                        //}
        //                        //else
        //                        //{
        //                        //    lblCombinedImprovementsReplaceValue.Text = ((double)(combinedImprovementsRate * combinedImprovementsExtent)).ToString(SAHL.Common.Constants.NumberFormat);
        //                        //}

        //                    }
        //                }
        //                else
        //                {

        //                    if (Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] != null && Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] != "")
        //                    {
        //                        lblCombinedImprovementsReplaceValue.Text = Request.Form[lblCombinedImprovementsReplaceValue.UniqueID];
        //                    }


        //                    double combinedOutbuildingsExtent = 0;
        //                    if (Request.Form[txtCombinedOutbuildingsExtent.UniqueID] != null && Request.Form[txtCombinedOutbuildingsExtent.UniqueID] != "")
        //                    {
        //                        combinedOutbuildingsExtent = Convert.ToDouble(Request.Form[txtCombinedOutbuildingsExtent.UniqueID]);
        //                    }

        //                    double combinedOutbuildingsRate = 0;
        //                    if (Request.Form[txtCombinedOutbuildingsRate.UniqueID] != null && Request.Form[txtCombinedOutbuildingsRate.UniqueID] != "")
        //                    {
        //                        combinedOutbuildingsRate = Convert.ToDouble(Request.Form[txtCombinedOutbuildingsRate.UniqueID]);
        //                    }
        //                }

        //                lblCombinedImprovementsReplaceValue.Enabled = true;
        //            }
        //            break;
        //    }
        //}


        //private void bindDropDowns()
        //{
        //    if (m_MyController.Lookups != null)
        //    {
        //        ddlImprovementType.DataSource = m_MyController.Lookups.ValuationImprovementType;
        //        ddlImprovementType.DataValueField = "ValuationImprovementTypeKey";
        //        ddlImprovementType.DataTextField = "Description";
        //        ddlImprovementType.DataBind();

        //        ddlCombinedOutbuildingsRoofType.DataSource = m_MyController.Lookups.ValuationRoofType;
        //        ddlCombinedOutbuildingsRoofType.DataValueField = "ValuationRoofTypeKey";
        //        ddlCombinedOutbuildingsRoofType.DataTextField = "Description";
        //        ddlCombinedOutbuildingsRoofType.DataBind();

        //        ddlCottageRoofType.DataSource = m_MyController.Lookups.ValuationRoofType;
        //        ddlCottageRoofType.DataValueField = "ValuationRoofTypeKey";
        //        ddlCottageRoofType.DataTextField = "Description";
        //        ddlCottageRoofType.DataBind();

        //        ddlMainBuildingRoofType.DataSource = m_MyController.Lookups.ValuationRoofType;
        //        ddlMainBuildingRoofType.DataValueField = "ValuationRoofTypeKey";
        //        ddlMainBuildingRoofType.DataTextField = "Description";
        //        ddlMainBuildingRoofType.DataBind();

        //        ddlMainBuildingClassification.DataSource = m_MyController.Lookups.ValuationClassification;
        //        ddlMainBuildingClassification.DataValueField = "ValuationClassificationKey";
        //        ddlMainBuildingClassification.DataTextField = "Description";
        //        ddlMainBuildingClassification.DataBind();
        //    }
        //}

        //protected void PropertyGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Cells[1].Text = e.Row.Cells[1].Text + " " + e.Row.Cells[2].Text;
        //        e.Row.Cells[3].Text = e.Row.Cells[3].Text + " " + e.Row.Cells[4].Text;

        //        int propertyKey = 0;
        //        int finKey = 0;
        //        GetAddressPropertyAndFinKey(int.Parse(e.Row.Cells[9].Text), ref propertyKey, ref finKey);
        //        e.Row.Cells[8].Text = propertyKey.ToString();
        //        e.Row.Cells[10].Text = finKey.ToString();
        //    }
        //}

        //protected void PropertyGrid_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    bindDisplay();
        //}

        //protected void ImprovementsGrid_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    bindDisplay();
        //}

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    m_MyController.m_ValuationExtDS = null;
        //    m_MyController.m_ValuationsImprovements = null;
        //    m_MyController.m_CrossControllerPropertyKey = -1;
        //    m_MyController.m_ExtentValuationClassification = -1;
        //    m_MyController.m_ExtentValuationEscalationPercentage = -1;
        //    m_MyController.m_AddOnUpdate = false;
        //    m_MyController.Navigator.Navigate("Cancel");
        //}

        //protected void BackButton_Click(object sender, EventArgs e)
        //{
        //    if (m_MyController.m_AddingNew == true)
        //        m_MyController.m_ResetDS = false;
        //    m_MyController.Navigator.Navigate("Back");
        //}


        //protected void AddNewButton_Click(object sender, EventArgs e)
        //{
        //    if (m_MyController.m_AddOnUpdate == true)
        //    {
        //        m_MyController.m_AddOnUpdate = false;
        //        AddNewButton.Text = "Add New Entry";
        //    }
        //    else
        //    {
        //        m_MyController.m_AddOnUpdate = true;
        //        AddNewButton.Text = "Cancel Add";
        //    }

        //    //txtCombinedImprovementsExtent.Text = "";
        //    //txtCombinedImprovementsRate.Text = "";
        //    //DateofImprovement.Date = DateTime.Today;
        //    ddlCombinedOutbuildingsRoofType.SelectedValue = "-select-";
        //    txtCombinedOutbuildingsExtent.Text = "";
        //    txtCombinedOutbuildingsRate.Text = "";
        //    m_MyController.m_ValuationsImprovements = null;
        //    lblCombinedImprovementsReplaceValue.Text = "";
        //    lblCombinedOutbuildingsReplaceValue.Text = "";
        //    bindDisplay();
        //}


        //protected void AddButton_Click(object sender, EventArgs e)
        //{
        //    string buildingType = "";
        //    if (m_PageState == PageState.AddAddition || m_MyController.m_AddOnUpdate)
        //    {
        //        if (Request.Form[ddlBuildingType.UniqueID] != null &&
        //       Request.Form[ddlBuildingType.UniqueID] == "Outbuilding")
        //        {
        //            buildingType = "Outbuilding";
        //        }
        //        else
        //        {
        //            buildingType = "Improvement";
        //        }

        //    }
        //    else
        //    {
        //        if (ImprovementsGrid.SelectedIndexInternal < m_MyController.m_ValuationsImprovements.Count)
        //        {
        //            buildingType = m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].BuildingType;
        //        }
        //    }
        //    if (buildingType != "Outbuilding")
        //    {
        //        bool addRow = true;
        //        //if (DateofImprovement.Date == DateofImprovement.DefaultDate)
        //        //{
        //        //    valDateofImprovement.IsValid = false;
        //        //    addRow = false;
        //        //}

        //        if (Request.Form[ddlImprovementType.UniqueID] == null || Request.Form[ddlImprovementType.UniqueID] == "-select-")
        //        {
        //            valddlImprovementType.IsValid = false;
        //            addRow = false;
        //            return;
        //        }


        //        if (Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] == null || Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] == "" || Convert.ToDouble(Request.Form[lblCombinedImprovementsReplaceValue.UniqueID]) == 0)
        //        {
        //            vallblCombinedImprovementsReplaceValue.IsValid = false;
        //            addRow = false;
        //        }


        //        if (addRow == true)
        //        {
        //            Valuation.ValuationImprovementRow newCombinedImprovementRow;
        //            if (m_PageState == PageState.AddAddition || m_MyController.m_AddOnUpdate)
        //            {
        //                newCombinedImprovementRow = m_MyController.m_ValuationExtDS.ValuationImprovement.NewValuationImprovementRow();
        //                newCombinedImprovementRow.ValuationKey = -1;
        //            }
        //            else
        //            {
        //                newCombinedImprovementRow = m_MyController.m_ValuationExtDS.ValuationImprovement.FindByValuationImprovementKey(m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].ImprovementKey);
        //            }

        //            if (Request.Form[ddlImprovementType.UniqueID] != null && Request.Form[ddlImprovementType.UniqueID] != "-select-")
        //                newCombinedImprovementRow.ValuationImprovementTypeKey = Convert.ToInt32(Request.Form[ddlImprovementType.UniqueID]);
        //            //if (newCombinedImprovementRow.ValuationImprovementTypeKey == 6)
        //            //{
        //            if (Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] != null && Request.Form[lblCombinedImprovementsReplaceValue.UniqueID] != "")
        //                newCombinedImprovementRow.ImprovementValue = Convert.ToDouble(Request.Form[lblCombinedImprovementsReplaceValue.UniqueID]);
        //            newCombinedImprovementRow.SetExtentNull();
        //            newCombinedImprovementRow.SetRateNull();
        //            //}
        //            //else
        //            //{
        //            //    //if (Request.Form[txtCombinedImprovementsExtent.UniqueID] != null && Request.Form[txtCombinedImprovementsExtent.UniqueID] != "")
        //            //    //    newCombinedImprovementRow.Extent = Convert.ToDouble(Request.Form[txtCombinedImprovementsExtent.UniqueID]);
        //            //    //if (Request.Form[txtCombinedImprovementsRate.UniqueID] != null && Request.Form[txtCombinedImprovementsRate.UniqueID] != "")
        //            //    //    newCombinedImprovementRow.Rate = Convert.ToDouble(Request.Form[txtCombinedImprovementsRate.UniqueID]);
        //            //    if (!newCombinedImprovementRow.IsExtentNull() && !newCombinedImprovementRow.IsRateNull())
        //            //        newCombinedImprovementRow.ImprovementValue = newCombinedImprovementRow.Rate * newCombinedImprovementRow.Extent;
        //            //}
        //            //if (DateofImprovement.Date != DateofImprovement.DefaultDate)
        //            //        newCombinedImprovementRow.ImprovementDate = DateofImprovement.Date;
        //            if (m_PageState == PageState.AddAddition || m_MyController.m_AddOnUpdate)
        //            {
        //                m_MyController.m_ValuationExtDS.ValuationImprovement.AddValuationImprovementRow(newCombinedImprovementRow);
        //            }
        //            //else
        //            //{
        //            //    m_MyController.m_ValuationExtDS.ValuationImprovement.AcceptChanges();
        //            //}
        //        }
        //        else
        //            return;
        //    }
        //    else
        //    {
        //        bool addRow = true;
        //        if (Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID] == null || Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID] == "-select-")
        //        {
        //            valddlCombinedOutbuildingsRoofType.IsValid = false;
        //            addRow = false;
        //        }

        //        if (Request.Form[txtCombinedOutbuildingsExtent.UniqueID] == null || Request.Form[txtCombinedOutbuildingsExtent.UniqueID] == "" || Convert.ToDouble(Request.Form[txtCombinedOutbuildingsExtent.UniqueID]) == 0)
        //        {
        //            valtxtCombinedOutbuildingsExtent.IsValid = false;
        //            addRow = false;
        //        }

        //        if (Request.Form[txtCombinedOutbuildingsRate.UniqueID] == null || Request.Form[txtCombinedOutbuildingsRate.UniqueID] == "" || Convert.ToDouble(Request.Form[txtCombinedOutbuildingsRate.UniqueID]) == 0)
        //        {
        //            valtxtCombinedOutbuildingsRate.IsValid = false;
        //            addRow = false;
        //        }

        //        if (addRow == true)
        //        {
        //            Valuation.ValuationOutbuildingRow newCombinedOutbuildingsRow;
        //            if (m_PageState == PageState.AddAddition || m_MyController.m_AddOnUpdate)
        //            {
        //                newCombinedOutbuildingsRow = m_MyController.m_ValuationExtDS.ValuationOutbuilding.NewValuationOutbuildingRow();
        //                newCombinedOutbuildingsRow.ValuationKey = -1;
        //            }
        //            else
        //            {
        //                newCombinedOutbuildingsRow = m_MyController.m_ValuationExtDS.ValuationOutbuilding.FindByValuationOutbuildingKey(m_MyController.m_ValuationsImprovements[ImprovementsGrid.SelectedIndexInternal].ImprovementKey);
        //            }
        //            if (Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID] != null && Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID] != "-select-")
        //                newCombinedOutbuildingsRow.ValuationRoofTypeKey = Convert.ToInt32(Request.Form[ddlCombinedOutbuildingsRoofType.UniqueID]);
        //            if (Request.Form[txtCombinedOutbuildingsExtent.UniqueID] != null && Request.Form[txtCombinedOutbuildingsExtent.UniqueID] != "")
        //                newCombinedOutbuildingsRow.Extent = Convert.ToDouble(Request.Form[txtCombinedOutbuildingsExtent.UniqueID]);
        //            if (Request.Form[txtCombinedOutbuildingsRate.UniqueID] != null && Request.Form[txtCombinedOutbuildingsRate.UniqueID] != "")
        //                newCombinedOutbuildingsRow.Rate = Convert.ToDouble(Request.Form[txtCombinedOutbuildingsRate.UniqueID]);
        //            if (m_PageState == PageState.AddAddition || m_MyController.m_AddOnUpdate)
        //            {
        //                m_MyController.m_ValuationExtDS.ValuationOutbuilding.AddValuationOutbuildingRow(newCombinedOutbuildingsRow);
        //            }
        //            //else
        //            //{
        //            //    m_MyController.m_ValuationExtDS.ValuationOutbuilding.AcceptChanges();
        //            //}
        //        }
        //        else
        //            return;
        //    }
        //    if (m_PageState == PageState.AddAddition)
        //    {
        //        //txtCombinedImprovementsExtent.Text = "";
        //        //txtCombinedImprovementsRate.Text = "";
        //        //DateofImprovement.Date = DateTime.Today ;
        //        ddlCombinedOutbuildingsRoofType.SelectedValue = "-select-";
        //        txtCombinedOutbuildingsExtent.Text = "";
        //        txtCombinedOutbuildingsRate.Text = "";
        //        m_MyController.m_ValuationsImprovements = null;
        //    }
        //    if (m_MyController.m_AddOnUpdate)
        //        m_MyController.m_AddOnUpdate = false;
        //    m_MyController.m_ValuationsImprovements = null;
        //    bindGridImprovements();
        //    bindDisplay();
        //}

        //protected void SubmitButton_Click(object sender, EventArgs e)
        //{

        //    switch (m_PageState)
        //    {
        //        case PageState.AddMain:
        //        case PageState.UpdateMain:
        //            bool founderror = false;
        //            if (Request.Form[txtMainBuildingExtent.UniqueID] == null || Request.Form[txtMainBuildingExtent.UniqueID] == "" || Convert.ToDouble(Request.Form[txtMainBuildingExtent.UniqueID]) == 0)
        //            {
        //                valtxtMainBuildingExtent.IsValid = false;
        //                founderror = true;
        //            }
        //            if (Request.Form[txtMainBuildingRate.UniqueID] != null && Request.Form[txtMainBuildingRate.UniqueID] != "" && Convert.ToDouble(Request.Form[txtMainBuildingRate.UniqueID]) == 0)
        //            {
        //                valtxtMainBuildingRate.IsValid = false;
        //                founderror = true;
        //            }

        //            if (founderror == true)
        //                return;

        //            if (PropertyGrid.SelectedRow != null)
        //            {
        //                m_MyController.m_CrossControllerPropertyKey = int.Parse(PropertyGrid.SelectedRow.Cells[8].Text);
        //            }
        //            else
        //            {
        //                ValSubmit.ErrorMessage = "Please select a property";
        //                ValSubmit.IsValid = false;
        //                return;
        //            }

        //            bool foundError = false;
        //            if (
        //                (Request.Form[txtCottageExtent.UniqueID] != null && Request.Form[txtCottageExtent.UniqueID] != "") ||
        //                (Request.Form[txtCottageRate.UniqueID] != null && Request.Form[txtCottageRate.UniqueID] != "")
        //                )
        //            {

        //                if (Request.Form[txtCottageExtent.UniqueID] == null || Request.Form[txtCottageExtent.UniqueID] == "" || Convert.ToDouble(Request.Form[txtCottageExtent.UniqueID]) == 0)
        //                {
        //                    valtxtCottageExtent.IsValid = false;
        //                    foundError = true;
        //                }
        //                if (Request.Form[txtCottageRate.UniqueID] == null || Request.Form[txtCottageRate.UniqueID] == "" || Convert.ToDouble(Request.Form[txtCottageRate.UniqueID]) == 0)
        //                {
        //                    valtxtCottageRate.IsValid = false;
        //                    foundError = true;
        //                }
        //            }

        //            if (
        //            (Request.Form[txtCombinedThatchValue.UniqueID] != null && Request.Form[txtCombinedThatchValue.UniqueID] != "")
        //            )
        //            {
        //                if (Convert.ToDouble(Request.Form[txtCombinedThatchValue.UniqueID]) == 0)
        //                {
        //                    valtxtCombinedThatchValue.IsValid = false;
        //                    foundError = true;
        //                }
        //            }

        //            if (foundError)
        //                return;

        //            if (m_MyController.m_ValuationExtDS == null)
        //            {
        //                m_MyController.m_ValuationExtDS = new Valuation();
        //                m_MyController.m_ValuationsImprovements = null;

        //            }
        //            if (m_MyController.m_ValuationExtDS.ValuationMainBuilding.Count > 0 && m_PageState == PageState.AddMain)
        //                m_MyController.m_ValuationExtDS.ValuationMainBuilding.Clear();
        //            Valuation.ValuationMainBuildingRow newMainBuildingRow;
        //            bool addedRow = false;
        //            if (m_MyController.m_ValuationExtDS.ValuationMainBuilding.Count == 0)
        //            {
        //                newMainBuildingRow = m_MyController.m_ValuationExtDS.ValuationMainBuilding.NewValuationMainBuildingRow();
        //                newMainBuildingRow.ValuationKey = -1;
        //                addedRow = true;
        //            }
        //            else
        //                newMainBuildingRow = m_MyController.m_ValuationExtDS.ValuationMainBuilding[0];
        //            newMainBuildingRow.ValuationRoofTypeKey = Convert.ToInt32(Request.Form[ddlMainBuildingRoofType.UniqueID]);
        //            newMainBuildingRow.Extent = Convert.ToDouble(Request.Form[txtMainBuildingExtent.UniqueID]);
        //            if (Request.Form[txtMainBuildingRate.UniqueID] != null && Request.Form[txtMainBuildingRate.UniqueID] != "")
        //                newMainBuildingRow.Rate = Convert.ToDouble(Request.Form[txtMainBuildingRate.UniqueID]);
        //            else
        //                newMainBuildingRow.SetRateNull();
        //            if (addedRow)
        //                m_MyController.m_ValuationExtDS.ValuationMainBuilding.AddValuationMainBuildingRow(newMainBuildingRow);

        //            if (
        //                (Request.Form[ddlCottageRoofType.UniqueID] != null && Request.Form[ddlCottageRoofType.UniqueID] != "-select-") ||
        //                (Request.Form[txtCottageExtent.UniqueID] != null && Request.Form[txtCottageExtent.UniqueID] != "") ||
        //                (Request.Form[txtCottageRate.UniqueID] != null && Request.Form[txtCottageRate.UniqueID] != "")
        //                )
        //            {
        //                if (m_MyController.m_ValuationExtDS.ValuationCottage.Count > 0 && m_PageState == PageState.AddMain)
        //                    m_MyController.m_ValuationExtDS.ValuationCottage.Clear();
        //                Valuation.ValuationCottageRow newCottageRow;
        //                addedRow = false;
        //                if (m_MyController.m_ValuationExtDS.ValuationCottage.Count == 0)
        //                {
        //                    newCottageRow = m_MyController.m_ValuationExtDS.ValuationCottage.NewValuationCottageRow();
        //                    addedRow = true;
        //                    newCottageRow.ValuationKey = -1;
        //                }
        //                else
        //                    newCottageRow = m_MyController.m_ValuationExtDS.ValuationCottage[0];
        //                if (Request.Form[ddlCottageRoofType.UniqueID] != null && Request.Form[ddlCottageRoofType.UniqueID] != "-select-")
        //                    newCottageRow.ValuationRoofTypeKey = Convert.ToInt32(Request.Form[ddlCottageRoofType.UniqueID]);
        //                else
        //                    newCottageRow.SetValuationRoofTypeKeyNull();
        //                if (Request.Form[txtCottageExtent.UniqueID] != null && Request.Form[txtCottageExtent.UniqueID] != "")
        //                    newCottageRow.Extent = Convert.ToDouble(Request.Form[txtCottageExtent.UniqueID]);
        //                else
        //                    newCottageRow.SetExtentNull();
        //                if (Request.Form[txtCottageRate.UniqueID] != null && Request.Form[txtCottageRate.UniqueID] != "")
        //                    newCottageRow.Rate = Convert.ToDouble(Request.Form[txtCottageRate.UniqueID]);
        //                else
        //                    newCottageRow.SetRateNull();
        //                if (addedRow)
        //                    m_MyController.m_ValuationExtDS.ValuationCottage.AddValuationCottageRow(newCottageRow);
        //            }
        //            else
        //            {
        //                if (m_MyController.m_ValuationExtDS.ValuationCottage.Count > 0)
        //                    m_MyController.m_ValuationExtDS.ValuationCottage[0].Delete();
        //            }

        //            if (
        //            (Request.Form[txtCombinedThatchValue.UniqueID] != null && Request.Form[txtCombinedThatchValue.UniqueID] != "")
        //            )
        //            {
        //                if (m_MyController.m_ValuationExtDS.ValuationCombinedThatch.Count > 0 && m_PageState == PageState.AddMain)
        //                    m_MyController.m_ValuationExtDS.ValuationCombinedThatch.Clear();
        //                Valuation.ValuationCombinedThatchRow newCombinedThatchRow = m_MyController.m_ValuationExtDS.ValuationCombinedThatch.NewValuationCombinedThatchRow();
        //                addedRow = false;
        //                if (m_MyController.m_ValuationExtDS.ValuationCombinedThatch.Count == 0)
        //                {
        //                    addedRow = true;
        //                    newCombinedThatchRow = m_MyController.m_ValuationExtDS.ValuationCombinedThatch.NewValuationCombinedThatchRow();
        //                    newCombinedThatchRow.ValuationKey = -1;
        //                }
        //                else
        //                    newCombinedThatchRow = m_MyController.m_ValuationExtDS.ValuationCombinedThatch[0];
        //                if (Request.Form[txtCombinedThatchValue.UniqueID] != null && Request.Form[txtCombinedThatchValue.UniqueID] != "")
        //                    newCombinedThatchRow.Value = Convert.ToDouble(Request.Form[txtCombinedThatchValue.UniqueID]);
        //                if (addedRow)
        //                    m_MyController.m_ValuationExtDS.ValuationCombinedThatch.AddValuationCombinedThatchRow(newCombinedThatchRow);
        //            }
        //            else
        //            {
        //                if (m_MyController.m_ValuationExtDS.ValuationCombinedThatch.Count > 0)
        //                    m_MyController.m_ValuationExtDS.ValuationCombinedThatch[0].Delete();
        //            }

        //            if (m_PageState == PageState.AddMain)
        //            {
        //                if (Request.Form[txtValuationEscalationPercentage.UniqueID] != null &&
        //                    Request.Form[txtValuationEscalationPercentage.UniqueID] != "")
        //                    m_MyController.m_ExtentValuationEscalationPercentage = Convert.ToDouble(Request.Form[txtValuationEscalationPercentage.UniqueID]);
        //                else
        //                    m_MyController.m_ExtentValuationEscalationPercentage = 20;

        //                if (Request.Form[ddlMainBuildingClassification.UniqueID] != null && Request.Form[ddlMainBuildingClassification.UniqueID] != "-select-")
        //                {
        //                    m_MyController.m_ExtentValuationClassification = Convert.ToInt32(Request.Form[ddlMainBuildingClassification.UniqueID]);
        //                }
        //            }
        //            else if (m_PageState == PageState.UpdateMain)
        //            {
        //                if (m_MyController.m_ValuationExtDS._Valuation != null &&
        //                    m_MyController.m_ValuationExtDS._Valuation.Count != 0)
        //                {
        //                    if (Request.Form[txtValuationEscalationPercentage.UniqueID] != null &&
        //                        Request.Form[txtValuationEscalationPercentage.UniqueID] != "")
        //                    {
        //                        m_MyController.m_ValuationExtDS._Valuation[0].ValuationEscalationPercentage = Convert.ToDouble(Request.Form[txtValuationEscalationPercentage.UniqueID]);
        //                        m_MyController.m_ExtentValuationEscalationPercentage = m_MyController.m_ValuationExtDS._Valuation[0].ValuationEscalationPercentage;
        //                    }
        //                    else
        //                    {
        //                        m_MyController.m_ValuationExtDS._Valuation[0].ValuationEscalationPercentage = 20;
        //                        m_MyController.m_ExtentValuationEscalationPercentage = 20;
        //                    }

        //                    if (Request.Form[ddlMainBuildingClassification.UniqueID] != null && Request.Form[ddlMainBuildingClassification.UniqueID] != "-select-")
        //                    {
        //                        m_MyController.m_ValuationExtDS._Valuation[0].ValuationClassificationKey = Convert.ToInt32(Request.Form[ddlMainBuildingClassification.UniqueID]);
        //                        m_MyController.m_ExtentValuationClassification = m_MyController.m_ValuationExtDS._Valuation[0].ValuationClassificationKey;
        //                    }
        //                }
        //            }

        //            m_MyController.Navigator.Navigate("Next");
        //            break;


        //        case PageState.AddAddition:
        //        case PageState.UpdateAddition:
        //            //if (m_MyController.m_AddOnUpdate == true)
        //            //{
        //            //    ValSubmit.IsValid = false;
        //            //    ValSubmit.ErrorMessage = "The Add operation needs to be completed or cancelled before continuing";
        //            //}
        //            //else
        //            {
        //                m_MyController.Navigator.Navigate("Next");
        //            }
        //            break;
        //    }


        //}


        //private void GetAddressPropertyAndFinKey(int addressKey, ref int propertyKey, ref int finKey)
        //{
        //    foreach (Property.PropertyRow row in m_MyController.PropertyDS._Property)
        //    {
        //        if (row.AddressKey == addressKey)
        //        {
        //            if (row.IsNull("PropertyKey"))
        //                propertyKey = -1;
        //            else
        //                propertyKey = row.PropertyKey;
        //            if (row.IsFinancialServiceKeyNull())
        //                finKey = -1;
        //            else
        //                finKey = row.FinancialServiceKey;
        //            return;
        //        }
        //    }
        //    finKey = -1;
        //    propertyKey = -1;

        //}



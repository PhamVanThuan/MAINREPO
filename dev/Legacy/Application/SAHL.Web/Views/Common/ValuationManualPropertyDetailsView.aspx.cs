using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common
{
    public partial class ManualValuationMainDwellingDetails : SAHLCommonBaseView, IValuationManualPropertyDetailsView
    {
        //private IEventList<IProperty> _properties;
        private IProperty _property;
        bool _showExtendedDetailsGrid;
        readonly ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

        private enum ExtendedDetailsGridColumnPositions
        {
            Key = 0,
            BuildingTypeDesc = 1,
            RoofTypeDesc = 2,
            Extent = 3,
            Rate = 4,
            ReplacementValue = 5,
            ReplacementValueText = 6,
            ImprovementTypeDesc = 7,
            ImprovementDate = 8,
            RoofTypeKey = 9,
            ImprovementTypeKey = 10,
            BuildingTypeKey = 11,
            Action = 12
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            txtMainBuildingExtent.Attributes["onkeyup"] = "calculateMainbuildingReplacementValue()" + txtMainBuildingExtent.Attributes["onkeyup"];
            txtMainBuildingRate.Attributes["onkeyup"] = "calculateMainbuildingReplacementValue()" + txtMainBuildingRate.Attributes["onkeyup"];

            txtCottageExtent.Attributes["onkeyup"] = "calculateCottageReplacementValue()" + txtCottageExtent.Attributes["onkeyup"];
            txtCottageRate.Attributes["onkeyup"] = "calculateCottageReplacementValue()" + txtCottageRate.Attributes["onkeyup"];

            txtCombinedOutbuildingsExtent.Attributes["onkeyup"] = "calculateOutbuildingReplacementValue()" + txtCombinedOutbuildingsExtent.Attributes["onkeyup"];
            txtCombinedOutbuildingsRate.Attributes["onkeyup"] = "calculateOutbuildingReplacementValue()" + txtCombinedOutbuildingsRate.Attributes["onkeyup"];

            ddlCottageRoofType.Attributes["onChange"] = "CottageRoofTypeChanged();" + ddlCottageRoofType.Attributes["onChange"];

            BindPropertyGrid();
            
            ExtendedDetailsGrid.Visible = _showExtendedDetailsGrid;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public IEventList<IProperty> Properties
        //{
        //    set { _properties = value; }
        //}

        public IProperty Property
        {
            set 
            {
                _property = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
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
            RemoveButton.Visible = false;
            BackButton.Visible = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void ShowPanelsForAddExtended()
        {
            AddButton.Visible = true;
            RemoveButton.Visible = true;
            SubmitButton.Visible = true;

            CancelButton.Visible = true;
            BackButton.Visible = true;

            trBuildingType.Visible = true;


        }
        /// <summary>
        /// 
        /// </summary>
        public void HidePanelsForAddExtended()
        {
            AddButton.Visible = false;
            RemoveButton.Visible = false;
            SubmitButton.Visible = false;

            CombinedOutbuildingsPanel.Visible = false;
            pnlImprovement.Visible = false;
            trBuildingType.Visible = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void ShowPanelForOutBuildingAdd()
        {
            CombinedOutbuildingsPanel.Visible = true;
            lblOutBuildingRoofType.Visible = false;
            lblCombinedOutbuildingsExtent.Visible = false;
            lblCombinedOutbuildingsRate.Visible = false;
            lblCombinedOutbuildingsReplaceValue.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowPanelForImprovementAdd()
        {
            pnlImprovement.Visible = true;
            lblImprovementType.Visible = false;
            lblImprovementDate.Visible = false;
        }

        protected void ddlBuildingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBuildingTypeOnSelectedIndexChanged != null)
                ddlBuildingTypeOnSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlBuildingType.SelectedValue));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowExtendedDetailsGrid
        {
            set { _showExtendedDetailsGrid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedGridRowItem
        {
            get
            {
                string selectedItem = "";
                if (ExtendedDetailsGrid.SelectedRow != null)
                {
                    int selectedIndex = ExtendedDetailsGrid.SelectedIndex;
                    int selectedBuildingTypeKey = Convert.ToInt32(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.BuildingTypeKey].Text);
                    int selectedKey = Convert.ToInt32(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.Key].Text);

                    if (selectedBuildingTypeKey == (int)SAHL.Common.Globals.ValuationBuildingType.Outbuilding)
                    {
                        int selectedRoofTypeKey = Convert.ToInt32(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.RoofTypeKey].Text);
                        double selectedExtent = Convert.ToDouble(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.Extent].Text);
                        double selectedRate = Convert.ToDouble(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.Rate].Text);

                        selectedItem = selectedIndex + "," + selectedKey + "," + selectedBuildingTypeKey + "," + selectedRoofTypeKey + "," + selectedExtent + "," + selectedRate;
                    }
                    else
                    {
                        int selectedImprovementTypeKey = Convert.ToInt32(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.ImprovementTypeKey].Text);
                        string selectedImprovementDate = ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.ImprovementDate].Text;
                        double selectedImprovementValue = Convert.ToDouble(ExtendedDetailsGrid.SelectedRow.Cells[(int)ExtendedDetailsGridColumnPositions.ReplacementValue].Text);

                        selectedItem = selectedIndex + "," + selectedKey + "," + selectedBuildingTypeKey + "," + selectedImprovementTypeKey + "," + selectedImprovementDate + "," + selectedImprovementValue;
                    }
                }

                return selectedItem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowUpdateButton()
        {
            AddButton.Visible = true;
            AddButton.Text = "Update";

            SubmitButton.Visible = false;
            BackButton.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowBackButton()
        {
            AddButton.Visible = false;
            SubmitButton.Visible = true;
            BackButton.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
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

            trBuildingType.Visible = false;

            CombinedImprovementType.Visible = false;
            CombinedOutbuildingRoofType.Visible = false;

            BackButton.Visible = false;
            AddButton.Visible = false;
            RemoveButton.Visible = false;

            pnlImprovement.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valManual"></param>
        /// <param name="valMainBuilding"></param>
        /// <param name="valCottage"></param>
        /// <param name="valCombinedThatch"></param>
        public void BindValuationDisplay(IValuationDiscriminatedSAHLManual valManual, IValuationMainBuilding valMainBuilding, IValuationCottage valCottage, double valCombinedThatchValue)
        {
            if (valManual == null)
                return;

            lblMainBuildingClassification.Text = valManual.ValuationClassification == null ? "-" : valManual.ValuationClassification.Description;

            double mainBuildingReplacementValue = 0;
            double mainBuildingExtent = 0;
            double mainBuildingRate = 0;
            if (valMainBuilding != null && valMainBuilding.Extent.HasValue && valMainBuilding.Extent.Value > 0)
            {
                lblMainBuildingRoofType.Text = valMainBuilding.ValuationRoofType.Description;

                mainBuildingExtent = valMainBuilding.Extent.HasValue ? valMainBuilding.Extent.Value : 0;
                lblMainBuildingExtent.Text = mainBuildingExtent.ToString(SAHL.Common.Constants.NumberFormat);

                mainBuildingRate = valMainBuilding.Rate.HasValue ? valMainBuilding.Rate.Value : 0;
                lblMainBuildingRate.Text = mainBuildingRate.ToString(SAHL.Common.Constants.CurrencyFormat);

                mainBuildingReplacementValue = mainBuildingExtent * mainBuildingRate;
                lblMainBuildingReplacementValue.Text = mainBuildingReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            double cottageReplacementValue = 0;
            double cottageExtent = 0;
            double cottageRate = 0;
            if (valCottage != null && valCottage.Extent.HasValue && valCottage.Extent.Value > 0)
            {
                lblCottageRoofType.Text = valCottage.ValuationRoofType == null ? "-" : valCottage.ValuationRoofType.Description;

                cottageExtent = valCottage.Extent.HasValue ? valCottage.Extent.Value : 0;
                lblCottageExtent.Text = cottageExtent == 0 ? "-" : cottageExtent.ToString(SAHL.Common.Constants.NumberFormat);

                cottageRate = valCottage.Rate.HasValue ? valCottage.Rate.Value : 0;
                lblCottageRate.Text = cottageRate == 0 ? "-" : cottageRate.ToString(SAHL.Common.Constants.CurrencyFormat);

                cottageReplacementValue = cottageExtent * cottageRate;
                lblCottageReplacementValue.Text = cottageReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            double outBuildingReplacementValue = 0;
            double outBuildingExtent = 0;
            double outBuildRate = 0;

            if (valManual.ValuationOutBuildings != null && valManual.ValuationOutBuildings.Count > 0)
            {
                for (int i = 0; i < valManual.ValuationOutBuildings.Count; i++)
                {
                    outBuildingReplacementValue += Convert.ToDouble(valManual.ValuationOutBuildings[i].Rate * valManual.ValuationOutBuildings[i].Extent);
                    outBuildingExtent += Convert.ToDouble(valManual.ValuationOutBuildings[i].Extent);
                }
                if (outBuildingExtent > 0)
                    outBuildRate = outBuildingReplacementValue / outBuildingExtent;
            }

            lblCombinedOutbuildingsReplaceValue.Text = outBuildingReplacementValue == 0 ? "-" : outBuildingReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCombinedOutbuildingsRate.Text = outBuildRate == 0 ? "-" : outBuildRate.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCombinedOutbuildingsExtent.Text = outBuildingExtent == 0 ? "-" : outBuildingExtent.ToString(SAHL.Common.Constants.NumberFormat);

            double ImprovementReplacementValue = 0;
            double improvementExtent = 0;

            for (int x = 0; x < valManual.ValuationImprovements.Count; x++)
            {
                ImprovementReplacementValue += Convert.ToDouble(valManual.ValuationImprovements[x].ImprovementValue);
            }

            lblCombinedImprovementsReplaceValue.Text = ImprovementReplacementValue == 0 ? "-" : ImprovementReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            TrImprovementDate.Visible = false;
            TrImprovementExt.Visible = false;
            TrImprovementRate.Visible = false;

            txtEscalation.Visible = false;
            double valuationEscalationPercentage = valManual.ValuationEscalationPercentage.HasValue ? valManual.ValuationEscalationPercentage.Value : 0;
            lblEscalation.Text = valuationEscalationPercentage.ToString(SAHL.Common.Constants.NumberFormat) + " %";

            double thatchValue = valCombinedThatchValue;
            double thatchValuePercentageCheck = 0;
            double sumReplacementValues = mainBuildingReplacementValue + cottageReplacementValue + outBuildingReplacementValue + ImprovementReplacementValue;

            if (thatchValue > 0)
            {
                lblCombinedThatchValue.Text = thatchValue.ToString(SAHL.Common.Constants.CurrencyFormat);

                // calculate the Thatch Value % Check
                // Thatch Value / (Main Building Value + Cottage Value + Outbuilding Value + Improvements Value)
                thatchValuePercentageCheck = thatchValue / sumReplacementValues;
                lblCombinedThatchValuePercCheck.Text = thatchValuePercentageCheck.ToString(SAHL.Common.Constants.RateFormat);

                // calculate the Thatch Extent Check
                // (Main Building Extent + Cottage Extent + Outbuilding Extent + Improvements Extent) * Thatch Value % Check
                double thatchValueExtentCheck = (mainBuildingExtent + cottageExtent + outBuildingExtent + improvementExtent) * thatchValuePercentageCheck;
                lblCombinedThatchExtCheck.Text = thatchValueExtentCheck.ToString(SAHL.Common.Constants.NumberFormat);
            }

            // calculate Total Conventional Value
            // (Main Building Value + Cottage Value + Outbuilding Value + Improvements Value - Thatch Value)
            double totalConventionalValue = (sumReplacementValues - thatchValue) * (1 + (valuationEscalationPercentage / 100));
            lblTotalConventionalValue.Text = totalConventionalValue.ToString(SAHL.Common.Constants.CurrencyFormat);

            // calculate Total Thatch Value
            double totalThatchValue = thatchValue * (1 + (valuationEscalationPercentage / 100));
            lblTotalThatchValue.Text = totalThatchValue.ToString(SAHL.Common.Constants.CurrencyFormat);

            // sum the Total Value
            double totalValue = totalConventionalValue + totalThatchValue;
            lblTotalValue.Text = totalValue.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valManual"></param>
        /// <param name="valMainBuilding"></param>
        /// <param name="valCottage"></param>
        public void BindUpdateableFields(IValuationDiscriminatedSAHLManual valManual, IValuationMainBuilding valMainBuilding, IValuationCottage valCottage)
        {

            if (valManual.ValuationClassification != null)
                ddlMainBuildingClassification.SelectedValue = valManual.ValuationClassification.Key.ToString();

            txtEscalation.Text = valManual.ValuationEscalationPercentage.ToString();

            if (valMainBuilding != null)
            {
                if (valMainBuilding.ValuationRoofType != null)
                    ddlMainBuildingRoofType.SelectedValue = valMainBuilding.ValuationRoofType.Key.ToString();

                double extent = valMainBuilding.Extent.HasValue ? valMainBuilding.Extent.Value : 0;
                txtMainBuildingExtent.Text = extent > 0 ? extent.ToString() : "";

                double rate = valMainBuilding.Rate.HasValue ? valMainBuilding.Rate.Value : 0;
                txtMainBuildingRate.Text = rate > 0 ? rate.ToString() : "";

                txtMainBuildingReplaceValue.Text = Convert.ToDouble((extent * rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            if (valCottage != null)
            {
                if (valCottage.ValuationRoofType != null)
                    ddlCottageRoofType.SelectedValue = valCottage.ValuationRoofType.Key.ToString();

                double extent = valCottage.Extent.HasValue ? valCottage.Extent.Value : 0;
                txtCottageExtent.Text = extent > 0 ? extent.ToString() : "";

                double rate = valCottage.Rate.HasValue ? valCottage.Rate.Value : 0;
                txtCottageRate.Text = rate > 0 ? rate.ToString() : "";

                txtCottageReplaceValue.Text = Convert.ToDouble((extent * rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valOutBuilding"></param>
        public void ShowValuationOutBuildingDisplayDetails(IValuationOutbuilding valOutBuilding)
        {
            CombinedOutbuildingsPanel.Visible = true;
            trBuildingType.Visible = true;

            txtCombinedOutbuildingsRate.Visible = false;
            txtCombinedOutbuildingsExtent.Visible = false;

            ddlBuildingType.SelectedValue = Convert.ToString((int)SAHL.Common.Globals.ValuationBuildingType.Outbuilding);
            ddlBuildingType.Enabled = false;
            lblOutBuildingRoofType.Text = valOutBuilding.ValuationRoofType.Description;
            ddlCombinedOutbuildingsRoofType.Visible = false;

            lblCombinedOutbuildingsExtent.Text = valOutBuilding.Extent.ToString();
            lblCombinedOutbuildingsRate.Text = valOutBuilding.Rate.ToString();
            txtCombinedOutbuildingsReplaceValue.Text = Convert.ToDouble((valOutBuilding.Extent * valOutBuilding.Rate)).ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCombinedOutbuildingsReplaceValue.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valImprovements"></param>
        public void ShowValuationImprovementDisplayDetails(IValuationImprovement valImprovements)
        {
            pnlImprovement.Visible = true;
            trBuildingType.Visible = true;

            ddlBuildingType.SelectedValue = Convert.ToString((int)SAHL.Common.Globals.ValuationBuildingType.Improvement);
            ddlBuildingType.Enabled = false;

            ddlImprovementType.Visible = false;
            dteImprovementDate.Visible = false;

            lblImprovementDate.Text = valImprovements.ImprovementDate != null ? Convert.ToDateTime(valImprovements.ImprovementDate).ToString(SAHL.Common.Constants.DateFormat) : "Unknown Date";

            lblImprovementType.Text = valImprovements.ValuationImprovementType.Description;
            txtImprovementReplacementValue.Text = Convert.ToDouble(valImprovements.ImprovementValue).ToString(SAHL.Common.Constants.CurrencyFormat);
            txtImprovementReplacementValue.Enabled = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void HideAllPanels()
        {
            MainBuildingPanel.Visible = false;
            CottagePanel.Visible = false;
            CombinedThatchPanel.Visible = false;
            tblValuation.Visible = false;
            CombinedImprovementsPanel.Visible = false;
            pnlImprovement.Visible = false;
            CombinedOutbuildingsPanel.Visible = false;
            trBuildingType.Visible = false;
        }

        private void BindPropertyGrid()
        {
            IEventList<IAddress> lstPropertyAddresses = new EventList<IAddress>();
            if (_property != null)
                lstPropertyAddresses.Add(this.Messages, _property.Address);
            PropertyAddressGrid.HeaderCaption = "Property Details";
            PropertyAddressGrid.EffectiveDateColumnVisible = false;
            PropertyAddressGrid.FormatColumnVisible = false;
            PropertyAddressGrid.StatusColumnVisible = false;
            PropertyAddressGrid.TypeColumnVisible = false;

            PropertyAddressGrid.GridHeight = 90;

            PropertyAddressGrid.BindAddressList(lstPropertyAddresses);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindExtendedDetailsGrid(List<ValuationExtendedDetails> lstValuationExtendedDetails)
        {
            ExtendedDetailsGrid.Columns.Clear();
            ExtendedDetailsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(14), HorizontalAlign.Left, false);
            ExtendedDetailsGrid.AddGridBoundColumn("", "Building Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ExtendedDetailsGrid.AddGridBoundColumn("", "Roof Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ExtendedDetailsGrid.AddGridBoundColumn("", SAHL.Common.Constants.NumberFormat, GridFormatType.GridNumber, "Extent (sq meter)", false, Unit.Percentage(14), HorizontalAlign.Left, true);
            ExtendedDetailsGrid.AddGridBoundColumn("", SAHL.Common.Constants.NumberFormat, GridFormatType.GridString, "Rate (R/sq meter)", false, Unit.Percentage(14), HorizontalAlign.Left, true);
            ExtendedDetailsGrid.AddGridBoundColumn("", SAHL.Common.Constants.NumberFormat, GridFormatType.GridString, "Replacement Value Number", false, Unit.Percentage(16), HorizontalAlign.Left, false);
            ExtendedDetailsGrid.AddGridBoundColumn("", "Replacement Value", Unit.Percentage(16), HorizontalAlign.Left, true);
            ExtendedDetailsGrid.AddGridBoundColumn("", "Improvement Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ExtendedDetailsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Improvement Date", false, Unit.Percentage(14), HorizontalAlign.Center, true);

            // non display items
            ExtendedDetailsGrid.AddGridBoundColumn("RoofTypeKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            ExtendedDetailsGrid.AddGridBoundColumn("ImprovementTypeKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            ExtendedDetailsGrid.AddGridBoundColumn("BuildingTypeKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            ExtendedDetailsGrid.AddGridBoundColumn("Action", "", Unit.Percentage(0), HorizontalAlign.Left, false);

            ExtendedDetailsGrid.DataSource = lstValuationExtendedDetails;

            ExtendedDetailsGrid.DataBind();

            if (lstValuationExtendedDetails != null && lstValuationExtendedDetails.Count > 0)
                RemoveButton.Enabled = true;
            else
                RemoveButton.Enabled = false;
        }

        protected void ExtendedDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ValuationExtendedDetails valuationExtendedDetails = e.Row.DataItem as ValuationExtendedDetails;

                e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.BuildingTypeDesc].Text = Enum.GetName(typeof(ValuationBuildingType),valuationExtendedDetails.BuildingTypeKey);
                e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.Action].Text = valuationExtendedDetails.Action.ToString();

                if (valuationExtendedDetails.BuildingTypeKey == (int)ValuationBuildingType.Outbuilding)
                {
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.RoofTypeDesc].Text = valuationExtendedDetails.RoofTypeDesc;
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.Extent].Text = valuationExtendedDetails.Extent.HasValue ? valuationExtendedDetails.Extent.Value.ToString() : "";
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.Rate].Text = valuationExtendedDetails.Rate.HasValue ? valuationExtendedDetails.Rate.Value.ToString() : "";
                    double replacementvalue = valuationExtendedDetails.Extent.HasValue && valuationExtendedDetails.Rate.HasValue ? valuationExtendedDetails.Extent.Value * valuationExtendedDetails.Rate.Value : 0;
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.ReplacementValue].Text = replacementvalue > 0 ? replacementvalue.ToString() : "";
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.ReplacementValueText].Text = replacementvalue > 0 ? replacementvalue.ToString(SAHL.Common.Constants.CurrencyFormat) : "";
                }
                else
                {
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.ImprovementTypeDesc].Text = valuationExtendedDetails.ImprovementTypeDesc;
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.ImprovementDate].Text = valuationExtendedDetails.ImprovementDate.HasValue ? valuationExtendedDetails.ImprovementDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.ReplacementValue].Text = valuationExtendedDetails.ImprovementValue.ToString();
                    e.Row.Cells[(int)ExtendedDetailsGridColumnPositions.ReplacementValueText].Text = valuationExtendedDetails.ImprovementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
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
            if (OnAddButtonClicked != null)
                OnAddButtonClicked(sender, e);
        }

        protected void Remove_ButtonClick(object sender, EventArgs e)
        {
            if (OnRemoveButtonClicked != null)
                OnRemoveButtonClicked(sender, new KeyChangedEventArgs(SelectedGridRowItem));
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnBackButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnAddButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnRemoveButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler ddlBuildingTypeOnSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hocRoof"></param>
        public void BindCottageRoofTypes(IEventList<IValuationRoofType> hocRoof)
        {
            ddlCottageRoofType.DataTextField = "Description";
            ddlCottageRoofType.DataValueField = "Key";
            ddlCottageRoofType.DataSource = hocRoof;
            ddlCottageRoofType.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hocRoof"></param>
        public void BindMainBuildingRoofType(IEventList<IValuationRoofType> hocRoof)
        {
            ddlMainBuildingRoofType.DataTextField = "Description";
            ddlMainBuildingRoofType.DataValueField = "Key";
            ddlMainBuildingRoofType.DataSource = hocRoof;
            ddlMainBuildingRoofType.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuationClassification"></param>
        public void BindBuildingClassification(IEventList<IValuationClassification> valuationClassification)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valImprovementTypes"></param>
        public void BindImprovementType(IEventList<IValuationImprovementType> valImprovementTypes)
        {
            ddlImprovementType.DataTextField = "Description";
            ddlImprovementType.DataValueField = "Key";
            ddlImprovementType.DataSource = valImprovementTypes;
            ddlImprovementType.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valMainBuilding"></param>
        /// <returns></returns>
        public IValuationMainBuilding GetValuationMainBuilding(IValuationMainBuilding valMainBuilding)
        {
            // if no values have been entered, return null object
            if (ddlMainBuildingRoofType.SelectedValue == "-select-" && ddlMainBuildingRoofType.Text.Length == 0 && ddlMainBuildingRoofType.Text.Length == 0)
                return null;

            if (ddlMainBuildingRoofType.SelectedValue != "-select-")
                valMainBuilding.ValuationRoofType = lookups.ValuationRoofTypes.ObjectDictionary[ddlMainBuildingRoofType.SelectedValue];
            else
                valMainBuilding.ValuationRoofType = null;

            if (txtMainBuildingExtent.Text.Length > 0)
                valMainBuilding.Extent = Convert.ToDouble(txtMainBuildingExtent.Text);
            else
                valMainBuilding.Extent = 0;

            if (txtMainBuildingRate.Text.Length > 0)
                valMainBuilding.Rate = Convert.ToDouble(txtMainBuildingRate.Text);
            else
                valMainBuilding.Rate = 0;

            return valMainBuilding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valCottage"></param>
        /// <returns></returns>
        public IValuationCottage GetValuationCottage(IValuationCottage valCottage)
        {
            // if no values have been entered, return null object
            if (ddlCottageRoofType.SelectedValue == "-select-" && txtCottageExtent.Text.Length == 0 && txtCottageRate.Text.Length == 0)
                return null;

            if (ddlCottageRoofType.SelectedValue != "-select-")
                valCottage.ValuationRoofType = lookups.ValuationRoofTypes.ObjectDictionary[ddlCottageRoofType.SelectedValue];
            else
                valCottage.ValuationRoofType = null;

            if (txtCottageExtent.Text.Length > 0)
                valCottage.Extent = Convert.ToDouble(txtCottageExtent.Text);
            else
                valCottage.Extent = 0;

            if (txtCottageRate.Text.Length > 0)
                valCottage.Rate = Convert.ToDouble(txtCottageRate.Text);
            else
                valCottage.Rate = 0;

            return valCottage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valSAHLManual"></param>
        /// <returns></returns>
        public IValuationDiscriminatedSAHLManual GetValuationManual(IValuationDiscriminatedSAHLManual valSAHLManual)
        {

            if (ddlMainBuildingClassification.SelectedValue != "-select-")
                valSAHLManual.ValuationClassification = lookups.ValuationClassification.ObjectDictionary[ddlMainBuildingClassification.SelectedValue];
            if (txtEscalation.Text.Length > 0)
                valSAHLManual.ValuationEscalationPercentage = Convert.ToDouble(txtEscalation.Text);
            valSAHLManual.ValuationUserID = CurrentPrincipal.Identity.Name;
            return valSAHLManual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valImprovement"></param>
        /// <returns></returns>
        public IValuationImprovement GetValuationImprovement(IValuationImprovement valImprovement)
        {
            if (ddlImprovementType.SelectedValue != "-select-")
                valImprovement.ValuationImprovementType = lookups.ValuationImprovementType.ObjectDictionary[ddlImprovementType.SelectedValue];
            else
                valImprovement.ValuationImprovementType = null;

            if (txtImprovementReplacementValue.Text.Length > 0)
                valImprovement.ImprovementValue = Convert.ToDouble(txtImprovementReplacementValue.Text);
            else
                valImprovement.ImprovementValue = 0;

            valImprovement.ImprovementDate = dteImprovementDate.Date;

            return valImprovement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valOutbuilding"></param>
        /// <returns></returns>
        public IValuationOutbuilding GetCapturedOutbuilding(IValuationOutbuilding valOutbuilding)
        {
            if (ddlCombinedOutbuildingsRoofType.SelectedValue != "-select-")
                valOutbuilding.ValuationRoofType = lookups.ValuationRoofTypes.ObjectDictionary[ddlCombinedOutbuildingsRoofType.SelectedValue];
            else
                valOutbuilding.ValuationRoofType = null;
            
            if (txtCombinedOutbuildingsExtent.Text.Length > 0)
                valOutbuilding.Extent = Convert.ToDouble(txtCombinedOutbuildingsExtent.Text);
            else
                valOutbuilding.Extent = 0;

            if (txtCombinedOutbuildingsRate.Text.Length > 0)
                valOutbuilding.Rate = Convert.ToDouble(txtCombinedOutbuildingsRate.Text);
            else
                valOutbuilding.Rate = 0;

            return valOutbuilding;
        }

    }
}




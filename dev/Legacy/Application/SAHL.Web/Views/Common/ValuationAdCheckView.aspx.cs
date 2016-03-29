using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationAdCheckView : SAHLCommonBaseView, IValuationAdCheckView
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler TabsActiveTabChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnBackClicked;

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Return to Default Screen
            if (btnBackClicked != null)
                btnBackClicked(sender, e);
        }


        protected void Tabs_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabsActiveTabChanged != null)
                TabsActiveTabChanged(sender, e);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdConditions(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdConditions.AutoGenerateColumns = false;
                grdConditions.AddGridBoundColumn("Description", "Description", Unit.Percentage(100), HorizontalAlign.Left,
                                                 true);
                grdConditions.DataSource = DT;
                grdConditions.DataBind();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdConditionComments(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdConditionComments.AutoGenerateColumns = false;
                grdConditionComments.AddGridBoundColumn("description", "Description", Unit.Percentage(100), HorizontalAlign.Left, true);
                grdConditionComments.DataSource = DT;
                grdConditionComments.DataBind();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdComparativeProperties(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdComparativeProperties.AutoGenerateColumns = false;
                grdComparativeProperties.AddGridBoundColumn("AssesmentDate", "Assessment Date", Unit.Percentage(20), HorizontalAlign.Left, false);
                grdComparativeProperties.AddGridBoundColumn("PurchaseValue", "Purchase Price", Unit.Percentage(30), HorizontalAlign.Left, true);
                grdComparativeProperties.AddGridBoundColumn("StandNo", "Stand No", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdComparativeProperties.AddGridBoundColumn("Suburb", "Suburb", Unit.Percentage(30), HorizontalAlign.Left, true);
                grdComparativeProperties.DataSource = DT;
                grdComparativeProperties.DataBind();
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdImprovementResults(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdImprovementResults.AutoGenerateColumns = false;
                grdImprovementResults.AddGridBoundColumn("number", "", Unit.Percentage(25), HorizontalAlign.Left, false);
                grdImprovementResults.AddGridBoundColumn("description", "Description", Unit.Percentage(25), HorizontalAlign.Left, true);
                grdImprovementResults.AddGridBoundColumn("rooftype", "Roof Type", Unit.Percentage(25), HorizontalAlign.Left, true);
                grdImprovementResults.AddGridBoundColumn("value", "Value", Unit.Percentage(25), HorizontalAlign.Left,true);
                grdImprovementResults.DataSource = DT;
                grdImprovementResults.DataBind();

            }
        }



        //***********************************************************************************
        // SHOW / HIDE SCREEN OBJECTS

        /// <summary>
        /// Gets or Sets the Active Tab Index for the Main Tab Control
        /// </summary>
        public int SetTabsActiveTabIndex
        {
            set { Tabs.ActiveTabIndex = value; }
            get { return Tabs.ActiveTabIndex; }
        }


        ////***********************************************************************************


        /// <summary>
        /// Method to set the 'lblAssessmentNumberValue' text property
        /// </summary>
        public string SetlblAssessmentNumberValue
        {
            set { lblAssessmentNumberValue.Text = value; }
        }


        /// <summary>
        /// Method to set the 'lblRequestNumberValue' text property
        /// </summary>
        public string SetlblRequestNumberValue
        {
            set { lblRequestNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblAssessmentReasonValue' text property
        /// </summary>
        public string SetlblAssessmentReasonValue
        {
            set { lblAssessmentReasonValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblRequestedbyValue' text property
        /// </summary>
        public string SetlblRequestedbyValue
        {
            set { lblRequestedbyValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCorrectPropertyValue' text property
        /// </summary>
        public string SetlblCorrectPropertyValue
        {
            set { lblCorrectPropertyValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCorrectAddressValue' text property
        /// </summary>
        public string SetlblCorrectAddressValue
        {
            set { lblCorrectAddressValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblValueAsIsValue' text property
        /// </summary>
        public string SetlblValueAsIsValue
        {
            set { lblValueAsIsValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblCostToCompleteValue' text property
        /// </summary>
        public string SetlblCostToCompleteValue
        {
            set { lblCostToCompleteValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblValueOnCompletionValue' text property
        /// </summary>
        public string SetlblValueOnCompletionValue
        {
            set { lblValueOnCompletionValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblClassificationValue' text property
        /// </summary>
        public string SetlblClassificationValue
        {
            set { lblClassificationValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblRoofTypeValue' text property
        /// </summary>
        public string SetlblRoofTypeValue
        {
            set { lblRoofTypeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingExtentValue' text property
        /// </summary>
        public string SetlblMainBuildingExtentValue
        {
            set { lblMainBuildingExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainbuildingRateValue' text property
        /// </summary>
        public string SetlblMainbuildingRateValue
        {
            set { lblMainbuildingRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingValue' text property
        /// </summary>
        public string SetlblMainBuildingValue
        {
            set { lblMainBuildingValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageExtentValue' text property
        /// </summary>
        public string SetlblCottageExtentValue
        {
            set { lblCottageExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageRateValue' text property
        /// </summary>
        public string SetlblCottageRateValue
        {
            set { lblCottageRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageReplacementValue' text property
        /// </summary>
        public string SetlblCottageReplacementValue
        {
            set { lblCottageReplacementValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingExtentValue' text property
        /// </summary>
        public string SetlblOutbuildingExtentValue
        {
            set { lblOutbuildingExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingRateValue' text property
        /// </summary>
        public string SetlblOutbuildingRateValue
        {
            set { lblOutbuildingRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingReplacementValue' text property
        /// </summary>
        public string SetlblOutbuildingReplacementValue
        {
            set { lblOutbuildingReplacementValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblThatchValue' text property
        /// </summary>
        public string SetlblThatchValue
        {
            set { lblThatchValue.Text = value; }
        }


        /// <summary>
        /// Method to set the 'lblThatchValueCheckValue' text property
        /// </summary>
        public string SetlblThatchValueCheckValue
        {
            set { lblThatchValueCheckValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblThatchExtentCheckValue' text property
        /// </summary>
        public string SetlblThatchExtentCheckValue
        {
            set { lblThatchExtentCheckValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblThatchMinimumDistanceValue' text property
        /// </summary>
        public string SetlblThatchMinimumDistanceValue
        {
            set { lblThatchMinimumDistanceValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblImprovementSummaryValue' text property
        /// </summary>
        public string SetlblImprovementSummaryValue
        {
            set { lblImprovementSummaryValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblImprovementValue' text property
        /// </summary>
        public string SetlblImprovementValue
        {
            set { lblImprovementValue.Text = value; }
        }


        /// <summary>
        /// Method to set the 'txtComment' text property
        /// </summary>
        public string SettxtComment
        {
            set { txtComment.Text = value; }
        }


        /// <summary>
        /// Method to set the 'lblStreetAddressValue' text property
        /// </summary>
        public string SetlblStreetAddressValue
        {
            set { lblStreetAddressValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblSuburbValue' text property
        /// </summary>
        public string SetlblSuburbValue
        {
            set { lblSuburbValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCityValue' text property
        /// </summary>
        public string SetlblCityValue
        {
            set { lblCityValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblProvinceValue' text property
        /// </summary>
        public string SetlblProvinceValue
        {
            set { lblProvinceValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCountryValue' text property
        /// </summary>
        public string SetlblCountryValue
        {
            set { lblCountryValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPostalCodeValue' text property
        /// </summary>
        public string SetlblPostalCodeValue
        {
            set { lblPostalCodeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblStandNumberValue' text property
        /// </summary>
        public string SetlblStandNumberValue
        {
            set { lblStandNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblStreetNumberValue' text property
        /// </summary>
        public string SetlblStreetNumberValue
        {
            set { lblStreetNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblStreetNameValue' text property
        /// </summary>
        public string SetlblStreetNameValue
        {
            set { lblStreetNameValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblAreaTypeValue' text property
        /// </summary>
        public string SetlblAreaTypeValue
        {
            set { lblAreaTypeValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblSectorTypeValue' text property
        /// </summary>
        public string SetlblSectorTypeValue
        {
            set { lblSectorTypeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblLocalityCommentValue' text property
        /// </summary>
        public string SetlblLocalityCommentValue
        {
            set { lblLocalityCommentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMarketCommentValue' text property
        /// </summary>
        public string SetlblMarketCommentValue
        {
            set { lblMarketCommentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblRetentionReasonValue' text property
        /// </summary>
        public string SetlblRetentionReasonValue
        {
            set { lblRetentionReasonValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblRetentionAmountValue' text property
        /// </summary>
        public string SetlblRetentionAmountValue
        {
            set { lblRetentionAmountValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblERFTypeValue' text property
        /// </summary>
        public string SetlblERFTypeValue
        {
            set { lblERFTypeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblERFNumberValue' text property
        /// </summary>
        public string SetlblERFNumberValue
        {
            set { lblERFNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPortionValue' text property
        /// </summary>
        public string SetlblPortionValue
        {
            set { lblPortionValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPortionOfPortionValue' text property
        /// </summary>
        public string SetlblPortionOfPortionValue
        {
            set { lblPortionOfPortionValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblSubdivisionPortionValue' text property
        /// </summary>
        public string SetlblSubdivisionPortionValue
        {
            set { lblSubdivisionPortionValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblLandSizeValue' text property
        /// </summary>
        public string SetlblLandSizeValue
        {
            set { lblLandSizeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblSectionNumberValue' text property
        /// </summary>
        public string SetlblSectionNumberValue
        {
            set { lblSectionNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'txtLegalDescriptionValue' text property
        /// </summary>
        public string SettxtLegalDescriptionValue
        {
            set { txtLegalDescriptionValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPropertyUseValue' text property
        /// </summary>
        public string SetlblPropertyUseValue
        {
            set { lblPropertyUseValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPropertyTypeValue' text property
        /// </summary>
        public string SetlblPropertyTypeValue
        {
            set { lblPropertyTypeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPropertyCommentValue' text property
        /// </summary>
        public string SetlblPropertyCommentValue
        {
            set { lblPropertyCommentValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblFlatNumberValue' text property
        /// </summary>
        public string SetlblFlatNumberValue
        {
            set { lblFlatNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblUnitSizeValue' text property
        /// </summary>
        public string SetlblUnitSizeValue
        {
            set { lblUnitSizeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblComplexNameValue' text property
        /// </summary>
        public string SetlblComplexNameValue
        {
            set { lblComplexNameValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblFloorsInComplexValue' text property
        /// </summary>
        public string SetlblFloorsInComplexValue
        {
            set { lblFloorsInComplexValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblUnitsInComplexValue' text property
        /// </summary>
        public string SetlblUnitsInComplexValue
        {
            set { lblUnitsInComplexValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblBathRoomCountValue' text property
        /// </summary>
        public string SetlblBathRoomCountValue
        {
            set { lblBathRoomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblStudyCountValue' text property
        /// </summary>
        public string SetlblStudyCountValue
        {
            set { lblStudyCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblFamilyRoomCountValue' text property
        /// </summary>
        public string SetlblFamilyRoomCountValue
        {
            set { lblFamilyRoomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblEntranceHallCountValue' text property
        /// </summary>
        public string SetlblEntranceHallCountValue
        {
            set { lblEntranceHallCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblLaundryRoomCountValue' text property
        /// </summary>
        public string SetlblLaundryRoomCountValue
        {
            set { lblLaundryRoomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblDiningCountValue' text property
        /// </summary>
        public string SetlblDiningCountValue
        {
            set { lblDiningCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblLoungeCountValue' text property
        /// </summary>
        public string SetlblLoungeCountValue
        {
            set { lblLoungeCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingBedroomCountValue' text property
        /// </summary>
        public string SetlblMainBuildingBedroomCountValue
        {
            set { lblMainBuildingBedroomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingKitchenCountValue' text property
        /// </summary>
        public string SetlblMainBuildingKitchenCountValue
        {
            set { lblMainBuildingKitchenCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblPantryCountValue' text property
        /// </summary>
        public string SetlblPantryCountValue
        {
            set { lblPantryCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingWCCountValue' text property
        /// </summary>
        public string SetlblMainBuildingWCCountValue
        {
            set { lblMainBuildingWCCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOtherRoomCountValue' text property
        /// </summary>
        public string SetlblOtherRoomCountValue
        {
            set { lblOtherRoomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingThatchRateValue' text property
        /// </summary>
        public string SetlblMainBuildingThatchRateValue
        {
            set { lblMainBuildingThatchRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingThatchExtentValue' text property
        /// </summary>
        public string SetlblMainBuildingThatchExtentValue
        {
            set { lblMainBuildingThatchExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingConventionalRateValue' text property
        /// </summary>
        public string SetlblMainBuildingConventionalRateValue
        {
            set { lblMainBuildingConventionalRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblMainBuildingConventionalExtentValue' text property
        /// </summary>
        public string SetlblMainBuildingConventionalExtentValue
        {
            set { lblMainBuildingConventionalExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingBathroomCountValue' text property
        /// </summary>
        public string SetlblOutbuildingBathroomCountValue
        {
            set { lblOutbuildingBathroomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingWorkshopCountValue' text property
        /// </summary>
        public string SetlblOutbuildingWorkshopCountValue
        {
            set { lblOutbuildingWorkshopCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingLaundryCountValue' text property
        /// </summary>
        public string SetlblOutbuildingLaundryCountValue
        {
            set { lblOutbuildingLaundryCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingGarageCountValue' text property
        /// </summary>
        public string SetlblOutbuildingGarageCountValue
        {
            set { lblOutbuildingGarageCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingBedroomCountValue' text property
        /// </summary>
        public string SetlblOutbuildingBedroomCountValue
        {
            set { lblOutbuildingBedroomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingStoreRoomcountValue' text property
        /// </summary>
        public string SetlblOutbuildingStoreRoomcountValue
        {
            set { lblOutbuildingStoreRoomcountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingKitchenCountValue' text property
        /// </summary>
        public string SetlblOutbuildingKitchenCountValue
        {
            set { lblOutbuildingKitchenCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingWCCountValue' text property
        /// </summary>
        public string SetlblOutbuildingWCCountValue
        {
            set { lblOutbuildingWCCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingThatchRateValue' text property
        /// </summary>
        public string SetlblOutbuildingThatchRateValue
        {
            set { lblOutbuildingThatchRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingThatchExtentValue' text property
        /// </summary>
        public string SetlblOutbuildingThatchExtentValue
        {
            set { lblOutbuildingThatchExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingConventionalRateValue' text property
        /// </summary>
        public string SetlblOutbuildingConventionalRateValue
        {
            set { lblOutbuildingConventionalRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblOutbuildingConventionalExtentValue' text property
        /// </summary>
        public string SetlblOutbuildingConventionalExtentValue
        {
            set { lblOutbuildingConventionalExtentValue.Text = value; }
        }



        /// <summary>
        /// Method to set the 'lblSCottageCottageStandNumberValue' text property
        /// </summary>
        public string SetlblSCottageCottageStandNumberValue
        {
            set { lblSCottageCottageStandNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblBedroomCountValue' text property
        /// </summary>
        public string SetlblBedroomCountValue
        {
            set { lblBedroomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageKitchenCountValue' text property
        /// </summary>
        public string SetlblCottageKitchenCountValue
        {
            set { lblCottageKitchenCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageLivingRoomCountValue' text property
        /// </summary>
        public string SetlblCottageLivingRoomCountValue
        {
            set { lblCottageLivingRoomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageOtherRoomCountValue' text property
        /// </summary>
        public string SetlblCottageOtherRoomCountValue
        {
            set { lblCottageOtherRoomCountValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageThatchRateValue' text property
        /// </summary>
        public string SetlblCottageThatchRateValue
        {
            set { lblCottageThatchRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageThatchExtentValue' text property
        /// </summary>
        public string SetlblCottageThatchExtentValue
        {
            set { lblCottageThatchExtentValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageConventionalRateValue' text property
        /// </summary>
        public string SetlblCottageConventionalRateValue
        {
            set { lblCottageConventionalRateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblCottageConventionalExtentValue' text property
        /// </summary>
        public string SetlblCottageConventionalExtentValue
        {
            set { lblCottageConventionalExtentValue.Text = value; }
        }


        /// <summary>
        /// Method to set the 'lblSwimmingPoolValue' text property
        /// </summary>
        public string SetlblSwimmingPoolValue
        {
            set { lblSwimmingPoolValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblSwimmingPoolTypeValue' text property
        /// </summary>
        public string SetlblSwimmingPoolTypeValue
        {
            set { lblSwimmingPoolTypeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblTennisCourtValue' text property
        /// </summary>
        public string SetlblTennisCourtValue
        {
            set { lblTennisCourtValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblTennisCourtTypeValue' text property
        /// </summary>
        public string SetlblTennisCourtTypeValue
        {
            set { lblTennisCourtTypeValue.Text = value; }
        }






        //***********************************************************************************
    }
}

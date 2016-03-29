using System;
using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IValuationAdCheckView : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event EventHandler TabsActiveTabChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnBackClicked;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdConditions(DataTable DT);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdComparativeProperties(DataTable DT);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdConditionComments(DataTable DT);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdImprovementResults(DataTable DT);

        //***********************************************************************************
        // SHOW / HIDE SCREEN OBJECTS

        /// <summary>
        /// Gets or Sets the Active Tab Index for the Main Tab Control
        /// </summary>
        int SetTabsActiveTabIndex { set;  get;}



        ////***********************************************************************************


        /// <summary>
        /// Method to set the 'lblAssessmentNumberValue' text property
        /// </summary>
        string SetlblAssessmentNumberValue { set;}


        /// <summary>
        /// Method to set the 'lblRequestNumberValue' text property
        /// </summary>
        string SetlblRequestNumberValue { set; }

        /// <summary>
        /// Method to set the 'lblAssessmentReasonValue' text property
        /// </summary>
        string SetlblAssessmentReasonValue { set; }

        /// <summary>
        /// Method to set the 'lblRequestedbyValue' text property
        /// </summary>
        string SetlblRequestedbyValue { set;  }

        /// <summary>
        /// Method to set the 'lblCorrectPropertyValue' text property
        /// </summary>
        string SetlblCorrectPropertyValue { set; }

        /// <summary>
        /// Method to set the 'lblCorrectAddressValue' text property
        /// </summary>
        string SetlblCorrectAddressValue { set;  }

        /// <summary>
        /// Method to set the 'lblValueAsIsValue' text property
        /// </summary>
        string SetlblValueAsIsValue { set; }


        /// <summary>
        /// Method to set the 'lblCostToCompleteValue' text property
        /// </summary>
        string SetlblCostToCompleteValue { set; }


        /// <summary>
        /// Method to set the 'lblValueOnCompletionValue' text property
        /// </summary>
        string SetlblValueOnCompletionValue { set; }

        /// <summary>
        /// Method to set the 'lblClassificationValue' text property
        /// </summary>
        string SetlblClassificationValue { set;  }

        /// <summary>
        /// Method to set the 'lblRoofTypeValue' text property
        /// </summary>
        string SetlblRoofTypeValue { set;}

        /// <summary>
        /// Method to set the 'lblMainBuildingExtentValue' text property
        /// </summary>
        string SetlblMainBuildingExtentValue { set; }

        /// <summary>
        /// Method to set the 'lblMainbuildingRateValue' text property
        /// </summary>
        string SetlblMainbuildingRateValue { set; }

        /// <summary>
        /// Method to set the 'lblMainBuildingValue' text property
        /// </summary>
        string SetlblMainBuildingValue { set;}

        /// <summary>
        /// Method to set the 'lblCottageExtentValue' text property
        /// </summary>
        string SetlblCottageExtentValue { set; }

        /// <summary>
        /// Method to set the 'lblCottageRateValue' text property
        /// </summary>
        string SetlblCottageRateValue { set; }

        /// <summary>
        /// Method to set the 'lblCottageReplacementValue' text property
        /// </summary>
        string SetlblCottageReplacementValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingExtentValue' text property
        /// </summary>
        string SetlblOutbuildingExtentValue { set; }

        /// <summary>
        /// Method to set the 'lblOutbuildingRateValue' text property
        /// </summary>
        string SetlblOutbuildingRateValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingReplacementValue' text property
        /// </summary>
        string SetlblOutbuildingReplacementValue { set; }

        /// <summary>
        /// Method to set the 'lblThatchValue' text property
        /// </summary>
        string SetlblThatchValue { set;}

        /// <summary>
        /// Method to set the 'lblThatchValueCheckValue' text property
        /// </summary>
        string SetlblThatchValueCheckValue { set;}

        /// <summary>
        /// Method to set the 'lblThatchExtentCheckValue' text property
        /// </summary>
        string SetlblThatchExtentCheckValue { set;}

        /// <summary>
        /// Method to set the 'lblThatchMinimumDistanceValue' text property
        /// </summary>
        string SetlblThatchMinimumDistanceValue { set;}

        /// <summary>
        /// Method to set the 'lblImprovementSummaryValue' text property
        /// </summary>
        string SetlblImprovementSummaryValue { set;}

        /// <summary>
        /// Method to set the 'lblImprovementValue' text property
        /// </summary>
        string SetlblImprovementValue { set;}


        /// <summary>
        /// Method to set the 'txtComment' text property
        /// </summary>
        string SettxtComment { set;}


        /// <summary>
        /// Method to set the 'lblStreetAddressValue' text property
        /// </summary>
        string SetlblStreetAddressValue { set; }

        /// <summary>
        /// Method to set the 'lblSuburbValue' text property
        /// </summary>
        string SetlblSuburbValue { set; }

        /// <summary>
        /// Method to set the 'lblCityValue' text property
        /// </summary>
        string SetlblCityValue { set; }

        /// <summary>
        /// Method to set the 'lblProvinceValue' text property
        /// </summary>
        string SetlblProvinceValue { set; }

        /// <summary>
        /// Method to set the 'lblCountryValue' text property
        /// </summary>
        string SetlblCountryValue { set;}

        /// <summary>
        /// Method to set the 'lblPostalCodeValue' text property
        /// </summary>
        string SetlblPostalCodeValue { set; }

        /// <summary>
        /// Method to set the 'lblStandNumberValue' text property
        /// </summary>
        string SetlblStandNumberValue { set;}

        /// <summary>
        /// Method to set the 'lblStreetNumberValue' text property
        /// </summary>
        string SetlblStreetNumberValue { set;}

        /// <summary>
        /// Method to set the 'lblStreetNameValue' text property
        /// </summary>
        string SetlblStreetNameValue { set;}

        /// <summary>
        /// Method to set the 'lblAreaTypeValue' text property
        /// </summary>
        string SetlblAreaTypeValue { set;}



        /// <summary>
        /// Method to set the 'lblSectorTypeValue' text property
        /// </summary>
        string SetlblSectorTypeValue { set; }

        /// <summary>
        /// Method to set the 'lblLocalityCommentValue' text property
        /// </summary>
        string SetlblLocalityCommentValue { set;}

        /// <summary>
        /// Method to set the 'lblMarketCommentValue' text property
        /// </summary>
        string SetlblMarketCommentValue { set; }

        /// <summary>
        /// Method to set the 'lblRetentionReasonValue' text property
        /// </summary>
        string SetlblRetentionReasonValue { set;}

        /// <summary>
        /// Method to set the 'lblRetentionAmountValue' text property
        /// </summary>
        string SetlblRetentionAmountValue { set;}



        /// <summary>
        /// Method to set the 'lblERFTypeValue' text property
        /// </summary>
        string SetlblERFTypeValue { set;}

        /// <summary>
        /// Method to set the 'lblERFNumberValue' text property
        /// </summary>
        string SetlblERFNumberValue { set;  }

        /// <summary>
        /// Method to set the 'lblPortionValue' text property
        /// </summary>
        string SetlblPortionValue { set; }

        /// <summary>
        /// Method to set the 'lblPortionOfPortionValue' text property
        /// </summary>
        string SetlblPortionOfPortionValue { set; }

        /// <summary>
        /// Method to set the 'lblSubdivisionPortionValue' text property
        /// </summary>
        string SetlblSubdivisionPortionValue { set; }

        /// <summary>
        /// Method to set the 'lblLandSizeValue' text property
        /// </summary>
        string SetlblLandSizeValue { set;}

        /// <summary>
        /// Method to set the 'lblSectionNumberValue' text property
        /// </summary>
        string SetlblSectionNumberValue { set; }

        /// <summary>
        /// Method to set the 'txtLegalDescriptionValue' text property
        /// </summary>
        string SettxtLegalDescriptionValue { set; }

        /// <summary>
        /// Method to set the 'lblPropertyUseValue' text property
        /// </summary>
        string SetlblPropertyUseValue { set; }

        /// <summary>
        /// Method to set the 'lblPropertyTypeValue' text property
        /// </summary>
        string SetlblPropertyTypeValue { set;}

        /// <summary>
        /// Method to set the 'lblPropertyCommentValue' text property
        /// </summary>
        string SetlblPropertyCommentValue { set;}



        /// <summary>
        /// Method to set the 'lblFlatNumberValue' text property
        /// </summary>
        string SetlblFlatNumberValue { set;}

        /// <summary>
        /// Method to set the 'lblUnitSizeValue' text property
        /// </summary>
        string SetlblUnitSizeValue { set;}

        /// <summary>
        /// Method to set the 'lblComplexNameValue' text property
        /// </summary>
        string SetlblComplexNameValue { set; }

        /// <summary>
        /// Method to set the 'lblFloorsInComplexValue' text property
        /// </summary>
        string SetlblFloorsInComplexValue { set; }

        /// <summary>
        /// Method to set the 'lblUnitsInComplexValue' text property
        /// </summary>
        string SetlblUnitsInComplexValue { set;}



        /// <summary>
        /// Method to set the 'lblBathRoomCountValue' text property
        /// </summary>
        string SetlblBathRoomCountValue { set;}

        /// <summary>
        /// Method to set the 'lblStudyCountValue' text property
        /// </summary>
        string SetlblStudyCountValue { set;}

        /// <summary>
        /// Method to set the 'lblFamilyRoomCountValue' text property
        /// </summary>
        string SetlblFamilyRoomCountValue { set; }

        /// <summary>
        /// Method to set the 'lblEntranceHallCountValue' text property
        /// </summary>
        string SetlblEntranceHallCountValue { set;}

        /// <summary>
        /// Method to set the 'lblLaundryRoomCountValue' text property
        /// </summary>
        string SetlblLaundryRoomCountValue { set;}

        /// <summary>
        /// Method to set the 'lblDiningCountValue' text property
        /// </summary>
        string SetlblDiningCountValue { set; }

        /// <summary>
        /// Method to set the 'lblLoungeCountValue' text property
        /// </summary>
        string SetlblLoungeCountValue { set; }

        /// <summary>
        /// Method to set the 'lblMainBuildingBedroomCountValue' text property
        /// </summary>
        string SetlblMainBuildingBedroomCountValue { set;}

        /// <summary>
        /// Method to set the 'lblMainBuildingKitchenCountValue' text property
        /// </summary>
        string SetlblMainBuildingKitchenCountValue { set;}

        /// <summary>
        /// Method to set the 'lblPantryCountValue' text property
        /// </summary>
        string SetlblPantryCountValue { set; }

        /// <summary>
        /// Method to set the 'lblMainBuildingWCCountValue' text property
        /// </summary>
        string SetlblMainBuildingWCCountValue { set; }

        /// <summary>
        /// Method to set the 'lblOtherRoomCountValue' text property
        /// </summary>
        string SetlblOtherRoomCountValue { set;}

        /// <summary>
        /// Method to set the 'lblMainBuildingThatchRateValue' text property
        /// </summary>
        string SetlblMainBuildingThatchRateValue { set; }

        /// <summary>
        /// Method to set the 'lblMainBuildingThatchExtentValue' text property
        /// </summary>
        string SetlblMainBuildingThatchExtentValue { set;  }

        /// <summary>
        /// Method to set the 'lblMainBuildingConventionalRateValue' text property
        /// </summary>
        string SetlblMainBuildingConventionalRateValue { set; }

        /// <summary>
        /// Method to set the 'lblMainBuildingConventionalExtentValue' text property
        /// </summary>
        string SetlblMainBuildingConventionalExtentValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingBathroomCountValue' text property
        /// </summary>
        string SetlblOutbuildingBathroomCountValue { set; }

        /// <summary>
        /// Method to set the 'lblOutbuildingWorkshopCountValue' text property
        /// </summary>
        string SetlblOutbuildingWorkshopCountValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingLaundryCountValue' text property
        /// </summary>
        string SetlblOutbuildingLaundryCountValue { set; }

        /// <summary>
        /// Method to set the 'lblOutbuildingGarageCountValue' text property
        /// </summary>
        string SetlblOutbuildingGarageCountValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingBedroomCountValue' text property
        /// </summary>
        string SetlblOutbuildingBedroomCountValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingStoreRoomcountValue' text property
        /// </summary>
        string SetlblOutbuildingStoreRoomcountValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingKitchenCountValue' text property
        /// </summary>
        string SetlblOutbuildingKitchenCountValue { set; }

        /// <summary>
        /// Method to set the 'lblOutbuildingWCCountValue' text property
        /// </summary>
        string SetlblOutbuildingWCCountValue { set;}

        /// <summary>
        /// Method to set the 'lblOutbuildingThatchRateValue' text property
        /// </summary>
        string SetlblOutbuildingThatchRateValue { set; }

        /// <summary>
        /// Method to set the 'lblOutbuildingThatchExtentValue' text property
        /// </summary>
        string SetlblOutbuildingThatchExtentValue { set;  }

        /// <summary>
        /// Method to set the 'lblOutbuildingConventionalRateValue' text property
        /// </summary>
        string SetlblOutbuildingConventionalRateValue { set; }

        /// <summary>
        /// Method to set the 'lblOutbuildingConventionalExtentValue' text property
        /// </summary>
        string SetlblOutbuildingConventionalExtentValue { set;}



        /// <summary>
        /// Method to set the 'lblSCottageCottageStandNumberValue' text property
        /// </summary>
        string SetlblSCottageCottageStandNumberValue { set;  }

        /// <summary>
        /// Method to set the 'lblBedroomCountValue' text property
        /// </summary>
        string SetlblBedroomCountValue { set;  }

        /// <summary>
        /// Method to set the 'lblCottageKitchenCountValue' text property
        /// </summary>
        string SetlblCottageKitchenCountValue { set; }

        /// <summary>
        /// Method to set the 'lblCottageLivingRoomCountValue' text property
        /// </summary>
        string SetlblCottageLivingRoomCountValue { set;}

        /// <summary>
        /// Method to set the 'lblCottageOtherRoomCountValue' text property
        /// </summary>
        string SetlblCottageOtherRoomCountValue { set; }

        /// <summary>
        /// Method to set the 'lblCottageThatchRateValue' text property
        /// </summary>
        string SetlblCottageThatchRateValue { set;}

        /// <summary>
        /// Method to set the 'lblCottageThatchExtentValue' text property
        /// </summary>
        string SetlblCottageThatchExtentValue { set; }

        /// <summary>
        /// Method to set the 'lblCottageConventionalRateValue' text property
        /// </summary>
        string SetlblCottageConventionalRateValue { set; }

        /// <summary>
        /// Method to set the 'lblCottageConventionalExtentValue' text property
        /// </summary>
        string SetlblCottageConventionalExtentValue { set; }



        /// <summary>
        /// Method to set the 'lblSwimmingPoolValue' text property
        /// </summary>
        string SetlblSwimmingPoolValue { set; }

        /// <summary>
        /// Method to set the 'lblSwimmingPoolTypeValue' text property
        /// </summary>
        string SetlblSwimmingPoolTypeValue { set;  }

        /// <summary>
        /// Method to set the 'lblTennisCourtValue' text property
        /// </summary>
        string SetlblTennisCourtValue { set; }

        /// <summary>
        /// Method to set the 'lblTennisCourtTypeValue' text property
        /// </summary>
        string SetlblTennisCourtTypeValue { set; }

    }
}

using System;
using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IValuationLightstoneView : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event EventHandler ddlMapProviderSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnBackClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler TabsActiveTabChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdOwners(DataTable DT);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdComparativeSales(DataTable DT);



        /// <summary>
        /// Get the Map URL from the ddlMapProviderDropDownlist
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string GetddlMapProviderURL { get;}


        /// <summary>
        /// Get the Map URL from the ddlMapProviderDropDownlist Index 1=google 2=MapX
        /// </summary>
        int GetdllMapProviderIndex { get; set;}

        //***********************************************************************************
        // SHOW / HIDE SCREEN OBJECTS

        /// <summary>
        /// Method to show/hide  the 'pnlOwners' Panel
        /// </summary>
        bool SetpnlOwners { set;}

        /// <summary>
        /// Gets or Sets the Active Tab Index for the Main Tab Control
        /// </summary>
        int SetTabsActiveTabIndex { get; set;}


        //***********************************************************************************
        // SET UP THE PROPERTY CHANGE METHODS


        /// <summary>
        /// Method to set the 'lblReferenceNumberValue' text property
        /// </summary>
        string SetlblReferenceNumberValue { set;}

        /// <summary>
        /// Method to set the 'lblLastSaleDateValue' text property
        /// </summary>
        string SetlblLastSaleDateValue { set;}

        /// <summary>
        /// Method to set the 'lblAutomatedValueDateValue' text property
        /// </summary>
        string SetlblAutomatedValueDateValue { set;}

        /// <summary>
        /// Method to set the 'lblLastSalePriceValue' text property
        /// </summary>
        string SetlblLastSalePriceValue { set;}


        /// <summary>
        /// Method to set the 'lblERFSize' text property
        /// </summary>
        string SetlblERFSizeValue { set;}

        /// <summary>
        /// Method to set the 'lblEstimatedMarketValue' text property
        /// </summary>
        string SetlblEstimatedMarketValue { set;}

        /// <summary>
        /// Method to set the 'lblAreaQualityGradeValue' text property
        /// </summary>
        string SetlblAreaQualityGradeValue { set;}

        /// <summary>
        /// Method to set the 'lblConfidenceScoreValue' text property
        /// </summary>
        string SetlblConfidenceScoreValue { set;}

        /// <summary>
        /// Method to set the 'lblAreaExposureValue' text property
        /// </summary>
        string SetlblAreaExposureValue { set;}

        /// <summary>
        /// Method to set the 'lblExpectedHighValue' text property
        /// </summary>
        string SetlblExpectedHighValue { set;}

        /// <summary>
        /// Method to set the 'lblAutomatedValuationDecisionValue' text property
        /// </summary>
        string SetlblAutomatedValuationDecisionValue { set;}

        /// <summary>
        /// Method to set the 'lblExpectedLowValue' text property
        /// </summary>
        string SetlblExpectedLowValue { set;}

        /// <summary>
        /// Method to set show/hide  the 'MapLiteral' test to add and populate the Iframe
        /// </summary>
        string SetMapLiteral { set;}

        
    }
}

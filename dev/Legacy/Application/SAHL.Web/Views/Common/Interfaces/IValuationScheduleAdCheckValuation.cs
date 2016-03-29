using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IValuationScheduleAdCheckValuation : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnValidatePropertyClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnInstructClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler grdPropertiesSelectedIndexChanged;

        //***********************************************************************************
        // SET UP THE PROPERTY DISPLAY VISIBILITY METHODS

        /// <summary>
        /// Property to show/hide the 'ddlValuer' object
        /// </summary>
        bool ShowddlValuer { set;}

        /// <summary>
        /// Property to show/hide the 'lblValuerValue' object
        /// </summary>
        bool ShowlblValuerValue { set;}

        /// <summary>
        /// Property to show/hide the 'lblContact1Value' object
        /// </summary>
        bool ShowlblContact1Value { set;}

        /// <summary>
        /// Property to show/hide the 'lblPhone1Value' object
        /// </summary>
        bool ShowlblPhone1Value { set;}


        /// <summary>
        /// Property to show/hide the 'lblWorkPhone1Value' object
        /// </summary>
        bool ShowlblWorkPhone1Value { set;}

        /// <summary>
        /// Property to show/hide the 'lblCellPhone1Value' object
        /// </summary>
        bool ShowlblCellPhone1Value { set;}

        /// <summary>
        /// Property to show/hide the 'lblContact2Value' object
        /// </summary>
        bool ShowlblContact2Value { set;}

        /// <summary>
        /// Property to show/hide the 'lblPhone2Value' object
        /// </summary>
        bool ShowlblPhone2Value { set;}

        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Property to show/hide the 'txtContact1Name' object
        /// </summary>
        bool ShowtxtContact1Name { set;}

        /// <summary>
        /// Property to show/hide the 'txtContact1Phone' object
        /// </summary>
        bool ShowtxtContact1Phone { set;}

        /// <summary>
        /// Property to show/hide the 'txtContact1WorkPhone' object
        /// </summary>
        bool ShowtxtContact1WorkPhone { set;}

        /// <summary>
        /// Property to show/hide the 'txtContact1MobilePhone' object
        /// </summary>
        bool ShowtxtContact1MobilePhone { set;}

        /// <summary>
        /// Property to show/hide the 'txtContact2Name' object
        /// </summary>
        bool ShowtxtContact2Name { set;}

        /// <summary>
        /// Property to show/hide the 'txtContact2Phone' object
        /// </summary>
        bool ShowtxtContact2Phone { set;}



        /// <summary>
        /// Property to show/hide the 'txtSpecialInstructions' text box
        /// </summary>
        bool ShowtxtSpecialInstructions { set;}


        /// <summary>
        /// Property to show/hide the 'pnlValuationAssignmentDetails' panel
        /// </summary>
        bool ShowpnlValuationAssignmentDetails { set;}


        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Property to show/hide the 'dtAssessmentByDateValue' object
        /// </summary>
        bool ShowdtAssessmentByDateValue { set;}

        /// <summary>
        /// Property to show/hide the 'lblAssessmentByDateValue' object
        /// </summary>
        bool ShowlblAssessmentByDateValue { set;}

        /// <summary>
        /// Property to show/hide the 'ddlAssessmentReasonValue' object
        /// </summary>
        bool ShowddlAssessmentReasonValue { set;}

        /// <summary>
        /// Property to show/hide the 'lblAssessmentReasonValue' object
        /// </summary>
        bool ShowlblAssessmentReasonValue { set;}

        /// <summary>
        /// Property to show/hide the 'ddlAssessmentPriorityValue' object
        /// </summary>
        bool ShowddlAssessmentPriorityValue { set;}

        /// <summary>
        /// Property to show/hide the 'lblAssessmentPriorityValue' object
        /// </summary>
        bool ShowlblAssessmentPriorityValue { set;}

        //  SHOW/HIDE THE FORM NAVIGATION BUTTONS


        /// <summary>
        /// Property to show/hide the 'btnInstruct' button
        /// </summary>
        bool ShowbtnInstruct { set;}

        /// <summary>
        /// Property to show/hide the 'btnValidate' button
        /// </summary>
        bool ShowbtnValidateProperty { set;}
        //***********************************************************************************

        /// <summary>
        /// Property to get or set the 'lblAssessmentNumberValue' text
        /// </summary>
        string SetlblAssessmentNumberValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblRequestNumberValue' text
        /// </summary>
        string SetlblRequestNumberValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblApplicationNameValue' text
        /// </summary>
        string SetlblApplicationNameValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblRequestedByValue' text
        /// </summary>
        string SetlblRequestedByValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblBuildingValue' text
        /// </summary>
        string SetlblBuildingValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblTitleTypeValue' text
        /// </summary>
        string SetlblTitleTypeValue { get; set;}


        /// <summary>
        /// Property to get or set the 'lblStreetValue' text
        /// </summary>
        string SetlblStreetValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblOccupancyTypeValue' text
        /// </summary>
        string SetlblOccupancyTypeValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblSuburbValue' text
        /// </summary>
        string SetlblSuburbValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblAreaClassificationValue' text
        /// </summary>
        string SetlblAreaClassificationValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblCityValue' text
        /// </summary>
        string SetlblCityValue { get; set;}

        /// <summary>
        /// Property to get or set the 'txtPropertyDescription' text
        /// </summary>
        string SettxtPropertyDescription { get; set;}

        /// <summary>
        /// Property to get or set the 'lblProvinceValue' text
        /// </summary>
        string SetlblProvinceValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblERFNumberValue' text
        /// </summary>
        string SetlblERFNumberValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblCountryValue' text
        /// </summary>
        string SetlblCountryValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblPortionNumberValue' text
        /// </summary>
        string SetlblPortionNumberValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblPostalCodeValue' text
        /// </summary>
        string SetlblPostalCodeValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblERFSuburbValue' text
        /// </summary>
        string SetlblERFSuburbValue { get; set;}

        /// <summary>
        /// Property to get or set the 'txtTitleDeedNumber' text
        /// </summary>
        string SettxtTitleDeedNumber { get; set;}

        /// <summary>
        /// Property to get or set the 'lblERFMetroDescriptionValue' text
        /// </summary>
        string SetlblERFMetroDescriptionValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblDeedsPropertyTypeValue' text
        /// </summary>
        string SetlblDeedsPropertyTypeValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblSectionalSchemeNameValue' text
        /// </summary>
        string SetlblSectionalSchemeNameValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblSAPTGPropertyNumberValue' text
        /// </summary>
        string SetlblSAPTGPropertyNumberValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblSectionalUnitNumberValue' text
        /// </summary>
        string SetlblSectionalUnitNumberValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblValuerValue' text
        /// </summary>
        string SetlblValuerValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblAssessmentByDateValue' text
        /// </summary>
        string SetlblAssessmentByDateValue { get; set;}

        /// <summary>
        ///  Set the ReadOnly status of txtSpecialInstructions
        /// </summary>
        bool SettxtSpecialInstructionsReadOnly { get; set;}

        /// <summary>
        ///  Set the ReadOnly status of txtPropertyDescription
        /// </summary>
        bool SettxtPropertyDescriptionReadOnly { get; set;}


        /// <summary>
        /// Property to get or set the 'lblContact1Value' text
        /// </summary>
        string SetlblContact1Value { get; set;}

        /// <summary>
        /// Property to get or set the 'lblAssessmentReasonValue' text
        /// </summary>
        string SetlblAssessmentReasonValue { get; set;}

        /// <summary>
        /// Property to get or set the 'lblPhone1Value' text
        /// </summary>
        string SetlblPhone1Value { get; set;}
        /// <summary>
        /// Property to get or set the 'lblAssessmentPriorityValue' text
        /// </summary>
        string SetlblAssessmentPriorityValue { get; set;}


        /// <summary>
        /// Property to get or set the 'lblWorkPhone1Value' text
        /// </summary>
        string SetlblWorkPhone1Value { get; set;}

        /// <summary>
        /// Property to get or set the 'lblCellPhone1Value' text
        /// </summary>
        string SetlblCellPhone1Value { get; set;}

        /// <summary>
        /// Property to get or set the 'lblContact2Value' text
        /// </summary>
        string SetlblContact2Value { get; set;}

        /// <summary>
        /// Property to get or set the 'lblPhone2Value' text
        /// </summary>
        string SetlblPhone2Value { get; set;}

        /// <summary>
        /// Property to get or set the 'txtContact1Name' text
        /// </summary>
        string SettxtContact1Name { get; set;}


        /// <summary>
        /// Property to get or set the 'txtContact1Phone' text
        /// </summary>
        string SettxtContact1Phone { get; set;}

        /// <summary>
        /// Property to get or set the 'txtContact1WorkPhone' text
        /// </summary>
        string SettxtContact1WorkPhone { get; set;}

        /// <summary>
        /// Property to get or set the 'txtContact1MobilePhone' text
        /// </summary>
        string SettxtContact1MobilePhone { get; set;}

        /// <summary>
        /// Property to get or set the 'txtContact2Name' text
        /// </summary>
        string SettxtContact2Name { get; set;}

        /// <summary>
        /// Property to get or set the 'txtContact2Phone' text
        /// </summary>
        string SettxtContact2Phone { get; set;}

        /// <summary>
        /// Property to get or set the 'txtContact2Phone' text
        /// </summary>
        int SetddlValuerSelectedIndex { get; set;}

        /// <summary>
        /// Property to get or set the 'ddlValuerSelectedIndex' SelectedValue
        /// </summary>
        int SetddlPropertiesSelectedSelectedIndex { get; set;}

        /// <summary>
        /// Property to get or set the 'SetddlAssessmentPrioritySelectedIndex' SelectedValue
        /// </summary>
        int SetddlAssessmentPrioritySelectedIndex { get; set;}

        /// <summary>
        /// Property to get or set the 'ddlAssessmentReasonValue' SelectedValue
        /// </summary>
        int ddlAssessmentReasonValueSelectedIndex { get; set;}

        /// <summary>
        /// Property to get or set the 'ddlAssessmentPriorityValue' SelectedValue
        /// </summary>
        int SetddlAssessmentPriorityValue { get; set;}


        /// <summary>
        /// Bind the Properties Grid
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdProperties(DataTable DT);

        /// <summary>
        /// Property to get or set the 'ddlAssessmentPriorityValue' Selected Text
        /// </summary>
        string SetddlAssessmentPriorityText { get;}

        /// <summary>
        /// Property to get or set the 'ddlAssessmentReasonValue' SelectedValue
        /// </summary>
        int SetddlAssessmentReasonValue { get; set;}

        /// <summary>
        /// Property to get or set the 'txtSpecialInstructions' text
        /// </summary>
        string SettxtSpecialInstructions { get; set;}

        /// <summary>
        /// Property to get or set the 'dtAssessmentByDateValue' text
        /// </summary>
        DateTime SetdtAssessmentByDateValue { get; set;}


        //***********************************************************
        // SET UP THE BEHAVIOUR FOR THE DROPDOWN LISTS

        /// <summary>
        /// bind the object list to the 'ddlValuer' drop down list
        /// </summary>
        void bindddlValuer(DataTable DT);

        /// <summary>
        /// bind the object list to the 'ddlAssessmentReasonValue' drop down list
        /// </summary>
        void bindddlAssessmentReasonValue(DataTable DT);

        /// <summary>
        ///  bind the object list to the 'ddlAssessmentPriorityValue' drop down list
        /// </summary>
        void bindddlAssessmentPriorityValue(DataTable DT);

        /// <summary>
        /// Gets or sets the Proeprty Item index
        /// </summary>
        int PropertyItemIndex { get; set;}


        /// <summary>
        ///  Set or Get the Properties Grid Postbacktype
        /// </summary>
        GridPostBackType SetPostBackTypegrdProperties { get; set;}

        /// <summary>
        /// Property to show/hide the 'grdProperties' grid
        /// </summary>
        bool ShowgrdProperties { set;}

        /// <summary>
        /// Property to show/hide the 'pnlSelectProperty' panel
        /// </summary>
        bool ShowpnlSelectProperty { get; set;}

    }
}

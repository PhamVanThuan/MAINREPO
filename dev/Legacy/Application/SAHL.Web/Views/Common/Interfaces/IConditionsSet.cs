using System;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConditionsSet : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnAddConditionClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnEditConditionClicked;
        
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnUpdateClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnSaveClicked;


        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnCancelClicked;

        /// <summary>
        /// Property to show/hide the Valudation Message
        /// </summary>
        bool ShowlblErrorMessage { get; set;}

        /// <summary>
        /// Sets whether to show/hide 'Update Condition' button
        /// </summary>
        bool ShowUpdateButton { set;}
        /// <summary>
        /// Sets whether to show/hide 'Save Condition Set' button
        /// </summary>
        bool ShowSaveButton { set;}
        /// <summary>
        /// Sets whether to show/hide 'Edit Condition' button
        /// </summary>
        bool ShowEditConditionButton { set;}
        /// <summary>
        /// Sets whether to show/hide 'Add Condition' button
        /// </summary>
        bool ShowAddButton { set;}
        /// <summary>
        /// Sets whether to enable 'Save Condition Set' button
        /// </summary>
        bool EnableSaveButton { set;}

        /// <summary>
        /// Property to enable the 'listGenericConditions' list
        /// </summary>
        bool SetlistGenericConditionsReadOnly { set;}

        ///// <summary>
        ///// Property to enable the 'listSelectedConditions' list
        ///// </summary>
        //bool SetlistSelectedConditionsReadOnly { set;}

        /// <summary>
        /// Gets the txtDisplay value
        /// </summary>
        string GettxtDisplayText { get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtSelectArrayStrings {set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayCSSColor value
        /// </summary>
        string SettxtSelectArrayCSSColor { set; get;}
        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtSelectArrayCSSWeight { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtSelectArrayValue { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtSelectArrayID { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtSelectArrayUserEdited { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtSelectUserConditionType { set; get;}

        /// <summary>
        /// Gets or Sets the txttxtSelectedOfferConditionKeys value
        /// </summary>
        string SettxtSelectedOfferConditionKeys { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectedOfferConditionSetKeys value
        /// </summary>
        string SettxtSelectedOfferConditionSetKeys { set; get;}


        /// <summary>
        /// Gets or Sets the txtChosenOfferConditionSetKeys value
        /// </summary>
        string SettxtChosenOfferConditionSetKeys { set; get;}



        /// <summary>
        /// Gets or Sets the txtChosenOfferConditionKeys value
        /// </summary>
        string SettxtChosenOfferConditionKeys { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        string SettxtChosenArrayStrings { set; get;}

        /// <summary>
        /// Gets or Sets the txtChosenArrayCSSColor value
        /// </summary>
        string SettxtChosenArrayCSSColor { set; get;}

        /// <summary>
        /// Gets or Sets the txtChosenArrayCSSWeight value
        /// </summary>
        string SettxtChosenArrayCSSWeight { set; get;}

        /// <summary>
        /// Gets or Sets the txtChosenArrayValue value
        /// </summary>
        string SettxtChosenArrayValue { set; get;}

        /// <summary>
        /// Gets or Sets the txtChosenArrayID value
        /// </summary>
        string SettxtChosenArrayID { set; get;}

        /// <summary>
        /// Gets or Sets the txtChosenArrayUserEdited value
        /// </summary>
        string SettxtChosenArrayUserEdited { set; get;}

        /// <summary>
        /// Gets or Sets the txtChosenUserConditionType value
        /// </summary>
        string SettxtChosenUserConditionType { set; get;}

        /// <summary>
        /// Gets or Sets the txtSelectedIndex value
        /// </summary>
        string SettxtSelectedIndex { set; get;}

        //**************************************************************************************
        ///// <summary>
        ///// Gets the listSelectedConditions.SelectedValue
        ///// </summary>
        //string GetlistSelectedConditionsValue { get;}

        /// <summary>
        /// Gets or Sets the txtConditionTableKey value
        /// </summary>
        string SettxttxtConditionTableKey { set; get;}

        ///// <summary>
        ///// Gets the listSelectedConditions.ClientID
        ///// </summary>
        //string GetlistSelectedConditionsClientID { get;}
        ///// <summary>
        ///// Gets the txtDisplay.ClientID
        ///// </summary>
        //string GetlistGenericConditionsClientID { get;}
        /// <summary>
        /// Gets the listGenericConditions.ClientID
        /// </summary>
        string GettxtDisplayClientID { get;}
        /// <summary>
        /// Gets the btnEditCondition.ClientID;
        /// </summary>
        string GetbtnEditConditionClientID { get;}
        /// <summary>
        /// Gets the txtSelectedIndex.ClientID
        /// </summary>
        string GettxtSelectedIndexClientID { get;}
        /// <summary>
        /// Gets the txtSelectArrayStrings.ClientID;
        /// </summary>
        string GettxtSelectArrayStringsClientID { get;}
        /// <summary>
        /// Gets the txtSelectArrayCSSColor.ClientID
        /// </summary>
        string GettxtSelectArrayCSSColorClientID { get;}
        /// <summary>
        /// Gets the txtSelectArrayCSSWeight.ClientID
        /// </summary>
        string GettxtSelectArrayCSSWeightClientID { get;}
        /// <summary>
        /// Gets the txtSelectArrayValue.ClientID
        /// </summary>
        string GettxtSelectArrayValueClientID { get;}
        /// <summary>
        /// Gets the txtSelectArrayID.ClientID
        /// </summary>
        string GettxtSelectArrayIDClientID { get;}
        /// <summary>
        /// Gets the txtSelectArrayUserEdited.ClientID
        /// </summary>
        string GettxtSelectArrayUserEditedClientID { get;}

        /// <summary>
        /// Gets the txtSelectUserConditionType.ClientID
        /// </summary>
        string GettxtSelectUserConditionType { get;}

        /// <summary>
        /// Gets the txtChosenArrayStrings.ClientID
        /// </summary>
        string GettxtChosenArrayStringsClientID { get;}
        /// <summary>
        /// Gets the txtChosenArrayCSSColor.ClientID
        /// </summary>
        string GettxtChosenArrayCSSColorClientID { get;}
        /// <summary>
        /// Gets the txtChosenArrayCSSWeight.ClientID
        /// </summary>
        string GettxtChosenArrayCSSWeightClientID { get;}
        /// <summary>
        /// Gets the txtChosenArrayValue.ClientID
        /// </summary>
        string GettxtChosenArrayValueClientID { get;}
        /// <summary>
        /// Gets the txtChosenArrayID.ClientID
        /// </summary>
        string GettxtChosenArrayIDClientID { get;}
        /// <summary>
        /// Gets the txtChosenArrayUserEdited.ClientID
        /// </summary>
        string GettxtChosenArrayUserEditedClientID { get;}

        /// <summary>
        /// Gets the txtChosenUserConditionType.ClientID
        /// </summary>
        string GettxtChosenUserConditionType { get;}

        /// <summary>
        /// Gets the txtSelectedOfferConditionKeys.ClientID
        /// </summary>
        string GettxtSelectedOfferConditionKeys { get;}

        /// <summary>
        /// Gets the txtChosenOfferConditionKeys.ClientID
        /// </summary>
        string GettxtChosenOfferConditionKeys { get;}

        /// <summary>
        /// Gets the txtChosenOfferConditionKeys.ClientID
        /// </summary>
        string GettxtChosenOfferConditionSetKeys { get;}
        /// <summary>
        /// Gets the txtSelectedOfferConditionSetKeys.ClientID
        /// </summary>
        string GettxtSelectedOfferConditionSetKeys { get;}
        /// <summary>
        /// Gets the txtConditionTableKeyClientID
        /// </summary>
        string GettxtConditionTableKeyClientID { get;}

        //**************************************************************************************

        /// <summary>
        /// Create and Register the Views Javascript Model
        /// </summary>
        /// <param name="mBuilder"></param>
        void RegisterClientScripts(System.Text.StringBuilder mBuilder);
    }
}

using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IConditionsTranslate : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnUpdateClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnCancelClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnEditClicked;


        /// <summary>
        /// Property to show/hide the 'lblErrorMessage' label
        /// </summary>
        bool ShowlblErrorMessage { set; }

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        bool ShowbtnCancel { set; }

        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        bool ShowbtnUpdate { set; }

        /// <summary>
        /// Property to show/hide the 'btnTokens' button
        /// </summary>
        bool ShowbtnEdit { set; }


        //*******************************************************************
        // HANDLE THE HIDDEN FIELDS
        /// <summary>
        /// Get the Value of the txtConditionToEdit hidden field
        /// </summary>
        string SettxtConditionToEdit { set; }
        /// <summary>
        /// Set the Value of the txtConditionToEdit hidden field
        /// </summary>
        string GettxtConditionToEdit { get; }

        /// <summary>
        /// Get the Value of the txtConditionsKey hidden field
        /// </summary>
        string SettxtConditionsKey { set; }
        /// <summary>
        /// Set the Value of the txtConditionsKey hidden field
        /// </summary>
        string GettxtConditionsKey { get; }


        //*******************************************************************
        // GET THE CLIENT IDS FOR THE SCREEN OBJECTS

        /// <summary>
        /// Get the ClientID for listGenericConditions
        /// </summary>
        string GetlistGenericConditionsClientID { get; }
        /// <summary>
        /// Get the ClientID for txtDisplay
        /// </summary>
        string GettxtDisplayClientID { get; }

        /// <summary>
        /// Get the ClientID for txtTranslate
        /// </summary>
        string GettxtTranslateClientID { get; }

        /// <summary>
        /// Get the ClientID for txtConditionToEdit
        /// </summary>
        string GettxtConditionToEditClientID { get; }


        /// <summary>
        /// Get the ClientID for txAfrikaansArrayStrings
        /// </summary>
        string GettxtConditionsKeyClientID { get; }

        /// <summary>
        /// Get the ClientID for btnTokens
        /// </summary>
        string GetbtnEditClientID { get; }

        /// <summary>
        /// Get the ClientID for btnUpdate
        /// </summary>
        string GetbtnUpdateClientID { get; }

        /// <summary>
        /// Get the ClientID for btnUpdate
        /// </summary>
        string GetbtnCancelClientID { get; }

        //*******************************************************************
        // HANDLE THE UNIQUE SCRIPTING FOR THIS VIEW
        /// <summary>
        /// 
        /// </summary>
        void RegisterClientScripts(System.Text.StringBuilder mBuilder);

        /// <summary>
        /// Adds an item to the Translatablle conditions List
        /// </summary>
        /// <param name="li"></param>
        void AddListBoxItem(ListItem li);

    }
}

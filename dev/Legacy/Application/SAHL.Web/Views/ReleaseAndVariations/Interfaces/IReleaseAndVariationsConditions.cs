using System;
using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReleaseAndVariationsConditions : IViewBase
    {

        // The Event Handlers
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnAddClicked;
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
        event EventHandler gridConditionsClicked;

        /// <summary>
        /// Populate the Grid with the conditions dataset
        /// </summary>
        void BindConditionGrid(DataTable DT);
        // ***********************  Interface Event Handler stuff *********************

        /// <summary>
        /// Property to show/hide the 'btnAdd' button
        /// </summary>
        bool ShowbtnAdd { set; get;}

        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        bool ShowbtnUpdate { set;}
        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        string Getcondition { get;}

        /// <summary>
        /// Cleat the Text in the Textbox;
        /// </summary>
        void ClearText();

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        bool ShowbtnCancel { set;}

        // Set Up the Grid
        /// <summary>
        /// Property to select the first row of the 'gridConditions' grid
        /// </summary>
        bool SelectFirstRowgridConditions { set;}

        /// <summary>
        /// Set the index for gridConditions
        /// </summary>
        int SetgridConditionsIndex { set; get;}

        /// <summary>
        /// Set the index for gridConditions
        /// </summary>
         string GetgridConditionsText { get;}

        /// <summary>
        /// Set the Text of txtNotes
        /// </summary>
        string  SettxtNotesText { set; get;}


        /// <summary>
        /// Property to set The readonly attribure of the 'txtNotes'  object
        /// </summary>
        bool SetReadOnlytxtNotes { set;}

    }
}

using System;
using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IConditionsEdit : IViewBase
    {
        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnRestoreStringClicked"/>.
        /// </summary>
        event EventHandler btnRestoreStringClicked;

        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnAddClicked"/>.
        /// </summary>
        event EventHandler btnAddClicked;

        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnUpdateClicked"/>.
        /// </summary>
        event EventHandler btnUpdateClicked;

        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnCancelClicked"/>.
        /// </summary>
        event EventHandler btnCancelClicked;


        /// <summary>
        /// Property to enable/disable the 'ADD' button
        /// </summary>
        /// 
        bool EnableAddButton { get; set;}

        /// <summary>
        /// Property to enable/disable 'Update Condition' button
        /// </summary>
        bool EnableUpdateButton { get; set;}

        //--------------------------------------------------------
        /// <summary>
        /// Property to show/hide the 'RestoreString' button
        /// </summary>
        /// 
        bool ShowRestoreStringButton { get; set;}
        /// <summary>
        /// Property to show/hide the 'Show' button
        /// </summary>
        /// 
        bool ShowAddButton { get; set;}

        /// <summary>
        /// Property to show/hide the 'Update Condition' button
        /// </summary>
        bool ShowUpdateButton { get; set;}

        /// <summary>
        /// Property to show/hide the 'Candel' button
        /// </summary>
        bool ShowCancelButton { get; set;}



        /// <summary>
        /// Property to show/hide the 'pnlTranslation' panel
        /// </summary>
        /// 
        bool ShowTranslatePanel { get; set;}

        /// <summary>
        /// Property to show/hide the 'txtTranslation' Textbox
        /// </summary>
        /// 
        bool ShowtxtTranslation { get; set;}

        /// <summary>
        /// Property to show/hide the 'txtTranslation' ReadOnly Status
        /// </summary>
        bool SettxtTranslationReadonly { get; set;}


        /// <summary>
        /// Get or Set the txtNotes Text
        /// </summary>
        string SettxtNotesText { get; set;}

        /// <summary>
        /// Get or Set Panel 2's Grouping Text
        /// </summary>
        string SetPanel2GroupingText { get; set;}

        /// <summary>
        /// Set the Readonly Value of txtNotes for Non-Editable Strings
        /// </summary>
        bool SettxtNotesReadOnly { get; set;}

        /// <summary>
        /// Get or Set the txtArraycount Text
        /// </summary>
        string SettxtTokenIDs { get; set;}

        /// <summary>
        /// Get or Set the txtTokenValues Text
        /// </summary>
        string SettxtTokenValues { get; set;}



        /// <summary>
        /// Get or Set the Value of The Translated Text
        /// </summary>
        string SettxtTranslation { get; set;}

        /// <summary>
        /// Add an Attribute to the txtNotes for editing the Text (when in edit mode)
        /// </summary>
        /// <param name="option">1=Add, 2=Update</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void txtNotesAddAttributeforEditable(int option);


        /// <summary>
        /// Create the Custom fields and parse the interface for them using a datatable
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="ConditionKey"></param>
        /// <param name="Translation"></param>
        void ConfigureTokenCaptureFields(DataTable tokens, int ConditionKey, bool Translation);




    }
}

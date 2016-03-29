using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface INoteMaintenance : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        int ADUserKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        string WorkflowName { get; set; }

        /// <summary>
        ///
        /// </summary>
        int GenericKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        DateTime? DiaryDate { get;  set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lstNoteDetails"></param>
        void BindNotesGrid(List<INoteDetail> lstNoteDetails);

        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler gvNotesGridRowInserting;

        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler gvNotesGridRowUpdating;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        string NoteText{ get; }

        string TagText { get; }
    }
}
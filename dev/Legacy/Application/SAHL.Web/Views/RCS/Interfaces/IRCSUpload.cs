using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.RCS.Interfaces
{
    public interface IRCSUpload : IViewBase
    {
        #region Methods
        /// <summary>
        /// Get the ImportFile history
        /// </summary>
        /// <param name="importFileHistory"></param>
        void BindUploadHistoryGrid(IReadOnlyEventList<IImportFile> importFileHistory);

        Boolean SaveUploadFile(string uploadFile);

        void DropControlItemsAdd(SAHLDropDownList dropDownControl, string s);

        string GetReplaceValues(string controlName);

        void RegisterClientScripts(string message);

        void ViewResults(IReadOnlyEventList<IImportLegalEntity> importLegalEntity);

        #endregion

        #region Properties

        #region Control Properties

        string UploadFileFileName {get; }

        /// <summary>
        /// Sets whether the UploadButton control should be enabled.
        /// </summary>
        bool UploadButtonEnabled { set; }

        /// <summary>
        /// Sets whether the SubmitButton control should be enabled.
        /// </summary>
        bool SubmitButtonEnabled { set; }

        /// <summary>
        /// Sets whether the ReplaceButton control should be enabled.
        /// </summary>
        bool ReplaceButtonEnabled { set; }

        /// <summary>
        /// Sets whether the ResultsButton control should be enabled.
        /// </summary>
        bool ResultsButtonEnabled { set; }

        /// <summary>
        /// Sets whether the FileName control should be enabled.
        /// </summary>
        bool FileNameEnabled { set; }

        /// <summary>
        /// Sets whether the FileName control should be visible.
        /// </summary>
        bool FileNameVisible { set; }

        /// <summary>
        /// Sets whether the FileNameVal control should be enabled.
        /// </summary>
        bool FileNameValEnabled { set; }

		/// <summary>
		/// Sets whether the FileNameVal control is valid.
		/// </summary>
		bool FileNameValIsValid { set; }

        /// <summary>
        /// Sets whether the FileNameDisplay control should be visible.
        /// </summary>
        bool FileNameDisplayVisible { set; }

        /// <summary>
        /// Sets the FileNameDisplay control text.
        /// </summary>
        string FileNameDisplayText { set; get; }

        /// <summary>
        /// Sets the ReplaceList control ListRows.Add.
        /// </summary>
        TableRow ReplaceListRowsAdd { set; }

        bool ReplacementTableVisible { set; get; }

        #endregion

        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        event EventHandler UploadClick;

        event EventHandler CancelClick;

        event EventHandler ReplaceClick;

        event EventHandler SubmitClick;

        event EventHandler ResultsClick;

        #endregion
    }
}

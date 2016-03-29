using System;
using System.Collections.Generic;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxHtmlEditor;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class NoteMaintenance : SAHLCommonBaseView, INoteMaintenance
    {
        private DateTime? _diaryDate;
        private int _genericKey, _aduserKey;
        private string _workflowname;

        #region Properties

        public int ADUserKey
        {
            get
            {
                return _aduserKey;
            }
            set
            {
                _aduserKey = value;
            }
        }

        public string WorkflowName
        {
            get
            {
                return _workflowname;
            }
            set
            {
                _workflowname = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? DiaryDate
        {
            get { return _diaryDate; }
            set { _diaryDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        public string NoteText
        {
            get
            {
                DXHtmlEditor notes = gvNotes.FindEditFormTemplateControl("notesEditor") as DXHtmlEditor;
                return notes.Html;
            }
        }

        public string TagText
        {
            get
            {
                SAHLTextBox txtTag = gvNotes.FindEditFormTemplateControl("txtTag") as SAHLTextBox;
                return txtTag.Text;
            }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler gvNotesGridRowUpdating;

        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler gvNotesGridRowInserting;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        #endregion Event Handlers

        #region Protected Methods Section

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvNotes.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(gvNotes_HtmlDataCellPrepared);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterWebService(ServiceConstants.Diary);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage)
                return;

            if (_diaryDate != System.DateTime.MinValue)
                dtDiaryDate.Date = _diaryDate;

            hidGenericKey.Value = _genericKey.ToString();
            HidADUserKey.Value = _aduserKey.ToString();
            HidWorkflowName.Value = _workflowname;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                _diaryDate = dtDiaryDate.Date;
                OnSubmitButtonClicked(sender, e);
            }
        }

        protected void gvNotes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (gvNotesGridRowUpdating != null)
            {
                int editingIndex = Convert.ToInt32(e.Keys[0]);
                Dictionary<string, string> dictNewValues = new Dictionary<string, string>();
                dictNewValues.Add("TagText", TagText);
                gvNotesGridRowUpdating(dictNewValues, new KeyChangedEventArgs(editingIndex));
                gvNotes.CancelEdit();
            }
        }

        protected void gvNotes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (gvNotesGridRowInserting != null)
            {
                Dictionary<string, string> dictNewValues = new Dictionary<string, string>();
                dictNewValues.Add("NoteText", NoteText);
                dictNewValues.Add("TagText", TagText);
                gvNotesGridRowInserting(dictNewValues, new KeyChangedEventArgs(0));
                gvNotes.CancelEdit();
            }
        }

        protected void gvNotes_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            if (gvNotes.IsNewRowEditing) // add mode
            {
                gvNotes.DoRowValidation();
            }
        }

        protected void gvNotes_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (gvNotes.IsNewRowEditing) // add mode
            {
                e.NewValues["NoteText"] = NoteText;
                if (String.IsNullOrEmpty(e.NewValues["NoteText"].ToString().Trim()))
                {
                    e.RowError = "Note is required.";
                }
            }
        }

        protected void gvNotes_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "InsertedDate")
            {
                DateTime dteTemp = Convert.ToDateTime(e.CellValue);

                e.Cell.Text = dteTemp.ToString(SAHL.Common.Constants.DateTimeFormat);
            }
        }

        #endregion Protected Methods Section

        /// <summary>
        ///
        /// </summary>
        /// <param name="lstNoteDetails"></param>
        public void BindNotesGrid(List<INoteDetail> lstNoteDetails)
        {
            // sort by latest note first
            lstNoteDetails.Sort(delegate(INoteDetail n1, INoteDetail n2) { return n2.Key.CompareTo(n1.Key); });

            gvNotes.Selection.UnselectAll();

            gvNotes.DataSource = lstNoteDetails;
            gvNotes.DataBind();
        }

        protected void gvNotes_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditFormEventArgs e)
        {
            Control ctrl = gvNotes.FindEditFormTemplateControl("notesEditor");
            SAHL.Common.Web.UI.Controls.DXHtmlEditor htmlEditor = (DXHtmlEditor)ctrl;
            if (ctrl != null)
            {
                if (htmlEditor.Toolbars.Count == 0)
                {
                    //Create Custom Toolbar
                    htmlEditor.Toolbars.Add(CreateCustomToolbar("CustomToolbar"));

                    //We have to create the standard toolbars first to hide them
                    htmlEditor.Toolbars.Add(HtmlEditorToolbar.CreateStandardToolbar1());
                    htmlEditor.Toolbars["StandardToolbar1"].Visible = false;
                    htmlEditor.Toolbars.Add(HtmlEditorToolbar.CreateStandardToolbar2());
                    htmlEditor.Toolbars["StandardToolbar2"].Visible = false;
                }

                if (((DXGridView)sender).IsNewRowEditing)
                {
                    htmlEditor.Settings.AllowHtmlView = false;
                    htmlEditor.Settings.AllowPreview = false;
                    htmlEditor.Settings.AllowDesignView = true;
                }
                else
                {
                    //Read only mode
                    htmlEditor.Settings.AllowHtmlView = false;
                    htmlEditor.Settings.AllowPreview = true;
                    htmlEditor.Settings.AllowDesignView = false;

                    //Hide Custom Toolbar as well
                    if (htmlEditor.Toolbars.Count > 0)
                    {
                        htmlEditor.Toolbars["CustomToolbar"].Visible = false;
                    }
                }
            }
        }

        protected static HtmlEditorToolbar CreateCustomToolbar(string name)
        {
            return new HtmlEditorToolbar(
                name,
                new ToolbarUndoButton(),
                new ToolbarRedoButton(),
                new ToolbarFontNameEdit(),
                new ToolbarFontSizeEdit(),
                new ToolbarJustifyLeftButton(true),
                new ToolbarJustifyCenterButton(),
                new ToolbarJustifyRightButton(),
                new ToolbarJustifyFullButton(),
                new ToolbarBoldButton(),
                new ToolbarItalicButton(),
                new ToolbarUnderlineButton(),
                new ToolbarInsertUnorderedListButton(),
                new ToolbarInsertOrderedListButton()
            ).CreateDefaultItems();
        }

        protected void gvNotes_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "NoteText")
            {
                e.DisplayText = Server.HtmlDecode(e.GetFieldValue("NoteText").ToString());
            }
        }
    }
}
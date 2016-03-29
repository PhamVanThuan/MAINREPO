using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using SAHL.Common.Authentication;
using SAHL.Web.Views.Common.Interfaces;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using DevExpress.Web.ASPxGridView;


namespace SAHL.Web.Views.Common
{
    public partial class StageTransition_History : SAHLCommonBaseView, IStageTransitionHistory
    {
        bool _genericKeyColumnVisible;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (IsMenuPostBack)
            {
                gridHistory.ClearSort();

                foreach (GridViewDataColumn col in gridHistory.GetGroupedColumns())
                {
                    gridHistory.UnGroup(col);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool GenericKeyColumnVisible
        {
            set { _genericKeyColumnVisible = value; }
        }

        /// <summary>
        /// Cancel Button Clicked event - navigate to next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            onCancelButtonClicked(sender, e);
        }

        #region IStageTransitionHistory Members

        /// <summary>
        /// Event handler for Cancel Button
        /// </summary>
        public event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Controls visibility of Cancel Button
        /// </summary>
        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Life Workflow Header control
        /// </summary>
        public bool ShowLifeWorkFlowHeader
        {
            set
            {
                WorkFlowHeader1.Visible = value;
            }
        }
      
        #endregion

        public void SetUpGrid()
        {
            gridHistory.SettingsPager.PageSize = 20;
            AddGridColumn(gridHistory, "StageTransitionKey", "StageTransitionKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            AddGridColumn(gridHistory, "GenericKey", "GenericKey", 10, GridFormatType.GridString, null, HorizontalAlign.Left, _genericKeyColumnVisible, true);
            AddGridColumn(gridHistory, "GenericKeyType", "GenericKey Type", 10, GridFormatType.GridString, null, HorizontalAlign.Left, _genericKeyColumnVisible, true);
            AddGridColumn(gridHistory, "StageDefinitionGroup", "Group", 15, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gridHistory, "StageDefinition", "Event", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gridHistory, "TransitionDate", "Date", 15, GridFormatType.GridDateTime, "dd/MM/yyyy HH:mm:ss", HorizontalAlign.Left, true, true);
            AddGridColumn(gridHistory, "ADUserName", "User", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gridHistory, "Comments", "Note", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            gridHistory.Settings.ShowGroupPanel = true;
        }

        public void BindHistoryGrid(DataTable dtStageTransitions)
        {
            gridHistory.DataSource = dtStageTransitions;
            gridHistory.DataBind();
        }

        private static void AddGridColumn(SAHL.Common.Web.UI.Controls.DXGridView gridview, string fieldName, string caption, int width, GridFormatType formatType, string format, HorizontalAlign align, bool visible, bool readOnly)
        {
            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = fieldName;
            col.Caption = caption;
            col.Width = Unit.Percentage(width);
            col.Format = formatType;
            if (!String.IsNullOrEmpty(format))
                col.FormatString = format;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            col.ReadOnly = readOnly;
            gridview.Columns.Add(col);
        }
    }
}
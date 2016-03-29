using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data;
using DevExpress.Web.ASPxGridView;
using SAHL.Web.Controls;

namespace SAHL.Web.Views.Common
{
	public partial class BaselSummary : SAHLCommonBaseView, IBaselSummary
	{
		/// <summary>
		/// On Page Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (IsMenuPostBack)
            {
                grdBehaviouralScores.ClearSort();

                foreach (GridViewDataColumn col in grdBehaviouralScores.GetGroupedColumns())
                {
                    grdBehaviouralScores.UnGroup(col);
                }
            }
        }

		/// <summary>
		/// Bind BehaviouralScores
		/// </summary>
		/// <param name="behaviouralScoresTable"></param>
		public void BindBehaviouralScores(System.Data.DataTable behaviouralScoresTable)
		{
			grdBehaviouralScores.Columns.Clear();
			grdBehaviouralScores.AutoGenerateColumns = false;
            grdBehaviouralScores.AddGridColumn("BSMonth", "Month", 30, SAHL.Common.Web.UI.Controls.GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            grdBehaviouralScores.AddGridColumn("BehaviouralScore", "Behavioural Score", 30, SAHL.Common.Web.UI.Controls.GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            grdBehaviouralScores.AddGridColumn("Description", "Risk Category", 30, SAHL.Common.Web.UI.Controls.GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            grdBehaviouralScores.AddGridColumn("ThresholdColour", "ThresholdColour", 30, SAHL.Common.Web.UI.Controls.GridFormatType.GridString, null, HorizontalAlign.Left, false, false);
            grdBehaviouralScores.SettingsPager.PageSize = 20;
            grdBehaviouralScores.Settings.ShowGroupPanel = true;
			grdBehaviouralScores.DataSource = behaviouralScoresTable;
			grdBehaviouralScores.DataBind();
		}

		/// <summary>
		/// On New Row Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void OnRowDataBound(object sender, ASPxGridViewTableRowEventArgs e)
		{
            if (e.RowType == GridViewRowType.Data && e.Row.Cells.Count > 1) 
			{
                if (!String.IsNullOrEmpty(e.GetValue("ThresholdColour").ToString()))
                    e.Row.Cells[2].Attributes.Add("style", "background-color:" + e.GetValue("ThresholdColour").ToString());
			}
		}
	}
}
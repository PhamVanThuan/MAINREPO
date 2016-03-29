using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.AffordabilityAssessment
{
    /// <summary>
    ///
    /// </summary>
    public partial class History : SAHLCommonBaseView, IHistory
    {
        #region Protected Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ViewAssessmentButton_Click(object sender, EventArgs e)
        {
            if (OnViewAssessmentButtonClicked != null)
            {
                SelectedAffordabilityAssessmentKey = Convert.ToInt32(AffordabilityAssessmentsGrid.SelectedRow.Cells[0].Text);
                OnViewAssessmentButtonClicked(sender, e);
            }
        }

        #endregion Protected Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="affordabilityAssessments"></param>
        public void BindAffordabilityAssessmentsGrid(IEnumerable<AffordabilityAssessmentSummaryModel> affordabilityAssessments)
        {
            AffordabilityAssessmentsGrid.HeaderCaption = AffordabilityAssessmentGridHeaderCaption;
            AffordabilityAssessmentsGrid.EmptyDataSetMessage = "There are no " + AffordabilityAssessmentGridHeaderCaption.ToLower() + "."; 

            AffordabilityAssessmentsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("GenericKey", "Application Key", Unit.Percentage(10), HorizontalAlign.Left, ShowApplicationKeyInGrid);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("DateLastAmended", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Modified Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("ClientDetail", "Contributors", Unit.Percentage(50), HorizontalAlign.Left, true);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("AssessmentStatus", "Status", Unit.Percentage(10), HorizontalAlign.Left, true); 
            AffordabilityAssessmentsGrid.AddGridBoundColumn("DateConfirmed", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Date Confirmed", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("", "Version", Unit.Percentage(10), HorizontalAlign.Left, ShowGeneralStatusInGrid);

            AffordabilityAssessmentsGrid.DataSource = affordabilityAssessments;
            AffordabilityAssessmentsGrid.DataBind();

            if (affordabilityAssessments.Count() > 0)
            {
                ButtonRow.Visible = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AffordabilityAssessmentsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AffordabilityAssessmentSummaryModel ro = e.Row.DataItem as AffordabilityAssessmentSummaryModel;

                e.Row.Cells[0].Text = ro.Key.ToString();
                e.Row.Cells[1].Text = ro.GenericKey.ToString();
                e.Row.Cells[2].Text = ro.DateLastAmended.ToString();
                e.Row.Cells[3].Text = ro.ClientDetail.ToString();
                e.Row.Cells[4].Text = ro.AssessmentStatus;
                e.Row.Cells[5].Text = ro.DateConfirmed.ToString();
                e.Row.Cells[6].Text = ro.GeneralStatusKey == (int)GeneralStatuses.Active ? "Current" : "Previous";
            }
        }

        public int SelectedAffordabilityAssessmentKey { get; set; }

        public event EventHandler OnViewAssessmentButtonClicked;

        public bool ShowApplicationKeyInGrid { get; set; }

        public bool ShowGeneralStatusInGrid { get; set; }

        public string AffordabilityAssessmentGridHeaderCaption { get; set; }
    }
}
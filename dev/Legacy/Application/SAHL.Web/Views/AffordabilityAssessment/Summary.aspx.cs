using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.AffordabilityAssessment
{
    /// <summary>
    ///
    /// </summary>
    public partial class Summary : SAHLCommonBaseView, ISummary
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="affordabilityAssessments"></param>
        public void BindAffordabilityAssessmentsGrid(IEnumerable<AffordabilityAssessmentSummaryModel> affordabilityAssessments)
        {
            AffordabilityAssessmentsGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Modified Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("", "Contributors", Unit.Percentage(60), HorizontalAlign.Left, true);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            AffordabilityAssessmentsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Date Confirmed", false, Unit.Percentage(15), HorizontalAlign.Center, true);

            AffordabilityAssessmentsGrid.DataSource = affordabilityAssessments;
            AffordabilityAssessmentsGrid.DataBind();
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
                e.Row.Cells[1].Text = ro.DateLastAmended.ToString();
                e.Row.Cells[2].Text = ro.ClientDetail.ToString();
                e.Row.Cells[3].Text = ro.AssessmentStatus;
                e.Row.Cells[4].Text = ro.DateConfirmed.ToString();
            }
        }
    }
}
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
    public partial class Delete : SAHLCommonBaseView, IDelete
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
        protected void DeleteAssessmentButton_Click(object sender, EventArgs e)
        {
            if (OnDeleteAssessmentButtonClicked != null)
            {
                SelectedUnconfirmedAffordabilityAssessmentKey = Convert.ToInt32(UnconfirmedAssessmentGrid.SelectedRow.Cells[0].Text);
                OnDeleteAssessmentButtonClicked(sender, e);
            }
        }

        #endregion Protected Members

        public int SelectedUnconfirmedAffordabilityAssessmentKey { get; set; }

        public event EventHandler OnDeleteAssessmentButtonClicked;

        public void BindUnconfirmedGrid(IEnumerable<AffordabilityAssessmentSummaryModel> unconfirmedAffordabilityAssessments)
        {
            UnconfirmedAssessmentGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            UnconfirmedAssessmentGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Modified Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            UnconfirmedAssessmentGrid.AddGridBoundColumn("", "Contributors", Unit.Percentage(60), HorizontalAlign.Left, true);
            UnconfirmedAssessmentGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);

            UnconfirmedAssessmentGrid.DataSource = unconfirmedAffordabilityAssessments;
            UnconfirmedAssessmentGrid.DataBind();

            if (unconfirmedAffordabilityAssessments.Count() > 0)
            {
                ButtonRow.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AssessmentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AffordabilityAssessmentSummaryModel ro = e.Row.DataItem as AffordabilityAssessmentSummaryModel;

                e.Row.Cells[0].Text = ro.Key.ToString();
                e.Row.Cells[1].Text = ro.DateLastAmended.ToString();
                e.Row.Cells[2].Text = ro.ClientDetail;
                e.Row.Cells[3].Text = ro.AssessmentStatus;
            }
        }
    }
}
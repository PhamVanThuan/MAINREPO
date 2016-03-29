using System;
using Microsoft.Reporting.WebForms;
using SAHL.Common.Web.UI;

//using SAHL.Common.BusinessModel.Repositories.Schemas;


namespace SAHL.Web.Views.ReleaseAndVariations
{


    /// <summary>
    /// 
    /// </summary>
    public partial class Report : SAHLCommonBaseView, Interfaces.IReleaseAndVariationsReport
    {

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnCancelClicked;


        /// <summary>
        /// Cancel The Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        public bool ShowbtnCancel
        {
            set { btnCancel.Visible = value; }
        }  

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            EnableViewState = true;
        }

        /// <summary>
        /// Adds the Report to the Presenter
        /// </summary>
        /// <param name="Report"></param>
        public void AddReport(ReportViewer Report)
        {
            if (Report != null) Panel1.Controls.Add(Report);
        }
  
    }
}
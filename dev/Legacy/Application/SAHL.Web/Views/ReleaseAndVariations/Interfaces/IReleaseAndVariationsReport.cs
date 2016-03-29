using System;
using Microsoft.Reporting.WebForms;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReleaseAndVariationsReport : IViewBase
    {

        event EventHandler btnCancelClicked;

        /// <summary>
        /// Adds the Report to the Presenter
        /// </summary>
        /// <param name="Report"></param>
        void AddReport(ReportViewer Report);


        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        bool ShowbtnCancel { set;}
        

    }
}

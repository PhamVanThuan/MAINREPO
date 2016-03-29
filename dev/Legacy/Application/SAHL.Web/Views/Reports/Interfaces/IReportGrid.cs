using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Reports.Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public interface IReportGrid : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnGoButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnExportButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        DataTable ResultsTableToBind { set;}

        /// <summary>
        /// 
        /// </summary>
        string ExportData { set;}

        /// <summary>
        /// 
        /// </summary>
        string ReportName { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowEmailPanel { set;}

        /// <summary>
        /// 
        /// </summary>
        bool Cancelled { get;}       

        /// <summary>
        /// Gets/Sets the record count for the report query
        /// </summary>
        long RecordCount { get;set;}
    }
}

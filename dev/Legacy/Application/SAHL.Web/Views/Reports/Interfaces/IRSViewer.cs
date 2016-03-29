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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Reports.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportParametersEventArgs : EventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        private IDictionary<IReportParameter, object> _parameters;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IReportParameter, object> Parameters
        {
            get { return _parameters; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReportParametersEventArgs()
        {
            _parameters = new Dictionary<IReportParameter, object>();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRSViewer : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ReportParametersEventArgs> onViewButtonClicked;


        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OntblReportsSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnReportsGroupChanged;


        #region Properties

        /// <summary>
        /// Sets whether Cancel Button is enabled
        /// </summary>
        bool CancelButtonEnable { set;}

        /// <summary>
        /// Sets whether View Button is enabled
        /// </summary>
        bool ViewButtonEnable { set;}

        /// <summary>
        /// Sets whether Available Reports Grid is shown
        /// </summary>
        bool ReportsGridShow { set;}

        /// <summary>
        /// Sets whether Parameters Panel is shown
        /// </summary>
        bool pnlParametersShow { set;}

        /// <summary>
        /// Sets whether Reports Data grid is shown
        /// </summary>
        bool grdDataReportShow { set;}

        /// <summary>
        /// Gets the key of the Report Group selected on the view.
        /// </summary>
        string SelectedReportGroup { get; set;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedReportType { get;}

        int SelectedReportGroupIndex { get;}


        /// <summary>
        /// 
        /// </summary>
        IReportStatement SelectedReportStatement { get;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedReportName { get;set;}

        /// <summary>
        /// 
        /// </summary>
        IList<string> OriginalReportParameters { get;set;}

        /// <summary>
        /// 
        /// </summary>
        int tblReportsSelectedIndex { get;set;}
       

        #endregion

        #region Methods

        /// <summary>
        /// Binds Report Group to Drop Down
        /// </summary>
        /// <param name="reportGroups">lstReport</param>
        void BindReportGroup(IEventList<IReportGroup> reportGroups);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstReports"></param>
        void BindReportStatement(IEventList<IReportStatement> lstReports);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportParms"></param>
        /// <param name="reportstatements"></param>
        void BindReportParameterList(IList<IReportParameter> reportParms, IEventList<IReportStatement> reportstatements);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportParms"></param>
        /// <param name="reportstatements"></param>
        void BuildParameterControls(IList<IReportParameter> reportParms, IEventList<IReportStatement> reportstatements);   

        #endregion

    }
}

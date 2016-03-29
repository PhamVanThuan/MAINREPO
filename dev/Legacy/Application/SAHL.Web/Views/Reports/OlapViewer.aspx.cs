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
using Microsoft.Reporting.WebForms;
using System.Globalization;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OlapViewer : SAHLCommonBaseView,IOlapViewer
    {
        private string _reportText;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
        }
        

        #region IOlapViewer Members

        public event EventHandler OnSubmitButtonClicked;
      
        public string ReportText
        {
            set { _reportText = value; }
        }

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();
            IReportStatement rs = rr.GetReportStatementByKey(int.Parse(_reportText));
            string rT = rr.GetUIStatementText(rs).ToString();
            rT = rT.Replace(Convert.ToChar(34), Convert.ToChar(39));
            string reportText = @"<object classid=""clsid:0002E55A-0000-0000-C000-000000000046"" id=""PivotTable"" width=""99%"" height=""400"" class=""borderAll""> <param name=""XMLData"" value=""" + rT + @""" /></object>";
            divReportView.InnerHtml = reportText;

        }
    }
}
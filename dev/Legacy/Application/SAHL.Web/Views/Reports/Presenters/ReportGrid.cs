using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using System.Text;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Xml.Xsl;
using System.Threading;
using System.Net.Mail;
using SAHL.Common.Authentication;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
//using SAHL.Common.Caching;


namespace SAHL.Web.Views.Reports.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportGrid : SAHLCommonBasePresenter<IReportGrid>
    {
        const string ReportStatementData = "REPORTSTATEMENTDATA";
        const string ReportParameterData = "REPORTPARAMETERDATA";
        private IReportStatement _reportStatement;
        Dictionary<SAHL.Common.BusinessModel.Interfaces.IReportParameter, object> _params;
        private long _recordCount;

        DataTable _tblReportData;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReportGrid(IReportGrid view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

       
        protected override void OnViewInitialised(object sender, EventArgs e)
        {           
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;         
            try
            {
                _view.OnExportButtonClicked += new EventHandler(_view_OnExportButtonClicked);
                _view.OnGoButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnGoButtonClicked);
                _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
                if (GlobalCacheData.ContainsKey(ViewConstants.ReportParameters))
                {
                    if (GlobalCacheData.ContainsKey(ViewConstants.ReportStatement))
                    {
                        _reportStatement = GlobalCacheData[ViewConstants.ReportStatement] as IReportStatement;
                    }
                    _params = GlobalCacheData[ViewConstants.ReportParameters] as Dictionary<SAHL.Common.BusinessModel.Interfaces.IReportParameter, object>;
                    IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();
                    ILookupRepository lr = RepositoryFactory.GetRepository<ILookupRepository>();
                    if (!_view.Cancelled)
                    {
                        _tblReportData = rr.ExecuteSqlReport(_params, _reportStatement);
                        double maxRecords = lr.Controls.ObjectDictionary["DataReportMaxRecords"].ControlNumeric.Value;
                        _recordCount = _tblReportData.Rows.Count;
                        _view.RecordCount = _recordCount;
                        if (_recordCount > maxRecords)
                        {
                            _view.ShowEmailPanel = true;
                        }
                        else
                        {
                            _view.ResultsTableToBind = _tblReportData;
                            _view.ReportName = _reportStatement.StatementName;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("Timeout expired") || exc.InnerException.Message.Contains("timeout"))
                {

                    _view.ShowEmailPanel = true;
                }
                else
                {
                    throw;
                }

            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("RSViewer");
        }

        void _view_OnGoButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            string addresses = e.Key as string;

            IBulkBatchRepository bbr = RepositoryFactory.GetRepository<IBulkBatchRepository>();
            ILookupRepository lr = RepositoryFactory.GetRepository<ILookupRepository>();

            IBulkBatch bb = bbr.GetEmptyBulkBatch();
            bb.BulkBatchStatus = lr.BulkBatchStatuses.ObjectDictionary[Convert.ToInt32(BulkBatchStatuses.ReadyforProcessing).ToString()];
            bb.Description = _reportStatement.StatementName;
            bb.BulkBatchType = lr.BulkBatchTypes.ObjectDictionary[Convert.ToInt32(BulkBatchTypes.DataReportBatch).ToString()];
            bb.EffectiveDate = DateTime.Now;
            bb.UserID = _view.CurrentPrincipal.Identity.Name;

            //Create email address parameter
            IBulkBatchParameter bbp = bbr.GetEmptyBulkBatchParameter();
            bbp.ParameterName = "MailAddress";
            bbp.ParameterValue = addresses;
            bbp.BulkBatch = bb;
            bb.BulkBatchParameters.Add(_view.Messages, bbp);

            //Add report key parameter
            IBulkBatchParameter bbp2 = bbr.GetEmptyBulkBatchParameter();
            bbp2.ParameterName = "ReportKey";
            bbp2.ParameterValue = _reportStatement.Key.ToString();
            bbp2.BulkBatch = bb;
            bb.BulkBatchParameters.Add(_view.Messages, bbp2);


            foreach (IReportParameter p in _params.Keys)
            {
                object val = _params[p];
                IBulkBatchParameter bbp3 = bbr.GetEmptyBulkBatchParameter();
                bbp3.ParameterName = p.ParameterName;
                if (p != null)
                {
                    if (p.ReportParameterType.Key == (int)SAHL.Common.Globals.ReportParameterTypes.DateTime)
                    {
                        bbp3.ParameterValue = ((DateTime)val).Ticks.ToString();
                    }
                    else
                    {
                        bbp3.ParameterValue = val.ToString();
                    }
                }
                else
                {
                    bbp3.ParameterValue = string.Empty;
                }
                bbp3.BulkBatch = bb;
                bb.BulkBatchParameters.Add(_view.Messages, bbp3);
            }

            TransactionScope txn = new TransactionScope();
            try
            {
                bbr.SaveBulkBatch(bb);                
                string extractJobName = lr.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Batch.DataReportJob].ControlText;
                if (!string.IsNullOrEmpty(extractJobName))
                {
                    ISqlAgentService sqlService = ServiceFactory.GetService<ISqlAgentService>();
                    sqlService.StartSQLServerAgentJob(extractJobName);
                }

            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

           
            _view.Navigator.Navigate("RSViewer");
        }

        void _view_OnExportButtonClicked(object sender, EventArgs e)
        {
            if (_tblReportData != null)
            {
                IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();
                string data = rr.ExportDataReportToExcel(_tblReportData, _reportStatement);
                _view.ExportData = data;

            }
        }
    }



}




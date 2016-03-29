using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.WebServices.ReportExecution2005;
using System.Web.Services.Protocols;
using System.Data.SqlClient;

namespace SAHL.Common.Service
{
    /// <summary>
    /// 
    /// </summary>
    [FactoryType(typeof(IReportExecution2005Service))]
    public class ReportExecution2005Service : IReportExecution2005Service
    {
        private ReportExecutionService _rsExec;

        /// <summary>
        /// 
        /// </summary>
        public ReportExecution2005Service()
        {
            _rsExec = new ReportExecutionService();
            _rsExec.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _rsExec.Url = Properties.Settings.Default.ReportExecutionURL;
        }

        public byte[] CreateReport(string reportPath, string format, List<SqlParameter> parameters)
        {
            // Render arguments
            byte[] result = null;
            string historyID = null;
            string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

            // Prepare report parameter.
            ParameterValue[] prms = new ParameterValue[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                prms[i] = new ParameterValue();
                prms[i].Name = parameters[i].ParameterName;
                prms[i].Value = parameters[i].Value.ToString();
            }

            //DataSourceCredentials[] credentials = null;
            //string showHideToggle = null;
            string encoding;
            string mimeType;
            string extension;
            Warning[] warnings = null;
            //ParameterValue[] reportHistoryParameters = null;
            string[] streamIDs = null;

            //ExecutionInfo execInfo = new ExecutionInfo();
            //ExecutionHeader execHeader = new ExecutionHeader();
            //_rsExec.ExecutionHeaderValue = execHeader;
            //execInfo = _rsExec.LoadReport(reportPath, historyID);
            _rsExec.LoadReport(reportPath, historyID);
            _rsExec.SetExecutionParameters(prms, "en-us");
            //String SessionId = _rsExec.ExecutionHeaderValue.ExecutionID;
            //Console.WriteLine("SessionID: {0}", _rsExec.ExecutionHeaderValue.ExecutionID);
            try
            {
                result = _rsExec.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
                //execInfo = _rsExec.GetExecutionInfo();
                //Console.WriteLine("Execution date and time: {0}", execInfo.ExecutionDateTime);
            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Detail.OuterXml);
                throw;
            }

            return result;
        }
    }
}

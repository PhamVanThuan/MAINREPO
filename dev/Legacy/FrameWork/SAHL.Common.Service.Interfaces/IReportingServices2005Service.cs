using System.Collections.Generic;
using System.Data.SqlClient;

namespace SAHL.Common.Service.Interfaces
{
    public interface IReportExecution2005Service
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="reportPath"></param>
        /// <param name="format"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        byte[] CreateReport(string reportPath, string format, List<SqlParameter> parameters);
    }
}
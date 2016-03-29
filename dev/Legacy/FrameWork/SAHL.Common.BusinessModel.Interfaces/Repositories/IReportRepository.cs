using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// A repository responsible for functionality surrounding reports.
    /// </summary>
    public interface IReportRepository
    {
        /// <summary>
        /// Gets a ReportStatement object by ReportStatementKey.
        /// </summary>
        /// <param name="Key">The integer ReportStatementKey.</param>
        /// <returns>The <see cref="IReportStatement">reportstatement object found using the supplied key, returns null if no reportstatement is found.</see></returns>
        IReportStatement GetReportStatementByKey(int Key);

        /// <summary>
        /// Gets a ReportParameter object by ReportParameterKey.
        /// </summary>
        /// <param name="Key">The integer ReportParameterKey.</param>
        /// <returns>The <see cref="IReportParameter">reportparameter object found using the supplied key, returns null if no reportparameter is found.</see></returns>
        IReportParameter GetReportParameterByKey(int Key);

        /// <summary>
        /// Gets a ReportStatement objects by ReportName.
        /// </summary>
        /// <param name="ReportName">The string ReportName.</param>
        /// <returns>The <see cref="IReportStatement">reportstatement object found using the supplied keys, returns null if no reportstatement is found.</see></returns>
        IEventList<IReportStatement> GetReportStatementByName(string ReportName);

        /// <summary>
        /// Gets a ReportStatement object by ReportName &amp; OriginationSourceProductKey.
        /// </summary>
        /// <param name="ReportName">The string ReportName.</param>
        /// <param name="OriginationSourceProductKey">The integer OriginationSourceProductKey.</param>
        /// <returns>The <see cref="IReportStatement">reportstatement object found using the supplied keys, returns null if no reportstatement is found.</see></returns>
        IReportStatement GetReportStatementByNameAndOSP(string ReportName, int OriginationSourceProductKey);

        IReadOnlyEventList<IReportStatement> GetReportStatementByReportGroupKey(int ReportGroupKey);

        IReadOnlyEventList<IReportParameter> GetReportParametersByReportStatementKey(int ReportStatementKey);

        //IList<NameValue> GetReportParameterValidValues( int ReportStatementKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="reportStatement"></param>
        /// <returns></returns>
        DataTable ExecuteSqlReport(Dictionary<SAHL.Common.BusinessModel.Interfaces.IReportParameter, object> dict, IReportStatement reportStatement);

        /// <summary>
        ///
        /// </summary>
        /// <param name="reportStatement"></param>
        /// <returns></returns>
        string GetUIStatementText(IReportStatement reportStatement);

        string ExportDataReportToExcel(DataTable tblReportData, IReportStatement reportStatement);

        IReportParameter CreateReportParameter();

        byte[] RenderSQLReport(string reportPath, IDictionary<string, string> parameterNamesAndValues, out string renderErrorMessage);

        byte[] RenderSQLReport(string reportPath, IDictionary<string, string> parameterNamesAndValues, string ReportFormat, out string renderErrorMessage);

        string GeneratePDFReport(int reportStatementKey, IDictionary<string, string> parameterNamesAndValues, out string renderErrorMessage);
    }
}
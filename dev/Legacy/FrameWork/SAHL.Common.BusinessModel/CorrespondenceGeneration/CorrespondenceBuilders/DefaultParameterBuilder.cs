using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders
{
    public class DefaultParameterBuilder : IReportParametersBuilder
    {
        #region IReportParametersBuilder Members

        public void GetReportParameters(int p_ReportStatementKey, List<ReportDataParameter> p_ReportParameters)
        {
            // Get the Report Repository
            IReportRepository ReportRepo = RepositoryFactory.GetRepository<IReportRepository>();

            // Get the ReportStatement Object using the ReportStatementKey
            IReportStatement _reportStatement = ReportRepo.GetReportStatementByKey(p_ReportStatementKey);

            // Get the ReportParameters
            if (_reportStatement.ReportParameters != null)
            {
                foreach (IReportParameter _reportParameter in _reportStatement.ReportParameters)
                {
                    ReportDataParameter rp = new ReportDataParameter(_reportParameter.Key, _reportParameter.ParameterName, null);
                    p_ReportParameters.Add(rp);

                }
            }
        }

        public void GetReportParameters(ReportData reportData)
        {
            // Get the Report Repository
            IReportRepository ReportRepo = RepositoryFactory.GetRepository<IReportRepository>();

            // Get the ReportStatement Object using the ReportStatementKey
            IReportStatement _reportStatement = ReportRepo.GetReportStatementByKey(reportData.ReportStatementKey);

            // Get the ReportParameters
            if (_reportStatement.ReportParameters != null)
            {
                foreach (IReportParameter _reportParameter in _reportStatement.ReportParameters)
                {
                    ReportDataParameter rp = new ReportDataParameter(_reportParameter.Key, _reportParameter.ParameterName, null);
                    reportData.ReportParameters.Add(rp);

                }
            }

        }

        #endregion
    }
}

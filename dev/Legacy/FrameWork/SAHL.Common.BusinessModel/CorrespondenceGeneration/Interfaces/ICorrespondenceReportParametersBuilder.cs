using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;

namespace SAHL.Common.BusinessModel.CorrespondenceGeneration.Interfaces
{
    public interface IReportParametersBuilder
    {
        void GetReportParameters(int p_ReportStatementKey, List<ReportDataParameter> p_ReportParameters);
    }
}

using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface ISqlReportParameter : IReportParameter
    {
        bool IsInternalParameter { get; }

        List<string> DefaultValues { get; }
    }
}
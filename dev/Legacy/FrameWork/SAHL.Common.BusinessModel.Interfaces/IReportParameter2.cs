using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IReportParameter
    {
        Dictionary<string, object> ValidValues { get; }
    }
}
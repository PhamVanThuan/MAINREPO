using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IReportStatement : IEntityValidation
    {
        List<IReportParameter> ReportParameters { get; set; }

        //Dictionary<string, object> GetParameterValidValues(IReportParameter parameter);

        //string[] GetDefaultValues(IReportParameter parameter);

        //TODO: Andrew
        //        DataTable ExecuteUIStatement(ParameterCollection parameters);
    }
}
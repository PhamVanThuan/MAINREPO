using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Query.Parsers.Elements
{
    public interface IWherePart
    {
        string ClauseOperator { get; set; }
        string Field { get; set; }
        string ParameterName { get; set; }
        string Value { get; set; }
        string Operator { get; set; }
        List<IWherePart> Where { get; set; }
    }
}
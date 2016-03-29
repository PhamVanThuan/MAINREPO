using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers.Elemets
{

    public class WherePart : IWherePart
    {
        public string ClauseOperator { get; set; }
        public string Field { get; set; }
        public string ParameterName { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public List<IWherePart> Where { get; set; } 
    }

}
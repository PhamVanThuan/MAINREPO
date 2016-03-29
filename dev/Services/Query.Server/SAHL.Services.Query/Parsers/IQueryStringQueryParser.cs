using System.Collections.Generic;
using System.Collections.Specialized;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers
{
    public interface IQueryStringQueryParser
    {
        IFindQuery FindManyQuery(NameValueCollection input);
        List<IWherePart> GetWhereParts(NameValueCollection input);
        ILimitPart GetLimitPart(NameValueCollection input);
        string GetColumnName(string key);
    }
}
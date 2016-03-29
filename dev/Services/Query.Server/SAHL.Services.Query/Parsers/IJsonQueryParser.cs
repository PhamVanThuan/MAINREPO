using System.Collections.Generic;
using System.Collections.Specialized;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers
{
    public interface IJsonQueryParser
    {
        IFindQuery FindManyQuery(string jsonInput);
    }
}
using System.Collections.Generic;

namespace SAHL.Services.Query.Controllers
{
    public interface IDataManagerRetriever
    {
        DataManagerQueryResult Get(string routeTemplate, IDictionary<string, object> routeValues);
    }
}
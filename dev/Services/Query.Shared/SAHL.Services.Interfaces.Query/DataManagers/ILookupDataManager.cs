using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;

namespace SAHL.Services.Interfaces.Query.DataManagers
{
    public interface ILookupDataManager
    {
        IEnumerable<ILookupDataModel> GetLookups(IFindQuery findQuery, string database, string schema, string lookupType, string keyColumn,string descriptionColumn);
        ILookupDataModel GetLookup(IFindQuery findQuery, string database, string schema, string lookupType, string keyColumn, string descriptionColumn, int keyValue);
    }
}
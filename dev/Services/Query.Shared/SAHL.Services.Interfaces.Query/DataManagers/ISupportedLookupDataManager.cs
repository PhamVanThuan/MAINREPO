using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;

namespace SAHL.Services.Interfaces.Query.DataManagers
{
    public interface ISupportedLookupDataManager
    {
        IEnumerable<ISupportedLookupModel> GetSupportedLookups();
        ILookupMetaDataModel GetLookupSchema(string lookupTableName);
    }
}
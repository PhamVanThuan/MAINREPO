using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Interfaces.Query.Helpers
{
    public interface ILookupTypesHelper
    {
        Dictionary<string, ISupportedLookup> ValidLookupTypes { get; }
        bool IsValidLookupType(string lookupType);
        ILookupMetaDataModel FindLookupMetaData(string lookupType);
        void LoadValidLookupTypes();
    }
}

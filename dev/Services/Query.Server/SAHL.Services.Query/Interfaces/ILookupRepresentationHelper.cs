using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Lookup;

namespace SAHL.Services.Query.Interfaces
{
    public interface ILookupRepresentationHelper
    {
        LookupTypeListRepresentation GetLookupTypesRepresentation();
        LookupListRepresentation GetLookupsRepresentation(string lookupType, IFindQuery findManyQuery);
        LookupRepresentation GetLookupRepresentation(string lookupType, int id, IFindQuery findManyQuery);

    }
}
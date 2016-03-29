using SAHL.Core.Data;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.DataManagers.Statements.Lookup
{
    public class GetSupportedLookupsStatement : ISqlStatement<SupportedLookupModel>
    {
        public string GetStatement()
        {
            return @"
                SELECT 
                    LookupKey = Lower(TABLE_NAME),
                    LookupTable = TABLE_NAME
                FROM INFORMATION_SCHEMA.COLUMNS
                Group By TABLE_NAME
                Having Count(ORDINAL_POSITION) = 2
            ";
        }
         
    }

}
using SAHL.Core.Data;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.DataManagers.Statements.Lookup
{
    public class GetSchemaStatement : ISqlStatement<LookupMetaDataModel>
    {
        public string LookupTableName { get; private set; }
        
        public GetSchemaStatement(string lookupTableName)
        {
            LookupTableName = lookupTableName;
        }

        public string GetStatement()
        {
            return @"
                SELECT LookupType = TABLE_NAME,
                        Db = TABLE_CATALOG, 
                        [Schema] = TABLE_SCHEMA,
                        LookupTable = TABLE_NAME,
                        LookupKey = COLUMN_NAME,
                        LookupDescription = (
                            Select COLUMN_NAME
                            From INFORMATION_SCHEMA.COLUMNS
                            Where TABLE_NAME = @LookupTableName And ORDINAL_POSITION = 2
                        )
                FROM INFORMATION_SCHEMA.COLUMNS T
                WHERE T.TABLE_NAME = @LookupTableName
                And ORDINAL_POSITION = 1
            ";
        }
    }
}

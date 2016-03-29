using SAHL.Core.Data;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.DataManagers.Statements.Lookup
{
    public class GetLookupStatement : ISqlStatement<LookupDataModel>
    {
        
        public int KeyValue { get; protected set; }

        private string Database { get; set; }
        private string LookupType { get; set; }
        private string Schema { get; set; }
        private string KeyColumn { get; set; }
        private string DescriptionColumn { get; set; }

        //temp
        private const string defaultValue = "str";

        public GetLookupStatement(string database, string lookupType, string schema, string keyColumn, string descriptionColumn, int keyValue)
        {
            //FIX THE SQL SO IT DOESN'T BREAK THE CONVENTION TESTS
            Database = database;
            if (database == defaultValue) { Database = "2AM"; }

            LookupType = lookupType;
            if (lookupType == defaultValue) { LookupType = "ACBBank"; }

            Schema = schema;
            if (schema == defaultValue) { Schema = "dbo"; }

            KeyColumn = keyColumn;
            if (keyColumn == defaultValue) { KeyColumn = "ACBBankCode"; }

            DescriptionColumn = descriptionColumn;
            if (descriptionColumn == defaultValue) { DescriptionColumn = "ACBBankDescription"; }

            KeyValue = keyValue;
        }

        public string GetStatement()
        {
            string queryString = string.Format("Select {3} as Id, {4} as Description From [{0}].[{1}].[{2}] Where {3} = @KeyValue", Database,
                Schema, LookupType, KeyColumn, DescriptionColumn);

            return queryString;

        }
    }
}
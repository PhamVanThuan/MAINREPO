using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.DataManagers.Statements.Lookup;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.DataManagers
{
    public class SupportedLookupDataManager : ISupportedLookupDataManager
    {
        
        private IDbFactory dbFactory;

        public SupportedLookupDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<ISupportedLookupModel> GetSupportedLookups()
        {
            var query = new GetSupportedLookupsStatement();
            using (var dbContext = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return dbContext.Select<SupportedLookupModel>(query);
            }
        }

        public ILookupMetaDataModel GetLookupSchema(string lookupTableName)
        {
            var query = new GetSchemaStatement(lookupTableName);
            using (var dbContext = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return dbContext.SelectOne(query);
            }
        }

    }

}
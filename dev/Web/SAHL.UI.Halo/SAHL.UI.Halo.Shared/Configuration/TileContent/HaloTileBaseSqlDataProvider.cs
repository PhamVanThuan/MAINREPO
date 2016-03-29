using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileBaseSqlDataProvider
    {
        private readonly IDbFactory dbFactory;

        protected HaloTileBaseSqlDataProvider(IDbFactory dbFactory)
        {
            if (dbFactory == null) { throw new ArgumentNullException("dbFactory"); }
            this.dbFactory = dbFactory;
        }

        public virtual string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Empty;
        }

        public virtual string GetSqlStatement(BusinessContext businessContext, params object[] additionalParameters)
        {
            return string.Empty;
        }

        protected dynamic RetrieveSqlDataRecord<T>(BusinessContext businessContext, params string[] additionalParameters)
        {
            if (businessContext == null) { throw new ArgumentNullException("businessContext"); }

            var contentSqlStatement = additionalParameters == null || !additionalParameters.Any()
                                        ? this.GetSqlStatement(businessContext)
                                        : this.GetSqlStatement(businessContext, additionalParameters);
            if (string.IsNullOrWhiteSpace(contentSqlStatement)) { return null; } 

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                dynamic result = db.Select<T>(contentSqlStatement);
                return result;
            }
        }

        protected IEnumerable<dynamic> RetrieveSqlDataRecords<T>(BusinessContext businessContext)
        {
            if (businessContext == null) { throw new ArgumentNullException("businessContext"); }

            var contentSqlStatement = this.GetSqlStatement(businessContext);
            if (string.IsNullOrWhiteSpace(contentSqlStatement)) { return null; } 

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                dynamic result = db.Select<T>(contentSqlStatement);
                return result;
            }
        }

        protected void Update<T>(BusinessContext businessContext, T updateDataModel) where T : IHaloTileModel
        {
            if (businessContext == null) { throw new ArgumentNullException("businessContext"); }

            var sqlStatement = this.GetSqlStatement(businessContext);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.ExecuteSqlStatement(sqlStatement, updateDataModel);
                db.Complete();
            }
        }
    }
}

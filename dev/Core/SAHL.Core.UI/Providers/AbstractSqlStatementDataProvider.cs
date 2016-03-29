using SAHL.Core.BusinessModel;
using SAHL.Core.Data;

namespace SAHL.Core.UI.Providers
{
    public abstract class AbstractSqlStatementDataProvider<T> : ISqlStatementDataProvider<T>
    {
        private string sqlStatement;
        private IDbFactory dbFactory;

        public AbstractSqlStatementDataProvider(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public dynamic GetData(BusinessModel.BusinessKey businessKey)
        {
            this.sqlStatement = this.GetStatement(businessKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                dynamic result = db.Select<T>(this);

                return result;
            }
        }

        public abstract string GetStatement(BusinessKey businessKey);

        public string GetStatement()
        {
            return this.sqlStatement;
        }
    }
}
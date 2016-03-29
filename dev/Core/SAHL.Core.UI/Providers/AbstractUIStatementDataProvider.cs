using SAHL.Core.Data;

namespace SAHL.Core.UI.Providers
{
    public abstract class AbstractUIStatementDataProvider<T> : ISqlUIStatementDataProvider<T>
        where T : IDataModel
    {
        private IDbFactory dbFactory;

        public AbstractUIStatementDataProvider(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public dynamic GetData(BusinessModel.BusinessKey businessKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                dynamic result = db.GetByKey<T, long>(businessKey.Key);

                return result;
            }
        }
    }
}
using SAHL.Core.Data;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication.Statements;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication
{
    public class ComcorpApplicationDataManager : IComcorpApplicationDataManager
    {
        private IDbFactory dbFactory;

        public ComcorpApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int? GetApplicationNumberForApplicationCode(long applicationCode)
        {
            var sql = new GetAppNumberForComcorpAppCodeStatement(applicationCode);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<int?>(sql);
            }
        }
    }
}
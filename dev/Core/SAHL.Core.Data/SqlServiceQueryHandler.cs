using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Data
{
    public class SqlServiceQueryHandler<T, U> : IServiceQueryHandler<T> where T : ISqlServiceQuery<U>
    {
        private IIocContainer ioc;
        private IDbFactory dbFactory;

        public SqlServiceQueryHandler(IIocContainer ioc, IDbFactory dbFactory)
        {
            this.ioc = ioc;
            this.dbFactory = dbFactory;
        }

        public ISystemMessageCollection HandleQuery(T query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            // find the sql statement for the query from the ioc

            IServiceQuerySqlStatement<T, U> queryStatement = this.ioc.GetInstance<IServiceQuerySqlStatement<T, U>>();

            // execute the statement generically passing in the query object as the data parameter
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                // set the result to the query object
                query.Result = new ServiceQueryResult<U>(db.Select<U>(queryStatement.GetStatement(), query));
            }

            // TODO: Catch the exceptions and pass them back in messages

            return messages;
        }
    }
}
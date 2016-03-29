using SAHL.Core.Data;

namespace SAHL.Core.Logging.Loggers.Database.Specs.Fakes
{
    public class FakeDbFactoryFunc
    {
        private IDbFactory dbFactory;

        public FakeDbFactoryFunc(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IDbFactory Get()
        {
            return dbFactory;
        }
    }
}
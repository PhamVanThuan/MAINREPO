using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowSqlExceptionCommandHandler : IHandlesDomainServiceCommand<ThrowSqlExceptionCommand>
    {
        public ThrowSqlExceptionCommandHandler()
        {
        }

        public void Handle(IDomainMessageCollection messages, ThrowSqlExceptionCommand command)
        {
            DBHelper dbHelper = new DBHelper(Databases.TwoAM);
            dbHelper.ExecuteNonQuery("SELECT 1/0");
        }
    }
}
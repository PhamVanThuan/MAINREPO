using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowSqlTimeOutExceptionCommandHandler : IHandlesDomainServiceCommand<ThrowSqlTimeOutExceptionCommand>
    {
        public ThrowSqlTimeOutExceptionCommandHandler()
        {
        }

        public void Handle(IDomainMessageCollection messages, ThrowSqlTimeOutExceptionCommand command)
        {
            DBHelper dbHelper = new DBHelper(Databases.TwoAM);
            dbHelper.ExecuteNonQuery("WAITFOR DELAY '00:00:32:';");
        }
    }
}
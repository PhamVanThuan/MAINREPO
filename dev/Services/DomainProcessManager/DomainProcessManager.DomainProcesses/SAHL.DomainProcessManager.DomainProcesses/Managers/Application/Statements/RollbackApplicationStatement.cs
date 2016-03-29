using SAHL.Core.Data;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements
{
    public class RollbackApplicationStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }
        public string EmploymentKeys { get; protected set; }

        public RollbackApplicationStatement(int applicationNumber, string employmentKeys)
        {
            ApplicationNumber = applicationNumber;
            EmploymentKeys = employmentKeys;
        }

        public string GetStatement()
        {
            var sql = @"EXEC RollbackApplication @ApplicationNumber,@EmploymentKeys";
            return sql;
        }
    }
}

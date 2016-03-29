using SAHL.Core.Data;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Client.Statements
{
    public class DoesEmployerExistStatement : ISqlStatement<int?>
    {
        public string EmployerName { get; private set; }

        public DoesEmployerExistStatement(string employerName)
        {
            this.EmployerName = employerName;
        }

        public string GetStatement()
        {
            return @"SELECT EmployerKey FROM [2AM].dbo.Employer WHERE Name = @EmployerName";
        }
    }
}
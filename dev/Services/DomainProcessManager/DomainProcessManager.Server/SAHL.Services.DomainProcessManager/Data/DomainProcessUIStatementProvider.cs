using SAHL.Core.Data;

namespace SAHL.Services.DomainProcessManager.Data
{
    public class DomainProcessUIStatementProvider : IUIStatementsProvider
    {
        public const string domainprocess_insert = "insert into [dbo].[DomainProcess] ([DomainProcessId], [Data]) values (@DomainProcessId, @Data)";

        public string UIStatementContext
        {
            get { return "SAHL.Services.Interfaces.DomainProcessManager.Models"; }
        }
    }
}

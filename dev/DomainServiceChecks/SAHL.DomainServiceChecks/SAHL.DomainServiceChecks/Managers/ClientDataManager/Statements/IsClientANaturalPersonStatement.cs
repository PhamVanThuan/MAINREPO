using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.ClientDataManager.Statements
{
    public class IsClientANaturalPersonStatement : ISqlStatement<int>
    {
        public int ClientKey { get; protected set; }

        public IsClientANaturalPersonStatement(int clientKey)
        {
            this.ClientKey = clientKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT [LegalEntityTypeKey] FROM [2AM].[dbo].[LegalEntity] WHERE [LegalEntityKey] = @ClientKey";
            return query;
        }
    }
}

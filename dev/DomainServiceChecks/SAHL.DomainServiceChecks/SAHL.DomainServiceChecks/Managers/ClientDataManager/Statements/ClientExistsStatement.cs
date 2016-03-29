using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.ClientDataManager.Statements
{
    public class ClientExistsStatement : ISqlStatement<int>
    {
        public int ClientKey { get; protected set; }

        public ClientExistsStatement(int ClientKey)
        {
            this.ClientKey = ClientKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT count(1) FROM [2AM].[dbo].[LegalEntity] WHERE [LegalEntityKey] = @ClientKey";
            return query;
        }
    }
}

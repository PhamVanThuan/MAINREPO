using SAHL.Core.Data;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Client.Statements
{
    public class GetClientKeyForClientAddressStatement : ISqlStatement<int>
    {
        public int ClientAddressKey { get; set; }

        public GetClientKeyForClientAddressStatement(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
        }

        public string GetStatement()
        {
            return @"SELECT le.LegalEntityKey
                    FROM [2AM].dbo.LegalEntityAddress lea
                    JOIN [2AM].dbo.LegalEntity le ON le.LegalEntityKey = lea.LegalEntityKey
                    WHERE lea.LegalEntityAddressKey = @ClientAddressKey";
        }
    }
}
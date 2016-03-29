using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.DomainQuery.Managers.Address.Statements
{
    public class AddressIsAClientAddressStatement : ISqlStatement<int>
    {
        public int ClientKey { get; protected set; }

        public int AddressKey { get; protected set; }

        public int GeneralStatusKey { get; protected set; }

        public AddressIsAClientAddressStatement(int addressKey, int clientKey)
        {
            this.ClientKey = clientKey;
            this.AddressKey = addressKey;

            this.GeneralStatusKey = (int)GeneralStatus.Active;
        }

        public string GetStatement()
        {
            var query = @"SELECT
                            COUNT([LegalEntityAddressKey]) AS Total
                        FROM
                            [2AM].[dbo].[LegalEntityAddress]
                        WHERE
                            AddressKey = @AddressKey
                        AND
                            LegalEntityKey = @ClientKey
                        AND
                            GeneralStatusKey = @GeneralStatusKey";

            return query;
        }
    }
}
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements
{
    public class AddressTypeIsAResidentialAddressTypeStatement : ISqlStatement<int>
    {
        public int ClientAddressKey { get; protected set; }
        public int ResidentialAddressTypeKey { get; protected set; }

        public AddressTypeIsAResidentialAddressTypeStatement(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
            this.ResidentialAddressTypeKey = (int)AddressType.Residential;
        }

        public string GetStatement()
        {
            var query = @"SELECT    
                            count(*) AS Total 
                        FROM 
                            [2AM].[dbo].[LegalEntityAddress] 
                        WHERE 
                            LegalEntityAddressKey = @ClientAddressKey 
                        AND 
                            AddressTypeKey = @ResidentialAddressTypeKey";
            return query;
        }
    }
}
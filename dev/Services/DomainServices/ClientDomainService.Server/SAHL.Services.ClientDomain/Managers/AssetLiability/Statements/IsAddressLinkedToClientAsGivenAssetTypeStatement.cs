using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.AssetLiability.Statements
{
    public class IsAddressLinkedToClientAsGivenAssetTypeStatement : ISqlStatement<int>
    {
        public int AddressKey { get; protected set; }
        public int ClientKey { get; protected set; }
        public AssetLiabilityType AssetLiabilityType { get; protected set; }
        public int AssetLiabilityTypeKey { get { return (int)AssetLiabilityType; } }

        public IsAddressLinkedToClientAsGivenAssetTypeStatement(int addressKey, int clientKey, AssetLiabilityType assetLiabilityType)
        {
            this.AddressKey = addressKey;
            this.ClientKey = clientKey;
            this.AssetLiabilityType = assetLiabilityType;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            Count(*) as total
                        FROM 
                            [2AM].[dbo].[LegalEntityAssetLiability] leal
                        JOIN
                            [2AM].[dbo].[AssetLiability] al
                        ON
                            leal.AssetLiabilityKey = al.AssetLiabilityKey
                        WHERE
                            leal.LegalEntityKey = @ClientKey
                        AND
                            al.AddressKey = @AddressKey
                        AND 
                            al.AssetLiabilityTypeKey = @AssetLiabilityTypeKey";
            return query;
        }
    }
}
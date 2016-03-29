using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Detail.Address;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail.Address
{
    public class AddressChildDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<AddressChildTileConfiguration>
    {
        public AddressChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT 
                                   LegalEntityAddressKey as BusinessKey, 0 as BusinessKeyType
                                   FROM [2AM].[dbo].[LegalEntityAddress] lea
                                   WHERE
                                   lea.GeneralStatusKey = 1
                                   AND lea.LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}
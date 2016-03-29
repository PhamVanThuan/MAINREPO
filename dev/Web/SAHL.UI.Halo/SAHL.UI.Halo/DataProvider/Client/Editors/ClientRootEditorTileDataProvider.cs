using System;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client
{
    public class ClientRootEditorTileDataProvider : HaloTileBaseEditorDataProvider<ClientRootModel>
    {
        public ClientRootEditorTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"UPDATE [2AM].[dbo].[LegalEntity]
                SET RegistrationNumber = @RegistrationNumber
                WHERE LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}

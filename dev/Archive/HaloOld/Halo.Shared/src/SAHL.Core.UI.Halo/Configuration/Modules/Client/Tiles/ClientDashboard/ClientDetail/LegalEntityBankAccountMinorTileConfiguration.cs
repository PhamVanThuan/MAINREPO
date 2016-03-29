using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.BankAccount.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityBankAccountMinorTileConfiguration : MinorTileConfiguration<LegalEntityBankAccountMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityBankAccountMinorTileConfiguration()
            : base("LegalEntityBankAccountMinorTileAccess", 8)
        {
        }
    }
}
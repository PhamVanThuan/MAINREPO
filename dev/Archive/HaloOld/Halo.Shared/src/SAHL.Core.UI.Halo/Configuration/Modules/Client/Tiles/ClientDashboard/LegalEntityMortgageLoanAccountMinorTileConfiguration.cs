using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard
{
    public class LegalEntityMortgageLoanAccountMinorTileConfiguration : MinorTileConfiguration<MortgageLoanAccountMinorTileModel>, 
        IParentedTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public LegalEntityMortgageLoanAccountMinorTileConfiguration()
            : base("LegalEntityMortgageLoanAccountMinorTileAccess", 0)
        {
        }
    }
}
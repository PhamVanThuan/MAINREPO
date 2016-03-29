using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.PersonalLoanAccount.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard
{
    public class LegalEntityPersonalLoanAccountMinorTileConfiguration : MinorTileConfiguration<PersonalLoanAccountMinorTileModel>, IParentedTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public LegalEntityPersonalLoanAccountMinorTileConfiguration()
            : base("LegalEntityPersonalLoanAccountMinorTileAccess", 0)
        {
        }
    }
}
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Team
{
    public class TeamModuleConfiguration : HaloModuleConfiguration,
                                           IHaloApplicationModuleConfiguration<MyHaloHaloApplicationConfiguration>
    {
        public TeamModuleConfiguration()
            : base("Team", 2)
        {
        }
    }
}
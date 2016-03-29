using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Team.Sales
{
    [HaloRole("CCC", "Sales")]
    public class SalesLeagueTileConfiguration : HaloTileConfiguration,
                                                IHaloModuleRootTileConfiguration<TeamModuleConfiguration>
    {
        public SalesLeagueTileConfiguration()
            : base("Sales League", "app/start/portalpages/home/team/salesleague/salesleague.tpl.html", 9, 1, 5)
        {
        }
    }
}
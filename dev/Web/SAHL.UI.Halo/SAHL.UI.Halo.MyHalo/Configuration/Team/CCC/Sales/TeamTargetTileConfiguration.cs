using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Team.Sales
{
    [HaloRole("CCC", "Sales")]
    public class TeamTargetTileConfiguration : HaloTileConfiguration,
                                               IHaloModuleRootTileConfiguration<TeamModuleConfiguration>
    {
        public TeamTargetTileConfiguration()
            : base("Team Target", "app/start/portalpages/home/team/teamtarget/teamtarget.tpl.html", 3, 2, 4)
        {
        }
    }
}
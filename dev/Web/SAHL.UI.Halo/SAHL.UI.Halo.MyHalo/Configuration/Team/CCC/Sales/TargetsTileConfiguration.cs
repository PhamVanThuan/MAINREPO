using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Team.Sales
{
    [HaloRole("CCC", "Sales")]
    public class TargetsTileConfiguration : HaloTileConfiguration,
                                            IHaloModuleRootTileConfiguration<TeamModuleConfiguration>
    {
        public TargetsTileConfiguration()
            : base("Targets", "app/start/portalpages/home/team/targets/targets.tpl.html", 9, 3, 2)
        {
        }
    }
}
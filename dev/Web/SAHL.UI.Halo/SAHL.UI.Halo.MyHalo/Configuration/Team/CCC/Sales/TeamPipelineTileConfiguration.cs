using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Team.Sales
{
    [HaloRole("CCC", "Sales")]
    public class TeamPipelineTileConfiguration : HaloTileConfiguration,
                                                 IHaloModuleRootTileConfiguration<TeamModuleConfiguration>
    {
        public TeamPipelineTileConfiguration()
            : base("Team Pipeline", "app/start/portalpages/home/team/teampipeline/teampipeline.tpl.html", 3, 6, 1)
        {
        }
    }
}
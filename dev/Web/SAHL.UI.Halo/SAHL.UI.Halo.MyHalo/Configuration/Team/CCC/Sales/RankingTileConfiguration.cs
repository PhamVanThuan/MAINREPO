using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Team.Sales
{
    [HaloRole("CCC", "Sales")]
    public class RankingTileConfiguration : HaloTileConfiguration,
                                            IHaloModuleRootTileConfiguration<TeamModuleConfiguration>
    {
        public RankingTileConfiguration()
            : base("Ranking", "app/start/portalpages/home/team/ranking/ranking.tpl.html", 9, 4, 3)
        {
        }
    }
}
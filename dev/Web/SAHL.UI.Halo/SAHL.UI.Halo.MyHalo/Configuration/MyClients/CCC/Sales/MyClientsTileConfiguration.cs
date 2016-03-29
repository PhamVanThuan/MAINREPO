using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.MyClients.Sales
{
    [HaloRole("CCC", "Sales")]
    public class MyClientsTileConfiguration : HaloTileConfiguration,
                                              IHaloModuleRootTileConfiguration<MyClientsModuleConfiguration>
    {
        public MyClientsTileConfiguration()
            : base("My Clients", "app/start/portalpages/home/myclients/myclients/myclients.tpl.html", 9, 6, 1)
        {
        }
    }
}
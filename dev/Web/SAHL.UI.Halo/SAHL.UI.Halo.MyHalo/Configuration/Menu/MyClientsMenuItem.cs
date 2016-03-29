using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Menu.CCC.Sales
{
    [HaloRole("CCC", "Sales")]
    public class MyClientsMenuItem : HaloMenuItem, IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>
    {
        public MyClientsMenuItem()
            : base("My Clients", "My Clients", 5)
        {
        }
    }
}
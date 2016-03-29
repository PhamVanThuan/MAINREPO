using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.MyClients
{
    public class MyClientsModuleConfiguration : HaloModuleConfiguration,
                                                IHaloApplicationModuleConfiguration<MyHaloHaloApplicationConfiguration>
    {
        public MyClientsModuleConfiguration()
            : base("My Clients", 5)
        {
        }
    }
}
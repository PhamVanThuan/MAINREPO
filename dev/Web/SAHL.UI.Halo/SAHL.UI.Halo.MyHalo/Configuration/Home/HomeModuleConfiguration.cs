using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Home
{
    public class HomeModuleConfiguration : HaloModuleConfiguration,
                                           IHaloModuleApplicationConfiguration<MyHaloHaloApplicationConfiguration>
    {
        public HomeModuleConfiguration() : base("Home", 1)
        {
        }
    }
}

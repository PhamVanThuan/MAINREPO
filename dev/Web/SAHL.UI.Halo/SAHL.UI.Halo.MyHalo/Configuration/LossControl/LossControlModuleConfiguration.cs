using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.LossControl
{
    public class LossControlModuleConfiguration : HaloModuleConfiguration,
                                                  IHaloModuleApplicationConfiguration<MyHaloHaloApplicationConfiguration>
    {
        public LossControlModuleConfiguration() : base("Loss Control", 1)
        {
        }
    }
}

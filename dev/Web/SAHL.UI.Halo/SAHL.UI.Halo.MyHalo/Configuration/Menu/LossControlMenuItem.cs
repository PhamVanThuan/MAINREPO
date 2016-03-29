using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Menu
{
    [HaloRole("Operations - Manager", "Invoice Processor")]
    public class LossControlMenuItem : HaloMenuItem,
                                       IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>
    {
        public LossControlMenuItem()
            : base("Central Invoice List", "Loss Control", 2)
        {
        }
    }
}

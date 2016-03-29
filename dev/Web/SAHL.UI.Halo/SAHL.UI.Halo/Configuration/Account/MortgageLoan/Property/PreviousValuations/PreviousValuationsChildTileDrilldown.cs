using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PropertyDetails;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations
{
    public class PreviousValuationsChildTileDrilldown: HaloTileActionDrilldownBase<PreviousValuationsChildTileConfiguration, PreviousValuationsRootTileConfiguration>,
                                                  IHaloTileActionDrilldown<PreviousValuationsChildTileConfiguration>
    {
        public PreviousValuationsChildTileDrilldown()
            : base("PreviousValuations")
        {
        }
    }
}

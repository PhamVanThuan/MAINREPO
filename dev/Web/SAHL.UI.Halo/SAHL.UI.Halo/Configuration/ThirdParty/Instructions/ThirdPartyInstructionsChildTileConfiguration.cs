using SAHL.UI.Halo.Models.ThirdParty.Instructions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Instructions
{
    public class ThirdPartyInstructionsChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyRootTileConfiguration>,
                                                            IHaloTileModel<InstructionChildModel>
    {
        public ThirdPartyInstructionsChildTileConfiguration()
            : base("Instructions", "ThirdPartyInstructions", 3, startRow: 1, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}
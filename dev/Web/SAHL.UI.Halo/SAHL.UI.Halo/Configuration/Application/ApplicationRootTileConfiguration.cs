using System;

using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Application.Mortgage;
using SAHL.UI.Halo.Shared.Configuration.TileConfiguration;

namespace SAHL.UI.Halo.Configuration.Application
{
    public class ApplicationRootTileConfiguration : HaloBaseDynamicTileConfiguration,
                                                    IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                    IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public ApplicationRootTileConfiguration()
            : base("Application", "Application", 2)
        {
        }

        public override IHaloRootTileConfiguration GetRootTileConfiguration(HaloDynamicTileDataModel dynamicTileDataModel)
        {
            switch (dynamicTileDataModel.SubType)
            {
                case "New Purchase Loan":
                    return new NewPurchaseApplicationRootTileConfiguration();
                case "Refinance Loan":
                    return new RefinanceApplicationRootTileConfiguration();
                case "Switch Loan":
                    return new SwitchApplicationRootTileConfiguration();
                default:
                    throw new Exception("Unsupported Application Type requested");
            }
        }
    }
}

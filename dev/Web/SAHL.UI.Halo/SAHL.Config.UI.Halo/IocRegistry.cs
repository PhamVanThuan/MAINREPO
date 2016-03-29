using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.Caching;
using SAHL.UI.Halo.Shared.Repository;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.UI.Halo
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));

                    scanner.Convention<HaloApplicationConfigurationConvention>();
                    scanner.Convention<HaloModuleConfigurationConvention>();

                    scanner.Convention<HaloModuleApplicationConfigurationConvention>();
                    scanner.Convention<HaloTileLinkedConfigurationConvention>();
                    scanner.Convention<HaloTileModuleConfigurationConvention>();
                    scanner.Convention<HaloChildTileConfigurationConvention>();
                    scanner.Convention<HaloTileEditorConfigurationConvention>();

                    scanner.Convention<HaloTileContentDataProviderConvention>();
                    scanner.Convention<HaloTileContentMultipleDataProviderConvention>();
                    scanner.Convention<HaloTileChildDataProviderConvention>();
                    scanner.Convention<HaloTileEditorDataProviderConvention>();
                    scanner.Convention<HaloTileDynamicDataProviderConvention>();

                    scanner.Convention<HaloTileActionDrillDownConvention>();
                    scanner.Convention<HaloTileActionEditConvention>();
                    scanner.Convention<HaloWorkflowTileActionProviderConvention>();
                    scanner.Convention<HaloWorkflowActionConditionalProviderConvention>();

                    scanner.Convention<HaloMenuItemConvention>();
                    scanner.Convention<HaloApplicationMenuItemConvention>();

                    scanner.Convention<HaloTileHeaderConvention>();
                    scanner.Convention<HaloTileHeaderIconProviderConvention>();
                    scanner.Convention<HaloTileHeaderTextProviderConvention>();

                    scanner.Convention<HaloWizardTileConfigurationConvention>();
                    scanner.Convention<HaloWizardWorkflowConfigurationConvention>();
                    scanner.Convention<HaloWizardTilePageConfigurationConvention>();

                    scanner.WithDefaultConventions();
                });

            this.For<IHaloConfigurationCacheManager>().Singleton()
                                                      .Use<HaloConfigurationCacheManager>();

            this.For<IHaloWorkflowTileActionProvider>().Use<HaloWorkflowTileActionProvider>();

            this.For<ITileConfigurationRepository>().Use<TileConfigurationRepository>();
            this.For<ITileDataRepository>().Use<TileDataRepository>();
            this.For<ITileWizardConfigurationRepository>().Use<TileWizardConfigurationRepository>();
        }
    }
}

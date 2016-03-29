using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;

namespace SAHL.UI.Halo.Shared
{
    public enum HaloTileConfigurationType
    {
        Tile,
        Root,
        Child
    };

    public interface ITileConfigurationRepository
    {
        IHaloApplicationConfiguration FindApplicationConfiguration(string applicationName);
        IHaloModuleConfiguration FindModuleConfiguration<T>(T applicationConfiguration, string moduleName) where T : IHaloApplicationConfiguration;
        IHaloRootTileLinkedConfiguration FindModuleLinkedTileConfigurationByName<T>(T moduleTileConfiguration, string linkedTileName) where T : IHaloModuleConfiguration;
        IHaloTileConfiguration FindRootTileConfiguration(IHaloModuleConfiguration moduleConfiguration, string tileName);
        IHaloChildTileConfiguration FindChildTileConfiguration(IHaloRootTileConfiguration rootTileConfiguration, string tileName);
        IHaloTileConfiguration FindTileEditorConfiguration(IHaloTileConfiguration tileConfiguration, string tileName);
        IHaloRootTileConfiguration FindRootTileConfigurationForLinkedConfiguration<T>(T linkedRootTileConfiguration) where T : IHaloRootTileLinkedConfiguration;

        IHaloTileActionDrilldown FindTileActionDrillDown<T>(T tileConfiguration) where T : IHaloTileConfiguration;

        IEnumerable<IHaloApplicationConfiguration> FindAllApplicationConfigurations();
        IEnumerable<IHaloModuleConfiguration> FindApplicationModuleConfigurations<T>(T applicationConfiguration) where T : IHaloApplicationConfiguration;
        IEnumerable<IHaloRootTileConfiguration> FindModuleRootTileConfigurations<T>(T moduleConfiguration) where T : IHaloModuleConfiguration;
        IEnumerable<IHaloRootTileLinkedConfiguration> FindAllModuleLinkedRootTileConfigurations<T>(T moduleConfiguration) where T : IHaloModuleConfiguration;
        IEnumerable<IHaloChildTileConfiguration> FindChildTileConfigurations<T>(T tileConfiguration) where T : IHaloRootTileConfiguration;
        IEnumerable<IHaloTileEditorConfiguration> FindTileEditorConfigurations<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IEnumerable<IHaloMenuItem> FindAllMenuItemsForApplication<T>(T applicationConfiguration) where T : IHaloApplicationConfiguration;

        IEnumerable<IHaloTileAction> FindAllTileActions<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IEnumerable<IHaloTileDynamicActionProvider> FindAllDynamicTileActionProviders<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IEnumerable<IHaloWorkflowTileActionProvider> FindAllWorkflowTileActionProviders<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IEnumerable<IHaloWorkflowActionConditionalProvider> FindAllWorkflowActionConditionalProviders(string processName, string workflowName, string activityName);

        IHaloTileHeader FindTileHeader<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IEnumerable<IHaloTileHeaderIconProvider> FindAllTileHeaderIconProviders<TTileHeader>(TTileHeader tileConfiguration)
            where TTileHeader : IHaloTileHeader;
        IEnumerable<IHaloTileHeaderTextProvider> FindAllTileHeaderTextProviders<TTileHeader>(TTileHeader tileHeader)
            where TTileHeader : IHaloTileHeader;
    }
}

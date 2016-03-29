using System;
using System.Linq;
using System.Collections.Generic;

using SAHL.Core;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;

namespace SAHL.UI.Halo.Shared
{
    public class TileConfigurationRepository : ITileConfigurationRepository
    {
        private readonly IIocContainer iocContainer;

        public TileConfigurationRepository(IIocContainer iocContainer)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            this.iocContainer = iocContainer;
        }

        public IHaloApplicationConfiguration FindApplicationConfiguration(string applicationName)
        {
            return iocContainer.GetAllInstances<IHaloApplicationConfiguration>()
                               .FirstOrDefault(configuration => configuration.Name == applicationName);
        }

        public IHaloModuleConfiguration FindModuleConfiguration<T>(T applicationConfiguration, string moduleName) where T : IHaloApplicationConfiguration
        {
            var allModuleConfigurations = this.FindApplicationModuleConfigurations(applicationConfiguration);
            if (allModuleConfigurations == null || !allModuleConfigurations.Any()) { return null; }

            var moduleConfiguration = allModuleConfigurations.FirstOrDefault(configuration => configuration.Name == moduleName);
            return moduleConfiguration;
        }

        public IHaloRootTileLinkedConfiguration FindModuleLinkedTileConfigurationByName<T>(T moduleTileConfiguration, string linkedTileName) where T : IHaloModuleConfiguration
        {
            var moduleType              = moduleTileConfiguration.GetType();
            var moduleConfigurationType = typeof(IHaloModuleTileConfiguration<>);
            var moduleGenericType       = moduleConfigurationType.MakeGenericType(moduleType);

            var moduleLinkedConfigurations = this.iocContainer.GetAllInstances(moduleGenericType);
            if (moduleTileConfiguration == null || !moduleLinkedConfigurations.Any()) { return null; }

            var linkedConfiguration = moduleLinkedConfigurations.Where(configuration => configuration is IHaloRootTileLinkedConfiguration)
                                                                .Cast<IHaloRootTileLinkedConfiguration>()
                                                                .FirstOrDefault(configuration => configuration.Name == linkedTileName);
            return linkedConfiguration;
        }

        public IHaloTileConfiguration FindRootTileConfiguration(IHaloModuleConfiguration moduleConfiguration, string tileName)
        {
            if (moduleConfiguration == null) { throw new ArgumentNullException("moduleConfiguration"); }

            var moduleTileConfigurations = this.FindModuleRootTileConfigurations(moduleConfiguration).ToList();
            if (moduleTileConfigurations == null || !moduleTileConfigurations.Any()) { return null; }

            return moduleTileConfigurations.FirstOrDefault(configuration => configuration.Name == tileName);
        }

        public IHaloChildTileConfiguration FindChildTileConfiguration(IHaloRootTileConfiguration rootTileConfiguration, string tileName)
        {
            if (rootTileConfiguration == null) { throw new ArgumentNullException("rootTileConfiguration"); }

            var childTileConfigurations = this.FindChildTileConfigurations(rootTileConfiguration);
            if (childTileConfigurations == null || !childTileConfigurations.Any()) { return null; }

            return childTileConfigurations.FirstOrDefault(configuration => configuration.Name == tileName);
        }

        public IHaloTileConfiguration FindTileEditorConfiguration(IHaloTileConfiguration tileConfiguration, string tileName)
        {
            if (tileConfiguration == null) { throw new ArgumentNullException("tileConfiguration"); }

            var editorConfigurations = this.FindTileEditorConfigurations(tileConfiguration);
            if (editorConfigurations == null || !editorConfigurations.Any()) { return null; }

            return editorConfigurations.FirstOrDefault(configuration => configuration.Name == tileName);
        }

        public IHaloTileActionDrilldown FindTileActionDrillDown<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            if (tileConfiguration == null) { throw new ArgumentNullException("tileConfiguration"); }

            var tileActionDrilldown = typeof(IHaloTileActionDrilldown<>);
            var genericType = tileActionDrilldown.MakeGenericType(tileConfiguration.GetType());

            var instance = iocContainer.GetInstance(genericType);
            return instance as IHaloTileActionDrilldown;
        }

        public IEnumerable<IHaloApplicationConfiguration> FindAllApplicationConfigurations()
        {
            var applicationConfigurations = iocContainer.GetAllInstances<IHaloApplicationConfiguration>().ToList();
            if (!applicationConfigurations.Any()) { return applicationConfigurations; }

            var orderedConfigurations = applicationConfigurations.OrderBy(configuration => configuration.Sequence);
            return orderedConfigurations;
        }

        public IEnumerable<IHaloModuleConfiguration> FindApplicationModuleConfigurations<T>(T applicationConfiguration) where T : IHaloApplicationConfiguration
        {
            var applicationType = applicationConfiguration.GetType();
            var appModuleConfiguration = typeof(IHaloModuleApplicationConfiguration<>);
            var genericType = appModuleConfiguration.MakeGenericType(applicationType);

            var moduleConfigurations = iocContainer.GetAllInstances(genericType).ToList();
            if (!moduleConfigurations.Any()) { return null; }

            IEnumerable<IHaloModuleConfiguration> orderedConfigurations = moduleConfigurations.Cast<IHaloModuleConfiguration>()
                                                                                              .OrderBy(configuration => configuration.Sequence);
            return orderedConfigurations;
        }

        public IEnumerable<IHaloRootTileConfiguration> FindModuleRootTileConfigurations<T>(T moduleConfiguration) where T : IHaloModuleConfiguration
        {
            var moduleType = moduleConfiguration.GetType();
            var tileModuleConfiguration = typeof(IHaloModuleTileConfiguration<>);
            var genericType = tileModuleConfiguration.MakeGenericType(moduleType);

            var tileConfigurations = iocContainer.GetAllInstances(genericType).ToList();
            if (!tileConfigurations.Any()) { return null; }

            IEnumerable<IHaloRootTileConfiguration> orderedConfigurations = tileConfigurations.Where(tile => tile is IHaloRootTileConfiguration)
                                                                                              .Cast<IHaloRootTileConfiguration>()
                                                                                              .OrderBy(configuration => configuration.Sequence);
            return orderedConfigurations;
        }

        public IEnumerable<IHaloRootTileLinkedConfiguration> FindAllModuleLinkedRootTileConfigurations<T>(T moduleConfiguration) where T : IHaloModuleConfiguration
        {
            var moduleType = moduleConfiguration.GetType();
            var tileModuleConfiguration = typeof(IHaloModuleTileConfiguration<>);
            var genericType = tileModuleConfiguration.MakeGenericType(moduleType);

            var tileConfigurations = iocContainer.GetAllInstances(genericType).ToList();
            if (!tileConfigurations.Any()) { return null; }

            IEnumerable<IHaloRootTileLinkedConfiguration> orderedConfigurations = tileConfigurations.Where(tile => tile is IHaloRootTileLinkedConfiguration)
                                                                                              .Cast<IHaloRootTileLinkedConfiguration>();
            return orderedConfigurations;
        }

        public IHaloRootTileConfiguration FindRootTileConfigurationForLinkedConfiguration<T>(T linkedRootTileConfiguration) where T : IHaloRootTileLinkedConfiguration
        {
            var linkedInterface = linkedRootTileConfiguration.GetType().GetInterfaces().FirstOrDefault(type => type.Name.StartsWith("IHaloRootTileLinkedConfiguration`1"));
            if (linkedInterface == null) { return null; }

            var rootTileConfigurationType = linkedInterface.GenericTypeArguments[0];
            return Activator.CreateInstance(rootTileConfigurationType) as IHaloRootTileConfiguration;
        }

        public IEnumerable<IHaloChildTileConfiguration> FindChildTileConfigurations<T>(T tileConfiguration) where T : IHaloRootTileConfiguration
        {
            var tileType = tileConfiguration.GetType();
            var tileRootConfiguration = typeof(IHaloChildTileConfiguration<>);
            var genericType = tileRootConfiguration.MakeGenericType(tileType);

            var tileConfigurations = iocContainer.GetAllInstances(genericType).ToList();
            if (!tileConfigurations.Any()) { return null; }

            IEnumerable<IHaloChildTileConfiguration> orderedConfigurations = tileConfigurations.Cast<IHaloChildTileConfiguration>()
                                                                                               .OrderBy(configuration => configuration.Sequence);
            return orderedConfigurations;
        }

        public IEnumerable<IHaloTileEditorConfiguration> FindTileEditorConfigurations<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetType();
            var editorConfiguration = typeof(IHaloTileEditorConfiguration<>);
            var genericType = editorConfiguration.MakeGenericType(tileType);

            var tileConfigurations = iocContainer.GetAllInstances(genericType).ToList();
            if (!tileConfigurations.Any()) { return null; }

            IEnumerable<IHaloTileEditorConfiguration> orderedConfigurations = tileConfigurations.Cast<IHaloTileEditorConfiguration>()
                                                                                                .OrderBy(configuration => configuration.Sequence);
            return orderedConfigurations;
        }

        public IEnumerable<IHaloMenuItem> FindAllMenuItemsForApplication<T>(T applicationConfiguration) where T : IHaloApplicationConfiguration
        {
            var moduleType = applicationConfiguration.GetType();
            var haloMenuItem = typeof(IHaloApplicationMenuItem<>);
            var genericType = haloMenuItem.MakeGenericType(moduleType);

            var moduleMenuItems = iocContainer.GetAllInstances(genericType).ToList();
            if (!moduleMenuItems.Any()) { return null; }

            IEnumerable<IHaloMenuItem> orderedMenuItems = moduleMenuItems.Cast<IHaloMenuItem>()
                                                                         .OrderBy(menuItem => menuItem.Sequence);
            return orderedMenuItems;
        }

        public IEnumerable<IHaloTileAction> FindAllTileActions<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var allActions = new List<IHaloTileAction>();

            var allDrilldownActions = this.FindAllTileActionDrilldowns(tileConfiguration);
            if (allDrilldownActions != null && allDrilldownActions.Any())
            {
                allActions.AddRange(allDrilldownActions);
            }

            var allEditActions = this.FindAllTileActionEdits(tileConfiguration);
            if (allEditActions != null && allEditActions.Any())
            {
                allActions.AddRange(allEditActions);
            }

            return allActions;
        }

        public IEnumerable<IHaloTileDynamicActionProvider> FindAllDynamicTileActionProviders<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var providers = new List<IHaloTileDynamicActionProvider>();

            var dynamicProviderTypes = tileConfiguration.GetType().GetInterfaces().Where(type => type.Name.StartsWith("IHaloTileDynamicActionProvider`1"));
            if (!dynamicProviderTypes.Any()) { return providers; }

            foreach (var providerType in dynamicProviderTypes)
            {
                var provider = this.iocContainer.GetConcreteInstance(providerType.GenericTypeArguments[0]);
                if (provider == null) { continue; }

                providers.Add(provider as IHaloTileDynamicActionProvider);
            }

            return providers;
        }

        public IEnumerable<IHaloWorkflowTileActionProvider> FindAllWorkflowTileActionProviders<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var providers = new List<IHaloWorkflowTileActionProvider>();

            var actionProviderTypes = tileConfiguration.GetType().GetInterfaces().Where(type => type.Name.StartsWith("IHaloWorkflowTileActionProvider`1"));
            if (!actionProviderTypes.Any()) { return providers; }

            foreach (var providerType in actionProviderTypes)
            {
                var provider = this.iocContainer.GetConcreteInstance(providerType.GenericTypeArguments[0]);
                if (provider == null) { continue; }

                providers.Add(provider as IHaloWorkflowTileActionProvider);
            }

            return providers;
        }

        public IEnumerable<IHaloWorkflowActionConditionalProvider> FindAllWorkflowActionConditionalProviders(string processName, string workflowName, string activityName)
        {
            var providers = new List<IHaloWorkflowActionConditionalProvider>();

            var conditionalProviders = this.iocContainer.GetAllInstances<IHaloWorkflowActionConditionalProvider>();
            if (conditionalProviders == null || !conditionalProviders.Any()) { return providers; }

            providers.AddRange(conditionalProviders.Where(provider => provider.ProcessName == processName &&
                                                                       provider.WorkflowName == workflowName &&
                                                                       provider.ActivityName == activityName));
            return providers;
        }

        public IHaloTileHeader FindTileHeader<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetType();
            var tileHeaderType = typeof(IHaloTileHeader<>);
            var genericType = tileHeaderType.MakeGenericType(tileType);

            var instance = this.iocContainer.GetInstance(genericType);
            return instance as IHaloTileHeader;
        }

        private IEnumerable<IHaloTileActionDrilldown> FindAllTileActionDrilldowns<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetType();
            var drilldownType = typeof(IHaloTileActionDrilldown<>);
            var genericType = drilldownType.MakeGenericType(tileType);

            var allInstances = this.iocContainer.GetAllInstances(genericType);
            if (allInstances == null || !allInstances.Any()) { return null; }
            return allInstances.Cast<IHaloTileActionDrilldown>();
        }

        private IEnumerable<IHaloTileActionEdit> FindAllTileActionEdits<T>(T tileConfiguration) where T : IHaloTileConfiguration
        {
            var tileType = tileConfiguration.GetType();
            var editType = typeof(IHaloTileActionEdit<>);
            var genericType = editType.MakeGenericType(tileType);

            var allInstances = this.iocContainer.GetAllInstances(genericType);
            if (allInstances == null || !allInstances.Any()) { return null; }
            return allInstances.Cast<IHaloTileActionEdit>();
        }

        public IEnumerable<IHaloTileHeaderIconProvider> FindAllTileHeaderIconProviders<TTileHeader>(TTileHeader tileHeader)
            where TTileHeader : IHaloTileHeader
        {
            var tileHeaderType = tileHeader.GetType();
            var iconProviderType = typeof(IHaloTileHeaderIconProvider<>);
            var genericType = iconProviderType.MakeGenericType(tileHeaderType);

            var allInstances = this.iocContainer.GetAllInstances(genericType);
            if (allInstances == null || !allInstances.Any()) { return null; }

            return allInstances.Cast<IHaloTileHeaderIconProvider>();
        }

        public IEnumerable<IHaloTileHeaderTextProvider> FindAllTileHeaderTextProviders<TTileHeader>(TTileHeader tileHeader)
            where TTileHeader : IHaloTileHeader
        {
            var tileHeaderType = tileHeader.GetType();
            var iconProviderType = typeof(IHaloTileHeaderTextProvider<>);
            var genericType = iconProviderType.MakeGenericType(tileHeaderType);

            var allInstances = this.iocContainer.GetAllInstances(genericType);
            if (allInstances == null || !allInstances.Any()) { return null; }

            return allInstances.Cast<IHaloTileHeaderTextProvider>();
        }
    }
}

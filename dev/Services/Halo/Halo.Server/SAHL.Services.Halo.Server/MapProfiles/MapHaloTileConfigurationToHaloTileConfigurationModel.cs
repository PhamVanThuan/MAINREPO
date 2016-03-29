using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.UI.Halo.Shared;
using SAHL.Core.BusinessModel;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models.Configuration;

namespace SAHL.Services.Halo.Server
{
    public class MapHaloTileConfigurationToHaloTileConfigurationModel : Profile, IAutoMapperProfile
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;
        private readonly ITileDataRepository tileDataRepository;

        public MapHaloTileConfigurationToHaloTileConfigurationModel(ITileConfigurationRepository tileConfigurationRepository, ITileDataRepository tileDataRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            if (tileDataRepository == null) { throw new ArgumentNullException("tileDataRepository"); }

            this.tileConfigurationRepository = tileConfigurationRepository;
            this.tileDataRepository          = tileDataRepository;
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IHaloTileConfiguration, HaloTileConfigurationModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.IsTileBased, expression => expression.MapFrom(configuration => configuration.IsTileBased))
                .ForMember(model => model.NonTilePageState, expression => expression.MapFrom(configuration => configuration.NonTilePageState))
                .ForMember(model => model.StartRow, expression => expression.Ignore())
                .ForMember(model => model.StartColumn, expression => expression.Ignore())
                .ForMember(model => model.NoOfRows, expression => expression.Ignore())
                .ForMember(model => model.NoOfColumns, expression => expression.Ignore())
                .ForMember(model => model.BusinessContext, expression => expression.Ignore())
                .ForMember(model => model.Role, expression => expression.Ignore())
                .ForMember(model => model.TileHeader, expression => expression.Ignore())
                .ForMember(model => model.HasDrilldown, expression => expression.Ignore())
                .ForMember(model => model.DataModelType, expression => expression.Ignore())
                .ForMember(model => model.TileData, expression => expression.Ignore())
                .ForMember(model => model.TileDataItems, expression => expression.Ignore())
                .ForMember(model => model.TileSubKeys, expression => expression.Ignore())
                .ForMember(model => model.TileActions, expression => expression.Ignore());

            Mapper.CreateMap<IHaloSubTileConfiguration, HaloTileConfigurationModel>()
                .Include<IHaloRootTileConfiguration, HaloTileConfigurationModel>()
                .Include<IHaloChildTileConfiguration, HaloTileConfigurationModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.TileType, expression => expression.MapFrom(configuration => configuration.TileType))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.IsTileBased, expression => expression.MapFrom(configuration => configuration.IsTileBased))
                .ForMember(model => model.NonTilePageState, expression => expression.MapFrom(configuration => configuration.NonTilePageState))
                .ForMember(model => model.StartRow, expression => expression.MapFrom(configuration => configuration.StartRow))
                .ForMember(model => model.StartColumn, expression => expression.MapFrom(configuration => configuration.StartColumn))
                .ForMember(model => model.NoOfRows, expression => expression.MapFrom(configuration => configuration.NoOfRows))
                .ForMember(model => model.NoOfColumns, expression => expression.MapFrom(configuration => configuration.NoOfColumns))
                .ForMember(model => model.BusinessContext, expression => expression.Ignore())
                .ForMember(model => model.Role, expression => expression.Ignore())
                .ForMember(model => model.TileHeader, expression => expression.Ignore())
                .ForMember(model => model.HasDrilldown, expression => expression.Ignore())
                .ForMember(model => model.DataModelType, expression => expression.Ignore())
                .ForMember(model => model.TileData, expression => expression.Ignore())
                .ForMember(model => model.TileDataItems, expression => expression.Ignore())
                .ForMember(model => model.TileSubKeys, expression => expression.Ignore())
                .ForMember(model => model.TileActions, expression => expression.Ignore())
                .AfterMap((configuration, model) =>
                    {
                        model.TileData         = this.LoadTileData(configuration, model.BusinessContext);
                        model.TileDataItems    = this.LoadTileDataItems(configuration, model.BusinessContext);
                        model.DataModelType    = this.RetrieveTileDataModelType(configuration, model.TileData);
                        model.NonTilePageState = this.RetrieveTilePageState(configuration);
                        model.TileHeader       = this.LoadTileHeader(configuration, model.TileData);
                        model.TileSubKeys      = this.LoadTileSubKeys(configuration as IHaloChildTileConfiguration, model.BusinessContext);
                    })
                .AfterMap((configuration, model) =>
                    {
                        var tileActionModels = new List<HaloTileActionModel>();
                        tileActionModels.AddRange(this.LoadTileActions(configuration));
                        tileActionModels.AddRange(this.LoadDynamicTileActions(configuration, model.BusinessContext));
                        tileActionModels.AddRange(this.LoadWorkflowTileActions(configuration, model.BusinessContext, model.Role));

                        model.TileActions = tileActionModels.OrderBy(actionModel => actionModel.Group)
                                                            .ThenBy(actionModel => actionModel.Sequence);
                    })
                .AfterMap((configuration, model) =>
                    {
                        if (model.TileActions == null) { return; }

                        foreach (var tileAction in model.TileActions)
                        {
                            if (tileAction.ActionType != "DrillDown") { continue; }
                            model.HasDrilldown = true;
                        }
                    });

        }

        private dynamic LoadTileData(IHaloSubTileConfiguration tileConfiguration, BusinessContext businessContext)
        {
            if (businessContext == null) { return null; }

            var dataProvider = this.tileDataRepository.FindTileContentDataProvider(tileConfiguration);
            return dataProvider == null 
                        ? null 
                        : dataProvider.Load(businessContext);
        }

        private dynamic LoadTileDataItems(IHaloSubTileConfiguration tileConfiguration, BusinessContext businessContext)
        {
            if (businessContext == null) { return null; }

            var multipleDataProvider = this.tileDataRepository.FindTileContentMultipleDataProvider(tileConfiguration);
            return multipleDataProvider == null 
                ? null 
                : multipleDataProvider.Load(businessContext);
        }

        private string RetrieveTileDataModelType(IHaloTileConfiguration tileConfiguration, IDataModel dataModel)
        {
            if (dataModel != null)
            {
                return dataModel.GetType().AssemblyQualifiedName;
            }

            var tileDataModel = this.tileDataRepository.FindTileDataModel(tileConfiguration);
            return tileDataModel == null 
                ? tileConfiguration.GetType().AssemblyQualifiedName
                : tileDataModel.GetType().AssemblyQualifiedName;
        }

        private string RetrieveTilePageState(IHaloSubTileConfiguration configuration)
        {
            if (configuration == null) { return null; }

            var tilePageState = this.tileDataRepository.FindTilePageState(configuration);
            return tilePageState == null ? null : tilePageState.GetType().FullName;
        }

        private HaloTileHeaderModel LoadTileHeader(IHaloTileConfiguration tileConfiguration, IDataModel dataModel)
        {
            var tileHeader = this.tileConfigurationRepository.FindTileHeader(tileConfiguration);
            if (tileHeader == null) { return null; }

            var tileHeaderModel = new HaloTileHeaderModel
                {
                    Data = dataModel,
                };

            var haloTileHeaderModel = Mapper.Map<IHaloTileHeader, HaloTileHeaderModel>(tileHeader, tileHeaderModel);
            return haloTileHeaderModel;
        }

        private IEnumerable<BusinessContext> LoadTileSubKeys(IHaloChildTileConfiguration tileConfiguration, BusinessContext businessContext)
        {
            if (tileConfiguration == null || businessContext == null) { return null; }

            var subDataProvider = this.tileDataRepository.FindTileChildDataProvider(tileConfiguration);
            if (subDataProvider == null) { return null; }

            var subKeys = subDataProvider.LoadSubKeys(businessContext);
            return subKeys;
        }

        private IEnumerable<HaloTileActionModel> LoadTileActions(IHaloTileConfiguration tileConfiguration)
        {
            var tileActionModels = new List<HaloTileActionModel>();
            if (tileConfiguration == null) { return tileActionModels; }

            var tileActions = this.tileConfigurationRepository.FindAllTileActions(tileConfiguration);
            if (tileActions == null) { return tileActionModels; }

            tileActionModels.AddRange(tileActions.Select(Mapper.Map<HaloTileActionModel>)
                                                 .OrderBy(model => model.Group)
                                                 .ThenBy(model => model.Sequence));
            return tileActionModels;
        }

        private IEnumerable<HaloTileActionModel> LoadDynamicTileActions(IHaloTileConfiguration tileConfiguration, BusinessContext businessContext)
        {
            var dynamicTileActions = new List<HaloTileActionModel>();
            if (tileConfiguration == null) { return dynamicTileActions; }

            var tileActionProviders = this.tileConfigurationRepository.FindAllDynamicTileActionProviders(tileConfiguration);
            if (!tileActionProviders.Any()) { return dynamicTileActions; }

            foreach (var provider in tileActionProviders)
            {
                var dynamicActions = provider.GetTileActions(businessContext);
                if (dynamicActions == null || !dynamicActions.Any()) { continue; }

                dynamicTileActions.AddRange(dynamicActions.Select(dynamicAction => Mapper.Map<HaloTileActionModel>(dynamicAction)));
            }

            if (!dynamicTileActions.Any()) { return dynamicTileActions; }

            var orderedTileActions = dynamicTileActions.OrderBy(model => model.Sequence);
            return orderedTileActions;
        }

        private IEnumerable<HaloTileActionModel> LoadWorkflowTileActions(IHaloTileConfiguration tileConfiguration, 
                                                                         BusinessContext businessContext, HaloRoleModel roleModel)
        {
            var tileActions = new List<HaloTileActionModel>();
            if (tileConfiguration == null || businessContext == null) { return tileActions; }

            var tileActionProviders = this.tileConfigurationRepository.FindAllWorkflowTileActionProviders(tileConfiguration);
            if (!tileActionProviders.Any()) { return tileActions; }

            foreach (var provider in tileActionProviders)
            {
                var workflowActions = provider.GetTileActions(businessContext, roleModel.RoleName, roleModel.Capabilities);
                if (workflowActions == null || !workflowActions.Any()) { continue; }

                foreach (var workflowAction in workflowActions)
                {
                    var conditionalResult = this.ProcessWorkflowConditionalProviders(workflowAction, businessContext, roleModel.Capabilities);
                    if (!conditionalResult.Item1) { continue; }

                    tileActions.AddRange(conditionalResult.Item2);
                }
            }

            if (!tileActions.Any()) { return tileActions; }

            var orderedTileActions = tileActions.OrderBy(model => model.Sequence);
            return orderedTileActions;
        }

        private Tuple<bool, List<HaloTileActionModel>> ProcessWorkflowConditionalProviders(IHaloWorkflowAction workflowAction, BusinessContext businessContext, string[] capabilities)
        {
            var haloTileActionModels = new List<HaloTileActionModel>
                {
                    Mapper.Map<HaloTileActionModel>(workflowAction),
                };
            var conditionalResult    = new Tuple<bool, List<HaloTileActionModel>>(true, haloTileActionModels);
            var conditionalProviders = this.tileConfigurationRepository.FindAllWorkflowActionConditionalProviders(workflowAction.ProcessName, 
                                                                                                                  workflowAction.WorkflowName, 
                                                                                                                  workflowAction.Name);
            if (!conditionalProviders.Any()) { return conditionalResult; }

            var workflowActions    = new List<HaloTileActionModel>();
            var workflowTileAction = Mapper.Map<HaloTileActionModel>(workflowAction);

            foreach (var conditionalProvider in conditionalProviders)
            {
                if (conditionalProvider.Execute(businessContext, capabilities))
                {
                    if (!workflowActions.Contains(workflowTileAction)) { workflowActions.Add(workflowTileAction); }
                    continue;
                }

                if (conditionalProvider.AlternativeWorkflowActionModel == null) { continue; }

                var alternativeAction = new HaloWorkflowAction(conditionalProvider.AlternativeWorkflowActionModel.ActivityName, "icon-spin", "Workflow", 1, 
                                                               conditionalProvider.AlternativeWorkflowActionModel.ProcessName,
                                                               conditionalProvider.AlternativeWorkflowActionModel.WorkflowName,
                                                               workflowAction.InstanceId);
                workflowActions.Add(Mapper.Map<HaloTileActionModel>(alternativeAction));
            }

            conditionalResult = new Tuple<bool, List<HaloTileActionModel>>(true, workflowActions);
            return conditionalResult;
        }
    }
}

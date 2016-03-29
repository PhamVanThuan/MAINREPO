using AutoMapper;
using SAHL.Core;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Halo.Server
{
    public class MapHaloModuleConfigurationToHaloModuleModel : Profile, IAutoMapperProfile
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public MapHaloModuleConfigurationToHaloModuleModel(ITileConfigurationRepository tileConfigurationRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            this.tileConfigurationRepository = tileConfigurationRepository;
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IHaloModuleConfiguration, HaloModuleModel>();

            Mapper.CreateMap<IHaloModuleConfiguration, HaloModuleTileModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.IsTileBased, expression => expression.MapFrom(configuration => configuration.IsTileBased))
                .ForMember(model => model.NonTilePageState, expression => expression.MapFrom(configuration => configuration.NonTilePageState))
                .ForMember(model => model.RootTileConfigurations, expression => expression.Ignore())
                .ForMember(model => model.ChildTileConfigurations, expression => expression.Ignore())
                .ForMember(model => model.AllRoots, expression => expression.Ignore())
                .ForMember(model => model.ModuleParameters, expression => expression.Ignore())
                .ForMember(model => model.RoleModel, expression => expression.Ignore())
                .AfterMap((configuration, model) =>
                    {
                        var tileConfigurationModels = new List<HaloTileConfigurationModel>();

                        tileConfigurationModels.AddRange(this.RetrieveRootTileConfigurations(configuration, model.AllRoots, model.ModuleParameters, model.RoleModel));

                        if (tileConfigurationModels.Any())
                        {
                            model.RootTileConfigurations = tileConfigurationModels.OrderBy(configurationModel => configurationModel.Sequence);
                        }
                    });
        }

        private IEnumerable<HaloTileConfigurationModel> RetrieveRootTileConfigurations(IHaloModuleConfiguration moduleConfiguration, bool includeAllRoots,
                                                                                       string moduleParameters, HaloRoleModel roleModel)
        {
            var allRootTileModels = new List<HaloTileConfigurationModel>();

            var linkedRootTileConfiguration = this.RetrieveLinkedRootTileConfiguration(moduleConfiguration, moduleParameters);
            if (linkedRootTileConfiguration != null) { allRootTileModels.Add(linkedRootTileConfiguration); }

            var moduleRootTileConfigurations = tileConfigurationRepository.FindModuleRootTileConfigurations(moduleConfiguration);
            if (moduleRootTileConfigurations == null || !moduleRootTileConfigurations.Any())
            {
                return allRootTileModels;
            }

            foreach (var rootTileConfiguration in moduleRootTileConfigurations)
            {
                if (!includeAllRoots && rootTileConfiguration.Sequence != 1)
                {
                    continue;
                }

                if (roleModel != null && !this.IsRoleSpecifiedInTileConfiguration(roleModel.RoleName, roleModel.Capabilities, rootTileConfiguration))
                {
                    continue;
                }

                var tileConfigurationModel = Mapper.Map<IHaloRootTileConfiguration, HaloTileConfigurationModel>(rootTileConfiguration);
                allRootTileModels.Add(tileConfigurationModel);
            }

            return allRootTileModels;
        }

        private bool IsRoleSpecifiedInTileConfiguration(string roleName, string[] capabilities, IHaloTileConfiguration tileConfiguration)
        {
            if (string.IsNullOrWhiteSpace(roleName) || capabilities == null) { return false; }

            var isInRole = tileConfiguration.IsInRole(roleName);
            return capabilities.Length > 0 ?
                    tileConfiguration.IsInCapability(capabilities) || isInRole
                    : isInRole;
        }

        private HaloTileConfigurationModel RetrieveLinkedRootTileConfiguration(IHaloModuleConfiguration moduleConfiguration, string moduleParameters)
        {
            var linkedConfiguration = tileConfigurationRepository.FindModuleLinkedTileConfigurationByName(moduleConfiguration, moduleParameters);
            if (linkedConfiguration == null) { return null; }

            var rootTileConfiguration = this.tileConfigurationRepository.FindRootTileConfigurationForLinkedConfiguration(linkedConfiguration);
            var model = Mapper.Map<IHaloRootTileConfiguration, HaloTileConfigurationModel>(rootTileConfiguration);
            return model;
        }
    }
}

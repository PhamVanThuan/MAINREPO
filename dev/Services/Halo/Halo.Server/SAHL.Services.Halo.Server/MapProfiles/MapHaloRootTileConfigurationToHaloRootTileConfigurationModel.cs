using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core;
using SAHL.UI.Halo.Shared;
using SAHL.Core.BusinessModel;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Halo.Server.MapProfiles
{
    public class MapHaloRootTileConfigurationToHaloRootTileConfigurationModel : Profile, IAutoMapperProfile
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;
        private readonly ITileDataRepository tileDataRepository;

        public MapHaloRootTileConfigurationToHaloRootTileConfigurationModel(ITileConfigurationRepository tileConfigurationRepository, ITileDataRepository tileDataRepository)
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
            Mapper.CreateMap<IHaloRootTileConfiguration, HaloRootTileConfigurationModel>()
                .ForMember(model => model.RootTileConfigurations, expression => expression.Ignore())
                .ForMember(model => model.ChildTileConfigurations, expression => expression.Ignore())
                .ForMember(model => model.BusinessContext, expression => expression.Ignore())
                .ForMember(model => model.Role, expression => expression.Ignore())
                .AfterMap((configuration, model) =>
                    {
                        var rootTileConfigurationModel = new HaloTileConfigurationModel
                            {
                                BusinessContext = model.BusinessContext,
                                Role            = model.Role,
                            };

                        model.RootTileConfigurations = new List<HaloTileConfigurationModel>
                            { 
                                Mapper.Map<IHaloRootTileConfiguration, HaloTileConfigurationModel>(configuration, rootTileConfigurationModel)
                            };
                        model.ChildTileConfigurations = this.RetrieveChildTileConfigurations(configuration, model.BusinessContext, model.Role);
                    });
        }

        private IEnumerable<HaloTileConfigurationModel> RetrieveChildTileConfigurations(IHaloRootTileConfiguration rootTileConfiguration, 
                                                                                        BusinessContext businessContext, HaloRoleModel roleModel)
        {
            var childTileConfigurations = new List<HaloTileConfigurationModel>();
            if (rootTileConfiguration == null || businessContext == null) { return childTileConfigurations; }

            var tileConfigurations = tileConfigurationRepository.FindChildTileConfigurations(rootTileConfiguration);
            if (tileConfigurations == null || !tileConfigurations.Any()) { return childTileConfigurations; }

            foreach (var childTileConfiguration in tileConfigurations)
            {
                if (!this.IsRoleSpecifiedInTileConfiguration(roleModel, childTileConfiguration)) { continue; }

                var childDataProvider = this.tileDataRepository.FindTileChildDataProvider(childTileConfiguration);
                if (childDataProvider == null) { continue; }

                childTileConfigurations.AddRange(this.LoadChildTileConfigurations(businessContext, roleModel, childDataProvider, childTileConfiguration));
            }

            return childTileConfigurations;
        }

        private IEnumerable<HaloTileConfigurationModel> LoadChildTileConfigurations(BusinessContext businessContext, 
                                                                                    HaloRoleModel roleModel,
                                                                                    IHaloTileChildDataProvider childDataProvider, 
                                                                                    IHaloChildTileConfiguration childTileConfiguration)
        {
            var childTileConfigurations = new List<HaloTileConfigurationModel>();

            try
            {
                var tileSubKeys = childDataProvider.LoadSubKeys(businessContext);
                if (tileSubKeys == null || !tileSubKeys.Any()) { return childTileConfigurations; }

                foreach (var tileSubKey in tileSubKeys)
                {
                    var tileConfigurationModel = this.ProcessTileSubKey(tileSubKey, roleModel, childTileConfiguration, childDataProvider);
                    if (tileConfigurationModel == null) { continue; }

                    childTileConfigurations.Add(tileConfigurationModel);
                }
            }
            catch (Exception runtimeException)
            {
                var errorMessage = string.Format("Exception occurred while loading Child Data for {0} using Data Provider {1}\n{2}\n{3}",
                    childTileConfiguration.Name, childDataProvider.GetType().Name, childDataProvider.GetSqlStatement(businessContext), runtimeException);
                Trace.WriteLine(errorMessage);

                // Do nothing else ...
            }

            return childTileConfigurations;
        }

        private bool IsRoleSpecifiedInTileConfiguration(HaloRoleModel roleModel, IHaloTileConfiguration tileConfiguration)
        {
            if (roleModel == null || string.IsNullOrWhiteSpace(roleModel.RoleName)) { return false; }

            return roleModel.Capabilities.Length > 0
                ? tileConfiguration.IsInRole(roleModel.RoleName)
                : tileConfiguration.IsInRole(roleModel.RoleName) && 
                  tileConfiguration.IsInCapability(roleModel.Capabilities);
        }

        private HaloTileConfigurationModel ProcessTileSubKey(BusinessContext tileSubKey, 
                                                             HaloRoleModel roleModel,
                                                             IHaloChildTileConfiguration childTileConfiguration, 
                                                             IHaloTileChildDataProvider childDataProvider)
        {
            try
            {
                var tileConfigurationModel = new HaloTileConfigurationModel
                    {
                        BusinessContext = tileSubKey,
                        Role            = roleModel,
                    };
                tileConfigurationModel = Mapper.Map<IHaloChildTileConfiguration, HaloTileConfigurationModel>(childTileConfiguration, tileConfigurationModel);
                return tileConfigurationModel;
            }
            catch (Exception runtimeException)
            {
                var errorMessage = string.Format("Exception occurred while loading Child Content Data for {0} using Data Provider {1}\n{2}\n{3}",
                    childTileConfiguration.Name, childDataProvider.GetType().Name, childDataProvider.GetSqlStatement(tileSubKey), runtimeException);
                Trace.WriteLine(errorMessage);

                // Do nothing else ...
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.UI.Halo.Shared;
using SAHL.Core.BusinessModel;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Queries;

namespace SAHL.Services.Halo.Server.QueryHandlers
{
    public class GetRootTileConfigurationQueryHandler : QueryHandlerBase, IServiceQueryHandler<GetRootTileConfigurationQuery>
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;
        private readonly ITileDataRepository tileDataRepository;

        public GetRootTileConfigurationQueryHandler(ITileConfigurationRepository tileConfigurationRepository,
                                                    ITileDataRepository tileDataRepository,
                                                    IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            if (tileDataRepository == null) { throw new ArgumentNullException("tileDataRepository"); }

            this.tileConfigurationRepository = tileConfigurationRepository;
            this.tileDataRepository = tileDataRepository;
        }

        public ISystemMessageCollection HandleQuery(GetRootTileConfigurationQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            try
            {
                var rootTileConfiguration = this.RetrieveRootTileConfiguration(query.ApplicationName, query.ModuleName, query.RootTileName);
                if (rootTileConfiguration == null) { return messages; }

                var configurationModel = this.CreateRootTileConfigurationModel(rootTileConfiguration, query.BusinessContext, query.RoleModel);

                if (rootTileConfiguration.IsDynamicTile())
                {
                    var dynamicTileConfiguration = this.LoadDynamicRootTileConfiguration(rootTileConfiguration as IHaloDynamicRootTileConfiguration, query.BusinessContext);
                    if (dynamicTileConfiguration == null)
                    {
                        throw new Exception(string.Format("Loading of the Dynamic Root Tile Configuration failed for {0}", query.RootTileName));
                    }

                    var dynamicTileModel = this.CreateRootTileConfigurationModel(dynamicTileConfiguration, query.BusinessContext, query.RoleModel);

                    var childTileConfigurations = new List<HaloTileConfigurationModel>();
                    childTileConfigurations.AddRange(configurationModel.ChildTileConfigurations);
                    childTileConfigurations.AddRange(dynamicTileModel.ChildTileConfigurations);

                    configurationModel.RootTileConfigurations = dynamicTileModel.RootTileConfigurations;
                    configurationModel.ChildTileConfigurations = childTileConfigurations;
                }

                var queryResult = new RootTileConfigurationQueryResult
                    {
                        RootTileConfiguration = configurationModel,
                    };
                query.Result = new ServiceQueryResult<RootTileConfigurationQueryResult>(new List<RootTileConfigurationQueryResult>() { queryResult });
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to load the Root Tile Configurations\n{0}", runtimeException),
                                                      SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }

        private IHaloRootTileConfiguration RetrieveRootTileConfiguration(string applicationName, string moduleName, string rootTileName)
        {
            var applicationConfiguration = this.tileConfigurationRepository.FindApplicationConfiguration(applicationName);
            if (applicationConfiguration == null)
            {
                throw new Exception("Application Configuration not found");
            }

            var moduleConfiguration = this.tileConfigurationRepository.FindModuleConfiguration(applicationConfiguration, moduleName);
            if (moduleConfiguration == null)
            {
                throw new Exception("Module Configuration not found");
            }

            var rootTileConfiguration = this.tileConfigurationRepository.FindRootTileConfiguration(moduleConfiguration, rootTileName);
            if (rootTileConfiguration == null)
            {
                throw new Exception("Root Tile Configuration not found");
            }

            return rootTileConfiguration as IHaloRootTileConfiguration;
        }

        private IHaloRootTileConfiguration LoadDynamicRootTileConfiguration(IHaloDynamicRootTileConfiguration rootTileConfiguration, BusinessContext businessContext)
        {
            IHaloRootTileConfiguration tileConfiguration = rootTileConfiguration;

            var dynamicDataProvider = this.tileDataRepository.FindTileDynamicDataProvider(rootTileConfiguration);
            if (dynamicDataProvider == null)
            {
                tileConfiguration = rootTileConfiguration.GetRootTileConfiguration(businessContext);
            }
            else
            {
                var dynamicData = dynamicDataProvider.LoadDynamicData(businessContext);
                tileConfiguration = rootTileConfiguration.GetRootTileConfiguration(dynamicData);
            }

            return tileConfiguration;
        }

        private HaloRootTileConfigurationModel CreateRootTileConfigurationModel(IHaloRootTileConfiguration rootTileConfiguration, BusinessContext businessContext, HaloRoleModel roleModel)
        {
            var rootTileConfigurationModel = new HaloRootTileConfigurationModel
                {
                    BusinessContext = businessContext,
                    Role            = roleModel,
                };
            return Mapper.Map<IHaloRootTileConfiguration, HaloRootTileConfigurationModel>(rootTileConfiguration, rootTileConfigurationModel);
        }
    }
}

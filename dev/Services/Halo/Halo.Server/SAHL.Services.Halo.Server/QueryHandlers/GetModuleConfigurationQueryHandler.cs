using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core.Services;
using SAHL.UI.Halo.Shared;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Queries;

namespace SAHL.Services.Halo.Server.QueryHandlers
{
    public class GetModuleConfigurationQueryHandler : IServiceQueryHandler<GetModuleConfigurationQuery>
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public GetModuleConfigurationQueryHandler(ITileConfigurationRepository tileConfigurationRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            this.tileConfigurationRepository = tileConfigurationRepository;
        }

        public ISystemMessageCollection HandleQuery(GetModuleConfigurationQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            try
            {
                var applicationConfiguration = tileConfigurationRepository.FindApplicationConfiguration(query.ApplicationName);
                if (applicationConfiguration == null)
                {
                    throw new Exception("Application Configuration not found");
                }

                var tileModels = this.RetrieveApplicationModule(applicationConfiguration, query.ModuleName, 
                                                                query.ReturnAllRoots, query.ModuleParameters, query.Role);

                var queryResult = new ModuleConfigurationQueryResult
                    {
                        ModuleConfiguration = tileModels,
                    };
                query.Result = new ServiceQueryResult<ModuleConfigurationQueryResult>(new List<ModuleConfigurationQueryResult>() { queryResult });
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to load the Application Configurations\n{0}", runtimeException),
                                                      SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }

        private HaloModuleTileModel RetrieveApplicationModule(IHaloApplicationConfiguration applicationConfiguration, string moduleName,
                                                              bool returnAllRoots, string moduleParameters, HaloRoleModel roleModel)
        {
            var moduleConfigurations = tileConfigurationRepository.FindApplicationModuleConfigurations(applicationConfiguration);
            if (moduleConfigurations == null || !moduleConfigurations.Any()) { return null; }

            var moduleConfiguration = moduleConfigurations.FirstOrDefault(configuration => configuration.Name == moduleName);
            if (moduleConfiguration == null) { return null; }

            var haloModuleModel = new HaloModuleTileModel
                {
                    AllRoots         = returnAllRoots,
                    ModuleParameters = moduleParameters,
                    RoleModel        = roleModel,
                };
            haloModuleModel = Mapper.Map<IHaloModuleConfiguration, HaloModuleTileModel>(moduleConfiguration, haloModuleModel);
            return haloModuleModel;
        }
    }
}

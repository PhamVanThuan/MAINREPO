using AutoMapper;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Halo.Server
{
    public class GetApplicationConfigurationMenuItemsQueryHandler : IServiceQueryHandler<GetApplicationConfigurationMenuItemsQuery>
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public GetApplicationConfigurationMenuItemsQueryHandler(ITileConfigurationRepository tileConfigurationRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            this.tileConfigurationRepository = tileConfigurationRepository;
        }

        public ISystemMessageCollection HandleQuery(GetApplicationConfigurationMenuItemsQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.ApplicationName)) { throw new ArgumentNullException("applicationName"); }

            var messages = SystemMessageCollection.Empty();

            try
            {
                var applicationConfiguration = tileConfigurationRepository.FindApplicationConfiguration(query.ApplicationName);
                var menuItems = tileConfigurationRepository.FindAllMenuItemsForApplication(applicationConfiguration);
                if (menuItems == null) { return messages; }

                var applicationMenuItems = new List<ApplicationMenuItem>();

                foreach (var haloMenuItem in menuItems)
                {
                    if (query.RoleModel != null &&
                        !this.IsRoleSpecifiedInTileConfiguration(query.RoleModel.RoleName, query.RoleModel.Capabilities, haloMenuItem))
                    {
                        continue;
                    }

                    var applicationMenuItem = Mapper.Map<IHaloMenuItem, ApplicationMenuItem>(haloMenuItem);
                    applicationMenuItems.Add(applicationMenuItem);
                }

                var serviceQueryResult = new ServiceQueryResult<ApplicationMenuItem>(applicationMenuItems);
                query.Result = serviceQueryResult;
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to load the Application Menu Items\n{0}", runtimeException),
                                                      SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }
        private bool IsRoleSpecifiedInTileConfiguration(string roleName, string[] capabilities, IHaloMenuItem haloMenuItem)
        {
            if (string.IsNullOrWhiteSpace(roleName) || capabilities == null) { return false; }
            var isInRole = haloMenuItem.IsInRole(roleName);
            return capabilities.Length > 0 ?
                    haloMenuItem.IsInCapability(capabilities) || isInRole
                    : isInRole;
        }
    }
}

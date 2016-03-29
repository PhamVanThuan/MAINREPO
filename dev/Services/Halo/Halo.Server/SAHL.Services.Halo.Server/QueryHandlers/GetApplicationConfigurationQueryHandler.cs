using System;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core.Services;
using SAHL.UI.Halo.Shared;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Halo.Server
{
    public class GetApplicationConfigurationQueryHandler : IServiceQueryHandler<GetApplicationConfigurationQuery>
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public GetApplicationConfigurationQueryHandler(ITileConfigurationRepository tileConfigurationRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            this.tileConfigurationRepository = tileConfigurationRepository;
        }

        public ISystemMessageCollection HandleQuery(GetApplicationConfigurationQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.ApplicationName)) { throw new ArgumentNullException("applicationName"); }

            var messages = SystemMessageCollection.Empty();

            try
            {
                var queryResult = new ApplicationConfigurationQueryResult
                                        {
                                            HaloApplicationModel = this.RetrieveApplicationConfiguration(query.ApplicationName),
                                        };
                query.Result = new ServiceQueryResult<ApplicationConfigurationQueryResult>(new List<ApplicationConfigurationQueryResult>() { queryResult } );
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to load the Application Configurations\n{0}", runtimeException),
                                                      SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }

        private HaloApplicationModel RetrieveApplicationConfiguration(string applicationName)
        {
            var applicationConfiguration = tileConfigurationRepository.FindApplicationConfiguration(applicationName);
            if (applicationConfiguration == null) { return null; }

            var haloApplication = Mapper.Map<IHaloApplicationConfiguration, HaloApplicationModel>(applicationConfiguration);
            return haloApplication;
        }
    }
}

using System;
using System.Linq;

using SAHL.Core.Services;
using SAHL.UI.Halo.Shared;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Halo.Server
{
    public class GetAllApplicationsQueryHandler : IServiceQueryHandler<GetAllApplicationsQuery>
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public GetAllApplicationsQueryHandler(ITileConfigurationRepository tileConfigurationRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            this.tileConfigurationRepository = tileConfigurationRepository;
        }

        public ISystemMessageCollection HandleQuery(GetAllApplicationsQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            try
            {
                var applicationConfigurations = tileConfigurationRepository.FindAllApplicationConfigurations();

                var haloApplications   = applicationConfigurations.Select(configuration => new HaloApplicationModel
                                                                                            {
                                                                                                Name     = configuration.Name,
                                                                                                Sequence = configuration.Sequence,
                                                                                            });
                var serviceQueryResult = new ServiceQueryResult<HaloApplicationModel>(haloApplications);

                query.Result = serviceQueryResult;
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to load the Application Configurations\n{0}", runtimeException), 
                                                      SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }
    }
}

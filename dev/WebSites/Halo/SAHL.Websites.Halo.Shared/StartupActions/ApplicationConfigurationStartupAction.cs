using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Services.Interfaces.Halo;

namespace SAHL.Websites.Halo.Shared
{
    public class ApplicationConfigurationStartupAction : UnitOfWorkActionBase
    {
        private HaloHttpServiceClient _serviceClient;

        public ApplicationConfigurationStartupAction()
        {
            this.Sequence  = 2;
            _serviceClient = HaloHttpServiceClient.Instance();
        }

        public static IEnumerable<HaloApplication> ApplicationConfigurations { get; internal set; }

        public override bool Execute()
        {
            try
            {
                var query = new GetAllApplicationsQuery();
                _serviceClient.PerformQuery(query);

                ApplicationConfigurationStartupAction.ApplicationConfigurations = this.RetrieveHomeApplicationConfiguration(query.Result.Results);

                return (ApplicationConfigurationStartupAction.ApplicationConfigurations != null) && 
                        ApplicationConfigurationStartupAction.ApplicationConfigurations.Any();
            }
            catch (Exception runtimeException)
            {
                return false;
            }
        }

        private IEnumerable<HaloApplication> RetrieveHomeApplicationConfiguration(IEnumerable<HaloApplication> applicationConfigurations)
        {
            var currentOrgRole = UserDetailsStartupAction.UserInfo.Roles.FirstOrDefault();
            var homeApplication = applicationConfigurations.FirstOrDefault(app => app.Sequence == 1);
            if (string.IsNullOrWhiteSpace(currentOrgRole) || (homeApplication == null)) { return applicationConfigurations; }

            //TODO: The Sales Org Role is being used for dev purposes - NEEDS TO BE REMOVED
            currentOrgRole = "Sales";

            var homeApplicationQuery = new GetApplicationConfigurationForRoleQuery(homeApplication.Name, currentOrgRole);
            _serviceClient.PerformQuery(homeApplicationQuery);
            if (!homeApplicationQuery.Result.Results.Any()) { return applicationConfigurations; }

            var queryResult = homeApplicationQuery.Result.Results.FirstOrDefault(result => result.HaloApplication.Name == homeApplication.Name);
            if ((queryResult == null) || (queryResult.HaloApplication == null)) { return applicationConfigurations; }

            var haloApplications = applicationConfigurations.Select(configuration => configuration.Name == homeApplication.Name
                                                                                            ? queryResult.HaloApplication
                                                                                            : configuration).ToList();
            return haloApplications;
        }
    }
}

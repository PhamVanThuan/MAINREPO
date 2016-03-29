using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Query;
using SAHL.Services.Query.Factories;

namespace SAHL.Services.Query.QueryHandlers
{
    public class QueryServiceQueryHandler : IServiceQueryHandler<QueryServiceQuery>
    {
        private readonly IWebHttpClient webHttpClient;
        private readonly IServiceUrlConfigurationProvider serviceUrlConfigurationProvider;

        public QueryServiceQueryHandler(IWebHttpClient webHttpClient, IServiceUrlConfigurationProvider serviceUrlConfigurationProvider)
        {
            this.webHttpClient = webHttpClient;
            this.serviceUrlConfigurationProvider = serviceUrlConfigurationProvider;

            var baseUrl = string.Format("http://{0}/{1}/",
                this.serviceUrlConfigurationProvider.ServiceHostName,
                this.serviceUrlConfigurationProvider.ServiceName
                );
            this.webHttpClient.BaseAddress = new Uri(baseUrl);
        }

        public ISystemMessageCollection HandleQuery(QueryServiceQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            //NOTE: webHttpClient.Get() is very sensitive to trailing slashes on the BaseAddress and slashes at the beginning of the relative URI
            //Only works if there is trailing slash on base, and no slash on relative
            var url = query.RelativeUrl.StartsWith("/") 
                ? query.RelativeUrl.Substring(1) 
                : query.RelativeUrl;

            var response = webHttpClient.Get(url).Result;

            if (response == null)
            {
                var message = string.Format("No response was received when attempting to retrieve content from {0}", url);
                messages.AddMessage(new SystemMessage(message, SystemMessageSeverityEnum.Error));
                return messages;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var result = new QueryServiceQueryResult(responseContent);
                query.Result = new ServiceQueryResult<QueryServiceQueryResult>(result.ToSingleItemEnumerable());
            }
            else
            {
                var message = string.Format("An HTTP status code of {0} was returned from the QueryService when attempting to retrieve content from {1}", 
                    response.StatusCode, 
                    url);

                messages.AddMessage(new SystemMessage(message, SystemMessageSeverityEnum.Error));
            }

            return messages;
        }
    }
}

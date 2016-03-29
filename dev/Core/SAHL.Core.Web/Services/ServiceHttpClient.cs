using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Web.Services
{
    public class ServiceHttpClient : IServiceClient, IServiceHttpClient
    {
        public delegate void CurrentlyPerformingRequestHandler(object sender, CurrentlyPerformingRequestEventArgs e);

        private const string CommandBaseUrl           = "api/commandhttphandler/performhttpcommand";
        private const string SelfHostedCommandBaseUrl = "api/commandhttphandler/performhttpcommand";
        private const string QueryBaseUrl             = "api/queryhttphandler/performhttpquery";
        public const string MetaHeaderPrefix          = "#md#_";

        private readonly IJsonActivator jsonActivator;
        private readonly IServiceUrlConfiguration serviceConfiguration;
        private readonly IServiceUrlConfigurationProvider serviceUrlConfigurationProvider;

        private IDictionary<string, string> baseHeaders = new Dictionary<string, string>();

        public event CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;

        [Obsolete("Use constructor with ServiceUrlConfigurationProvider")]
        public ServiceHttpClient(IServiceUrlConfiguration serviceConfiguration, IJsonActivator jsonActivator)
            : this(jsonActivator)
        {
            if (serviceConfiguration == null) { throw new ArgumentNullException("serviceConfiguration"); }
            this.serviceConfiguration = serviceConfiguration;
        }

        public ServiceHttpClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : this(jsonActivator)
        {
            if (serviceUrlConfigurationProvider == null) { throw new ArgumentNullException("serviceUrlConfigurationProvider"); }
            this.serviceUrlConfigurationProvider = serviceUrlConfigurationProvider;
        }

        protected ServiceHttpClient(IJsonActivator jsonActivator)
        {
            if (jsonActivator == null) { throw new ArgumentNullException("jsonActivator"); }

            this.jsonActivator     = jsonActivator;
            this.HttpClientHandler = new HttpClientHandler();
        }

        public string AuthenticationHeader { get; protected set; }

        public string AuthenticationToken { get; protected set; }

        protected HttpClientHandler HttpClientHandler { get; private set; }

        protected PaginationQueryParameter PaginationQueryParameter { get; private set; }

        protected FilterQueryParameter FilterQueryParameter { get; private set; }

        protected SortQueryParameter SortQueryParameter { get; private set; }

        public void UseCustomHeaderAuth(string authHeader)
        {
            this.AuthenticationHeader = authHeader;
        }

        public void AddCustomHeaders(IDictionary<string, string> headers)
        {
            this.baseHeaders = headers;
        }

        public void UseWindowsAuth()
        {
            this.HttpClientHandler.UseDefaultCredentials = true;
        }

        public void UseWindowsAuth(ICredentials userCredentials)
        {
            this.HttpClientHandler.Credentials = userCredentials;
        }

        public void SetPaginationQueryParameters(int pageSize, int currentPage)
        {
            this.PaginationQueryParameter = new PaginationQueryParameter
                {
                    CurrentPage = currentPage,
                    PageSize    = pageSize,
                };
        }

        public void SetFilterQueryParameters(string filterOn, string filterValue)
        {
            this.FilterQueryParameter = new FilterQueryParameter
                {
                    FilterOn    = filterOn,
                    FilterValue = filterValue,
                };
        }

        public void SetSortQueryParameters(string orderBy, SortQueryParameter.SortDirectionOptions sortDirection)
        {
            this.SortQueryParameter = new SortQueryParameter
                {
                    OrderBy       = orderBy,
                    SortDirection = sortDirection,
                };
        }

        protected ISystemMessageCollection PerformCommandInternal<T>(T command, IServiceRequestMetadata commandMetadata)
            where T : IServiceCommand
        {
            this.HandleCurrentPerformingRequest<T>(commandMetadata);

            var httpClient = this.GetConfiguredClient();

            this.ProcessMetadata(commandMetadata, httpClient);
            var httpResponseMessage = this.PostCommandToServer(command, httpClient);

            if (!string.IsNullOrEmpty(AuthenticationHeader) && httpResponseMessage.Headers.Contains(AuthenticationHeader))
            {
                this.AuthenticationToken = httpResponseMessage.Headers.GetValues(AuthenticationHeader).FirstOrDefault();
            }

            var jsonStr = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var result = this.jsonActivator.DeserializeObject<ServiceCommandResult>(jsonStr);
            if (result != null) { return result.SystemMessages; }

            if (!jsonStr.StartsWith("{"))
            {
                throw new Exception(httpResponseMessage.ReasonPhrase);
            }

            var exception = this.CustomFormatException(jsonStr);
            throw new Exception(exception.ToString());
        }

        private HttpResponseMessage PostCommandToServer<T>(T command, IWebHttpClient httpClient) where T : IServiceCommand
        {
            var commandContent = this.GetContentFromObject(command);
            var baseUrl = this.serviceUrlConfigurationProvider.IsSelfHostedService
                ? SelfHostedCommandBaseUrl
                : CommandBaseUrl;
            var httpResponseMessage = httpClient.PostAsync(baseUrl, commandContent).Result;
            return httpResponseMessage;
        }

        private void HandleCurrentPerformingRequest<T>(IServiceRequestMetadata metadata) where T : IServiceCommand
        {
            CurrentlyPerformingRequestHandler currentlyPerformingRequest = CurrentlyPerformingRequest;
            if (currentlyPerformingRequest != null)
            {
                currentlyPerformingRequest(this, new CurrentlyPerformingRequestEventArgs(typeof(T), metadata));
            }
        }

        private void ProcessMetadata(IServiceRequestMetadata commandMetadata, IWebHttpClient httpClient)
        {
            if (commandMetadata == null) { return; }
            foreach (var kvp in commandMetadata)
            {
                httpClient.DefaultRequestHeaders.Add(MetadataManager.METADATA_KEY_PREFIX + kvp.Key, kvp.Value);
            }
        }

        protected async Task<ISystemMessageCollection> PerformCommandInternalAsync<T>(T command, IDictionary<string, string> commandMetadata) 
            where T : IServiceCommand
        {
            var httpClient = this.GetConfiguredClient();

            if (commandMetadata != null)
            {
                foreach (KeyValuePair<string, string> kvp in commandMetadata)
                {
                    httpClient.DefaultRequestHeaders.Add(MetadataManager.METADATA_KEY_PREFIX + kvp.Key, kvp.Value);
                }
            }
            var commandContent = this.GetContentFromObject(command);
            var httpResponseMessage = await httpClient.PostAsync(ServiceHttpClient.CommandBaseUrl, commandContent);
            if (!string.IsNullOrEmpty(AuthenticationHeader) &&
                httpResponseMessage.Headers.Contains(AuthenticationHeader))
            {
                this.AuthenticationToken = httpResponseMessage.Headers.GetValues(AuthenticationHeader).FirstOrDefault();
            }
            try
            {
                var result = httpResponseMessage.Content.ReadAsAsync<ServiceCommandResult>().Result;
                return result.SystemMessages;
            }
            catch (Exception)
            {
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (!result.StartsWith("{"))
                {
                    throw new Exception(httpResponseMessage.ReasonPhrase);
                }

                var exception = this.CustomFormatException(result);
                throw new Exception(exception.ToString());
            }
        }

        protected ISystemMessageCollection PerformQueryInternal<T>(T query) where T : IServiceQuery
        {
            this.HandleCurrentPerformingRequest<T>(null);

            var httpClient = this.GetConfiguredClient();
            var queryUrl   = this.GetQueryUrl();

            var httpResponseMessage = httpClient.PostAsync(queryUrl, this.GetContentFromObject(query)).Result;
            var json                = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var serviceQueryResult  = jsonActivator.DeserializeObject<ServiceQueryResult>(json);

            if (serviceQueryResult != null)
            {
                this.SetQueryResult(query, serviceQueryResult.ReturnData);
                return serviceQueryResult.SystemMessages;
            }

            if (!json.StartsWith("{"))
            {
                throw new Exception(httpResponseMessage.ReasonPhrase);
            }

            var exception = CustomFormatException(json);
            throw new Exception(exception.ToString());
        }

        protected virtual IWebHttpClient GetConfiguredClient()
        {
            var httpClient = new WebHttpClient(this.HttpClientHandler)
            {
                BaseAddress = this.GetHttpBaseAddress()
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(this.AuthenticationHeader))
            {
                httpClient.DefaultRequestHeaders.Add(this.AuthenticationHeader, this.AuthenticationToken);
            }

            foreach (KeyValuePair<string, string> kvp in this.baseHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
            }

            return httpClient;
        }

        private Uri GetHttpBaseAddress()
        {
            Uri baseAddress = null;
            if (serviceConfiguration != null)
            {
                baseAddress = new Uri(this.serviceConfiguration.GetCommandServiceUrl());
            }

            if (serviceUrlConfigurationProvider == null)
            {
                return baseAddress;
            }

            var serviceName = string.IsNullOrWhiteSpace(serviceUrlConfigurationProvider.ServiceName)
                                            ? string.Empty
                                            : string.Format("{0}/", serviceUrlConfigurationProvider.ServiceName);
            var urlString = string.Format("http://{0}/{1}", serviceUrlConfigurationProvider.ServiceHostName, serviceName);
            baseAddress   = new Uri(urlString);

            return baseAddress;
        }

        private string GetQueryUrl()
        {
            var queryUrl = QueryBaseUrl;

            if (this.PaginationQueryParameter != null &&
                this.PaginationQueryParameter.PageSize > 0 &&
                this.PaginationQueryParameter.CurrentPage >= 0)
            {
                queryUrl = string.Format("{0}/?pageSize={1}&currentPage={2}", 
                                         queryUrl, this.PaginationQueryParameter.PageSize, this.PaginationQueryParameter.CurrentPage);
            }

            queryUrl = this.GetFilterQueryUrl(queryUrl);
            queryUrl = this.GetSortQueryUrl(queryUrl);

            return queryUrl;
        }

        private string GetFilterQueryUrl(string queryUrl)
        {
            if (this.FilterQueryParameter == null || 
                string.IsNullOrEmpty(this.FilterQueryParameter.FilterOn) || 
                string.IsNullOrEmpty(this.FilterQueryParameter.FilterValue)) { return queryUrl; }

            var filterQueryUrl = string.Format(queryUrl.Contains("performhttpquery/?") ? "{0}&filterOn={1}&filterValue={2}" : "{0}/?filterOn={1}&filterValue={2}", 
                                               queryUrl, this.FilterQueryParameter.FilterOn, this.FilterQueryParameter.FilterValue);
            return filterQueryUrl;
        }

        private string GetSortQueryUrl(string queryUrl)
        {
            var sortQueryUrl = queryUrl;

            if (this.SortQueryParameter != null &&
                !string.IsNullOrEmpty(this.SortQueryParameter.OrderBy) &&
                !string.IsNullOrEmpty(this.SortQueryParameter.SortDirection.ToString()))
            {
                sortQueryUrl = string.Format(queryUrl.Contains("performhttpquery/?") ? "{0}&orderBy={1}&sortDirection={2}" : "{0}/?orderBy={1}&sortDirection={2}",
                                             queryUrl, this.SortQueryParameter.OrderBy, this.SortQueryParameter.SortDirection);
            }
            return sortQueryUrl;
        }

        private StringBuilder CustomFormatException(string result)
        {
            var jObject          = JObject.Parse(result);
            var httpError        = jObject.ToObject<HttpError>();
            var exceptionMessage = new StringBuilder();

            foreach (var item in httpError)
            {
                exceptionMessage.AppendFormat("{0} : ", item.Key);
                exceptionMessage.AppendLine(item.Value != null ? item.Value.ToString() : string.Empty);
            }

            return exceptionMessage;
        }

        protected virtual void SetQueryResult(IServiceQuery query, object result)
        {
            var queryType          = query.GetType();
            var genericCommandType = queryType.GetInterfaces().SingleOrDefault(x => x.Name == typeof (IServiceQuery<>).Name);
            if (genericCommandType == null)
            {
                throw new Exception(string.Format("Unable to create IServiceQuery<> generic type with {0}", queryType));
            }

            var resultProp = genericCommandType.GetProperty("Result");
            resultProp.SetValue(query, result);
        }

        protected virtual HttpContent GetContentFromObject<T>(T objectToSerialize)
        {
            var content = new StringContent(jsonActivator.SerializeObject(objectToSerialize), Encoding.UTF8, "application/json");
            return content;
        }
    }
}

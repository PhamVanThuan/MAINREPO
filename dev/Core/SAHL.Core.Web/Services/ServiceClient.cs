using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Web.Http;

namespace SAHL.Core.Web.Services
{
    [Obsolete("Use the ServiceHttpClient class")]
    public abstract class ServiceClient
    {
        private IServiceUrlConfiguration serviceConfiguration;

        protected HttpClientHandler HttpClientHandler { get; private set; }

        public void UseWindowsAuth()
        {
            if (this.HttpClientHandler == null)
            {
                this.HttpClientHandler = new HttpClientHandler();
            }
            this.HttpClientHandler.UseDefaultCredentials = true;
        }

        public void UseWindowsAuth(ICredentials userCredentials)
        {
            if (this.HttpClientHandler == null)
            {
                this.HttpClientHandler = new HttpClientHandler();
            }

            this.HttpClientHandler.Credentials = userCredentials;
        }

        private static readonly DefaultContractResolver Resolver = new DefaultContractResolver
        {
            IgnoreSerializableAttribute = true
        };

        public ServiceClient(IServiceUrlConfiguration serviceConfiguration)
        {
            this.serviceConfiguration = serviceConfiguration;
        }

        protected ISystemMessageCollection PerformCommandInternal<T>(T command, IServiceRequestMetadata metadata) where T : IServiceCommand
        {
            HttpClient client = this.GetConfiguredClient();

            string url = string.Empty;
            if (command is IServiceCommandWithReturnedData)
            {
                url = "api/commandhandlerwithreturneddata/PerformCommandWithResult";
            }
            else
            {
                url = "api/commandhandler/performcommand";
            }

            ITraceWriter traceWriter = new DiagnosticsTraceWriter { LevelFilter = TraceLevel.Verbose };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All,
                ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance
                },
                TraceWriter = traceWriter,
                Error = (error, eventargs) =>
                {
                }
            };

            HttpResponseMessage response = client.PostAsync<T>(url, command,
                new JsonMediaTypeFormatter
            {
                SerializerSettings = settings
            }).Result;

            if (response.IsSuccessStatusCode)
            {
                if (command is IServiceCommandWithReturnedData)
                {
                    try
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        JObject jObj = JObject.Parse(json);
                        Type genericCommandType = jObj["$type"].ToObject<Type>();
                        ServiceQueryResult result = JsonConvert.DeserializeObject(json, genericCommandType, settings) as ServiceQueryResult;
                        this.SetCommandResult(command as IServiceCommandWithReturnedData, result.ReturnData);
                        return result.SystemMessages;
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    var result = response.Content.ReadAsAsync<ServiceCommandResult>().Result;
                    return result.SystemMessages;
                }
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                if (result.StartsWith("{"))
                {
                    JObject jObj = JObject.Parse(result);
                    HttpError error = jObj.ToObject<HttpError>();
                    StringBuilder exception = new StringBuilder();
                    foreach (var item in error)
                    {
                        exception.AppendFormat("{0} : ", item.Key);
                        exception.AppendLine(item.Value.ToString());
                    }
                    throw new Exception(exception.ToString());
                }
                throw new Exception(response.ReasonPhrase);
            }
        }

        private void SetCommandResult(IServiceCommandWithReturnedData command, object result)
        {
            Type commandType = command.GetType();
            Type genericCommandType = commandType.GetInterfaces().Where(x => x.Name == typeof(IServiceCommandWithReturnedData<>).Name).SingleOrDefault();
            PropertyInfo resultProp = genericCommandType.GetProperty("Result");
            resultProp.SetValue(command, result);
        }

        private HttpClient GetConfiguredClient()
        {
            var client = new WebHttpClient(this.HttpClientHandler)
            {
                BaseAddress = new Uri(this.serviceConfiguration.GetCommandServiceUrl())
            };

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
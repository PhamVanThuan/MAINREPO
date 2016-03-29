using DomainService2.V3.Client.Commands;
using DomainService2.V3.Client.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.V3.Client
{
    public class LightweightV3ClientBase
    {
        const string COMMAND_BASE_URL = "api/commandhttphandler/performhttpcommand";
        const string METADATA_KEY_PREFIX = "#md#_";

        private string serviceName;

        public LightweightV3ClientBase(string serviceName)
        {
            this.serviceName = serviceName;
        }

        protected CommandResult PerformCommand(IV3Command command)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseDefaultCredentials = true;

            using (var client = new HttpClient(httpClientHandler))
            {
                client.BaseAddress = new Uri(GetBaseAddressUrl());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(METADATA_KEY_PREFIX + "_username", command.ADUserName);

                HttpContent commandContent = new StringContent(command.ToJSON(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(COMMAND_BASE_URL, commandContent).Result;

                return new CommandResult(response.Content.ReadAsStringAsync().Result);
            }
        }

        private string GetBaseAddressUrl()
        {
            return string.Format("http://{0}/{1}", ServiceHostName, ServiceName);
        }


        private string ServiceHostName
        {
            get
            {
                var settingName = string.Format("{0}_HostName", serviceName);
                if (ConfigurationManager.AppSettings[settingName] == null)
                {
                    throw new Exception(string.Format("Service Setting {0} not found", settingName));
                }

                return ConfigurationManager.AppSettings[settingName];
            }
        }

        private string ServiceName
        {
            get
            {
                var settingName = string.Format("{0}_ServiceName", serviceName);
                if (ConfigurationManager.AppSettings[settingName] == null)
                {
                    throw new Exception(string.Format("Service Setting {0} not found", settingName));
                }

                var service = ConfigurationManager.AppSettings[settingName];
                if( ! service.EndsWith("/"))
                {
                    service = service + "/"; // <<--- dont forget the '/' at the end
                }

                return service;
            }
        }

    }
}

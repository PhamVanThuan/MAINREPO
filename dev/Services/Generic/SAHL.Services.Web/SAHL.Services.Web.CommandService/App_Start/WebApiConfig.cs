using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Config.Services;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Web.CommandService
{
    public static class WebApiConfig
    {
        private static readonly DefaultContractResolver Resolver = new DefaultContractResolver
        {
            IgnoreSerializableAttribute = true,
            DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        };

        public static void Register(HttpConfiguration config, IServiceCORSSettings serviceCORSSettings)
        {
            var cors = new EnableCorsAttribute( origins: serviceCORSSettings.AllowedOrigins, 
                                                headers: serviceCORSSettings.AllowedHeaders, 
                                                methods: serviceCORSSettings.AllowedMethods, 
                                                exposedHeaders: serviceCORSSettings.ExposedHeaders); //, exposedHeaders: "CAPITEC-AUTH"

            config.EnableCors(cors);

            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = Resolver;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            config.Formatters.JsonFormatter.SerializerSettings.Error = (error, eventargs) => { };
        }
    }
}
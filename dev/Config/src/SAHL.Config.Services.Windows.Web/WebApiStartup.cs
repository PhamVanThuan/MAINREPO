using System.Web.Mvc;
using System.Web.Http;
using System.Reflection;
using System.Web.Http.Cors;

using Owin;

using StructureMap;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using SAHL.Core.Web.Mvc.Ioc;

namespace SAHL.Config.Services.Windows.Web
{
    public class WebApiStartup
    {
        private static readonly DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                IgnoreSerializableAttribute = true,
                DefaultMembersSearchFlags   = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            };

        public void Configuration(IAppBuilder appBuilder)
        {
            var container                      = ObjectFactory.GetInstance<IContainer>();
            var structureMapDependencyResolver = new StructureMapDependencyResolver(container);

            DependencyResolver.SetResolver(structureMapDependencyResolver);
            GlobalConfiguration.Configuration.DependencyResolver = structureMapDependencyResolver;

            var httpConfiguration = new HttpConfiguration();
            var corsAttribute     = new EnableCorsAttribute("*", "*", "*", "CAPITEC-AUTH");
            httpConfiguration.EnableCors(corsAttribute);

            httpConfiguration.Routes.MapHttpRoute(name: "DefaultApi",
                                                  routeTemplate: "api/{controller}/{id}",
                                                  defaults: new { id = RouteParameter.Optional });

            httpConfiguration.DependencyResolver                                                     = structureMapDependencyResolver;
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling           = TypeNameHandling.All;
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver           = contractResolver;
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling      = ReferenceLoopHandling.Serialize;
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.TypeNameAssemblyFormat     = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.Error                      = (error, eventargs) => { };

            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}

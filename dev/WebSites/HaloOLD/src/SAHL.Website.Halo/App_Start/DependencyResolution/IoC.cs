using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Search;
using StructureMap;
using System.Configuration;

namespace SAHL.Website.Halo.App_Start.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                var haloCommandServiceUrl = ConfigurationManager.AppSettings["HaloCommandServiceUrl"];
                var searchCommandServiceUrl = ConfigurationManager.AppSettings["SearchCommandServiceUrl"];
                x.For<IServiceUrlConfiguration>().Use<ServiceUrlConfiguration>().Ctor<string>("serviceName").Is(haloCommandServiceUrl).Named("HaloService");
                x.For<IServiceUrlConfiguration>().Use<ServiceUrlConfiguration>().Ctor<string>("serviceName").Is(searchCommandServiceUrl).Named("SearchService");

                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });

            });
            return ObjectFactory.Container;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAHL.Config.Services.Query.Server.Scanners;
using SAHL.Core.Identity;
using SAHL.Core.IoC;
using SAHL.Core.Web.Identity;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Models;
using SAHL.Config.Services;
using SAHL.Config.Web.Mvc;
using SAHL.Core.Logging;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Parsers.Elemets;
using StructureMap;

namespace SAHL.Services.Query.Console
{
    public class DummyHttpHostContext : HttpHostContext
    {
        public override string GetApplicationPath()
        {
            return "/QueryService";
        }

        public override Uri GetCurrentRequestUrl()
        {
            return new Uri("http://localhost/" + GetApplicationPath() + "/somewhere/else");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            var dataManagerScanner = new DataManagerScanner("SAHL.Services.Query");

            dataManagerScanner.GetMappings();

            //var bootstrapper = new ServiceBootstrapper();
            //var container = bootstrapper.Initialise();

            //ObjectFactory.Configure(a =>
            //{
            //    a.For<IRawLogger>().Use<NullRawLogger>();

            //    a.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

            //    a.For<IHostContext>().Singleton().Use<DummyHttpHostContext>();
            //});

            //var queryCoordinator = container.GetInstance<IQueryCoordinator>();

            //var query = new FindManyQuery();
            //query.Includes = new List<string>
            //{
            //    "/"
            //};


            //var json = queryCoordinator.Execute(query, () => new AttorneyDataModel
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Test Attorney",
            //});

            //var jsonObject = JToken.Parse(json);
            //var indentedJson = jsonObject.ToString(Formatting.Indented);

            //System.Console.WriteLine(indentedJson);
            //System.Console.ReadKey();
        }
    }
}

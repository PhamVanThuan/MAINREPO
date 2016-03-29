using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAHL.Config.Services;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Query;
using SAHL.Services.Query.Connectors.Attorneys;
using SAHL.Services.Query.Connectors.Finance;
using StructureMap;

namespace SAHL.Services.Query.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            ObjectFactory.Configure(a =>
            {
                a.For<IRawLogger>().Use<NullRawLogger>();

                a.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

                a.For<ILoggerSource>().Use(new LoggerSource("SAHL.Services.Query.Shared.Tests", LogLevel.Error, true));
            });

            
            var instance = container.GetInstance<IQueryServiceClient>();

            var att = container.GetInstance<ThirdPartyInvoice>();

            ThirdPartyInvoice invoice = new ThirdPartyInvoice(instance);
            Attorney attorney = new Attorney(instance);


            dynamic result = FindAttorneys(attorney);
                
                //invoice.ForInvoice("292")
                //.Documents
                //.FindById("32cdc4f6-5312-4df9-9465-64020917a166")
                //.Execute();

            System.Console.Write(JsonConvert.SerializeObject(result, Formatting.Indented));            
            System.Console.ReadKey();

        }

        private static ExpandoObject FindAttorneys(Attorney attorney)
        {


            ExpandoObject result = attorney.Find()
                .IncludeField("Name")
                .OrderBy("Name").Asc()
                .Skip(50)
                .Limit(50)
                .Execute();

            //ExpandoObject result = attorney.Find()
            //    .IncludeField("Name")
            //    .OrderBy("Name").Asc()
            //    .Skip(50)
            //    .Limit(50)
                //.Limit(50).Skip(5)
                //.Limit(4)
                //
                //.IncludeField("Id")
                //.IncludeField("Name")
                //.IncludeRelationship("DeedsOffice")
                //.Where().And().Equals().Field("IsLitigationAttorney").Value("1")
                //.Where().And().Equals().Field("IsPanelAttorney").Value("1")
                //.IncludePaging().SetCurrentPageTo(1).WithPageSize(5)
            //    .Execute();
            return result;
        }

        private static ExpandoObject FindThirdPartyInvoice(ThirdPartyInvoice invoice)
        {
            int id = 7;
            dynamic result = invoice.FindById(id.ToString())
                .IncludeRelationship("Documents")
                .Execute();

            System.Console.WriteLine(result.id);

            return result;
        }

        
    }
}

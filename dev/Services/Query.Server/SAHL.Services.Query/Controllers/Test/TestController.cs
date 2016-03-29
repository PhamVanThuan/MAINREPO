using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Helpers;
using System.Web.Http;
using StructureMap;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.Test
{
    [ServiceGenerationToolExclude]
    public class TestController : ApiController
    {
        private readonly ILinkResolver linkResolver;

        public TestController(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        [RepresentationRoute("/api/test", typeof (TestListRepresentation))]
        public TestListRepresentation Get()
        {
            var value1 = new TestRepresentation(linkResolver);
            value1.Id = 1;
            value1.Value = "ValueTest" + value1.Id;

            var value2 = new TestRepresentation(linkResolver);
            value2.Id = 2;
            value2.Value = "ValueTest" + value2.Id;

            var value3 = new TestRepresentation(linkResolver);
            value3.Id = 3;
            value3.Value = "ValueTest" + value3.Id;

            var items = new List<Representation>
            {
                value1,
                value2,
                value3,
            };

            var result = new TestListRepresentation(linkResolver, items);

            var item = Request.RequestUri.ParseQueryString();
            var includeClause = item.GetValues("include");
            if (includeClause == null || !includeClause.Contains("test"))
            {
                result.Items = null;
            }

            return result;
        }

        [RepresentationRoute("/api/test/{id}", typeof (TestRepresentation))]
        public TestRepresentation Get(int id)
        {
            var queryString = Request.RequestUri.ParseQueryString();
            string[] includes = null;
            var includeClause = queryString["include"];
            if (includeClause != null)
            {
                includes = includeClause.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }

            var value = new TestRepresentation(linkResolver);
            value.Id = id;
            value.Value = "ValueTest" + value.Id;

            Test2Representation test2 = null;
            if (includes != null && includes.Contains("test2", StringComparer.OrdinalIgnoreCase))
            {
                test2 = new Test2Representation(linkResolver);
                test2.Id = 10;
                test2.Type = "TestValue";
                test2.TestId = id;
            }

            if (test2 != null)
            {
                value.List = new List<Representation>();
                value.List.Add(test2);
            }

            return value;
        }
    }

    [ServiceGenerationToolExclude]
    public class Test2Controller : ApiController
    {
        private readonly ILinkResolver linkResolver;

        public Test2Controller(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        [RepresentationRoute("/api/test2/{id}", typeof (Test2Representation))]
        public Test2Representation Get(int id)
        {
            //, id, "TestValue" + id, 1
            var value = new Test2Representation(linkResolver);
            value.Id = id;
            value.Type = "TestValue";
            value.TestId = -1;

            //now we exclude fields that are null
            value.TestId = null; //testing backing field
            return value;
        }
    }
}

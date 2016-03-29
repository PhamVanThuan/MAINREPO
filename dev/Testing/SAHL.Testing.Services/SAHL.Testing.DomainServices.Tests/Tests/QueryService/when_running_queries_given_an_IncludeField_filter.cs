using NUnit.Framework;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.QueryService
{
    [TestFixture]
    public class when_running_queries_given_an_IncludeField_filter : FluentTest
    {
        [Test]
        public void should_only_return_the_specified_field_including_the_id()
        {
            test.Setup<Query>(y =>
            {
                y.Set<string>("url", "/api/attorneys/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                return y.OrderBy("Name")
                      .Asc()
                      .IncludeField("Name")
                      .Limit(50)
                      .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                var _results = (IDictionary<string, object>)results["_embedded"];
                var attorneys = (IList<object>)_results["attorney"];
                ExpandoObject attorneyFields = (ExpandoObject)attorneys[0];

                var idGuid = attorneyFields.Where(z => z.Key == "id").FirstOrDefault();
                Assert.NotNull(idGuid, "Expected id to be included.");

                var id = Guid.Parse(idGuid.Value.ToString());
                Assert.AreNotEqual(id, Guid.Empty, "Expected id to not have an empty guid");

                var hasName = attorneyFields.Where(z => z.Key == "name").Any();
                Assert.True(hasName, "Expected name to be included.");
            });
        }
    }
}
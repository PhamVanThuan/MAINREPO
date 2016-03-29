using NUnit.Framework;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.QueryService
{
    [TestFixture]
    public class when_running_queries_given_an_order_filter : FluentTest
    {
        [Test]
        public void should_sort_the_records()
        {
            test.Setup<Query>(y =>
            {
                y.Set<string>("url", "/api/attorneys/");
            }).Execute<Query, IDictionary<string, object>>(y =>
            {
                return y.OrderBy("Name").Asc()
                        .IncludeField("Name")
                        .Limit(50)
                        .Execute();
            }).Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.NotNull(results);
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var attorneys = (IList<dynamic>)_results2["attorney"];
                CollectionAssert.IsOrdered(attorneys.Select(x => x.name));
            });
        }
    }
}
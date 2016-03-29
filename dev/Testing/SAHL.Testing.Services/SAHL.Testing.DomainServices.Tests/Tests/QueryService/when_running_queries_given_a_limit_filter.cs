using NUnit.Framework;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.QueryService
{
    [TestFixture]
    public class when_running_queries_given_a_limit_filter : FluentTest
    {
        [Test]
        public void should_return_fifty_records()
        {
            test.Setup<Query>(y =>
            {
                y.Set<string>("url", "/api/attorneys/");
            }).Execute<Query, IDictionary<string, object>>(y =>
            {
                return y.OrderBy("Name")
                        .Asc()
                        .Limit(50)
                        .Execute();
            }).Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.NotNull(results);
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var attorneys = (IList<object>)_results2["attorney"];
                Assert.AreEqual(50, attorneys.Count());
            });
        }
    }
}
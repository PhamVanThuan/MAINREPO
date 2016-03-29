using NUnit.Framework;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System.Collections.Generic;

namespace SAHL.Testing.Services.Tests.QueryService
{
    public class when_running_queries_given_paging_filters : FluentTest
    {
        [Test]
        public void should_page_the_records()
        {
            test.Setup<Query>(y =>
                {
                    y.Set<string>("url", "/api/attorneys/");
                }).Execute<Query, dynamic>(y =>
                {
                    return y.IncludePaging()
                            .SetCurrentPageTo(1)
                            .WithPageSize(10)
                            .Execute();
                }).Assert<Query, dynamic>((query, results) =>
                 {
                     CollectionAssert.AllItemsAreUnique(results);
                     CollectionAssert.IsNotEmpty(results);

                     var pageNumber = ((IDictionary<string, object>)results)["_paging"];
                     var currentPage = ((IDictionary<string, object>)pageNumber)["currentPage"];
                     Assert.AreEqual(1, currentPage);

                     var embeddedResults = ((IDictionary<string, object>)results)["_embedded"];
                     var attorneys = ((IDictionary<string, object>)embeddedResults)["attorney"];
                     var actualPageSize = ((IList<object>)attorneys).Count;
                     Assert.AreEqual(10, actualPageSize);
                 });
        }
    }
}
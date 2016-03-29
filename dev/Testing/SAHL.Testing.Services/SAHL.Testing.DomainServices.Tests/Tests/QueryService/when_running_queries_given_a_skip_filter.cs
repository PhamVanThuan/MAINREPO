using NUnit.Framework;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System.Collections.Generic;
using System.Dynamic;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Testing.Services.Tests.QueryService
{
    [TestFixture]
    public class when_running_queries_given_a_Skip_filter : FluentTest
    {
        [Test]
        public void should_return_the_next_50_records()
        {
            var attorneyList = TestApiClient.Get<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
            var sortedList = attorneyList.OrderByDescending(x => x.GenericKey).Skip(50).Take(50).ToList();
            var expectedAttorney = sortedList.First();
            test.Setup<Query>(y =>
            {
                y.Set<string>("url", "/api/attorneys/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                return y.OrderBy("AttorneyKey")
                      .Desc()
                      .IncludeField("Name")
                      .Skip(50)
                      .Limit(50)
                      .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                var _results = (IDictionary<string, object>)results["_embedded"];
                var attorneys = (IList<object>)_results["attorney"];
                dynamic attorneyFields = (ExpandoObject)attorneys[0];
                Assert.AreEqual(expectedAttorney.Id.ToString(), attorneyFields.id, string.Format("Expected: {0}, Actual {1}", expectedAttorney.Id.ToString(), attorneyFields.id));
            });
        }
    }
}
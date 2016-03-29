using NUnit.Framework;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System.Collections.Generic;
using SAHL.Core.Testing;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Testing.Services.Tests.QueryService
{
    [TestFixture]
    public class when_running_queries_given_an_IncludeRelationship_filter : FluentTest
    {
        [Test]
        public void should_return_related_tables()
        {
            var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
            string attorneyGuid = attorney.Id.ToString(); 
            test.Setup<QueryWithIncludes>(y =>
            {
                y.Set<string>("url", string.Format("/api/attorneys/{0}", attorneyGuid));
            })
            .Execute<QueryWithIncludes, dynamic>(y =>
            {
                return y.IncludeRelationship("DeedsOffice")
                        .IncludeRelationship("GeneralStatus")
                        .Execute();
            })
            .Assert<QueryWithIncludes, dynamic>((query, results) =>
            {
                var res = (IDictionary<string, object>)results;
                if (res.ContainsKey("_embedded"))
                {
                    var value = results._embedded.deedsOffice;
                    var value2 = results._embedded.generalStatus;
                }
            });
        }

        [Test]
        public void should_return_no_related_tables()
        {
            var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
            string attorneyGuid = attorney.Id.ToString();
            test.Setup<QueryWithIncludes>(y =>
            {
                y.Set<string>("url", string.Format("/api/attorneys/{0}", attorneyGuid));
            })
            .Execute<QueryWithIncludes, dynamic>(y =>
            {
                return y.Execute();
            })
            .Assert<QueryWithIncludes, dynamic>((query, results) =>
            {
                var res = (IDictionary<string, object>)results;
                Assert.False(res.ContainsKey("_embedded"));
            });
        }
    }
}
using NUnit.Framework;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_checking_if_a_client_exists : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            var applicant = query.Result.Results.FirstOrDefault();
            int clientKey = applicant.LegalEntityKey;
            DoesClientExistQuery doesClientExistQuery = new DoesClientExistQuery(clientKey);
            base.Execute<DoesClientExistQuery>(doesClientExistQuery);
            Assert.That(doesClientExistQuery.Result.Results.First().ClientExists == true);
        }

        [Test]
        public void when_unsuccessful()
        {
            int clientKey = Int32.MaxValue;
            DoesClientExistQuery query = new DoesClientExistQuery(clientKey);
            base.Execute<DoesClientExistQuery>(query);
            Assert.That(query.Result.Results.First().ClientExists == false);
        }
    }
}
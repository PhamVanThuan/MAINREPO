using NUnit.Framework;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_querying_for_a_client : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetNaturalPersonClientQuery getNaturalPersonClientQuery = new GetNaturalPersonClientQuery(true);
            base.PerformQuery(getNaturalPersonClientQuery);
            var client = getNaturalPersonClientQuery.Result.Results.First();
            GetClientDetailsQuery query = new GetClientDetailsQuery(client.LegalEntityKey);
            base.Execute<GetClientDetailsQuery>(query);
            Assert.That(query.Result.Results.First().IDNumber.Equals(client.IDNumber));
        }

        [Test]
        public void when_unsuccessful()
        {
            GetClientDetailsQuery query = new GetClientDetailsQuery(Int32.MaxValue);
            base.Execute<GetClientDetailsQuery>(query);
            Assert.That(query.Result.Results.Any() == false);
            Assert.That(query.Result != null);
        }
    }
}
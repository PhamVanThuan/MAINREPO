using NUnit.Framework;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_querying_for_a_client_address : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var applicantQuery = new GetNewBusinessApplicantQuery(true, true, false, false, true, true, false, true, false);
            base.PerformQuery(applicantQuery);
            var applicant = applicantQuery.Result.Results.First();
            var getClientAddressesQuery = new GetClientAddressesQuery(applicant.LegalEntityKey);
            base.PerformQuery(getClientAddressesQuery);
            var address = getClientAddressesQuery.Result.Results.First();
            var query = new GetClientAddressQuery(address.LegalEntityAddressKey);
            base.Execute<GetClientAddressQuery>(query);
            var clientAddressExists = query.Result.Results.Where(x => x.ClientKey == applicant.LegalEntityKey).First();
            Assert.IsNotNull(clientAddressExists);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetClientAddressQuery(Int32.MaxValue);
            base.Execute<GetClientAddressQuery>(query);
            Assert.That(query.Result.Results.Count().Equals(0));
        }
    }
}
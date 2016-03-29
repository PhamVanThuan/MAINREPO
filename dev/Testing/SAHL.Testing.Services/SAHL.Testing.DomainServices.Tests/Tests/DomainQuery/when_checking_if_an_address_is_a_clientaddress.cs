using NUnit.Framework;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_checking_if_an_address_is_a_clientaddress : ServiceTestBase<IDomainQueryServiceClient>
    {
        private GetApplicantWithApplicationCriteriaQueryResult _queryResult;

        [SetUp]
        public void OnSetup()
        {
            GetApplicantWithApplicationCriteriaQuery appQuery = new GetApplicantWithApplicationCriteriaQuery(true, false);
            base.PerformQuery(appQuery);
            _queryResult = appQuery.Result.Results.First();
        }

        [Test]
        public void when_successful()
        {
            var getClientAddressesQuery = new GetClientAddressesQuery(_queryResult.LegalEntityKey);
            base.PerformQuery(getClientAddressesQuery);
            var applicantAddresses = getClientAddressesQuery.Result.Results;
            IsAddressAClientAddressQuery query = new IsAddressAClientAddressQuery(applicantAddresses.First().AddressKey, _queryResult.LegalEntityKey);
            base.Execute<IsAddressAClientAddressQuery>(query);
            Assert.That(query.Result.Results.First().AddressIsAClientAddress == true);
        }

        [Test]
        public void when_unsuccessful()
        {
            IsAddressAClientAddressQuery query = new IsAddressAClientAddressQuery(Int32.MaxValue, _queryResult.LegalEntityKey);
            base.Execute<IsAddressAClientAddressQuery>(query);
            Assert.That(query.Result.Results.First().AddressIsAClientAddress == false);
        }
    }
}
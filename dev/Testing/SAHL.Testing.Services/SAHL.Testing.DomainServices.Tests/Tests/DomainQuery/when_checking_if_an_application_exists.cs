using NUnit.Framework;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_checking_if_an_application_exists : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            DoesOfferExistQuery doesOfferExistQuery = new DoesOfferExistQuery(applicationNumber);
            base.Execute<DoesOfferExistQuery>(doesOfferExistQuery);
            Assert.That(doesOfferExistQuery.Result.Results.First().OfferExist == true);
        }

        [Test]
        public void when_unsuccessful()
        {
            int applicationNumber = Int32.MaxValue;
            DoesOfferExistQuery query = new DoesOfferExistQuery(applicationNumber);
            base.Execute<DoesOfferExistQuery>(query);
            Assert.That(query.Result.Results.First().OfferExist == false);
        }
    }
}
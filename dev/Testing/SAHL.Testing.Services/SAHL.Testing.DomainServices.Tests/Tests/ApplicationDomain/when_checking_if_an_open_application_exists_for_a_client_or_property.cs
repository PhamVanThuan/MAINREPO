using NUnit.Framework;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_checking_if_an_open_application_exists_for_a_client_or_property : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var getOpenApplicationQuery = new GetOpenApplicationPropertyAndClientQuery();
            base.PerformQuery(getOpenApplicationQuery);
            var openApplication = getOpenApplicationQuery.Result.Results.First();
            DoesOpenApplicationExistForPropertyAndClientQuery query = new DoesOpenApplicationExistForPropertyAndClientQuery(openApplication.PropertyKey, openApplication.IDNumber);
            base.Execute<DoesOpenApplicationExistForPropertyAndClientQuery>(query);
            Assert.That(query.Result.Results.First() == true);
        }

        [Test]
        public void when_unsuccessful()
        {
            DoesOpenApplicationExistForPropertyAndClientQuery query = new DoesOpenApplicationExistForPropertyAndClientQuery(Int32.MaxValue, "8211045229080");
            base.Execute<DoesOpenApplicationExistForPropertyAndClientQuery>(query);
            Assert.That(query.Result.Results.First() == false);
        }
    }
}
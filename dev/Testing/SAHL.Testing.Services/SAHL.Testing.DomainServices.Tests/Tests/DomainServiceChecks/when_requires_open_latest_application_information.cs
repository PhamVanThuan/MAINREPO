using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainServiceChecks
{
    public class when_requires_open_latest_application_information : ServiceTestBase<IFinancialDomainServiceClient>
    {
        [Test]
        public void when_unsuccessful()
        {
            var application = TestApiClient.Get<OfferDataModel>(new { offerstatuskey = (int)OfferStatus.Accepted }, limit: 1).First();
            var command = new FundNewBusinessApplicationCommand(application.OfferKey);
            base.Execute<FundNewBusinessApplicationCommand>(command)
                .AndExpectThatErrorMessagesContain("The latest application information for your application is not open.");
        }
    }
}
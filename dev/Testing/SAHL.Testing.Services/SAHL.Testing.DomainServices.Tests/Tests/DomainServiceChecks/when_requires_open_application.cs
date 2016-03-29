using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainServiceChecks
{
    internal class when_requires_open_application : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_unsuccessful()
        {
            var application = TestApiClient.Get<OfferDataModel>(new { offerstatuskey = (int)OfferStatus.Accepted }, limit: 1).First();
            var command = new LinkExternalVendorToApplicationCommand(application.OfferKey, OriginationSource.Comcorp, "vendorcodeabc");
            base.Execute<LinkExternalVendorToApplicationCommand>(command)
                .AndExpectThatErrorMessagesContain("No open application could be found against your application number.");
        }
    }
}
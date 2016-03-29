using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_adding_the_external_originator_attribute : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            var command = new SetExternalOriginatorAttributeCommand(applicationNumber, OriginationSource.Comcorp);
            base.Execute<SetExternalOriginatorAttributeCommand>(command);
            var applicationAttributes = TestApiClient.Get<OfferAttributeDataModel>(new { offerkey = applicationNumber });
            Assert.That(applicationAttributes.Where(x => x.OfferAttributeTypeKey == (int)OfferAttributeType.ComcorpLoan).First() != null,
                "Origination Attribute was not added.");
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            var command = new SetExternalOriginatorAttributeCommand(applicationNumber, OriginationSource.SAHomeLoans);
            base.Execute<SetExternalOriginatorAttributeCommand>(command)
                .AndExpectThatErrorMessagesContain("The originator attribute can only be set for Comcorp or Capitec originated applications");
        }
    }
}
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_linking_an_external_vendor : ServiceTestBase<IApplicationDomainServiceClient>
    {
        private GetRandomOpenApplicationQueryResult _application;
        private VendorDataModel _vendor;

        [SetUp]
        public void OnSetup()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            _application = query.Result.Results.First();
        }

        [TearDown]
        public void OnTeardown()
        {
            _application = null;
            _vendor = null;
        }

        [Test]
        public void when_successful()
        {
            RemoveExistingVendorIfExists();
            var command = new LinkExternalVendorToApplicationCommand(_application.ApplicationNumber, OriginationSource.Comcorp, _vendor.VendorCode);
            base.Execute<LinkExternalVendorToApplicationCommand>(command);
            var applicationRoles = TestApiClient.Get<OfferRoleDataModel>(new { offerkey = _application.ApplicationNumber, generalstatuskey = (int)GeneralStatus.Active });
            Assert.That(applicationRoles.Where(x => x.OfferRoleTypeKey == (int)OfferRoleType.ExternalVendor
                && x.LegalEntityKey == _vendor.LegalEntityKey
                && x.GeneralStatusKey == 1).First() != null);
        }

        private void RemoveExistingVendorIfExists()
        {
            var roles = TestApiClient.Get<OfferRoleDataModel>(new { offerkey = _application.ApplicationNumber });
            var vendorRole = roles.Where(x => x.OfferRoleTypeKey == (int)OfferRoleType.ExternalVendor).FirstOrDefault();
            if (vendorRole != null)
            {
                var command = new RemoveApplicationRoleFromApplicationCommand(vendorRole.OfferRoleKey);
                base.PerformCommand(command);
            }
            var vendors = TestApiClient.Get<VendorDataModel>(new { generalstatuskey = 1 });
            _vendor = vendors.First();
        }

        [Test]
        public void when_unsuccessful()
        {
            var vendors = TestApiClient.Get<VendorDataModel>(new { generalstatuskey = 1 });
            _vendor = vendors.First();
            var command = new LinkExternalVendorToApplicationCommand(_application.ApplicationNumber, OriginationSource.SAHomeLoans, _vendor.VendorCode);
            base.Execute<LinkExternalVendorToApplicationCommand>(command)
                .AndExpectThatErrorMessagesContain("Origination source must be Comcorp.");
        }
    }
}
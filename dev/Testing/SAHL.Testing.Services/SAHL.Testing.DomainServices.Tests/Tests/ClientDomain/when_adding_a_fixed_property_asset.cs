using NUnit.Framework;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_a_fixed_property_asset : ServiceTestBase<IClientDomainServiceClient>
    {
        private GetApplicantWithoutAssetsOrLiabilitiesQueryResult _applicant;
        private IEnumerable<GetClientAddressesQueryResult> _addresses;
        private int _addressKey;

        [SetUp]
        public void SetUp()
        {
            var query = new GetApplicantWithoutAssetsOrLiabilitiesQuery();
            base.PerformQuery(query);
            _applicant = query.Result.Results.FirstOrDefault();
            var getAddressesQuery = new GetClientAddressesQuery(_applicant.LegalEntityKey);
            base.PerformQuery(getAddressesQuery);
            _addresses = getAddressesQuery.Result.Results;
            _addressKey = _addresses.First().AddressKey;
        }

        [TearDown]
        public void TearDown()
        {
            if (_applicant != null)
            {
                var command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(_applicant.OfferRoleKey);
                base.PerformCommand(command);
                _applicant = null;
            }
            _addresses = null;
            _addressKey = 0;
        }

        [Test]
        public void when_successful()
        {
            var model = new FixedPropertyAssetModel(DateTime.Now.AddYears(-2), _addressKey, 1500000D, 200000D);
            var command = new AddFixedPropertyAssetToClientCommand(_applicant.LegalEntityKey, model);
            base.Execute<AddFixedPropertyAssetToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(command.ClientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssets = clientAssetQuery.Result.Results;
            Assert.That(clientAssets.Where(x => x.AssetTypeDescription == "Fixed Property"
                && x.LiabilityValue == model.LiabilityValue
                && x.AddressKey == model.AddressKey
                && x.AssetValue == model.AssetValue
                && x.LiabilityValue == model.LiabilityValue).First() != null, "Fixed Property not added correctly");
        }

        [Test]
        public void when_unsuccessful()
        {
            //set acquisition date in the future
            var model = new FixedPropertyAssetModel(DateTime.Now.AddYears(+2), _addressKey, 1500000D, 200000D);
            var command = new AddFixedPropertyAssetToClientCommand(_applicant.LegalEntityKey, model);
            base.Execute<AddFixedPropertyAssetToClientCommand>(command)
                .AndExpectThatErrorMessagesContain("The acquisition date for a fixed property asset cannot be in the future.");
        }
    }
}
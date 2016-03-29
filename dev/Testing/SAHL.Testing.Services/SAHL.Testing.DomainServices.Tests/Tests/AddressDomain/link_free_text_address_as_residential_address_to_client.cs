using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.AddressDomain
{
    [TestFixture]
    public class link_free_text_address_as_residential_address_to_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        private GetApplicantOnActiveApplicationQueryResult _clientRole;

        [SetUp]
        public void OnSetup()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            _clientRole = query.Result.Results.FirstOrDefault();
        }

        [TearDown]
        public void OnTeardown()
        {
            _clientRole = null;
        }

        [Test]
        public void when_successful()
        {
            var freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby way", "Fairfield", "Sydney", "Australia", CombGuid.Instance.GenerateString(), "Australia");
            var command = new LinkFreeTextAddressAsResidentialAddressToClientCommand(freeTextAddressModel, _clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkFreeTextAddressAsResidentialAddressToClientCommand>(command);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var query = new GetClientAddressesQuery(_clientRole.LegalEntityKey);
            base.PerformQuery(query);
            IEnumerable<GetClientAddressesQueryResult> clientAddresses = query.Result.Results;
            Assert.IsNotNull(clientAddresses.First(x => x.LegalEntityAddressKey == linkedKey));
        }

        [Test]
        public void when_unsuccessful()
        {
            var addressCountBefore = GetAddressCount(_clientRole.LegalEntityKey);
            var freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby way", "Fairfield", "Sydney", "Australia", CombGuid.Instance.GenerateString(), "Langkawi");
            var command = new LinkFreeTextAddressAsResidentialAddressToClientCommand(freeTextAddressModel, _clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkFreeTextAddressAsResidentialAddressToClientCommand>(command)
                .AndExpectThatErrorMessagesContain(string.Format("Post Office not found for country : {0}", freeTextAddressModel.Country));
            var addressCountAfter = GetAddressCount(_clientRole.LegalEntityKey);
            Assert.AreEqual(addressCountAfter, addressCountBefore);
        }

        private int GetAddressCount(int clientKey)
        {
            var getClientAddressesQuery = new GetClientAddressesQuery(clientKey);
            base.PerformQuery(getClientAddressesQuery);
            var addressCount = getClientAddressesQuery.Result.Results.Count();
            return addressCount;
        }
    }
}
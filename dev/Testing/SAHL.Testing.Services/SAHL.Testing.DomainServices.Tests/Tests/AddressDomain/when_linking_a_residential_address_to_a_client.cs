using NUnit.Framework;
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
    public class when_linking_a_residential_address_to_a_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        private GetApplicantOnActiveApplicationQueryResult _clientRole;
        private string _streetNumber;

        [SetUp]
        public void OnSetup()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            _clientRole = query.Result.Results.FirstOrDefault();
            _streetNumber = this.randomizer.Next(1, 500).ToString();
        }

        [TearDown]
        public void OnTeardown()
        {
            _clientRole = null;
        }

        [Test]
        public void when_successful()
        {
            var streetAddressModel = new StreetAddressModel(string.Empty, string.Empty, string.Empty, _streetNumber, "Smith Street", "Durban", "Durban", "Kwazulu-Natal", "4001");
            var command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddressModel, _clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkStreetAddressAsResidentialAddressToClientCommand>(command);
            this.linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var query = new GetClientAddressesQuery(_clientRole.LegalEntityKey);
            base.PerformQuery(query);
            IEnumerable<GetClientAddressesQueryResult> clientAddresses = query.Result.Results;
            Assert.IsNotNull(clientAddresses.Where(x => x.LegalEntityAddressKey == linkedKey).First());
        }

        [Test]
        public void when_unsuccesful()
        {
            var streetAddressModel = new StreetAddressModel(string.Empty, string.Empty, string.Empty, _streetNumber, "Smith Street", CombGuid.Instance.GenerateString(), "Durban", "Kwazulu-Natal", "4001");
            var command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddressModel, _clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkStreetAddressAsResidentialAddressToClientCommand>(command)
                .AndExpectThatErrorMessagesContain(string.Format("No Suburb could be found for - Suburb: {0}; City: {1}; Province: {2}; PostalCode: {3}", streetAddressModel.Suburb, streetAddressModel.City,
                streetAddressModel.Province, streetAddressModel.PostalCode));
        }
    }
}
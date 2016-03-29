using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.AddressDomain
{
    [TestFixture]
    public class when_linking_a_postal_address_to_a_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        private GetApplicantOnActiveApplicationQueryResult _clientRole;
        private string _boxNumber;

        [SetUp]
        public void SetUp()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            _clientRole = query.Result.Results.FirstOrDefault();
            _boxNumber = this.randomizer.Next(0, Int32.MaxValue).ToString();
        }

        [Test]
        public void when_successful()
        {
            var postalAddress = new PostalAddressModel(_boxNumber, string.Empty, "Wandsbeck", "Kwazulu-natal", "Westville", "3631", AddressFormat.Box);
            var command = new LinkPostalAddressToClientCommand(postalAddress, _clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkPostalAddressToClientCommand>(command);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var query = new GetClientAddressesQuery(_clientRole.LegalEntityKey);
            base.PerformQuery(query);
            IEnumerable<GetClientAddressesQueryResult> clientAddresses = query.Result.Results;
            Assert.IsNotNull(clientAddresses.Where(x => x.LegalEntityAddressKey == linkedKey).First());
        }

        [Test]
        public void when_unsuccessful()
        {
            var postalAddress = new PostalAddressModel(_boxNumber, string.Empty, "Wandsbeck", "Kwazulu-natal", "Gennadi", "3631", AddressFormat.Box);
            var command = new LinkPostalAddressToClientCommand(postalAddress, _clientRole.LegalEntityKey, CombGuid.Instance.Generate());
            base.Execute<LinkPostalAddressToClientCommand>(command)
                .AndExpectThatErrorMessagesContain(string.Format(@"No Post Office could be found for - City: {0}; Province: {1}; PostalCode: {2}", postalAddress.City,
                postalAddress.Province, postalAddress.PostalCode));
        }
    }
}
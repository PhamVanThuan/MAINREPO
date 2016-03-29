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
    public class link_street_address_as_postal_address_to_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            var clientRole = query.Result.Results.FirstOrDefault();
            var streetAddressModel = new StreetAddressModel("", "", "", "12", CombGuid.Instance.GenerateString(), "Durban", "Durban", "Kwazulu-Natal", "4001");
            var command = new LinkStreetAddressAsPostalAddressToClientCommand(streetAddressModel, clientRole.LegalEntityKey, linkedGuid);
            base.Execute<LinkStreetAddressAsPostalAddressToClientCommand>(command);
            this.linkedKey = linkedKeyManager.RetrieveLinkedKey(linkedGuid);
            var getClientAddressesQuery = new GetClientAddressesQuery(clientRole.LegalEntityKey);
            base.PerformQuery(getClientAddressesQuery);
            IEnumerable<GetClientAddressesQueryResult> clientAddresses = getClientAddressesQuery.Result.Results;
            Assert.IsNotNull(clientAddresses.Where(x => x.LegalEntityAddressKey == linkedKey).First());
        }
    }
}
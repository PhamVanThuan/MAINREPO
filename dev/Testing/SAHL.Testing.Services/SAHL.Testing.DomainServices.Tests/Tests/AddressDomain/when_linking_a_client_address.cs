using NUnit.Framework;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.AddressDomain
{
    [TestFixture]
    public class when_linking_a_client_address : ServiceTestBase<IAddressDomainServiceClient>
    {
        [Test]
        public void and_the_address_exists()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            var clientRole = query.Result.Results.FirstOrDefault();
            string streetNumber = this.randomizer.Next(1, Int32.MaxValue).ToString();
            var streetAddressModel = new StreetAddressModel(string.Empty, string.Empty, string.Empty, streetNumber, "Smith Street", "Durban", "Durban", "Kwazulu-Natal", "4001");
            var command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddressModel, clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkStreetAddressAsResidentialAddressToClientCommand>(command);
            int linkedKey = this.linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            //get the address
            var getClientAddressesQuery = new GetClientAddressesQuery(clientRole.LegalEntityKey);
            base.PerformQuery(getClientAddressesQuery);
            var clientAddress = getClientAddressesQuery.Result.Results.Where(x => x.LegalEntityAddressKey == linkedKey).First();
            //execute it again, expecting messages
            base.Execute<LinkStreetAddressAsResidentialAddressToClientCommand>(command)
                .AndExpectThatErrorMessagesContain(string.Format(@"An active Residential address for ClientKey: {0} and AddressKey: {1} already exists.", clientAddress.LegalEntityKey, clientAddress.AddressKey));
        }
    }
}
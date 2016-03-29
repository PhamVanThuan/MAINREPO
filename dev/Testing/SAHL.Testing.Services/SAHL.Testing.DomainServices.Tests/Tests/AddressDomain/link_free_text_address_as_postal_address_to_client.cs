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
    public class link_free_text_address_as_postal_address_to_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            var applicant = query.Result.Results.FirstOrDefault();
            var freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby way", "Fairfield", "Sydney", "Australia", CombGuid.Instance.GenerateString(), "Australia");
            var command = new LinkFreeTextAddressAsPostalAddressToClientCommand(freeTextAddressModel, applicant.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkFreeTextAddressAsPostalAddressToClientCommand>(command);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var getClientAddressesQuery = new GetClientAddressesQuery(applicant.LegalEntityKey);
            base.PerformQuery(getClientAddressesQuery);
            IEnumerable<GetClientAddressesQueryResult> clientAddresses = getClientAddressesQuery.Result.Results;
            Assert.IsNotNull(clientAddresses.First(x => x.LegalEntityAddressKey == linkedKey && x.AddressTypeKey == (int)AddressType.Postal));
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            var applicant = query.Result.Results.FirstOrDefault();
            var addressCountBefore = GetAddressCount(applicant.LegalEntityKey);
            var freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby way", "Fairfield", "Sydney", "Australia", CombGuid.Instance.GenerateString(), "Langkawi");
            var command = new LinkFreeTextAddressAsPostalAddressToClientCommand(freeTextAddressModel, applicant.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkFreeTextAddressAsPostalAddressToClientCommand>(command)
                .AndExpectThatErrorMessagesContain(string.Format("Post Office not found for country : {0}", freeTextAddressModel.Country));
            var addressCountAfter = GetAddressCount(applicant.LegalEntityKey);
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
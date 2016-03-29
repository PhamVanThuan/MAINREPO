using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.AddressDomain
{
    [TestFixture]
    public class when_getting_a_free_text_address_is_linked_to_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        private LegalEntityAddressDataModel clientAddress;

        [TearDown]
        public void OnTestTeardown()
        {
            if (clientAddress != null)
            {
                var command = new SetClientAddressToInactiveCommand(clientAddress.AddressKey);
                base.PerformCommand(command);
                clientAddress = null;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_address_is_linked()
        {
            var clientAddressQuery = new GetActiveClientAddressByAddressFormatQuery((int)AddressFormat.FreeText);
            base.PerformQuery(clientAddressQuery);
            clientAddress = clientAddressQuery.Result.Results.First();
            var address = TestApiClient.GetByKey<AddressDataModel>(clientAddress.AddressKey);
            var addressFormat = (AddressFormat)address.AddressFormatKey;       
            var freeTextAddressModel = new FreeTextAddressModel(addressFormat, address.FreeText1, address.FreeText2, address.FreeText3, address.FreeText4, address.FreeText5,
                address.RRR_CountryDescription);
            GetClientFreeTextAddressQuery query = new GetClientFreeTextAddressQuery(clientAddress.LegalEntityKey, freeTextAddressModel, Core.BusinessModel.Enums.AddressType.Residential);
            base.Execute<GetClientFreeTextAddressQuery>(query);
            Assert.That(query.Result.Results.FirstOrDefault().AddressKey == clientAddress.AddressKey);
        }

        [Test]
        public void when_address_is_not_linked()
        {
            var freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "Not In our system", "Fairfield", "Sydney", "Australia", CombGuid.Instance.GenerateString(), "Langkawi");
            GetApplicantWithApplicationCriteriaQuery appQuery = new GetApplicantWithApplicationCriteriaQuery(true, false);
            base.PerformQuery(appQuery);
            var applicant = appQuery.Result.Results.First();
            GetClientFreeTextAddressQuery query = new GetClientFreeTextAddressQuery(Int32.MaxValue, freeTextAddressModel, Core.BusinessModel.Enums.AddressType.Residential);
            base.Execute<GetClientFreeTextAddressQuery>(query);
            Assert.That(query.Result.Results.First() == null);
        }
    }
}
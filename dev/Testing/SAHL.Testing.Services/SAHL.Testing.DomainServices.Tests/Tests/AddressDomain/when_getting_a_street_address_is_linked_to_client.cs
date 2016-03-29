using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.AddressDomain
{
    [TestFixture]
    public class when_getting_a_street_address_is_linked_to_client : ServiceTestBase<IAddressDomainServiceClient>
    {
        private LegalEntityAddressDataModel clientAddress;

        [Test]
        public void when_successful()
        {
            var clientAddressQuery = new GetActiveClientAddressByAddressFormatQuery((int)AddressFormat.Street);
            base.PerformQuery(clientAddressQuery);
            clientAddress = clientAddressQuery.Result.Results.First();
            var address = TestApiClient.GetByKey<AddressDataModel>(clientAddress.AddressKey);
            var streetAddressModel = new StreetAddressModel(address.UnitNumber, address.BuildingName, address.BuildingNumber, address.StreetNumber, address.StreetName, address.RRR_SuburbDescription,
                address.RRR_CityDescription, address.RRR_ProvinceDescription, address.RRR_PostalCode);
            AddressType addressTypeEnum = (AddressType)clientAddress.AddressTypeKey;
            var query = new GetClientStreetAddressQuery(clientAddress.LegalEntityKey, streetAddressModel, addressTypeEnum);
            base.Execute<GetClientStreetAddressQuery>(query);
            Assert.That(query.Result.Results.FirstOrDefault().AddressKey == clientAddress.AddressKey,
                string.Format("Query failed for: LegalEntityKey: {0}, StreetAddress: {1}", clientAddress.LegalEntityKey, clientAddress.AddressKey));
        }

        [Test]
        public void when_unsuccessful()
        {
            var streetAddressModel = new StreetAddressModel("Not in the system", "", "", "12", CombGuid.Instance.GenerateString(), "Durban", "Durban", "Kwazulu-Natal", "4001");
            GetApplicantWithApplicationCriteriaQuery appQuery = new GetApplicantWithApplicationCriteriaQuery(true, false);
            base.PerformQuery(appQuery);
            var applicant = appQuery.Result.Results.First();
            var query = new GetClientStreetAddressQuery(Int32.MaxValue, streetAddressModel, Core.BusinessModel.Enums.AddressType.Residential);
            base.Execute<GetClientStreetAddressQuery>(query);
            Assert.That(query.Result.Results.FirstOrDefault() == null);
        }
    }
}
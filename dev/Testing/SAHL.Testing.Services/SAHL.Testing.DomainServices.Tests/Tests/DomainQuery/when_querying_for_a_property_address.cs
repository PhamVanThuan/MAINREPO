using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_querying_for_a_property_address : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetOpenNewBusinessApplicationQuery query = new GetOpenNewBusinessApplicationQuery(hasDebitOrder: false, hasMailingAddress: false, hasProperty: true, isAccepted: false,
                householdIncome: 0);
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().OfferKey;
            var offerMortgageLoan = TestApiClient.GetByKey<OfferMortgageLoanDataModel>(applicationNumber);
            var property = TestApiClient.GetByKey<PropertyDataModel>(offerMortgageLoan.PropertyKey.Value);
            int addressKey = property.AddressKey.HasValue ? property.AddressKey.Value : 0;
            var address = TestApiClient.GetByKey<AddressDataModel>(addressKey);
            var getPropertyByAddressQuery = new GetPropertyByAddressQuery(address.UnitNumber, address.BuildingName, address.BuildingNumber, address.StreetNumber,
                address.StreetName, address.RRR_SuburbDescription, address.RRR_CityDescription, address.RRR_ProvinceDescription, address.RRR_PostalCode,
                property.ErfNumber, property.ErfPortionNumber);
            base.Execute<GetPropertyByAddressQuery>(getPropertyByAddressQuery).WithoutErrors();
            var propertyExists = getPropertyByAddressQuery.Result.Results.Where(x => x.PropertyKey == property.PropertyKey).First();
            Assert.IsNotNull(propertyExists);
        }

        [Test]
        public void when_unsuccessful()
        {
            int addressKey = 822670;
            var address = TestApiClient.GetByKey<AddressDataModel>(addressKey);
            var query = new GetPropertyByAddressQuery(address.UnitNumber, address.BuildingName, address.BuildingNumber, address.StreetNumber,
                address.StreetName, address.RRR_SuburbDescription, address.RRR_CityDescription, address.RRR_ProvinceDescription, address.RRR_PostalCode,
                CombGuid.Instance.GenerateString(), "0");
            base.Execute<GetPropertyByAddressQuery>(query);
            Assert.IsEmpty(query.Result.Results);
        }
    }
}
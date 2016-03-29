using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.PropertyDomain.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.PropertyDomain
{
    [TestFixture]
    public class when_querying_for_a_property_address : ServiceTestBase<IPropertyDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = 1781233;//query.Result.Results.FirstOrDefault().ApplicationNumber;
            var offerMortgageLoan = TestApiClient.GetByKey<OfferMortgageLoanDataModel>(applicationNumber);
            var property = TestApiClient.GetByKey<PropertyDataModel>(offerMortgageLoan.PropertyKey.Value);
            int addressKey = property.AddressKey.HasValue ? property.AddressKey.Value : 0;
            var address = TestApiClient.GetByKey<AddressDataModel>(addressKey);
            GetPropertyByAddressQuery getPropertyByAddressQuery = new GetPropertyByAddressQuery(address.UnitNumber, address.BuildingName, address.BuildingNumber, address.StreetNumber,
                address.StreetName, address.RRR_SuburbDescription, address.RRR_CityDescription, address.RRR_ProvinceDescription, address.RRR_PostalCode,
                property.ErfNumber, property.ErfPortionNumber);
            base.Execute<GetPropertyByAddressQuery>(getPropertyByAddressQuery).WithoutErrors();
            var propertyExists = getPropertyByAddressQuery.Result.Results.Where(x => x.PropertyKey == property.PropertyKey).FirstOrDefault();
            Assert.IsNotNull(propertyExists, string.Format(@"Failed: addressKey={0}, applicationNumber={1}", addressKey, applicationNumber));
            Assert.IsNotNull(propertyExists.PropertyKey, string.Format(@"Failed: addressKey={0}", addressKey));
            Assert.AreEqual(propertyExists.PropertyKey, property.PropertyKey);
        }

        [Test]
        public void when_unsuccessful()
        {
            int addressKey = 822670;
            var address = TestApiClient.GetByKey<AddressDataModel>(addressKey);
            GetPropertyByAddressQuery query = new GetPropertyByAddressQuery(address.UnitNumber, "not in the system", address.BuildingNumber, address.StreetNumber,
                address.StreetName, address.RRR_SuburbDescription, address.RRR_CityDescription, address.RRR_ProvinceDescription, address.RRR_PostalCode,
                CombGuid.Instance.GenerateString(), "0");
            base.Execute<GetPropertyByAddressQuery>(query);
            Assert.IsEmpty(query.Result.Results);
        }
    }
}
using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.AddressDomain
{
    [TestFixture]
    public class when_linking_a_street_address_to_a_property : ServiceTestBase<IAddressDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetOpenApplicationPropertyAndClientQuery();
            base.PerformQuery(query);
            var propertyKey = query.Result.Results.FirstOrDefault().PropertyKey;
            var property = TestApiClient.GetByKey<PropertyDataModel>(propertyKey);
            int? existingAddressKey = property.AddressKey;
            var streetAddressModel = new StreetAddressModel("", "", "", this.randomizer.Next(0, 100000).ToString(), "Smith Street", "Durban", "Durban", "Kwazulu-Natal", "4001");
            var command = new LinkStreetAddressToPropertyCommand(streetAddressModel, property.PropertyKey);
            base.Execute<LinkStreetAddressToPropertyCommand>(command);
            var newProperty = TestApiClient.GetByKey<PropertyDataModel>(propertyKey);
            Assert.AreNotEqual(property.AddressKey, newProperty.AddressKey);
        }
    }
}
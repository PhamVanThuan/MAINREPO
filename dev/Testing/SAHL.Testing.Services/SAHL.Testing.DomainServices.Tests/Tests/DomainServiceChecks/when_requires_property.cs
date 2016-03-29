using NUnit.Framework;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;

namespace SAHL.Testing.Services.Tests.DomainServiceChecks
{
    public class when_requires_property : ServiceTestBase<IAddressDomainServiceClient>
    {
        [Test]
        public void when_unsuccessful()
        {
            var streetAddressModel = new StreetAddressModel("", "", "", this.randomizer.Next(0, 100000).ToString(), "Smith Street", "Durban", "Durban", "Kwazulu-Natal", "4001");
            var command = new LinkStreetAddressToPropertyCommand(streetAddressModel, Int32.MaxValue - 1);
            base.Execute<LinkStreetAddressToPropertyCommand>(command)
                .AndExpectThatErrorMessagesContain("No property could be found against your property number.");
        }
    }
}
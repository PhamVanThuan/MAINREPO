using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.ClientAddressModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static Exception ex;
        private static int clientKey;
        private static int addressKey;
        private static ClientAddressModel model;

        private Establish context = () =>
        {
            clientKey = 1;
            addressKey = 1;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new ClientAddressModel(clientKey, addressKey, AddressType.Postal);
            });
        };

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_a_valid_model = () =>
        {
            model.ClientKey.ShouldEqual(clientKey);
            model.AddressKey.ShouldEqual(addressKey);
        };
    }
}
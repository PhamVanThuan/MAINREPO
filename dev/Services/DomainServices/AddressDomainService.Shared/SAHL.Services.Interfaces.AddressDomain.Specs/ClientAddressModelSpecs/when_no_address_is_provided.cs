using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.ClientAddressModelSpecs
{
    public class when_no_address_is_provided : WithFakes
    {
        private static Exception exception;
        private static int clientKey;
        private static int addressKey;

        Establish context = () =>
        {
            clientKey = 1;
            addressKey = 0;
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                new ClientAddressModel(clientKey, addressKey, AddressType.Postal);
            });
        };

        It should_throw_an_exception_when_constructing = () =>
        {
            exception.ShouldBeOfExactType(typeof(ValidationException));
        };

        It should_include_a_message= () =>
        {
            exception.Message.ShouldEqual("An AddressKey must be provided.");
        };

    }
}
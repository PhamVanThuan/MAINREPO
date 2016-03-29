using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.ClientAddressModelSpecs
{
    public class when_no_client_is_provided : WithFakes
    {
        private static Exception exception;
        private static int clientKey;
        private static int addressKey;

        private Establish context = () =>
        {
            clientKey = 0;
            addressKey = 1;
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                new ClientAddressModel(clientKey, addressKey, AddressType.Postal);
            });
        };

        private It should_throw_an_exception_when_constructing = () =>
        {
            exception.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_include_a_message = () =>
        {
            exception.Message.ShouldEqual("A ClientKey must be provided.");
        };
    }
}
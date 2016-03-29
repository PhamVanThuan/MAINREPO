using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.ApplicantDomiciliumAddressModelSpecs
{
    class when_client_domicilium_key_is_zero : WithFakes
    {
        static Exception ex;
        static ApplicantDomiciliumAddressModel model;
        static int applicationNumber, clientKey, clientDomiciliumKey;

        private Because of = () =>
        {
            applicationNumber = 1;
            clientKey = 1;
            clientDomiciliumKey = 0;

            ex = Catch.Exception(() =>
            {
                model = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_return_a_message = () =>
        {
            ex.Message.ShouldEqual("A ClientDomiciliumKey must be provided.");
        };
    }
}

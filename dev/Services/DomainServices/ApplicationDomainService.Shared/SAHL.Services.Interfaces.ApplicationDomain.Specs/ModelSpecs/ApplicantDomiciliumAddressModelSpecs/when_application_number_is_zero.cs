using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.ApplicantDomiciliumAddressModelSpecs
{
    class when_application_number_is_zero : WithFakes
    {
        static Exception ex;
        static ApplicantDomiciliumAddressModel model;
        static int applicationNumber, clientKey, clientDomiciliumKey;

        private Because of = () =>
        {
            applicationNumber = 0;
            clientKey = 1;
            clientDomiciliumKey = 1;

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
            ex.Message.ShouldEqual("An ApplicationNumber must be provided.");
        };
    }
}

using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.LinkDomiciliumAddressToApplicant
{
    class when_all_properties_are_provided : WithFakes
    {
        static Exception ex;
        static ApplicantDomiciliumAddressModel model;
        static int applicationNumber, clientKey, clientDomiciliumKey;

        private Because of = () =>
        {
            applicationNumber = 1;
            clientKey = 1;
            clientDomiciliumKey = 1;

            ex = Catch.Exception(() =>
            {
                model = new ApplicantDomiciliumAddressModel(applicationNumber, clientDomiciliumKey, clientKey);
            });
        };

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}

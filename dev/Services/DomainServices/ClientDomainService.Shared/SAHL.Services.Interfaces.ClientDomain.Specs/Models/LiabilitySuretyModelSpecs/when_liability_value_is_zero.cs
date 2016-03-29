using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LiabilitySuretyModelSpecs
{
    public class when_liability_value_is_zero : WithFakes
    {
        private static int liabilityValue, assetValue;
        private static string description;
        private static LiabilitySuretyModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            assetValue = 1;
            liabilityValue = 0;
            description = "Test";
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new LiabilitySuretyModel(assetValue, liabilityValue, description);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("Liability value must be provided.");
        };

        private It should_not_construct_the_model = () =>
        {
            model.ShouldBeNull();
        };
    }
}
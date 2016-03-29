using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LiabilitySuretyModelSpecs
{
    public class when_no_description_is_provided : WithFakes
    {
        private static int liabilityValue, assetValue;
        private static string description;
        private static LiabilitySuretyModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            assetValue = 1;
            liabilityValue = 1;
            description = string.Empty;
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
            ex.Message.ShouldEqual("The Description field is required.");
        };

        private It should_not_construct_the_model = () =>
        {
            model.ShouldBeNull();
        };
    }
}
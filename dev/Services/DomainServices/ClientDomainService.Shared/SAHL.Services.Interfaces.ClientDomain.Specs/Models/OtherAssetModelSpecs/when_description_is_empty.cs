using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.OtherAssetModelSpecs
{
    public class when_description_is_empty : WithFakes
    {
        private static string description;
        private static double assetValue;
        private static double liabilityValue;
        private static OtherAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            description = " ";
            assetValue = 1d;
            liabilityValue = 0d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { new OtherAssetModel(description, assetValue, liabilityValue); });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("The Description field is required.");
        };
    }
}

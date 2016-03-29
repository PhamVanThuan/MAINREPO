using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.OtherAssetModelSpecs
{
    public class when_liability_value_is_less_than_zero : WithFakes
    {
        private static string description;
        private static double assetValue;
        private static double liabilityValue;
        private static OtherAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            description = "Other asset";
            assetValue = 1d;
            liabilityValue = -1d;
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
            ex.Message.ShouldEqual("Liability value must be greater than or equal to zero.");
        };
    }
}
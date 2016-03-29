using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.FixedPropertyAssetModelSpecs
{
    public class when_liability_value_is_less_than_zero : WithFakes
    {
        private static DateTime dateAquired;
        private static int addressKey;
        private static double assetValue, liabilityValue;
        private static FixedPropertyAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            dateAquired = DateTime.Now;
            addressKey = 1;
            assetValue = 1d;
            liabilityValue = -1d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new FixedPropertyAssetModel(dateAquired, addressKey, assetValue, liabilityValue); });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("Liability value must be provided.");
        };
    }
}

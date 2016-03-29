using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.FixedPropertyAssetModelSpecs
{
    public class when_asset_value_is_zero : WithFakes
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
            assetValue = 0d;
            liabilityValue = 0d;
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
            ex.Message.ShouldEqual("Asset value must be provided.");
        };
    }
}
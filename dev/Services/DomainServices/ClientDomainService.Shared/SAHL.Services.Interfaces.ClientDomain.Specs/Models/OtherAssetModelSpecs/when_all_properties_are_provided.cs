using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.OtherAssetModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
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
            liabilityValue = 0d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { new OtherAssetModel(description, assetValue, liabilityValue); });
        };

        private It should_not_throw_an_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            ex.ShouldBeNull();
        };
    }
}
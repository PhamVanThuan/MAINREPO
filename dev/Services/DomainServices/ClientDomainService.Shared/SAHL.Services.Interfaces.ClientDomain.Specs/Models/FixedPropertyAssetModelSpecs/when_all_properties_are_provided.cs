using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.FixedPropertyAssetModelSpecs
{
    class when_all_properties_are_provided : WithFakes
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
            liabilityValue = 0d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new FixedPropertyAssetModel(dateAquired, addressKey, assetValue, liabilityValue); });
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

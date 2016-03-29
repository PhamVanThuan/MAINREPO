using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LiabilitySuretyModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static int liabilityValue, assetValue;
        private static string description;
        private static LiabilitySuretyModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            assetValue = 1;
            liabilityValue = 1;
            description = "Test";
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new LiabilitySuretyModel(assetValue, liabilityValue, description);
            });
        };

        private It should_not_throw_an_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}
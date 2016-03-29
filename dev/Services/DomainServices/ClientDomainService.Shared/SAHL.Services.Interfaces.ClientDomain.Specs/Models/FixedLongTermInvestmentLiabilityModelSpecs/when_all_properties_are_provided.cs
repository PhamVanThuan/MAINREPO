using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.FixedLongTermInvestmentLiabilityModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static string companyName;
        private static int liabilityValue;
        private static FixedLongTermInvestmentLiabilityModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            companyName = "Test";
            liabilityValue = 1;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new FixedLongTermInvestmentLiabilityModel(companyName, liabilityValue);
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
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.FixedLongTermInvestmentLiabilityModelSpecs
{
    public class when_liability_value_is_less_than_zero : WithFakes
    {
        private static string companyName;
        private static int liabilityValue;
        private static FixedLongTermInvestmentLiabilityModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            companyName = "Test";
            liabilityValue = -1;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new FixedLongTermInvestmentLiabilityModel(companyName, liabilityValue);
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
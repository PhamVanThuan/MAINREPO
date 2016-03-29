using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LifeAssuranceAssetModelSpecs
{
    public class when_surrender_amount_is_zero : WithFakes
    {
        private static string companyName;
        private static double surrenderValue;
        private static LifeAssuranceAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            companyName = "Company name";
            surrenderValue = 0d;
        };

        Because of = () =>
        {
            ex = Catch.Exception(() => { model = new LifeAssuranceAssetModel(companyName, surrenderValue); });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("Surrender Value must be greater than zero.");
        };
    }
}

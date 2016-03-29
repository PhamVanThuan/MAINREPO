using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LifeAssuranceAssetModelSpecs
{
    public class when_company_name_is_not_provided : WithFakes
    {
        private static string companyName;
        private static double surrenderValue;
        private static LifeAssuranceAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            companyName = " ";
            surrenderValue = 1d;
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
            ex.Message.ShouldEqual("The CompanyName field is required.");
        };
    }
}

using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LifeAssuranceAssetModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static string companyName;
        private static double surrenderValue;
        private static LifeAssuranceAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            companyName = "Company name";
            surrenderValue = 1d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new LifeAssuranceAssetModel(companyName, surrenderValue); });
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
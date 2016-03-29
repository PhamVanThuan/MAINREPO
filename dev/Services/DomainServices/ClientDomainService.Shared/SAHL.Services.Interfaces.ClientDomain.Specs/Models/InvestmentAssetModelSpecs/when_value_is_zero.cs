using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.InvestmentAssetModelSpecs
{
    public class when_value_is_zero : WithFakes
    {
        private static AssetInvestmentType investmentType;
        private static string companyName;
        private static double value;
        private static InvestmentAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            investmentType = AssetInvestmentType.ListedInvestments;
            companyName = "Company name";
            value = 0d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new InvestmentAssetModel(investmentType, companyName, value); });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("Asset value must be greater than zero.");
        };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.InvestmentAssetModelSpecs
{
    public class when_asset_liability_type_is_unlisted_investment : WithFakes
    {
        private static AssetInvestmentType investmentType;
        private static string companyName;
        private static double value;
        private static InvestmentAssetModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            investmentType = AssetInvestmentType.UnlistedInvestments;
            companyName = "Company name";
            value = 1d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new InvestmentAssetModel(investmentType, companyName, value); });
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

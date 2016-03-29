using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    public class when_saving_a_listed_investment_asset : WithFakes
    {
        private static IAssetLiabilityDataManager assetliabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static InvestmentAssetModel model;
        private static int result, assetLiabilityKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            assetliabilityDataManager = new AssetLiabilityDataManager(dbFactory);
            assetLiabilityKey = 1111;
            model = new InvestmentAssetModel(AssetInvestmentType.UnlistedInvestments, "Company name", 1d);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<AssetLiabilityDataModel>(Param.IsAny<AssetLiabilityDataModel>()))
                .Callback<AssetLiabilityDataModel>(y => y.AssetLiabilityKey = assetLiabilityKey);
        };

        private Because of = () =>
        {
            result = assetliabilityDataManager.SaveInvestmentAsset(model);
        };

        private It should_insert_life_assurance_asset_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<AssetLiabilityDataModel>(Arg.Is<AssetLiabilityDataModel>(y => y.AssetLiabilityTypeKey == (int)model.InvestmentType &&
                y.CompanyName == model.CompanyName &&
                y.AssetValue == model.AssetValue)));
        };

        private It should_return_an_assetLiability_key = () =>
        {
            result.ShouldEqual(assetLiabilityKey);
        };
    }
}

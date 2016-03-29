using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    public class when_saving_a_life_assurance_asset : WithFakes
    {
        private static IAssetLiabilityDataManager assetliabilityDataManager;
        private static LifeAssuranceAssetModel model;
        private static int result, assetLiabilityKey;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            assetliabilityDataManager = new AssetLiabilityDataManager(dbFactory);
            assetLiabilityKey = 54321;
            model = new LifeAssuranceAssetModel("Sanlam", 2000000);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>
             (x => x.Insert<AssetLiabilityDataModel>(Param.IsAny<AssetLiabilityDataModel>()))
              .Callback<AssetLiabilityDataModel>(y => y.AssetLiabilityKey = assetLiabilityKey);
        };

        private Because of = () =>
        {
            result = assetliabilityDataManager.SaveLifeAssuranceAsset(model);
        };

        private It should_insert_life_assurance_asset_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo
             (x => x.Insert<AssetLiabilityDataModel>
              (Arg.Is<AssetLiabilityDataModel>(y => y.CompanyName == model.CompanyName && y.AssetValue == model.SurrenderValue)));
        };

        private It should_return_an_assetLiability_key = () =>
        {
            result.ShouldEqual(assetLiabilityKey);
        };
    }
}

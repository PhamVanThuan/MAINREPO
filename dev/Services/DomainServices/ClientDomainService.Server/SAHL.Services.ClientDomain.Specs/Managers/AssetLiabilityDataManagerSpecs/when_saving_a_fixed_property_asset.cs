using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    public class when_saving_a_fixed_property_asset : WithFakes
    {
        private static IAssetLiabilityDataManager assetliabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static FixedPropertyAssetModel model;
        private static int result, assetLiabilityKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            assetliabilityDataManager = new AssetLiabilityDataManager(dbFactory);
            assetLiabilityKey = 54321;
            model = new FixedPropertyAssetModel(DateTime.Now, 2222, 1.1d, 0.1d);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>
             (x => x.Insert<AssetLiabilityDataModel>
              (Param.IsAny<AssetLiabilityDataModel>())).Callback<AssetLiabilityDataModel>
               (y => y.AssetLiabilityKey = assetLiabilityKey);
        };

        private Because of = () =>
        {
            result = assetliabilityDataManager.SaveFixedPropertyAsset(model);
        };

        private It should_insert_fixed_property_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo
             (x => x.Insert<AssetLiabilityDataModel>(Arg.Is<AssetLiabilityDataModel>(y => y.AddressKey == model.AddressKey)));
        };

        private It should_return_an_assetLiability_key = () =>
        {
            result.ShouldEqual(assetLiabilityKey);
        };
    }
}
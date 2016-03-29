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
    public class when_saving_a_long_term_investment_liability : WithFakes
    {
        private static IAssetLiabilityDataManager assetliabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static int assetLiabilityKey, result;
        private static FixedLongTermInvestmentLiabilityModel model;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            assetliabilityDataManager = new AssetLiabilityDataManager(dbFactory);
            assetLiabilityKey = 54321;
            model = new FixedLongTermInvestmentLiabilityModel("Test", 1);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>
             (x => x.Insert<AssetLiabilityDataModel>(Param.IsAny<AssetLiabilityDataModel>()))
              .Callback<AssetLiabilityDataModel>(y => y.AssetLiabilityKey = assetLiabilityKey);
        };

        private Because of = () =>
        {
            result = assetliabilityDataManager.SaveFixedLongTermInvestmentLiability(model);
        };

        private It should_insert_fixed_term_investment_liability_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo
             (x => x.Insert<AssetLiabilityDataModel>(Arg.Is<AssetLiabilityDataModel>
              (y => y.CompanyName == model.CompanyName)));
        };

        private It should_return_an_assetLiability_key = () =>
        {
            result.ShouldEqual(assetLiabilityKey);
        };
    }
}
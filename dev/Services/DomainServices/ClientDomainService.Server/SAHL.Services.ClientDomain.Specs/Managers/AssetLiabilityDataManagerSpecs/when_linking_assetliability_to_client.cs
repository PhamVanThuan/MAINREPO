using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;


namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    internal class when_linking_assetliability_to_client : WithFakes
    {
        private static IAssetLiabilityDataManager assetliabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static int result, clientKey, assetLiabilityKey, legalEntityAssetLiabilityKey;

        private Establish context = () =>
        {
            clientKey = 1111;
            assetLiabilityKey = 2222;
            dbFactory = new FakeDbFactory();
            assetliabilityDataManager = new AssetLiabilityDataManager(dbFactory);
            legalEntityAssetLiabilityKey = 3333;
            dbFactory.FakedDb.DbContext.WhenToldTo
              (x => x.Insert<LegalEntityAssetLiabilityDataModel>
               (Param.IsAny<LegalEntityAssetLiabilityDataModel>())).Callback<LegalEntityAssetLiabilityDataModel>
                (y => y.LegalEntityAssetLiabilityKey = legalEntityAssetLiabilityKey);
        };

        private Because of = () =>
        {
            result = assetliabilityDataManager.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey);
        };

        private It should_link_assetliability_to_client = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo
             (x => x.Insert<LegalEntityAssetLiabilityDataModel>
              (Arg.Is<LegalEntityAssetLiabilityDataModel>(y => y.AssetLiabilityKey == assetLiabilityKey 
               && y.LegalEntityKey == clientKey && y.GeneralStatusKey == (int)GeneralStatus.Active)));
        };

        private It should_return_an_assetLiability_key = () =>
        {
            result.ShouldEqual(legalEntityAssetLiabilityKey);
        };
    }
}
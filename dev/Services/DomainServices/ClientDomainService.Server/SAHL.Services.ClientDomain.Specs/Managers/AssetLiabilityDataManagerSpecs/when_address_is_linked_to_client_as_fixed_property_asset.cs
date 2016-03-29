using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.ClientDomain.Managers.AssetLiability.Statements;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    public class when_address_is_linked_to_client_as_fixed_property_asset : WithFakes
    {
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static int clientKey, addressKey;
        private static bool result;

        private Establish context = () =>
        {
            clientKey = 1111;
            addressKey = 2222;
            result = false;
            dbFactory = new FakeDbFactory();
            assetLiabilityDataManager = new AssetLiabilityDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo
             (x => x.SelectOne(Param.IsAny<ISqlStatement<int>>())).Return(1);
        };

        private Because of = () =>
        {
            result = assetLiabilityDataManager.CheckIsAddressLinkedToClientAsFixedPropertyAsset(clientKey, addressKey);
        };

        private It should_query_if_address_is_linked_to_client_as_fixed_property_asset = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo
             (x => x.SelectOne(Param.IsAny<IsAddressLinkedToClientAsGivenAssetTypeStatement>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
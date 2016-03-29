using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.ClientDomain.Managers.AssetLiability.Statements;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.AssetLiabilityDataManagerSpecs
{
    public class when_client_address_exists : WithFakes
    {
        private static AssetLiabilityDataManager assetLiabilityDataManager;
        private static int clientKey, addressKey;
        private static bool result;
        private static LegalEntityAddressDataModel queryResult;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            clientKey = 1111;
            addressKey = 2222;
            result = false;
            fakeDbFactory = new FakeDbFactory();
            queryResult = new LegalEntityAddressDataModel(1, 2, 1, DateTime.Now, 1);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(
                Param.IsAny<ISqlStatement<int>>())).Return(1);
            assetLiabilityDataManager = new AssetLiabilityDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            result = assetLiabilityDataManager.CheckIsAddressLinkedToClientAsFixedPropertyAsset(clientKey, addressKey);
        };

        private It should_query_if_address_is_linked_to_client_as_fixed_property_asset = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<IsAddressLinkedToClientAsGivenAssetTypeStatement>(
                y => y.AddressKey == addressKey && y.ClientKey == clientKey && y.AssetLiabilityType == AssetLiabilityType.FixedProperty)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
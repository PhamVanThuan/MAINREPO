using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;


namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddFixedPropertyAssetToClient
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddFixedPropertyAssetToClientCommandHandler handler;
        private static IDomainRuleManager<FixedPropertyAssetModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<FixedPropertyAssetModel>>();
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
        };

        Because of = () =>
        {
            handler = new AddFixedPropertyAssetToClientCommandHandler
                (domainRuleManager, domainQueryService, assetLiabilityDataManager, unitOfWorkFactory, eventRaiser);
        };

        It should_register_the_fixed_property_date_rules = () =>
        {
            domainRuleManager.WasToldTo
                (x => x.RegisterRule(Param.IsAny<IDomainRule<FixedPropertyAssetModel>>()));
        };
    }
}

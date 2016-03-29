using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AcceptThirdPartyInvoice
{
    public class when_creating_handler : WithCoreFakes
    {
        private static IDomainQueryServiceClient domainQueryServiceClient;
        private static IDomainRuleManager<IAccountRuleModel> domainRuleManager;
        private static IThirdPartyInvoiceDataManager thirdPartyDataManager;
        private static AcceptThirdPartyInvoiceCommandHandler commandHandler;

        private Establish context = () =>
        {
            thirdPartyDataManager = An<IThirdPartyInvoiceDataManager>();
            domainQueryServiceClient = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<IAccountRuleModel>>();
        };

        private Because of = () =>
        {
            commandHandler = new AcceptThirdPartyInvoiceCommandHandler(unitOfWorkFactory, domainRuleManager, domainQueryServiceClient,
                serviceCommandRouter, linkedKeyManager, eventRaiser, serviceCoordinator, thirdPartyDataManager);
        };

        private It Should_register_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterPartialRule<IAccountRuleModel>(Param.IsAny<TheInvoicesAccountNumberMustBeAValidSAHLAccountRule>()));
        };
    }
}
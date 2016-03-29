using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.ITC.CommandHandlers;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Rules;
using SAHL.Services.ITC.Server.Specs.Managers.ITC;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformClientITCCheck
{
    public class when_creating_command : WithITCManager
    {
        private static PerformClientITCCheckCommandHandler handler;
        private static ITransUnionService transUnionService;
        private static IITCCommon itcCommon;

        private Establish context = () =>
        {
            transUnionService = An<ITransUnionService>();
            itcCommon = An<IITCCommon>();
        };

        private Because of = () =>
        {
            handler = new PerformClientITCCheckCommandHandler(itcManager, transUnionService, itcCommon, clientDomainService, addressDomainService,
                 domainRuleContext, applicationDomainClient);
        };

        private It should_register_the_client_should_be_related_to_account_rule = () =>
        {
            domainRuleContext.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientShouldBeRelatedToAccountRule>()));
        };
    }
}
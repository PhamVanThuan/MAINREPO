using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.ITC.CommandHandlers;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.Server.Specs.Managers.ITC;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Linq;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformClientITCCheck
{
    public class when_performing_an_itc_check_and_error_is_thrown : WithITCManager
    {
        private static PerformClientITCCheckCommandHandler handler;
        private static ITransUnionService transUnionService;
        private static PerformClientITCCheckCommand command;
        private static Guid itcID;
        private static ISystemMessageCollection messages;
        private static Exception runtimeException;
        private static Exception thrownException;
        private static IITCCommon itcCommon;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();
            itcCommon = An<IITCCommon>();
            runtimeException = new Exception("Something bad happened!");

            itcID = Guid.Parse("{CAA5DD1C-0770-49E3-96CB-99A64FCC9AFD}");

            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses)).Return(itcRequest);
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>())).Throw(runtimeException);
            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(clientDetails.ToList());
                return new SystemMessageCollection();
            });

            addressDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<GetClientStreetAddressByClientKeyQuery>())).Return<GetClientStreetAddressByClientKeyQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientStreetAddressByClientKeyQueryResult>(clientAddresses);
                return SystemMessageCollection.Empty();
            });
            handler = new PerformClientITCCheckCommandHandler(itcManager, transUnionService, itcCommon, clientDomainService, addressDomainService,
                domainRuleContext, applicationDomainClient);
            command = new PerformClientITCCheckCommand(clientIdNumber, applicationNumber, userId);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => messages = handler.HandleCommand(command, new ServiceRequestMetadata()));
        };

        private It should_execute_domain_rules = () =>
        {
            domainRuleContext.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command));
        };

        private It should_get_the_client_using_the_id_number = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>()));
        };

        private It should_get_the_client_address = () =>
        {
            addressDomainService.WasToldTo(x => x.PerformQuery(Param.IsAny<GetClientStreetAddressByClientKeyQuery>()));
        };

        private It should_create_an_itc_request = () =>
        {
            itcManager.WasToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses));
        };

        private It should_perform_an_itc_check = () =>
        {
            itcCommon.WasToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_log_the_itc_request_and_response = () =>
        {
            itcManager.WasToldTo(x => x.LogFailedITCRequestAndResponse(itcRequest, null, "PerformClientITCCheckCommandHandler"));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };

    }
}
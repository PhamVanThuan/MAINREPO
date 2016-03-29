using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.AddressDomain;
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

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformClientITCCheck
{
    public class when_client_has_no_street_addresses : WithITCManager
    {
        private static PerformClientITCCheckCommandHandler handler;
        private static ITransUnionService transUnionService;
        private static PerformClientITCCheckCommand command;
        private static ISystemMessageCollection messages;
        private static Exception runtimeException;
        private static IITCCommon itcCommon;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();
            itcCommon = An<IITCCommon>();
            runtimeException = new Exception("Something bad happened!");
            addressDomainService = An<IAddressDomainServiceClient>();
            applicationNumber = 1001;
            userId = "X2";

            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses)).Return(itcRequest);
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()))
                .Throw(runtimeException);
            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(clientDetails.ToList());
                return new SystemMessageCollection();
            });

            addressDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<GetClientStreetAddressByClientKeyQuery>()))
                .Return<GetClientStreetAddressByClientKeyQuery>(y =>
                {
                    y.Result =
                        new ServiceQueryResult<GetClientStreetAddressByClientKeyQueryResult>(new List<GetClientStreetAddressByClientKeyQueryResult>());
                    return SystemMessageCollection.Empty();
                });

            handler = new PerformClientITCCheckCommandHandler(itcManager,
                transUnionService,
                itcCommon,
                clientDomainService,
                addressDomainService,
                domainRuleContext,
                applicationDomainClient);
            command = new PerformClientITCCheckCommand(clientIdNumber, applicationNumber, userId);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, new ServiceRequestMetadata());
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

        private It should_not_create_an_itc_request = () =>
        {
            itcManager.WasNotToldTo(
                x => x.CreateITCRequestForApplicant(clientDetails.First(), Param.IsAny<List<GetClientStreetAddressByClientKeyQueryResult>>()));
        };

        private It should_not_perform_itc_check = () =>
        {
            itcCommon.WasNotToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_not_log_the_itc_request_and_response = () =>
        {
            itcManager.WasNotToldTo(x => x.LogFailedITCRequestAndResponse(itcRequest, null, "PerformClientITCCheckCommandHandler"));
        };

        private It should_contain_an_error_message = () =>
        {
            messages.ErrorMessages().Any(x => x.Message == "The client provided does not have a street address.");
        };
    }
}

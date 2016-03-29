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
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.CommandHandlers;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.Server.Specs.Managers.ITC;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Linq;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformClientITCCheck
{
    public class when_performing_an_itc_check_and_response_is_null : WithITCManager
    {
        private static PerformClientITCCheckCommandHandler handler;
        private static ITransUnionService transUnionService;
        private static PerformClientITCCheckCommand command;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid itcID;
        private static ISystemMessageCollection messages;
        private static ItcResponse itcResponse;
        private static IITCCommon itcCommon;
        private static int clientID;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();
            itcCommon = An<IITCCommon>();
            itcID = Guid.Parse("{71FBC3BE-1CF8-4A4E-87E7-2D1F4A8720B7}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");
            userId = "1001";

            clientID = 10011;
            applicationNumber = 110001;
            handler = new PerformClientITCCheckCommandHandler(itcManager, transUnionService, itcCommon, clientDomainService, addressDomainService,
                domainRuleContext, applicationDomainClient);
            command = new PerformClientITCCheckCommand(clientIdNumber, applicationNumber, userId);

            itcResponse = null;
            itcRequest = new ItcRequest();

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

            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses)).Return(itcRequest);
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>())).Return(itcResponse);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_execute_domain_rules = () =>
        {
            domainRuleContext.WasToldTo(x => x.ExecuteRules(messages, command));
        };

        private It should_create_an_itc_request = () =>
        {
            itcManager.WasToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses));
        };

        private It should_call_the_external_itc_service = () =>
        {
            itcCommon.WasToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_not_store_the_itc = () =>
        {
            itcManager.WasNotToldTo(x => x.SaveITC(clientID, applicationNumber, Param.IsAny<ItcResponse>(), Param.IsAny<String>()));
        };
    }
}
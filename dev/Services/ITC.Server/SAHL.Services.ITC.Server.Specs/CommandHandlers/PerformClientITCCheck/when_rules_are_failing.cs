using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.CommandHandlers;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.Server.Specs.Managers.ITC;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformClientITCCheck
{
    public class when_rules_are_failing : WithITCManager
    {
        private static PerformClientITCCheckCommandHandler handler;
        private static ITransUnionService transUnionService;
        private static PerformClientITCCheckCommand command;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid itcID;
        private static ISystemMessageCollection messages;
        private static ItcResponse itcResponse;
        private static IITCCommon itcCommon;
        private static IUnitOfWork unitOfWork;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();
            itcCommon = An<IITCCommon>();
            clientDomainService = An<IClientDomainServiceClient>();
            addressDomainService = An<IAddressDomainServiceClient>();

            messages = new SystemMessageCollection();
            userId = "1001";
            applicationNumber = 110001;
            itcID = Guid.Parse("{CAA5DD1C-0770-49E3-96CB-99A64FCC9AFD}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            domainRuleContext = new DomainRuleManager<PerformClientITCCheckCommand>();
            handler = new PerformClientITCCheckCommandHandler(itcManager, transUnionService, itcCommon, clientDomainService, addressDomainService,
                domainRuleContext, applicationDomainClient);
            command = new PerformClientITCCheckCommand(clientIdNumber, applicationNumber, userId);
            itcRequest = new ItcRequest();
            itcResponse = new ItcResponse("", "", DateTime.Now, ServiceResponseStatus.Success, XDocument.Parse("<request></request>"), XDocument.Parse("<response></response>"));
            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses)).Return(itcRequest);
            transUnionService.WhenToldTo(x => x.PerformRequest(itcRequest)).Return(itcResponse);
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>())).Return(itcResponse);

            var errorMessage = SystemMessageCollection.Empty();
            errorMessage.AddMessage(new SystemMessage("The client is not related to the account.", SystemMessageSeverityEnum.Error));

            applicationDomainClient.WhenToldTo(x => x.PerformQuery(Param.IsAny<DoesAccountBelongToClientQuery>())).Return<DoesAccountBelongToClientQuery>(y =>
            {
                y.Result = new ServiceQueryResult<DoesAccountBelongToClientQueryResult>(
                    new List<DoesAccountBelongToClientQueryResult> { new DoesAccountBelongToClientQueryResult ()   
                });
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_create_an_itc_request = () =>
        {
            itcManager.WasNotToldTo(x => x.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses));
        };

        private It should_call_the_external_itc_service = () =>
        {
            itcCommon.WasNotToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_store_the_itc = () =>
        {
            itcManager.WasNotToldTo(x => x.SaveITC(clientId, applicationNumber, itcResponse, userId));
        };

        private It should_not_log_itc_request_or_response = () =>
        {
            itcManager.WasNotToldTo(x => x.LogFailedITCRequestAndResponse(Param.IsAny<ItcRequest>(), Param.IsAny<ItcResponse>(), Param.IsAny<string>()));
        };

        private It should_not_return_any_errors = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}
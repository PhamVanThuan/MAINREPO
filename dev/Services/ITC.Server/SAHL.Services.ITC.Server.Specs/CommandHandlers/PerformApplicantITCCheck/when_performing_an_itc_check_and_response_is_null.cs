using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.CommandHandlers;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.TransUnion;
using System;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformApplicantITCCheck
{
    public class when_performing_an_itc_check_and_response_is_null : WithFakes
    {
        private static PerformCapitecITCCheckCommandHandler handler;
        private static IItcManager itcManager;
        private static ITransUnionService transUnionService;
        private static PerformCapitecITCCheckCommand command;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid itcID;
        private static ISystemMessageCollection messages;
        private static ItcRequest itcRequest;
        private static ItcResponse itcResponse;
        private static IITCCommon itcCommon;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();

            itcCommon = An<IITCCommon>();
            itcID = Guid.Parse("{71FBC3BE-1CF8-4A4E-87E7-2D1F4A8720B7}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            handler = new PerformCapitecITCCheckCommandHandler(itcManager, transUnionService, itcCommon);
            command = new PerformCapitecITCCheckCommand(itcID, applicantITCRequestDetails);

            itcRequest = new ItcRequest();
            itcResponse = null;
            itcRequest = new ItcRequest();
            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(applicantITCRequestDetails)).Return(itcRequest);
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>())).Return(itcResponse);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_create_an_itc_request = () =>
        {
            itcManager.WasToldTo(x => x.CreateITCRequestForApplicant(applicantITCRequestDetails));
        };

        private It should_call_the_external_itc_service = () =>
        {
            itcCommon.WasToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_not_store_the_itc = () =>
        {
            itcManager.WasNotToldTo(x => x.SaveITC(Param.IsAny<Guid>(), Param.IsAny<ItcResponse>()));
        };
    }
}
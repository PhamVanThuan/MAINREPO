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
    public class when_performing_an_itc_check_and_error_is_thrown : WithFakes
    {
        private static PerformCapitecITCCheckCommandHandler handler;
        private static IItcManager itcManager;
        private static ITransUnionService transUnionService;
        private static PerformCapitecITCCheckCommand command;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid itcID;
        private static ISystemMessageCollection messages;
        private static ItcRequest itcRequest;
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
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            handler = new PerformCapitecITCCheckCommandHandler(itcManager, transUnionService, itcCommon);
            command = new PerformCapitecITCCheckCommand(itcID, applicantITCRequestDetails);

            itcRequest = new ItcRequest();
            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(applicantITCRequestDetails)).Return(itcRequest);
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>())).Throw(runtimeException);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => messages = handler.HandleCommand(command, new ServiceRequestMetadata()));
        };

        private It should_create_an_itc_request = () =>
        {
            itcManager.WasToldTo(x => x.CreateITCRequestForApplicant(applicantITCRequestDetails));
        };

        private It should_log_the_itc_request_and_response = () =>
        {
            itcManager.WasToldTo(x => x.LogFailedITCRequestAndResponse(itcRequest, null, "PerformCapitecITCCheckCommandHandler"));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}
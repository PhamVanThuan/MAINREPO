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
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformApplicantITCCheck
{
    public class when_performing_an_itc_check_and_itc_has_error_code : WithFakes
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

            itcID = Guid.Parse("{CAA5DD1C-0770-49E3-96CB-99A64FCC9AFD}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            handler = new PerformCapitecITCCheckCommandHandler(itcManager, transUnionService, itcCommon);
            command = new PerformCapitecITCCheckCommand(itcID, applicantITCRequestDetails);

            itcRequest = new ItcRequest();
            itcResponse = new ItcResponse("B4D", "Something's wrong", DateTime.Now, ServiceResponseStatus.Success, XDocument.Parse("<request></request>"), XDocument.Parse("<response></response>"));
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
            itcCommon.WhenToldTo(x => x.PerformITCRequest(itcManager, transUnionService, itcRequest, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_store_the_itc = () =>
        {
            itcManager.WasToldTo(x => x.SaveITC(itcID, itcResponse));
        };

        private It should_not_return_an_error = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
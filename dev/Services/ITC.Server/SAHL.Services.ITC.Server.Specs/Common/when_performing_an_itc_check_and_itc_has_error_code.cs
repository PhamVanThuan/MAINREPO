using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformApplicantITCCheck
{
    public class when_performing_an_itc_check_and_itc_has_error_code1 : WithFakes
    {
        private static IItcManager itcManager;
        private static ITransUnionService transUnionService;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid applicantID, itcID;
        private static ISystemMessageCollection messages;
        private static ItcRequest itcRequest;
        private static ItcResponse itcResponse;
        private static ItcResponse expectedITCResponse;
        private static ITCCommon itcCommon;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();

            messages = SystemMessageCollection.Empty();
            applicantID = Guid.Parse("{E78EDA84-A90A-4A3A-8A17-BFBF1C7B820C}");
            itcID = Guid.Parse("{CAA5DD1C-0770-49E3-96CB-99A64FCC9AFD}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            itcRequest = new ItcRequest();
            itcResponse = new ItcResponse("B4D", "Something's wrong", DateTime.Now, ServiceResponseStatus.Success, XDocument.Parse("<request></request>"), XDocument.Parse("<response></response>"));
            transUnionService.WhenToldTo(x => x.PerformRequest(itcRequest)).Return(itcResponse);
            itcCommon = new ITCCommon();
        };

        private Because of = () =>
        {
            expectedITCResponse = itcCommon.PerformITCRequest(itcManager, transUnionService, itcRequest, messages);
        };

        private It should_call_the_external_itc_service = () =>
        {
            transUnionService.WasToldTo(x => x.PerformRequest(itcRequest));
        };

        private It should_log_the_failed_itc_request = () =>
        {
            itcManager.WasToldTo(x => x.LogFailedITCRequestAndResponse(itcRequest, itcResponse, "ITCCommon"));
        };

        private It should_not_return_an_error = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_return_the_expected_itc_response = () =>
        {
            expectedITCResponse.ErrorCode.ShouldEqual(itcResponse.ErrorCode);
        };
    }
}
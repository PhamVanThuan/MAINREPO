using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Linq;

namespace SAHL.Services.ITC.Server.Specs.CommandHandlers.PerformApplicantITCCheck
{
    public class when_performing_an_itc_check_and_response_is_null1 : WithFakes
    {
        private static IItcManager itcManager;
        private static ITransUnionService transUnionService;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid applicantID;
        private static Guid itcID;
        private static ISystemMessageCollection messages;
        private static ItcRequest itcRequest;
        private static ItcResponse expectedITCResponse;
        private static ITCCommon itcCommon;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();

            applicantID = Guid.Parse("{E78EDA84-A90A-4A3A-8A17-BFBF1C7B820C}");
            itcID = Guid.Parse("{71FBC3BE-1CF8-4A4E-87E7-2D1F4A8720B7}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            itcRequest = new ItcRequest();
            itcManager.WhenToldTo(x => x.CreateITCRequestForApplicant(applicantITCRequestDetails)).Return(itcRequest);
            messages = new SystemMessageCollection();
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

        private It should_not_store_the_itc = () =>
        {
            itcManager.WasNotToldTo(x => x.SaveITC(Param.IsAny<Guid>(), Param.IsAny<ItcResponse>()));
        };

        private It should_log_the_request_and_response = () =>
        {
            itcManager.WasToldTo(x => x.LogFailedITCRequestAndResponse(itcRequest, null, "ITCCommon"));
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().Any(x => x.Message.Contains("ITC Response object is empty")).ShouldBeTrue();
        };
    }
}
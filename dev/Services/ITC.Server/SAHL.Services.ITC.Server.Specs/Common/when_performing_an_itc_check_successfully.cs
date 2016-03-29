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
    public class when_performing_an_itc_check_successfully1 : WithFakes
    {
        private static IItcManager itcManager;
        private static ITransUnionService transUnionService;
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static Guid applicantID, itcID;
        private static ISystemMessageCollection messages;
        private static ItcRequest itcRequest;
        private static ItcResponse expectedITCResponse;
        private static ItcResponse itcResponse;
        private static ITCCommon itcCommon;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            transUnionService = An<ITransUnionService>();

            applicantID = Guid.Parse("{E78EDA84-A90A-4A3A-8A17-BFBF1C7B820C}");
            itcID = Guid.Parse("{CAA5DD1C-0770-49E3-96CB-99A64FCC9AFD}");
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart", "Smith", new DateTime(1990, 11, 21), "90045689745632101", "Mr",
                "0315689741", "", "0725698741", "stewart@smith.com", "123 Park lane", "", "Appleville", "Durban", "1154");

            itcRequest = new ItcRequest();
            itcResponse = new ItcResponse("", "", DateTime.Now, ServiceResponseStatus.Success, XDocument.Parse("<request></request>"), XDocument.Parse("<response></response>"));
            transUnionService.WhenToldTo(x => x.PerformRequest(itcRequest)).Return(itcResponse);
            messages = SystemMessageCollection.Empty();
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

        private It should_not_log_itc_request_or_response = () =>
        {
            itcManager.WasNotToldTo(x => x.LogFailedITCRequestAndResponse(Param.IsAny<ItcRequest>(), Param.IsAny<ItcResponse>(), Param.IsAny<string>()));
        };

        private It should_not_return_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
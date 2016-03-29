using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.Runtime.Serialization.Formatters;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_logging_a_failed_itc_request_and_response : WithITCManager
    {
        private static ItcResponse itcResponse;
        private static string callingMethod;
        private static string serializedRequest;
        private static string serializedResponse;

        private Establish context = () =>
        {
            itcRequest = new ItcRequest
            {
                Forename1 = "Bob",
                Surname = "Smith",
                IdentityNo1 = "9255648993541"
            };
            itcResponse = new ItcResponse("", "", new DateTime(2014, 12, 22), ServiceResponseStatus.Success,
                XDocument.Parse("<request></request>"), XDocument.Parse("<response></response>"));
            callingMethod = "ICalledThis";

            var serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ContractResolver = new DefaultContractResolver()
            };
            serializedRequest = JsonConvert.SerializeObject(itcRequest, serializerSettings);
            serializedResponse = JsonConvert.SerializeObject(itcResponse, serializerSettings);
        };

        private Because of = () =>
        {
            itcManager.LogFailedITCRequestAndResponse(itcRequest, itcResponse, callingMethod);
        };

        private It should_log_the_itc_request = () =>
        {
            logger.WasToldTo(x => x.LogError(loggerSource, "System", callingMethod, Param<string>.Matches(m => m.Contains(serializedRequest)), null));
        };

        private It should_log_the_itc_response = () =>
        {
            logger.WasToldTo(x => x.LogError(loggerSource, "System", callingMethod, Param<string>.Matches(m => m.Contains(serializedResponse)), null));
        };
    }
}
using System;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_saving_an_itc_check_with_application_and_client_id : WithITCManager
    {
        private static ItcResponse itcResponse;

        private Establish context = () =>
        {
            clientId = 123456;
            applicationNumber = 42;
            userId = "3501";
            itcResponse = new ItcResponse("",
                "",
                new DateTime(2014, 12, 22),
                ServiceResponseStatus.Success,
                XDocument.Parse("<request><root><response><item1>Some request stuff</item1></response></root></request>"),
                XDocument.Parse("<root><response><item1>Some stuff</item1></response></root>"));
        };

        private Because of = () =>
        {
            itcManager.SaveITC(clientId, applicationNumber, itcResponse, userId);
        };

        private It should_save_the_itc = () =>
        {
            dataManager.WasToldTo(x => x.SaveITC(clientId,
                applicationNumber,
                itcResponse.ITCDate,
                itcResponse.Request.Root.ToString(),
                itcResponse.Response.Root.ToString(),
                itcResponse.ResponseStatus.ToString(),
                userId));
        };
    }
}

using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_saving_an_itc_check_without_application_and_client_id : WithITCManager
    {
        private static Guid itcID;
        private static ItcResponse itcResponse;

        private Establish context = () =>
        {
            itcID = Guid.Parse("{ED147936-0137-4243-8A92-6910C656E131}");
            itcResponse = new ItcResponse("", "", new DateTime(2014, 12, 22), ServiceResponseStatus.Success,
                XDocument.Parse("<request></request>"), XDocument.Parse("<root><response><item1>Some stuff</item1></response></root>"));
        };

        private Because of = () =>
        {
            itcManager.SaveITC(itcID, itcResponse);
        };

        private It should_save_the_itc = () =>
        {
            dataManager.WasToldTo(x => x.SaveITC(itcID, itcResponse.ITCDate, itcResponse.Response.Root.ToString()));
        };
    }
}
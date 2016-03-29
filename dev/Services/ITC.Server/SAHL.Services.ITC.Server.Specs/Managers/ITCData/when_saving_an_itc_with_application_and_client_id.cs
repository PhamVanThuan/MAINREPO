using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ITC.Managers.Itc;
using System;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITCData
{
    public class when_saving_an_itc_with_application_and_client_id : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static ItcDataManager dataManager;
        private static Guid itcID;
        private static DateTime itcDate;
        private static int clientId, applicationNumber;
        private static string itcRequestXML, itcResponseXML, responseStatus, userId;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ItcDataManager(dbFactory);

            clientId = 10001;
            applicationNumber = 11101;
            itcID = Guid.Parse("{80116B9C-2DF4-4F3C-93CA-22049E5A81B5}");
            itcDate = new DateTime(2014, 12, 22);
            responseStatus = "";
            userId = "3501";
            itcRequestXML = "<itc>this is the request</itc>";
            itcResponseXML = "<itc>this is the response</itc>";
        };

        private Because of = () =>
        {
            dataManager.SaveITC(clientId, applicationNumber, itcDate, itcRequestXML, itcResponseXML, responseStatus, userId);
        };

        private It should_insert_an_itc_record = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<ITCDataModel>.Matches(m =>
                m.LegalEntityKey == clientId &&
                m.AccountKey == applicationNumber &&
                m.ChangeDate == itcDate &&
                 m.UserID == userId &&
                m.ResponseXML == itcResponseXML)));
        };

    }
}
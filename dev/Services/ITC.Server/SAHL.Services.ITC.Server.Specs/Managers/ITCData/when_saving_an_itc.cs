using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ITC.Managers.Itc;
using System;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITCData
{
    public class when_saving_an_itc : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static ItcDataManager dataManager;
        private static Guid itcID;
        private static DateTime itcDate;
        private static string itcData;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ItcDataManager(dbFactory);

            itcID = Guid.Parse("{80116B9C-2DF4-4F3C-93CA-22049E5A81B5}");
            itcDate = new DateTime(2014, 12, 22);
            itcData = "This is a person's ITC";
        };

        private Because of = () =>
        {
            dataManager.SaveITC(itcID, itcDate, itcData);
        };

        private It should_insert_an_itc_record = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<ITCRequestDataModel>.Matches(m =>
                m.Id == itcID &&
                m.ITCData == itcData &&
                m.ITCDate == itcDate)));
        };
    }
}
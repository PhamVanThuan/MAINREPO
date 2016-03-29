using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ITC.Managers.Itc;
using System;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITCData
{
    public class when_getting_an_itc : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static ItcDataManager dataManager;
        private static Guid itcID;
        private static ITCRequestDataModel result;
        private static ITCRequestDataModel itcData;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ItcDataManager(dbFactory);
            itcID = Guid.Parse("{1393E299-C48D-4399-9C55-E7384A275524}");
            itcData = new ITCRequestDataModel(itcID, new DateTime(2014, 12, 23), "<request></request>");
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.GetByKey<ITCRequestDataModel, Guid>(itcID))
                                                    .Return(itcData);
        };

        private Because of = () =>
        {
            result = dataManager.GetITCByID(itcID);
        };

        private It should_get_the_itc = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.GetByKey<ITCRequestDataModel, Guid>(itcID));
        };

        private It should_return_the_itc = () =>
        {
            result.ShouldMatch(x =>
                x.Id == itcData.Id &&
                x.ITCData == itcData.ITCData &&
                x.ITCDate == itcData.ITCDate);
        };
    }
}
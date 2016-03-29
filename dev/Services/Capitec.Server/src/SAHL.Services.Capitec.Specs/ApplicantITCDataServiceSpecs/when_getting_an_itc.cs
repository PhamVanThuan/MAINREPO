using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.ITC;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantITCDataServiceSpecs
{
    public class when_getting_an_itc : WithCoreFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static ITCDataManager dataManager;
        private static Guid itcID;
        private static ITCDataModel ItcDataModel;
        private static ITCDataModel result;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ITCDataManager(fakeDbFactory);
            itcID = Guid.Parse("{4BD4D77F-39DE-449C-9BA9-0D46EBFDB24D}");
            ItcDataModel = new ITCDataModel(itcID, new DateTime(2015, 01, 09), "ITC");
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.GetByKey<ITCDataModel, Guid>(itcID))
                .Return(ItcDataModel);
        };

        private Because of = () =>
        {
            result = dataManager.GetItcById(itcID);
        };

        private It should_query_for_the_itcs_belonging_to_that_person = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.GetByKey<ITCDataModel, Guid>(itcID));
        };

        private It should_return_the_result = () =>
        {
            result.ShouldMatch(m =>
                m.ITCData == ItcDataModel.ITCData &&
                m.Id == ItcDataModel.Id &&
                m.ITCDate == ItcDataModel.ITCDate);
        };
    }
}
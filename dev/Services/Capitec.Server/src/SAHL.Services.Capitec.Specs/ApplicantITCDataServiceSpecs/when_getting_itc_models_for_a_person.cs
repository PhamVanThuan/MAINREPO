using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.ITC.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ApplicantITCDataServiceSpecs
{
    public class when_getting_itc_models_for_a_person : WithCoreFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static ITCDataManager dataManager;
        private static string identityNumber;
        private static ITCDataModel itcDataModel;
        private static List<ITCDataModel> result;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ITCDataManager(fakeDbFactory);
            identityNumber = "1234567890123";
            itcDataModel = new ITCDataModel(Guid.Parse("{44A858CA-5BA4-4AEF-915C-52FD8A9106CF}"), new DateTime(2015, 01, 09), "Itc");
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.Select(Param.IsAny<GetItcsForIdentityNumberStatement>()))
                .Return(new List<ITCDataModel> { itcDataModel });
        };

        private Because of = () =>
        {
            result = dataManager.GetItcModelsForPerson(identityNumber);
        };

        private It should_query_for_the_itcs_belonging_to_that_person = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(
                Param<GetItcsForIdentityNumberStatement>.Matches(m => m.IdentityNumber == identityNumber)));
        };

        private It should_return_the_result = () =>
        {
            result.First().ShouldMatch(m =>
                m.Id == itcDataModel.Id &&
                m.ITCData == itcDataModel.ITCData &&
                m.ITCDate == itcDataModel.ITCDate);
        };
    }
}
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
    public class when_getting_a_person_itc : WithCoreFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static ITCDataManager dataManager;
        private static Guid personID;
        private static PersonITCDataModel personItcDataModel;
        private static PersonITCDataModel result;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ITCDataManager(fakeDbFactory);
            personID = Guid.Parse("{4BD4D77F-39DE-449C-9BA9-0D46EBFDB24D}");
            personItcDataModel = new PersonITCDataModel(personID, Guid.Parse("{0FF05A81-EE28-40AC-934D-198A42465664}"), new DateTime(2015, 01, 09));
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.GetByKey<PersonITCDataModel, Guid>(Param.IsAny<Guid>()))
                .Return(personItcDataModel);
        };

        private Because of = () =>
        {
            result = dataManager.GetPersonITC(personID);
        };

        private It should_query_for_the_itcs_belonging_to_that_person = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.GetByKey<PersonITCDataModel, Guid>(personID));
        };

        private It should_return_the_result = () =>
        {
            result.ShouldMatch(m =>
                m.CurrentITCId == personItcDataModel.CurrentITCId &&
                m.Id == personItcDataModel.Id &&
                m.ITCDate == personItcDataModel.ITCDate);
        };
    }
}
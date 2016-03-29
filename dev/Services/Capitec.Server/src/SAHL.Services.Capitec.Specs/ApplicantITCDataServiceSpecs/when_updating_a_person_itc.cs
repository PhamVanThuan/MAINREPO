using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.ITC;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantITCDataServiceSpecs
{
    public class when_updating_a_person_itc : WithCoreFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static ITCDataManager dataManager;
        private static Guid personID, itcID;
        private static DateTime itcDate;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ITCDataManager(fakeDbFactory);
            itcID = Guid.Parse("{44A858CA-5BA4-4AEF-915C-52FD8A9106CF}");
            personID = Guid.Parse("{F25C63ED-4B29-4936-A71E-20F1FDD5EA06}");
            itcDate = new DateTime(2015, 01, 09);
        };

        private Because of = () =>
        {
            dataManager.UpdatePersonItc(personID, itcID, itcDate);
        };

        private It should_update_the_person_itc = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Update(Param<PersonITCDataModel>.Matches(m =>
                m.CurrentITCId == itcID &&
                m.Id == personID &&
                m.ITCDate == itcDate)));
        };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_getting_the_unique_id_for_a_person : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static string idNumber;
        private static PersonDataModel person;
        private static Guid result;

        Establish context = () =>
        {
            idNumber = "8001015000100";
            person = new PersonDataModel(Guid.NewGuid(), "Bob", "Smith", idNumber);
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetPersonFromIdentityNumberQuery>())).Return(person);
        };

        Because of = () =>
        {
            result = service.GetPersonID(idNumber);
        };

        It should_return_the_person_id = () =>
        {
            result.ShouldEqual(person.Id);
        };
    }
}
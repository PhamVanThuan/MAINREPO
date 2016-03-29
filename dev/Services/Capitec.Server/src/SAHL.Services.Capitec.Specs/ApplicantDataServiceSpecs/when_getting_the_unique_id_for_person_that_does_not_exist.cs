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
    class when_getting_the_unique_id_for_person_that_does_not_exist : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static string idNumber;
        private static PersonDataModel person;
        private static Guid result;
        private static Exception ex;

        Establish context = () =>
        {
            idNumber = "8001015000100";
            person = null;
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetPersonFromIdentityNumberQuery>())).Return(person);
        };

        Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                result = service.GetPersonID(idNumber);
            });
        };

        It should_throw_a_null_reference_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(NullReferenceException));
        };

        It should_return_a_custom_exception_message = () =>
        {
            ex.Message.ShouldEqual("person does not exist");
        };
    }
}

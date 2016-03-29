using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_checking_if_a_person_exists_and_they_do_not : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static string idNumber;
        private static PersonDataModel person;
        private static bool result;

        private Establish context = () =>
        {
            idNumber = "8001015000100";
            person = null;
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetPersonFromIdentityNumberQuery>())).Return(person);
        };

        private Because of = () =>
        {
            result = service.DoesPersonExist(idNumber);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_adding_a_person : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid personId, salutationEnumId;
        private static string firstName, surname, idNumber;

        Establish context = () =>
        {
            personId = Guid.NewGuid();
            salutationEnumId = Guid.NewGuid();
            firstName = "Clint";
            surname = "Speed";
            idNumber = "8211045229080";
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
        };

        Because of = () =>
        {
            service.AddPerson(personId, salutationEnumId, firstName, surname, idNumber);
        };

        It should_insert_a_person_with_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<PersonDataModel>(y => y.Id == personId 
                && y.SalutationEnumId == salutationEnumId
                && y.FirstName == firstName
                && y.Surname == surname
                && y.IdentityNumber == idNumber
                )));
        };
    }
}
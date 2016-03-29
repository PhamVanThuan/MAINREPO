using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using System;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_adding_a_person : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static ILookupManager lookupService;
        private static ApplicantManager applicantService;
        private static Guid personID;
        private static string firstName;
        private static string surname;
        private static string identityNumber;
        private static Guid salutationEnumID;

        private Establish context = () =>
        {
            applicantDataManager = An<IApplicantDataManager>();
            personID = Guid.NewGuid();
            firstName = "Bob";
            surname = "Smith";
            identityNumber = "1234567890123";
            salutationEnumID = Guid.Parse(SalutationEnumDataModel.LORD);

            applicantService = new ApplicantManager(applicantDataManager, An<IContactDetailDataManager>(), lookupService, An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), An<IAddressDataManager>());
        };

        private Because of = () =>
        {
            applicantService.AddPerson(personID, salutationEnumID, firstName, surname, identityNumber);
        };

        private It should_store_the_person = () =>
        {
            applicantDataManager.WasToldTo(x => x.AddPerson(personID, salutationEnumID, firstName, surname, identityNumber));
        };
    }
}
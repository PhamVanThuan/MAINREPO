using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using System;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs.CalculateAgeSpecs
{
    public class when_the_dob_is_today : WithFakes
    {
        private static DateTime? birthDate;
        private static IApplicantManager applicantService;
        private static IApplicantDataManager applicantDataService;
        private static IContactDetailDataManager contactDetailDataService;
        private static ILookupManager lookupService;
        private static IDeclarationDataManager declarationDataService;
        private static IAddressDataManager addressDataService;
        private static IEmploymentDataManager employmentDataService;
        private static int age;

        private Establish context = () =>
        {
            applicantDataService = An<IApplicantDataManager>();
            contactDetailDataService = An<IContactDetailDataManager>();
            lookupService = An<ILookupManager>();
            declarationDataService = An<IDeclarationDataManager>();
            addressDataService = An<IAddressDataManager>();
            employmentDataService = An<IEmploymentDataManager>();
            birthDate = DateTime.Now.AddYears(-25);
            applicantService = new ApplicantManager(applicantDataService, contactDetailDataService, lookupService, declarationDataService, employmentDataService, addressDataService);
        };

        private Because of = () =>
        {
            age = applicantService.CalculateAge(birthDate, DateTime.Now);
        };

        private It should_return_the_correct_age = () =>
        {
            age.ShouldEqual(25);
        };
    }
}
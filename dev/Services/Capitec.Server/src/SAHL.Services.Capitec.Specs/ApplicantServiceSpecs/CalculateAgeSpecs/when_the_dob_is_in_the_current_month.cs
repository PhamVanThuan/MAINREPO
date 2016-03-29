using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using System;
using System.Globalization;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs.CalculateAgeSpecs
{
    public class when_the_dob_is_in_the_current_month : WithFakes
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
        private static DateTime now;

        private Establish context = () =>
        {
            applicantDataService = An<IApplicantDataManager>();
            contactDetailDataService = An<IContactDetailDataManager>();
            lookupService = An<ILookupManager>();
            declarationDataService = An<IDeclarationDataManager>();
            addressDataService = An<IAddressDataManager>();
            employmentDataService = An<IEmploymentDataManager>();
            birthDate = DateTime.ParseExact("19800515", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            now = DateTime.ParseExact("20140514", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            applicantService = new ApplicantManager(applicantDataService, contactDetailDataService, lookupService, declarationDataService, employmentDataService, addressDataService);
        };

        private Because of = () =>
        {
            age = applicantService.CalculateAge(birthDate, now);
        };

        private It should_return_the_correct_age = () =>
        {
            age.ShouldEqual(33);
        };
    }
}
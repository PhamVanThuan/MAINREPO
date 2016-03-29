using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_adding_employment_details_for_unemployed_applicant : WithFakes
    {
        private static ApplicantManager applicantService;
        private static Guid applicantId;
        private static ApplicantEmploymentDetails employmentDetails;
        private static ILookupManager lookupService;
        private static IEmploymentDataManager employmentDataService;
        private static Guid employmentId;

        private static UnEmployedDetails unEmployedDetais;
        private static decimal grossMonthlyIncome;

        private Establish context = () =>
            {
                employmentId = Guid.NewGuid();
                employmentDataService = An<IEmploymentDataManager>();
                lookupService = An<ILookupManager>();
                applicantId = Guid.NewGuid();
                applicantService = new ApplicantManager(An<IApplicantDataManager>(), An<IContactDetailDataManager>(), lookupService, An<IDeclarationDataManager>(), employmentDataService, An<IAddressDataManager>());
                grossMonthlyIncome = 0;

                unEmployedDetais = new UnEmployedDetails(0);

                employmentDetails = new ApplicantEmploymentDetails(Guid.NewGuid(), null, null, null, null, unEmployedDetais);
            };

        private Because of = () =>
            {
                applicantService.AddEmploymentForApplicant(applicantId, employmentDetails);
            };

        private It should_store_employmentdetails_for_the_applicant = () =>
            {
                employmentDataService.WasToldTo(x => x.AddApplicantEmployment(applicantId, employmentDetails.EmploymentTypeEnumId, grossMonthlyIncome, 0, 0));
            };
    }
}
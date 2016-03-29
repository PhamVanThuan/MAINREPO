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
    public class when_adding_employment_details_for_salaried_with_commission_applicant : WithFakes
    {
        private static ApplicantManager applicantService;
        private static Guid applicantID;
        private static ApplicantEmploymentDetails employment;
        private static ILookupManager lookupService;
        private static IEmploymentDataManager employmentDataService;
        private static Guid employmentID;

        private static decimal grossMonthlyIncome;
        private static SalariedWithCommissionDetails salariedwithCommissionDetails;
        private static decimal threeMonthAverageCommission;

        private Establish context = () =>
        {
            applicantID = Guid.NewGuid();
            employmentID = Guid.NewGuid();

            employmentDataService = An<IEmploymentDataManager>();
            lookupService = An<ILookupManager>();
            applicantService = new ApplicantManager(An<IApplicantDataManager>(), An<IContactDetailDataManager>(), lookupService, An<IDeclarationDataManager>(), employmentDataService, An<IAddressDataManager>());

            grossMonthlyIncome = 20000;
            threeMonthAverageCommission = 5000;

            salariedwithCommissionDetails = new SalariedWithCommissionDetails(grossMonthlyIncome, threeMonthAverageCommission);

            employment = new ApplicantEmploymentDetails(Guid.NewGuid(), null, salariedwithCommissionDetails, null, null, null);
        };

        private Because of = () =>
        {
            applicantService.AddEmploymentForApplicant(applicantID, employment);
        };

        private It should_store_employment_for_the_applicant = () =>
        {
            employmentDataService.WasToldTo(x => x.AddApplicantEmployment(applicantID, employment.EmploymentTypeEnumId, grossMonthlyIncome, threeMonthAverageCommission, 0));
        };
    }
}
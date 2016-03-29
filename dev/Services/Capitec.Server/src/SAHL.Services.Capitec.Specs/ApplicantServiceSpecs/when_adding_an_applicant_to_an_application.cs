using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using System;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_adding_an_applicant_to_an_application : WithFakes
    {
        private static IApplicantDataManager applicantDataService;
        private static ILookupManager lookupService;
        private static ApplicantManager applicantService;
        private static Guid applicantID;
        private static Guid applicationID;

        private Establish context = () =>
        {
            applicantDataService = An<IApplicantDataManager>();
            applicantID = Guid.NewGuid();
            applicationID = Guid.NewGuid();
            applicantService = new ApplicantManager(applicantDataService, An<IContactDetailDataManager>(), lookupService, An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), An<IAddressDataManager>());
        };

        private Because of = () =>
        {
            applicantService.AddApplicantToApplication(applicantID, applicationID);
        };

        private It should_store_the_applicant_against_the_application = () =>
        {
            applicantDataService.WasToldTo(x => x.AddApplicantToApplication(applicationID, applicantID));
        };
    }
}
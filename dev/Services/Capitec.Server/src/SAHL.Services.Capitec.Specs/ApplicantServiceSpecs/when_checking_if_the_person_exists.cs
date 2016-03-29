using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_checking_if_the_person_exists : WithFakes
    {
        private static ApplicantManager applicantService;
        private static IApplicantDataManager dataManager;
        private static string identityNumber;

        private Establish context = () =>
            {
                dataManager = An<IApplicantDataManager>();
                applicantService = new ApplicantManager(dataManager, An<IContactDetailDataManager>(), An<ILookupManager>(), An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), An<IAddressDataManager>());
                identityNumber = "1234567890123";
            };

        private Because of = () =>
            {
                applicantService.DoesPersonExist(identityNumber);
            };

        private It should_check_if_the_person_exists = () =>
            {
                dataManager.WasToldTo(x => x.DoesPersonExist(identityNumber));
            };
    }
}
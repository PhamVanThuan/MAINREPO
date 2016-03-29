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
    public class when_getting_a_person_by_their_identity_number : WithFakes
    {
        private static ApplicantManager applicantService;
        private static IApplicantDataManager dataService;
        private static string identityNumber;

        private Establish context = () =>
            {
                dataService = An<IApplicantDataManager>();
                applicantService = new ApplicantManager(dataService, An<IContactDetailDataManager>(), An<ILookupManager>(), An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), An<IAddressDataManager>());
                identityNumber = "1234567890123";
            };

        private Because of = () =>
            {
                applicantService.GetPersonIDFromIdentityNumber(identityNumber);
            };

        private It should_get_the_id_of_the_existing_person = () =>
            {
                dataService.WasToldTo(x => x.GetPersonID(identityNumber));
            };
    }
}
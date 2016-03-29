using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using System;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_told_to_add_an_applicant : WithFakes
    {
        private static IApplicantDataManager applicantDataService;
        private static ILookupManager lookupService;
        private static ApplicantManager applicantService;
        private static Guid applicantID;
        private static Guid personID;
        private static bool mainContact;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            applicantDataService = An<IApplicantDataManager>();
            applicantID = Guid.NewGuid();
            personID = Guid.NewGuid();
            mainContact = true;
            applicantService = new ApplicantManager(applicantDataService, An<IContactDetailDataManager>(), lookupService, An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), An<IAddressDataManager>());
        };

        private Because of = () =>
        {
            applicantService.SaveApplicant(applicantID, personID, mainContact);
        };

        private It should_store_the_applicant = () =>
        {
            applicantDataService.WasToldTo(x => x.AddApplicant(applicantID, personID, mainContact));
        };
    }
}
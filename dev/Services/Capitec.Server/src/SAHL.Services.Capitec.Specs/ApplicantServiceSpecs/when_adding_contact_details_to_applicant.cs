using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
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
    public class when_adding_contact_details_to_applicant : WithFakes
    {
        private static ApplicantManager applicantService;
        private static Guid applicantID;
        private static ApplicantInformation applicantInformation;
        private static ILookupManager lookupService;
        private static IContactDetailDataManager contactDetailDataService;
        private static Guid contactDetailID;

        private Establish context = () =>
        {
            contactDetailID = Guid.NewGuid();
            lookupService = An<ILookupManager>();
            contactDetailDataService = An<IContactDetailDataManager>();
            applicantService = new ApplicantManager(An<IApplicantDataManager>(), contactDetailDataService, lookupService, An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), An<IAddressDataManager>());

            applicantID = Guid.NewGuid();
            applicantInformation = new ApplicantInformation(string.Empty, string.Empty, string.Empty, Guid.NewGuid(), "0315771122", "0315771122", "0315771122", "email@address.com", DateTime.MinValue, "Mr", true);

            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(contactDetailID);
        };

        private Because of = () =>
        {
            applicantService.AddContactDetailsForApplicant(applicantID, applicantInformation);
        };

        private It should_add_email_address_contact_detail = () =>
        {
            contactDetailDataService.WasToldTo(x => x.AddEmailAddressContactDetail(contactDetailID,
                Guid.Parse(EmailAddressContactDetailTypeEnumDataModel.HOME), applicantInformation.EmailAddress));
        };

        private It should_add_phone_number_contact_detail = () =>
        {
            contactDetailDataService.WasToldTo(x => x.AddPhoneNumberContactDetail(contactDetailID, Guid.Parse(PhoneNumberContactDetailTypeEnumDataModel.WORK), null, applicantInformation.WorkPhoneNumber));
        };

        private It should_add_cellphone_number_contact_detail = () =>
        {
            contactDetailDataService.WasToldTo(x => x.AddPhoneNumberContactDetail(contactDetailID, Guid.Parse(PhoneNumberContactDetailTypeEnumDataModel.MOBILE), null, applicantInformation.WorkPhoneNumber));
        };

        private It should_only_add_two_phone_contact_details = () =>
        {
            contactDetailDataService.WasToldTo(x => x.AddPhoneNumberContactDetail(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<string>(), Param.IsAny<string>())).Times(2);
        };

        private It should_link_the_contact_detail_to_the_applicant = () =>
        {
            contactDetailDataService.WasToldTo(x => x.LinkContactDetailToApplicant(applicantID, contactDetailID));
        };
    }
}
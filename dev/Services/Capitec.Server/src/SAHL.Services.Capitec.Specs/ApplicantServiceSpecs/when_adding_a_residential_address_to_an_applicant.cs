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
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_adding_a_residential_address_to_an_applicant : WithFakes
    {
        private static ApplicantManager applicantService;
        private static IApplicantDataManager dataManager;
        private static ILookupManager lookupService;
        private static IAddressDataManager addressDataService;
        private static Guid applicantID;
        private static Guid addressID;
        private static Guid addressTypeID;
        private static ApplicantResidentialAddress applicantResidentialAddress;

        private Establish context = () =>
        {
            dataManager = An<IApplicantDataManager>();
            lookupService = An<ILookupManager>();
            addressDataService = An<IAddressDataManager>();
            applicantService = new ApplicantManager(dataManager, An<IContactDetailDataManager>(), lookupService, An<IDeclarationDataManager>(), An<IEmploymentDataManager>(), addressDataService);
            applicantID = Guid.NewGuid();
            addressID = Guid.NewGuid();
            addressTypeID = Guid.Parse(AddressTypeEnumDataModel.RESIDENTIAL);
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(addressID);
            applicantResidentialAddress = new ApplicantResidentialAddress("unit", "buildingNumber", "buildingName", "streetNumber", "streetName", "suburb", "province", "city", "postalCode", Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicantService.AddResidentialAddressForApplicant(applicantID, applicantResidentialAddress);
        };

        private It should_save_the_address = () =>
        {
            addressDataService.AddResidentialAddress(addressID, applicantResidentialAddress.UnitNumber, applicantResidentialAddress.BuildingNumber, applicantResidentialAddress.BuildingName, applicantResidentialAddress.StreetNumber, applicantResidentialAddress.StreetName, applicantResidentialAddress.Province, applicantResidentialAddress.SuburbId, applicantResidentialAddress.City, applicantResidentialAddress.PostalCode);
        };

        private It should_add_the_address_to_the_applicant = () =>
        {
            dataManager.WasToldTo(x => x.AddAddressToApplicant(addressID, applicantID, addressTypeID));
        };
    }
}
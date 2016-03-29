using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Capitec.Managers.Applicant
{
    public class ApplicantManager : IApplicantManager
    {
        private IApplicantDataManager applicantDataService;
        private IContactDetailDataManager contactDetailDataService;
        private IDeclarationDataManager declarationDataService;
        private IEmploymentDataManager employmentDataService;
        private IAddressDataManager addressDataService;
        private ILookupManager lookupService;

        public ApplicantManager(IApplicantDataManager applicantDataService, IContactDetailDataManager contactDetailDataService, ILookupManager lookupService, IDeclarationDataManager declarationDataService, IEmploymentDataManager employmentDataService, IAddressDataManager addressDataService)
        {
            this.applicantDataService = applicantDataService;
            this.contactDetailDataService = contactDetailDataService;
            this.declarationDataService = declarationDataService;
            this.lookupService = lookupService;
            this.employmentDataService = employmentDataService;
            this.addressDataService = addressDataService;
        }

        public bool DoesPersonExist(string identityNumber)
        {
            return this.applicantDataService.DoesPersonExist(identityNumber);
        }

        public Guid GetPersonIDFromIdentityNumber(string identityNumber)
        {
            return this.applicantDataService.GetPersonID(identityNumber);
        }

        public void SaveApplicant(Guid applicantID, Guid personID, bool mainContact)
        {
            this.applicantDataService.AddApplicant(applicantID, personID, mainContact);
        }

        public void AddPerson(Guid personID, Guid salutationEnumId, string firstName, string surname, string identityNumber)
        {
            this.applicantDataService.AddPerson(personID, salutationEnumId, firstName, surname, identityNumber);
        }

        public void AddContactDetailsForApplicant(Guid applicantID, ApplicantInformation applicantInformation)
        {
            AddApplicantEmailAddressContactDetail(applicantID, Guid.Parse(EmailAddressContactDetailTypeEnumDataModel.HOME), applicantInformation.EmailAddress);
            AddApplicantPhoneNumberContactDetail(applicantID, Guid.Parse(PhoneNumberContactDetailTypeEnumDataModel.MOBILE), applicantInformation.CellPhoneNumber);
            AddApplicantPhoneNumberContactDetail(applicantID, Guid.Parse(PhoneNumberContactDetailTypeEnumDataModel.WORK), applicantInformation.WorkPhoneNumber);
        }

        public void AddDeclarationsForApplicant(Guid applicantID, Interfaces.Capitec.ViewModels.Apply.ApplicantDeclarations declarations)
        {
            AddDeclarationForApplicant(applicantID, declarations.DeclarationsDate, DeclarationDefinitions.AllowCreditBureauCheck, declarations.AllowCreditBureauCheck);
            AddDeclarationForApplicant(applicantID, declarations.DeclarationsDate, DeclarationDefinitions.HasCapitecTransactionAccount, declarations.HasCapitecTransactionAccount);
            AddDeclarationForApplicant(applicantID, declarations.DeclarationsDate, DeclarationDefinitions.MarriedInCommunityOfProperty, declarations.MarriedInCommunityOfProperty);
            AddDeclarationForApplicant(applicantID, declarations.DeclarationsDate, DeclarationDefinitions.IncomeContributor, declarations.IncomeContributor);
        }

        public void AddEmploymentForApplicant(Guid applicantID, Interfaces.Capitec.ViewModels.Apply.ApplicantEmploymentDetails employmentDetails)
        {
            this.employmentDataService.AddApplicantEmployment(applicantID, employmentDetails.EmploymentTypeEnumId, employmentDetails.BasicMonthlyIncome(), employmentDetails.ThreeMonthAverageCommission(), employmentDetails.HousingAllowance());
        }

        public void AddResidentialAddressForApplicant(Guid applicantID, ApplicantResidentialAddress residentialAddress)
        {
            var addressID = lookupService.GenerateCombGuid();
            addressDataService.AddResidentialAddress(addressID, residentialAddress.UnitNumber, residentialAddress.BuildingNumber, residentialAddress.BuildingName,
                residentialAddress.StreetNumber, residentialAddress.StreetName, residentialAddress.Province, residentialAddress.SuburbId, residentialAddress.City, residentialAddress.PostalCode);
            var addressTypeID = Guid.Parse(AddressTypeEnumDataModel.RESIDENTIAL);
            applicantDataService.AddAddressToApplicant(addressID, applicantID, addressTypeID);
        }

        public void AddApplicantToApplication(Guid applicantID, Guid applicationID)
        {
            this.applicantDataService.AddApplicantToApplication(applicationID, applicantID);
        }

        private void AddApplicantEmailAddressContactDetail(Guid applicantID, Guid emailAddressContactDetailType, string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress)) { return; }
            var id = this.lookupService.GenerateCombGuid();
            this.contactDetailDataService.AddEmailAddressContactDetail(id, emailAddressContactDetailType, emailAddress);
            this.contactDetailDataService.LinkContactDetailToApplicant(applicantID, id);
        }

        private void AddApplicantPhoneNumberContactDetail(Guid applicantID, Guid phoneNumberContactDetailTypeEnumId, string contactDetailNumber)
        {
            if (string.IsNullOrEmpty(contactDetailNumber)) { return; }
            var id = this.lookupService.GenerateCombGuid();
            this.contactDetailDataService.AddPhoneNumberContactDetail(id, phoneNumberContactDetailTypeEnumId, null, contactDetailNumber);
            this.contactDetailDataService.LinkContactDetailToApplicant(applicantID, id);
        }

        private void AddDeclarationForApplicant(Guid applicantID, DateTime declarationDate, string declarationText, Guid declarationTypeEnumId)
        {
            var declarationDefinition = this.declarationDataService.GetDeclarationDefinition(declarationTypeEnumId, declarationText);
            var declarationID = this.lookupService.GenerateCombGuid();
            this.declarationDataService.AddDeclaration(declarationID, declarationDefinition, declarationDate);
            this.applicantDataService.AddDeclarationToApplicant(applicantID, declarationID);
        }

        public int CalculateAge(DateTime? birthDate, DateTime now)
        {
            if (!birthDate.HasValue)
                return 0;
            int age = now.Year - birthDate.Value.Year;
            if (now.Month < birthDate.Value.Month || (now.Month == birthDate.Value.Month && now.Day < birthDate.Value.Day)) age--;
            return age;
        }
    }
}
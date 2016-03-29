using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Capitec.Managers.Applicant
{
    public interface IApplicantManager
    {
        void SaveApplicant(Guid applicantID, Guid personID, bool mainContact);

        void AddContactDetailsForApplicant(Guid applicantID, ApplicantInformation applicantInformation);

        void AddDeclarationsForApplicant(Guid applicantID, ApplicantDeclarations declarations);

        void AddEmploymentForApplicant(Guid applicantID, ApplicantEmploymentDetails employmentDetails);

        bool DoesPersonExist(string identityNumber);

        Guid GetPersonIDFromIdentityNumber(string identityNumber);

        void AddPerson(Guid personID, Guid salutationEnumId, string firstName, string surname, string identityNumber);

        void AddResidentialAddressForApplicant(Guid applicantID, ApplicantResidentialAddress residentialAddress);

        void AddApplicantToApplication(Guid applicantID, Guid applicationID);

        int CalculateAge(DateTime? birthDate, DateTime now);
    }
}
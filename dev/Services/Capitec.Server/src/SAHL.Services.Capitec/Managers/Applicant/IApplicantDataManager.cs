using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.Applicant
{
    public interface IApplicantDataManager
    {
        Guid GetDeclarationForApplicant(Guid applicantID, string declarationText);

        void AddApplicantToApplication(Guid applicationID, Guid applicantID);

        void AddAddressToApplicant(Guid addressID, Guid applicantID, Guid addressType);

        void AddDeclarationToApplicant(Guid applicantID, Guid declarationID);

        bool DidApplicantAnswerYesToDeclaration(Guid applicantID, string declarationText);

        void AddApplicant(Guid applicantID, Guid personID, bool mainContact);

        void AddPerson(Guid personID, Guid salutationEnumId, string firstName, string surname, string identityNumber);

        bool DoesPersonExist(string identityNumber);

        Guid GetPersonID(string identityNumber);
    }
}

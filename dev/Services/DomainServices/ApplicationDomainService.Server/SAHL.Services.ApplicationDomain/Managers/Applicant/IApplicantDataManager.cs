using SAHL.Core.Data.Models._2AM;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant
{
    public interface IApplicantDataManager
    {
        int SaveApplicant(LegalEntityDataModel legalEntity);

        void UpdateApplicant(LegalEntityDataModel legalEntity);

        LegalEntityDataModel GetApplicantByIDNumber(string idnumber);

        int AddApplicantRole(OfferRoleDataModel offerRole);

        bool DoesBankAccountBelongToApplicantOnApplication(int applicationNumber, int clientBankAccountKey);

        int LinkDomiciliumAddressToApplicant(OfferRoleDomiciliumDataModel offerRoleDomiciliumDataModel);

        bool CheckClientIsAnApplicantOnTheApplication(int clientKey, int applicationNumber);

        IEnumerable<OfferRoleDataModel> GetActiveClientRoleOnApplication(int applicationNumber, int clientKey);

        void SaveApplicantDeclarations(IEnumerable<OfferDeclarationDataModel> offerDeclarations);

        IEnumerable<EmploymentDataModel> GetIncomeContributorApplicantsCurrentEmployment(int applicationNumber);

        bool DoesClientAddressBelongToApplicant(int clientAddressKey, int applicationNumber);

        OfferRoleDataModel GetActiveApplicationRole(int applicationNumber, int clientKey);

        bool DoesApplicantHavePendingDomiciliumOnApplication(int ClientKey, int ApplicationNumber);

        void AddOfferRoleAttribute(OfferRoleAttributeDataModel offerRoleAttribute);

        bool ApplicantHasCurrentSAHLBusiness(int applicantKey);

        IEnumerable<OfferRoleAttributeDataModel> GetApplicantAttributes(int applicationRoleKey);

        IEnumerable<OfferDeclarationDataModel> GetApplicantDeclarations(int applicationNumber, int clientKey);

        System.DateTime? GetClientDateOfBirth(int clientKey);
    }
}

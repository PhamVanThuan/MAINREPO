using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment
{
    public interface IAffordabilityAssessmentManager
    {
        AffordabilityAssessmentModel GetAffordabilityAssessment(int affordabilityAssessmentKey);

        double? GetBondInstalmentForNewBusinessApplication(int applicationKey);

        double? GetBondInstalmentForFurtherLendingApplication(int applicationKey);

        double? GetHocInstalmentForNewBusinessApplication(int applicationKey);

        double? GetHocInstalmentForFurtherLendingApplication(int applicationKey);

        IEnumerable<AffordabilityAssessmentModel> GetApplicationAffordabilityAssessments(int applicationKey);

        int CreateAffordabilityAssessment(AffordabilityAssessmentModel affordabilityAssessment, int createdByUserId);

        int LinkLegalEntityToAffordabilityAssessment(int affordabilityAssessmentKey, int applicantLegalEntityKey);

        void CreateBlankAffordabilityAssessmentItems(int affordabilityAssessmentKey, int userKey);

        void DeleteUnconfirmedAffordabilityAssessment(int affordabilityAssessmentKey);

        void CopyAndArchiveAffordabilityAssessmentWithNewAffordabilityAssessmentItems(AffordabilityAssessmentModel affordabilityAssessment, int copiedByUserId);

        void CopyAndArchiveAffordabilityAssessmentWithNewIncomeContributors(AffordabilityAssessmentModel affordabilityAssessment, int copiedByUserId);

        void UpdateAffordabilityAssessmentAndAffordabilityAssessmentItems(AffordabilityAssessmentModel affordabilityAssessment, int updatedByUserId);

        void UpdateAffordabilityAssessmentAndIncomeContributors(AffordabilityAssessmentModel affordabilityAssessment, int updatedByUserId);

        void ConfirmAffordabilityAssessments(IEnumerable<AffordabilityAssessmentModel> affordabilityAssessmentModels);
    }
}
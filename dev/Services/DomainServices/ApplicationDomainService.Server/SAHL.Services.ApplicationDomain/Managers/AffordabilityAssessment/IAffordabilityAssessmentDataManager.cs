using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment
{
    public interface IAffordabilityAssessmentDataManager
    {
        int GetAffordabilityAssessmentStressFactorKeyByStressFactorPercentage(string percentage);

        void InsertAffordabilityAssessment(AffordabilityAssessmentDataModel affordabilityAssessmentDataModel);

        void UpdateAffordabilityAssessment(AffordabilityAssessmentDataModel affordabilityAssessmentDataModel);

        AffordabilityAssessmentSummaryModel GetAffordabilityAssessmentSummary(int affordabilityAssessmentKey);

        IEnumerable<AffordabilityAssessmentItemDataModel> GetAffordabilityAssessmentItems(int affordabilityAssessmentKey);

        IEnumerable<int> GetAffordabilityAssessmentContributors(int affordabilityAssessmentKey);

        void InsertAffordabilityAssessmentLegalEntity(AffordabilityAssessmentLegalEntityDataModel affordabilityAssessmentLegalEntityDataModel);

        void DeleteUnconfirmedAffordabilityAssessment(int affordabilityAssessmentKey);

        void InsertAffordabilityAssessmentItem(AffordabilityAssessmentItemDataModel itemDataModel);

        AffordabilityAssessmentDataModel GetAffordabilityAssessmentByKey(int affordabilityAssessmentKey);

        AffordabilityAssessmentDataModel GetLatestConfirmedAffordabilityAssessmentByGenericKey(int genericKey, int genericKeyTypeKey);

        void DeleteAffordabilityAssessmentLegalEntity(int affordabilityAssessmentKey, int legalEntityKey);

        void UpdateAffordabilityAssessmentItem(AffordabilityAssessmentItemDataModel affordabilityAssessmentItemDataModel);

        double? GetBondInstalmentForNewBusinessApplication(int applicationKey);

        double? GetBondInstalmentForFurtherLendingApplication(int applicationKey);

        double? GetHocInstalmentForNewBusinessApplication(int applicationKey);

        double? GetHocInstalmentForFurtherLendingApplication(int applicationKey);

        void ArchiveAffordabilityAssessment(int affordabilityAssessmentKey, int _ADUserKey);

        void ConfirmAffordabilityAssessment(int affordabilityAssessmentKey);

        IEnumerable<AffordabilityAssessmentDataModel> GetActiveAffordabilityAssessmentsForApplication(int applicationKey);

        Tuple<decimal, string> GetAffordabilityAssessmentStressFactorByKey(int affordabilityAssessmentStressFactorKey);
    }
}
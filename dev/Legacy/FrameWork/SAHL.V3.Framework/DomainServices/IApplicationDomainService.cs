using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;

namespace SAHL.V3.Framework.DomainServices
{
    public interface IApplicationDomainService : IV3Service
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery;

        IEnumerable<AffordabilityAssessmentSummaryModel> GetAffordabilityAssessmentsForApplication(int applicationKey);

        IEnumerable<AffordabilityAssessmentSummaryModel> GetUnconfirmedAffordabilityAssessments(int applicationKey);

        IEnumerable<AffordabilityAssessmentStressFactorModel> GetAffordabilityAssessmentStressFactors();

        IEnumerable<AffordabilityAssessmentSummaryModel> GetHistoricalAffordabilityAssessmentsForApplication(int applicationKey);

        IEnumerable<AffordabilityAssessmentSummaryModel> GetAffordabilityAssessmentsForAccount(int accountKey);

        bool DeleteUnconfirmedAffordabilityAssessment(int affordabilityAssessmentKey);

        bool AddAffordabilityAssessment(AffordabilityAssessmentModel affordabilityAssessment);

        bool AmendAffordabilityAssessment(AffordabilityAssessmentModel affordabilityAssessment);

        bool AmendAffordabilityAssessmentIncomeContributors(AffordabilityAssessmentModel affordabilityAssessment);
    }
}
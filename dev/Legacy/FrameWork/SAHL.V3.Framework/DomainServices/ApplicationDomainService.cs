using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System.Collections.Generic;

namespace SAHL.V3.Framework.DomainServices
{
    public class ApplicationDomainService : DomainServiceClientBase<IApplicationDomainServiceClient>, IApplicationDomainService
    {
        private IV3ServiceCommon v3ServiceCommon;

        public ApplicationDomainService(IApplicationDomainServiceClient applicationDomainServiceClient, IV3ServiceCommon v3ServiceCommon)
            : base(applicationDomainServiceClient)
        {
            this.v3ServiceCommon = v3ServiceCommon;
        }

        public new ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery
        {
            return base.PerformQuery(query);
        }

        public IEnumerable<AffordabilityAssessmentStressFactorModel> GetAffordabilityAssessmentStressFactors()
        {
            var query = new GetAffordabilityAssessmentStressFactorsQuery();
            var response = this.PerformQuery(query);
            return query.Result.Results;
        }

        public IEnumerable<AffordabilityAssessmentSummaryModel> GetAffordabilityAssessmentsForApplication(int applicationKey)
        {
            var query = new GetAffordabilityAssessmentsForApplicationQuery(applicationKey);
            var response = this.PerformQuery(query);
            return query.Result.Results;
        }

        public IEnumerable<AffordabilityAssessmentSummaryModel> GetUnconfirmedAffordabilityAssessments(int applicationKey)
        {
            var query = new GetUnconfirmedAffordabilityAssessmentsForApplicationQuery(applicationKey);
            var response = this.PerformQuery(query);
            return query.Result.Results;
        }

        public IEnumerable<AffordabilityAssessmentSummaryModel> GetHistoricalAffordabilityAssessmentsForApplication(int applicationKey)
        {
            var query = new GetHistoricalAffordabilityAssessmentsForApplicationQuery(applicationKey);
            var response = this.PerformQuery(query);
            return query.Result.Results;
        }

        public IEnumerable<AffordabilityAssessmentSummaryModel> GetAffordabilityAssessmentsForAccount(int accountKey)
        {
            var query = new GetAffordabilityAssessmentsForAccountQuery(accountKey);
            var response = this.PerformQuery(query);
            return query.Result.Results;
        }

        public bool DeleteUnconfirmedAffordabilityAssessment(int affordabilityAssessmentKey)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            DeleteUnconfirmedAffordabilityAssessmentCommand command = new DeleteUnconfirmedAffordabilityAssessmentCommand(affordabilityAssessmentKey);
            systemMessageCollection = this.PerformCommand(command);
            this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
            return !systemMessageCollection.HasErrors;
        }

        public bool AddAffordabilityAssessment(AffordabilityAssessmentModel affordabilityAssessment)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            AddApplicationAffordabilityAssessmentCommand command = new AddApplicationAffordabilityAssessmentCommand(affordabilityAssessment);
            systemMessageCollection = this.PerformCommand(command);
            this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
            return !systemMessageCollection.HasErrors;
        }

        public bool AmendAffordabilityAssessment(AffordabilityAssessmentModel affordabilityAssessment)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            AmendAffordabilityAssessmentCommand command = new AmendAffordabilityAssessmentCommand(affordabilityAssessment);
            systemMessageCollection = this.PerformCommand(command);
            this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
            return !systemMessageCollection.HasErrors;
        }

        public bool AmendAffordabilityAssessmentIncomeContributors(AffordabilityAssessmentModel affordabilityAssessment)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            AmendAffordabilityAssessmentIncomeContributorsCommand command = new AmendAffordabilityAssessmentIncomeContributorsCommand(affordabilityAssessment);
            systemMessageCollection = this.PerformCommand(command);
            this.v3ServiceCommon.HandleSystemMessages(systemMessageCollection);
            return !systemMessageCollection.HasErrors;
        }
    }
}
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager.Statements;

namespace SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager
{
    public class AffordabilityAssessmentDataManager : IAffordabilityAssessmentDataManager
    {
        private IDbFactory dbFactory;

        public AffordabilityAssessmentDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public AffordabilityAssessmentDataModel GetAffordabilityAssessmentByKey(int affordabilityAssessmentKey)
        {
            GetAffordabilityAssessmentByKeyStatement query = new GetAffordabilityAssessmentByKeyStatement(affordabilityAssessmentKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<AffordabilityAssessmentDataModel>(query);
                return results;
            }
        }
    }
}
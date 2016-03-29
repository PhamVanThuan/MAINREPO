using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Statements;
using System;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult
{
    public class DecisionTreeResultDataManager : IDecisionTreeResultDataManager
    {
        private IDbFactory dbFactory;
        public DecisionTreeResultDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void SaveCreditPricingTreeResult(Guid decisionTreeResultID, string decisionTreeQuery, string decisionTreeMessages, Guid applicationID, DateTime queryDate)
        {
            var decisionTreeResult = new CreditPricingTreeResultDataModel(decisionTreeResultID, decisionTreeMessages, decisionTreeQuery, applicationID, queryDate);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<CreditPricingTreeResultDataModel>(decisionTreeResult);
                db.Complete();
            }
        }

        public void SaveCreditAssessmentTreeResult(Guid decisionTreeResultID, string decisionTreeQuery, string decisionTreeMessages, Guid applicationID, DateTime queryDate)
        {
            var decisionTreeResult = new CreditAssessmentTreeResultDataModel(decisionTreeResultID, decisionTreeMessages, decisionTreeQuery, applicationID, queryDate);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<CreditAssessmentTreeResultDataModel>(decisionTreeResult);
                db.Complete();
            }
        }

        public CreditAssessmentTreeResultDataModel GetCreditBureauAssessmentTreeResultForApplicant(Guid applicantID)
        {
            var query = new GetCreditAssessmentForApplicantQuery(applicantID);
            using(var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<CreditAssessmentTreeResultDataModel>(query);
            }
        }

        public CreditPricingTreeResultDataModel GetCreditPricingResultForApplication(Guid applicationID)
        {
            var query = new GetCreditPricingResultForApplicationQuery(applicationID);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<CreditPricingTreeResultDataModel>(query);
            }
        }
    }
}
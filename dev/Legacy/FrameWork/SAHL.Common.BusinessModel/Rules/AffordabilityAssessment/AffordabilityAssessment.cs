using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Rules.AffordabilityAssessment
{
    [RuleDBTag("CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck",
        @"This client and\or a related legalentity are linked to an affordability assessment.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck")]
    [RuleInfo]
    public class CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck : BusinessRuleBase
    {
        private ICastleTransactionsService castleTransactionService;
        private IUIStatementService uiStatementService;

        public CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck(ICastleTransactionsService castleTransactionService, IUIStatementService uiStatementService)
        {
            this.castleTransactionService = castleTransactionService;
            this.uiStatementService = uiStatementService;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length != 2 ||
                !(Parameters[0] is int) ||
                !(Parameters[1] is int))
            {
                throw new ArgumentException("The CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck rule expects an Application Key to be passed.");
            }

            int result = 0, applicationKey = int.Parse(Parameters[0].ToString()), legalEntityKey = int.Parse(Parameters[1].ToString());
            string query = uiStatementService.GetStatement("Rules.AffordabilityAssessment", "CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ApplicationKey", applicationKey));
            prms.Add(new SqlParameter("@LegalEntityKey", legalEntityKey));
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, Databases.TwoAM, prms);
            if (ds != null &&
                ds.Tables.Count == 1 &&
                ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (int.TryParse(dr[0].ToString(), out result)
                    && result == 1)
                {
                    return result;
                }
            }

            string message = @"This client and\or a legal entity related to this client is linked to an affordability assessment.";

            AddMessage(message, message, Messages);
            return result;
        }
    }

    [RuleDBTag("AffordabilityAssessmentMandatory",
    "Check that an Active Affordability Assessment exists",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.AffordabilityAssessmentMandatory")]
    [RuleInfo]
    public class AffordabilityAssessmentMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (!(parameters[0] is IApplication))
                throw new ArgumentException("Parameter[0] is not of type IApplication.");

            IApplication application = (IApplication)parameters[0];

            IAffordabilityAssessmentRepository affordabilityAssessmentRepo = RepositoryFactory.GetRepository<IAffordabilityAssessmentRepository>();

            IList<IAffordabilityAssessment> activeAffordabilityAssessments = affordabilityAssessmentRepo.GetActiveApplicationAffordabilityAssessments(application.Key);

            if (activeAffordabilityAssessments == null || activeAffordabilityAssessments.Count <= 0)
            {
                string errorMessage = "Application does not have an Active Affordability Assessment.";
                AddMessage(errorMessage, errorMessage, messages);
                return 0;
            }
            return 1;
        }
    }
}
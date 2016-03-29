using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using System;

namespace SAHL.Common.BusinessModel.Rules.Workflow
{
    [RuleDBTag("JumboApprove",
        "",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Workflow.JumboApprove")]
    [RuleInfo]
    public class JumboApprove : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameters[0] is not of type IApplication.");

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationMortgageLoan IAML = Parameters[0] as IApplicationMortgageLoan;

            double ThreshHoldValue = Convert.ToDouble(Parameters[1]);
            double LoanAmount = IAML.LoanAgreementAmount.Value;
            // Further lending types
            if (IAML.ApplicationType.Key == 2 || IAML.ApplicationType.Key == 3 || IAML.ApplicationType.Key == 4)
            {
                // go get the existing loan amount and add to the total value
                LoanAmount += ((IMortgageLoanAccount)IAML.Account).LoanCurrentBalance;
            }
            if (LoanAmount > ThreshHoldValue)
            {
                // Dont add to the DMC here. This rule is run by WF in credit at a system stage so the DMC's can never be shown
                // to the user anyway. Also means the DMC has messages and stops us saving things later on. (Odd behvaiour I know but
                // WF is a strange beast. SHouldnt be using rules at system stages anyway!
                //AddMessage(string.Format("Loan is greater than R{0}, Supervisor Approval is required", Parameters[1]), "", Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("CheckEmploymentTypeConfirmed",
        "",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Workflow.CheckEmploymentTypeConfirmed")]
    [RuleInfo]
    public class CheckEmploymentTypeConfirmed : BusinessRuleBase
    {
        private IX2Repository x2Repository;

        public CheckEmploymentTypeConfirmed(IX2Repository x2Repository)
        {
            this.x2Repository = x2Repository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            long? instanceId = Parameters[0] as long?;
            if (!(instanceId.HasValue))
                throw new ArgumentException("Parameters[0] is not a valid instanceId.");

            bool employmentTypeConfirmed = x2Repository.HasInstancePerformedActivity(instanceId.Value, SAHL.Common.Constants.WorkFlowActivityName.ConfirmApplicationEmploymentType);

            if (employmentTypeConfirmed)
            {
                return 1;
            }
            AddMessage("Please perform the 'Confirm Application Employment' action before proceeding", "Please perform the 'Confirm Application Employment' action before proceeding", Messages);
            return 0;
        }
    }
}
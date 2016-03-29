using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class LoanAdjustmentsScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.LoanAdjustments;
            }
        }

        public Automation.Framework.WorkflowScript Create()
        {
            var workflow = new WorkflowScript
            {
                DataTable = X2DataTable.LoanAdjustments,
                WorkflowName = Workflows.LoanAdjustments,
                PrimaryKey = "@AccountKey",
                ProcessName = Processes.LoanAdjustments
            };
            #region TermRequestTimeout

            workflow.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.LoanAdjustments.TermRequestTimeout,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = ScheduledActivities.LoanAdjustments.TermRequestTimeout,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            }
						}
                   });

            #endregion TermRequestTimeout
            return workflow;
        }
    }
}
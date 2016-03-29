using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class ReadvancePaymentsScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.ReadvancePayments;
            }
        }
        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.ReadvancePayments;
            workflowScript.WorkflowName = Workflows.ReadvancePayments;
            workflowScript.PrimaryKey = "@ApplicationKey";
            workflowScript.ProcessName = Processes.Origination;

            #region FireNTUTimeoutTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ReadvancePayments.FireNTUTimeoutTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ReadvancePayments.NTUTimeout,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.FLProcessor3,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FireNTUTimeoutTimer

            #region FireDeclineTimeoutTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ReadvancePayments.FireDeclineTimeoutTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ReadvancePayments.DeclineTimeout,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.FLProcessor3,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FireDeclineTimeoutTimer

            #region OverrideTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ReadvancePayments.OverrideTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.ReadvancePayments._12HourOverride,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.ClintonS,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion OverrideTimer

            #region ApproveRapid

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ReadvancePayments.ApproveRapid,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.ReadvancePayments.ApproveRapid,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.FLSupervisor,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion ApproveRapid
            #region OnFollowupTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ReadvancePayments.OnFollowupTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ReadvancePayments.OnFollowup,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.FLSupervisor,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion OnFollowupTimer
            #region TwelveHourOverride

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ReadvancePayments.TwelveHourOverride,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ReadvancePayments._12hrs,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.FLSupervisor,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion TwelveHourOverride

            return workflowScript;
        }
    }
}
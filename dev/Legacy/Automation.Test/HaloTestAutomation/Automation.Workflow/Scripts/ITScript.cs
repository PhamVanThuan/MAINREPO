using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class ITScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.IT;
            }
        }

        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.IT;
            workflowScript.WorkflowName = Workflows.IT;
            workflowScript.PrimaryKey = "@InstanceID";
            workflowScript.ProcessName = Processes.IT;

            #region EXTRefreshDSCache

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "EXTRefreshDSCache",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = "EXTRefreshDSCache",
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.ClintonS,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion EXTRefreshDSCache

            #region EXTRefreshScheduledEvents

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "EXTRefreshScheduledEvents",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = "EXTRefreshScheduledEvents",
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.ClintonS,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion EXTRefreshScheduledEvents
            return workflowScript;
        }
    }
}
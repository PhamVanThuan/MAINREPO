using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class HelpDeskScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.HelpDesk;
            }
        }
 
        public WorkflowScript Create()
        {
            var workflow = new WorkflowScript();
            workflow.DataTable = X2DataTable.HelpDesk;
            workflow.WorkflowName = Workflows.HelpDesk;
            workflow.PrimaryKey = "@LegalEntityKey";
            workflow.ProcessName = Processes.HelpDesk;

            workflow.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.HelpDesk.CreateCaseToRequestComplete,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = "Create",
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.HelpdeskAdminUser,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 2,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.HelpDesk.Proceed,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.HelpdeskAdminUser,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 3,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.HelpDesk.CompleteRequest,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.HelpdeskAdminUser,
                                                        IgnoreWarnings = true
                                                    }
						                        }
               });
            return workflow;
        }
    }
}
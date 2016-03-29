using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;
using System.Xaml;

namespace WorkflowAutomation.Harness.Scripts
{
    public class ValuationsScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.Valuations;
            }
        }
        public WorkflowScript Create()
        {

            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.Valuations;
            workflowScript.WorkflowName = Workflows.Valuations;
            workflowScript.PrimaryKey = "@ApplicationKey";
            workflowScript.ProcessName = Processes.Origination;

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Valuations.PerformManualValuation,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					    {
                            new Step {
                                Order = 1,
                                PriorToStart = "code:Valuations.InsertValuation",
                                WorkflowActivity = WorkflowActivities.Valuations.PerformManualValuation,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ValuationsProcessor,
                                IgnoreWarnings = true
                            }
					    }
                });

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Valuations.EscalateToManager,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = ScheduledActivities.Valuations.EscalateToManager,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ValuationsProcessor,
                                IgnoreWarnings = true
                            }
						}
                });
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Valuations.ManagerArchive,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
					    {
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = ScheduledActivities.Valuations.ManagerArchive,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ValuationsManager,
                                IgnoreWarnings = true
                            }
					    }
               });
            workflowScript.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.Valuations.FurtherValuationRequired,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						    {
                                new Step {
                                    Order = 1,
                                    PriorToStart = string.Empty,
                                    WorkflowActivity = ScheduledActivities.Valuations.FurtherValuationRequired,
                                    PostComplete = string.Empty,
                                    Identity = TestUsers.ValuationsManager,
                                    IgnoreWarnings = true
                                }
						    }
                   });
            workflowScript.Scripts.Add(
                 new Script
                 {
                     Name = WorkflowAutomationScripts.Valuations.RenstructValuer,
                     Complete = string.Empty,
                     Setup = string.Empty,
                     Steps = new List<Step>()
						    {
                                new Step {
                                    Order = 1,
                                    PriorToStart = string.Empty,
                                    WorkflowActivity = ScheduledActivities.Valuations.RenstructValuer,
                                    PostComplete = string.Empty,
                                    Identity = TestUsers.ValuationsProcessor,
                                    IgnoreWarnings = true
                                }
						    }
                 });
            workflowScript.Scripts.Add(
                  new Script
                  {
                      Name = WorkflowAutomationScripts.Valuations.ReviewValuationRequired,
                      Complete = string.Empty,
                      Setup = string.Empty,
                      Steps = new List<Step>()
						    {
                                new Step {
                                    Order = 1,
                                    PriorToStart = string.Empty,
                                    WorkflowActivity = ScheduledActivities.Valuations.ReviewValuationRequired,
                                    PostComplete = string.Empty,
                                    Identity = TestUsers.ValuationsManager,
                                    IgnoreWarnings = true
                                }
						    }
                  });
            workflowScript.Scripts.Add(
                  new Script
                  {
                      Name = WorkflowAutomationScripts.Valuations.RequestValuationReview,
                      Complete = string.Empty,
                      Setup = string.Empty,
                      Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = ScheduledActivities.Valuations.RequestValuationReview,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ValuationsProcessor,
                                IgnoreWarnings = true
                            }
						}
                  });

            workflowScript.Scripts.Add(
                 new Script
                 {
                     Name = WorkflowAutomationScripts.Valuations.ValuationinOrder,
                     Complete = string.Empty,
                     Setup = string.Empty,
                     Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = ScheduledActivities.Valuations.ValuationinOrder,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ValuationsProcessor,
                                IgnoreWarnings = true
                            }
						}
                 });
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Valuations.InstructEzValValuer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
					    {
                            new Step {
                                Order = 1,
                                PriorToStart = "code:Valuations.StartEzVal",
                                WorkflowActivity = ScheduledActivities.Valuations.InstructEzValValuer,
                                PostComplete = "code:Valuations.EndEzVal",
                                Identity =TestUsers.ValuationsProcessor,
                                IgnoreWarnings = true
                            }
					    }
               });

            return workflowScript;
        }
    }
}
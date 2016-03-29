using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;
namespace WorkflowAutomation.Harness.Scripts
{
    public class LifeOriginationScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.LifeOrigination;
            }
        }
        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.LifeOrigination;
            workflowScript.WorkflowName = "LifeOrigination";
            workflowScript.PrimaryKey = "@OfferKey";
            workflowScript.ProcessName = "LifeOrigination";
         
            workflowScript.Scripts.Add(
              new Script
              {
                  Name = WorkflowAutomationScripts.LifeOrigination.CreateInstance,
                  Complete = string.Empty,
                  Setup = string.Empty,
                  Steps = new List<Step>()  {
                                                                new Step {
                                                                    Order = 1,
                                                                    PriorToStart = string.Empty,
                                                                    WorkflowActivity = ScheduledActivities.LifeOrigination.CreateInstance,
                                                                    PostComplete = string.Empty,
                                                                    Identity = TestUsers.LifeConsultant,
                                                                    IgnoreWarnings = false
                                                                }
                                                            }
              });

            workflowScript.Scripts.Add(
                new Script
                 {
                     Name = WorkflowAutomationScripts.LifeOrigination._45DayTimeout,
                     Complete = string.Empty,
                     Setup = string.Empty,
                     Steps = new List<Step>()
						                            {
                                                        new Step {
                                                            Order = 1,
                                                            PriorToStart = string.Empty,
                                                            WorkflowActivity = ScheduledActivities.LifeOrigination._45DayTimeout,
                                                            PostComplete = string.Empty,
                                                            Identity = TestUsers.MarchuanV,
                                                            IgnoreWarnings = true
                                                        }
                                                    }
                 });
            workflowScript.Scripts.Add(
             new Script
             {
                 Name = WorkflowAutomationScripts.LifeOrigination.ArchiveNTU,
                 Complete = string.Empty,
                 Setup = string.Empty,
                 Steps = new List<Step>()
						                            {
                                                        new Step {
                                                            Order = 1,
                                                            PriorToStart = string.Empty,
                                                            WorkflowActivity = ScheduledActivities.LifeOrigination.ArchiveNTU,
                                                            PostComplete = string.Empty,
                                                            Identity = TestUsers.LifeConsultant,
                                                            IgnoreWarnings = true
                                                        }
                                                    }
             });
            workflowScript.Scripts.Add(
              new Script
              {
                  Name = WorkflowAutomationScripts.LifeOrigination.WaitforCallback,
                  Complete = string.Empty,
                  Setup = string.Empty,
                  Steps = new List<Step>()
						                                {
                                                            new Step {
                                                                Order = 1,
                                                                PriorToStart = string.Empty,
                                                                WorkflowActivity = ScheduledActivities.LifeOrigination.WaitforCallback,
                                                                PostComplete = string.Empty,
                                                                Identity = TestUsers.LifeConsultant,
                                                                IgnoreWarnings = true
                                                            }
                                                        }
              });

      
            return workflowScript;
        }
    }
}
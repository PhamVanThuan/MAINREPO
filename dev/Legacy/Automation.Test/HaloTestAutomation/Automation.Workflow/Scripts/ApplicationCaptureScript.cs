using System;
using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class ApplicationCaptureScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.ApplicationCapture;
            }
        }
 
        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.ApplicationCapture;
            workflowScript.WorkflowName = Workflows.ApplicationCapture;
            workflowScript.PrimaryKey = "@ApplicationKey";
            workflowScript.ProcessName = Processes.Origination;

            #region EXTCreateCapitecInstance
            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "EXTCreateCapitecInstance",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "EXT Create Capitec Instance",
                                PostComplete = String.Empty,
                                Identity = TestUsers.BranchAdmin,
                                IgnoreWarnings = true
                            }
						}
                });

            #endregion EXTCreateCapitecInstance

            #region SubmitApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationCapture.SubmitApplication,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = "code:ApplicationCapture.CleanupNewBusinessOffer",
                                            WorkflowActivity = WorkflowActivities.ApplicationCapture.SubmitApplication,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.BranchConsultant,
                                            IgnoreWarnings = true
                                        }
						            }
                });
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.ApplicationCapture.SubmitApplicationNoCleanup,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.ApplicationCapture.SubmitApplication,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.BranchConsultant,
                                                        IgnoreWarnings = true
                                                    }
						                        }
               });

            #endregion SubmitApplication

            #region EscalateToManager

            workflowScript.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.ApplicationCapture.EscalateToManager,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
						                    {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart = string.Empty,
                                                    WorkflowActivity = WorkflowActivities.ApplicationCapture.EscalatetoManager,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.BranchConsultant,
                                                    IgnoreWarnings = true
                                                }
						                    }
            });

            #endregion EscalateToManager

            #region FireArchiveApplicationTimer

            workflowScript.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.ApplicationCapture.FireArchiveApplicationTimer,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
						                    {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart = string.Empty,
                                                    WorkflowActivity = ScheduledActivities.ApplicationCapture.ArchiveApplication,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.BranchConsultant,
                                                    IgnoreWarnings = true
                                                }
						                    }
            });

            #endregion FireArchiveApplicationTimer

            #region FireDeclineTimeoutTimer

            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.ApplicationCapture.FireDeclineTimeoutTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
						                                    {
                                                                new Step {
                                                                    Order = 1,
                                                                    PriorToStart = string.Empty,
                                                                    WorkflowActivity = ScheduledActivities.ApplicationCapture.DeclineTimeout,
                                                                    PostComplete = string.Empty,
                                                                    Identity = TestUsers.BranchConsultant,
                                                                    IgnoreWarnings = true
                                                                }
						                                    }
               });

            #endregion FireDeclineTimeoutTimer

            #region FireWaitForFollowupTimer

            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.ApplicationCapture.FireWaitForFollowupTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
						                                                {
                                                                            new Step {
                                                                                Order = 1,
                                                                                PriorToStart = string.Empty,
                                                                                WorkflowActivity = ScheduledActivities.ApplicationCapture.WaitforFollowup,
                                                                                PostComplete = string.Empty,
                                                                                Identity = TestUsers.BranchConsultant,
                                                                                IgnoreWarnings = true
                                                                            }
						                                                }
               });

            #endregion FireWaitForFollowupTimer

            #region Fire45DayTimer

            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.ApplicationCapture.Fire45DayTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
						                                                            {
                                                                                        new Step {
                                                                                            Order = 1,
                                                                                            PriorToStart = string.Empty,
                                                                                            WorkflowActivity = ScheduledActivities.ApplicationCapture._45daytimer,
                                                                                            PostComplete = string.Empty,
                                                                                            Identity = TestUsers.BranchConsultant,
                                                                                            IgnoreWarnings = true
                                                                                        }
						                                                            }
               });

            #endregion Fire45DayTimer

            return workflowScript;
        }
    }
}
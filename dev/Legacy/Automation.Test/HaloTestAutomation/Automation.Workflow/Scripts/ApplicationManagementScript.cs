using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class ApplicationManagementScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.ApplicationManagement;
            }
        }
 
        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.ApplicationManagement;
            workflowScript.WorkflowName = Workflows.ApplicationManagement;
            workflowScript.PrimaryKey = "@ApplicationKey";
            workflowScript.ProcessName = Processes.Origination;

            #region QAComplete

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "QAComplete",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.QAComplete,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditUnderwriter,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion QAComplete

            #region ClientAccepts

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "ClientAccepts",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.ClientAccepts,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.BranchConsultant,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ClientAccepts

            #region SubmitToCredit

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "SubmitToCredit",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.ApplicationinOrder,
                                            PostComplete = "code: ApplicationManagement.PostApplicationInOrder",
                                            Identity = TestUsers.NewBusinessProcessor,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion SubmitToCredit

            #region RequestResolved

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "RequestResolved",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.RequestResolved,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.BranchConsultant,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion RequestResolved

            #region CreateAtRequestAtQA

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateAtRequestAtQA",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.QAQuery,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditUnderwriter,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion CreateAtRequestAtQA

            #region Decline

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "Decline",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = "code:ApplicationManagement.PriorToStartDecline",
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.Decline,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.BranchConsultant,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion Decline

            #region NTU

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "NTU",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = "code:ApplicationManagement.PriorToStartNTU",
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.NTU,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.BranchConsultant,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion NTU

            #region QueryOnApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "QueryOnApplication",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = "code:ApplicationManagement.PriorToQueryOnApplication",
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.QueryOnApplication,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.BranchConsultant,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion QueryOnApplication

            #region QAToManageApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "QAToManageApplication",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.QAComplete,
                                            PostComplete = "code: ApplicationManagement.PostQAComplete",
                                            Identity = TestUsers.CreditUnderwriter,
                                            IgnoreWarnings = true
                                        },
                                        new Step {
                                            Order = 2,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.ClientAccepts,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditUnderwriter,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion QAToManageApplication

            #region ApplicationInOrder

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "ApplicationInOrder",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.ApplicationinOrder,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.NewBusinessProcessor,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ApplicationInOrder

            #region QACompleteFL

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.FurtherLending.QACompleteFL,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.QAComplete,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.FLProcessor3,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion QACompleteFL

            #region ApplicationReceived

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.FurtherLending.ApplicationReceived,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.ApplicationManagement.ApplicationReceived,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.FLProcessor3,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ApplicationReceived

            #region Fire2MonthsTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.FurtherLending.Fire2MonthsTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ApplicationManagement._2Months,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.FLProcessor3,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion Fire2MonthsTimer

            #region FireNTUTimeoutTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.FireNTUTimeoutTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ApplicationManagement.NTUTimeout,
                                                        PostComplete = string.Empty,
                                                        Identity = string.Empty,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FireNTUTimeoutTimer

            #region NTUPipeline

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.NTUPipeline,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = "code:ApplicationManagement.PriorToStartNTUPipeline",
                                                        WorkflowActivity = WorkflowActivities.ApplicationManagement.NTUPipeLine,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.RegistrationsManager,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion NTUPipeline

            #region QueryOnApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.QueryOnApplication,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = "",
                                                        WorkflowActivity = WorkflowActivities.ApplicationManagement.QueryOnApplication,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.NewBusinessProcessor1,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion QueryOnApplication

            #region FeedbackOnQuery

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.FeedbackOnQuery,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = "",
                                                        WorkflowActivity = WorkflowActivities.ApplicationManagement.FeedbackonQuery,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.BranchConsultant10,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FeedbackOnQuery

            #region TwoMonthQAQueryTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.TwoMonthQAQueryTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = "",
                                                        WorkflowActivity = ScheduledActivities.ApplicationManagement._2Months,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.CreditSupervisor,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion TwoMonthQAQueryTimer

            #region FireArchivedCompletedFollowupTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.FireArchivedCompletedFollowupTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = "",
                                                        WorkflowActivity = ScheduledActivities.ApplicationManagement.ArchiveCompletedFollowup,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.NewBusinessProcessor,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FireArchivedCompletedFollowupTimer

            #region FireOnFollowupTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.FireOnFollowupTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = "",
                                                        WorkflowActivity = ScheduledActivities.ApplicationManagement.OnFollowup,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.NewBusinessProcessor,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FireOnFollowupTimer

            #region FireDeclineTimeoutTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.ApplicationManagement.FireDeclineTimeoutTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = ScheduledActivities.ApplicationManagement.DeclineTimeout,
                                                        PostComplete = string.Empty,
                                                        Identity = string.Empty,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion FireDeclineTimeoutTimer

            return workflowScript;
        }
    }
}
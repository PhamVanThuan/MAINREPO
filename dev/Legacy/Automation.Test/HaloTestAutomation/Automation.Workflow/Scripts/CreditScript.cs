using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class CreditScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.Credit;
            }
        }

        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript();
            workflowScript.DataTable = X2DataTable.Credit;
            workflowScript.WorkflowName = Workflows.Credit;
            workflowScript.PrimaryKey = "@ApplicationKey";
            workflowScript.ProcessName = Processes.Origination;

            #region ValuationApproved

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "ValuationApproved",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.Credit.ValuationApproved,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditUnderwriter,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ValuationApproved

            #region ApproveApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Credit.ApproveApplication,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.Credit.ApproveApplication,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditSupervisor2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ApproveApplication

            #region ApproveWithPricingChanges

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Credit.ApproveWithPricingChanges,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.Credit.ApprovewithPricingChanges,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditSupervisor2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ApproveWithPricingChanges

            #region DeclineWithOffer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Credit.DeclineWithOffer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.Credit.DeclinewithOffer,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditSupervisor2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion DeclineWithOffer
            #region DeclineApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Credit.DeclineApplication,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.Credit.DeclineApplication,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditSupervisor2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion DeclineApplication
            #region ExceptionsDeclinewithOffer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.Credit.ExceptionsDeclinewithOffer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.Credit.ExceptionsDeclinewithOffer,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.CreditSupervisor2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion ExceptionsDeclinewithOffer
            #region ExceptionsRateAdjustment
            workflowScript.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.Credit.ExceptionsRateAdjustment,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
						                    {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart = string.Empty,
                                                    WorkflowActivity = WorkflowActivities.Credit.ExceptionsRateAdjustment,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.CreditSupervisor2,
                                                    IgnoreWarnings = true
                                                }
						                    }
            });
            #endregion ExceptionsRateAdjustment
            #region ConfirmApplicationEmployment
            workflowScript.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.Credit.ConfirmApplicationEmployment,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
						                    {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart = string.Empty,
                                                    WorkflowActivity = WorkflowActivities.Credit.ConfirmApplicationEmployment,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.CreditSupervisor2,
                                                    IgnoreWarnings = true,
                                                    Data = 1 //salaried
                                                }
						                    }
            });
            #endregion ConfirmApplicationEmployment
            return workflowScript;
        }
    }
}
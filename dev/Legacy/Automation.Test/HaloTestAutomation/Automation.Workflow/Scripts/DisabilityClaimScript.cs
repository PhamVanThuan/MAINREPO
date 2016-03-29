using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;
namespace WorkflowAutomation.Harness.Scripts
{
    public class DisabilityClaimScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get 
            {
                return WorkflowEnum.DisabilityClaim;
            }
        }

        public WorkflowScript Create()
        {
            var workFlowScript = new WorkflowScript()
            {
                DataTable = X2DataTable.DisabilityClaim,
                WorkflowName = Workflows.DisabilityClaim,
                PrimaryKey = "@DisabilityClaimKey",
                ProcessName = Processes.LifeClaims
            };

            #region CreateCaseToClaimDetails

            workFlowScript.Scripts.Add(
            new Script
                {
                    Name =  "CreateCaseToClaimDetails",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                    {
                        new Step
                        {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        }
                    }
                });

            #endregion

            #region CreateCaseToAssessClaim

            workFlowScript.Scripts.Add(
            new Script
            {
                Name = "CreateCaseToAssessClaim",
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                    {
                        new Step
                        {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        },
                        new Step
                        {
                            Order = 2,
                            PriorToStart = "code:DisabilityClaim.PriorToCaptureDetails",
                            WorkflowActivity = WorkflowActivities.DisabilityClaim.CaptureDetails,
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        }

                    }
            });

            #endregion

            #region CreateCaseToSendApprovalLetter

            workFlowScript.Scripts.Add(
            new Script
            {
                Name = "CreateCaseToSendApprovalLetter",
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                    {
                        new Step
                        {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        },
                        new Step
                        {
                            Order = 2,
                            PriorToStart = "code:DisabilityClaim.PriorToCaptureDetails",
                            WorkflowActivity = WorkflowActivities.DisabilityClaim.CaptureDetails,
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        },
                        new Step
                        {
                            Order = 3,
                            PriorToStart = "code:DisabilityClaim.PriorToApprove",
                            WorkflowActivity = WorkflowActivities.DisabilityClaim.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        }
                    }
            });

            #endregion

            #region CreateCaseToApprovedClaim

            workFlowScript.Scripts.Add(
            new Script
            {
                Name = "CreateCaseToApprovedClaim",
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                    {
                        new Step
                        {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        },
                        new Step
                        {
                            Order = 2,
                            PriorToStart = "code:DisabilityClaim.PriorToCaptureDetails",
                            WorkflowActivity = WorkflowActivities.DisabilityClaim.CaptureDetails,
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        },
                        new Step
                        {
                            Order = 3,
                            PriorToStart = "code:DisabilityClaim.PriorToApprove",
                            WorkflowActivity = WorkflowActivities.DisabilityClaim.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        },
                        new Step
                        {
                            Order = 4,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.DisabilityClaim.SendApprovalLetter,
                            PostComplete = string.Empty,
                            Identity = TestUsers.LifeClaimsAssessor,
                            IgnoreWarnings = true
                        }
                    }
            });

            #endregion

            return workFlowScript;
        }
    }
}
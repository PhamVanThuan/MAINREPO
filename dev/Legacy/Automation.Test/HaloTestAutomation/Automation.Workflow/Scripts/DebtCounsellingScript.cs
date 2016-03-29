using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class DebtCounsellingScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.DebtCounselling;
            }
        }
 
        public WorkflowScript Create()
        {
            var workflow = new WorkflowScript
            {
                DataTable = X2DataTable.DebtCounselling,
                WorkflowName = Workflows.DebtCounselling,
                PrimaryKey = "@DebtCounsellingKey",
                ProcessName = Processes.DebtCounselling
            };

            #region CreateCase

            workflow.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.DebtCounselling.CaseCreate,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.DebtCounsellingAdmin,
                                IgnoreWarnings = true
                            }
						}
                   });

            #endregion CreateCase

            #region RespondToDebtCounsellor

            workflow.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = "code:DCWorkflow.PriorToRespondToDebtCounsellor",
                                WorkflowActivity = WorkflowActivities.DebtCounselling.RespondtoDebtCounsellor,
                                PostComplete = string.Empty,
                                Identity = TestUsers.DebtCounsellingAdmin,
                                IgnoreWarnings = true
                            }
						}
                   });

            #endregion RespondToDebtCounsellor

            #region NegotiateProposal

            workflow.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.DebtCounselling.NegotiateProposal,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.DebtCounselling.NegotiateProposal,
                                PostComplete = string.Empty,
                                Identity = TestUsers.DebtCounsellingConsultant,
                                IgnoreWarnings = true
                            }
						}
                   });

            #endregion NegotiateProposal

            #region SendProposalForApproval

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.SendProposalForApproval,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                    {
                        new Step {
                            Order = 1,
                            PriorToStart = "code:DCWorkflow.PriorToSendProposalForApproval",
                            WorkflowActivity = WorkflowActivities.DebtCounselling.SendProposalforApproval,
                            PostComplete = string.Empty,
                            Identity = TestUsers.DebtCounsellingConsultant,
                            IgnoreWarnings = true
                        }
                    }
                });

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = "code:DCWorkflow.PriorToSendProposalForApprovalNoProposalInsertRequired",
                                        WorkflowActivity = WorkflowActivities.DebtCounselling.SendProposalforApproval,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.DebtCounsellingConsultant,
                                        IgnoreWarnings = true
                                    }
                                }
                });

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalAssignToManager,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                        {
                                            new Step {
                                                Order = 1,
                                                PriorToStart = "code:DCWorkflow.PriorToSendProposalAssignToManager",
                                                WorkflowActivity = WorkflowActivities.DebtCounselling.SendProposalforApproval,
                                                PostComplete = string.Empty,
                                                Identity = TestUsers.DebtCounsellingConsultant,
                                                IgnoreWarnings = true
                                            }
                                        }
            });

            #endregion SendProposalForApproval

            #region AcceptProposal

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.AcceptProposal,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                {
                                    new Step {
                                        Order = 1,
                                        PriorToStart="code:DCWorkflow.PriorToProposalAcceptance",
                                        WorkflowActivity = WorkflowActivities.DebtCounselling.Accept,
                                        PostComplete = "code:DCWorkflow.PostApproval",
                                        Identity = TestUsers.DebtCounsellingSupervisor,
                                        IgnoreWarnings = true
                                    }
                                }
                });

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.AcceptProposalAsManager,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                        {
                                            new Step {
                                                Order = 1,
                                                PriorToStart="code:DCWorkflow.PriorToProposalAcceptance",
                                                WorkflowActivity = WorkflowActivities.DebtCounselling.Accept,
                                                PostComplete = "code:DCWorkflow.PostApproval",
                                                Identity = TestUsers.DebtCounsellingManager,
                                                IgnoreWarnings = true
                                            }
                                        }
            });

            #endregion AcceptProposal

            #region SignedDocsReceived

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.SignedDocsReceived,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart=string.Empty,
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.SignedDocumentsReceived,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion SignedDocsReceived

            #region NotificationOfDecision

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.NotificationOfDecision,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToNotificationOfDecision",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.NotificationofDecision,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion NotificationOfDecision

            #region PaymentReceived

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.PaymentReceived,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToPaymentReceived",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.PaymentReceived,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion PaymentReceived

            #region PaymentInOrder

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.PaymentInOrder,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart=string.Empty,
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.PaymentinOrder,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingSupervisor,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion PaymentInOrder

            #region EXTUnderCancellation

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.EXTUnderCancellation,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToPendCancellation",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.EXTUnderCancellation,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingSupervisor,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion EXTUnderCancellation

            #region CaptureRecoveriesProposal

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.CaptureRecoveriesProposal,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToCaptureRecoveriesProposal",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.CaptureRecoveriesProposal,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion CaptureRecoveriesProposal

            #region EscalateRecoveriesProposal

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.EscalateRecoveriesProposal,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToEscalateRecoveriesProposal",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.EscalateRecoveriesProposal,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion EscalateRecoveriesProposal

            #region SendToLitigation

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.SendToLitigation,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToSendToLitigation",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.SendtoLitigation,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion SendToLitigation

            #region BondExclusionsArrears

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.BondExclusionsArrears,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToBondExclusionsArrears",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.BondExclusionsArrears,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion BondExclusionsArrears

            #region ExcludeBond

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.ExcludeBond,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart=string.Empty,
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.ExcludeBond,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion ExcludeBond

            #region NotificationOfSequestration

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.NotificationOfSequestration,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToNotificationOfSequestration",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.NotifiedofSequestration,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion NotificationOfSequestration

            #region Fire60DayTimer

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.Fire60DayTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart=string.Empty,
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.SixtyDayTimer,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion Fire60DayTimer

            #region TerminateApplication

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.TerminateApplication,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToTerminateApplication",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.TerminateApplication,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion TerminateApplication

            #region SendTerminationLetter

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.SendTerminationLetter,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToSendTerminationLetter",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.SendTerminationLetter,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion SendTerminationLetter

            #region EXTIntoArrears

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.EXTIntoArrears,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="code:DCWorkflow.PriorToEXTIntoArrears",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.EXTIntoArrears,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion EXTIntoArrears

            #region FireTermExpiredTimer

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.FireTermExpiredTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart=string.Empty,
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.TermExpired,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion FireTermExpiredTimer

            #region AttorneyToOppose

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.CaptureCourtDetailsToAttorneyToOppose,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                                    {   new Step {
                                                            Order = 1,
                                                            PriorToStart="code:DCWorkflow.PriorToCourtDetails",
                                                            WorkflowActivity = WorkflowActivities.DebtCounselling.CourtDetails,
                                                            PostComplete = string.Empty,
                                                            Identity = TestUsers.DebtCounsellingConsultant,
                                                            IgnoreWarnings = true
                                                        },
                                                        new Step {
                                                            Order = 2,
                                                            PriorToStart="code:DCWorkflow.PriorToAttorneyToOppose",
                                                            WorkflowActivity = WorkflowActivities.DebtCounselling.AttorneytoOppose,
                                                            PostComplete = string.Empty,
                                                            Identity = TestUsers.DebtCounsellingCourtConsultant,
                                                            IgnoreWarnings = true
                                                        }
                                                    }
            });

            #endregion AttorneyToOppose

            #region Fire10DaysTimer

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.Fire10DaysTimer,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                                    {   new Step {
                                                            Order = 1,
                                                            PriorToStart=string.Empty,
                                                            WorkflowActivity = WorkflowActivities.DebtCounselling.TenDayTimer,
                                                            PostComplete = string.Empty,
                                                            Identity = TestUsers.DebtCounsellingConsultant,
                                                            IgnoreWarnings = true
                                                        }
                                                    }
            });

            #endregion Fire10DaysTimer

            #region Fire5DaysTimer

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.Fire5DaysTimer,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                                                {   new Step {
                                                                        Order = 1,
                                                                        PriorToStart=string.Empty,
                                                                        WorkflowActivity = WorkflowActivities.DebtCounselling.FiveDayTimer,
                                                                        PostComplete = string.Empty,
                                                                        Identity = TestUsers.DebtCounsellingConsultant,
                                                                        IgnoreWarnings = true
                                                                    }
                                                                }
            });

            #endregion Fire5DaysTimer

            #region RaiseEXT60DateNoDateOrPayment

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.RaiseEXT60DateNoDateOrPayment,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                                                            {   new Step {
                                                                                    Order = 1,
                                                                                    PriorToStart=string.Empty,
                                                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.EXT_60daynodateorpay,
                                                                                    PostComplete = string.Empty,
                                                                                    Identity = TestUsers.DebtCounsellingConsultant,
                                                                                    IgnoreWarnings = true
                                                                                }
                                                                            }
            });

            #endregion RaiseEXT60DateNoDateOrPayment

            #region FortyFiveDayTimer

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.Fire45DaysTimer,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                                                                {   new Step {
                                                                                        Order = 1,
                                                                                        PriorToStart=string.Empty,
                                                                                        WorkflowActivity = WorkflowActivities.DebtCounselling.FortyFiveDayTimer,
                                                                                        PostComplete = string.Empty,
                                                                                        Identity = TestUsers.DebtCounsellingConsultant,
                                                                                        IgnoreWarnings = true
                                                                                    }
                                                                                }
                });

            #endregion FortyFiveDayTimer

            #region DeclineProposal

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.DeclineProposal,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                                        {
                                                            new Step {
                                                                Order = 1,
                                                                PriorToStart="code:DCWorkflow.PriorToDecline",
                                                                WorkflowActivity = WorkflowActivities.DebtCounselling.Decline,
                                                                PostComplete = "code:DCWorkflow.PostDecline",
                                                                Identity = TestUsers.DebtCounsellingSupervisor,
                                                                IgnoreWarnings = true
                                                            }
                                                        }
                });

            #endregion DeclineProposal

            #region DeclineProposalCourtOrderWithAppeal

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.DeclineProposalCourtOrderWithAppeal,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                                        {
                                                            new Step {
                                                                Order = 1,
                                                                PriorToStart="code:DCWorkflow.PriorToDeclineProposalCourtOrderWithAppeal",
                                                                WorkflowActivity = WorkflowActivities.DebtCounselling.Decline,
                                                                PostComplete = "code:DCWorkflow.PostDecline",
                                                                Identity = TestUsers.DebtCounsellingSupervisor,
                                                                IgnoreWarnings = true
                                                            }
                                                        }
                });

            #endregion DeclineProposalCourtOrderWithAppeal

            #region ApproveShortfall

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.ApproveShortfall,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart=string.Empty,
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.ApproveShortfall,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingSupervisor,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion ApproveShortfall

            #region EXTCancellationRegistered

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.EXTCancellationRegistered,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                            {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart="",
                                                    WorkflowActivity = WorkflowActivities.DebtCounselling.EXTCancellationRegistered,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.DebtCounsellingSupervisor,
                                                    IgnoreWarnings = true
                                                }
                                            }
                });

            #endregion EXTCancellationRegistered

            #region ChangeInCircumstance

            workflow.Scripts.Add(
                new Script
                {
                    Name = WorkflowAutomationScripts.DebtCounselling.ChangeInCircumstance,
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                                                {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart="",
                                                        WorkflowActivity = WorkflowActivities.DebtCounselling.ChangeinCircumstance,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.DebtCounsellingConsultant,
                                                        IgnoreWarnings = true
                                                    }
                                                }
                });

            #endregion ChangeInCircumstance

            #region Bypass10DayTimer

            workflow.Scripts.Add(
            new Script
            {
                Name = WorkflowAutomationScripts.DebtCounselling.Bypass10DayTimer,
                Complete = string.Empty,
                Setup = string.Empty,
                Steps = new List<Step>()
                                                    {   new Step {
                                                            Order = 1,
                                                            PriorToStart=string.Empty,
                                                            WorkflowActivity = WorkflowActivities.DebtCounselling.Bypass10DayTimer,
                                                            PostComplete = string.Empty,
                                                            Identity = TestUsers.DebtCounsellingSupervisor,
                                                            IgnoreWarnings = true
                                                        }
                                                    }
            });

            #endregion Bypass10DayTimer            

            return workflow;
        }
    }
}
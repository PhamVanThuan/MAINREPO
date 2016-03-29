using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class Cap2OffersScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.CAP2Offers;
            }
        }

        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript
            {
                DataTable = X2DataTable.Cap2,
                WorkflowName = Workflows.CAP2Offers,
                PrimaryKey = "@CapOfferKey",
                ProcessName = "CAP2 Offers"
            };

            #region FurtherAdvanceCAPToCredit

            workflowScript.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.Cap2Offers.FurtherAdvanceCAPToCredit,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                                new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.FormsSent,
                                PostComplete = "code:CAP2.FormsSentComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = "script:test.MarkCAP2OfferForFAdvDecision",
                                WorkflowActivity = WorkflowActivities.Cap2Offers.FurtherAdvanceDecision,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                            	Order = 4,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.CreditApproval,
                                PostComplete = "code:CAP2.CreditApprovalComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                            	Order = 5,
                                PriorToStart = "code:CAP2.GrantCAP2OfferStart",
                                WorkflowActivity = WorkflowActivities.Cap2Offers.GrantCAP2Offer,
                                PostComplete = string.Empty,
                                Identity = TestUsers.CreditUnderwriter2,
                                IgnoreWarnings = true
                            },
                            new Step {
                            	Order = 6,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.LASent,
                                PostComplete = "code:CAP2.LASentComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                            	Order = 7,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.ReadyForReadvance,
                                PostComplete = "code:CAP2.ReadyForReadvanceComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            }
						}
                   });

            #endregion FurtherAdvanceCAPToCredit

            #region ReadvanceRequiredNotIgnoringWarnings

            workflowScript.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.Cap2Offers.ReadvanceDone,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
                       {
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                                new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.FormsSent,
                                PostComplete = "code:CAP2.FormsSentComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = "script:test.MarkCAP2OfferForReadvanceRequired",
                                WorkflowActivity = WorkflowActivities.Cap2Offers.ReadvanceRequired,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 4,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.ReadvanceDone,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = false
                            }
                       }
                   }
                   );

            #endregion ReadvanceRequiredNotIgnoringWarnings

            #region CAP2OfferGranted

            workflowScript.Scripts.Add(
                 new Script
                 {
                     Name = WorkflowAutomationScripts.Cap2Offers.CAP2OfferGranted,
                     Complete = string.Empty,
                     Setup = string.Empty,
                     Steps = new List<Step>()
						{
                                new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.FormsSent,
                                PostComplete = "code:CAP2.FormsSentComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = "script:test.MarkCAP2OfferForFAdvDecision",
                                WorkflowActivity = WorkflowActivities.Cap2Offers.FurtherAdvanceDecision,
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                            	Order = 4,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.Cap2Offers.CreditApproval,
                                PostComplete = "code:CAP2.CreditApprovalComplete",
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            },
                            new Step {
                            	Order = 5,
                                PriorToStart = "code:CAP2.GrantCAP2OfferStart",
                                WorkflowActivity = WorkflowActivities.Cap2Offers.GrantCAP2Offer,
                                PostComplete = string.Empty,
                                Identity = TestUsers.CreditUnderwriter2,
                                IgnoreWarnings = true
                            }
						}
                 }
             );

            #endregion CAP2OfferGranted

            #region CAPCaseCreate

            workflowScript.Scripts.Add(
                   new Script
                   {
                       Name = WorkflowAutomationScripts.Cap2Offers.CAPCaseCreate,
                       Complete = string.Empty,
                       Setup = string.Empty,
                       Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.ClintonS,
                                IgnoreWarnings = true
                            }
						}
                   });

            #endregion CAPCaseCreate

            #region FormsSent

            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Cap2Offers.FormsSent,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
						                    {
                                                new Step {
                                                    Order = 1,
                                                    PriorToStart = string.Empty,
                                                    WorkflowActivity = "Create",
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.ClintonS,
                                                    IgnoreWarnings = true
                                                },
                                                new Step {
                                                    Order = 2,
                                                    PriorToStart = "code:CAP2.FormsSentComplete",
                                                    WorkflowActivity = WorkflowActivities.Cap2Offers.FormsSent,
                                                    PostComplete = string.Empty,
                                                    Identity = TestUsers.ClintonS,
                                                    IgnoreWarnings = true
                                                }
						                    }
               });

            #endregion FormsSent

            #region CompletedExpiredTimer
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Cap2Offers.CompletedExpiredTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
				    {
                        new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = ScheduledActivities.CAP2Offers.CompletedExpired,
                            PostComplete = string.Empty,
                            Identity = TestUsers.ClintonS,
                            IgnoreWarnings = true
                        }
				    }
               });
            #endregion

            #region NTUOfferTimer
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Cap2Offers.NTUOfferTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
				                {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = ScheduledActivities.CAP2Offers.NTUOffer,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.ClintonS,
                                        IgnoreWarnings = true
                                    }
				                }
               });
            #endregion

            #region DeclinedTimer
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Cap2Offers.DeclinedTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
				                {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = ScheduledActivities.CAP2Offers.Declined,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.ClintonS,
                                        IgnoreWarnings = true
                                    }
				                }
               });
            #endregion

            #region OfferExpiredTimer
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Cap2Offers.OfferExpiredTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
				                {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = ScheduledActivities.CAP2Offers.OfferExpired,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.ClintonS,
                                        IgnoreWarnings = true
                                    }
				                }
               });
            #endregion

            #region WaitForCallbackTimer
            workflowScript.Scripts.Add(
               new Script
               {
                   Name = WorkflowAutomationScripts.Cap2Offers.WaitForCallbackTimer,
                   Complete = string.Empty,
                   Setup = string.Empty,
                   Steps = new List<Step>()
				                {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = ScheduledActivities.CAP2Offers.WaitforCallback,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.ClintonS,
                                        IgnoreWarnings = true
                                    }
				                }
               });
            #endregion
            return workflowScript;
        }
    }
}
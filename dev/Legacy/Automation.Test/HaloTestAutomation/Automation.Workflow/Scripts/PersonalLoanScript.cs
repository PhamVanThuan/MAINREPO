using Automation.Framework;
using Common.Constants;
using Common.Enums;
using System.Collections.Generic;

namespace WorkflowAutomation.Harness.Scripts
{
    public class PersonalLoanScript : IAutomationScript
    {
        public WorkflowEnum Workflow
        {
            get
            {
                return WorkflowEnum.PersonalLoans;
            }
        }

        public WorkflowScript Create()
        {
            var workflowScript = new WorkflowScript
            {
                DataTable = X2DataTable.PersonalLoans,
                WorkflowName = Workflows.PersonalLoans,
                PrimaryKey = "@ApplicationKey",
                ProcessName = Processes.PersonalLoan
            };

            #region CreateCaseToManageLead

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToManageLead",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant2,
                                IgnoreWarnings = true
                            }
						}
                });

            #endregion CreateCaseToManageLead

            #region CreateCaseToManageApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToManageApplication",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = "Create",
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.PersonalLoanConsultant2,
                                            IgnoreWarnings = true
                                        },
                                        new Step {
                                            Order = 2,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.PersonalLoanConsultant2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion CreateCaseToManageApplication

            #region CreateCaseToDocumentCheck

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToDocumentCheck",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = "Create",
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 2,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 3,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion CreateCaseToDocumentCheck

            #region CreateCaseToCredit

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToCredit",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = "Create",
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 2,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 3,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 4,
                                                        PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanAdmin3,
                                                        IgnoreWarnings = true
                                                    }
						                        }
                });

            #endregion CreateCaseToCredit

            #region CreateCaseToDeclinedByCredit

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToDeclinedByCredit",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						{
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 4,
                                PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 5,
                                PriorToStart = "code:PersonalLoanWF.PriorToDecline",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.Decline,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanCreditAnalyst1,
                                IgnoreWarnings = true
                            }
						}
                });

            #endregion CreateCaseToDeclinedByCredit

            #region CreateCaseToLegalAgreements

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToLegalAgreements",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					{
                         new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 2,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 3,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 4,
                            PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 5,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanCreditAnalyst1,
                            IgnoreWarnings = true
                        }
					}
                });

            #endregion CreateCaseToLegalAgreements

            #region CreateCaseToVerifyDocuments

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToVerifyDocuments",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
                    {
                        new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 2,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 3,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 4,
                            PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 5,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanCreditAnalyst1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 6,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.SendDocuments,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        }
                    }
                });

            #endregion CreateCaseToVerifyDocuments

            #region CreateAndNTUFromManageApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateAndNTUFromManageApplication",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					{
                        new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant2,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 2,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant2,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 3,
                            PriorToStart = "code:PersonalLoanWF.PriorToNTU",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.NTU,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant2,
                            IgnoreWarnings = true
                        }
					}
                });

            #endregion CreateAndNTUFromManageApplication

            #region CreateAndNTUFromLegalAgreements

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateAndNTUFromLegalAgreements",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					{
                        new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 2,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 3,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 4,
                            PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 5,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanCreditAnalyst1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 6,
                            PriorToStart = "code:PersonalLoanWF.PriorToNTU",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.NTU,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        }
					}
                });

            #endregion CreateAndNTUFromLegalAgreements

            #region CreateCaseToDisbursement

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToDisbursement",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					{
                        new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 2,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 3,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 4,
                            PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 5,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanCreditAnalyst1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 6,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.SendDocuments,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 7,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentsVerified,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        }
					}
                });

            #endregion CreateCaseToDisbursement

            #region CreateAndNTUFromDisbursement

            workflowScript.Scripts.Add(
                    new Script
                    {
                        Name = "CreateAndNTUFromDisbursement",
                        Complete = string.Empty,
                        Setup = string.Empty,
                        Steps = new List<Step>()
					    {
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = "Create",
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 4,
                                PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 5,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanCreditAnalyst1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 6,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.SendDocuments,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 7,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentsVerified,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 8,
                                PriorToStart = "code:PersonalLoanWF.PriorToNTU",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.NTU,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanSupervisor1,
                                IgnoreWarnings = true
                            }
					    }
                    });

            #endregion CreateAndNTUFromDisbursement

            #region CreateAndReturnToManageApplicationPostCreditApproval

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateAndReturnToManageApplicationPostCreditApproval",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					            {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = "code:PersonalLoanWF.PriorToCreateApplication",
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanConsultant1,
                                        IgnoreWarnings = true
                                    },
                                    new Step {
                                        Order = 2,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanConsultant1,
                                        IgnoreWarnings = true
                                    },
                                    new Step {
                                        Order = 3,
                                        PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanAdmin1,
                                        IgnoreWarnings = true
                                    },
                                    new Step {
                                        Order = 4,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanCreditAnalyst1,
                                        IgnoreWarnings = true
                                    },
                                    new Step {
                                        Order = 5,
                                        PriorToStart = string.Empty,
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanAdmin1,
                                        IgnoreWarnings = true
                                    }
					            }
                });

            #endregion CreateAndReturnToManageApplicationPostCreditApproval

            #region CalculateApplication

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CalculateApplication",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					            {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = "code:PersonalLoanWF.PriorToCreateApplication",
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanConsultant1,
                                        IgnoreWarnings = true
                                    },
                                }
                });

            #endregion CalculateApplication

            #region CalculateApplicationWithNoLifeCover

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CalculateApplicationWithNoLifeCover",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					            {
                                    new Step {
                                        Order = 1,
                                        PriorToStart = "code:PersonalLoanWF.PriorToCreateApplicationNoLifeCover",
                                        WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                        PostComplete = string.Empty,
                                        Identity = TestUsers.PersonalLoanConsultant1,
                                        IgnoreWarnings = true
                                    },
                                }
                });

            #endregion CalculateApplicationWithNoLifeCover

            #region CalculateApplicationToDisbursement

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CalculateApplicationToDisbursement",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					    {
                            new Step {
                                Order = 1,
                                PriorToStart = "code:PersonalLoanWF.PriorToCreateApplication",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            },
                            new Step
                            {
                                Order = 4,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanCreditAnalyst1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 5,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.SendDocuments,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step
                            {
                                Order = 6,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentsVerified,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            }
                        }
                });

            #endregion CalculateApplicationToDisbursement

            #region CalculateApplicationToDisbursementWithExternalLife

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CalculateApplicationToDisbursementWithNoLifeCover",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
					    {
                            new Step {
                                Order = 1,
                                PriorToStart = "code:PersonalLoanWF.PriorToCreateApplicationNoLifeCover",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 2,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 3,
                                PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            },
                            new Step
                            {
                                Order = 4,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanCreditAnalyst1,
                                IgnoreWarnings = true
                            },
                            new Step {
                                Order = 5,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.SendDocuments,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanConsultant1,
                                IgnoreWarnings = true
                            },
                            new Step
                            {
                                Order = 6,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentsVerified,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin1,
                                IgnoreWarnings = true
                            }
                        }
                });

            #endregion CalculateApplicationToDisbursementWithExternalLife

            #region CreateApplicationToDisbursed

            workflowScript.Scripts.Add(
             new Script
             {
                 Name = "CreateCaseToDisbursed",
                 Complete = string.Empty,
                 Setup = string.Empty,
                 Steps = new List<Step>()
					{
                        new Step {
                            Order = 1,
                            PriorToStart = string.Empty,
                            WorkflowActivity = "Create",
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant3,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 2,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant3,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 3,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant3,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 4,
                            PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 5,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.Approve,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanCreditAnalyst2,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 6,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.SendDocuments,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanConsultant1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 7,
                            PriorToStart = string.Empty,
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentsVerified,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanAdmin1,
                            IgnoreWarnings = true
                        },
                        new Step {
                            Order = 8,
                            PriorToStart = "code:PersonalLoanWF.PriorToDisburseFunds",
                            WorkflowActivity = WorkflowActivities.PersonalLoans.DisburseFunds,
                            PostComplete = string.Empty,
                            Identity = TestUsers.PersonalLoanSupervisor1,
                            IgnoreWarnings = true
                        }
					}
             });

            #endregion CreateApplicationToDisbursed

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
                                PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanAdmin2,
                                IgnoreWarnings = true
                            }
						}
                });

            #endregion ApplicationInOrder

            #region FireDisbursementTimer

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "FireDisbursementTimer",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						            {
                                        new Step {
                                            Order = 1,
                                            PriorToStart = string.Empty,
                                            WorkflowActivity = WorkflowActivities.PersonalLoans.DisbursedTimer,
                                            PostComplete = string.Empty,
                                            Identity = TestUsers.PersonalLoanConsultant2,
                                            IgnoreWarnings = true
                                        }
						            }
                });

            #endregion FireDisbursementTimer

            #region CreateCaseToCreditAndEscalateToExceptions

            workflowScript.Scripts.Add(
                new Script
                {
                    Name = "CreateCaseToCreditAndEscalateToExceptions",
                    Complete = string.Empty,
                    Setup = string.Empty,
                    Steps = new List<Step>()
						                        {
                                                    new Step {
                                                        Order = 1,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = "Create",
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 2,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.CalculateApplication,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 3,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.DocumentCheck,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanConsultant3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 4,
                                                        PriorToStart = "code:PersonalLoanWF.PriorToApplicationInOrder",
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.ApplicationinOrder,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanAdmin3,
                                                        IgnoreWarnings = true
                                                    },
                                                    new Step {
                                                        Order = 5,
                                                        PriorToStart = string.Empty,
                                                        WorkflowActivity = WorkflowActivities.PersonalLoans.EscalateToExceptionsManager,
                                                        PostComplete = string.Empty,
                                                        Identity = TestUsers.PersonalLoanCreditAnalyst1,
                                                        IgnoreWarnings = true,
                                                        Data = TestUsers.PersonalLoansCreditExceptionsManager1
                                                    }
						                        }
                });

            #endregion CreateCaseToCreditAndEscalateToExceptions

            workflowScript.Scripts.Add(
            new Script
            {
            Name = "DisburseFundsForTest",
            Complete = string.Empty,
            Setup = string.Empty,
            Steps = new List<Step>()
                                {
                            new Step {
                                Order = 1,
                                PriorToStart = string.Empty,
                                WorkflowActivity = WorkflowActivities.PersonalLoans.DisburseFunds,
                                PostComplete = string.Empty,
                                Identity = TestUsers.PersonalLoanSupervisor1,
                                IgnoreWarnings = true
                            }
                        }
            });
            return workflowScript;
        }
    }
}
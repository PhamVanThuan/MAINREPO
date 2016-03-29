using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCaptureTests.Rules
{
    [RequiresSTA]
    public class RateAdjustmentTests : ApplicationCaptureTests.TestBase<BasePage>
    {
        private Dictionary<string, int> OfferKeys;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant10);
            OfferKeys = new Dictionary<string, int>();
        }

        protected override void OnTestFixtureTearDown()
        {
            base.OnTestFixtureTearDown();
        }

        [Test, TestCaseSource(typeof(RateAdjustmentTests), "TestCasesToRun")]
        public void ExecuteTest(PricingForRiskTest testCase)
        {
            int offerKey = 0;
            bool deleteCapitec = false;
            bool insertCapitec = false;
            bool deleteGEPFdisqualified = false;
            try
            {
                var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationCaptureWF.ApplicationCapture, Workflows.ApplicationCapture, OfferTypeEnum.NewPurchase, string.Empty);
                offerKey = results.Rows(0).Column("offerkey").GetValueAs<int>();
                Console.WriteLine(offerKey + ' ' + testCase.Description);
                OfferKeys.Add(testCase.Description, offerKey);
                //We need the correct LAA based on required LTV
                float purchasePrice = testCase.PurchasePrice > 0f ? testCase.PurchasePrice : 500000f;
                float LAA = (testCase.RequiredLTV / 100) * purchasePrice;
                float fees = 10000f;
                float cashDeposit = (purchasePrice - LAA);
                Service<IApplicationService>().UpdateNewPurchaseVariableLoanOffer(offerKey, householdIncome: testCase.HouseholdIncome, loanAgreementAmount: LAA,
                                                                                  feesTotal: fees, cashDeposit: cashDeposit,
                                                                                  propertyValuation: purchasePrice,
                                                                                  instalment: 1145.71f,
                                                                                  bondToRegister: purchasePrice + 30000f,
                                                                                  linkRate: 0.039f,
                                                                                  term: 240,
                                                                                  marketRateKey: MarketRateEnum._3MonthJibar_Rounded,
                                                                                  rateConfigurationKey: 204,
                                                                                  employmentType: testCase.EmploymentType,
                                                                                  purchasePrice: purchasePrice);
                //update all the required data
                Service<IApplicationService>().CleanupNewBusinessOffer(offerKey);
                string date = Service<IControlService>().GetControlTextValue("ITCEmpirica4GoLive");
                DateTime offerStartDate = Convert.ToDateTime(date).AddDays(1);
                Service<IApplicationService>().UpdateOfferStartDate(offerStartDate, offerKey);
                var legalEntityKey = Service<IApplicationService>().GetActiveOfferRolesByOfferRoleType(offerKey, OfferRoleTypeEnum.LeadMainApplicant).First().Column("legalentitykey").GetValueAs<int>();
                Service<ILegalEntityService>().UpdateLegalEntityInitials(legalEntityKey, "I");
                //GEPF
                var isGEPFfunded = (from att in Service<IApplicationService>().GetOfferAttributes(offerKey)
                                    where att.OfferAttributeTypeKey == OfferAttributeTypeEnum.GovernmentEmployeePensionFund
                                    select att != null).FirstOrDefault();
                if (!testCase.GEPFfunded && isGEPFfunded)
                {
                    Service<IApplicationService>().DeleteOfferAttribute(offerKey, OfferAttributeTypeEnum.GovernmentEmployeePensionFund);
                    Service<IApplicationService>().InsertOfferAttribute(offerKey, OfferAttributeTypeEnum.DisqualifiedForGEPF);
                    deleteGEPFdisqualified = true;
                }
                Service<IApplicationService>().UpdateAllMainApplicantEmploymentRecords(offerKey, (int)testCase.EmploymentType, testCase.HouseholdIncome, testCase.GEPFfunded);
                //Insert the ITC
                Service<ILegalEntityService>().InsertITC(offerKey, testCase.EmpiricaUpperBound, testCase.EmpiricaScoreLowerBound);
                //Capitec
                var isCapitec = (from att in Service<IApplicationService>().GetOfferAttributes(offerKey)
                                 where att.OfferAttributeTypeKey == OfferAttributeTypeEnum.Capitec
                                 select att != null).FirstOrDefault();
                if (testCase.Capitec && !isCapitec)
                {
                    Service<IApplicationService>().InsertOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);
                    deleteCapitec = true;
                }
                else if (!testCase.Capitec && isCapitec)
                {
                    Service<IApplicationService>().DeleteOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);
                    insertCapitec = true;
                }
                //complete the action
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
                //Recalc and Save
                base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
                base.Browser.Page<ApplicationLoanDetailsUpdate>().ChangeApplicationProduct(Products.NewVariableLoan);
                base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);
                base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
                base.Browser.Page<ApplicationLoanDetailsUpdate>().UpdateInterestOnlyAttribute(false);
                //submit it
                base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
                base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            }
            finally
            {
                if (deleteCapitec)
                    Service<IApplicationService>().DeleteOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);
                else if (insertCapitec)
                    Service<IApplicationService>().InsertOfferAttribute(offerKey, OfferAttributeTypeEnum.Capitec);
                else if (deleteGEPFdisqualified)
                    Service<IApplicationService>().DeleteOfferAttribute(offerKey, OfferAttributeTypeEnum.DisqualifiedForGEPF);
            }
        }

        [Test, TestCaseSource(typeof(RateAdjustmentTests), "TestCasesToRun")]
        public void ValidateTestResults(PricingForRiskTest testCase)
        {
            int offerKey = 0;
            offerKey = (from o in OfferKeys where o.Key == testCase.Description select o.Value).FirstOrDefault();
            if (offerKey == 0)
            {
                throw new WatiN.Core.Exceptions.WatiNException(string.Format("No Offer was created for {0}, or the test was not selected to be run.", testCase.Description));
            }
            //Wait for application to go through pricing for risk.
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.QA, 30);
            if (testCase.ExpectedAdjustment != -1)
            {
                FinancialAdjustmentAssertions.AssertOfferAdjustments(offerKey, testCase.ExpectedAdjustment, testCase.ExpectedFinancialAdjustmentToBeApplied);
            }
            else
            {
                FinancialAdjustmentAssertions.AssertNoOfferAdjustments(offerKey, testCase.ExpectedFinancialAdjustmentToBeApplied);
            }
            if (!String.IsNullOrEmpty(testCase.ExpectedRateAdjustmentElementDescription))
                FinancialAdjustmentAssertions.AssertFinancialAdjustmentCreatedByCorrectRateAdjustmentElement(offerKey, testCase.ExpectedAdjustment, testCase.ExpectedRateAdjustmentElementDescription);
        }

        public class PricingForRiskTest
        {
            public string Description { get; set; }

            public int EmpiricaScoreLowerBound { get; set; }

            public int EmpiricaUpperBound { get; set; }

            public float RequiredLTV { get; set; }

            public double ExpectedAdjustment { get; set; }

            public float HouseholdIncome { get; set; }

            public string ExpectedRateAdjustmentElementDescription { get; set; }

            public EmploymentTypeEnum EmploymentType { get; set; }

            public FinancialAdjustmentTypeSourceEnum ExpectedFinancialAdjustmentToBeApplied { get; set; }

            public bool Capitec { get; set; }

            public float PurchasePrice { get; set; }

            public bool GEPFfunded { get; set; }

            public override string ToString()
            {
                return Description.ToString();
            }
        }

        public List<PricingForRiskTest> TestCasesToRun()
        {
            List<PricingForRiskTest> testCases = new List<PricingForRiskTest>();

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 0 Salaried [Emp 575-595]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 595,
                RequiredLTV = 69,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_0_Salaried_Emp_575_595,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 0 Salaried [Emp 575-600 Capitec]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 600,
                RequiredLTV = 69,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_0_Salaried_Emp_575_600_Capitec,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                Capitec = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 0 Salaried [LAA > R2,750,000]",
                EmpiricaScoreLowerBound = 650,
                EmpiricaUpperBound = 750,
                RequiredLTV = 69,
                PurchasePrice = 3985508f,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 125000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_0_Salaried_LAA_R2750000,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 1 Salaried [Emp 575-595]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 595,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_1_Salaried_Emp_575_595,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 1 Salaried [Emp 575-600 Capitec]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 600,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_1_Salaried_Emp_575_600_Capitec,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                Capitec = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 2 Salaried [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.007,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_2_Salaried_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 2 Salaried [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_2_Salaried_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 2 Salaried [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_2_Salaried_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 3 Salaried [Emp 595-629]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 629,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_3_Salaried_Emp_595_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 4 Salaried [Emp 595-629]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 629,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_4_Salaried_Emp_595_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 Salaried [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.009,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_Salaried_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 Salaried [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.006,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_Salaried_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 Salaried [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_Salaried_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 Salaried [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_Salaried_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 Salaried [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.01,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_Salaried_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 Salaried [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.007,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_Salaried_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 Salaried [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_Salaried_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 Salaried [Emp 629-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_Salaried_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 Salaried [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.013,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_Salaried_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 Salaried [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.008,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_Salaried_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 Salaried [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_Salaried_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 Salaried [Emp 629-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_Salaried_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 Salaried [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.013,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_Salaried_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 Salaried [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.008,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_Salaried_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 Salaried [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_Salaried_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 Salaried [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_Salaried_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 11 Salaried [Emp 640-649]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 649,
                RequiredLTV = 100,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_11_Salaried_Emp_640_649,
                EmploymentType = EmploymentTypeEnum.Salaried,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 0 Self Employed [LAA > R1,800,000]",
                EmpiricaScoreLowerBound = 650,
                EmpiricaUpperBound = 700,
                RequiredLTV = 69,
                PurchasePrice = 2608698f,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 75000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_0_Self_Employed_LAA_R1800000,
                EmploymentType = EmploymentTypeEnum.SelfEmployed,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 1 Self Employed [LAA > R1,800,000]",
                EmpiricaScoreLowerBound = 650,
                EmpiricaUpperBound = 700,
                RequiredLTV = 79,
                PurchasePrice = 2278483f,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 75000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_1_Self_Employed_LAA_R1800000,
                EmploymentType = EmploymentTypeEnum.SelfEmployed,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 0 SWD [Emp 575-595]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 595,
                RequiredLTV = 69,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_0_SWD_Emp_575_595,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 1 SWD [Emp 575-600]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 600,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_1_SWD_Emp_575_600,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 2 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.007,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_2_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 2 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_2_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 2 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_2_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 3 SWD [Emp 575-600]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 600,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_3_SWD_Emp_575_600,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 4 SWD [Emp 575-600]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 600,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_4_SWD_Emp_575_600,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 5 SWD [Emp 575-600]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 600,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_5_SWD_Emp_575_600,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.009,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.006,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 6 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_6_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.01,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.007,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 7 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_7_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.013,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.008,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 8 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_8_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.013,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.008,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 9 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_9_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 10 SWD [Emp 595-600]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 629,
                RequiredLTV = 100,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_10_SWD_Emp_595_600,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 12 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 69,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_12_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 13 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_13_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 13 SWD [Emp 595-629]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 629,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_13_SWD_Emp_595_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 14 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_14_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 14 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_14_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 14 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_14_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 14 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_14_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 15 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_15_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 15 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_15_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 15 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_15_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 15 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_15_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 16 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_16_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 16 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_16_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 16 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_16_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 16 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_16_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 17 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 100,
                ExpectedAdjustment = 0.009,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_17_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 17 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 100,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_17_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 17 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 100,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_17_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 17 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 100,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 20000f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_17_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 18 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_18_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 18 SWD [Emp 595-629]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 629,
                RequiredLTV = 79,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_18_SWD_Emp_595_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 19 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_19_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 19 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_19_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 19 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.002,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_19_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 19 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 84,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_19_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 20 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.005,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_20_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 20 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_20_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 20 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_20_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 20 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 89,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_20_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 21 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.006,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_21_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 21 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.004,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_21_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 21 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_21_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 21 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 94,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_21_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 22 SWD [Emp 575-594]",
                EmpiricaScoreLowerBound = 0,
                EmpiricaUpperBound = 594,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.011,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_22_SWD_Emp_575_594,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 22 SWD [Emp 595-609]",
                EmpiricaScoreLowerBound = 595,
                EmpiricaUpperBound = 609,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.006,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_22_SWD_Emp_595_609,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 22 SWD [Emp 610-629]",
                EmpiricaScoreLowerBound = 610,
                EmpiricaUpperBound = 629,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.003,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_22_SWD_Emp_610_629,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            testCases.Add(new PricingForRiskTest
            {
                Description = "CM54 Category 22 SWD [Emp 630-649]",
                EmpiricaScoreLowerBound = 630,
                EmpiricaUpperBound = 649,
                RequiredLTV = 99,
                ExpectedAdjustment = 0.001,
                HouseholdIncome = 19999f,
                ExpectedRateAdjustmentElementDescription = RateAdjustmentElements.CM54_Category_22_SWD_Emp_630_649,
                EmploymentType = EmploymentTypeEnum.SalariedWithDeductions,
                ExpectedFinancialAdjustmentToBeApplied = FinancialAdjustmentTypeSourceEnum.PricingForRisk_InterestRateAdjustment,
                GEPFfunded = true
            });

            return testCases;
        }
    }
}
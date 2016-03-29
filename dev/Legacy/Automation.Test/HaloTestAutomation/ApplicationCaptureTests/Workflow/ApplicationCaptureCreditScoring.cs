//using System;
//using System.Threading;
//using BuildingBlocks;
//using BuildingBlocks.Assertions;
//using BuildingBlocks.FLOBO;
//using BuildingBlocks.Navigation.FLOBO.Common;
//using BuildingBlocks.Presenters.Common;
//using BuildingBlocks.Presenters.LegalEntity;
//using BuildingBlocks.Presenters.Origination;
//using BuildingBlocks.Services.Contracts;
//using CommonData.Constants;
//using CommonData.Enums;
//using NUnit.Framework;
//using SQLQuerying;
//using Navigation = BuildingBlocks.Navigation;

//namespace ApplicationCaptureTests.CreditScoring
//{
//    //[TestFixture, RequiresSTA]
//    public class ApplicationCaptureCreditScoring : ApplicationCaptureTests.TestBase<BasePage>
//    {
//        TestBrowser browser;

//        protected override void OnTestFixtureSetup()
//        {
//            base.OnTestFixtureSetup();
//            browser = new TestBrowser(TestUsers.BranchConsultant10);
//        }

//        #region CreditScoringTests

//        /// <summary>
//        /// Creates our test cases required for Credit Scoring tests
//        /// </summary>
//        /// <param name="testIdentifier"></param>
//        /// <param name="username"></param>
//        /// <param name="password"></param>
//        /// <param name="loanType"></param>
//        /// <param name="product"></param>
//        /// <param name="marketValue"></param>
//        /// <param name="existingLoan"></param>
//        /// <param name="cashDeposit"></param>
//        /// <param name="cashOut"></param>
//        /// <param name="employmentType"></param>
//        /// <param name="term"></param>
//        /// <param name="percentageToFix"></param>
//        /// <param name="capitaliseFees"></param>
//        /// <param name="interestOnly"></param>
//        /// <param name="houseHoldIncome"></param>
//        /// <param name="legalEntityType"></param>
//        /// <param name="legalEntityRole"></param>
//        /// <param name="firstname"></param>
//        /// <param name="surname"></param>
//        /// <param name="companyName"></param>
//        [Test, Sequential, Description("Create a basic Application and Lead at the Application Capture stage.")]
//        public void _057_CreateCreditScoringTestCases(
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "TestIdentifier")] string testIdentifier,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "Username")] string username,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "Password")] string password,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "LoanType")] string loanType,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "Product")] string product,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "MarketValue")] string marketValue,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "ExistingLoan")] string existingLoan,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "CashDeposit")] string cashDeposit,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "CashOut")] string cashOut,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "EmploymentType")] string employmentType,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "Term")] string term,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "PercentageToFix")] string percentageToFix,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "CapitaliseFees")] string capitaliseFees,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "InterestOnly")] string interestOnly,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "HouseHoldIncome")] string houseHoldIncome,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "LegalEntityType")] string legalEntityType,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "LegalEntityRole")] string legalEntityRole,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "Firstname")] string firstname,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "Surname")] string surname,
//            [ValueSource(typeof(CreditScoringCreateSequentialData), "CompanyName")] string companyName)
//        {
//            int offerKey = 0;

//            try
//            {
//                const string phoneCode = "031";
//                const string phoneNumber = "1234567";

//                //Application Calculator
//                Navigation.Helper.Menu(browser);
//                Navigation.CalculatorsNode.Calculators(browser, CalculatorNodeType.ApplicationCalculator);

//                switch (loanType)
//                {
//                    case "New purchase":
//                        browser.Page<Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(product, marketValue, cashDeposit, employmentType,
//                            term, Convert.ToBoolean(interestOnly), percentageToFix, houseHoldIncome, ButtonType.CreateApplication);
//                        break;
//                    case "Switch loan":
//                        browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Switch(product, marketValue, existingLoan, cashOut,
//                            employmentType, term, percentageToFix, Convert.ToBoolean(capitaliseFees), Convert.ToBoolean(interestOnly),
//                            houseHoldIncome, ButtonType.CreateApplication);
//                        break;
//                    case "Refinance":
//                        browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Refinance(product, marketValue, cashOut, employmentType,
//                            term, percentageToFix, Convert.ToBoolean(capitaliseFees), Convert.ToBoolean(interestOnly), houseHoldIncome,
//                            ButtonType.CreateApplication);
//                        break;
//                }

//                if (legalEntityType == "Natural Person")
//                {
//                    browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(legalEntityRole, true, IDNumbers.GetNextIDNumber(),
//                        SalutationType.Mr, "auto", firstname, surname, "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown",
//                        CitizenType.SACitizenNonResident, "auto", null, null, "Unknown", Language.English, null,
//                        phoneCode, phoneNumber, null, null, null, null, null, null, true, false, false, false, false, ButtonType.Next);
//                    offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
//                }
//                else
//                {
//                    browser.Page<LegalEntityDetails>().AddLegalEntityCompany(legalEntityType, companyName, phoneCode, phoneNumber);
//                    offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey(); ;
//                }
//            }
//            // Let NUnit catch any exceptions
//            finally
//            {
//                //Commit OfferKey
//                Service<ICommonService>().CommitOfferKeyForTestIdentifier(testIdentifier, offerKey);
//            }
//        }

//        /// <summary>
//        /// This is a sequential test that submits cases out of the Application Capture state so that they can have a Credit Score
//        /// generated against the application.
//        /// </summary>
//        /// <param name="testIdentifier">TestIdentifier</param>
//        /// <param name="username">Branch Consultant User</param>
//        /// <param name="password">Password</param>
//        /// <param name="loanType">Switch/Refinance/New Purchase</param>
//        /// <param name="mainApplicantLegalEntityType">Natural Person/Company etc</param>
//        /// <param name="houseHoldIncome">Household Income</param>
//        /// <param name="aggregateDecision">Expected Aggregate Decision</param>
//        [Test, Sequential, Description("Verify that a Branch Consultant can perform the 'Submit Application' action at 'Application Capture' state, which moves the case to the next state depending on the LoanType")]
//        public void _058_CreditScoringSubmitApplicationTests(
//            [ValueSource(typeof(CreditScoringSequentialData), "TestIdentifier")] string testIdentifier,
//            [ValueSource(typeof(CreditScoringSequentialData), "Username")] string username,
//            [ValueSource(typeof(CreditScoringSequentialData), "Password")] string password,
//            [ValueSource(typeof(CreditScoringSequentialData), "LoanType")] string loanType,
//            [ValueSource(typeof(CreditScoringSequentialData), "LegalEntityType")] string mainApplicantLegalEntityType,
//            [ValueSource(typeof(CreditScoringSequentialData), "HouseHoldIncome")] string houseHoldIncome,
//            [ValueSource(typeof(CreditScoringSequentialData), "AggregateDecision")] string aggregateDecision)
//        {
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            BuildingBlocks.Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);

//            const string phoneCode = "031";
//            const string phoneNumber = "1234567";
//            // Get LegalEntityLegalNames of all Main Applicant and Suretor LegalEntities on Offer
//            QueryResults results = Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);

//            // Iterate through Main Applicant and Suretor LegalEntities on Offer and update LegalEntity Information
//            for (int row = 0; row < results.RowList.Count; row++)
//            {
//                string applicantIdentifier = results.Rows(row).Column("ApplicantIdentifier").Value;
//                string offerRoleType = results.Rows(row).Column("OfferRoleType").Value;
//                string firstname = results.Rows(row).Column("ApplicantType").Value;
//                string surname = testIdentifier;

//                string idNumber = string.Empty;
//                int legalEntityKey;

//                if (row == 0)
//                {
//                    var legalEntity = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey);
//                    legalEntityKey = legalEntity.Rows(row).Column("LegalEntityKey").GetValueAs<int>();
//                    //Companies/Trusts/CC's
//                    if (legalEntity.Rows(row).Column("LegalEntityTypeKey").GetValueAs<int>() != 2)
//                    {
//                        idNumber = "DNE";
//                    }
//                    //Natural Persons
//                    else if (legalEntity.Rows(row).Column("LegalEntityTypeKey").GetValueAs<int>() == 2)
//                    {
//                        idNumber = legalEntity.Rows(row).Column("IDNumber").Value;
//                        //update current LE
//                        browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
//                        browser.Navigate<LegalEntityNode>().LegalEntityDetails(NodeType.Update);
//                        browser.Page<LegalEntityDetailsUpdateApplicant>().UpdateLegalEntityDetails_NaturalPerson(null, true, null, SalutationType.Mr, "auto",
//                        firstname, surname, "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto",
//                        null, null, "Unknown", Language.English, null, phoneCode, phoneNumber, null, null, null, null, null, null, true,
//                        false, false, false, false, ButtonType.Update);
//                    }
//                }
//                else
//                {
//                    idNumber = IDNumbers.GetNextIDNumber();

//                    browser.Navigate<ApplicantsNode>().Applicants(NodeType.Add);

//                    switch (offerRoleType)
//                    {
//                        case OfferRoleTypes.LeadMainApplicant:
//                            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(offerRoleType, true, idNumber,
//                                SalutationType.Mr, "auto", firstname, surname, "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto",
//                                null, null, "Unknown", Language.English, null, phoneCode, phoneNumber, null, null, null, null, null, null,
//                                true, false, false, false, false, ButtonType.Next);
//                            break;
//                        case OfferRoleTypes.LeadSuretor:
//                            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(offerRoleType, true, idNumber, SalutationType.Mr,
//                                "auto", firstname, surname, "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto",
//                                null, null, "Unknown", Language.English, null, phoneCode, phoneNumber, null, null, null, null, null, null,
//                                true, false, false, false, false, ButtonType.Next);
//                            break;
//                    }

//                    QueryResults legalEntitys = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey);
//                    legalEntityKey = legalEntitys.Rows(row).Column("LegalEntityKey").GetValueAs<int>();
//                }

//                base.Service<ICreditScoringService>().CommitCreditScoringTestLegalEntityIDNumber(Convert.ToInt32(applicantIdentifier), idNumber);

//                browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
//            }

//            //this will clean up the data and set the income contributor flags/correct income values
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(Convert.ToInt32(offerKey), testIdentifier);

//            results = Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);

//            for (int row = 0; row < results.RowList.Count; row++)
//            {
//                string idNumber = results.Rows(row).Column("LegalEntityID").Value;
//                string applicantDecision = results.Rows(row).Column("ApplicantDecision").Value;
//                if (applicantDecision != "No Score - No ITC" && idNumber != "DNE")
//                {
//                    //Insert ITC
//                    Service<ICreditScoringService>().InsertCSDecision(offerKey, idNumber, applicantDecision);
//                }
//            }
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, true);
//            //Check the correct aggregate decision
//            browser.Page<CreditScoringSummaryFull>().AssertAggregateDecision("Submit Application", aggregateDecision);
//            //Check that the correct legal entities have been made Primary/Secondary Applicants
//            CreditScoringAssertions.AssertPrimaryAndSecondaryApplicants(testIdentifier);
//            browser.Page<CreditScoringSummaryFull>().SelectRowFromScoreGrid("Submit Application");
//            //We need to check the Emperica and SBC Scores
//            for (int row = 0; row < results.RowList.Count; row++)
//            {
//                //we only need to check the scores for our primary and secondary applicants
//                string applicantType = results.Rows(row).Column("ApplicantType").Value;
//                if (applicantType == CSApplicantType.Primary || applicantType == CSApplicantType.Secondary)
//                {
//                    string idNumber = results.Rows(row).Column("LegalEntityID").Value;
//                    string applicantName = Service<ILegalEntityService>().GetLegalEntityLegalNameByIDNumber(idNumber);
//                    int expectedEmpiricaScore = results.Rows(row).Column("ApplicantEmpiricaScore").GetValueAs<int>();
//                    int expectedSBCScore = results.Rows(row).Column("ApplicantSBCScore").GetValueAs<int>();
//                    string expectedApplicantDecision = results.Rows(row).Column("ApplicantDecision").Value;
//                    browser.Page<CreditScoringSummaryFull>().AssertApplicantDecisions(applicantName, expectedEmpiricaScore, expectedSBCScore,
//                        expectedApplicantDecision);
//                }
//            }
//            //We need to check that the correct reasons have been added for the decisions made
//            CreditScoringAssertions.AssertApplicantDecisionReasons(testIdentifier, offerKey, Workflows.ApplicationCapture,
//                WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
//        }

//        /// <summary>
//        /// Checks that the SBC section of the score card is being correctly validated against the values that we expect. This
//        /// test looks for an out of range value in the AT107 tag and then reports the correct error to the user.
//        /// </summary>
//        [Test, Description(@"Checks that the SBC section of the score card is being correctly validated against the values that we expect. This
//		test looks for an out of range value in the AT107 tag and then reports the correct error to the user.")]
//        public void _059_CreditScoringITCValidSBCAT107()
//        {
//            const string testIdentifier = "ITCValidSBC";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            BuildingBlocks.Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(Convert.ToInt32(offerKey), testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCValidSBC);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
//            @"CreditScoring ITCValidSBC has an out-of-range SBC attribute: AT107 and will not be used for CreditScoring");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.NocreditscoreMissingIncompleteSBCinformation, ReasonType.CreditScoringQuery, offerKey,
//                GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// The empirica score needs to be validated before the application can be credit scored. This test will insert a blank
//        /// value into the empirica field in the XML and ensure that the correct warning message is displayed to the user.
//        /// </summary>
//        [Test, Description(@"The empirica score needs to be validated before the application can be credit scored. This test will insert a blank
//		value into the empirica field in the XML and ensure that the correct warning message is displayed to the user.")]
//        public void _060_CreditScoringValidEmpiricaScore()
//        {
//            const string testIdentifier = "ITCValidEmpericaScore";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            BuildingBlocks.Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(Convert.ToInt32(offerKey), testIdentifier);
//            //insert the knockout itc
//            QueryResults results = Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCValidEmpericaScore);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
//            @"CreditScoring ITCValidEmpericaScore has missing empirica score and will not be used for credit scoring.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.NocreditscoreEmpiricaScorenotavailable, ReasonType.CreditScoringQuery, offerKey,
//                GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// A portion of the XML contains details of the disputes against the applicants on the application. This rule will check this
//        /// section and ensure that if a dispute has been indicated that the correct warning message is displayed to the user.
//        /// </summary>
//        [Test, Description(@"A portion of the XML contains details of the disputes against the applicants on the application. This rule will check this
//		section and ensure that if a dispute has been indicated that the correct warning message is displayed to the user.")]
//        public void _061_CreditScoringITCDisputeIndicated()
//        {
//            const string testIdentifier = "ITCDisputeIndicated";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            BuildingBlocks.Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(Convert.ToInt32(offerKey), testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCDisputeIndicated);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists("CreditScoring ITCDisputeIndicated has disputes indicated.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleDisputes, ReasonType.CreditScoringQuery, offerKey, GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// A portion of the XML contains details of the client's worst ever payment profile status. This rule will check this
//        /// section and ensure that if the status is a particular value then a warning message is displayed and the case is
//        /// given a credit decision of DECLINE.
//        /// </summary>
//        [Test, Description(@"A portion of the XML contains details of the client's worst ever payment profile status. This rule will check this
//		section and ensure that if the status is a particular value then a warning message is displayed and the case is
//		given a credit scoring aggregate decision of DECLINE.")]
//        public void _062_CreditScoringITCAccountCustomerWEPPStatusDecline()
//        {
//            const string testIdentifier = "ITCAccountCustomerWEPPStatusDecline";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            BuildingBlocks.Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value,
//                CSKnockOutType.ITCAccountCustomerWEPPStatusDecline);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCAccountCustomerWEPPStatusDecline has a poor payment profile.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason("ITC Knockout Rule - WorstEverPaymentProfile", ReasonType.CreditScoringDecline, offerKey,
//                GenericKeyType.Offer_OfferKey);
//            //Check the correct aggregate decision
//            browser.Page<CreditScoringSummaryFull>().AssertAggregateDecision("Submit Application", CreditScoreDecision.Decline);
//        }

//        /// <summary>
//        /// This test checks the portion of the ITC XML that gives an indicator of whether or not the client has any defaults, if they
//        /// do then a warning message is displayed to the user.
//        /// </summary>
//        [Test, Description(@"This test checks the portion of the ITC XML that gives an indicator of whether or not the client has any defaults, if they
//		do then a warning message is displayed to the user.")]
//        public void _063_CreditScoringITCAccountDefaultsIndicated()
//        {
//            const string testIdentifier = "ITCAccountDefaultsIndicated";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            BuildingBlocks.Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCAccountDefaultsIndicated);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCAccountDefaultsIndicated has defaults currently against them.");
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleDefaults, ReasonType.CreditScoringDecline, offerKey, GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// This test checks the portion of the ITC XML that gives an indicator of whether or not the client has any legal notices
//        /// currently issued against them, if so then a warning message is displayed to the user.
//        /// </summary>
//        [Test, Description(@"This test checks the portion of the ITC XML that gives an indicator of whether or not the client has any legal notices
//		currently issued against them, if so then a warning message is displayed to the user.")]
//        public void _064_CreditScoringITCAccountLegalNoticesIndicated()
//        {
//            const string testIdentifier = "ITCAccountLegalNoticesIndicated";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCAccountLegalNoticesIndicated);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCAccountLegalNoticesIndicated has legal notices currently against them.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleNotices, ReasonType.CreditScoringDecline, offerKey, GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// This test checks the portion of the ITC XML that gives an indicator of whether or not the client has any judgments
//        /// currently against them, if so then a warning message is displayed to the user.
//        /// </summary>
//        [Test, Description(@"This test checks the portion of the ITC XML that gives an indicator of whether or not the client has any judgments
//		currently against them, if so then a warning message is displayed to the user.")]
//        public void _065_CreditScoringITCAccountJudgementsIndicated()
//        {
//            const string testIdentifier = "ITCAccountJudgementsIndicated";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(Convert.ToInt32(offerKey), testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCAccountJudgementsIndicated);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCAccountJudgementsIndicated has Judgements currently against them.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleJudgments, ReasonType.CreditScoringDecline, offerKey, GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// This test checks the portion of the ITC XML that gives an indicator of whether or not the client is under debt review.
//        /// if the client is under debt review then a warning message is displayed to the user.
//        /// </summary>
//        [Test, Description(@"This test checks the portion of the ITC XML that gives an indicator of whether or not the client is under debt review.
//		if the client is under debt review then a warning message is displayed to the user.")]
//        public void _066_CreditScoringITCDebtReviewIndicated()
//        {
//            const string testIdentifier = "ITCDebtReviewIndicated";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCDebtReviewIndicated);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCDebtReviewIndicated is under debt review.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleDebtReview, ReasonType.CreditScoringDecline, offerKey, GenericKeyType.Offer_OfferKey);
//        }

//        /// <summary>
//        /// This test checks a portion of the ITC XML that gives an indicator of why the client has no Empirica Score value. The test
//        /// executes the insert of the XML multiple times, inserting a new reason code each time, and then checking that the correct
//        /// validation message is displayed.
//        /// </summary>
//        [Test, Description(@"This test checks a portion of the ITC XML that gives an indicator of why the client has no Empirica Score value. The test
//		executes the insert of the XML multiple times, inserting a new reason code each time, and then checking that the correct
//		validation message is displayed.")]
//        public void _067_CreditScoringITCValidEmpiricaScoreReasonCodes()
//        {
//            const string testIdentifier = "ITCValidEmpiricaScoreReasonCodes";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCValidEmpericaScoreCodeD);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            string error =
//                "CreditScoring ITCValidEmpiricaScoreReasonCodes has missing empirica score and will not be used for credit scoring. (Reason code D - Consumer is deceased)";
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
//            error);
//            browser.Page<WorkflowYesNo>().DoNotContinueWithWarnings(); ;
//            //insert a different reason code
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCValidEmpericaScoreCodeL);
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            error =
//                "CreditScoring ITCValidEmpiricaScoreReasonCodes has missing empirica score and will not be used for credit scoring. (Reason code L - Legal)";
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
//            error);
//            browser.Page<WorkflowYesNo>().DoNotContinueWithWarnings(); ;
//            //insert a different reason code
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCValidEmpericaScoreCodeN);
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            error =
//                "CreditScoring ITCValidEmpiricaScoreReasonCodes has missing empirica score and will not be used for credit scoring. (Reason code N - Insufficient data to be scored)";
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
//            error);
//            browser.Page<WorkflowYesNo>().DoNotContinueWithWarnings(); ;
//            //insert a different reason code
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value, CSKnockOutType.ITCValidEmpericaScoreCodeZ);
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            error =
//                "CreditScoring ITCValidEmpiricaScoreReasonCodes has missing empirica score and will not be used for credit scoring. (Reason code Z - EMPIRICA is not available)";
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
//            error);
//        }

//        /// <summary>
//        /// A portion of the XML contains details of the client's worst ever payment profile status. This rule will check this
//        /// section and ensure that if the status is a particular value then a warning message is displayed and the case is
//        /// given a credit decision of REFER.
//        /// </summary>
//        [Test, Description(@"A portion of the XML contains details of the client's worst ever payment profile status. This rule will check this
//		section and ensure that if the status is a particular value then a warning message is displayed and the case is
//		given a credit decision of REFER.")]
//        public void _068_CreditScoringITCAccountCustomerWEPPStatusRefer()
//        {
//            const string testIdentifier = "ITCAccountCustomerWEPPStatusRefer";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSDecision(offerKey, results.Rows(0).Column("LegalEntityID").Value,
//                CSKnockOutType.ITCAccountCustomerWEPPStatusRefer);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCAccountCustomerWEPPStatusRefer has a poor payment profile.");
//            browser.Page<BasePage>().DomainWarningClickYes();
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleWorstEverPaymentProfile,
//                ReasonType.CreditScoringDecline, offerKey, GenericKeyType.Offer_OfferKey);
//            //Check the correct aggregate decision
//            browser.Page<CreditScoringSummaryFull>().AssertAggregateDecision("Submit Application", CreditScoreDecision.Refer);
//        }

//        /// <summary>
//        /// A portion of the XML contains details of the client's worst ever payment profile status. This rule will check this
//        /// section and ensure that if the status is a particular value then a warning message is displayed and the case is
//        /// given a credit decision of DECLINE.
//        /// </summary>
//        [Test, Description(@"A portion of the XML contains details of the client's worst ever payment profile status. This rule will check this
//		section and ensure that if the status is a particular value then a warning message is displayed and the case is
//		given a credit decision of DECLINE")]
//        public void _069_CreditScoringITCAccountCustomerWEPPStatusAlpha()
//        {
//            const string testIdentifier = "ITCAccountCustomerWEPPStatusAlpha";
//            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
//            Navigation.WorkFlowsNode.WorkFlows(browser);
//            WorkflowSuperSearch.SearchByOfferKey(browser, offerKey);
//            //update the ID Number
//            Service<ICreditScoringService>().UpdateLegalEntityIDNumber(offerKey, testIdentifier);
//            //we need to clean up the data
//            base.Service<ICreditScoringService>().CleanUpCreditScoringOfferData(offerKey, testIdentifier);
//            //insert the knockout itc
//            QueryResults results = base.Service<ICreditScoringService>().GetCreditScoringTestDataByTestIdentifier(testIdentifier);
//            base.Service<ICreditScoringService>().InsertCSKnockout(offerKey, results.Rows(0).Column("LegalEntityID").Value,
//                CSKnockOutType.ITCAccountCustomerWEPPStatusAlpha);
//            //submit the application
//            browser.Navigate<Navigation.ApplicationCapture>().LoanNode();
//            browser.Navigate<Navigation.ApplicationCapture>().SubmitApplication();
//            browser.Page<WorkflowYesNo>().Confirm(true, false);
//            //Check that the warning message is displayed
//            browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"CreditScoring ITCAccountCustomerWEPPStatusAlpha has a poor payment profile.");
//            Thread.Sleep(5000);
//            ReasonAssertions.AssertReason(ReasonDescription.ITCKnockoutRuleWorstEverPaymentProfile,
//                ReasonType.CreditScoringDecline, offerKey, GenericKeyType.Offer_OfferKey);
//            //Check the correct aggregate decision
//            browser.Page<CreditScoringSummaryFull>().AssertAggregateDecision("Submit Application", CreditScoreDecision.Decline);
//        }

//        #endregion CreditScoringTests
//    }
//}
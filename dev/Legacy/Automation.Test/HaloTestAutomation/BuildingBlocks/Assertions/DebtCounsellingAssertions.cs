using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// contains assertions specific to Debt Counselling
    /// </summary>
    public static class DebtCounsellingAssertions
    {
        private static readonly IDebtCounsellingService debtCounsellingService;
        private static readonly IExternalRoleService externalRoleService;
        private static readonly ILegalEntityService legalEntityService;
        private static readonly IX2WorkflowService x2Service;
        private static readonly IProposalService proposalService;
        private static readonly ICourtDetailsService courtDetailsService;
        private static readonly IClientEmailService clientEmailService;

        static DebtCounsellingAssertions()
        {
            debtCounsellingService = ServiceLocator.Instance.GetService<IDebtCounsellingService>();
            externalRoleService = ServiceLocator.Instance.GetService<IExternalRoleService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            proposalService = ServiceLocator.Instance.GetService<IProposalService>();
            courtDetailsService = ServiceLocator.Instance.GetService<ICourtDetailsService>();
            clientEmailService = ServiceLocator.Instance.GetService<IClientEmailService>();
        }

        /// <summary>
        /// Asserts that a set of accounts belong to the same debt counselling group
        /// </summary>
        /// <param name="accountKeyList">List of Accounts</param>
        public static void AssertDebtCounsellingGroup(List<int> accountKeyList)
        {
            int debtCounsellingGroupKeyMain = 0;
            if (accountKeyList.Count > 1)
            {
                //get the debtcounsellinggroupkey
                var results = debtCounsellingService.GetDebtCounsellingByAccountKeyAndStatus(accountKeyList[0], DebtCounsellingStatusEnum.Open);
                debtCounsellingGroupKeyMain = results.Rows(0).Column("DebtCounsellingGroupKey").GetValueAs<int>();
                results.Dispose();
                for (int i = 1; i < accountKeyList.Count; i++)
                {
                    results = debtCounsellingService.GetDebtCounsellingByAccountKeyAndStatus(accountKeyList[i], DebtCounsellingStatusEnum.Open);
                    int debtCounsellingGroupKey = results.Rows(0).Column("DebtCounsellingGroupKey").GetValueAs<int>();
                    Assert.AreEqual(debtCounsellingGroupKeyMain, debtCounsellingGroupKey, "Debt Counselling Group Keys are different");
                    results.Dispose();
                }
            }
            else if (accountKeyList.Count == 1)
            {
                //get the debtcounsellinggroupkey
                var results = debtCounsellingService.GetDebtCounsellingByAccountKeyAndStatus(accountKeyList[0], DebtCounsellingStatusEnum.Open);
                debtCounsellingGroupKeyMain = results.Rows(0).Column("DebtCounsellingGroupKey").GetValueAs<int>();
                Assert.Greater(debtCounsellingGroupKeyMain, 0);
            }
        }

        /// <summary>
        /// Asserts the current state of a debt counselling case in the workflow.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="expectedWorkflowState">expected workflow state</param>
        public static void AssertX2State(int debtCounsellingKey, string expectedWorkflowState)
        {
            var results = x2Service.GetDebtCounsellingInstanceDetails(debtCounsellingKey);
            Logger.LogAction("Asserting debt counselling case {0} is at state {1}", debtCounsellingKey, expectedWorkflowState);
            var actualState = results.Rows(0).Column("StateName").Value;
            Assert.AreEqual(expectedWorkflowState, actualState, string.Format("Case not at expected debt counselling state. Expected: {0}; was {1}", expectedWorkflowState, actualState));
            results.Dispose();
        }

        /// <summary>
        /// Asserts the current state of a debt counselling case in the workflow.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="reference">expected reference</param>
        public static void AssertReference(int debtCounsellingKey, string reference)
        {
            QueryResults results = debtCounsellingService.GetReferenceByDebtCounsellingKey(debtCounsellingKey);
            Logger.LogAction("Asserting debt counselling case {0} has the correct reference {1}", debtCounsellingKey, reference);
            Assert.AreEqual(reference, results.Rows(0).Column("ReferenceNumber").Value);
            results.Dispose();
        }

        /// <summary>
        /// Asserts the current state of a debt counselling case in the workflow.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="workflowState">expected workflow state</param>
        public static void AssertX2StateByAccountKey(int accountKey, string workflowState)
        {
            QueryResults results = debtCounsellingService.GetDebtCounsellingByAccountKeyAndStatus(accountKey);

            if (results.RowList.Count > 0)
                AssertX2State(results.Rows(0).Column("DebtCounsellingKey").GetValueAs<int>(), workflowState);
            else
                throw new WatiN.Core.Exceptions.WatiNException(String.Format(@"No DebtCounsellingKey found for Account {0}", accountKey));

            results.Dispose();
        }

        /// <summary>
        /// Asserts the debtcounselling case exist and that the status is active.
        /// </summary>
        /// <param name="accountKeys">list of one or more accounts to assert</param>
        public static void AssertDebtCounselingCase(params int[] accountKeys)
        {
            QueryResults results;
            //Debt Counselling case is always valid unless proven below
            bool debtCounsellingCaseValid = true;
            int accountkeyOfInvalidCase = 0;
            //if there is one account then assert for the one account.
            if (accountKeys.Length == 1)
            {
                //Get an active debtcounseling case
                results = debtCounsellingService.GetDebtCounsellingByAccountKeyAndStatus(accountKeys[0]);
                if (!results.HasResults)
                {
                    accountkeyOfInvalidCase = accountKeys[0];
                    debtCounsellingCaseValid = false;
                }
            }
            //if there are more than one account then assert them all.
            else
            {
                foreach (int accountkey in accountKeys)
                {
                    //Get the active debt counselling case
                    results = debtCounsellingService.GetDebtCounsellingByAccountKeyAndStatus(accountkey);
                    //if there is no active debt counselling case then break out setting debtCounsellingCaseValid as false.
                    if (!results.HasResults)
                    {
                        accountkeyOfInvalidCase = accountkey;
                        debtCounsellingCaseValid = false;
                        break;
                    }
                }
            }
            Assert.IsTrue(debtCounsellingCaseValid, String.Format("Account: {0}, doesn't have a valid debt counselling case.", accountkeyOfInvalidCase));
        }

        /// <summary>
        /// This method will assert that the search results returned by the debtcounsellingcreatecase presenter is valid,
        /// by comparing everything that was expected to be in the search per debtcounsellingtestcase
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="passportIDnumber"></param>
        /// <param name="accountsOfImportance"></param>
        public static void AssertCreateCaseSearch(string testIdentifier, string passportIDnumber, List<string> accountsOfImportance)
        {
            QueryResults results = debtCounsellingService.GetAccountsForDCTestIdentifier(testIdentifier);
            string accKey = String.Empty;
            //Positive test
            //Assert that everything in the query results exist in the accounts of importance list.
            foreach (QueryResultsRow row in results.RowList)
            {
                accKey = row.Column("accountkey").Value;
                bool searchContainsAccountKey = accountsOfImportance.Contains(row.Column("accountkey").Value);
                Assert.True(searchContainsAccountKey, String.Format("Account:{0} was not returned by the search.", accKey));
            }
            //Negative test
            //Assert that everything in the accounts of importance list exist in the query results.
            //Example would be if the search returns more accounts than the query when it shouldn't
            foreach (string acc in accountsOfImportance)
            {
                QueryResultsRow row = results.FindRowByExpression<string>("accountkey", true, acc)[0];
                Assert.NotNull(row, String.Format("Account:{0} should not have been returned by the search.", accKey));
            }
        }

        /// <summary>
        /// Checks if all values in any Proposal filtered on AccountKey and ProposalStatusKey match the expected values
        /// </summary>
        /// <param name="accountKey">AccountKey to filter by</param>
        /// <param name="propType">Expected Proposal ProposalTypeKey</param>
        /// <param name="status">Expected Proposal ProposalStatusKey</param>
        /// <param name="hocInclusive">Expected Proposal HOCInclusive indicator</param>
        /// <param name="lifeInclusive">Expected Proposal LifeInclusive indicator</param>
        /// <param name="createdDate">Expected Proposal Creation Date</param>
        public static void AssertProposalExists(int accountKey, int expectedProposalTypeKey, int expectedProposalStatusKey, string expectedHOCInclusive,
             string expectedLifeInclusive, DateTime expectedCreateDate)
        {
            QueryResults dbResults = proposalService.GetLatestProposalRecordByAccountKey(accountKey, DebtCounsellingStatusEnum.Open);

            bool testResult = false;

            if (dbResults.RowList.Count > 0)
            {
                for (int row = 0; row < dbResults.RowList.Count; row++)
                {
                    for (int col = 0; col < dbResults.Rows(row).Count; col++)
                    {
                        string actualValue = dbResults.Rows(row).Column(col).Value;

                        string expectedValue;
                        switch (dbResults.Rows(row).Column(col).Name)
                        {
                            case "ProposalTypeKey":
                                expectedValue = expectedProposalTypeKey.ToString();
                                break;

                            case "ProposalStatusKey":
                                expectedValue = expectedProposalStatusKey.ToString();
                                break;

                            case "HOCInclusive":
                                if (expectedHOCInclusive == "Inclusive")
                                    expectedValue = "True";
                                else if (expectedHOCInclusive == "Exclusive")
                                    expectedValue = "False";
                                else
                                    expectedValue = "Invalid";
                                break;

                            case "LifeInclusive":
                                if (expectedLifeInclusive == "Inclusive")
                                    expectedValue = "True";
                                else if (expectedLifeInclusive == "Exclusive")
                                    expectedValue = "False";
                                else
                                    expectedValue = "Invalid";
                                break;

                            case "CreateDate":
                                expectedValue = expectedCreateDate.ToString(Formats.DateFormat2);
                                actualValue = DateTime.Parse(dbResults.Rows(row).Column(col).Value).ToString(Formats.DateFormat2);
                                break;

                            default:
                                expectedValue = "skip";
                                break;
                        }

                        if (actualValue == expectedValue)
                            testResult = true;
                        else if (expectedValue != "skip")
                        {
                            testResult = false;
                            break;
                        }
                    }

                    Logger.LogAction(@"Asserting the latest Proposal matches ProposalTypeKey: {0}, ProposalStatusKey: {1}, HOCInclusive {2}, LifeInclusive {3}, CreateDate {4}, exists for AccountKey: {5}",
                        expectedProposalTypeKey, expectedProposalStatusKey, expectedHOCInclusive, expectedLifeInclusive, expectedCreateDate.ToString(Formats.DateFormat2),
                        accountKey);

                    Assert.IsTrue(testResult, string.Format(@"The latest Proposal did not match ProposalTypeKey: {0}, ProposalStatusKey: {1}, HOCInclusive {2}, LifeInclusive {3}, CreateDate {4}, or no Proposal could be found for AccountKey: {5} ",
                        expectedProposalTypeKey, expectedProposalStatusKey, expectedHOCInclusive, expectedLifeInclusive, expectedCreateDate.ToString(Formats.DateFormat2),
                        accountKey));
                }
            }
        }

        /// <summary>
        /// Checks that the debt counselling case status is correct
        /// </summary>
        /// <param name="debtCounsellingStatus"></param>
        /// <param name="debtCounsellingKey"></param>
        public static void AssertDebtCounsellingStatus(DebtCounsellingStatusEnum debtCounsellingStatus, int debtCounsellingKey)
        {
            var results = debtCounsellingService.GetDebtCounsellingRowByDebtCounsellingKey(debtCounsellingKey, debtCounsellingStatus);
            Logger.LogAction(string.Format(@"Asserting that the debt counselling case (debtCounsellingKey={0}) status is set to {1}",
                debtCounsellingKey, (int)debtCounsellingStatus));
            if (!results.HasResults)
            {
                Assert.Fail(string.Format("No debt counselling case found for {0} with a status of {1}", debtCounsellingKey, debtCounsellingStatus));
            }
        }

        /// <summary>
        /// Ensures that a record exists in the database for the debt counselling case with the details provided
        /// </summary>
        /// <param name="courtDetails">courtDetails</param>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        public static void AssertCourtDetailsExist(int debtCounsellingKey, Automation.DataModels.CourtDetails courtDetails)
        {
            QueryResults r = null;
            switch (courtDetails.hearingType)
            {
                case HearingType.Court:
                    r = courtDetailsService.GetCourtDetails(debtCounsellingKey, courtDetails);
                    break;

                case HearingType.Tribunal:
                    r = courtDetailsService.GetTribunalDetails(debtCounsellingKey, courtDetails);
                    break;
            }
            Logger.LogAction(string.Format(@"Asserting that court details of Hearing Type: {0}, Appearance Type: {1} have been added for DCKey: {2}",
                courtDetails.hearingType, courtDetails.appearanceType, debtCounsellingKey));
            if (!r.HasResults)
            {
                Assert.Fail(string.Format(@"No court details found for DCKey: {0}", debtCounsellingKey));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="pmtDate"></param>
        /// <param name="amt"></param>
        /// <param name="reviewDate"></param>
        public static void AssertPaymentReceivedDetails(int debtCounsellingKey, string pmtDate, decimal amt, string reviewDate)
        {
            var r = debtCounsellingService.GetDebtCounsellingRowByDebtCounsellingKey(debtCounsellingKey, DebtCounsellingStatusEnum.Open);
            decimal actualAmt = r.Rows(0).Column("PaymentReceivedAmount").GetValueAs<decimal>();
            DateTime actualPmtDate = r.Rows(0).Column("PaymentReceivedDate").GetValueAs<DateTime>();
            Assert.AreEqual(amt, actualAmt);
            StringAssert.AreEqualIgnoringCase(pmtDate, actualPmtDate.ToString(Formats.DateFormat));
        }

        /// <summary>
        /// This will assert that the SnapShot taken from the account is correct
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// /// <param name="expectedSnapShot"></param>
        public static void AssertDebtCounsellingAccountSnapShot(int debtCounsellingKey, QueryResults expectedSnapShot)
        {
            QueryResults currentSnapShotData = debtCounsellingService.GetDebtCounsellingAccountSnapShot(debtCounsellingKey);
            Assert.True(currentSnapShotData.HasResults, string.Format(@"No Snapshot found for debtcounsellingKey: {0}", debtCounsellingKey));

            int accountkey = expectedSnapShot.Rows(0).Column("accountkey").GetValueAs<int>();
            int expectedproductkey = expectedSnapShot.Rows(0).Column("rrr_productkey").GetValueAs<int>();
            int currentproductkey = currentSnapShotData.Rows(0).Column("productkey").GetValueAs<int>();

            Assert.AreEqual(expectedproductkey, currentproductkey,
                String.Format("Expected Productkey:{0}, but was ProductKey:{1}", expectedproductkey, currentproductkey));

            //Assert financial and mortgageloan data
            List<int> currentfinancialservicekeyList = currentSnapShotData.GetColumnValueList<int>("financialservicekey");
            foreach (int expectedfinancialservicekey in expectedSnapShot.GetColumnValueList<int>("financialservicekey"))
            {
                Assert.IsTrue(currentfinancialservicekeyList.Contains(expectedfinancialservicekey),
                    String.Format("FinancialServiceKey: {0}, is not present in the snapshot.", expectedfinancialservicekey));

                List<QueryResultsRow> expectedFinancialServiceMortgageLoan
                    = expectedSnapShot.FindRowByExpression<int>("financialservicekey", true, expectedfinancialservicekey);

                List<QueryResultsRow> currentFinancialServiceMortgageLoan
                    = currentSnapShotData.FindRowByExpression<int>("financialservicekey", true, expectedfinancialservicekey);

                //Can safely assume that there will always be one mortgageloan per financialservicekey
                QueryResultsRow expectedFinancialMortgageLoanRow = expectedFinancialServiceMortgageLoan[0];
                QueryResultsRow currentFinancialMortgageLoanRow = currentFinancialServiceMortgageLoan[0];

                double expectedactivemarketrate = expectedFinancialMortgageLoanRow.Column("activemarketrate").GetValueAs<double>();
                double expectedMarginKey = expectedFinancialMortgageLoanRow.Column("marginkey").GetValueAs<double>();
                int expectedresetconfigurationkey = expectedFinancialMortgageLoanRow.Column("resetconfigurationkey").GetValueAs<int>();
                int expectedremaininginstallments = expectedFinancialMortgageLoanRow.Column("remaininginstalments").GetValueAs<int>();

                double cuurentactivemarketrate = currentFinancialMortgageLoanRow.Column("activemarketrate").GetValueAs<double>();
                double currentMarginKey = currentFinancialMortgageLoanRow.Column("marginkey").GetValueAs<double>();
                int currentresetconfigurationkey = currentFinancialMortgageLoanRow.Column("resetconfigurationkey").GetValueAs<int>();
                int currentremaininginstallments = currentFinancialMortgageLoanRow.Column("remaininginstallments").GetValueAs<int>();

                Assert.AreEqual(expectedactivemarketrate, cuurentactivemarketrate,
                    String.Format("Expected BaseRate:{0}, but was BaseRate:{1}", expectedactivemarketrate, cuurentactivemarketrate));
                Assert.AreEqual(expectedMarginKey, currentMarginKey,
                    String.Format("Expected Productkey:{0}, but was ProductKey:{1}", expectedMarginKey, currentMarginKey));
                Assert.AreEqual(expectedresetconfigurationkey, currentresetconfigurationkey,
                    String.Format("Expected ResetConfigurationKey:{0}, but was ResetConfigurationKey:{1}", expectedresetconfigurationkey, currentresetconfigurationkey));
                Assert.AreEqual(expectedremaininginstallments, currentremaininginstallments,
                    String.Format("Expected RemainingInstallments:{0}, but was RemainingInstallments:{1}", expectedremaininginstallments, currentremaininginstallments));
            }

            //Assert Rate Overrides Types (Remember by default .NET sets a Int32 variable  = 0, so if the value in the database
            // was null then the following will be zero.
            List<int> currentFinancialAdjustmentTypeList = currentSnapShotData.GetColumnValueList<int>("FinancialAdjustmentTypeKey");
            List<int> expectedRateAdjustmentTypeList = expectedSnapShot.GetColumnValueList<int>("FinancialAdjustmentTypeKey");
            List<int> currentRateAdjustmentSourceList = currentSnapShotData.GetColumnValueList<int>("FinancialAdjustmentSourceKey");
            List<int> expectedRateAdjustmentSourceList = expectedSnapShot.GetColumnValueList<int>("FinancialAdjustmentSourceKey");

            if (!expectedRateAdjustmentTypeList.Contains(0) || !currentFinancialAdjustmentTypeList.Contains(0))
            {
                foreach (int expectedRateAdjustmentTypeKey in expectedRateAdjustmentTypeList)
                {
                    Assert.True(currentFinancialAdjustmentTypeList.Contains(expectedRateAdjustmentTypeKey),
                        String.Format("FinancialAdjustmentType: {0}, is not present in the snapshot.", expectedRateAdjustmentTypeKey));
                }
                foreach (int expectedRateAdjustmentSourceKey in expectedRateAdjustmentSourceList)
                {
                    Assert.True(currentRateAdjustmentSourceList.Contains(expectedRateAdjustmentSourceKey),
                        String.Format("FinancialAdjustmentSource: {0}, is not present in the snapshot.", expectedRateAdjustmentSourceKey));
                }
            }

            //Assert Valuations
            List<int> currentvaluationkeyList = currentSnapShotData.GetColumnValueList<int>("valuationkey");
            foreach (int expectedvaluationkey in expectedSnapShot.GetColumnValueList<int>("valuationkey"))
            {
                Assert.True(currentvaluationkeyList.Contains(expectedvaluationkey),
                    String.Format("ValuationKey: {0}, is not present in the snapshot.", expectedvaluationkey));
                //Will only ever have one active valuation
                List<QueryResultsRow> expectedvaluationRowList = expectedSnapShot.FindRowByExpression<int>("valuationkey", true, expectedvaluationkey);
                List<QueryResultsRow> currentvaluationRowList = currentSnapShotData.FindRowByExpression<int>("valuationkey", true, expectedvaluationkey);
                int expectedvaluationamount = expectedvaluationRowList[0].Column("valuationamount").GetValueAs<int>();
                int currentvaluationamount = currentvaluationRowList[0].Column("valuationamount").GetValueAs<int>();

                Assert.AreEqual(expectedvaluationamount, currentvaluationamount, String.Format("Expected valuationamount:{0}, but was valuationamount:{1}",
                    expectedvaluationamount, currentvaluationamount));
            }
        }

        /// <summary>
        /// Finds the instance id for the debt counselling key provided and then runs the AssertLatestWorkflowRoleAssignment assertions
        /// </summary>
        /// <param name="debtCounsellingKey">debtcounselling.debtCounsellingKey</param>
        /// <param name="adUserName">adUser.AdUserName</param>
        /// <param name="wfRoleType">WorkflowRoleTypeKey</param>
        /// <param name="checkInactiveRecords">TRUE = check all other WorkflowRoleAssignment records are inactive</param>
        /// <param name="checkWorklist"></param>
        public static void AssertLatestDebtCounsellingWorkflowRoleAssignment(int debtCounsellingKey, string adUserName, WorkflowRoleTypeEnum wfRoleType,
            bool checkInactiveRecords, bool checkWorklist)
        {
            if (adUserName.Contains("DCCCUser"))
                wfRoleType = WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD;
            int instanceid = x2Service.GetInstanceIDByDebtCounsellingKey(debtCounsellingKey);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(instanceid, adUserName, wfRoleType, checkInactiveRecords, checkWorklist);
        }

        /// <summary>
        /// This will assert whether all ricipientNames in the grid is correct for external roles.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="isSMS"></param>
        /// <param name="isEmail"></param>
        /// <param name="recipientNames"></param>
        public static void AssertClientCommunicationRecipientsByExternalRoleType(int debtCounsellingKey, List<string> recipientNames, ExternalRoleTypeEnum externalRoleType)
        {
            var expectedActiveExternalRoles =
                externalRoleService.GetActiveExternalRoleList(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, externalRoleType, true);

            foreach (var expectedExtRole in expectedActiveExternalRoles)
            {
                string toCompare = expectedExtRole.LegalEntityLegalName.RemoveWhiteSpace();

                // 'trading as' part needs to be removed from the legalentitylegalname
                if (toCompare.Contains("tradingas"))
                {
                    var split = toCompare.Split(new string[] { "tradingas" }, StringSplitOptions.RemoveEmptyEntries);
                    toCompare = split[0];
                }
                Assert.True(recipientNames.Contains(toCompare), String.Format("Legal Entity '{0}' should be displayed in the ricipients grid.",
                        expectedExtRole.LegalEntityLegalName));
            }
        }

        /// <summary>
        /// This will assert whether all ricipientNames in the grid is correct for legalentity roles.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="recipientNames"></param>
        public static void AssertClientCommunicationRecipientsByLegalEntityRoleType(int debtCounsellingKey, List<string> recipientNames, RoleTypeEnum roleType)
        {
            var debtcounsellingAcc = debtCounsellingService.GetDebtCounsellingAccount(debtCounsellingKey);
            var expectedLegalEntityRoles = legalEntityService.GetActiveLegalEntityRoles(debtcounsellingAcc.DebtCounsellingKey, roleType);
            foreach (var leRole in expectedLegalEntityRoles)
            {
                string toCompare = leRole.LegalEntityLegalName.RemoveWhiteSpace();
                Assert.True(recipientNames.Contains(toCompare),
                    String.Format("Legal Entity '{0}' should be displayed in the recipients grid.", leRole.LegalEntityLegalName));
            }
        }

        /// <summary>
        /// Assert that the right validation messages are
        /// </summary>
        public static void AssertClientCommunicationValidationMessages(string[] validationMessages, bool isSMS = false, bool isEmail = false)
        {
            var expectedvalidationMessageList = new List<string>();

            if (isSMS)
                expectedvalidationMessageList.AddRange(new string[] { "Must select at least one recipient.", "Must select at least one recipient.", "SMS Type must be selected." });
            if (isEmail)
                expectedvalidationMessageList.AddRange(new string[] { "Must select at least one recipient.", "Email Subject must be entered.", "Email Body must be entered." });

            foreach (string message in validationMessages)
                Assert.That(expectedvalidationMessageList.Contains(message), String.Format("The following validation message was displayed:{0}", message));
        }

        /// <summary>
        /// This will assert that a client email exist in the clientemail table.
        /// </summary>
        public static void AssertClientCommunication(int debtCounsellingKey, bool isEmail = false, bool isSMS = false)
        {
            ContentTypeEnum contentType = new ContentTypeEnum();
            if (isSMS)
                contentType = ContentTypeEnum.StandardText;
            if (isEmail)
                contentType = ContentTypeEnum.HTML;
            int accountkey = debtCounsellingService.GetAccountKeyByDebtCounsellingKey(debtCounsellingKey, DebtCounsellingStatusEnum.Open);
            QueryResults results = clientEmailService.GetClientCommunication(contentType, accountkey);
            Assert.That(results.HasResults, String.Format("Email entry was not created in the clientemail table for account:{0}.", accountkey));
        }

        /// <summary>
        /// Assert DiaryDate saved against the debtcounselling case
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="expecteddiarydate"></param>
        public static void AssertDiaryDate(int debtCounsellingKey, DateTime? expecteddiarydate)
        {
            if (expecteddiarydate.HasValue)
            {
                DateTime savedDiaryDate = debtCounsellingService.GetDebtCounsellingDiaryDate(debtCounsellingKey);
                Assert.AreEqual(savedDiaryDate.Date, expecteddiarydate.Value.Date,
                    String.Format("Expected DiaryDate:{0}, but DiaryDate was:{1}", expecteddiarydate.Value, savedDiaryDate));
            }
        }

        /// <summary>
        /// Asserts that an active Accepted Proposal exists for the specified debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalAccepted">Proposal Accepted Status</param>
        /// <param name="proposalType">Type of Proposal that should exist</param>
        /// <param name="status">Status of Proposal</param>
        public static void AssertProposalExists(int debtCounsellingKey, ProposalTypeEnum proposalType, ProposalStatusEnum status, ProposalAcceptedEnum proposalAccepted)
        {
            Logger.LogAction(string.Format("Asserting that an Proposal exists for debt counselling case: {0}", debtCounsellingKey));
            QueryResults r = proposalService.GetProposalDetails(debtCounsellingKey, status, proposalAccepted, proposalType);
            Assert.True(r.HasResults, string.Format("No Proposal was found for debt counselling case: {0}. ProposalType={1}, ProposalStatus={2}, ProposalAccepted={3}",
                debtCounsellingKey, proposalType, status, proposalAccepted));
        }

        /// <summary>
        /// Asserts that an active Accepted Proposal exists for the specified debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalAccepted">Proposal Accepted Status</param>
        /// <param name="proposalType">Type of Proposal that should exist</param>
        /// <param name="status">Status of Proposal</param>
        public static void AssertProposalDoesNotExist(int debtCounsellingKey, ProposalTypeEnum proposalType, ProposalStatusEnum status, ProposalAcceptedEnum proposalAccepted)
        {
            Logger.LogAction(string.Format("Asserting that an Proposal does not exist for debt counselling case: {0}", debtCounsellingKey));
            QueryResults r = proposalService.GetProposalDetails(debtCounsellingKey, status, proposalAccepted, proposalType);
            Assert.IsFalse(r.HasResults, string.Format(@"A Proposal was found for debt counselling case: {0}. ProposalType={1}, ProposalStatus={2}, ProposalAccepted={3}
                            when none was expected", debtCounsellingKey, proposalType, status, proposalAccepted));
        }

        /// <summary>
        /// Asserts that the expected proposalitem exist for the debtcounselling case.
        /// </summary>
        /// <param name="debtcounsellingKey"></param>
        /// <param name="expectedProposalItem"></param>
        public static void AssertProposalItemExists(int debtcounsellingKey, ProposalTypeEnum proposalType, ProposalStatusEnum propStatus, Automation.DataModels.ProposalItem expectedProposalItem)
        {
            bool assertPassed = true;
            var savedProposals = proposalService.GetProposals(debtcounsellingKey);
            savedProposals = from proposal in savedProposals
                             where proposal.ProposalTypeKey == proposalType && proposal.ProposalStatusKey == propStatus
                             select proposal;

            var propItemList = new List<Automation.DataModels.ProposalItem>();
            foreach (var proposal in savedProposals)
            {
                var savedpropItems = proposalService.GetProposalItems(proposal.ProposalKey).ToList();
                propItemList.AddRange(savedpropItems);
            }
            //Get the proposal item mathcing the start and enddate of the expected proposal item.
            var savedPropItem = (from proposalItem in propItemList
                                 where expectedProposalItem.StartDate == proposalItem.StartDate
                                          && expectedProposalItem.EndDate == proposalItem.EndDate
                                 select proposalItem).FirstOrDefault();

            if (savedPropItem != null)
            {
                Assert.That(expectedProposalItem.InterestRate.Equals((savedPropItem.InterestRate * 100)));

                Assert.That(expectedProposalItem.InstalmentPercent.Equals((savedPropItem.InstalmentPercent * 100)));

                Assert.That(expectedProposalItem.AnnualEscalation.Equals((savedPropItem.AnnualEscalation * 100)));

                if (expectedProposalItem.CreateDate.Year != 1)
                    Assert.That(savedPropItem.CreateDate.Date.Equals(expectedProposalItem.CreateDate.Date));

                Assert.That(savedPropItem.MonthlyServiceFee == expectedProposalItem.MonthlyServiceFee);
            }
            Logger.LogAction(@"Asserting Proposal Item matching Start Date: {0}, End Date: {1}, Interest Rate: {2}, Instalment %: {3}, Annual Escalation %: {4},
                           Create Date: {5}, exist in the Proposal Details grid", expectedProposalItem.StartDate.ToString(Formats.DateFormat),
                    expectedProposalItem.EndDate.ToString(Formats.DateFormat), expectedProposalItem.InterestRate.ToString(Formats.Percentage),
                    expectedProposalItem.InstalmentPercent.ToString(Formats.Percentage), expectedProposalItem.AnnualEscalation.ToString(Formats.Percentage2),
                    expectedProposalItem.CreateDate.ToString(Formats.DateFormat));
            Assert.IsTrue(assertPassed, String.Format(@"No Proposal Item could be found matching Start Date: {0}, End Date: {1}, Interest Rate: {2}, Instalment %: {3},
                                Annual Escalation %: {4}, Create Date: {5}", expectedProposalItem.StartDate.ToString(Formats.DateFormat),
                    expectedProposalItem.EndDate.ToString(Formats.DateFormat), expectedProposalItem.InterestRate.ToString(Formats.Percentage),
                    expectedProposalItem.InstalmentPercent.ToString(Formats.Percentage), expectedProposalItem.AnnualEscalation.ToString(Formats.Percentage2),
                    expectedProposalItem.CreateDate.ToString(Formats.DateFormat)));
        }

        /// <summary>
        /// This assertion takes in account and a related account. We then loop through the roles on the related account. If we find a common role
        /// between the account and the related account, then we expect a warning message to be displayed for that person as the account is under debt counselling.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="relatedAccountKey"></param>
        public static void AssertRelatedAccountDebtCounsellingRule(TestBrowser browser, int accountKey, int relatedAccountKey, int? applicationKey)
        {
            //account under debt counselling roles
            var accountRoles = legalEntityService.GetLegalEntityRoles(accountKey);
            //case we are trying to perform an activity on
            var relatedAccountRoles = legalEntityService.GetLegalEntityRoles(relatedAccountKey);
            relatedAccountRoles = (from a in relatedAccountRoles where a.LegalEntityTypeKey == (int)LegalEntityTypeEnum.NaturalPerson select a);
            string reference = applicationKey == null ? string.Format(@"account ({0})", accountKey) : string.Format(@"application ({0})", applicationKey);
            foreach (var item in relatedAccountRoles)
            {
                var commonRole = (from ar in accountRoles where ar.LegalEntityKey == item.LegalEntityKey select ar).FirstOrDefault();
                if (commonRole != null)
                {
                    string warning = string.Format(@"{0} ({1}) on {2} is under debt counselling.", item.LegalEntityLegalName, item.RoleDescription, reference);
                    browser.Page<BasePageAssertions>().AssertValidationMessageExists(warning);
                }
            }
        }

        /// <summary>
        /// This assertion takes in account and a related account. We then loop through the roles on the related account. If we find a common role
        /// between the account and the related account, then we expect a warning message to be displayed for that person as the account is under debt counselling.
        /// </summary>
        /// <param name="accountKey"></param>
        public static void AssertLegalEntitiesOnAccountUnderDebtCounsellingRule(TestBrowser browser, int accountKey)
        {
            //account under debt counselling roles
            var debtCounsellingKey = debtCounsellingService.GetDebtCounsellingKey(accountKey);
            var accountRoles = legalEntityService.GetLegalEntityRoles(accountKey);
            var externalRoles = externalRoleService.GetActiveExternalRoleList(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.Client, true);
            //case we are trying to perform an activity on
            foreach (var item in externalRoles)
            {
                var roleDescription = (from a in accountRoles
                                       where a.LegalEntityKey == item.LegalEntityKey && a.GeneralStatusKey == GeneralStatusEnum.Active
                                       select a.RoleDescription).FirstOrDefault();

                string warning = string.Format(@"{0} ({1}) on account ({2}) is under debt counselling.", item.LegalEntityLegalName,
                roleDescription, accountKey);
                browser.Page<BasePageAssertions>().AssertValidationMessageExists(warning);
            }
        }

        /// <summary>
        /// Runs the assertion in order to check whether the client roles have been correctly setup for our debt counselling test cases
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        public static void AssertClientRolesExist(string testIdentifier)
        {
            //need to check that the client role is correctly setup, build an array of dckeys and lekeys
            int legalEntityKey = 0;
            int debtCounsellingKey = 0;
            int[,] arr = debtCounsellingService.CreateDCKeyLEKeyArray(testIdentifier, 1);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    switch (j)
                    {
                        case 0:
                            debtCounsellingKey = arr[i, j];
                            break;

                        case 1:
                            legalEntityKey = arr[i, j];
                            break;
                    }
                }
                ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.Client, legalEntityKey);
            }
        }

        /// <summary>
        /// Runs the assertion in order to check whether the client roles that we did not want created have not erroneously been created.
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        public static void AssertClientRolesDoNotExist(string testIdentifier)
        {
            //need to check that the client role is correctly setup, build an array of dckeys and lekeys
            int legalEntityKey = 0;
            int debtCounsellingKey = 0;
            int[,] arr = debtCounsellingService.CreateDCKeyLEKeyArray(testIdentifier, 0);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    switch (j)
                    {
                        case 0:
                            debtCounsellingKey = arr[i, j];
                            break;

                        case 1:
                            legalEntityKey = arr[i, j];
                            break;
                    }
                }
                ExternalRoleAssertions.AssertActiveExternalRoleDoesNotExistForLegalEntity(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.Client, legalEntityKey);
            }
        }

        /// <summary>
        /// Runs the assertions for the accepting of a proposal
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="accountKey"></param>
        /// <param name="tranDate"></param>
        /// <param name="proposalKey"></param>
        public static void AssertProposalAcceptance(int debtCounsellingKey, int accountKey, int proposalKey, int expectedRemainingTerm)
        {
            // Assertions.MortgageLoan.AssertMortgageLoanInformation(accountKey, (int)FinancialServiceType.VariableLoan,"RemainingInstalments", expectedRemainingTerm.ToString());
            //case has moved
            DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.AcceptedProposal);
            StageTransitionAssertions.AssertStageTransitionCreated(debtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_AccountAdjusted);
            //check that the start debt counselling memo transaction has been posted
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.StartDebtCounselling);
        }

        /// <summary>
        /// Asserts that no active court details exist for the debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        public static void AssertActiveCourtDetailsDoNotExist(int debtCounsellingKey)
        {
            bool courtDetails = courtDetailsService.ActiveCourtDetailsExist(debtCounsellingKey);

            Logger.LogAction(string.Format("Asserting that no active court details exist for debt counselling case {0}.", debtCounsellingKey));
            Assert.IsFalse(courtDetails, "Active court details exist for debt counselling case {0}.", debtCounsellingKey);
        }

        /// <summary>
        /// Asserts the value of the court case indicator.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="courtCase"></param>
        public static void AssertCourtCaseIndicator(int debtCounsellingKey, bool courtCase)
        {
            var results = x2Service.GetDebtCounsellingInstanceDetails(debtCounsellingKey: debtCounsellingKey);
            Assert.AreEqual(Convert.ToBoolean(results.Rows(0).Column("CourtCase").Value), courtCase, "Court case indicator is not {0}.", courtCase);
        }
    }
}
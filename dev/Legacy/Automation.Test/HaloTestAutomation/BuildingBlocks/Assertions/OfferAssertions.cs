using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class OfferAssertions
    {
        private static readonly IApplicationService applicationService;
        private static readonly IADUserService adUserService;
        private static readonly ILegalEntityAddressService legalEntityAddressService;

        static OfferAssertions()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            adUserService = ServiceLocator.Instance.GetService<IADUserService>();
            legalEntityAddressService = ServiceLocator.Instance.GetService<ILegalEntityAddressService>();
        }

        /// <summary>
        /// Assert the mailing address captured against an offer.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="expectedformattedlegalentityAddress"></param>
        /// <param name="correspondenceMedium"></param>
        /// <param name="onlineStatementFormat"></param>
        /// <param name="language"></param>
        /// <param name="isonlineStatement"></param>
        public static void AssertOfferMailingAddress(int offerKey, string expectedformattedlegalentityAddress, string correspondenceMedium, string onlineStatementFormat,
                string language, bool isonlineStatement)
        {
            //Get the mailing address saved against offer and if no results return then it was not saved.
            var offerMailingAddress = applicationService.GetOfferMailingAddress(offerKey);
            Assert.That(offerMailingAddress != null, "No offer mailing address exists.");

            Assert.AreEqual(expectedformattedlegalentityAddress, offerMailingAddress.FormattedAddressDelimited);
            Assert.AreEqual(isonlineStatement, offerMailingAddress.OnlineStatement);
            Assert.AreEqual(language, offerMailingAddress.Language);
            Assert.AreEqual(correspondenceMedium, offerMailingAddress.CorrespondenceMedium);
            Assert.AreEqual(onlineStatementFormat, offerMailingAddress.OnlineStatementFormat);
        }

        /// <summary>
        /// This assertion will check that an offer role exists against a particular aduser for an offer
        /// We first fetch the ADUser's ADUserKey by passing their ADUserName minus the domain (e.g.BAUser), which is then
        /// compared against the ADUserKey on the OfferRole
        /// </summary>
        /// <param name="offerkey">The OfferKey to be checked</param>
        /// <param name="adUserName">The ADUser who is supposed to be assigned the role</param>
        /// <param name="offerRoleType">The Offer Role Type Key to be checked</param>
        /// <param name="orgStructureKey">The OrgStructureKey of the ADUser</param>
        public static void AssertOfferRoleCreatedAndAssignedToADUser(int offerkey, string adUserName, OfferRoleTypeEnum offerRoleType, int orgStructureKey)
        {
            QueryResults results1 = adUserService.GetADUserKeyByADUserName(adUserName, orgStructureKey);
            string adUserKeyExpected = results1.Rows(0).Column("aduserkey").Value;
            int adUserExp = int.Parse(adUserKeyExpected);
            var results = applicationService.GetActiveOfferRolesByOfferRoleType(offerkey, offerRoleType);
            string adUserKeyActual = results.Rows(0).Column("ADUserKey").Value;
            int adUserAct = int.Parse(adUserKeyActual);
            Assert.AreEqual(adUserExp, adUserAct, String.Format("Expected AdUser: {0}", adUserName));
            results.Dispose();
            results1.Dispose();
        }
        public static void AssertOfferRoleCreatedAndAssignedToADUser(int offerkey, string[] adUserNames, OfferRoleTypeEnum offerRoleType, int orgStructureKey)
        {
            QueryResults results1 = null;
            var pass = false;
            foreach (var adUser in adUserNames)
            {
                results1 = adUserService.GetADUserKeyByADUserName(adUser, orgStructureKey);
                //the test user is in specific org structure
                if (results1.HasResults)
                {
                    string adUserKeyExpected = results1.Rows(0).Column("aduserkey").Value;
                    int adUserExp = int.Parse(adUserKeyExpected);
                    var results = applicationService.GetActiveOfferRolesByOfferRoleType(offerkey, offerRoleType);
                    string adUserKeyActual = results.Rows(0).Column("ADUserKey").Value;
                    int adUserAct = int.Parse(adUserKeyActual);

                    if (adUserExp == adUserAct)
                        pass = true;
                    results.Dispose();
                }
                results1.Dispose();
            }
            Assert.True(pass);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="leKey"></param>
        /// <param name="offerKey"></param>
        /// <param name="gs"></param>
        /// <param name="exists"></param>
        public static void AssertOfferRoleExists(int leKey, int offerKey, GeneralStatusEnum gs, bool exists)
        {
            bool res = applicationService.OfferRoleExists(leKey, offerKey, gs);
            Assert.AreEqual(exists, res,"OfferRole for legalentitykey {0} does not exist for offerkey {1}",leKey,offerKey);
        }

        /// <summary>
        /// This assertion accepts a list of conditions that are expected to exist against a particular offer. This list is then
        /// check against the conditions that have been saved against the offer.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="expectedConditionNumbers">String Array of expected Conditions (using ConditionName)</param>
        public static void AssertOfferConditionsExist(int offerKey, string[] expectedConditionNumbers)
        {
            QueryResults results = applicationService.GetOfferConditions(offerKey);
            bool found = false;
            foreach (string s in expectedConditionNumbers)
            {
                for (int i = 0; i < results.RowList.Count; i++)
                {
                    if (results.Rows(i).Column("ConditionName").Value == s)
                    {
                        found = true;
                    }
                }
                Logger.LogAction("Asserting condition: " + s + " exists ");
                Assert.True(found, "Expected Condition: " + s + " was not found");
            }
        }

        /// <summary>
        /// Allows a test to assert a column value in the OfferInformationVariableLoan table. This assertion should
        /// be used for the value columns on the table and not the string as it does an integer comparison on the
        /// two values
        /// </summary>
        /// <param name="expectedAmount">Expected Column Value</param>
        /// <param name="offerKey">Offer.OfferKey</param>
        /// <param name="columnName">Column that is being checked</param>
        public static void AssertApplicationInformation(int expectedAmount, int offerKey, string columnName)
        {
            var results = applicationService.GetLatestOfferInformationByOfferKey(offerKey);
            int actualAmount = results.Rows(0).Column(columnName).GetValueAs<int>();
            results.Dispose();
            //log
            Logger.LogAction("Asserting the further lending app amount has been updated for column: " + columnName);
            Assert.AreEqual(expectedAmount, actualAmount, columnName + " is not the expected value.");
        }

        /// <summary>
        /// Allows a test to assert a column value in the OfferInformationVariableLoan table. This assertion should
        /// be used for the value columns on the table and not the string as it does an integer comparison on the
        /// two values
        /// </summary>
        /// <param name="expectedBool">Expected Column Value</param>
        /// <param name="offerKey">Offer.OfferKey</param>
        /// <param name="columnName">Column that is being checked</param>
        public static void AssertApplicationInformation(bool expectedBool, int offerKey, string columnName)
        {
            var results = applicationService.GetLatestOfferInformationByOfferKey(offerKey);
            var actualBool = results.Rows(0).Column(columnName).GetValueAs<bool>();
            results.Dispose();
            //log
            Logger.LogAction("Asserting the further lending app amount has been updated for column: " + columnName);
            Assert.AreEqual(expectedBool, actualBool, columnName + " is not the expected value.");
        }

        /// <summary>
        /// This assertion will check that the OfferStatus is equal to what a test expects.
        /// </summary>
        /// <param name="offerkey">The OfferKey</param>
        /// <param name="status">OfferStatus</param>
        public static void AssertOfferStatus(int offerkey, OfferStatusEnum status)
        {
            QueryResults results = applicationService.GetOfferData(offerkey);
            string offerStatus = results.Rows(0).Column("OfferStatusKey").Value;
            int offerStatusAct = int.Parse(offerStatus);
            Logger.LogAction(string.Format(@"Asserting that Offer Status is set to {0} for application {1}", (int)status, offerkey));
            Assert.AreEqual((int)status, offerStatusAct);
            results.Dispose();
        }

        /// <summary>
        /// This assertion will check the difference between an OfferEndDate and an Expected Date. If you pass a 0
        /// as the ExpectedDifferenceInDays it will check that the OfferEndDate is equal to the Expected Date. You
        /// can also perform a NULL check for the OfferEndDate by setting NullCheck to TRUE
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedValue">The Expected EndDate</param>
        /// <param name="expectedDifferenceInDays">The diff in days between the Actual OfferEndDate and the ExpectedValue</param>
        /// <param name="nullCheck">True=check that EndDate is NULL, False=run the date check</param>
        public static void AssertOfferEndDate(int offerkey, DateTime expectedValue, int expectedDifferenceInDays, bool nullCheck)
        {
            QueryResults results = applicationService.GetOfferData(offerkey);
            string offerEndDate = results.Rows(0).Column("OfferEndDate").Value;

            if (!nullCheck)
            {
                DateTime offerEndDateAct = DateTime.Parse(offerEndDate);
                //get the number of days between the two dates
                TimeSpan ts = offerEndDateAct - expectedValue;
                int daysDiff = ts.Days;
                //compare the expected difference versus actual
                Logger.LogAction("Asserting the OfferEndDate is correctly set for " + offerkey);
                Assert.AreEqual(daysDiff, expectedDifferenceInDays);
                results.Dispose();
            }
            else
            {
                Logger.LogAction("Asserting that OfferEndDate is NULL for application " + offerkey);
                Assert.IsNullOrEmpty(offerEndDate);
                results.Dispose();
            }
        }

        /// <summary>
        /// Checks that the count of OfferInformation records in the database match the expected count for an offer
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="expCount">Expected number of revisions</param>
        public static void AssertOfferInformationCount(int offerKey, int expCount)
        {
            QueryResults results = applicationService.GetOfferInformationRecordsByOfferKey(offerKey);
            int count = results.RowList.Count;
            Logger.LogAction(String.Format(@"Asserting that there are {0} OfferInformation records", count));
            Assert.AreEqual(expCount, count, "The expected number of OfferInformation records do not exist");
            results.Dispose();
        }

        /// <summary>
        /// Checks that the product linked to the latest Offer Information matches the expected product. When the expected product
        /// is VariFix or Edge we need to check that the appropriate OfferInformation tables have been populated with the product
        /// specific data.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="product">Expected Product i.e. "Edge", "VariFix Loan", "New Variable Loan"</param>
        public static void AssertLatestOfferInformationProduct(int offerKey, string product)
        {
            QueryResults results = applicationService.GetLatestOfferInformationByOfferKey(offerKey);
            string actProduct = results.Rows(0).Column("ProductDescription").Value;
            Logger.LogAction(String.Format(@"Asserting the product for {0} has changed to {1}", offerKey, product));
            Assert.AreEqual(product, actProduct, "Offer Information product has not been changed correctly");
            //need extra assertions for VariFix and Edge to ensure OfferInformation records created.
            switch (product)
            {
                case Products.VariFixLoan:
                    Logger.LogAction(String.Format(@"Asserting the VariFix OI records exist"));
                    Assert.Greater(results.Rows(0).Column("VariFixOI").GetValueAs<int>(), 0, "No OfferInformationVarifixLoan record");
                    break;

                case Products.Edge:
                    Logger.LogAction(String.Format(@"Asserting the Edge OI records exist"));
                    Assert.Greater(results.Rows(0).Column("EdgeOI").GetValueAs<int>(), 0, "No OfferInformationRateOverride record exists");
                    break;
            }
            results.Dispose();
        }

        /// <summary>
        /// Checks that the OfferInformation records in the database was updated
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="expectedOfferInformationType">OfferInformationType expected</param>
        /// <param name="expectedProduct">Product</param>
        public static void OfferInformationUpdated(int offerKey, OfferInformationTypeEnum expectedOfferInformationType, ProductEnum expectedProduct)
        {
            bool offerRevised = false;
            QueryResults results = applicationService.GetOfferInformationRecordsByOfferKey(offerKey);
            Logger.LogAction(String.Format(@"Asserting that Offer {0} has an OfferInformation record of type {1}", offerKey, expectedOfferInformationType.ToString()));
            var r = (from res in results orderby res.Column("OfferInformationKey").GetValueAs<int>() descending select res).FirstOrDefault();
            //QueryResultsRow r = results.FindRowByExpression<DateTime>("OfferInsertDate", true, DateTime.Now)[0];
            if (r == null)
            {
                Assert.Fail("No rows were found.");
            }
            else
            {
                int offerInfoTypeKey = (int)expectedOfferInformationType;
                int productKey = (int)expectedProduct;
                if (r.Column("OfferInformationTypeKey").GetValueAs<int>() == offerInfoTypeKey && r.Column("ProductKey").GetValueAs<int>() == productKey)
                    offerRevised = true;
                Assert.True(offerRevised, "Offer {0} does not have an updated OfferInformation of type {1}", offerKey, expectedOfferInformationType.ToString(), DateTime.Now);
            }
            results.Dispose();
        }

        /// <summary>
        /// Checks to see if an offer debit order exists
        /// </summary>
        /// <param name="_offerKey"></param>
        /// <param name="financialServicePaymentType"></param>
        /// <param name="debitOrderDay"></param>
        public static void OfferDebitOrderExists(int _offerKey, FinancialServicePaymentTypeEnum financialServicePaymentType, int debitOrderDay)
        {
            var offerDebitOrders = applicationService.GetOfferDebitOrder(_offerKey);
            var exists = (from o in offerDebitOrders
                          where o.DebitOrderDay == debitOrderDay
                              && o.FinancialServicePaymentTypeKey == financialServicePaymentType
                          select o).FirstOrDefault();
            Assert.That(exists != null);
        }

        /// <summary>
        /// Asserts that an ITC records exists against the account linked to the application
        /// </summary>
        /// <param name="offerKey"></param>
        public static void AssertITCRecordExists(int offerKey)
        {
            var ITCs = applicationService.GetITCRecordsByOfferKey(offerKey);
            var itc = (from i in ITCs select i).FirstOrDefault();
            Logger.LogAction(string.Format(@"Asserting that an ITC record exists against the account linked to {0}", offerKey));
            Assert.That(itc != null, string.Format(@"No ITC records exists against the account linked to application {0}", offerKey));
        }

        public static void AssertOfferCreated(int generickey, OfferTypeEnum offerType)
        {
            var offerRow = (from row in applicationService.GetOfferData(generickey)
                            where row.Column("OfferTypeKey").GetValueAs<int>() == (int)offerType
                                        && row.Column("OfferKey").GetValueAs<string>() == generickey.ToString()
                            select row).FirstOrDefault();

            Assert.That(offerRow != null, "OfferKey:{0} with OfferType:{1} was not created.", generickey, offerType);
        }

        /// <summary>
        /// Checks that the OfferInformationPersonalLoan records in the database was updated
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="expectedOfferInformationType">OfferInformationType expected</param>
        /// <param name="expectedProduct">Product</param>
        public static void OfferInformationPersonalLoanUpdated(int offerKey, int expectedLoanAmount, int expectedTerm, int expectedMonthlyInstalment, int expectedLifePremium)
        {
            bool offerInserted = false;
            QueryResults results = applicationService.GetOfferInformationPersonalLoanRecordsByOfferKey(offerKey);
            Logger.LogAction(String.Format(@"Asserting that Offer {0} has an OfferInformationPersonalloan record of type with correct information", offerKey));
            var r = (from res in results orderby res.Column("OfferInformationKey").GetValueAs<int>() descending select res).FirstOrDefault();
            //QueryResultsRow r = results.FindRowByExpression<DateTime>("OfferInsertDate", true, DateTime.Now)[0];
            if (r == null)
            {
                Assert.Fail("No rows were found.");
            }
            else
            {
                offerInserted = true;
                Assert.True(offerInserted, "Offer {0} does have an updated OfferInformationPersonalLoan ", offerKey, DateTime.Now);
            }
            results.Dispose();
        }

        /// <summary>
        /// Checks that the OfferInformation records in the database was updated
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="expectedOfferInformationType">OfferInformationType expected</param>
        /// <param name="expectedProduct">Product</param>
        public static void OfferExpenseUpdated(int offerKey, string expectedexpenseType, int expectedAmount)
        {
            bool offerExpenseInserted = false;
            QueryResults results = applicationService.GetOfferExpenseByOfferKey(offerKey, expectedexpenseType);
            Logger.LogAction(String.Format(@"Asserting that Offer {0} has an OfferExpense record of type {1}", offerKey, expectedexpenseType.ToString()));
            var r = (from res in results orderby res.Column("ExpenseTypeKey").GetValueAs<int>() descending select res).FirstOrDefault();
            //QueryResultsRow r = results.FindRowByExpression<DateTime>("OfferInsertDate", true, DateTime.Now)[0];
            if (r == null)
            {
                Assert.Fail("No rows were found.");
            }
            else
            {
                string expenseTypeKey = (string)expectedexpenseType;
                int amount = (int)expectedAmount;
                if (r.Column("Description").GetValueAs<string>() == expectedexpenseType && r.Column("TotalOutstandingAmount").GetValueAs<int>() == expectedAmount)
                    offerExpenseInserted = true;
                Assert.True(offerExpenseInserted, "Offer {0} does not have an updated OfferExpense of type {1} with amount of {2}", offerKey, expectedexpenseType.ToString(), expectedAmount, DateTime.Now);
            }
            results.Dispose();
        }

        /// <summary>
        /// Checks if an offer attribute has or has not been loaded against an offer.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="offerAttributeType">Attribute Type</param>
        /// <param name="exists">TRUE = expect attribute to exist, FALSE = expect to not exist.</param>
        public static void AssertOfferAttributeExists(int offerKey, Common.Enums.OfferAttributeTypeEnum offerAttributeType, bool exists = true)
        {
            var hasAttribute = (from att in applicationService.GetOfferAttributes(offerKey)
                                where att.OfferAttributeTypeKey == offerAttributeType
                                select att != null).FirstOrDefault();
            Assert.AreEqual(exists, hasAttribute);
        }

        public static void AssertOfferExpense(int offerKey, double expectedValue, bool overrideCheck, ExpenseTypeEnum expenseType)
        {
            var cancellationFeeExpense = (from exp in applicationService.GetOfferExpenses(offerKey)
                                          where exp.ExpenseTypeKey == expenseType
                                          select exp).FirstOrDefault();

            Assert.AreEqual(expectedValue, cancellationFeeExpense.TotalOutstandingAmount, "Fee value did not match the expected value");
            Assert.AreEqual(overrideCheck, cancellationFeeExpense.OverRidden, "Expected override");
        }

        public static void AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkExist(int offerKey, int legalEntityKey)
        {
            var domiciliumAddresses = applicationService.GetOfferRoleDomiciliums(offerKey);
            Assert.AreNotEqual(domiciliumAddresses.Count(), 0, "There are no offer domiciliums for legalentitykey: {0} on offerkey: {1}", legalEntityKey, offerKey);
            Assert.AreEqual(domiciliumAddresses.Count(), 1, "There are more than one offer domicilium for legalentitykey: {0} on offerkey: {1}", legalEntityKey, offerKey);

            var pendingLegalEntityDomiciliumAddress = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityKey, GeneralStatusEnum.Pending).FirstOrDefault();
            var activeLegalEntityDomiciliumAddress = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityKey, GeneralStatusEnum.Active).FirstOrDefault();
            var legalEntityDomiciliumKey = (from dom in domiciliumAddresses
                                            select dom.LegalEntityDomiciliumKey).FirstOrDefault();

            if (activeLegalEntityDomiciliumAddress != null)
                Assert.AreNotEqual(legalEntityDomiciliumKey, activeLegalEntityDomiciliumAddress.LegalEntityDomiciliumKey, "Expected a new LegalEntityDomicilium link to be created.");

            var legalEntityDomiciliumAddress = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityKey, GeneralStatusEnum.Pending).FirstOrDefault();
            Assert.AreEqual(legalEntityDomiciliumKey, pendingLegalEntityDomiciliumAddress.LegalEntityDomiciliumKey, "OfferRoleDomecilium link was not propery created for LegalEntityDomecilium");
        }

        public static void AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkNotExist(int offerKey, int legalEntityKey)
        {
            var domiciliumAddresses = applicationService.GetOfferRoleDomiciliums(offerKey);
            Assert.AreEqual(domiciliumAddresses.Count(), 0, "There are no offer domiciliums for legalentitykey: {0} on offerkey: {1}", legalEntityKey, offerKey);
        }

        public static void AssertDomiciliumAddressActive(int legalEntityKey, int offerKey)
        {
            var legalEntityDomiciliumAddress = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityKey, GeneralStatusEnum.Active).FirstOrDefault();
            Assert.AreEqual((int)GeneralStatusEnum.Active, (int)legalEntityDomiciliumAddress.GeneralStatusKey, "There is no active domicilium address for legalentitykey: {0} on offerkey: {1}", legalEntityKey, offerKey);
        }

        public static void AssertDomiciliumAddressSetToSpecifiedStatus(int legalEntityKey, int offerKey, int legalEntityDomiciliumKey, GeneralStatusEnum specifiedStatus)
        {
            var legalEntityDomiciliumAddress = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityKey, specifiedStatus).FirstOrDefault();
            Assert.AreEqual(legalEntityDomiciliumKey, legalEntityDomiciliumAddress.LegalEntityDomiciliumKey, "Pending domicilium address has not been set active for legalentitykey: {0} on offerkey: {1}", legalEntityKey, offerKey);
        }
        public static void AssertOfferRoleAttributeExists(int offerKey, int legalEntityKey, Common.Enums.OfferRoleAttributeTypeEnum offerAttributeType, bool exists = true)
        {
            var hasAttribute = (from att in applicationService.GetOfferRoleAttributes(offerKey)
                                where att.LegalEntityKey == legalEntityKey && att.OfferRoleAttribute.OfferRoleAttributeTypeKey == (int)offerAttributeType
                                select att).FirstOrDefault() != null;
            Assert.AreEqual(exists, hasAttribute);
        }

    }
}
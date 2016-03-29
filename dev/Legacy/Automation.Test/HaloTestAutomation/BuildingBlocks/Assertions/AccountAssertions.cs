using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public class AccountAssertions
    {
        private static IAccountService accountService;
        private static IDetailTypeService detailTypeService;

        static AccountAssertions()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
            detailTypeService = ServiceLocator.Instance.GetService<IDetailTypeService>();
        }

        /// <summary>
        /// Gets a column from the account table and checks that the string value of the colummn is equal to the expected value provided.
        /// </summary>
        /// <param name="accountKey">AccounKey</param>
        /// <param name="column">Column to Fetch</param>
        /// <param name="expectedValue">Expected column value</param>
        public static void AssertAccountInformation(int accountKey, string column, string expectedValue)
        {
            string actualValue = accountService.GetAccountColumn(accountKey, column);
            Logger.LogAction(String.Format("Asserting the expected value for the {0} column is {1}", column, expectedValue));
            StringAssert.AreEqualIgnoringCase(expectedValue, actualValue,
                String.Format("The expected value of {0} for the Account column {1} was not found. The actual value is: {2}",
                expectedValue, column, actualValue));
        }

        /// <summary>
        /// Ensures that an expected detail type exists against an account
        /// </summary>
        /// <param name="accountKey">account number</param>
        /// <param name="detailType">detail type</param>
        /// <param name="detailClass">detail class</param>
        /// <param name="exists">TRUE = exists, FALSE = does not exist</param>
        public static void AssertDetailType(int accountKey, DetailTypeEnum detailType, DetailClassEnum detailClass, bool exists)
        {
            //TODO: Update this assertion to take in a LoanDetail model and assert the contents of the model and not just that one of the type exists.
            var loanDetail = detailTypeService.GetLoanDetailRecord(detailType, accountKey, detailClass);
            if (exists)
            {
                Assert.That(loanDetail != null, "Detail Type {0} was not found against account {1}", detailType.ToString(), accountKey);
            }
            else
            {
                Assert.That(loanDetail == null, "Detail Type {0} was found against account {1}, when we expected it to not be",
                    detailType.ToString(), accountKey);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="legalEntityKey"></param>
        public static void AssertAccountRoleExists(int accountKey, int legalEntityKey)
        {
            var roles = accountService.AccountRoleLegalEntityKeys(accountKey);
            var leExist = (from r in roles where r.Key == legalEntityKey select r.Key).FirstOrDefault();
            Assert.That(leExist > 0);
        }

        public static void AssertAccountStatus(int accountkey, AccountStatusEnum expectedStatus)
        {
            var account = accountService.GetAccountRecord(accountkey);
            Assert.True(account.AccountStatusKey == expectedStatus, "Account not updated to status: {0} ", expectedStatus);
        }
    }
}
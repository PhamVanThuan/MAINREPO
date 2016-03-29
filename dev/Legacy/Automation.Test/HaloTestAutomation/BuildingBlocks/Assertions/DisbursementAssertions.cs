using System.Linq;
using Automation.DataAccess;
using BuildingBlocks.Services;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Logging;
using System;

namespace BuildingBlocks.Assertions
{
    public static class DisbursementAssertions
    {
        private static IDisbursementService disbursementService;

        static DisbursementAssertions()
        {
            disbursementService = new DisbursementService();
        }

        /// <summary>
        /// This assertion only ensures that there are disbursement records for the given account for a Disbursement Type at a specific disbursement
        /// status.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="status">DisbursementStatus</param>
        /// <param name="type">DisbursementTransactionType</param>
        public static void AssertDisbursementExistsAtStatus(int accountKey, DisbursementStatusEnum status, DisbursementTransactionTypeEnum type)
        {
            QueryResults r = disbursementService.GetDisbursementRecords(accountKey, (int)status, (int)type);
            Logger.LogAction(String.Format(@"Asserting that disbursement records exist for Account {0}", accountKey));
            Assert.True(r.HasResults, "No Disbursement Records exists for this Account at the provided Status");
            r.Dispose();
        }

        /// <summary>
        /// This assertion will check that the sum of the value of the disbursement records for our disbursement are equal to the total amount that we
        /// are expecting to be disbursed.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="status">DisbursementStatus</param>
        /// <param name="type">DisbursementTransactionType</param>
        /// <param name="expectedAmt">Expected Total to Disburse</param>
        public static void AssertDisbursementAmount(int accountKey, DisbursementStatusEnum status, DisbursementTransactionTypeEnum type, decimal expectedAmt)
        {
            QueryResults r = disbursementService.GetDisbursementRecords(accountKey, (int)status, (int)type);
            //check that the sum of the disbursement records match
            decimal actualAmt = r.RowList.Sum(row => row.Column("Amount").GetValueAs<decimal>());
            r.Dispose();
            Logger.LogAction(String.Format(@"Asserting that the sum of the disbursement records match the expected total disbursement value against Account {0}", accountKey));
            Assert.AreEqual(Math.Round(expectedAmt, 2), Math.Round(actualAmt, 2), "The sum of the disbursements does not equal the expected value.");
        }

        /// <summary>
        /// This override will check both the readvance value and the amount that has been disbursed to the SAHL Valuation Recovery Account when we are
        /// splitting the disbursement in order to recover the valuation fee.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="status">DisbursementStatus</param>
        /// <param name="type">DisbursementTransactionType</param>
        /// <param name="expReadvAmt">Expected Readvance Value</param>
        /// <param name="expValAmt">Expected Valuation Fee Value</param>
        public static void AssertDisbursementAmount(int accountKey, DisbursementStatusEnum status, DisbursementTransactionTypeEnum type, double expReadvAmt, double expValAmt)
        {
            double actualReadvAmt = 0.00;
            double actualValAmt = 0.00;
            QueryResults r = disbursementService.GetDisbursementRecords(accountKey, (int)status, (int)type);
            foreach (QueryResultsRow row in r.RowList)
            {
                if (row.Column("AccountName").Value == "SAHL Valuation Recovery Account  ")
                {
                    actualValAmt = row.Column("Amount").GetValueAs<double>();
                }
                else
                {
                    actualReadvAmt = row.Column("Amount").GetValueAs<double>();
                }
            }
            r.Dispose();
            //check the readvance amount
            Logger.LogAction(String.Format(@"Asserting that expected readvance value of R{0} has been posted against Account {1}", expReadvAmt, accountKey));
            Assert.AreEqual(expReadvAmt, actualReadvAmt, "Readvance value posted for the disbursement does not equal the expected value.");
            //check the valuation amount
            Logger.LogAction(String.Format(@"Asserting that expected valuation recovery amount of R{0} has been posted against Account {1}", expValAmt, accountKey));
            Assert.AreEqual(expValAmt, actualValAmt, "The payment to the SAHL Valuation Recovery account does not equal the expected value.");
        }

        /// <summary>
        /// This assertion finds the loan transactions linked to a disbursement and checks that a specific transaction has been linked to the disbursement and that the
        /// loan transaction amount is the expected value
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="status">DisbursementStatus</param>
        /// <param name="type">DisbursementTransactionType</param>
        /// <param name="expReadvAmt">Expected Transaction Value linked to the Disbursement</param>
        /// <param name="loanTranType">Expected LoanTransactionType linked to the Disbursement</param>
        public static void AssertReadvanceDisbursementFinancialTransaction(int accountKey, DisbursementStatusEnum status, DisbursementTransactionTypeEnum type,
            double expReadvAmt, TransactionTypeEnum loanTranType)
        {
            double actualReadvAmt = 0.00;
            var disbursementRecords = disbursementService.GetDisbursementRecords(accountKey, (int)status, (int)type);
            foreach (QueryResultsRow row in disbursementRecords.RowList)
            {
                var loanTransactions = disbursementService.GetLoanTransactionRecordsByDisbursementKey(row.Column("DisbursementKey").GetValueAs<int>());
                actualReadvAmt = (from lt in loanTransactions
                                  where lt.Column("TransactionTypeKey").GetValueAs<int>() == (int)loanTranType
                                  select lt.Column("Amount").GetValueAs<double>()).FirstOrDefault();
            }
            Logger.LogAction(String.Format(@"Asserting that expected value of R{0} for Transaction Type {1} been posted against Account {2}",
                expReadvAmt, (int)loanTranType, accountKey));
            Assert.AreEqual(expReadvAmt, actualReadvAmt, "Loan Transaction for the transaction type does not match the expected value");
            disbursementRecords.Dispose();
        }

        public static void AssertDisbursementStatus(List<int> disbursementKeys, DisbursementStatusEnum disbursementStatus)
        {
            Logger.LogAction(@"Asserting the Disbursement record\s {0} have a DisbursementStatus of {1}", string.Concat<int>(disbursementKeys), disbursementStatus);

            var disbursements = disbursementService.GetDisbursementByDisbursementKey(disbursementKeys);

            foreach (Automation.DataModels.Disbursement d in disbursements)
                Assert.AreEqual(disbursementStatus, d.DisbursementStatusKey, string.Format(@"Disbursement {0} does not have a DisbursmentStatus of {1} as expected", d.DisbursementKey, disbursementStatus));
        }
    }
}
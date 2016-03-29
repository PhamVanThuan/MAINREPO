using NUnit.Framework;
using ObjectMaps.Pages;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.LoanServicing.CATSDisbursement
{
    public class CATSDisbursementBase : CATSDisbursementBaseControls
    {
        /// <summary>
        /// Assert that a record exists in the Disbursement Bank Account Details Grid
        /// </summary>
        /// <param name="disbursement"></param>
        public void AssertRecordExistsInDisbursementBankAccountDetailsGrid(Automation.DataModels.Disbursement disbursement)
        {
            Logger.LogAction(@"Asserting Disbursement Bank Account Details displayed in grid");
            var row = base.DisbursementBankAccountDetailRow(
                disbursement.PreparedDate,
                disbursement.ACBBankDescription,
                disbursement.ACBBranchCode,
                disbursement.ACBBranchDescription,
                disbursement.ACBTypeDescription,
                disbursement.AccountNumber,
                disbursement.AccountName,
                disbursement.Amount
                );
            Assert.False(row == null, @"No matching row was found in the Disbursement Bank Account Details grid");
        }
    }
}
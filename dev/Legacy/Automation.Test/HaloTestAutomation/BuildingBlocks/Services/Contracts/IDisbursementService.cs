using Automation.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IDisbursementService
    {
        Automation.DataModels.Disbursement GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum disbursementTransactionType, DisbursementStatusEnum disbursementStatus);

        Automation.DataModels.Disbursement GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum disbursementStatus);

        IEnumerable<Automation.DataModels.Disbursement> GetDisbursementByDisbursementKey(List<int> disbursementKey);

        Automation.DataModels.Disbursement GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum disbursementTransactionTypeKey, DisbursementStatusEnum disbursementStatusKey);

        QueryResults GetDisbursementRecords(int accountKey, int disbursementStatusKey, int disbTranTypeKey);

        QueryResults GetLoanTransactionRecordsByDisbursementKey(int disbursementKey);

        QueryResults GetDisbursementFinancialTransactionByAccountKeyAndDisbursementType(int accountKey, string disbType, string rows);

        double GetDisbursementRecords(int accountKey, DisbursementStatusEnum disbursementStatus, DisbursementTransactionTypeEnum disbursementType);

        void UpdateReadyForDisbursementToDisbursed(int p);
    }
}
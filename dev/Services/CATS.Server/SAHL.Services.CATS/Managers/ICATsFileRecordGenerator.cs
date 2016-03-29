using System;
using SAHL.Services.Interfaces.CATS.Enums;

namespace SAHL.Services.CATS.Managers
{
    public interface ICATSFileRecordGenerator
    {
        String GenerateHeader(string bankProfileNo, int fileSequence, DateTime creationDateTime, CATsEnvironment env);

        string GenerateDetailRecord(int fileSequenceNo, int subBatchSequenceNo, int transactionSequenceNo
            , int ACBBranchCode, string ACBAccount, int ACBAccountType, string ACBAccountName,
            decimal disbursementAmountInCents, string userReference);

        string GenerateSubTotalRecord(int fileSequenceNo, int subBatchSequenceNo, int ACBBranchCode
            , string ACBAccount, int ACBAccountType, string ACBAccountName, long disbursementAmountInCents, string reference);

        string GenerateTrailerRecord(int fileSequenceNo, int numberOfContraRecords, int numberOfCreditRecords
            , decimal totalContraDebitValue, decimal totalDetailDebitValue, decimal totalDebitValue, decimal NettValue
            , int homingAccountChecksum);

        string GenerateTrailerRecordAccToStdBankSpec(int fileSequenceNo, int numberOfContraRecords, int numberOfCreditRecords
            , decimal totalContraDebitValue, decimal totalDetailDebitValue, decimal totalDebitValue, decimal NettValue
            , int homingAccountChecksum);
    }
}

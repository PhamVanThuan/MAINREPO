using System;
using System.Text;
using SAHL.Core.Extensions;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.CATS.Utils;

namespace SAHL.Services.CATS.Managers
{
    public class CATSFileRecordGenerator : ICATSFileRecordGenerator
    {
        public String GenerateHeader(string bankProfileNo, int fileSequence, DateTime creationDateTime, CATsEnvironment env)
        {
            StringBuilder headerLine = new StringBuilder(200);
            headerLine.Append("FHSSVS");
            headerLine.Append(bankProfileNo);
            headerLine.Append(fileSequence.ToString().PadLeft(5, '0'));
            headerLine.Append(DateTimeExtensions.GetDateYYYYMMDD(creationDateTime));
            headerLine.Append(DateTimeExtensions.GetTimeHHMMSSFromDateTime(creationDateTime));
            headerLine.Append(DateTimeExtensions.GetDateYYYYMMDD(creationDateTime));
            if (CATsEnvironment.Live.Equals(env))
            {
                headerLine.Append('L');
            }
            else
            {
                headerLine.Append('T');
            }
            headerLine.Append("".PadRight(161));
            return headerLine.ToString();
        }

        public string GenerateDetailRecord(int fileSequenceNo, int subBatchSequenceNo, int transactionSequenceNo
            , int ACBBranchCode, string ACBAccount, int ACBAccountType, string ACBAccountName,
            decimal disbursementAmountInCents, string userReference)
        {
            StringBuilder detailRecord = new StringBuilder(200);
            detailRecord.Append(CATsFileRecordType.DetailRecord);
            detailRecord.Append(fileSequenceNo.ToString().PadLeft(5, '0'));
            detailRecord.Append(subBatchSequenceNo.ToString().PadLeft(3, '0'));
            detailRecord.Append(transactionSequenceNo.ToString().PadLeft(5, '0'));
            detailRecord.Append('C');
            detailRecord.Append(ACBBranchCode.ToString().PadLeft(6, '0'));
            detailRecord.Append(ACBAccount.ToString().PadLeft(13, '0'));
            detailRecord.Append(ACBAccountType);
            detailRecord.Append(ACBAccountName.PadRight(30));
            detailRecord.Append("".PadRight(16));
            detailRecord.Append(disbursementAmountInCents.ToString("00").PadLeft(15, '0'));
            detailRecord.Append(userReference.PadLeft(30));
            detailRecord.Append("".PadLeft(13, '0'));
            detailRecord.Append("".PadLeft(60));

            return detailRecord.ToString();
        }

        public string GenerateSubTotalRecord(int fileSequenceNo, int subBatchSequenceNo, int ACBBranchCode, string ACBAccount
            , int ACBAccountType, string ACBAccountName, long disbursementAmountInCents, string reference)
        {
            StringBuilder detailRecord = new StringBuilder(200);
            detailRecord.Append(CATsFileRecordType.Subtotal);
            detailRecord.Append(fileSequenceNo.ToString().PadLeft(5, '0'));
            detailRecord.Append(subBatchSequenceNo.ToString().PadLeft(3, '0'));
            detailRecord.Append('D');
            detailRecord.Append(ACBBranchCode.ToString().PadLeft(6, '0'));
            detailRecord.Append(ACBAccount.ToString().PadLeft(13, '0'));
            detailRecord.Append(ACBAccountType);
            detailRecord.Append(ACBAccountName.PadRight(30));
            detailRecord.Append(disbursementAmountInCents.ToString("00").PadLeft(15, '0'));
            detailRecord.Append(reference.PadRight(30));
            detailRecord.Append("".PadLeft(94));

            return detailRecord.ToString();
        }

        public string GenerateTrailerRecord(int fileSequenceNo, int numberOfContraRecords, int numberOfCreditRecords, decimal totalContraDebitValue
            , decimal totalDetailDebitValue, decimal totalDebitValue, decimal NettValue, int homingAccountChecksum)
        {
            StringBuilder trailerRecord = new StringBuilder(200);
            trailerRecord.Append(CATsFileRecordType.TrailerRecord);
            trailerRecord.Append(fileSequenceNo.ToString().PadLeft(5, '0'));
            trailerRecord.Append("".PadLeft(15, '0'));
            trailerRecord.Append(numberOfContraRecords.ToString().PadLeft(7, '0'));
            trailerRecord.Append("".PadLeft(3, '0'));
            trailerRecord.Append(numberOfContraRecords.ToString().PadLeft(3, '0'));
            trailerRecord.Append((numberOfContraRecords + numberOfCreditRecords).ToString().PadLeft(9, '0'));
            trailerRecord.Append(totalContraDebitValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(totalDetailDebitValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(numberOfCreditRecords.ToString().PadLeft(7, '0'));
            trailerRecord.Append(totalDebitValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(NettValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(homingAccountChecksum.ToString().PadLeft(15, '0'));

            return trailerRecord.ToString().PadRight(200);
        }

        public string GenerateTrailerRecordAccToStdBankSpec(int fileSequenceNo, int numberOfContraRecords, int numberOfCreditRecords, decimal totalContraDebitValue
            , decimal totalDetailDebitValue, decimal totalDebitValue, decimal NettValue, int homingAccountChecksum)
        {
            StringBuilder trailerRecord = new StringBuilder(200);
            trailerRecord.Append(CATsFileRecordType.TrailerRecord);
            trailerRecord.Append(fileSequenceNo.ToString().PadLeft(5, '0'));
            trailerRecord.Append("".PadLeft(7, '0'));
            trailerRecord.Append(numberOfContraRecords.ToString().PadLeft(7, '0'));
            trailerRecord.Append("".PadLeft(3, '0'));
            trailerRecord.Append(numberOfContraRecords.ToString().PadLeft(3, '0'));
            trailerRecord.Append((numberOfContraRecords + numberOfCreditRecords).ToString().PadLeft(9, '0'));
            trailerRecord.Append(totalContraDebitValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(totalDetailDebitValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(numberOfCreditRecords.ToString().PadLeft(15, '0'));
            trailerRecord.Append(totalDebitValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(NettValue.ToString("00").PadLeft(15, '0'));
            trailerRecord.Append(homingAccountChecksum.ToString().PadLeft(15, '0'));

            return trailerRecord.ToString().PadRight(200);
        }
    }
    
}
;
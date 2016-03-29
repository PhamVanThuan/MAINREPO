using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.Managers
{
    public class CATSFileGenerator : ICATSFileGenerator
    {


        public string GenerateSubTotalRecord(ICATSFileRecordGenerator catsFileRecordGenerator, PaymentBatch PaymentBatch, int fileSequenceNo, string reference, int subBatchSequence)
        {
            var amount = Convert.ToInt64(PaymentBatch.Amount * 100);
            var line = catsFileRecordGenerator.GenerateSubTotalRecord(fileSequenceNo, subBatchSequence, Int32.Parse(PaymentBatch.SourceAccount.ACBBranchCode)
                , PaymentBatch.SourceAccount.AccountNumber, PaymentBatch.SourceAccount.ACBTypeNumber
                , PaymentBatch.SourceAccount.AccountName, amount, reference);
            return line;
        }

        public void WritePaymentBatchDetailRecords(int fileSequence, int subBatchSequence, PaymentBatch paymentBatch, ICATSFileRecordGenerator catsFileRecordGenerator
            , IFileWriter fileWriter, string outputFileName, string reference)
        {
            int transactionSequenceNo = 1;
            foreach (var payment in paymentBatch.Payments)
            {
                var amount = Convert.ToInt64(payment.Amount * 100);
                var DetailRecord = catsFileRecordGenerator.GenerateDetailRecord(fileSequence, subBatchSequence, transactionSequenceNo
                    , Int32.Parse(payment.TargetAccount.ACBBranchCode), payment.TargetAccount.AccountNumber, payment.TargetAccount.ACBTypeNumber
                    , payment.TargetAccount.AccountName, amount
                    , payment.Reference);
                fileWriter.WriteFileLine(DetailRecord, outputFileName);
                transactionSequenceNo++;
            }
            transactionSequenceNo++;
            var subTotalRecord = GenerateSubTotalRecord(catsFileRecordGenerator, paymentBatch, fileSequence, paymentBatch.Reference, subBatchSequence);
            fileWriter.WriteFileLine(subTotalRecord, outputFileName);
        }

        public void WriteHeader(IFileWriter fileWriter, string outputFileName, ICATSFileRecordGenerator catsFileRecordGenerator
            , int fileSequenseNo, string profile, DateTime creationDateTime, CATsEnvironment cATsEnvironment)
        {
            var header = catsFileRecordGenerator.GenerateHeader(profile, fileSequenseNo, creationDateTime, cATsEnvironment);
            fileWriter.WriteFileLine(header, outputFileName);
        }

        public void WriteTrailerRecord(IFileWriter fileWriter, string outputFileName, ICATSFileRecordGenerator catsFileRecordGenerator
            , IEnumerable<PaymentBatch> paymentBatches, int fileSequence)
        {
            var numberOfCreditRecords = GetNumberOfCreditRecords(paymentBatches);
            var trailerRecord = catsFileRecordGenerator.GenerateTrailerRecord(fileSequence
                , paymentBatches.Count()
                , numberOfCreditRecords
                , CalculateTotalContraDebitValue(paymentBatches) * 100
                , CalculateTotalDebitValue(paymentBatches) * 100
                , CalculateTotalDebitValue(paymentBatches) * 100
                , CalculateNettValue(CalculateTotalDebitValue(paymentBatches), 0) * 100
                , CalculateHomingAccountChecksum(paymentBatches));
            fileWriter.WriteFileLine(trailerRecord, outputFileName);
        }

        private int GetNumberOfContra(IEnumerable<PaymentBatch> paymentBatches)
        {
            var numberOfContraAndCredit = paymentBatches.Count();
            return numberOfContraAndCredit;
        }

        private static int GetNumberOfCreditRecords(IEnumerable<PaymentBatch> paymentBatches)
        {
            int numberOfCredit = 0;
            foreach (var paymentsBatch in paymentBatches)
            {
                numberOfCredit += paymentsBatch.Payments.Where(x => x.TargetAccount.ACBTypeNumber == 1).Count();
            }
            return numberOfCredit;
        }

        public decimal CalculateNettValue(decimal TotalDebitValue, decimal TotalCreditValue)
        {
            return TotalDebitValue - TotalCreditValue;
        }

        public decimal CalculateTotalDebitValue(IEnumerable<PaymentBatch> paymentBatches)
        {
            decimal TotalsDebiteValue = 0m;
            foreach (var paymentsBatch in paymentBatches)
            {
                TotalsDebiteValue += paymentsBatch.Payments.Sum(x => x.Amount);
            }
            return TotalsDebiteValue;
        }

        public decimal CalculateTotalContraDebitValue(IEnumerable<PaymentBatch> paymentBatches)
        {
            return paymentBatches.Sum(x => x.Amount);
        }

        public int CalculateHomingAccountChecksum(IEnumerable<PaymentBatch> paymentBatches)
        {
            var AccountNumberSum = paymentBatches.Sum(x => Convert.ToDecimal(x.SourceAccount.AccountNumber));
            var Client = paymentBatches.SelectMany(x => x.Payments).Sum(x => Convert.ToDecimal(x.TargetAccount.AccountNumber));
            var TotalHomingAccountSum = AccountNumberSum + Client;
            var Checksum = Math.Floor(TotalHomingAccountSum /
                ((CalculateTotalContraDebitValue(paymentBatches) * 100) +
                (CalculateTotalDebitValue(paymentBatches) * 100)));
            return (int)Checksum;
        }


    }
}
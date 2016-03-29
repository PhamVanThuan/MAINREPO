using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Managers
{
    public interface ICATSFileGenerator
    {
        void WriteHeader(IFileWriter fileWriter, string outputFileName, ICATSFileRecordGenerator catsFileRecordGenerator
            , int fileSequenseNo, string profile, DateTime creationDateTime, CATsEnvironment cATsEnvironment);

        void WritePaymentBatchDetailRecords(int fileSequence, int paymentBatchSequenceNo, PaymentBatch paymentBatch
            , ICATSFileRecordGenerator catsFileRecordGenerator, IFileWriter fileWriter, string outputFileName, string reference);

        void WriteTrailerRecord(SAHL.Services.CATS.IFileWriter fileWriter, string outputFileName
            , ICATSFileRecordGenerator catsFileRecordGenerator, IEnumerable<PaymentBatch> paymentBatchs, int fileSequence);
    }
}
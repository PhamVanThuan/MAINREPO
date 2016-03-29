using FluentAssertions;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy;
using NSubstitute;
using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    [TestFixture]
    public class CATsFileGeneration : WithFakes
    {
        private ICATSFileRecordGenerator catsFileRecordGenerator;
        private CATSFileGenerator catsFileGenerator;

        private string profile;
        private int fileSequenceNo;
        private DateTime creationDateTime;
        private DateTime ActionDateTime;
        private string accountName;
        private int targetBranchCode;
        private string targetAccountNo;
        private int targetAccountType;
        private decimal amount;
        private string reference;
        private string accountName1, subTotalsRecord;
        private int targetBranchCode1;
        private string targetAccountNo1;
        private int targetAccountType1;
        private decimal amount1;
        private string reference1;
        private int sourceBranchCode;
        private string sourceAccountNo;
        private int sourceAccountType;
        private string sourceAccountName;
        private List<Payment> payments;
        private IEnumerable<PaymentBatch> paymentBatches;
        private PaymentBatch paymentsBatch;
        private BankAccountDataModel sourceAccount;
        private int HomingAccountChecksum;
        private IFileWriter fileWriter;
        public string outputFileName;

        private decimal TotalDebit;
        private decimal CalculatedTotalDebit;
        private decimal TotalCantraDebitValue;
        private decimal TotalDetailDebitValue;

        [Test]
        public void GeneratingSubTotal()
        {

            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => GenerateSubTotal())
                 .Then(x => SubtotalRecordCreated())
                .BDDfy("generating sub total");
        }

        [Test]
        public void WritingPaymentBatchDetailsRecords()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => WriteDetailsRecords())
                 .Then(x => GenerateDetailsRecords())
                 .And(x => WriteAllPaymentBatchDetailsRecords(), "Write all payment batch details records")
                 .And(x => WriteTotalsRecords(), "Write payment batch totals record")
                .BDDfy("Writing payment batch details records ");
        }

        [Test]
        public void WritingPaymentBatchDetailsRecordsWithTwoBatches()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => WriteDetailsRecords())
                 .Then(x => GenerateDetailsRecords())
                 .And(x => WriteAllPaymentBatchDetailsRecords(), "Write all payment batch details records")
                 .And(x => WriteTotalsRecords(), "Write sub totals record")
                .BDDfy("Writing payment batch details records ");
        }

        [Test]
        public void WritingHeaderRecords()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => WriteHeader())
                 .Then(x => GenerateHeaderRecord())
                 .And(x => WriteHeaderRecord())
                .BDDfy("Writing header records");
        }

        [Test]
        public void WritingTrailerRecords()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 2000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => WriteTrailer())
                 .Then(x => GenerateTrailerRecord())
                 .And(x => WriteTrailerRecord())
                .BDDfy("Writing trailer records");
        }

        [Test]
        public void CalculatingHomingAccountChecksum()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 2000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => CalculateHomingAccountChecksum())
                 .Then(x => HomingAccountChecksumShouldBe(13143))
                .BDDfy("Calculating homing account check sum");
        }

        [Test]
        public void CalculatingTotalContraDebitValue()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => CalculateTotalContraDebitValue())
                 .Then(x => TotalContraDebitValueShouldBe(amount + amount1))
                .BDDfy("Calculating total contra debit value");
        }

        [Test]
        public void CalculatingTotalDetailDebitValue()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => CalculateTotalDetailDebitValue())
                 .Then(x => TotalDetailDebitValueShouldBe(amount + amount1))
                .BDDfy("Calculating total detail debitValue");
        }

        [Test]
        public void CalculatingTotalDebitValue()
        {
            this.Given(x => AValidPaymentsDetails("SHL02", 1167, new DateTime(2015, 04, 21, 14, 25, 10), new DateTime(2015, 04, 21, 14, 25, 10)
                    , " P. Miller", 632005, "9212020837", 1, 5000M, "1492225 Disburse"
                    , "Ms MARLENE THERESA HECTOR", 42826, "9086504522", 1, 5000M, "1492759 Disburse"
                    , 42826, "51403153", 1, "SA Home Loans (Pty) Limited"),
                    "Given a Profile{0}, File Sequence No {1}, Date Time {2}, Action Date {3}" +
                    ", payment 1) :target account name {4}, target branch code {5}, target account No {6}, target account Type {7} amount {8}, reference {9}" +
                    ", payment 2) :target account name {10}, target branch code {11}, target account No {12}, target account Type {13} amount {14}, reference {15}" +
                    ", Source Branch Code {16}, Source Account {17}, Source Account Type {18}, Source Account Name {19}")
                 .When(x => CalculateTotalDebitValue())
                 .Then(x => TotalDebitValueShouldBe(amount + amount1))
                .BDDfy("Calculating total debit value");
        }

        [Test]
        public void CalculatingNettValue()
        {
            this.Given(x => TotalDebitValue(1222m))
                 .When(x => CalculateNettValue())
                 .Then(x => NettValueShouldBe(1222m))
                .BDDfy("Calculating Nett value");
        }

        private void TotalDebitValue(decimal totalDebitValue)
        {
            catsFileGenerator = new CATSFileGenerator();
            TotalDebit = totalDebitValue;
        }

        private void NettValueShouldBe(decimal totalDebitValue)
        {
            CalculatedTotalDebit.Should().Be(totalDebitValue);
        }

        private void CalculateNettValue()
        {
            var TotalCreditValue = 0m;
            CalculatedTotalDebit = catsFileGenerator.CalculateNettValue(TotalDebit, TotalCreditValue);
        }

        private void HomingAccountChecksumShouldBe(int homingAccountChecksum)
        {
            HomingAccountChecksum.Should().Be(homingAccountChecksum);
        }

        private void CalculateHomingAccountChecksum()
        {
            HomingAccountChecksum = catsFileGenerator.CalculateHomingAccountChecksum(paymentBatches);
        }

        private void TotalDebitValueShouldBe(decimal totalDebit)
        {
            TotalDebit.Should().Be(totalDebit);
        }

        private void CalculateTotalDebitValue()
        {
            TotalDebit = catsFileGenerator.CalculateTotalDebitValue(paymentBatches);
        }

        private void TotalDetailDebitValueShouldBe(decimal amount)
        {
            TotalDetailDebitValue.Should().Be(amount);
        }

        private void CalculateTotalDetailDebitValue()
        {

            TotalDetailDebitValue = catsFileGenerator.CalculateTotalDebitValue(paymentBatches);
        }

        private void TotalContraDebitValueShouldBe(decimal amount)
        {
            TotalCantraDebitValue.Should().Be(amount);
        }

        private void CalculateTotalContraDebitValue()
        {
            TotalCantraDebitValue = catsFileGenerator.CalculateTotalContraDebitValue(paymentBatches);
        }

        private void WriteTrailerRecord()
        {
            fileWriter.WasToldTo(x =>
                x.WriteFileLine("ST0116700000000000000000000020000020000000040000000007000000000000007000000000002000000000700000000000000700000000000000013143"
+ "                                                                          ", Param.IsAny<string>()));
        }

        private void GenerateTrailerRecord()
        {
            catsFileRecordGenerator.WasToldTo(x => x.GenerateTrailerRecord(6, 2, 2, 700000, 700000, 700000, 700000, 13143));
        }

        private void WriteTrailer()
        {
            catsFileRecordGenerator
                .WhenToldTo(x => x.GenerateTrailerRecord(6, 2, 2, 700000, 700000, 700000, 700000, 13143))
                .Return("ST0116700000000000000000000020000020000000040000000007000000000000007000000000002000000000700000000000000700000000000000013143"
                + "                                                                          ");
            catsFileGenerator.WriteTrailerRecord(fileWriter, outputFileName, catsFileRecordGenerator, paymentBatches, 6);
        }

        private void GenerateHeaderRecord()
        {
            catsFileRecordGenerator.WasToldTo(x => x.GenerateHeader(profile, fileSequenceNo, creationDateTime, CATsEnvironment.Live));
        }

        private void WriteHeader()
        {
            catsFileGenerator.WriteHeader(fileWriter
                , outputFileName
                , catsFileRecordGenerator
                , fileSequenceNo
                , profile
                , creationDateTime
                , CATsEnvironment.Live);
        }

        private void WriteHeaderRecord()
        {
            fileWriter.WasToldTo(x => x.WriteFileLine("FHSSVSSHL02011672015042114251020150421L        "
                + "                                                                                    "
                + "                                                                     ", Param.IsAny<string>()));
        }

        private void GenerateDetailsRecords()
        {
            catsFileRecordGenerator.WasToldTo(x => x.GenerateDetailRecord(fileSequenceNo
                , 1, 1, targetBranchCode
                , targetAccountNo, targetAccountType, accountName
                , amount * 100, Param.IsAny<string>()));

            catsFileRecordGenerator.WasToldTo(x => x.GenerateDetailRecord(fileSequenceNo
                , 1, 2, targetBranchCode1
                , targetAccountNo1, targetAccountType1, accountName1
                , amount1 * 100, Param.IsAny<string>()));


        }

        private void WriteAllPaymentBatchDetailsRecords()
        {
            fileWriter.WasToldTo(x => x.WriteFileLine("SD00004001002", Param.IsAny<string>()));
            fileWriter.WasToldTo(x => x.WriteFileLine("SD00003001001", Param.IsAny<string>()));
        }

        private void WriteTotalsRecords()
        {
            fileWriter.WasToldTo(x => x.WriteFileLine("SC00003001D04282600000514031531SA Home Loans (Pty) Limited   000000000010000SAHL      SPV 34"
                + "                                                                                                            ", Param.IsAny<string>()));
        }

        private void WriteDetailsRecords()
        {
            catsFileRecordGenerator.WhenToldTo(x => x.GenerateDetailRecord(fileSequenceNo
                , 1, 2, targetBranchCode1
                , targetAccountNo1, targetAccountType1, accountName1
                , amount1 * 100, Param.IsAny<string>())).Return("SD00004001002");
            catsFileRecordGenerator.WhenToldTo(x => x.GenerateDetailRecord(fileSequenceNo
                , 1, 1, targetBranchCode
                , targetAccountNo, targetAccountType, accountName
                , amount * 100, Param.IsAny<string>())).Return("SD00003001001");
            catsFileGenerator.WritePaymentBatchDetailRecords(fileSequenceNo, 1, paymentsBatch, catsFileRecordGenerator, fileWriter, outputFileName, reference);
        }


        private void SubtotalRecordCreated()
        {
            catsFileRecordGenerator.WasToldTo(t => t.GenerateSubTotalRecord(1, 1
                , sourceBranchCode
                , sourceAccountNo
                , sourceAccountType
                , sourceAccountName
                , Convert.ToInt64(10000 * 100)
                , Param.IsAny<string>()));
        }

        private void GenerateSubTotal()
        {
            subTotalsRecord = catsFileGenerator.GenerateSubTotalRecord(catsFileRecordGenerator, paymentsBatch, 1, reference, 1);
        }


        private void AValidPaymentsDetails(string profile, int fileSequenceNo, DateTime creationDateTime, DateTime ActionDateTime
            , string accountName, int targetBranchCode, string targetAccountNo, int targetAccountType, decimal amount, string reference
            , string accountName1, int targetBranchCode1, string targetAccountNo1, int targetAccountType1, decimal amount1, string reference1
            , int sourceBranchCode, string sourceAccountNo, int sourceAccountType, string sourceAccountName)
        {

            this.profile = profile;
            this.fileSequenceNo = fileSequenceNo;
            this.creationDateTime = creationDateTime;
            this.ActionDateTime = ActionDateTime;
            this.accountName = accountName;
            this.targetBranchCode = targetBranchCode;
            this.targetAccountNo = targetAccountNo;
            this.targetAccountType = targetAccountType;
            this.amount = amount;
            this.reference = reference;
            this.accountName1 = accountName1;
            this.targetBranchCode1 = targetBranchCode1;
            this.targetAccountNo1 = targetAccountNo1;
            this.targetAccountType1 = targetAccountType1;
            this.amount1 = amount1;
            this.reference1 = reference1;
            this.sourceBranchCode = sourceBranchCode;
            this.sourceAccountNo = sourceAccountNo;
            this.sourceAccountType = sourceAccountType;
            this.sourceAccountName = sourceAccountName;
            this.outputFileName = System.IO.Path.GetTempPath() + "SHL04_Disbursement";
            catsFileGenerator = new CATSFileGenerator();
            catsFileRecordGenerator = An<ICATSFileRecordGenerator>();
            payments = new List<Payment>();
            payments.Add(new Payment(new BankAccountDataModel(targetBranchCode.ToString(), targetAccountNo, targetAccountType, accountName, null, null), amount, reference));
            payments.Add(new Payment(new BankAccountDataModel(targetBranchCode1.ToString(), targetAccountNo1, targetAccountType1, accountName1, null, null), amount1, reference1));
            catsFileRecordGenerator.WhenToldTo(m => m.GenerateSubTotalRecord(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()
                , Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Convert.ToInt64(amount + amount1) * 100, Param.IsAny<string>()))
                .Return("SC00003001D04282600000514031531SA Home Loans (Pty) Limited   000000000010000SAHL      SPV 34                                                                                                            ");

            catsFileRecordGenerator.WhenToldTo(x => x.GenerateHeader(profile, fileSequenceNo, creationDateTime, CATsEnvironment.Live))
                .Return("FHSSVSSHL02011672015042114251020150421L                                                                                                                                                                 ");
            sourceAccount = new BankAccountDataModel(sourceBranchCode.ToString(), sourceAccountNo, sourceAccountType, sourceAccountName, null, null);
            paymentsBatch = new PaymentBatch(payments, sourceAccount, payments.Sum(x => x.Amount), reference);
            fileWriter = An<IFileWriter>();

            paymentBatches = new List<PaymentBatch>
            {new PaymentBatch(new List<Payment>
                {
                   new Payment(new BankAccountDataModel(targetBranchCode.ToString(), targetAccountNo, targetAccountType, accountName, null, null), amount, reference)}
                    , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "Reference")
                ,new PaymentBatch(new List<Payment>{
                    new Payment(new BankAccountDataModel(targetBranchCode1.ToString(), targetAccountNo1, targetAccountType1, accountName1, null, null), amount1, reference1)
                }
                    , new BankAccountDataModel("42826", "51403153", (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 2000M, "Reference")
            };
        }
    }
}
using NUnit.Framework;
using System;
using TestStack.BDDfy;
using FluentAssertions;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Utils;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    [TestFixture]
    public class CATsFileTrailerRecordGeneration
    {
        private string trailerRecordLine;
        private int fileSequenceNo;
        private int numberOfContraRecords;
        private int numberOfCreditRecords;
        private decimal totalContraDebitValue;
        private decimal totalDetailDebitValue;
        private decimal totalDebitValue;
        private decimal NettValue;
        private int homingAccountChecksum;
        private CATSFileRecordGenerator catsFileGenerator;

        [Test]
        public void GeneratingCATsFileTrailerRecord()
        {
            this.Given(x => AValidTrillerRecord(123, 2, 2, 368198.0M, 368198.0M, 368198.0M, 368198.0M, 84418),
                 "Given a file sequence No {0}, number of contra records {1}, number of credit records {2}" +
                 ", total contra debit value {3}, total detail debit value {4}" +
                 ", total debit value {5}, Nett value {6}, homing account checksum {7}")
                 .When(x => GenerateTrailerRecordLine())
                 .Then(x => TheDataAtPositionShouldBe(1, 2, CATsFileRecordType.TrailerRecord), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(3, 5, "00123"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(8, 15, "000000000000000"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(23, 7, "0000002"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(30, 3, "000"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(33, 3, "002"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(36, 9, "000000004"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(45, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(60, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(75, 7, "0000002"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(82, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(97, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(112, 15, "000000000084418"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TrailerRecordLengthShouldBe200())
                .BDDfy("Generating a trailer record");
        }

        [Test]
        public void GeneratingCATsFileTrailerRecordAccStdBankSpec()
        {
            this.Given(x => AValidTrillerRecord(123, 2, 2, 368198.0M, 368198.0M, 368198.0M, 368198.0M, 84418),
                 "Given a file sequence No {0}, number of contra records {1}, number of credit records {2}" +
                 ", total contra debit value {3}, total detail debit value {4}" +
                 ", total debit value {5}, Nett value {6}, homing account checksum {7}")
                 .When(x => GenerateTrailerRecordLineAccToStdBankSpec())
                 .Then(x => TheDataAtPositionShouldBe(1, 2, CATsFileRecordType.TrailerRecord), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(3, 5, "00123"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(8, 7, "0000000"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(15, 7, "0000002"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(22, 3, "000"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(25, 3, "002"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(28, 9, "000000004"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(37, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(52, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(67, 15, "000000000000002"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(82, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(97, 15, "000000000368198"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TheDataAtPositionShouldBe(112, 15, "000000000084418"), "The data at position [{0},{1}] should be {2}")
                 .And(x => TrailerRecordLengthShouldBe200())
                .BDDfy("Generating a trailer record According to standard bank spec");
        }

        private void GenerateTrailerRecordLineAccToStdBankSpec()
        {
            catsFileGenerator = new CATSFileRecordGenerator();
            trailerRecordLine = catsFileGenerator.GenerateTrailerRecordAccToStdBankSpec(fileSequenceNo, numberOfContraRecords, numberOfCreditRecords
            , totalContraDebitValue, totalDetailDebitValue, totalDebitValue, NettValue, homingAccountChecksum);
        }

        private void AValidTrillerRecord(int fileSequenceNo, int numberOfContraRecords, int numberOfCreditRecords
            , decimal totalContraDebitValue, decimal totalDetailDebitValue, decimal totalDebitValue, decimal NettValue, int homingAccountChecksum)
        {
            this.fileSequenceNo = fileSequenceNo;
            this.numberOfContraRecords = numberOfContraRecords;
            this.numberOfCreditRecords = numberOfCreditRecords;
            this.totalContraDebitValue = totalContraDebitValue;
            this.totalDetailDebitValue = totalDetailDebitValue;
            this.totalDebitValue = totalDebitValue;
            this.NettValue = NettValue;
            this.homingAccountChecksum = homingAccountChecksum;
        }

        private void TrailerRecordLengthShouldBe200()
        {
            trailerRecordLine.Length.Should().Be(200);
        }

        private void TheDataAtPositionShouldBe(int startPosition, int length, string value)
        {
            trailerRecordLine.Substring(startPosition - 1, length).Should().Be(value);
        }

        private void GenerateTrailerRecordLine()
        {
            catsFileGenerator = new CATSFileRecordGenerator();
            trailerRecordLine = catsFileGenerator.GenerateTrailerRecord(fileSequenceNo, numberOfContraRecords, numberOfCreditRecords
            , totalContraDebitValue, totalDetailDebitValue, totalDebitValue, NettValue, homingAccountChecksum);
        }
    }
}
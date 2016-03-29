using NUnit.Framework;
using System;
using TestStack.BDDfy;
using FluentAssertions;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Utils;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    [TestFixture]
    public class CATsFileDetailRecordGeneration
    {
        private int fileSequenceNo;
        private int subBatchSequenceNo;
        private int transactionSequenceNo;
        private string userReference;
        private string detailRecordLine;
        private Decimal disbursementAmountInCents;
        private CATSFileRecordGenerator catsFileGenerator;
        private int ACBBranchCode;
        private string ACBAccount;
        private int ACBAccountType;
        private string ACBAccountName;

        [Test]
        public void GeneratingCATsFileDetailRecord()
        {
            this.Given(x => AValidDetailRecord(123, 1, 10, 254905, "62011554046", 1, "RCS JV Account", 1121.0M, "1724072 Disburse"),
                "Given a file sequence No {0}, sub batch  sequence No {1}, transaction sequence No {2}," +
                 ", ACB branch code {3}, ACB account No {4}, ACB account Type {5}" +
                ", account name {6}, disbursement mount in cents {7}, user reference {8}")
                 .When(x => GenerateDetailRecordLine())
                 .Then(x => TheDataAtPositionShouldBe(1, 2, CATsFileRecordType.DetailRecord))
                 .And(x => TheDataAtPositionShouldBe(3, 5, "00123"))
                 .And(x => TheDataAtPositionShouldBe(8, 3, "001"))
                 .And(x => TheDataAtPositionShouldBe(11, 5, "00010"))
                 .And(x => TheDataAtPositionShouldBe(16, 1, "C"))
                 .And(x => TheDataAtPositionShouldBe(17, 6, "254905"))
                 .And(x => TheDataAtPositionShouldBe(23, 13, "0062011554046"))
                 .And(x => TheDataAtPositionShouldBe(36, 1, "1"))
                 .And(x => TheDataAtPositionShouldBe(37, 30, "RCS JV Account".PadRight(30)))
                 .And(x => TheDataAtPositionShouldBe(67, 16, "".PadRight(16)))
                 .And(x => TheDataAtPositionShouldBe(83, 15, disbursementAmountInCents.ToString().PadLeft(15, '0')))
                 .And(x => TheDataAtPositionShouldBe(98, 30, "1724072 Disburse".PadLeft(30)))
                 .And(x => TheDataAtPositionShouldBe(128, 13, "".PadLeft(13, '0')))
                 .And(x => DetailRecordLengthShouldBe200())
                .BDDfy("Generating a CATs detail record");
        }

        private void DetailRecordLengthShouldBe200()
        {
            detailRecordLine.Length.Should().Be(200);
        }

        private void AValidDetailRecord(int fileSequenceNo, int subBatchSequenceNo, int transactionSequenceNo
            , int ACBBranchCode, string ACBAccount, int ACBAccountType, string ACBAccountName,
            decimal disbursementAmountInCents, string userReference)
        {
            this.catsFileGenerator = new CATSFileRecordGenerator();
            this.fileSequenceNo = fileSequenceNo;
            this.subBatchSequenceNo = subBatchSequenceNo;
            this.transactionSequenceNo = transactionSequenceNo;
            this.ACBBranchCode = ACBBranchCode;
            this.ACBAccount = ACBAccount;
            this.ACBAccountType = ACBAccountType;
            this.ACBAccountName = ACBAccountName;
            this.disbursementAmountInCents = disbursementAmountInCents;
            this.userReference = userReference;
        }

        private void TheDataAtPositionShouldBe(int startPosition, int length, string value)
        {
            detailRecordLine.Substring(startPosition - 1, length).Should().Be(value);
        }

        private void GenerateDetailRecordLine()
        {
            detailRecordLine = catsFileGenerator.GenerateDetailRecord(fileSequenceNo, subBatchSequenceNo
                , transactionSequenceNo, ACBBranchCode, ACBAccount, ACBAccountType, ACBAccountName, disbursementAmountInCents, userReference);
        }
    }
}
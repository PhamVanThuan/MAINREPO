using NUnit.Framework;
using System;
using TestStack.BDDfy;
using FluentAssertions;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Utils;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    [TestFixture]
    public class CATsFileTotalsGeneration
    {
        private int fileSequenceNo;
        private int subBatchSequenceNo;
        private string detailRecordLine;
        private long totalAmountInCents;
        private CATSFileRecordGenerator catsFileGenerator;
        private int ACBBranchCode;
        private string ACBAccount;
        private int ACBAccountType;
        private string Reference;
        private string ACBAccountName;

        [Test]
        public void GeneratingCATsFileSubTotalRecord()
        {
            this.Given(x => AValidSubTotalRecord(123, 1, 25490, "62011554046", 1, "RCS JV Account", 1121, "SAHL      SPV 20"),
                "Given a file sequence No {0}, sub batch  sequence No {1}" +
                 ", ACB branch code {2}, ACB account No {3}, ACB account Type {4}" +
                ", account name {5}, total mount in cents {6}")
                 .When(x => GenerateSubTotalRecordLine())
                 .Then(x => TheDataAtPositionShouldBe(detailRecordLine, 1, 2, CATsFileRecordType.Subtotal))
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 3, 5, "00123"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 8, 3, "001"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 11, 1, "D"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 12, 6, "025490"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 18, 13, "0062011554046"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 31, 1, "1"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 32, 30, "RCS JV Account".PadRight(30)), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 62, 15, "000000000001121"), "The data at position [{1},{2}] should be {3}")
                 .And(x => TheDataAtPositionShouldBe(detailRecordLine, 77, 30, Reference.PadRight(30)), "The data at position [{1},{2}] should be {3}")
                 .And(x => DetailRecordLengthShouldBe200())
                .BDDfy("Generating a CATs sub total record");
        }

        private void DetailRecordLengthShouldBe200()
        {
            detailRecordLine.Length.Should().Be(200);
        }

        private void AValidSubTotalRecord(int fileSequenceNo, int subBatchSequenceNo
            , int ACBBranchCode, string ACBAccount, int ACBAccountType, string ACBAccountName,
            long totalAmountInCents, string reference)
        {
            this.catsFileGenerator = new CATSFileRecordGenerator();
            this.fileSequenceNo = fileSequenceNo;
            this.subBatchSequenceNo = subBatchSequenceNo;
            this.ACBBranchCode = ACBBranchCode;
            this.ACBAccount = ACBAccount;
            this.ACBAccountType = ACBAccountType;
            this.ACBAccountName = ACBAccountName;
            this.totalAmountInCents = totalAmountInCents;
            this.Reference = reference;
        }

        private void TheDataAtPositionShouldBe(string text, int startPosition, int length, string value)
        {
            text.Substring(startPosition - 1, length).Should().Be(value);
        }

        private void GenerateSubTotalRecordLine()
        {
            detailRecordLine = catsFileGenerator.GenerateSubTotalRecord(fileSequenceNo, subBatchSequenceNo
                , ACBBranchCode, ACBAccount, ACBAccountType, ACBAccountName, totalAmountInCents, Reference);
        }
    }
}
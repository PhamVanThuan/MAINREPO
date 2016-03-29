using NUnit.Framework;
using System;
using TestStack.BDDfy;
using FluentAssertions;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Enums;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    [TestFixture]
    public class CATsFileHeaderGeneration
    {
        private string fileHeader;
        private string bankProfileNo;
        private int fileSequence;
        private DateTime creationDateTime;
        private CATsEnvironment env;
        private CATSFileRecordGenerator catsFileGenerator;

        [Test]
        public void GeneratingCATsFileHeader()
        {
            this.Given(x => AProfileNoFileSeqNoCreationDateTimeAndEnv("SHL04", 123, new DateTime(2015, 04, 01, 12, 11, 1), CATsEnvironment.Live),
                "Given a profile No {0}, file sequence No {1}, creation date time {2} and env {3}")
                .When(x => GeneratingACATsFileHeader(), "When generating a CATs file header")
                .Then(x => TheHeaderShouldStartWithFHSSVS(), "The header should start with FHSSVS")
                .And(x => TheDataAtPositionShouldBe(7, 5, "SHL04"), "The data at position [{0},{1}] should be {2}")
                .And(x => TheDataAtPositionShouldBe(12, 5, "00123"), "The data at position [{0},{1}] should be {2}")
                .And(x => TheDataAtPositionShouldBe(17, 8, "20150401"), "The data at position [{0},{1}] should be {2}")
                .And(x => TheDataAtPositionShouldBe(25, 6, "121101"), "The data at position [{0},{1}] should be {2}")
                .And(x => TheDataAtPositionShouldBe(31, 8, "20150401"), "The data at position [{0},{1}] should be {2}")
                .And(x => TheDataAtPositionShouldBe(39, 1, "L"), "The data at position [{0},{1}] should be {2}")
                .And(x => TheDataAtPositionShouldBe(40, 161, "".PadRight(161)), "The data at position [{0},{1}] should be white spaces")
                .And(x => DetailRecordLengthShouldBe200())
                .BDDfy("Generating a CATs file header");
        }

        [Test]
        public void GeneratingCATsFileHeaderForTesting()
        {
            this.Given(x => AProfileNoFileSeqNoCreationDateTimeAndEnv("SHL04", 123, new DateTime(2015, 04, 01, 12, 11, 1), CATsEnvironment.Test),
                "Given a profile No {0}, file sequence No {1}, creation date time {2} and env {3}")
                .When(x => GeneratingACATsFileHeader(), "When generating a CATs file header")
                .Then(x => TheDataAtPositionShouldBe(39, 1, "T"), "The data at position [{0},{1}] should be {2}")
                .BDDfy("Generating a CATs file header for Testing");
        }

        private void TheHeaderShouldStartWithFHSSVS()
        {
            Assert.That(fileHeader.StartsWith("FHSSVS"));
        }

        private void TheDataAtPositionShouldBe(int startPosition, int length, string value)
        {
            fileHeader.Substring(startPosition - 1, length).Should().Be(value);
        }

        private void GeneratingACATsFileHeader()
        {
            fileHeader = catsFileGenerator.GenerateHeader(this.bankProfileNo, this.fileSequence, this.creationDateTime, this.env);
        }

        private void AProfileNoFileSeqNoCreationDateTimeAndEnv(string bankProfileNo, int fileSequence, DateTime creationDateTime, CATsEnvironment env)
        {
            catsFileGenerator = new CATSFileRecordGenerator();
            this.bankProfileNo = bankProfileNo;
            this.fileSequence = fileSequence;
            this.creationDateTime = creationDateTime;
            this.env = env;
        }

        private void DetailRecordLengthShouldBe200()
        {
            fileHeader.Length.Should().Be(200);
        }
    }
}
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using TestStack.BDDfy;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    [TestFixture]
    public class CreatingFile
    {
        private String fileName;
        private IFileWriter fileWriter;

        [Test]
        public void CreatingAFile()
        {
            this.Given(x => FullFileNamePath("SHL04_Disbursement"),
                 "Given a file name {0}")
                 .When(x => CreateCATsFile(), "Create CATs file")
                 .Then(x => CATsFileIsCreated(), "CATs file should be created")
                .BDDfy("Creating a CATs file");
        }

        private void CATsFileIsCreated()
        {
            File.Exists(fileName).Should().Be(true);
        }

        private void CreateCATsFile()
        {
            fileWriter.CreateFile(fileName);
        }

        private void FullFileNamePath(string fileName)
        {
            this.fileName = System.IO.Path.GetTempPath() + fileName;
            this.fileWriter = new FileWriter();
        }
    }
}
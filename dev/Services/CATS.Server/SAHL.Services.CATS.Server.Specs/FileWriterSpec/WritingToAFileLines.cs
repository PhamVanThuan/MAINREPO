using NUnit.Framework;
using System;
using System.IO;
using TestStack.BDDfy;

namespace SAHL.Services.CATS.Server.Specs.FileWriterSpec
{
    [TestFixture]
    public class WritingToAFileLines
    {
        private String fileName;
        private String text;
        private IFileWriter fileWriter;

        [Test]
        public void WritingFileLines()
        {
            this.Given(x => FileAndTextToWriteToAFile("SHL04_Disbursement", "Write this text to a file"),
                 "Given a file name: {0} and text to write to a file: \"{1}\"")
                 .When(x => WritingLineToFile())
                 .Then(x => LineShouldBeInFile(text))
                .BDDfy("Writing text to a file");
        }

        private void LineShouldBeInFile(string text)
        {
            bool fileConatsText = false;
            using (StreamReader sr = new StreamReader(fileName))
            {
                while (sr.Peek() >= 0)
                {
                    string nextLine = sr.ReadLine();
                    if (nextLine.Equals(text))
                    {
                        fileConatsText = true;
                    }
                }
            }
            Assert.That(fileConatsText);
        }

        private void WritingLineToFile()
        {
            var fs = System.IO.File.Create(fileName);
            fs.Close();
            fileWriter.WriteFileLine(text, fileName);
        }

        private void FileAndTextToWriteToAFile(string fileName, string text)
        {
            this.fileName = System.IO.Path.GetTempPath() + fileName;
            this.text = text;
            this.fileWriter = new FileWriter();
        }
    }
}
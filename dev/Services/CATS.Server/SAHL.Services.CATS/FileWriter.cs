using System;
using System.IO;

namespace SAHL.Services.CATS
{
    public class FileWriter : IFileWriter
    {
        public void WriteFileLine(string text, string fileName)
        {
            using (StreamWriter w = File.AppendText(fileName))
            {
                w.WriteLine(text);
                w.Close();
            }
        }

        public string CreateFile(string fileName)
        {
            var fs = System.IO.File.Create(fileName);
            fs.Close();
            return fileName;
        }
    }
}
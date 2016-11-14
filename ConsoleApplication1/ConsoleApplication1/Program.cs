using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fileName = @"i:\temp\AAP00.PD06.P.OUT.PAYMENTS.G0353";
            var fileContents = System.IO.File.ReadAllText(fileName);
            if (fileName.Contains("PAYMENTS"))
            {
                string mainframePlaceholder = "MF=";
                fileContents = fileContents.Replace("{DATE}", DateTime.Now.ToString("yyyyMMdd"));
                if (fileContents.Contains(mainframePlaceholder))
                {
                    var mainFrame = fileContents.Substring(fileContents.IndexOf("MF=") + 3, 3);
                    string nextBatchNumber = "123";
                    string batchPlaceHolder = "0000";
                    var stringToReplace = $"{mainframePlaceholder}{mainFrame}{batchPlaceHolder}";
                    string newBatchNumber = string.Concat(stringToReplace.Remove(stringToReplace.IndexOf(mainFrame) + 3, nextBatchNumber.Length), nextBatchNumber);
                    fileContents = fileContents.Replace(stringToReplace, newBatchNumber.Replace(mainframePlaceholder, string.Empty));
                }
            }
            var destinationPath = Path.Combine(@"i:\temp\", Guid.NewGuid().ToString());
            var fileHandle = System.IO.File.Create(destinationPath);
            fileHandle.Close();
            System.IO.File.WriteAllText(destinationPath, fileContents);
        }
    }
}
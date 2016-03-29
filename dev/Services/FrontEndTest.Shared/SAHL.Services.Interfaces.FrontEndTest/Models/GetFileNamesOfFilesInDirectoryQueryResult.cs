using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetFileNamesOfFilesInDirectoryQueryResult
    {
        public GetFileNamesOfFilesInDirectoryQueryResult(string directoryName, string fileName, string fullName, DateTime creationTime)
        {
            this.DirectoryName = directoryName;
            this.FileName = fileName;
            this.FullName = fullName;
            this.CreationTime = creationTime;
        }

        public string DirectoryName { get; set; }
        public string FileName { get; set; }
        public string FullName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

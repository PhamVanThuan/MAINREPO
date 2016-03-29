using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetFileNamesOfFilesInDirectoryQuery : ServiceQuery<GetFileNamesOfFilesInDirectoryQueryResult>, IFrontEndTestQuery, IServiceQuery
    {
        public string DirectoryPath { get; set; }
        public string SearchPattern { get; set; }
        public SearchOption SearchOption { get; set; }

        public GetFileNamesOfFilesInDirectoryQuery(string directoryPath, string searchPattern, SearchOption searchOption)
        {
            this.DirectoryPath = directoryPath;
            this.SearchPattern = searchPattern;
            this.SearchOption = searchOption;
        }
    }
}

using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.QueryHandlers
{
    public class GetFileNamesOfFilesInDirectoryQueryHandler : IServiceQueryHandler<GetFileNamesOfFilesInDirectoryQuery>
    {
        public ISystemMessageCollection HandleQuery(GetFileNamesOfFilesInDirectoryQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            var results = new List<GetFileNamesOfFilesInDirectoryQueryResult>();
            var files = new DirectoryInfo(query.DirectoryPath).GetFiles(query.SearchPattern, query.SearchOption).ToList();
            foreach (var file in files)
            {
                results.Add(new GetFileNamesOfFilesInDirectoryQueryResult(file.DirectoryName, file.Name, file.FullName, file.CreationTime));
            }
            query.Result = new ServiceQueryResult<GetFileNamesOfFilesInDirectoryQueryResult>(results);
            return messages;
        }
    }
}

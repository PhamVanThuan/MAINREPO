using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.IO;

namespace SAHL.Services.FrontEndTest.QueryHandlers
{
    public class GetFileUsingPathQueryHandler : IServiceQueryHandler<GetFileUsingPathQuery>
    {
        public ISystemMessageCollection HandleQuery(GetFileUsingPathQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            var results = new List<string>();
            results.Add(File.ReadAllText(query.fullpath));
            query.Result = new ServiceQueryResult<string>(results);
            return messages;
        }
    }
}

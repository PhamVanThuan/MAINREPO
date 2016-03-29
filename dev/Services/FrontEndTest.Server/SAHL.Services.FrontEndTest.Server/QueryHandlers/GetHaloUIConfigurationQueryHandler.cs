using Newtonsoft.Json;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Collections.Generic;
using System.IO;
namespace SAHL.Services.FrontEndTest.QueryHandlers
{
    public class GetHaloUIConfigurationQueryHandler : IServiceQueryHandler<GetHaloUIConfigurationQuery>
    {
        public ISystemMessageCollection HandleQuery(GetHaloUIConfigurationQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();

            var jsonReader = new StreamReader("HaloUIConfig.json");
            var json = jsonReader.ReadToEnd();
            jsonReader.Close();

            var haloUIConfigurationQueryResult = new GetHaloUIConfigurationQueryResult()
            {
                HaloUIConfigJson = json,
            };
            query.Result = new ServiceQueryResult<GetHaloUIConfigurationQueryResult>(new List<GetHaloUIConfigurationQueryResult>()
                {
                    haloUIConfigurationQueryResult
                });
            return systemMessages;
        }
    }
}

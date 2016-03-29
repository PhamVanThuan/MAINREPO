using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetLatestEventForGenericKeyTypeQuery : ServiceQuery<GetLatestEventForGenericKeyTypeQueryResult>, IFrontEndTestQuery, 
                                                        ISqlServiceQuery<GetLatestEventForGenericKeyTypeQueryResult>
    {
        public GetLatestEventForGenericKeyTypeQuery(int genericKeyTypeKey) 
        {
            GenericKeyTypeKey = genericKeyTypeKey;
        }
        public int GenericKeyTypeKey { get; protected set; }
    }
}

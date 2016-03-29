using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetImageIndexDataQuery : ServiceQuery<GetImageIndexDataQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetImageIndexDataQueryResult>
    {
        public int StorId { get; set; }

        public GetImageIndexDataQuery(int StorId)
        {
            this.StorId = StorId;
        }
    }
}
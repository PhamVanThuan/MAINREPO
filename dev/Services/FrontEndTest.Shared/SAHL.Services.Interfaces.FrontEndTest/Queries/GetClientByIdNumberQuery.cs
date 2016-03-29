using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetClientByIdNumberQuery : ServiceQuery<LegalEntityDataModel>, IFrontEndTestQuery, ISqlServiceQuery<LegalEntityDataModel>
    {
        public string IdNumber { get; protected set; }

        public GetClientByIdNumberQuery(string IdNumber)
        {
            this.IdNumber = IdNumber;
        }
    }
}
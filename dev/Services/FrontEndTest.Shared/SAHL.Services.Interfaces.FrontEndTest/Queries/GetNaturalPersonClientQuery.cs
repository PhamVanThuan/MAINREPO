using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetNaturalPersonClientQuery : ServiceQuery<LegalEntityDataModel>, IFrontEndTestQuery, ISqlServiceQuery<LegalEntityDataModel>
    {
        public bool IsActive { get; protected set; }

        public GetNaturalPersonClientQuery(bool isActive)
        {
            this.IsActive = isActive;
        }
    }
}
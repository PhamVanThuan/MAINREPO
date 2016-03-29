using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetDocumentByGUIDDataStorRecordsQuery : ServiceQuery<LegalEntityDataModel>, IFrontEndTestQuery, ISqlServiceQuery<LegalEntityDataModel>
    {
        public string GUID { get; protected set; }

        public GetDocumentByGUIDDataStorRecordsQuery(string GUID)
        {
            this.GUID = GUID;
        }
    }
}

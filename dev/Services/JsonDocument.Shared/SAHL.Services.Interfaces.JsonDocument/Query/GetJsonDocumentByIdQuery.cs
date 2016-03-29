using SAHL.Core.Data;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.JsonDocument.Query
{
    public class GetJsonDocumentByIdQuery : ServiceQuery<GetJsonDocumentByIdQueryResult>, ISqlServiceQuery<GetJsonDocumentByIdQueryResult>
    {
        public GetJsonDocumentByIdQuery(Guid id)
            : base(id)
        {
        }
    }
}
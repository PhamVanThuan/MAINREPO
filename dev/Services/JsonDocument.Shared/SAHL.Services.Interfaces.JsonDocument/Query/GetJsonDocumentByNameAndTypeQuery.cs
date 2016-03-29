using SAHL.Core.Data;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.JsonDocument.Query
{
    public class GetJsonDocumentByNameAndTypeQuery : ServiceQuery<GetJsonDocumentByNameAndTypeQueryResult>, ISqlServiceQuery<GetJsonDocumentByNameAndTypeQueryResult>
    {
        public GetJsonDocumentByNameAndTypeQuery(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public string Type { get; protected set; }

        public string Name { get; protected set; }
    }
}
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetPublishedMessageSetByMessageSetIdQuery : ServiceQuery<PublishedMessageSetQueryResult>, ISqlServiceQuery<PublishedMessageSetQueryResult>
    {
        public GetPublishedMessageSetByMessageSetIdQuery(Guid messageSetId)
        {
            this.MessageSetId = messageSetId;
        }

        public Guid MessageSetId { get; protected set; }
    }
}
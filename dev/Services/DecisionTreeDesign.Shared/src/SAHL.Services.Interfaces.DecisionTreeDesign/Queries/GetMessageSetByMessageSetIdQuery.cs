using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetMessageSetByMessageSetIdQuery : ServiceQuery<GetMessageSetByMessageSetIdQueryResult>, ISqlServiceQuery<GetMessageSetByMessageSetIdQueryResult>
    {
        public Guid MessageSetId { get; protected set; }

        public GetMessageSetByMessageSetIdQuery(Guid messageSetId)
        {
            this.MessageSetId = messageSetId;
        }
    }
}

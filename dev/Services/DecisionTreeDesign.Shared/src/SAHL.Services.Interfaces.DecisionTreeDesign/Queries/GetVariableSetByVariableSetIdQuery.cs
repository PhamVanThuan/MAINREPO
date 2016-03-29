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
    public class GetVariableSetByVariableSetIdQuery : ServiceQuery<GetVariableSetByVariableSetIdQueryResult>, ISqlServiceQuery<GetVariableSetByVariableSetIdQueryResult>
    {
        public GetVariableSetByVariableSetIdQuery(Guid variableSetId)
        {
            this.VariableSetId = variableSetId;
        }

        public Guid VariableSetId { get; protected set; }
    }
}

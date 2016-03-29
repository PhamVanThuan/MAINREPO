using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.X2Engine2.Tests.X2.Models;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public sealed class X2ActivitiesSqlStatement : ISqlStatement<X2.Models.X2StateActivity>
    {
        public int WorkflowId { get; protected set; }

        public X2ActivitiesSqlStatement(int WorkflowId)
        {
            this.WorkflowId = @WorkflowId;
        }
        public string GetStatement()
        {
            return @"select 
	            s.Name as State,
	            a.Name as Activity
            from x2.x2.activity a 
                join X2.X2.state s
		            on a.stateID=s.id
            where a.WorkflowId=@WorkflowId and s.WorkflowId=@WorkflowId";
        }
    }
}

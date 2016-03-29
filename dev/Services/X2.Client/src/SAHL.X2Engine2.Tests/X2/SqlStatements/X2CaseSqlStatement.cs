using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.X2Engine2.Tests.X2.Models;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public sealed class X2CaseSqlStatement : ISqlStatement<X2Case>
    {
        public long InstanceId { get; protected set; }

        public X2CaseSqlStatement(long InstanceId)
        {
            this.InstanceId = InstanceId;
        }
        public string GetStatement()
        {
            return @"select 
	            i.ID as InstanceId,
	            s.Name as State,
	            w.Name as Workflow,
                p.Name as Process
            from x2.x2.instance i 
                join X2.X2.state s
		            on i.stateID=s.id
	            join X2.X2.WorkFlow w 
		            on i.WorkFlowID=w.ID
                join X2.X2.Process p
                    on w.ProcessID=p.ID
            where i.ID=@InstanceId";
        }
    }
}

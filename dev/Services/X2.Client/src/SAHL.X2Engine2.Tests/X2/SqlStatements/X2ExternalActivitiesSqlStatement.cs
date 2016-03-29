using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.X2Engine2.Tests.X2.Models;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public sealed class X2ExternalActivitiesSqlStatement : ISqlStatement<X2.Models.X2ExternalActivity>
    {
        public int WorkflowId { get; protected set; }

        public X2ExternalActivitiesSqlStatement(int WorkflowId)
        {
            this.WorkflowId = @WorkflowId;
        }
        public string GetStatement()
        {
            return @"select a.ID from x2.x2.ExternalActivity a
                     where a.WorkflowId=@WorkflowId";
        }
    }
}

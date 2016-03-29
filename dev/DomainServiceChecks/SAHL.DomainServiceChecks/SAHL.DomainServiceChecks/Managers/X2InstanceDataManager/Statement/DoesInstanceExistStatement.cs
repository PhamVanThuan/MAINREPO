using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.X2InstanceDataManager.Statement
{
    public class DoesInstanceExistStatement : ISqlStatement<int>
    {
        public int InstanceId { get; protected set; }

        public DoesInstanceExistStatement(int InstanceId)
        {
            this.InstanceId = InstanceId;
        }
        public string GetStatement()
        {
            return @"SELECT count(*)
              FROM [X2].[X2].[Instance] 
              Where ID = @InstanceId";
        }

    }
}

using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class DeleteAllScheduledActivitiesSqlStatement : ISqlStatement<ScheduledActivityDataModel>
    {
        public long InstanceId { get; set; }

        public DeleteAllScheduledActivitiesSqlStatement(long instanceId)
        {
            this.InstanceId = instanceId;
        }

        public string GetStatement()
        {
            return "DELETE FROM [x2].[x2].[ScheduledActivity] with(rowlock) WHERE InstanceID=@InstanceId";
        }
    }
}

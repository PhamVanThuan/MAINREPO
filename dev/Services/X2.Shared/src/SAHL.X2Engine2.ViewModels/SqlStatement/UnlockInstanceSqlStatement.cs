using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class UnlockInstanceSqlStatement : ISqlStatement<InstanceDataModel>
    {
        public long InstanceId { get; protected set; }

        public UnlockInstanceSqlStatement(long instanceId)
        {
            this.InstanceId = instanceId;
        }

        public string GetStatement()
        {
            return @"UPDATE [x2].[x2].[Instance] with(rowlock) SET ActivityADUserName = null, ActivityID = null, ActivityDate = null WHERE ID = @InstanceId";
        }
    }
}
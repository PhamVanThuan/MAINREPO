using SAHL.Core.Data;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public sealed class X2GetInstanceFromParentInstanceSqlStatement : ISqlStatement<long>
    {
        public long InstanceId { get; protected set; }

        public X2GetInstanceFromParentInstanceSqlStatement(long InstanceId)
        {
            this.InstanceId = InstanceId;
        }

        public string GetStatement()
        {
            return @"select ID from x2.x2.instance (nolock) where ParentInstanceID=@InstanceId";
        }
    }
}
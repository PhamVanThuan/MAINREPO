using SAHL.Core.Data;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public sealed class X2GetInstanceFromSourceInstanceSqlStatement : ISqlStatement<long>
    {
        public long InstanceId { get; protected set; }

        public X2GetInstanceFromSourceInstanceSqlStatement(long InstanceId)
        {
            this.InstanceId = InstanceId;
        }

        public string GetStatement()
        {
            return @"select ID from x2.x2.instance (nolock) where SourceInstanceID=@InstanceId";
        }
    }
}
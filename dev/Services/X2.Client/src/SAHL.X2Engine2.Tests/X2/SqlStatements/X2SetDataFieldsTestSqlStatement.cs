using SAHL.Core.Data;
using SAHL.X2Engine2.Tests.X2.Models;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public class X2SetDataFieldsTestSqlStatement : ISqlStatement<X2SetDataFieldsTest>
    {
        public long InstanceId { get; protected set; }

        public X2SetDataFieldsTestSqlStatement(long instanceId)
        {
            InstanceId = instanceId;
        }

        public string GetStatement()
        {
            return string.Format(@"select [ApplicationKey]
      ,[TestBigInt]
      ,[TestBool]
      ,[TestString]
      ,[TestDate]
      ,[TestDecimal]
      ,[TestSingle]
      ,[TestDouble]
from [X2].[X2DATA].[SetDataFieldsTest] (nolock)
where InstanceID = @InstanceId");
        }
    }
}
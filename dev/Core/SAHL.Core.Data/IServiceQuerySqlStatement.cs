using SAHL.Core.Data;

namespace SAHL.Core.Services
{
    public interface IServiceQuerySqlStatement<T, U> : ISqlStatement<U> where T : ISqlServiceQuery<U>
    {
    }
}
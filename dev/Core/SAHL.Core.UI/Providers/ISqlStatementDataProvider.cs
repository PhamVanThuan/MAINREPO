using SAHL.Core.Data;

namespace SAHL.Core.UI.Providers
{
    public interface ISqlStatementDataProvider<T> : ISqlDataProvider, ISqlStatement<T>
    {
    }
}
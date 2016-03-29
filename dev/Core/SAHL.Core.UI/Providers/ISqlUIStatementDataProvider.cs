using SAHL.Core.Data;

namespace SAHL.Core.UI.Providers
{
    public interface ISqlUIStatementDataProvider<T> : ISqlDataProvider
        where T : IDataModel
    {
    }
}
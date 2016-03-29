namespace SAHL.Core.Data.Context
{
    public interface IDbConnectionProviderStorage
    {
        bool HasConnectionProvider { get; }

        IDbConnectionProvider ConnectionProvider { get; }

        void RegisterConnectionProvider(IDbConnectionProvider connectionProvider);

        void UnRegisterConnectionProvider();
    }
}
using System.Data;

namespace SAHL.Core.Data.Context
{
    public interface IDbConnectionProvider
    {
        bool HasUnitOfWorkContexts { get; }

        bool HasRegisteredContexts { get; }

        void RegisterUnitOfWorkContext(string uowContextName);

        void UnregisterUnitOfWorkContext(string uowContextName);

        IDbConnection RegisterContext(string connectionContextName);

        void UnRegisterContext(string connectionContextName);

        IDbConnection GetConnectionForContext(string connectionContextName);
    }
}
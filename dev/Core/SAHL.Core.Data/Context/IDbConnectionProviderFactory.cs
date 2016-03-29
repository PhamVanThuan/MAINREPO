namespace SAHL.Core.Data.Context
{
    public interface IDbConnectionProviderFactory
    {
        IDbConnectionProvider GetNewConnectionProvider();
    }
}
namespace SAHL.Websites.Halo.Shared
{
    public interface IUnitOfWorkExecutor
    {
        bool Execute<T>() where T : IUnitOfWorkAction;
    }
}
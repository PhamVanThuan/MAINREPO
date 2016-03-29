namespace SAHL.Core.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Build();
    }
}
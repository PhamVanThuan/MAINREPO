namespace SAHL.Core.Services
{
    public interface IServiceQueryPaginationHandler
    {
        void ReceivePaginationOptions(int pageSize, int currentPage);
    }
}
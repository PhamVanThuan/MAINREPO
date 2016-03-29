namespace SAHL.Core.Services
{
    public class PaginationQueryParameter : IQueryParameter
    {
        public PaginationQueryParameter()
        {
        }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
    }
}
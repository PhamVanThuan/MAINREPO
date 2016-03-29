using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Builders.Core
{
    public class PagedPart : IPagedPart
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
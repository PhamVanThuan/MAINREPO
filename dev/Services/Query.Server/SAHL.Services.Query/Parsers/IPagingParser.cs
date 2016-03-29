using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers
{
    public interface IPagingParser
    {
        IPagedPart FindPaging(string paging);
    }
}
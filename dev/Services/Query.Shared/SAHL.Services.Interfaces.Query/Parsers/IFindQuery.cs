using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Interfaces.Query.Parsers
{
    public interface IFindQuery
    {
        bool IsValid { get; }
        List<string> Errors { get; set; }
        ILimitPart Limit { get; set; }
        List<string> Fields { get; set; }
        List<string> Includes { get; set; }
        List<IWherePart> Where { get; set; }
        List<IOrderPart> OrderBy { get; set; }
        ISkipPart Skip { get; set; }
        IPagedPart PagedPart { get; set; }
        string FullFilterString { get; set; }
    }

}
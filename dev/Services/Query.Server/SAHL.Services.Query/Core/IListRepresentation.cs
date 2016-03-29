using System.Collections.Generic;
using SAHL.Services.Query.Resources;
using WebApi.Hal;

namespace SAHL.Services.Query.Core
{
    public interface IListRepresentation
    {
        IList<Representation> List { get; }
        IPagingRepresentation _paging { get; set; }
        int? TotalCount { get; set; }
        int? ListCount { get; set; }
    }
}
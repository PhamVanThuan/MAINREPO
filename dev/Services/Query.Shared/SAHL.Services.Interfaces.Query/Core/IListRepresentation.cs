using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Query.Core
{
    public interface IListRepresentation
    {
        IList<Representation> List { get; }
        IPagingRepresentation _paging { get; set; }
        int Count { get; set; }
    }
}
using System.Collections.Generic;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources
{
    public class PagingRepresentation : IPagingRepresentation 
    {

        public PagingRepresentation()
        {
            _links = new List<Link>();    
        }

        public int Count { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public List<Link> _links { get; private set; } 

    }

    public interface IPagingRepresentation
    {
        int Count { get; set; }
        int PageSize { get; set; }
        int CurrentPage { get; set; }
        List<Link> _links { get; }
    }
}
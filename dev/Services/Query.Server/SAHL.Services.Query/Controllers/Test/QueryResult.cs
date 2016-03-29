using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.Test
{
    public class QueryResult
    {
        public Representation Representation { get; set; }

        public object Content { get; set; }
    }
}
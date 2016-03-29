using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers
{
    public class FinanceController : ApiController
    {
        private readonly ILinkResolver linkResolver;
        private readonly IHalSerialiser serialiser;

        public FinanceController(ILinkResolver linkResolver, IHalSerialiser serialiser)
        {
            this.linkResolver = linkResolver;
            this.serialiser = serialiser;
        }

        [RepresentationRoute("/api/finance", typeof(FinanceRepresentation))]
        public HttpResponseMessage Get()
        {
            var representation = new FinanceRepresentation(this.linkResolver);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }
    }
}
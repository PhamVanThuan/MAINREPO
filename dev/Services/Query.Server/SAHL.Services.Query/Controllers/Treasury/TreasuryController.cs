using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers
{
    public class TreasuryController : ApiController
    {
        private readonly ILinkResolver linkResolver;
        private readonly IHalSerialiser serialiser;

        public TreasuryController(ILinkResolver linkResolver, IHalSerialiser serialiser)
        {
            this.linkResolver = linkResolver;
            this.serialiser = serialiser;
        }

        [RepresentationRoute("/api/treasury", typeof(TreasuryRepresentation))]
        public HttpResponseMessage Get()
        {
            var representation = new TreasuryRepresentation(this.linkResolver);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }
    }
}
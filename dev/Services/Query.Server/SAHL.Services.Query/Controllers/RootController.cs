using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers
{
    public class RootController : ApiController
    {
        private readonly ILinkResolver linkResolver;
        private readonly IHalSerialiser serialiser;

        public RootController(ILinkResolver linkResolver, IHalSerialiser serialiser)
        {
            this.linkResolver = linkResolver;
            this.serialiser = serialiser;
        }

        [RepresentationRoute("/api", typeof(RootRepresentation))]
        public HttpResponseMessage Get()
        {
            var representation = new RootRepresentation(this.linkResolver);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }
    }
}

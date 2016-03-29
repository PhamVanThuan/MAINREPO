using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers
{
    public class OrganisationController : ApiController
    {
        private readonly ILinkResolver linkResolver;
        private readonly IHalSerialiser serialiser;

        public OrganisationController(ILinkResolver linkResolver, IHalSerialiser serialiser)
        {
            this.linkResolver = linkResolver;
            this.serialiser = serialiser;
        }

        [RepresentationRoute("/api/organisation", typeof(OrganisationRepresentation))]
        public HttpResponseMessage Get()
        {
            return new OrganisationRepresentation(this.linkResolver).ToHttpResponseMessage(Request, this.serialiser);
        }
    }
}
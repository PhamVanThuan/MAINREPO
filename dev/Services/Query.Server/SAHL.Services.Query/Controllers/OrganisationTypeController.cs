using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.DataManagers.OrganisationStructure;
using SAHL.Services.Query.Resources.OrganisationStructure;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers
{
    public class OrganisationTypeController : ApiController
    {
        private readonly IOrganisationStructureDataManager dataManager;
        private readonly IHalSerialiser serialiser;
        private readonly ILinkResolver linkResolver;

        public OrganisationTypeController(ILinkResolver linkResolver, IOrganisationStructureDataManager dataManager, IHalSerialiser serialiser)
        {
            this.linkResolver = linkResolver;
            this.dataManager = dataManager;
            this.serialiser = serialiser;
        }

        [RepresentationRoute("/api/organisation/types", typeof(OrganisationTypeListRepresentation))]
        public HttpResponseMessage Get()
        {
            var results = this.dataManager.GetOrganisationTypes();

            var items = results.Select(a => new OrganisationTypeRepresentation(linkResolver, a.OrganisationTypeKey, a.Description)).ToList();

            var representation = new OrganisationTypeListRepresentation(linkResolver, items);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }

        [RepresentationRoute("/api/organisation/types/{id}", typeof(OrganisationTypeRepresentation))]
        public HttpResponseMessage Get(int id)
        {
            var result = this.dataManager.GetOrganisationType(id);

            if (result == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            
            var representation = new OrganisationTypeRepresentation(linkResolver, result.OrganisationTypeKey, result.Description);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }
    }
}
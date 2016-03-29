using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Query.DataManagers.OrganisationStructure;
using SAHL.Services.Query.Resources.OrganisationStructure;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers
{
    public class OrganisationStructureController : ApiController
    {
        private readonly IOrganisationStructureDataManager dataManager;
        private readonly ILinkResolver linkResolver;
        private readonly IHalSerialiser serialiser;

        public OrganisationStructureController(ILinkResolver linkResolver, IOrganisationStructureDataManager dataManager, IHalSerialiser serialiser)
        {
            this.linkResolver = linkResolver;
            this.dataManager = dataManager;
            this.serialiser = serialiser;
        }

        [RepresentationRoute("/api/organisation/structure", "OrganisationStructure_root", typeof (OrganisationStructureRepresentation))]
        public HttpResponseMessage Get()
        {
            var queryResults = this.dataManager.GetOrganisationStructureByParentKey(null);

            var representations = queryResults
                .Select(a => new OrganisationStructureRepresentation(
                    linkResolver, a.OrganisationStructureKey, a.Description, a.OrganisationTypeKey, a.OrganisationType))
                .ToList();

            var representation = new OrganisationStructureRepresentation(linkResolver, null, "root", null, null, representations);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }

        [RepresentationRoute("/api/organisation/structure/{id}", typeof (OrganisationStructureRepresentation))]
        public HttpResponseMessage Get(int id)
        {
            var result = this.dataManager.GetOrganisationStructure(id);

            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var children = this.dataManager.GetOrganisationStructureByParentKey(id)
                .Select(a => new OrganisationStructureRepresentation(
                    linkResolver, a.OrganisationStructureKey, a.Description, a.OrganisationTypeKey, a.OrganisationType))
                .ToList();

            var representation = new OrganisationStructureRepresentation(linkResolver, result.OrganisationStructureKey, result.Description,
                result.OrganisationTypeKey, result.OrganisationType, children);

            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }
    }
}

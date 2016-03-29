using System.Net;
using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Resources.Lookup;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers.Lookup
{
    public class LookupsRootController : ApiController
    {

        private ILookupTypesHelper LookupTypesHelper;
        private readonly IHalSerialiser serialiser;
        private ILookupRepresentationHelper LookupRepresentationHelper;

        public LookupsRootController(ILookupRepresentationHelper lookupRepresentationHelper, ILookupTypesHelper lookupTypesHelper, IHalSerialiser serialiser)
        {
            this.LookupTypesHelper = lookupTypesHelper;
            this.serialiser = serialiser;
            this.LookupRepresentationHelper = lookupRepresentationHelper;
        }

        [RepresentationRoute("/api/lookup", typeof(LookupTypeListRepresentation))]
        public HttpResponseMessage Get()
        {
            return LookupRepresentationHelper.GetLookupTypesRepresentation().ToHttpResponseMessage(Request, this.serialiser);
        }

        [RepresentationRoute("/api/lookupType/{id}", typeof(LookupTypeRepresentation))]
        public HttpResponseMessage Get(string id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}
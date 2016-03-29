using System.Net;
using System.Net.Http;
using System.Web.Http;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Resources.Lookup;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers.Lookup
{
    public class LookupController : ApiController
    {
        private readonly IQueryFactory queryFactory;
        private readonly IHalSerialiser serialiser;

        public LookupController(ILookupRepresentationHelper lookupRepresentationHelper,
            ILookupTypesHelper lookupTypesHelper, IQueryFactory queryFactory, IHalSerialiser serialiser)
        {
            this.lookupTypesHelper = lookupTypesHelper;
            this.lookupRepresentationHelper = lookupRepresentationHelper;
            this.queryFactory = queryFactory;
            this.serialiser = serialiser;
        }

        private readonly ILookupTypesHelper lookupTypesHelper;
        private readonly ILookupRepresentationHelper lookupRepresentationHelper;

        [RepresentationRoute("/api/lookup/{lookupType}", typeof (LookupListRepresentation))]
        public HttpResponseMessage Get(string lookUpType)
        {
            //Check if we are dealing with the correct lookup type
            if (!this.lookupTypesHelper.IsValidLookupType(lookUpType))
            {
                return new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            IFindQuery findManyQuery = this.queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
            var representation = this.lookupRepresentationHelper.GetLookupsRepresentation(lookUpType, findManyQuery);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }

        [RepresentationRoute("/api/lookup/{lookupType}/{id}", typeof (LookupRepresentation))]
        public HttpResponseMessage Get(string lookUpType, int id)
        {
            //Check if we are dealing with the correct lookup type
            if (!this.lookupTypesHelper.IsValidLookupType(lookUpType))
            {
                return new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            IFindQuery findManyQuery = this.queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
            var representation = this.lookupRepresentationHelper.GetLookupRepresentation(lookUpType, id, findManyQuery);
            return representation.ToHttpResponseMessage(Request, this.serialiser);
        }

    }

}

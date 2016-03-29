using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.SqlServer.Server;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Builders;
using SAHL.Services.Query.Controllers.Attorney;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Serialiser;
using StructureMap;
using StructureMap.TypeRules;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers
{
    [DoNotRouteParametersInQueryString]
    public class QueryServiceBaseController : ApiController
    {
        protected IQueryServiceDataManager dataManager;
        protected IQueryCoordinator queryCoordinator;
        private readonly IQueryFactory queryFactory;
        private readonly IHalSerialiser halSerialiser;
        private readonly IDataManagerRetriever dataManagerRetriever;

        public QueryServiceBaseController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IHalSerialiser halSerialiser, IDataManagerRetriever dataManagerRetriever)
        {
            this.queryCoordinator = queryCoordinator;
            this.queryFactory = queryFactory;
            this.halSerialiser = halSerialiser;
            this.dataManagerRetriever = dataManagerRetriever;
        }

        protected QueryServiceBaseController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IQueryServiceDataManager dataManager)
        {
            this.queryCoordinator = queryCoordinator;
            this.dataManager = dataManager;
            this.queryFactory = queryFactory;
        }

        [HttpGet]
        public virtual HttpResponseMessage Count()
        {
            var result = GetDataManager();

            if (result == null || result.DataManager == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var count = result.DataManager.GetCount(queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString()));

            var countRepresentation = new CountRepresentation();
            countRepresentation.Count = count;
            countRepresentation.Rel = "count";
            countRepresentation.Href = "not/yet/implemented";

            var json = this.halSerialiser.Serialise(countRepresentation);

            return WrapHttpResponseMessage(json);
        }

        [HttpGet]
        public virtual HttpResponseMessage Get()
        {
            return GetFromCoordinator();
        }

        [HttpGet]
        public virtual HttpResponseMessage Get(string id)
        {
            return GetFromCoordinator(id);
        }

        [HttpPost]
        public virtual HttpResponseMessage Post()
        {
            return CreateHttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        [HttpPut]
        public virtual HttpResponseMessage Put()
        {
            return CreateHttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        [HttpDelete]
        public virtual HttpResponseMessage Delete()
        {
            return CreateHttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        [HttpHead]
        public virtual HttpResponseMessage Head()
        {
            return CreateHttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        [HttpPatch]
        public virtual HttpResponseMessage Patch()
        {
            return CreateHttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        [HttpOptions]
        public virtual HttpResponseMessage Options()
        {
            return CreateHttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        protected virtual HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode httpStatusCode, string content)
        {
            return CreateHttpResponseMessage(httpStatusCode, new StringContent(content));
        }

        protected virtual HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode httpStatusCode, HttpContent content = null)
        {
            var message = new HttpResponseMessage(httpStatusCode);
            message.Content = content;
            return message;
        }

        protected HttpResponseMessage Execute(IFindQuery findQuery)
        {
            string response = this.queryCoordinator.Execute(findQuery, () => dataManager.GetList(findQuery), () => dataManager.GetCount(findQuery));
            return WrapHttpResponseMessage(response);
        }

        protected HttpResponseMessage Execute(IFindQuery findQuery, string id)
        {
            string response = this.queryCoordinator.Execute(findQuery, () => dataManager.GetById(id, findQuery));
            return WrapHttpResponseMessage(response);
        }

        protected HttpResponseMessage ExecuteOne(IFindQuery findQuery)
        {
            string response = this.queryCoordinator.Execute(findQuery, () => dataManager.GetOne(findQuery));
            return WrapHttpResponseMessage(response);
        }

        protected HttpResponseMessage ExecuteUrl(List<IRelationshipDefinition> relationships, string relationshipName, IFindQuery findQuery)
        {
            IRelationshipDefinition relationship = relationships.FirstOrDefault(x => x.RelatedEntity.ToLower() == relationshipName.ToLower());

            if (relationship != null)
            {
                List<IRelationshipDefinition> relationshipToResolve = new List<IRelationshipDefinition>() { relationship };
                var linkUrls = queryCoordinator.BuildUrlsForLinks_1(relationshipToResolve);
                queryCoordinator.FetchResultsForLinks(linkUrls);
                return WrapHttpResponseMessage(linkUrls.First().JsonResult);
            }

            return CreateNoContentMessage();
        }
        
        protected WherePart CreateWherePart(string value, string field, string parameter)
        {
            return new WherePart()
            {
                ClauseOperator = "and",
                Field = field,
                ParameterName = parameter,
                Operator = "=",
                Value = value
            };
        }
        
        private HttpResponseMessage GetFromCoordinator(string id = null)
        {

            DataManagerQueryResult result = PrepareDataManagerQuery(id);

            if (result.DataManager == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            
            var findQuery = GetFindQueryFromRequest();
            if (!findQuery.IsValid)
            {
                return PrepareQueryError(findQuery);
            }
            
            var response = this.queryCoordinator.Execute(findQuery, result.DataManager, result.Id);
            return WrapHttpResponseMessage(response);

        }

        private HttpResponseMessage PrepareQueryError(IFindQuery findQuery)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            httpResponse.Content = new StringContent(findQuery.Errors[0]);
            return httpResponse;
        }

        private DataManagerQueryResult PrepareDataManagerQuery(string id)
        {
            DataManagerQueryResult result;
            if (this.dataManager == null)
            {
                result = GetDataManager();
            }
            else
            {
                result = new DataManagerQueryResult(this.dataManager, null, id, null);
            }
            return result;
        }

        private IFindQuery GetFindQueryFromRequest()
        {
            return queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
        }

        private DataManagerQueryResult GetDataManager()
        {
            var routeData = this.Request.GetRouteData();
            var routeTemplate = routeData.Route.RouteTemplate;
            var routeValues = routeData.Values;

            return GetDataManagerFromChain(routeTemplate, routeValues);
        }

        private DataManagerQueryResult GetDataManagerFromChain(string routeTemplate, IDictionary<string, object> routeValues)
        {
            return this.dataManagerRetriever.Get(routeTemplate, routeValues);
        }

        private bool HasValidResponse(string response)
        {
            return response != null;
        }

        protected HttpResponseMessage CreateNoContentMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        private HttpResponseMessage WrapHttpResponseMessage(string response)
        {
            return HasValidResponse(response) 
                ? response.ToHttpResponseMessage() 
                : CreateNoContentMessage();
        }
    }
}

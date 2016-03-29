using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omu.ValueInjecter;
using SAHL.Core;
using SAHL.Core.Identity;
using SAHL.Core.Web.Identity;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Builders;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Url;
using StructureMap;
using WebApi.Hal;

namespace SAHL.Services.Query.Coordinators
{
    public class QueryCoordinator : IQueryCoordinator
    {
        //TODO: REDUCE DEPENDENCIES, should refactor this out into components, builders perhaps?
        private readonly IAbsoluteUrlBuilder absoluteUrlBuilder;
        private readonly IHalSerialiser halSerialiser;
        private readonly HttpHostContext hostContext;
        private readonly IIncludeRelationshipCoordinator includeRelationshipCoordinator;
        private readonly ILinkResolver linkResolver;
        private readonly IPagingCoordinator pagingCoordinator;
        private readonly IQueryFactory queryFactory;
        private readonly IRepresentationTemplateCache representationTemplateCache;
        private readonly IUrlParameterSubstituter urlParameterSubstituter;
        private readonly IValueInjecter valueInjecter;

        public QueryCoordinator(IHostContext hostContext, IAbsoluteUrlBuilder absoluteUrlBuilder,
            IRepresentationTemplateCache representationTemplateCache, IHalSerialiser halSerialiser, IValueInjecter valueInjecter,
            IIncludeRelationshipCoordinator includeRelationshipCoordinator, ILinkResolver linkResolver,
            IUrlParameterSubstituter urlParameterSubstituter, IPagingCoordinator pagingCoordinator, IQueryFactory queryFactory)
        {
            //TODO: need to make an interface specifically for IHttpHostContext, however it may interfere with the existing HttpHostContext class
            this.hostContext = hostContext as HttpHostContext;
            if (this.hostContext == null)
            {
                throw new ArgumentException("Cannot instantiate the QueryCoordinator with a IHostContext that is not an HttpHostContext.");
            }
            this.absoluteUrlBuilder = absoluteUrlBuilder;
            this.representationTemplateCache = representationTemplateCache;
            this.halSerialiser = halSerialiser;
            this.valueInjecter = valueInjecter;
            this.includeRelationshipCoordinator = includeRelationshipCoordinator;
            this.linkResolver = linkResolver;
            this.urlParameterSubstituter = urlParameterSubstituter;
            this.pagingCoordinator = pagingCoordinator;
            this.queryFactory = queryFactory;
        }

        public virtual string Execute(NameValueCollection queryString, IQueryServiceDataManager dataManager, string id = null)
        {
            var query = queryFactory.CreateFindManyQuery(queryString);

            return Execute(query, dataManager, id);
        }

        public virtual string Execute(IFindQuery query, IQueryServiceDataManager dataManager, string id)
        {
            //TODO: temporary fix to resolve lookups, as lookups currently aren't IQueryServiceDataManager compatible
            var dataManagerType = dataManager.GetType();
            if (dataManagerType.IsGenericType)
            {
                var dataModelType = dataManagerType.GenericTypeArguments[0];
                if (typeof (LookupDataModel).IsAssignableFrom(dataModelType))
                {
                    var lookupDataManager = ObjectFactory.GetInstance<ILookupRepresentationHelper>();
                    var tokens = HttpContext.Current.Request.Url.ToString().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    var type = tokens[tokens.Length - 1]; //remove query string
                    var lookupRepresentation = lookupDataManager.GetLookupRepresentation(type, int.Parse(id), this.queryFactory.CreateEmptyFindQuery());

                    return halSerialiser.Serialise(lookupRepresentation);
                }
            }

            return id == null
                ? Execute(query, () => dataManager.GetList(query), () => dataManager.GetCount(query))
                : Execute(query, () => dataManager.GetById(id, query));
        }

        public virtual string Execute(IFindQuery query, Func<IEnumerable<IQueryDataModel>> funcThatRetreivesDataModels,
            Func<int> funcThatReturnsTotalCountForPaging)
        {
            var models = funcThatRetreivesDataModels();

            if (models == null)
            {
                return string.Empty;
            }

            var representation = CreateRepresentationFromModel(models);

            ApplyPaging(query, funcThatReturnsTotalCountForPaging, representation, models.Count());

            var actions = models
                .Select(a => new RepresentationSerialisationAction(a))
                .ToList();

            RetrieveListQueryResults(query, actions);

            var jsonString = this.halSerialiser.Serialise(representation);

            var json = JObject.Parse(jsonString);

            var linksArray = new JArray();
            var embeddedItemArray = new JArray();
            SetLinksInParentRepresentation(actions, embeddedItemArray, linksArray);

            var relationshipName = "items";
            if (actions.Any())
            {
                var dataModelType = actions.First().DataModel.GetType();
                relationshipName = representationTemplateCache.Get(dataModelType).Rel;
            }

            AddJsonObjectWithProperties(json, "_links", new JProperty(relationshipName, linksArray).ToSingleItemEnumerable());

            AddNewJsonPropertyWithValue(json, "_embedded", CreateProperty(relationshipName, embeddedItemArray).ToSingleItemEnumerable());

            SetSelfLink(json, representation.GetType(), false);

            return FormattedJson(json);
        }

        public virtual string Execute(IFindQuery query, Func<IQueryDataModel> functionThatRetrievesADataModel)
        {
            return Execute(query, functionThatRetrievesADataModel());
        }

        protected virtual string Execute(IFindQuery query, IQueryDataModel model)
        {
            return Execute(query, model, false);
        }

        protected virtual string Execute(IFindQuery query, IQueryDataModel model, bool isChildOfList)
        {
            if (model == null)
            {
                return string.Empty;
            }

            var representation = CreateRepresentationFromModel(model);
            
            //TODO: supply the accept header, might need to modify IHostContext to retrieve it
            var jsonString = this.halSerialiser.Serialise(representation);

            var json = JObject.Parse(jsonString);

            ApplyRelationships(query, model, json);

            SetSelfLink(json, representation.GetType(), isChildOfList);

            return FormattedJson(json);
        }

        private void SetSelfLink(JObject json, Type representationType, bool isChildOfList)
        {
            return;

            if (!HasSelfLink(json))
            {
                return;
            }
            var newSelfLink = this.linkResolver.GetLink(representationType, isSelf: true);
            if (newSelfLink == null)
            {
                return;
            }

            
            //should never be null, as HasSelfLink ensures value is present
            var hrefProperty = json["_links"]["self"]["href"] as JValue;
            if (isChildOfList)
            {
                hrefProperty.Value = newSelfLink.Href + "/" + json["id"];
                SetLinksOnChildProperties(hrefProperty.Value.ToString(), json["_links"]);
            }
            else
            {
                hrefProperty.Value = newSelfLink.Href;
            }
        }

        private bool HasSelfLink(JObject json)
        {
            var links = json["_links"];
            if (links == null)
            {
                return false;
            }
            var selfLink = links["self"];
            if (selfLink == null)
            {
                return false;
            }
            if (selfLink["href"] == null)
            {
                return false;
            }
            return selfLink["href"] as JValue != null;
        }

        private static void SetLinksOnChildProperties(string url, JToken links)
        {
            foreach (var item in links.Children<JProperty>())
            {
                if (item.Name.Equals("self", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                var link = item.Value["href"] as JValue;
                if (link == null)
                {
                    continue;
                }
                link.Value = url + "/" + item.Name.ToLower();
            }
        }

        private void ApplyRelationships(IFindQuery query, IQueryDataModel model, JObject json)
        {
            if (!ShouldPerformLinkInclude(model))
            {
                return;
            }

            var linkUrls = BuildUrlsForLinks(model.Relationships);

            AddLinksToJson(json, linkUrls);

            if (!ShouldPerformInclude(query))
            {
                return;
            }

            var linkUrlsToInclude = linkUrls.Where(a => query.Includes.Any(b => b.Equals(a.Relationship, StringComparison.OrdinalIgnoreCase)));

            if (!linkUrlsToInclude.Any())
            {
                return;
            }

            FetchResultsForLinks(linkUrlsToInclude);
            AddEmbeddedToJson(json, linkUrlsToInclude);
        }

        private bool ShouldPerformInclude(IFindQuery query)
        {
            return query != null
                && query.Includes != null
                && query.Includes.Any();
        }

        private bool ShouldPerformLinkInclude(IQueryDataModel model)
        {
            return model.Relationships != null
                   && model.Relationships.Any();
        }

        private void ApplyPaging(IFindQuery query, Func<int> funcThatReturnsTotalCountForPaging, Representation representation, int queryRecordCount)
        {
            if (this.pagingCoordinator == null)
            {
                return;
            }
            this.pagingCoordinator.ApplyPaging((IListRepresentation) representation, query, funcThatReturnsTotalCountForPaging,
                representation.GetType(), queryRecordCount);
        }

        private void SetLinksInParentRepresentation(List<RepresentationSerialisationAction> actions, JArray embeddedItemArray,
            JArray linksArray)
        {
            foreach (var item in actions)
            {
                var result = item.SerialisationResult;
                embeddedItemArray.Add(result);

                var href = result["_links"]["self"];
                if (href == null)
                {
                    continue;
                }
                var hrefCopy = new JObject(href.First);
                linksArray.Add(hrefCopy);
            }
        }

        private void RetrieveListQueryResults(IFindQuery query, List<RepresentationSerialisationAction> actions)
        {
            var currentContext = HttpContext.Current;
            var contextLock = new object();
            Parallel.ForEach(actions, CoreGlobals.DefaultParallelOptions, item =>
            {
                SetHttpContext(contextLock, currentContext);

                var jsonResult = this.Execute(query, item.DataModel, isChildOfList: true);
                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return;
                }
                item.SerialisationResult = JToken.Parse(jsonResult);
            });
        }

        private static void SetHttpContext(object contextLock, HttpContext currentContext)
        {
            //have no way of knowing which thread we're running on
            //so we set the HttpContext of the thread to the HttpContext of the calling thread

            //standard double-checked locking
            if (HttpContext.Current != null)
            {
                return;
            }
            lock (contextLock)
            {
                if (HttpContext.Current != null)
                {
                    return;
                }
                //no need for memory barrier because currentContext will have already been constructed
                HttpContext.Current = currentContext;
            }
        }

        public virtual void FetchResultsForLinks(IEnumerable<LinkQuery> linkUrls)
        {
            this.includeRelationshipCoordinator.Fetch(linkUrls);
        }

        protected virtual void AddEmbeddedToJson(JObject json, IEnumerable<LinkQuery> linkUrls)
        {
            var embeddedPropertyValue = linkUrls.Select(a => CreateProperty(a.Relationship, a.JsonResult));
            AddJsonObjectWithProperties(json, "_embedded", embeddedPropertyValue);
        }

        protected virtual JProperty CreateProperty(string relationship, string json)
        {
            if (json != string.Empty)
            {
                return CreateProperty(relationship, JToken.Parse(json));
            }

            return new JProperty(relationship, JToken.Parse("{'error': 'No Valid Data'}"));
        }

        protected virtual JProperty CreateProperty(string relationship, object token)
        {
            return new JProperty(relationship.ToCamelCase(), token);
        }

        protected virtual void AddLinksToJson(JObject json, IEnumerable<LinkQuery> linkUrls)
        {
            var linksToAdd = linkUrls.Select(a =>
            {
                var propertyValue = new JObject();
                var hrefProperty = new JProperty("href", a.AbsolutePath);
                propertyValue.Add(hrefProperty);
                if (a.IsTemplatedUrl())
                {
                    var isTemplatedProperty = new JProperty("templated", true);
                    propertyValue.Add(isTemplatedProperty);
                }
                var property = new JProperty(a.Relationship.ToCamelCase(), propertyValue);
                return property;
            });

            AddJsonObjectWithProperties(json, "_links", linksToAdd);
        }

        protected virtual void AddJsonObjectWithProperties(JObject jsonObjectWithProperty, string propertyNameToAddTo,
            IEnumerable<JProperty> listOfChildrenPropertiesToAdd)
        {
            var existingProperty = jsonObjectWithProperty[propertyNameToAddTo];
            if (existingProperty == null)
            {
                AddNewJsonPropertyWithValue(jsonObjectWithProperty, propertyNameToAddTo, listOfChildrenPropertiesToAdd);
            }
            else
            {
                CombineJsonProperties(jsonObjectWithProperty, propertyNameToAddTo, listOfChildrenPropertiesToAdd, existingProperty);
            }
        }

        protected virtual void CombineJsonProperties(JObject jsonObjectWithProperty, string propertyNameToAddTo,
            IEnumerable<JProperty> listOfChildrenPropertiesToAdd, JToken linksProperty)
        {
            var linksPropertyAsObject = new JObject();
            //keep the existing properties
            foreach (var item in linksProperty)
            {
                linksPropertyAsObject.Add(item);
            }
            //add the new ones
            foreach (var item in listOfChildrenPropertiesToAdd)
            {
                linksPropertyAsObject.Add(item);
            }
            jsonObjectWithProperty[propertyNameToAddTo] = linksPropertyAsObject;
        }

        protected virtual void AddNewJsonPropertyWithValue(JObject jsonObjectWithProperty, string propertyNameToAddTo,
            IEnumerable<JProperty> listOfChildrenPropertiesToAdd)
        {
            jsonObjectWithProperty.Add(propertyNameToAddTo, new JObject(listOfChildrenPropertiesToAdd));
        }

        protected virtual string FormattedJson(JObject json)
        {
            return json.ToString(Formatting.None);
        }

        protected virtual Representation CreateRepresentationFromModel<T>(IEnumerable<T> models) where T : IQueryDataModel
        {

            var itemType = models.GetType().GetGenericArguments().First();

            var enumerableType = models
                .GetType()
                .GetInterfaces()
                .SingleOrDefault(a => a.IsConstructedGenericType
                    && a.GenericTypeArguments.Length == 1
                    && a.GenericTypeArguments.Single().TypeHandle.Equals(itemType.TypeHandle)
                    && a.Name.StartsWith("IEnumerable"))
                ;

            return CreateRepresentationFromModel(enumerableType, models);
        }

        protected virtual Representation CreateRepresentationFromModel(IQueryDataModel model)
        {
            return CreateRepresentationFromModel(model.GetType(), model);
        }

        protected virtual Representation CreateRepresentationFromModel(Type modelType, object model)
        {
            var representation = this.representationTemplateCache.Get(modelType);
            this.valueInjecter.Inject(representation, model);
            return representation;
        }

        public virtual IEnumerable<LinkQuery> BuildUrlsForLinks(IEnumerable<IRelationshipDefinition> relationships)
        {
            var queries = new List<LinkQuery>();
            var currentUrl = this.hostContext.GetCurrentRequestUrl();
            var applicationPath = this.hostContext.GetApplicationPath();
            foreach (var item in relationships)
            {
                var includeRelationshipQuery = BuildLinkQuery(item, currentUrl, applicationPath.ToLower());
                queries.Add(includeRelationshipQuery);
            }
            return queries;
        }
        
        public IEnumerable<LinkQuery> BuildUrlsForLinks_1(IEnumerable<IRelationshipDefinition> relationships)
        {
            var queries = new List<LinkQuery>();
            var currentUrl = this.hostContext.GetCurrentRequestUrl();
            var applicationPath = this.hostContext.GetApplicationPath();
            foreach (var item in relationships)
            {
                var includeRelationshipQuery = BuildLinkQuery(item, currentUrl, applicationPath);
                queries.Add(includeRelationshipQuery);
            }
            return queries;
        }

        protected virtual LinkQuery BuildLinkQuery(IRelationshipDefinition item, Uri currentUrl)
        {
            var currentRelativeUrl = currentUrl.AbsolutePath + "/" + item.RelatedEntity.ToLower();
            var absoluteUrl = this.absoluteUrlBuilder.BuildUrl(currentRelativeUrl, currentUrl);
            return new LinkQuery(item.RelatedEntity, currentRelativeUrl, absoluteUrl, currentRelativeUrl);
        }

        protected virtual LinkQuery BuildLinkQuery(IRelationshipDefinition item, Uri currentUrl, string applicationPath)
        {
            var representationCache = this.representationTemplateCache.Get(item.DataModelType);
            if (representationCache == null)
            {
                var message = string.Format("Could not find a representation template instance for {0}", item.DataModelType.Name);
                throw new InvalidOperationException(message);
            }

            var relatedField = item.RelatedFields.FirstOrDefault();
            if (relatedField == null)
            {
                var message = string.Format("Could not find any related fields on {0}", item.DataModelType.Name);
                throw new InvalidOperationException(message);
            }

            var link = this.linkResolver.GetLink(representationCache.GetType());
            var relativeUrl = link.Href;
            var relationship = item.RelatedEntity;

            var relativeUrlWithParameters = BuildUrlWithParameters(item, relativeUrl, relatedField);

            var absolutePath = this.absoluteUrlBuilder.BuildPath(relativeUrlWithParameters, applicationPath);
            var absoluteUrl = this.absoluteUrlBuilder.BuildUrl(absolutePath, currentUrl);

            return new LinkQuery(relationship, relativeUrlWithParameters, absoluteUrl, absolutePath);
        }

        protected virtual string BuildUrlWithParameters(IRelationshipDefinition item, string relativeUrl, IRelatedField relatedField)
        {
            bool isTemplated = IsTemplated(relativeUrl);
            var replacements = GetKeywordReplacements(item, relatedField, isTemplated);

            var url = this.urlParameterSubstituter.Replace(relativeUrl, replacements);

            if (item.RelationshipType != RelationshipType.OneToMany || (isTemplated && item.RelationshipType == RelationshipType.OneToMany))
            {
                return url;
            }

            var builder = new WhereBuilder(); //TODO: inject?
            var whereClause = builder.BuildWhereFilter(item);

            return string.IsNullOrWhiteSpace(whereClause) ? url : url + "?" + whereClause;
        }

        private bool IsTemplated(string relativeUrl)
        {
            return relativeUrl.Contains("{");
        }

        protected virtual List<KeyValuePair<string, string>> GetKeywordReplacements(IRelationshipDefinition item,
            IRelatedField relatedField, bool isTemplated)
        {
            //TODO: refactor this into a action dictionary, especially if RelationshipType grows
            var replacements = new List<KeyValuePair<string, string>>();
            const string idPlaceholder = "{id}";
            const string lookupTypePlaceholder = "{lookupType}";
            switch (item.RelationshipType)
            {
                case RelationshipType.OneToOne:
                case RelationshipType.ManyToOne:
                    replacements.Add(new KeyValuePair<string, string>(idPlaceholder, CleanParameterValue(idPlaceholder, relatedField.Value)));
                    break;
                case RelationshipType.OneToOneLookup:
                    replacements.Add(new KeyValuePair<string, string>(lookupTypePlaceholder,
                        CleanParameterValue(lookupTypePlaceholder, item.RelatedEntity)));
                    replacements.Add(new KeyValuePair<string, string>(idPlaceholder, CleanParameterValue(idPlaceholder, relatedField.Value)));
                    break;
                case RelationshipType.ManyToOneLookup:
                    //1 replacement for lookupType
                    replacements.Add(new KeyValuePair<string, string>(lookupTypePlaceholder,
                        CleanParameterValue(lookupTypePlaceholder, item.RelatedEntity)));
                    //also needs where clause
                    throw new NotImplementedException();
                case RelationshipType.OneToMany:
                case RelationshipType.OneToManyWhere:
                    //could have replacement values, but could also use a where clause it deponds on weathe
                    if (isTemplated)
                    {
                        string placeHolder = "{" + relatedField.RelatedKey + "}";
                        replacements.Add(new KeyValuePair<string, string>(placeHolder, CleanParameterValue(placeHolder, relatedField.Value)));
                    }
                    break;

                default:
                    //no replacements
                    break;
            }
            return replacements;
        }

        protected virtual string CleanParameterValue(string key, string value)
        {
            return string.IsNullOrWhiteSpace(value) ? key : value;
        }
    }
}

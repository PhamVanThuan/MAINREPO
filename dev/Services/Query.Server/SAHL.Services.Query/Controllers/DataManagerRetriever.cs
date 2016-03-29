using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Controllers
{
    public class DataManagerRetriever : IDataManagerRetriever
    {
        private readonly IDataManagerCollection dataManagerCollection;
        private readonly Dictionary<RelationshipType, Func<string, Relationship, IntermediateQuery>> intermediateQueryFunctionFactory;
        private readonly IQueryFactory queryFactory;
        private readonly IRepresentationDataModelMapCollection representationDataModelMapCollection;
        private readonly IRouteMetadataCollection routeMetadataCollection;

        public DataManagerRetriever(IRouteMetadataCollection routeMetadataCollection,
            IRepresentationDataModelMapCollection representationDataModelMapCollection, IDataManagerCollection dataManagerCollection,
            IQueryFactory queryFactory)
        {
            this.routeMetadataCollection = routeMetadataCollection;
            this.representationDataModelMapCollection = representationDataModelMapCollection;
            this.dataManagerCollection = dataManagerCollection;
            this.queryFactory = queryFactory;

            //TODO: this can be made a singleton
            this.intermediateQueryFunctionFactory = new Dictionary<RelationshipType, Func<string, Relationship, IntermediateQuery>>
            {
                { RelationshipType.OneToOne, SingleFieldMapIdValue },
                { RelationshipType.OneToOneLookup, SingleFieldMapIdValue },
                { RelationshipType.ManyToOne, SingleFieldMapIdValue },
                { RelationshipType.OneToMany, SingleFieldMapIdValue },
                { RelationshipType.OneToManyWhere, SingleFieldConstructWhereClause },
            };
        }

        public DataManagerQueryResult Get(string routeTemplate, IDictionary<string, object> routeValues)
        {
            var tokens = routeTemplate.Split('/');

            var dataManagerChain = new Stack<DataManagerQueryResult>();

            for (var i = 0; i < tokens.Length; i++)
            {
                var partialRouteTemplate = ConstructUrlFromTokens(tokens, i);

                TryProcessPartialRouteTemplate(routeValues, partialRouteTemplate, tokens, i, dataManagerChain);
            }

            return dataManagerChain.Count > 0 ? dataManagerChain.Peek() : null;
        }

        private static string ConstructUrlFromTokens(string[] tokens, int indexFrom)
        {
            return string.Join("/", tokens, 0, indexFrom + 1);
        }

        private void TryProcessPartialRouteTemplate(IDictionary<string, object> routeValues, string partialRouteTemplate
            , string[] tokens, int tokenIndex, Stack<DataManagerQueryResult> dataManagerChain)
        {
            var representationType = GetRepresentationType(partialRouteTemplate);

            if (representationType == null)
            {
                //if no representation for this url, then we try next token
                return;
            }

            var mappedType = GetMappedType(representationType);

            if (mappedType == null)
            {
                //if no data model for this representation, try next token
                return;
            }

            var dataModelType = GetDataModelType(mappedType);

            var dataManagerToUse = GetDataManager(dataModelType);

            if (IsListTypeAndMoreTokensArePresentInUrl(tokens, tokenIndex, representationType))
            {
                return;
            }

            ProcessPartialRouteTemplate(routeValues, partialRouteTemplate, tokens, tokenIndex, dataManagerChain, dataManagerToUse);
        }

        private void ProcessPartialRouteTemplate(IDictionary<string, object> routeValues, string partialRouteTemplate, string[] tokens,
            int tokenIndex, Stack<DataManagerQueryResult> dataManagerChain, IQueryServiceDataManager dataManagerToUse)
        {
            var query = ConstructIntermediateQuery(routeValues, partialRouteTemplate, tokens, tokenIndex, dataManagerChain);

            DataManagerQueryResult result;
            if (AreMoreTokensToProcess(tokenIndex, tokens))
            {
                var emptyQuery = this.queryFactory.CreateEmptyFindQuery();
                var model = dataManagerToUse.GetById(query.IdValue, emptyQuery); //performing intermediate query
                result = new DataManagerQueryResult(dataManagerToUse, model, query.IdValue, emptyQuery);
            }
            else
            {
                result = new DataManagerQueryResult(dataManagerToUse, null, query.IdValue, query.FindQuery);
            }
            dataManagerChain.Push(result);
        }

        private IntermediateQuery ConstructIntermediateQuery(IDictionary<string, object> routeValues, string partialRouteTemplate, 
            string[] tokens, int tokenIndex, Stack<DataManagerQueryResult> dataManagerChain)
        {
            var idValue = GetIdValue(routeValues, partialRouteTemplate);

            if (!AreDataManagersInChain(dataManagerChain))
            {
                return new IntermediateQuery(idValue, this.queryFactory.CreateEmptyFindQuery());
            }

            var lastDataModelInChain = dataManagerChain.Peek().DataModel;

            var relationship = GetRelationship(tokens, tokenIndex, lastDataModelInChain);

            var create = TryGetQueryCreationFunction(relationship);
            return create(idValue, relationship);
        }

        private Func<string, Relationship, IntermediateQuery> TryGetQueryCreationFunction(Relationship relationship)
        {
            Func<string, Relationship, IntermediateQuery> function;
            var retrieved = this.intermediateQueryFunctionFactory.TryGetValue(relationship.RelationshipDefinition.RelationshipType, out function);

            if (!retrieved)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "No definition for relationship resolution was found for a {0} relationship",
                        relationship.RelationshipDefinition.RelationshipType));
            }

            return function;
        }

        private IntermediateQuery SingleFieldMapIdValue(string idValue, Relationship relationship)
        {
            return new IntermediateQuery(
                relationship.RelationshipDefinition.RelatedFields.Single().Value,
                this.queryFactory.CreateEmptyFindQuery()
                );
        }

        private IntermediateQuery SingleFieldConstructWhereClause(string idValue, Relationship relationship)
        {
            if (relationship.IsTailARelationship)
            {
                this.queryFactory.CreateEmptyFindQuery()
                    .Where
                    .Add(ConstructWherePart(relationship.RelationshipDefinition.RelatedFields.Single()));
            }
            return new IntermediateQuery(idValue, this.queryFactory.CreateEmptyFindQuery());
        }

        private WherePart ConstructWherePart(IRelatedField relationshipKey)
        {
            var wherePart = new WherePart();
            wherePart.Field = relationshipKey.RelatedKey;
            wherePart.Value = relationshipKey.Value;
            wherePart.Operator = "=";
            wherePart.ClauseOperator = "and";
            return wherePart;
        }

        private static Relationship GetRelationship(string[] tokens, int tokenIndex, IQueryDataModel lastDataModelInChain)
        {
            var relationshipFromPreviousEntityToThisOne = FindRelationshipOnDataModel(lastDataModelInChain, tokens[tokenIndex]);

            //use found relationship if one is present, otherwise try and find relationship on previous token
            return relationshipFromPreviousEntityToThisOne == null
                ? new Relationship(FindRelationshipOnDataModel(lastDataModelInChain, tokens[tokenIndex - 1]), false)
                : new Relationship(relationshipFromPreviousEntityToThisOne, true);
        }

        private static IRelationshipDefinition FindRelationshipOnDataModel(IQueryDataModel lastDataModelInChain, string relationshipName)
        {
            return lastDataModelInChain.Relationships
                .SingleOrDefault(a => a.RelatedEntity.Equals(relationshipName, StringComparison.OrdinalIgnoreCase));
        }

        private static bool AreDataManagersInChain(Stack<DataManagerQueryResult> dataManagerChain)
        {
            return dataManagerChain.Count > 0;
        }

        private static string GetIdValue(IDictionary<string, object> routeValues, string partialRouteTemplate)
        {
            var lastIndexOfOpen = partialRouteTemplate.LastIndexOf("{");
            var lastIndexOfClose = partialRouteTemplate.LastIndexOf("}");

            if (lastIndexOfOpen >= lastIndexOfClose)
            {
                //null means there is no placeholder for the id value
                return null;
            }

            var key = partialRouteTemplate.Substring(lastIndexOfOpen + 1, lastIndexOfClose - lastIndexOfOpen - 1);
            object value;
            routeValues.TryGetValue(key, out value);

            return value == null
                ? string.Empty
                : value.ToString();
        }

        private static bool IsListTypeAndMoreTokensArePresentInUrl(string[] tokens, int i, Type representationType)
        {
            return typeof (IListRepresentation).IsAssignableFrom(representationType) && AreMoreTokensToProcess(i, tokens);
        }

        private static bool AreMoreTokensToProcess(int i, string[] tokens)
        {
            return i < tokens.Length - 1;
        }

        private IQueryServiceDataManager GetDataManager(Type dataModelType)
        {
            return this.dataManagerCollection.Get(dataModelType);
        }

        private static Type GetDataModelType(Type dataModelType)
        {
            return dataModelType.IsGenericType && dataModelType.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                ? dataModelType.GenericTypeArguments[0]
                : dataModelType;
        }

        private Type GetMappedType(Type representationType)
        {
            return this.representationDataModelMapCollection.Get(representationType);
        }

        private Type GetRepresentationType(string partialRouteTemplate)
        {
            return this.routeMetadataCollection.Get(partialRouteTemplate);
        }

        public class IntermediateQuery
        {
            public IntermediateQuery(string idValue, IFindQuery findQuery)
            {
                this.FindQuery = findQuery;
                this.IdValue = idValue;
            }

            public IFindQuery FindQuery { get; private set; }
            public string IdValue { get; private set; }
        }

        public class Relationship
        {
            public Relationship(IRelationshipDefinition relationshipDefinition, bool isTailARelationship)
            {
                this.IsTailARelationship = isTailARelationship;
                this.RelationshipDefinition = relationshipDefinition;
            }

            public bool IsTailARelationship { get; private set; }
            public IRelationshipDefinition RelationshipDefinition { get; private set; }
        }
    }
}

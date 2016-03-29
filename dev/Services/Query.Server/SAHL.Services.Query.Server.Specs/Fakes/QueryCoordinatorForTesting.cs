using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Omu.ValueInjecter;
using SAHL.Core.Identity;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Url;

namespace SAHL.Services.Query.Server.Specs
{
    public class QueryCoordinatorForTesting : QueryCoordinator
    {
        private readonly IQueryFactory queryFactory;
        //Name of method, *Calls, -> Name of parameter, value of parameter
        public List<KeyValuePair<string, List<KeyValuePair<string, object>>>> MethodCallParameters;

        public QueryCoordinatorForTesting(IHostContext hostContext, IAbsoluteUrlBuilder absoluteUrlBuilder
            , IRepresentationTemplateCache representationTemplateCache, IHalSerialiser halSerialiser, IValueInjecter valueInjecter
            , IIncludeRelationshipCoordinator includeRelationshipCoordinator, ILinkResolver linkResolver
            , IUrlParameterSubstituter urlParameterSubstituter, IPagingCoordinator pagingCoordinator, IQueryFactory queryFactory)
            : base(hostContext, absoluteUrlBuilder, representationTemplateCache, halSerialiser, valueInjecter
            , includeRelationshipCoordinator, linkResolver, urlParameterSubstituter, pagingCoordinator, queryFactory)
        {
            this.queryFactory = queryFactory;
            MethodCallParameters = new List<KeyValuePair<string, List<KeyValuePair<string, object>>>>();
        }

        protected override string Execute(Services.Interfaces.Query.Parsers.IFindQuery query
            , Services.Interfaces.Query.Models.IQueryDataModel model
            , bool isChildOfList)
        {
            var parameters = new List<KeyValuePair<string, object>>();
            parameters.Add(new KeyValuePair<string, object>("query", query));
            parameters.Add(new KeyValuePair<string, object>("model", model));
            parameters.Add(new KeyValuePair<string, object>("isChildOfList", isChildOfList));
            MethodCallParameters.Add(new KeyValuePair<string, List<KeyValuePair<string, object>>>("Execute", parameters));

            return base.Execute(query, model, isChildOfList);
        }
    }
}

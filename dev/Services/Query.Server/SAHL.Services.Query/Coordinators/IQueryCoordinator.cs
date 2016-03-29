using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Interfaces;

namespace SAHL.Services.Query.Coordinators
{
    public interface IQueryCoordinator
    {
        [Obsolete("Use Execute(IFindQuery, IQueryServiceDataManager, string id = null) overload")]
        string Execute(IFindQuery query, Func<IQueryDataModel> functionThatRetrievesADataModel);

        [Obsolete("Use Execute(IFindQuery, IQueryServiceDataManager, string id = null) overload")]
        string Execute(IFindQuery query, Func<IEnumerable<IQueryDataModel>> funcThatRetreivesDataModels, Func<int> funcThatReturnsTotalCountForPaging);


        IEnumerable<LinkQuery> BuildUrlsForLinks(IEnumerable<IRelationshipDefinition> relationships);
        IEnumerable<LinkQuery> BuildUrlsForLinks_1(IEnumerable<IRelationshipDefinition> relationships);
        void FetchResultsForLinks(IEnumerable<LinkQuery> linkUrls);

        string Execute(NameValueCollection queryString, IQueryServiceDataManager dataManager, string id = null);
        string Execute(IFindQuery query, IQueryServiceDataManager dataManager, string id);
    }
}

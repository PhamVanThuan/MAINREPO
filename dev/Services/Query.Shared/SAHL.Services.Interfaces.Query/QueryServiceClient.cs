using System;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.Query
{
    public interface IQueryServiceClient
    {
        ISystemMessageCollection HandleQuery<T>(T query) where T : IQueryServiceQuery;
    }

    public class QueryServiceClient : ServiceHttpClientWindowsAuthenticated, IQueryServiceClient
    {
        public QueryServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator) 
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public virtual ISystemMessageCollection HandleQuery<T>(T query) where T : IQueryServiceQuery
        {
            return base.PerformQueryInternal(query);
        }
    }

    public class QueryServiceQuery : ServiceQuery<QueryServiceQueryResult>, IQueryServiceQuery
    {
        public Guid Id { get; private set; }

        public string RelativeUrl { get; private set; }

        public QueryServiceQuery(string relativeUrl)
        {
            this.RelativeUrl = relativeUrl;
        }
    }

    public class QueryServiceQueryResult
    {
        public string Result { get; set; }

        public QueryServiceQueryResult(string result)
        {
            this.Result = result;
        }
    }

    public interface IQueryServiceQuery : IServiceQuery
    {
    }
}

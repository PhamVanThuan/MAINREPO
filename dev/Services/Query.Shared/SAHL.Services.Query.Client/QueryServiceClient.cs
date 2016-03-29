using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;

namespace SAHL.Services.Query.Client
{
    public class QueryServiceClient
    {
        private readonly IServiceUrlConfigurationProvider serivceUrlConfigurationProvider;
        private readonly IQueryServiceUrlBuilder builder;

        public QueryServiceClient(IServiceUrlConfigurationProvider serivceUrlConfigurationProvider, IQueryServiceUrlBuilder builder)
        {
            this.serivceUrlConfigurationProvider = serivceUrlConfigurationProvider;
            this.builder = builder;
        }

        //TODO: FluentQuery to still be built
        public QueryServiceResponse Search(object someQueryFromFluentApi)
        {
            return new QueryServiceResponse();
        }
    }

    public class QueryServiceResponse
    {
        public bool IsSuccess { get; private set; }
        public dynamic Result { get; private set; }
    }

    public class QueryServiceUrlBuilder : IQueryServiceUrlBuilder
    {
        public string BuildUrl()
        {
            return string.Empty;
        }
    }

    public interface IQueryServiceUrlBuilder
    {
        string BuildUrl();
    }
}

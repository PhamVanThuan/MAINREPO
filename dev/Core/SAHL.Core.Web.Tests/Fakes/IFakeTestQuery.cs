using SAHL.Core.Services;

namespace SAHL.Core.Web.Tests
{
    public interface IFakeTestQuery<T> : IFakeTestQuery, IServiceQuery<T> where T : IServiceQueryResult
    {
    }

    public interface IFakeTestQuery : IServiceQuery
    {
    }
}
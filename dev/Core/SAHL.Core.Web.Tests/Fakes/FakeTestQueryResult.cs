using SAHL.Core.Services;

namespace SAHL.Core.Web.Tests
{
    public class FakeTestQueryResult : IServiceQueryResult
    {
        public int NumberOfPages { get; set; }

        public int ResultCountInAllPages { get; set; }

        public int ResultCountInPage { get; set; }

        public int QueryDurationInMilliseconds { get; set; }
    }
}
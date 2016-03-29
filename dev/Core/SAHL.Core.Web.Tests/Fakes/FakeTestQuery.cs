using System;

namespace SAHL.Core.Web.Tests
{
    public class FakeTestQuery : IFakeTestQuery<FakeTestQueryResult>
    {
        public Guid Id { get; set; }

        public FakeTestQueryResult Result { get; set; }
    }
}

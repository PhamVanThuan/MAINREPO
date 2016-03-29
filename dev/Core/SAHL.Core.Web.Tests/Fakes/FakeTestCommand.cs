using System;

namespace SAHL.Core.Web.Tests
{
    public class FakeTestCommand : IFakeTestCommand
    {
        public Guid Id { get; private set; }

        public string ResultMessage { get; set; }
    }
}
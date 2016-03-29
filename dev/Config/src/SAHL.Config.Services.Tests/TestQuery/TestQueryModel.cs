using System;

namespace SAHL.Config.Services.Tests
{
    public class TestQueryModel
    {
        public TestQueryModel()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}

using SAHL.Core.Testing.Ioc;
using StructureMap.Pipeline;
using System.Collections.Generic;
namespace SAHL.Core.Testing.Fluent
{
    public sealed class FluentTestContext
    {
        internal ITestingIoc TestingIoc { get; private set; }
        internal Dictionary<string, object> Results { get; set; }
        private Dictionary<string, ExplicitArguments> typesAndArgs { get; set; } 
        internal FluentTestContext(ITestingIoc testingIoc)
        {
            this.Results = new Dictionary<string, object>();
            this.typesAndArgs = new Dictionary<string, ExplicitArguments>();
            this.TestingIoc = testingIoc;
        }
    }
}
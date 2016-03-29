using SAHL.Core.Testing.Ioc;
namespace SAHL.Core.Testing.Fluent
{
    public abstract class FluentTest
    {
        private static ITestingIoc testingIoc;
        static FluentTest()
        {
            testingIoc = TestingIoc.Initialise();
        }
        protected FluentTestSetup test
        {
            get
            {
                var context = new FluentTestContext(testingIoc);
                var fluentTestSetup = new FluentTestSetup(context);
                return fluentTestSetup;
            }
        }
    }
}
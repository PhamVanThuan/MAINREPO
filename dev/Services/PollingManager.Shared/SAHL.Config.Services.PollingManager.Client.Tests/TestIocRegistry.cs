using NUnit.Framework;
using StructureMap;

namespace SAHL.Config.Services.PollingManager.Client.Tests
{
    [TestFixture]
    public class TestIocRegistry
    {
        [TestFixtureTearDown]
        public void Teardown()
        {
            ObjectFactory.Container.Dispose();
        }

        [Test]
        public void Initialize_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(this.ConfigureIoc);
            //---------------Test Result -----------------------
        }

        private void ConfigureIoc()
        {
            ObjectFactory.Initialize(expression => expression.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                    scanner.LookForRegistries();
                }));
        }
    }
}

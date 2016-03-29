using AutoMapper;
using StructureMap;

using NUnit.Framework;

namespace SAHL.Config.AutoMapper.Tests
{
    [TestFixture]
    public class TestIocRegistry
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                            scanner.Convention<AutoMapperProfileConvention>();
                            scanner.LookForRegistries();
                        });
                });
        }

        [Test]
        public void Registry_ShouldRegisterConfigurationStore()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var instance = ObjectFactory.GetInstance<ConfigurationStore>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(instance);
        }

        [Test]
        public void Registry_ShouldRegisterIConfigurationProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var instance = ObjectFactory.GetInstance<IConfigurationProvider>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(instance);
        }

        [Test]
        public void Registry_ShouldRegisterIConfiguration()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var instance = ObjectFactory.GetInstance<IConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(instance);
        }

        [Test]
        public void Registry_ShouldRegisterITypeMapFactory()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var instance = ObjectFactory.GetInstance<ITypeMapFactory>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(instance);
        }

        [Test]
        public void Registry_ShouldRegisterIMappingEngine()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var instance = ObjectFactory.GetInstance<IMappingEngine>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(instance);
        }
    }
}

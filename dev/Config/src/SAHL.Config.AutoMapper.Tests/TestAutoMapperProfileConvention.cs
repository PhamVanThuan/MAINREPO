using System.Linq;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;

namespace SAHL.Config.AutoMapper.Tests
{
    [TestFixture]
    public class TestAutoMapperProfileConvention
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                            scanner.Convention<AutoMapperProfileConvention>();
                        });

                    expression.For<IIocContainer>().Use<StructureMapIocContainer>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var convention = new AutoMapperProfileConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterAutoMapperProfiles()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var profiles = this.iocContainer.GetAllInstances<IAutoMapperProfile>();
            //---------------Test Result -----------------------
            Assert.IsTrue(profiles.Any());
        }

        [Test]
        public void Process_ShouldRegisterTestAutoMapperProfile()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var profiles = this.iocContainer.GetAllInstances<IAutoMapperProfile>();
            //---------------Test Result -----------------------
            Assert.IsTrue(profiles.Any());

            var testProfile = new TestAutoMapperMapProfile();
            var foundProfile = profiles.FirstOrDefault(configuration => configuration.ProfileName == testProfile.ProfileName);
            Assert.IsNotNull(foundProfile);
        }
    }
}

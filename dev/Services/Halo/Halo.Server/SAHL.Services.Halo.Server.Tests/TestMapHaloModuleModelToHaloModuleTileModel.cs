using NUnit.Framework;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloModuleModelToHaloModuleTileModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloModuleModelToHaloModuleTileModel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var expectedName = typeof(MapHaloModuleModelToHaloModuleTileModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloModuleModelToHaloModuleTileModel();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }
    }
}

using AutoMapper;
using NUnit.Framework;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloMenuItemToApplicationMenuItem
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloMenuItemToApplicationMenuItem();
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [Test]
        public void Mapper_ShouldBeValidConfiguration()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Mapper.AssertConfigurationIsValid<MapHaloMenuItemToApplicationMenuItem>();
            //---------------Test Result -----------------------
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var expectedName = typeof(MapHaloMenuItemToApplicationMenuItem).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloMenuItemToApplicationMenuItem();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }
    }
}

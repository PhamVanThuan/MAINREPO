using System;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;

using SAHL.UI.Halo.Shared;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloApplicationConfigurationToHaloApplicationModel
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloApplicationConfigurationToHaloApplicationModel(repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [Test]
        public void Constructor_GivenNullTileConfigurationRespository_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new MapHaloApplicationConfigurationToHaloApplicationModel(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("tileConfigurationRepository", exception.ParamName);
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var repository   = Substitute.For<ITileConfigurationRepository>();
            var expectedName = typeof (MapHaloApplicationConfigurationToHaloApplicationModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloApplicationConfigurationToHaloApplicationModel(repository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }
    }
}

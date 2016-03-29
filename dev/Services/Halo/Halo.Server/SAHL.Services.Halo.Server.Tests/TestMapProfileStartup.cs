using System;

using AutoMapper;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using System.Collections.Generic;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapProfileStartup
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startup = new MapProfileStartup(iocContainer);
            //---------------Test Result -----------------------
            Assert.IsNotNull(startup);
        }

        [Test]
        public void Constructor_GivenNullIocContainer_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new MapProfileStartup(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("iocContainer", exception.ParamName);
        }

        [Test]
        public void Start_GivenIConfigurationNotRegistered_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance<IConfiguration>().Returns(info => null);

            var startup = new MapProfileStartup(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<Exception>(startup.Start);
            //---------------Test Result -----------------------
            Assert.AreEqual("AutoMapper Configuration not found in Ioc Container", exception.Message);
        }

        [Test]
        public void Start_ShouldLoadAllAutoMapperProfilesFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            var startup      = new MapProfileStartup(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            startup.Start();
            //---------------Test Result -----------------------
            iocContainer.Received(1).GetAllInstances<IAutoMapperProfile>();
        }

        [Test]
        public void Start_GivenAutoMapperProfiles_ShouldAddToConfiguration()
        {
            //---------------Set up test pack-------------------
            var configuration     = Substitute.For<IConfiguration>();
            var autoMapperProfile = Substitute.For<IAutoMapperProfile>();

            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetAllInstances<IAutoMapperProfile>().Returns(new List<IAutoMapperProfile> { autoMapperProfile });
            iocContainer.GetInstance<IConfiguration>().Returns(configuration);

            var startup = new MapProfileStartup(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            startup.Start();
            //---------------Test Result -----------------------
            configuration.Received(1).AddProfile(autoMapperProfile as Profile);
        }
    }
}

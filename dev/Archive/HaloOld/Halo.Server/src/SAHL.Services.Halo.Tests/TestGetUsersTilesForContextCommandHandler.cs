using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;
using NUnit.Framework;

using SAHL.Core.Testing;
using SAHL.Core.UI.ApplicationState.Managers;
using SAHL.Core.Caching;
using SAHL.Services.Halo.CommandHandlers;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using SAHL.Core.UI.Context;
using SAHL.Core.BusinessModel;

namespace SAHL.Services.Halo.Tests
{
    [TestFixture]
    public class TestGetUsersTilesForContextCommandHandler
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            TestHelper.SetupIoc();
            //DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);

            //var iocContainer = ObjectFactory.Container as IIocContainer;


            //ObjectFactory.Configure(expression =>
            //    {
            //        expression.For<ICache>().Use<InMemoryCache>();
            //        //expression.For<IIocContainer>().Use(iocContainer);
                //});
        }

        [Test]
        public void HandleCommand_GivenNullContext_ShouldReturn()
        {
            //---------------Set up test pack-------------------
            var userStateManager = ObjectFactory.GetInstance<IUserStateManager>();
            var commandHandler   = new GetUsersTilesForContextCommandHandler(userStateManager);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var systemMessageCollection = commandHandler.HandleCommand(new GetUsersTilesForContextCommand(new TileBusinessContext("Client", BusinessKeyType.LegalEntity, 841167, null, null), "sahl\\eugened"));
            //---------------Test Result -----------------------
            Assert.Fail("Test Not Yet Implemented");
        }
    }
}

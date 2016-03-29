using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Testing;
using SAHL.UI.Halo.Shared;
using SAHL.Core.BusinessModel;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;
using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Halo.Server.CommandHandlers;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestTileEditorUpdateCommandHandler
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileDataRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var commandHandler = new TileEditorUpdateCommandHandler(repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(commandHandler);
        }

        [TestCase("tileDataRepository")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<TileEditorUpdateCommandHandler>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void HandleCommand_GivenRepositoryThrowsException_ShouldReturnExceptionDetailsInSystemCollection()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMessage = "Test exception message";
            var repository = Substitute.For<ITileDataRepository>();

            repository.FindTileEditorDataProvider(Arg.Any<IHaloTileConfiguration>()).Returns(info =>
                {
                    throw new Exception(testExceptionMessage);
                });

            var systemMessageCollection = SystemMessageCollection.Empty();
            var commandHandler          = new TileEditorUpdateCommandHandler(repository);
            var tileConfiguration       = Substitute.For<IHaloTileConfiguration>();
            var haloTileModel           = Substitute.For<IHaloTileModel>();
            var command                 = new TileEditorUpdateCommand(tileConfiguration, new BusinessContext("context", GenericKeyType.Account, 0), haloTileModel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => { systemMessageCollection = commandHandler.HandleCommand(command, null); });
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.AllMessages.Count());
            StringAssert.Contains(testExceptionMessage, systemMessageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void handleCommand_WhenEditorDataProviderNotFound_ShouldReturnErrorInSystemCollection()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileDataRepository>();
            repository.FindTileEditorDataProvider(Arg.Any<IHaloTileConfiguration>()).Returns(info => null);

            var systemMessageCollection = SystemMessageCollection.Empty();
            var commandHandler          = new TileEditorUpdateCommandHandler(repository);
            var tileConfiguration       = Substitute.For<IHaloTileConfiguration>();
            var haloTileModel           = Substitute.For<IHaloTileModel>();
            var command                 = new TileEditorUpdateCommand(tileConfiguration, new BusinessContext("context", GenericKeyType.Account, 0), haloTileModel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            systemMessageCollection = commandHandler.HandleCommand(command, null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.AllMessages.Count());
            StringAssert.Contains("Editor Data Provider not found", systemMessageCollection.AllMessages.FirstOrDefault().Message);
        }
    }
}

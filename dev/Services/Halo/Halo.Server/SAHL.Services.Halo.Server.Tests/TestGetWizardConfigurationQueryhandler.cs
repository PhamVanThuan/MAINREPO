using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Testing;
using SAHL.Core.Logging;
using SAHL.Core.BusinessModel;
using SAHL.Core.SystemMessages;
using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Repository;
using SAHL.Services.Interfaces.Halo.Queries;
using SAHL.Services.Halo.Server.QueryHandlers;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestGetWizardConfigurationQueryhandler : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var tileWizardRepository = Substitute.For<ITileWizardConfigurationRepository>();
            var rawLogger            = Substitute.For<IRawLogger>();
            var loggerSource         = Substitute.For<ILoggerSource>();
            var loggerAppSource      = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryHandler = new GetWizardConfigurationQueryHandler(tileWizardRepository, rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryHandler);
        }

        [TestCase("tileWizardRepository")]
        [TestCase("rawLogger")]
        [TestCase("loggerSource")]
        [TestCase("loggerAppSource")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<GetWizardConfigurationQueryHandler>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void HandleQuery_GivenRepositoryThrowsException_ShouldNotThrowExceptionAndReturnExceptionDetailsInSystemCollection()
        {
            //---------------Set up test pack-------------------
            const string testExceptionMessage = "Test exception message";
            var tileWizardRepository          = Substitute.For<ITileWizardConfigurationRepository>();

            tileWizardRepository.FindWizardConfiguration(Arg.Any<string>()).Returns(info =>
                {
                    throw new Exception(testExceptionMessage);
                });

            var businessContext         = new BusinessContext("", GenericKeyType.Account, 1234);
            var systemMessageCollection = SystemMessageCollection.Empty();
            var queryHandler            = this.CreateQueryHandler(tileWizardRepository);
            var query                   = new GetWizardConfigurationQuery("wizardName", null, null, null, businessContext);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => { systemMessageCollection = queryHandler.HandleQuery(query); });
            //---------------Test Result -----------------------
            Assert.IsNotNull(systemMessageCollection);
            Assert.IsTrue(systemMessageCollection.HasErrors);
            Assert.AreEqual(1, systemMessageCollection.AllMessages.Count());
            StringAssert.Contains(testExceptionMessage, systemMessageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenWizardConfigurationNotFound_ShouldReturnErrorInMessageCollection()
        {
            //---------------Set up test pack-------------------
            var businessContext = new BusinessContext("", GenericKeyType.Account, 1234);
            var query           = new GetWizardConfigurationQuery("wizardName", null, null, null, businessContext);
            var queryHandler    = this.CreateQueryHandler();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageCollection);
            Assert.IsTrue(messageCollection.HasErrors);
            Assert.AreEqual(1, messageCollection.AllMessages.Count());
            StringAssert.Contains("Unable to find the Wizard Configuration named", messageCollection.AllMessages.FirstOrDefault().Message);
        }

        [Test]
        public void HandleQuery_GivenWizardConfigurationExists_ShouldReturnMappedWizardConfigurationModel()
        {
            //---------------Set up test pack-------------------
            var businessContext = new BusinessContext("", GenericKeyType.ThirdPartyInvoice, 1234);
            var query           = new GetWizardConfigurationQuery("Third Party Invoice Capture", null, null, null, businessContext);
            var queryHandler    = this.CreateQueryHandler();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var messageCollection = queryHandler.HandleQuery(query);
            //---------------Test Result -----------------------
            Assert.IsNotNull(messageCollection);
            Assert.IsFalse(messageCollection.HasErrors);

            Assert.AreEqual(1, query.Result.Results.Count());
            var queryResult = query.Result.Results.FirstOrDefault();
            Assert.AreEqual(query.WizardName, queryResult.WizardConfiguration.Name);
        }

        private GetWizardConfigurationQueryHandler CreateQueryHandler(ITileWizardConfigurationRepository tileWizardRepository = null)
        {
            var rawLogger       = Substitute.For<IRawLogger>();
            var loggerSource    = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();

            var wizardRepository = tileWizardRepository ?? new TileWizardConfigurationRepository(this.iocContainer);

            var queryHandler = new GetWizardConfigurationQueryHandler(wizardRepository, rawLogger, loggerSource, loggerAppSource);
            return queryHandler;
        }
    }
}

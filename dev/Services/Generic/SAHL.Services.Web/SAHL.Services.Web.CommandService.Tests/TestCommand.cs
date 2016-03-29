using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Services.Metrics;
using SAHL.Services.Web.CommandService.Models;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestCommand
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var metrics = Substitute.For<ICommandServiceRequestMetrics>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new Command(metrics, "test");
            //---------------Test Result -----------------------
            Assert.IsNotNull(command);
        }

        [Test]
        public void Constructor_GivenValues_ShouldsetProperties()
        {
            //---------------Set up test pack-------------------
            var metrics     = Substitute.For<ICommandServiceRequestMetrics>();
            var commandName = "testcommand";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new Command(metrics, commandName);
            //---------------Test Result -----------------------
            Assert.AreEqual(metrics, command.CommandServiceMetrics);
            Assert.AreEqual(commandName, command.CommandName);
        }
    }
}

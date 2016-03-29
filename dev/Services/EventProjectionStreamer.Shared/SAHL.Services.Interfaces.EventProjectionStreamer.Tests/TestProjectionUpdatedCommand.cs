using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.Core.Testing;

namespace SAHL.Services.Interfaces.EventProjectionStreamer.Tests
{
    [TestFixture]
    public class TestProjectionUpdatedCommand
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new ProjectionUpdatedCommand("test", new List());
            //---------------Test Result -----------------------
            Assert.IsNotNull(command);
        }

        [TestCase("projectionName")]
        [TestCase("projectionData")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<ProjectionUpdatedCommand>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Constructor_GivenProjectionName_ShouldSetProjectNameProperty()
        {
            //---------------Set up test pack-------------------
            var projectionName = "test";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new ProjectionUpdatedCommand(projectionName, new List());
            //---------------Test Result -----------------------
            Assert.AreEqual(projectionName, command.ProjectionName);
        }

        [Test]
        public void Constructor_GivenProjectionData_ShouldSetProjectDataProperty()
        {
            //---------------Set up test pack-------------------
            var projectionData = "some projection data";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new ProjectionUpdatedCommand("test", projectionData);
            //---------------Test Result -----------------------
            Assert.AreEqual(projectionData, command.ProjectionData);
        }
    }
}

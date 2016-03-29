using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.Web.CommandService.Controllers;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestProfileController
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var controller = new ProfileController();
            //---------------Test Result -----------------------
            Assert.IsNotNull(controller);
        }

        [Test]
        public void GetImage_GivenUserName_ShouldReturnImageInResponse()
        {
            //---------------Set up test pack-------------------
            var controller = new ProfileController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var httpResponseMessage = controller.GetImage("johand");
            //---------------Test Result -----------------------
            Assert.IsNotNull(httpResponseMessage);
            Assert.IsNotNull(httpResponseMessage.Content);
        }
    }
}

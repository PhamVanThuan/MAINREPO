using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.Web.CommandService.Controllers;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestHomeController
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var homeController = new HomeController();
            //---------------Test Result -----------------------
            Assert.IsNotNull(homeController);
        }

        [Test]
        public void Index_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var homeController = new HomeController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => homeController.Index());
            //---------------Test Result -----------------------
        }
    }
}

using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.Web.CommandService.Models;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestPageModel
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var pageModel = new PageModel(null, 1, 1);
            //---------------Test Result -----------------------
            Assert.IsNotNull(pageModel);
        }

        [Test]
        public void Constructor_GivenValues_ShouldsetProperties()
        {
            //---------------Set up test pack-------------------
            var pageNumber  = 1;
            var total       = 2;
            var commandList = new List<string> { "test1", "test2" };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var pageModel   = new PageModel(commandList, pageNumber, total);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(commandList, pageModel.Commands);
            Assert.AreEqual(pageNumber, pageModel.PageNumber);
            Assert.AreEqual(total, pageModel.TotalPageCount);
        }
    }
}

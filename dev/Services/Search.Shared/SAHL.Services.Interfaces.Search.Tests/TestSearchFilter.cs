using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Tests
{
    [TestFixture]
    public class TestSearchFilter
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var searchFilter = new SearchFilter("name", "value");
            //---------------Test Result -----------------------
            Assert.IsNotNull(searchFilter);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var searchName = "name";
            var searchValue = "value";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var searchFilter = new SearchFilter(searchName, searchValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(searchName, searchFilter.Name);
            Assert.AreEqual(searchValue, searchFilter.Value);
        }
    }
}

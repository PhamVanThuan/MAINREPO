using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Tests
{
    [TestFixture]
    public class TestGetClientSearchDetailQueryResult
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryResult = new GetClientSearchDetailQueryResult("type", Product.Edge, 
                                                                   1234, 4321, "role", 1, "address");
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryResult);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var resultType = "type";
            var product = Product.Edge;
            var key = 1234;
            var parentKey = 4321;
            var role = "role";
            var level = 1;
            var address = "address";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryResult = new GetClientSearchDetailQueryResult(resultType, product,
                                                                   key, parentKey, role, level, address);
            //---------------Test Result -----------------------
            Assert.AreEqual(resultType, queryResult.Type);
            Assert.AreEqual(product, queryResult.Product);
            Assert.AreEqual(key, queryResult.Key);
            Assert.AreEqual(parentKey, queryResult.ParentKey);
            Assert.AreEqual(role, queryResult.Role);
            Assert.AreEqual(level, queryResult.Level);
            Assert.AreEqual(address, queryResult.Address);
        }
    }
}

using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.Interfaces.Search.Queries;

namespace SAHL.Services.Interfaces.Search.Tests
{
    [TestFixture]
    public class TestGetTaskSearchDetailQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetTaskSearchDetailQuery(1234);
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProprties()
        {
            //---------------Set up test pack-------------------
            var instanceId = 1234;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetTaskSearchDetailQuery(instanceId);
            //---------------Test Result -----------------------
            Assert.AreEqual(instanceId, query.InstanceId);
        }
    }
}

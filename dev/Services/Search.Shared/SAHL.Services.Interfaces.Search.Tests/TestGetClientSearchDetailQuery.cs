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
    public class TestGetClientSearchDetailQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetClientSearchDetailQuery(0);
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_GivenLegalEntityKey_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var legalEntityKey = 1234;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetClientSearchDetailQuery(legalEntityKey);
            //---------------Test Result -----------------------
            Assert.AreEqual(legalEntityKey, query.LegalEntityKey);
        }
    }
}

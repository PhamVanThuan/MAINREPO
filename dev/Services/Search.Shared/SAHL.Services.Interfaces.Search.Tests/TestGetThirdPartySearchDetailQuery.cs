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
    public class TestGetThirdPartySearchDetailQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetThirdPartySearchDetailQuery(1234);
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var legalEntityKey = 1234;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetThirdPartySearchDetailQuery(legalEntityKey);
            //---------------Test Result -----------------------
            Assert.AreEqual(legalEntityKey, query.LegalEntityKey);
        }
    }
}

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
    public class TestGetThirdPartySearchDetailQueryResult
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryResult = new GetThirdPartySearchDetailQueryResult("type", "addressType", "address", true, 
                                                                       "contact", false, false, "deedsOffice");
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryResult);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var resultType = "type";
            var addressType = "addressType";
            var address = "address";
            var isActive = true;
            var contact = "contact";
            var isLitigationAttorney = false;
            var isRegistrationAttorney = false;
            var deedsOffice = "deedsOffice";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryResult = new GetThirdPartySearchDetailQueryResult(resultType, addressType, address, isActive,
                                                                       contact, isLitigationAttorney, isRegistrationAttorney, deedsOffice);
            //---------------Test Result -----------------------
            Assert.AreEqual(resultType, queryResult.Type);
            Assert.AreEqual(addressType, queryResult.AddressType);
            Assert.AreEqual(address, queryResult.Address);
            Assert.AreEqual(isActive, queryResult.IsActive);
            Assert.AreEqual(contact, queryResult.Contact);
            Assert.AreEqual(isLitigationAttorney, queryResult.IsLitigationAttorney);
            Assert.AreEqual(isRegistrationAttorney, queryResult.IsRegistrationAttorney);
            Assert.AreEqual(deedsOffice, queryResult.DeedsOffice);
        }
    }
}

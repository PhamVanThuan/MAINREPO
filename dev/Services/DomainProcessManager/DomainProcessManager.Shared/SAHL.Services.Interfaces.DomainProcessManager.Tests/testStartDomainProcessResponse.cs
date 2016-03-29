using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Core.Data;

namespace SAHL.Services.Interfaces.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestStartDomainProcessResponse
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var response = new StartDomainProcessResponse(true, null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(response);
        }

        [Test]
        public void Properties_GivenData_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var result        = true;
            var testDataModel = new TestDataModel();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var response = new StartDomainProcessResponse(result, testDataModel);
            //---------------Test Result -----------------------
            Assert.AreEqual(result, response.Result);
            Assert.AreSame(testDataModel, response.Data);
        }

        private class TestDataModel : IDataModel
        {
        }
    }
}

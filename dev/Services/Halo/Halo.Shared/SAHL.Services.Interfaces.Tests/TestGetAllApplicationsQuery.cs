using NUnit.Framework;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestGetAllApplicationsQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetAllApplicationsQuery();
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }
    }
}

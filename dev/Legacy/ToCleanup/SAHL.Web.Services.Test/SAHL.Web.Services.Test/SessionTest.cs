using System.Configuration;
using NUnit.Framework;


namespace SAHL.Web.Services.Test
{
    /// <summary>
    /// Class for testing the Session Class
    /// </summary>
    [TestFixture]
    public class SessionTest : TestServiceBase
    {

        //private SAHL.Web.Services.SAHLSession _SessionObject;
        private string domain;
        private string domainUser;
        private string domainPassword;

        
        /// <summary>
        /// reates common objects and values for the tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
           // _SessionObject = new SAHLSession();
            domain = ConfigurationManager.AppSettings["Domain"]; 
            domainUser = ConfigurationManager.AppSettings["DomainUser"]; 
            domainPassword = ConfigurationManager.AppSettings["DomainPassword"]; 
           

        }

        /// <summary>
        /// destroys all objects created for the test
        /// </summary>
        [TearDown]
        public void Teardown()
        {
           // _SessionObject = null;
            domain = null;
            domainUser = null;
            domainPassword = null;

        }


        /// <summary>
        /// Tests that the session can retrieve an ADUser
        /// </summary>
        [Test]
        public void GetADUserTest()
        {

        }
    }
}

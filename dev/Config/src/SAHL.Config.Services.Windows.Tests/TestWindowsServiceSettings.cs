using System;
using System.Collections.Specialized;

using NUnit.Framework;

namespace SAHL.Config.Services.Windows.Tests
{
    [TestFixture]
    public class TestWindowsServiceSettings
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Test Result -----------------------
            Assert.IsNotNull(settings);
        }

        [Test]
        public void EnableFirstChanceException_GivenNoValue_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var enableFirstChanceException = settings.EnableFirstChanceException;
            //---------------Test Result -----------------------
            Assert.IsFalse(enableFirstChanceException);
        }

        [Test]
        public void EnableFirstChanceException_GivenTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            valueCollection.Add("EnableFirstChanceException", "true");
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var enableFirstChanceException = settings.EnableFirstChanceException;
            //---------------Test Result -----------------------
            Assert.IsTrue(enableFirstChanceException);
        }

        [Test]
        public void EnableFirstChanceException_GivenFalse_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            valueCollection.Add("EnableFirstChanceException", "false");
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var enableFirstChanceException = settings.EnableFirstChanceException;
            //---------------Test Result -----------------------
            Assert.IsFalse(enableFirstChanceException);
        }

        [Test]
        public void EnableUnhandledException_GivenNoValue_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var enableUnhandledException = settings.EnableUnhandledException;
            //---------------Test Result -----------------------
            Assert.IsFalse(enableUnhandledException);
        }

        [Test]
        public void EnableUnhandledException_GivenTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            valueCollection.Add("EnableUnhandledException", "true");
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var enableUnhandledException = settings.EnableUnhandledException;
            //---------------Test Result -----------------------
            Assert.IsTrue(enableUnhandledException);
        }

        [Test]
        public void EnableUnhandledException_GivenFalse_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var valueCollection = new NameValueCollection();
            valueCollection.Add("EnableUnhandledException", "false");
            var settings = new WindowsServiceSettings(valueCollection);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var enableUnhandledException = settings.EnableUnhandledException;
            //---------------Test Result -----------------------
            Assert.IsFalse(enableUnhandledException);
        }
    }
}

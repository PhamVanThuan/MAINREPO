using System;

using NUnit.Framework;
using SAHL.Core.Extensions;

namespace SAHL.Core.Tests.Extensions
{
    [TestFixture]
    public class TestExceptionExtensions
    {
        [Test]
        public void BuildExceptionErrorMessage_GivenNoInnerException_ShouldreturnOnlyExceptionMessage()
        {
            //---------------Set up test pack-------------------
            var testException = "Test Exception";
            var exception     = new Exception(testException);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var errorMessage = exception.BuildExceptionErrorMessage();
            //---------------Test Result -----------------------
            testException = testException + "\r\n";
            Assert.AreEqual(testException, errorMessage);
        }

        [Test]
        public void BuildExceptionErrorMessage_GivenInnerException_ShouldreturnExceptionMessageAndInnerException()
        {
            //---------------Set up test pack-------------------
            const string testException     = "Test Exception";
            const string innerExceptionMsg = "Inner Exception";
            var innerException             = new Exception(innerExceptionMsg);
            var exception                  = new Exception(testException, innerException);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var errorMessage = exception.BuildExceptionErrorMessage();
            //---------------Test Result -----------------------
            StringAssert.Contains(testException, errorMessage);
            StringAssert.Contains(innerExceptionMsg, errorMessage);
        }

        [Test]
        public void BuildExceptionErrorMessage_Given2InnerExceptions_ShouldreturnExceptionMessageAndInnerExceptions()
        {
            //---------------Set up test pack-------------------
            const string testException      = "Test Exception";
            const string innerExceptionMsg1 = "Inner Exception 1";
            const string innerExceptionMsg2 = "Inner Exception 1";
            var innerException1             = new Exception(innerExceptionMsg1);
            var innerException2             = new Exception(innerExceptionMsg2, innerException1);
            var exception                   = new Exception(testException, innerException2);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var errorMessage = exception.BuildExceptionErrorMessage();
            //---------------Test Result -----------------------
            StringAssert.Contains(testException, errorMessage);
            StringAssert.Contains(innerExceptionMsg1, errorMessage);
            StringAssert.Contains(innerExceptionMsg2, errorMessage);
        }
    }
}

using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Core.Extensions;

namespace SAHL.Core.Tests.Extensions
{
    [TestFixture]
    public class TestTypeExtensions
    {
        [Test]
        public void GetPropertyValue_GivenNullSource_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var type = typeof(FakeTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => type.GetPropertyValue(null, "SomeTest"));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("source", exception.ParamName);
        }

        [Test]
        public void GetPropertyValue_GivenEmptyPropertyName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var type = typeof(FakeTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => type.GetPropertyValue(new FakeTestType(), ""));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("propertyName", exception.ParamName);
        }

        [Test]
        public void GetPropertyValue_GivenPropertyDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "test";
            var fakeTestType = new FakeTestType();
            var type         = fakeTestType.GetType();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propertyValue = type.GetPropertyValue(fakeTestType, propertyName);
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValue);
        }

        [Test]
        public void GetPropertyValue_GivenPropertyExists_ShouldReturnPropertyValue()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "SomeTest";
            var expectedValue         = Guid.NewGuid();
            var fakeTestType          = new FakeTestType { SomeTest = expectedValue };
            var type                  = fakeTestType.GetType();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propertyValue = type.GetPropertyValue(fakeTestType, propertyName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(propertyValue);
            Assert.AreEqual(expectedValue, propertyValue);
        }

        private class FakeTestType
        {
            public Guid SomeTest { get; set; }
        }
    }
}

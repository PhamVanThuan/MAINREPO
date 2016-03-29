using System;
using NUnit.Framework;
using SAHL.Core.Validation.Attributes;

namespace SAHL.Core.Tests.Validation.Attributes
{
    [TestFixture]
    public class ShouldBeInSahlDateFormatAttributeTests
    {
        [Test]
        public void IsValid_GivenNonMatchingDateOnString_ShouldReturnFalse()
        {
            const string date = "9999-99-AA";

            var attribute = new ShouldBeInSahlDateFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsValid_GivenMatchingDateOnString_ShouldReturnTrue()
        {
            const string date = "9999-99-99";

            var attribute = new ShouldBeInSahlDateFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenNullDateOnDateTime_ShouldReturnTrue()
        {
            DateTime? date = null;

            var attribute = new ShouldBeInSahlDateFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenMatchingDateOnDateTime_ShouldReturnTrue()
        {
            var date = DateTime.Today;

            var attribute = new ShouldBeInSahlDateFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenNonMidnightTimeOnDateTime_ShouldReturnFalse()
        {
            var date = DateTime.Now;

            var attribute = new ShouldBeInSahlDateFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsFalse(result);
        }
    }
}
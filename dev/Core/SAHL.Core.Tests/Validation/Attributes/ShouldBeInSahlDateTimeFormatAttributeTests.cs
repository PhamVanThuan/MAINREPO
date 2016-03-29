using System;
using NUnit.Framework;
using SAHL.Core.Validation.Attributes;

namespace SAHL.Core.Tests.Validation.Attributes
{
    [TestFixture]
    public class ShouldBeInSahlDateTimeFormatAttributeTests
    {
        [Test]
        public void IsValid_GivenNonMatchingDateTimeOnString_ShouldReturnFalse()
        {
            const string date = "9999-99-99 00:01:01 MM";

            var attribute = new ShouldBeInSahlDateTimeFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsValid_GivenMatchingAnteMeridianDateTimeOnString_ShouldReturnTrue()
        {
            const string date = "9999-99-99 99:99:99 AM";

            var attribute = new ShouldBeInSahlDateTimeFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenMatchingPostMeridianDateTimeOnString_ShouldReturnTrue()
        {
            const string date = "9999-99-99 99:99:99 AM";

            var attribute = new ShouldBeInSahlDateTimeFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenNullDateTime_ShouldReturnTrue()
        {
            DateTime? date = null;

            var attribute = new ShouldBeInSahlDateTimeFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenMatchingDateTime_ShouldReturnTrue()
        {
            var date = DateTime.Today;

            var attribute = new ShouldBeInSahlDateTimeFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenNonMidnightTimeOnDateTime_ShouldReturnTrue()
        {
            var date = DateTime.Now;

            var attribute = new ShouldBeInSahlDateTimeFormatAttribute();
            var result = attribute.IsValid(date);

            Assert.IsTrue(result);
        }
    }
}
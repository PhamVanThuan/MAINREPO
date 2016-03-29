using System;
using NUnit.Framework;
using SAHL.Core.Validation.Attributes;

namespace SAHL.Core.Tests.Validation.Attributes
{
    [TestFixture]
    public class TieredRegularExpressionAttributeTests
    {
        [Test]
        public void IsValid_GivenNullServerSidePattern_ShouldThrowException()
        {
            var attribute = new TieredReguarExpressionAttribute(ValidationFakesHelpers.MatchAnyCharacterRegularExpressionPattern, null);

            var thrownException = Assert.Throws(typeof(InvalidOperationException), () =>
            {
                attribute.IsValid(null);
            });

            Assert.AreEqual("The pattern must be set to a valid regular expression.", thrownException.Message);
        }

        [Test]
        public void IsValid_GivenEmptyServerSidePattern_ShouldThrowException()
        {
            var attribute = new TieredReguarExpressionAttribute(ValidationFakesHelpers.MatchAnyCharacterRegularExpressionPattern, "");

            var thrownException = Assert.Throws(typeof(InvalidOperationException), () =>
            {
                attribute.IsValid(null);
            });

            Assert.AreEqual("The pattern must be set to a valid regular expression.", thrownException.Message);
        }

        [Test]
        public void IsValid_GivenMatchAnythingServerSidePatternOnNullValue_ShouldReturnTrue()
        {
            var attribute = new TieredReguarExpressionAttribute(ValidationFakesHelpers.MatchAnyCharacterRegularExpressionPattern, ValidationFakesHelpers.MatchAnyCharacterRegularExpressionPattern);

            var result = attribute.IsValid(null);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenMatchingString_ShouldReturnTrue()
        {
            var attribute = new TieredReguarExpressionAttribute(ValidationFakesHelpers.MatchSingleDigitRegularExpressionPattern, ValidationFakesHelpers.MatchSingleLetterRegularExpressionPattern);

            var result = attribute.IsValid("a");
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_GivenNonMatchingString_ShouldReturnFalse()
        {
            var attribute = new TieredReguarExpressionAttribute(ValidationFakesHelpers.MatchSingleDigitRegularExpressionPattern, ValidationFakesHelpers.MatchSingleLetterRegularExpressionPattern);

            var result = attribute.IsValid("1");
            Assert.False(result);
        }
    }
}
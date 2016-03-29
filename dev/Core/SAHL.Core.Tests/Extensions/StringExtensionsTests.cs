using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using SAHL.Core.DataType;
using SAHL.Core.Strings;

namespace SAHL.Core.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void TryFormat_GivenNullString_ShouldReturnEmptyString()
        {
            const string format = "{0}";

            var result = StringExtensions.TryFormat(format, null);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TryFormat_GivenEmptyString_ShouldReturnEmptyString()
        {
            const string format = "{0}";

            var result = StringExtensions.TryFormat(format, string.Empty);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TryFormat_GivenValidDate_ShouldReturnFormattedDateString()
        {
            const string format = "{0:yyyy-MM-dd}";

            var date = DateTime.Today;

            var result = StringExtensions.TryFormat(format, date);

            Assert.That(result, Is.EqualTo(string.Format(format, date)));
        }

        [Test]
        public void TryFormat_GivenValidCurrency_ShouldReturnFormattedCurrencyString()
        {
            const string format = "{0:c}";

            const decimal amount = 12345.67890m;

            var result = StringExtensions.TryFormat(format, amount);

            Assert.That(result, Is.EqualTo(string.Format(CultureInfo.InvariantCulture, format, amount)));
        }

        [Test]
        public void TryFormat_GivenSpecificCurrencyFormat_ShouldReturnFormattedCurrencyString()
        {
            const string format = "R{0:n2}";

            const decimal amount = 12345.67890m;

            var result = StringExtensions.TryFormat(format, amount);

            Assert.That(result, Is.EqualTo(string.Format(CultureInfo.InvariantCulture, format, amount)));
            Assert.That(result, Is.EqualTo("R12,345.68"));
        }

        [Test]
        public void TryFormat_GivenSahlCurrencyFormat_ShouldReturnFormattedCurrencyStringInTheExpectedFormat()
        {
            const string format = Formats.Currency.Default;

            const decimal amount = 12345.67890m;

            var result = StringExtensions.TryFormat(format, amount);

            Assert.That(result, Is.EqualTo(string.Format(CultureInfo.InvariantCulture, format, amount)));
            Assert.That(result, Is.EqualTo("R12,345.68"));
        }

        [Test]
        public void TryFormat_GivenInvalidFormatString_ShouldReturnStandardToStringResult()
        {
            const string format = "{";

            var date = DateTime.Today;

            var result = StringExtensions.TryFormat(format, date);

            Assert.That(result, Is.EqualTo(date.ToString()));
        }

        [Test]
        public void TryFormat_GivenNullFormatString_ShouldReturnStandardToStringResult()
        {
            var date = DateTime.Today;

            var result = StringExtensions.TryFormat(null, date);

            Assert.That(result, Is.EqualTo(date.ToString()));
        }

        [Test]
        public void TryFormat_GivenEmptyFormatString_ShouldReturnEmptyString()
        {
            var date = DateTime.Today;

            var result = StringExtensions.TryFormat(string.Empty, date);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TryFormat_GivenNullFormatAndNullValue_ShouldReturnNullString()
        {
            var result = StringExtensions.TryFormat(null, null);

            Assert.That(result, Is.Null);
        }
    }
}
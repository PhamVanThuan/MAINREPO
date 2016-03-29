using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SAHL.Core.Strings;

namespace SAHL.Core.Tests.Strings
{
    [TestFixture]
    public class StringReplacerTests
    {
        [Test]
        public void Replace_GivenNullSource_ShouldReturnSourceString()
        {
            var replacer = new StringReplacer();
            var result = replacer.Replace(null, Enumerable.Empty<KeyValuePair<string, string>>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Replace_GivenEmptySource_ShouldReturnSourceString()
        {
            var replacer = new StringReplacer();
            var result = replacer.Replace(string.Empty, Enumerable.Empty<KeyValuePair<string, string>>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_GivenNullReplacements_ShouldReturnSourceString()
        {
            const string source = "banana";
            var replacer = new StringReplacer();
            var result = replacer.Replace(source, null);

            Assert.That(result, Is.EqualTo(source));
        }

        [Test]
        public void Replace_GivenEmptyReplacements_ShouldReturnSourceString()
        {
            const string source = "banana";
            var replacer = new StringReplacer();
            var result = replacer.Replace(source, Enumerable.Empty<KeyValuePair<string, string>>());

            Assert.That(result, Is.EqualTo(source));
        }

        [Test]
        public void Replace_GivenValidSourceAndValidReplacements_ShouldReturnStringWithValuesReplaced()
        {
            const string source = "{p1}/{p2}/{p3}/something/{p4}";

            var replacements = new Dictionary<string, string>
            {
                { "{p1}", "banana1" },
                { "{p2}", "banana2" },
                { "{p3}", "banana3" },
                { "{p4}", "banana4" },
            };

            var replacer = new StringReplacer();
            var result = replacer.Replace(source, replacements);

            Assert.That(result, Is.EqualTo("banana1/banana2/banana3/something/banana4"));
        }

        [Test]
        public void Replace_GivenValidSourceAndValidReplacementsWithOutOfOrderArguments_ShouldThrowException()
        {
            const string source = "{p1}/{p2}/{p3}/something/{p4}";

            var replacements = new Dictionary<string, string>
            {
                { "{p4}", "banana4" },
                { "{p3}", "banana3" },
                { "{p2}", "banana2" },
                { "{p1}", "banana1" },
            };

            try
            {
                var replacer = new StringReplacer();
                replacer.Replace(source, replacements);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (InvalidOperationException ex)
            {
                var message = "Replacements must be in the order that they appear in the string. " +
                    "Use a regex if you need to replace elements within a string out-of-linear-order.";
                Assert.That(ex.Message, Is.EqualTo(message));
            }
        }

        [Test]
        public void Replace_GivenNullReplacementKey_ShouldReturnSourceString()
        {
            const string source = "banana";

            var replacements = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(null, "blah"),
            };

            var replacer = new StringReplacer();
            var result = replacer.Replace(source, replacements);

            Assert.That(result, Is.EqualTo(source));
        }

        [Test]
        public void Replace_GivenCustomComparison_ShouldPerformReplaceWithCustomComparer()
        {
            const string source = "banana";

            var replacements = new Dictionary<string, string>
            {
                { "BANANA", "apple" },
            };

            var replacer = new StringReplacer();
            var result = replacer.Replace(source, replacements, StringComparison.OrdinalIgnoreCase);

            Assert.That(result, Is.EqualTo("apple"));
        }

        [Test]
        public void Replace_GivenNullReplacementValue_ShouldReplaceWithEmptyString()
        {
            const string source = "banana";

            var replacements = new Dictionary<string, string>
            {
                { "ba", null },
            };

            var replacer = new StringReplacer();
            var result = replacer.Replace(source, replacements);

            Assert.That(result, Is.EqualTo("nana"));
        }

        [Test]
        public void Replace_GivenReplacementsThatDoNotAppearInTheSourceString_ShouldPerformNoReplacements()
        {
            const string source = "banana";

            var replacements = new Dictionary<string, string>
            {
                { "apple", null },
            };

            var replacer = new StringReplacer();
            var result = replacer.Replace(source, replacements);

            Assert.That(result, Is.EqualTo(source));
        }
    }
}
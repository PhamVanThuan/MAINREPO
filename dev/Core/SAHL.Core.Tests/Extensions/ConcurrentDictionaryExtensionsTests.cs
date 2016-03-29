using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SAHL.Core.Extensions;

namespace SAHL.Core.Tests.Extensions
{
    [TestFixture]
    public class ConcurrentDictionaryExtensionsTests
    {
        [Test]
        public void ToConcurrentDictionary_GivenEmptySource_ShouldReturnEmptyDictionary()
        {
            var source = Enumerable.Empty<string>();
            var result = source.ToConcurrentDictionary(a => a, a => a);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ToConcurrentDictionary_GivenNonEmptySource_ShouldReturnDictionaryWithMatchingElements()
        {
            var source = Enumerable.Range(0, 10).Select(a => "Banana " + a);
            var result = source.ToConcurrentDictionary(a => a + " " + a, a => a + "_" + a);

            IDictionary<string, string> dictionary = new ConcurrentDictionary<string, string>();
            foreach (var item in source)
            {
                dictionary.Add(item + " " + item, item + "_" + item);
            }

            Assert.That(result, Is.EqualTo(dictionary));
        }

        [Test]
        public void ToConcurrentDictionary_GivenCustomEqualityComparer_ShouldFailWhenComparerPresentsTwoDifferentValuesAsEqual()
        {
            var source = new List<string>
            {
                "banana",
                "BANANA",
            };
            try
            {
                source.ToConcurrentDictionary(a => a, a => a, StringComparer.OrdinalIgnoreCase);
                Assert.Fail("Did not catch duplicate key value");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentException>());
                Assert.That(ex.Message, Contains.Substring("key already exist"));
            }
        }
    }
}

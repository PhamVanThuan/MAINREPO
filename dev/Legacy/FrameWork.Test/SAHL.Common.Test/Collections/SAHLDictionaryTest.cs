using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.Collections;

namespace SAHL.Common.Test.Collections
{
    [TestFixture]
    public class SAHLDictionaryTest
    {

        /// <summary>
        /// Tests the GetKeyByValue method.
        /// </summary>
        [Test]
        public void GetKeyByValue()
        {
            SAHLDictionary<int, string> dict = new SAHLDictionary<int, string>();
            dict.Add(1, "one");
            dict.Add(2, "two");
            dict.Add(3, "three");
            dict.Add(4, "four");
            dict.Add(5, "five");
            dict.Add(6, "six");
            dict.Add(7, "three");

            Assert.AreEqual(2, dict.GetKeyByValue("two"));
            Assert.AreEqual(3, dict.GetKeyByValue("three"));
        }
    }
}

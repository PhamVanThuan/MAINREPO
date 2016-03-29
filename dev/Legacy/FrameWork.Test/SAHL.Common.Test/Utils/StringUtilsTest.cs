using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord.Framework.Internal;

namespace SAHL.Common.Utils.Test
{
    /// <summary>
    /// Enum used for testing.
    /// </summary>
    internal enum StringUtilsEnum
    {
        A = 1,
        B = 2,
        C = 3
    }
    
    [TestFixture]
    public class StringUtilsTest : TestBase
    {
        [Test]
        public void JoinNullableStrings()
        {
            string s = StringUtils.JoinNullableStrings(null, null, null, null);
            Assert.AreEqual(s, "");

            s = StringUtils.JoinNullableStrings("a", null, null);
            Assert.AreEqual(s, "a");

            s = StringUtils.JoinNullableStrings(null, null, "b");
            Assert.AreEqual(s, "b");


            s = StringUtils.JoinNullableStrings("a", null, "b", null, null, "c");
            Assert.AreEqual(s, "abc");
        }

        [Test]
        public void JoinEnumValues()
        {
            string s = StringUtils.JoinEnumValues("");
            Assert.AreEqual(s, "");

            s = StringUtils.JoinEnumValues(",", StringUtilsEnum.A);
            Assert.AreEqual(s, "1");

            s = StringUtils.JoinEnumValues(",", StringUtilsEnum.A, StringUtilsEnum.C);
            Assert.AreEqual(s, "1,3");

            s = StringUtils.JoinEnumValues("--", StringUtilsEnum.A, StringUtilsEnum.C, StringUtilsEnum.B);
            Assert.AreEqual(s, "1--3--2");
        }
    }
}

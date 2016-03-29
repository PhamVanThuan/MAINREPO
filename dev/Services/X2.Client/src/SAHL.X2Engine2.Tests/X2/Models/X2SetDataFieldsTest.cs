using System;

namespace SAHL.X2Engine2.Tests.X2.Models
{
    public class X2SetDataFieldsTest
    {
        public int ApplicationKey { get; set; }
        public long TestBigInt { get; set; }
        public bool TestBool { get; set; }
        public string TestString { get; set; }
        public DateTime TestDate { get; set; }
        public decimal TestDecimal { get; set; }
        public Single TestSingle { get; set; }
        public double TestDouble { get; set; }

        public X2SetDataFieldsTest(int applicationKey, long testBigInt, bool testBool, string testString, DateTime testDate, decimal testDecimal, Single testSingle, double testDouble)
        {
            this.ApplicationKey = applicationKey;
            this.TestBigInt = testBigInt;
            this.TestBool = testBool;
            this.TestString = testString;
            this.TestDate = testDate;
            this.TestDecimal = testDecimal;
            this.TestSingle = testSingle;
            this.TestDouble = testDouble;
        }
    }
}
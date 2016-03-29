using System;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenericTest : System.Attribute
    {
        public GenericTest()
        {
        }

        public GenericTest(TestType testType)
        {
            this.TestType = testType;
        }

        // accessor
        public TestType TestType { get; set; }

        public string Description { get; set; }
    }
}
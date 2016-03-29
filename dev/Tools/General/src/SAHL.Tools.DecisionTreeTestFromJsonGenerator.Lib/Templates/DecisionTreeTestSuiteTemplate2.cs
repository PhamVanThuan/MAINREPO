using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Templates
{
    public partial class DecisionTreeTestSuiteTemplate
    {
        public string Namespace { get; set; }
        public string TestSuiteName { get; set; }
        public List<TestCase> TestCases { get; set; }

        public DecisionTreeTestSuiteTemplate(string nameSpace, string name, List<TestCase> testCases)
        {
            this.Namespace = nameSpace;
            this.TestSuiteName = name;
            this.TestCases = testCases;
        }
    }
}

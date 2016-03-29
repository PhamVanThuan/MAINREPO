using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public class ScenarioResult
    {
        public string ScenarioName { get; set; }
        public List<ITestResult> Results { get; set; }
        public string DecisionTreeName { get; set; }

        public ScenarioResult(string scenarioName, string decisionTreeName)
        {
            this.ScenarioName = scenarioName;
            this.DecisionTreeName = decisionTreeName;
            this.Results = new List<ITestResult>();
        }

        public bool Passed()
        {
            return Results.All(x => x.Passed);
        }
    }
}

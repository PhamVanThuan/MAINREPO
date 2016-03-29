using System;

namespace WorkflowAutomation.Harness
{
    public interface IDataCreationHarness
    {
        /// <summary>
        /// Create the test data for a workflow from the configuration data in 2am
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        bool CreateTestCases(Common.Enums.WorkflowEnum workflow);

        /// <summary>
        /// Create a single test case in the workflow.
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="testCase"></param>
        /// <returns></returns>
        bool CreateSingleTestCase(Common.Enums.WorkflowEnum workflow, Automation.DataModels.TestCase testCase);
    }
}
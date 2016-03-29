using Automation.Framework;
using Automation.Framework.DataAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowAutomation.Harness
{
    public class DataCreationHarness : IDataCreationHarness
    {
        public DataCreationHarness()
        {
            ScriptManager.CreateAutomationScripts();
        }

        public bool CreateTestCases(Common.Enums.WorkflowEnum workflow)
        {
            var testCases = CreateCasesIn2am(workflow);
            var scriptStart = DateTime.Now;
            RunScripts(testCases);
            Debug.WriteLine(string.Format(@"{0}", new TimeSpan(DateTime.Now.Ticks - scriptStart.Ticks)));
            VerifyX2ReturnData(testCases);
            return VerifyEndStates(testCases);
        }

        public bool CreateSingleTestCase(Common.Enums.WorkflowEnum workflow, Automation.DataModels.TestCase testCase)
        {
            var testCases = CreateCasesIn2am(workflow, testCase);
            RunScripts(testCases);
            return VerifyEndStates(testCases);
        }

        private static bool VerifyEndStates(IEnumerable<Automation.DataModels.TestCase> testCases)
        {
            foreach (var tc in testCases)
            {
                var instances = DataHelper.GetInstances(tc.IdentityKey, tc.DataTable, tc.KeyType);
                var instanceAtState = (from i in instances
                                       where i.StateName == tc.ExpectedEndState
                                       select i).FirstOrDefault();
                if (instanceAtState == null)
                    return false;
            }
            return true;
        }

        private static List<Automation.DataModels.TestCase> CreateCasesIn2am(Common.Enums.WorkflowEnum workflow, Automation.DataModels.TestCase testCase = null)
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                var failedCasesMessages = new List<string>();
                List<Automation.DataModels.TestCase> testCases = new List<Automation.DataModels.TestCase>();
                bool caseCreated = false;
                if (testCase != null)
                {
                    //we have been given a case to work with and dont need to fetch them.
                    testCases.Add(testCase);
                }
                else
                {
                    testCases = GetTestCasesForWorkflow(workflow);
                }
                foreach (var tc in testCases)
                {
                    if (tc.IdentityKey == 0)
                    {
                        caseCreated = Create2amTestCaseData(workflow, tc);
                        if (caseCreated)
                        {
                            tc.IdentityKey = GetIdentityKey(workflow, tc);
                        }
                    }
                    else
                    {
                        caseCreated = true;
                    }
                    if (!caseCreated)
                        failedCasesMessages.Add(string.Format("Case creation stored proc failed for Account: {0} when running Script: {1}.", tc.AccountKey, tc.ScriptToRun));
                }
                if (failedCasesMessages.Count > 0)
                    throw new Exception(String.Join(" \n ", failedCasesMessages));
                return testCases;
            }
        }

        private static List<Automation.DataModels.TestCase> GetTestCasesForWorkflow(Common.Enums.WorkflowEnum workflow)
        {
            var methodForTestCaseFetch = typeof(DataHelper).GetMethod(string.Format("Get{0}TestCases", workflow));
            List<Automation.DataModels.TestCase> testCases = methodForTestCaseFetch != null ? (List<Automation.DataModels.TestCase>)methodForTestCaseFetch.Invoke(null, null) : null;
            return testCases;
        }

        private static bool Create2amTestCaseData(Common.Enums.WorkflowEnum workflow, Automation.DataModels.TestCase tc)
        {
            bool caseCreated = false;
            string procToRun = GetSQLProcedureFor2AMDataCreation(workflow);
            var dictionary = new Dictionary<string, string>();
            if (workflow != Common.Enums.WorkflowEnum.PersonalLoans && workflow != Common.Enums.WorkflowEnum.HelpDesk && workflow != Common.Enums.WorkflowEnum.DisabilityClaim)
            {
                dictionary.Add("@AccountKey", tc.AccountKey.ToString());
                caseCreated = DataHelper.ExecuteProcedure(procToRun, dictionary);
            }
            else if (workflow == Common.Enums.WorkflowEnum.DisabilityClaim)
            {
                dictionary.Add("@lifeAccountKey", tc.LifeAccountKey.ToString());
                dictionary.Add("@legalEntityKey", tc.LegalEntityKey.ToString());
                caseCreated = DataHelper.ExecuteProcedure(procToRun, dictionary);
            }
            else if (workflow == Common.Enums.WorkflowEnum.PersonalLoans)
            {
                dictionary.Add("@legalEntityKey", tc.LegalEntityKey.ToString());
                caseCreated = DataHelper.ExecuteProcedure(procToRun, dictionary);
            }
            else if (workflow == Common.Enums.WorkflowEnum.HelpDesk)
            {
                //no 2am data is required for HelpDesk
                caseCreated = true;
            }
            return caseCreated;
        }

        private static int GetIdentityKey(Common.Enums.WorkflowEnum workflow, Automation.DataModels.TestCase tc)
        {
            int identityKey;
            var methodForIdentityKeyFetch = typeof(DataHelper).GetMethod(string.Format("GetIdentityKey{0}", workflow));
            if (workflow != Common.Enums.WorkflowEnum.PersonalLoans && workflow != Common.Enums.WorkflowEnum.HelpDesk && workflow != Common.Enums.WorkflowEnum.DisabilityClaim)
            {
                identityKey = methodForIdentityKeyFetch != null ? (int)methodForIdentityKeyFetch.Invoke(null, new object[] { tc.AccountKey }) : 0;
            }
            else
            {
                identityKey = methodForIdentityKeyFetch != null ? (int)methodForIdentityKeyFetch.Invoke(null, new object[] { tc.LegalEntityKey }) : 0;
            }
            return identityKey;
        }

        private static string GetSQLProcedureFor2AMDataCreation(Common.Enums.WorkflowEnum workflow)
        {
            string sqlProc = string.Empty;
            switch (workflow)
            {
                case Common.Enums.WorkflowEnum.CAP2Offers:
                    sqlProc = "test.CreateCap2Offer";
                    break;

                case Common.Enums.WorkflowEnum.DebtCounselling:
                    sqlProc = "test.CreateDebtCounsellingCase";
                    break;

                case Common.Enums.WorkflowEnum.PersonalLoans:
                    sqlProc = "test.createPersonalLoanOffer";
                    break;

                case Common.Enums.WorkflowEnum.DisabilityClaim:
                    sqlProc = "test.CreateDisabilityClaim";
                    break;

                case Common.Enums.WorkflowEnum.HelpDesk:
                    sqlProc = string.Empty;
                    break;

                default:
                    break;
            }
            return sqlProc;
        }

        private static List<Automation.DataModels.TestCase> RunScripts(List<Automation.DataModels.TestCase> testCases)
        {
            var engine = new ScriptEngine();
            var i = 0;
            foreach (var t in testCases)
            {
                var returnData = engine.ExecuteScript(t.ScriptFile, t.ScriptToRun, t.IdentityKey);
                t.TestCaseResults = returnData;
                i++;
            }
            return testCases;
        }

        private static List<Automation.DataModels.TestCase> RunScriptsAsync(List<Automation.DataModels.TestCase> testCases)
        {
            var engine = new ScriptEngine();
            var i = 0;
            Parallel.ForEach(testCases, testCase =>
            {
                Debug.WriteLine(String.Format("Started case {0} of {1}: IdentityKey={2}, Expected End State = {3}, ScriptFile = {4}", i, testCases.Count, testCase.IdentityKey, testCase.ExpectedEndState, testCase.ScriptFile));
                var returnData = engine.ExecuteScript(testCase.ScriptFile, testCase.ScriptToRun, testCase.IdentityKey);
                testCase.TestCaseResults = returnData;
                i++;
                Debug.WriteLine(String.Format("Completed case {0} of {1}: IdentityKey={2}, Expected End State = {3}", i, testCases.Count, testCase.IdentityKey, testCase.ExpectedEndState));
            }
            );
            return testCases;
        }

        private static void VerifyX2ReturnData(List<Automation.DataModels.TestCase> testCases)
        {
            var returnDataErrors = new List<string>();
            foreach (var t in testCases)
            {
                if (t.TestCaseResults != null)
                {
                    foreach (var keyval in t.TestCaseResults)
                    {
                        if (!keyval.Value.ActivityCompleted)
                        {
                            returnDataErrors.Add(String.Format(@"The Activity that is responsible for moving case to state ""{0}"" failed., Errors:{1}, Domain Messages: {2}",
                                                    t.ExpectedEndState,
                                                    keyval.Value.Error,
                                                    String.Join(" \n ", keyval.Value.X2Messages)));
                        }
                    }
                }
            }
            if (returnDataErrors.Count > 0)
                throw new Exception(String.Join(" \n ", returnDataErrors));
        }
    }
}
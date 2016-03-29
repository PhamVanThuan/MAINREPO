using Common.Models;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public class TestCase
    {
        public int AccountKey { get; set; }

        public int IdentityKey { get; set; }

        public string ExpectedEndState { get; set; }

        public string ScriptToRun { get; set; }

        public string ScriptFile { get; set; }

        public Dictionary<int, WorkflowReturnData> TestCaseResults { get; set; }

        public string DataTable { get; set; }

        public string KeyType { get; set; }

        public int LifeAccountKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int DisabilityClaimKey { get; set; }
    }
}
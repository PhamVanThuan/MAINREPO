using System.Collections.Generic;

namespace Automation.Framework
{
    public class Step
    {
        public int Order { get; set; }

        public string PriorToStart { get; set; }

        public string PostComplete { get; set; }

        public string WorkflowActivity { get; set; }

        public bool IgnoreWarnings { get; set; }

        public string Identity { get; set; }

        public Dictionary<string, string> FieldInputList { get; set; }

        public object Data { get; set; }
    }
}
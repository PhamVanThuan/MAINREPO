using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.DecisionTree.TreeTests.Models
{
    public class OutputMessage : IOutputMessage
    {
        public OutputMessage(string message, string assertion, string severity)
        {
            this.ExpectedMessage = message;
            this.Assertion = assertion;
            this.ExpectedMessageSeverity = severity;
        }

        public string Assertion
        {
            get;
            set;
        }

        public string ExpectedMessage
        {
            get;
            set;
        }

        public string ExpectedMessageSeverity
        {
            get;
            set;
        }
    }
}
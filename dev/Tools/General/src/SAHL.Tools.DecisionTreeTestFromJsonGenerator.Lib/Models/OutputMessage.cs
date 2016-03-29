namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class OutputMessage
    {
        public string Assertion { get; set; }

        public string ExpectedMessage { get; set; }

        public string ExpectedMessageSeverity { get; set; }

        public OutputMessage(string assertion, string expectedMessage, string expectedMessageSeverity)
        {
            this.Assertion = assertion;
            this.ExpectedMessage = expectedMessage;
            this.ExpectedMessageSeverity = expectedMessageSeverity;
        }
    }
}
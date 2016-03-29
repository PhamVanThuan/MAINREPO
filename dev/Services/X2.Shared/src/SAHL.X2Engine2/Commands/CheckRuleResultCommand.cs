using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class CheckRuleResultCommand : ServiceCommand
    {
        public bool Result { get; protected set; }

        public string RuleName { get; protected set; }

        public CheckRuleResultCommand(bool result, string ruleName)
        {
            this.Result = result;
            this.RuleName = ruleName;
        }
    }
}
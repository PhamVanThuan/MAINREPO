using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class RuleCommand : ServiceCommand, IRuleCommand
    {
        public bool Result { get; set; }

        public string Message { get; set; }
    }
}
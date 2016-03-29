using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CheckRuleResultCommandHandler : IServiceCommandHandler<CheckRuleResultCommand>
    {
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(CheckRuleResultCommand command, IServiceRequestMetadata metadata)
        {
            if (!command.Result)
            {
                throw new RuleCommandException(string.Format("Rule:{0} returned false", command.RuleName));
            }
            else
            {
                return new SystemMessageCollection();
            }
        }
    }
}
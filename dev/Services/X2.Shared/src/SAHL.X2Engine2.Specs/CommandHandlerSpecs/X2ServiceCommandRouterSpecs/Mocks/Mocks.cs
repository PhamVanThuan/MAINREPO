using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs.Mocks
{
    public class MockRuleCommand : RuleCommand
    {
    }

    public class MockRuleCommandHandler : IServiceCommandHandler<MockRuleCommand>
    {
        public ISystemMessageCollection HandleCommand(MockRuleCommand command, IServiceRequestMetadata metadata)
        {
            return new SystemMessageCollection();
        }
    }
}

using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Factories
{
    public interface ICommandFactory
    {
        IEnumerable<IServiceCommand> CreateCommands(IX2Request request);
    }
}
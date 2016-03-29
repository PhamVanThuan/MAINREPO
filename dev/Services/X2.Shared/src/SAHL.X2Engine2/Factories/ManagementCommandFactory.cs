using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Factories
{
    public class ManagementCommandFactory : IManagementCommandFactory
    {
        private Dictionary<X2ManagementType, Func<IX2NodeManagementMessage, IEnumerable<IServiceCommand>>> internalCommandFactory;

        public ManagementCommandFactory()
        {
            internalCommandFactory = new Dictionary<X2ManagementType, Func<IX2NodeManagementMessage, IEnumerable<IServiceCommand>>>
            {
                { X2ManagementType.RefreshCache, GetRefreshCache }
            };

        }

        public IEnumerable<IServiceCommand> CreateCommands(IX2NodeManagementMessage message)
        {
            var commandCreator = internalCommandFactory[message.ManagementType];
            return commandCreator(message);
        }

        public IEnumerable<IServiceCommand> GetRefreshCache(IX2NodeManagementMessage message)
        {
            List<IServiceCommand> commands = new List<IServiceCommand>
            {
                new RefreshCacheCommand(message.Data)
            };

            return commands;
        }

    }
}

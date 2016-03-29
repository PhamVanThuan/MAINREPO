using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.Common
{
    public class ClearCacheCommandHandler : IHandlesDomainServiceCommand<ClearCacheCommand>
    {
        private ICommandHandler commandHandler;

        public ClearCacheCommandHandler(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public void Handle(IDomainMessageCollection messages, ClearCacheCommand command)
        {
            string[] cacheData = command.Data.ToString().Split(',');
            var cacheType = (CacheTypes)Enum.Parse(typeof(CacheTypes), cacheData[0], true);

            switch (cacheType)
            {
                case CacheTypes.UIStatement:
                    this.commandHandler.HandleCommand<RefreshUIStatementCacheCommand>(messages, new RefreshUIStatementCacheCommand());
                    break;

                case CacheTypes.Lookups:
                    this.commandHandler.HandleCommand<RefreshAllLookupsCommand>(messages, new RefreshAllLookupsCommand());
                    break;

                case CacheTypes.LookupItem:
                    var lookupTable = cacheData[1];
                    this.commandHandler.HandleCommand<RefreshLookupItemCommand>(messages, new RefreshLookupItemCommand(lookupTable));
                    break;

                case CacheTypes.DomainService:
                    this.commandHandler.HandleCommand<RefreshUIStatementCacheCommand>(messages, new RefreshUIStatementCacheCommand());
                    this.commandHandler.HandleCommand<RefreshAllLookupsCommand>(messages, new RefreshAllLookupsCommand());
                    this.commandHandler.HandleCommand<RefreshDSCacheCommand>(messages, new RefreshDSCacheCommand());
                    break;

                default:
                    break;
            }
        }
    }
}

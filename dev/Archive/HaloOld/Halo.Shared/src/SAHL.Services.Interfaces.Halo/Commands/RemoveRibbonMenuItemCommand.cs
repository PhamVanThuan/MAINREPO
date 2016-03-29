using SAHL.Core.BusinessModel;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class RemoveRibbonMenuItemCommand : ServiceCommand, IHaloServiceCommand
    {
        public RemoveRibbonMenuItemCommand(string userName, string context, long businessKey, string businessKeyType)
        {
            this.UserName = userName;
            this.BusinessKey = businessKey;
            this.BusinessKeyType = (BusinessKeyType)Enum.Parse(typeof(BusinessKeyType), businessKeyType);
            this.Context = context;
        }

        public string UserName { get; protected set; }

        public string Context { get; protected set; }

        public long BusinessKey { get; protected set; }

        public BusinessKeyType BusinessKeyType { get; protected set; }
    }
}
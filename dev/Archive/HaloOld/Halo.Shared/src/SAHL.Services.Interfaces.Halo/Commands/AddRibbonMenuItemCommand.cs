using SAHL.Core.BusinessModel;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class AddRibbonMenuItemCommand : ServiceCommand, IServiceCommandWithReturnedData<BusinessContext>, IHaloServiceCommand
    {
        public AddRibbonMenuItemCommand(string userName, long businessKey, BusinessKeyType businessKeyType, string displayName, string url, string context)
        {
            this.UserName = userName;
            this.BusinessKey = businessKey;
            this.BusinessKeyType = businessKeyType;
            this.DisplayName = displayName;
            this.Url = url;
            this.Context = context;
        }

        public string UserName { get; protected set; }

        public long BusinessKey { get; protected set; }

        public string DisplayName { get; protected set; }

        public string Url { get; protected set; }

        public string Context { get; protected set; }

        public BusinessKeyType BusinessKeyType { get; protected set; }

        public BusinessContext Result { get; set; }
    }
}
using SAHL.Core.Identity;
using System;

namespace SAHL.Core.X2.Messages.Management
{
    public class X2NodeManagementMessage : IX2NodeManagementMessage
    {
        public X2NodeManagementMessage(X2ManagementType managementType, Object data)
        {
            this.ManagementType = managementType;
            this.Id = CombGuid.Instance.Generate();
            this.Data = data;
        }

        public Object Data { get; private set; }

        public Guid Id
        {
            get;
            protected set;
        }

        public X2Response Result
        {
            get;
            set;
        }

        public X2ManagementType ManagementType
        {
            get;
            protected set;
        }
    }
}
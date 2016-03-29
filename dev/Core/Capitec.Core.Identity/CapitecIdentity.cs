using System;
using System.Security.Principal;

namespace Capitec.Core.Identity
{
    public class CapitecIdentity : IIdentity
    {
        public CapitecIdentity(bool isAuthenticated, string name, Guid userId, string displayName)
        {
            this.IsAuthenticated = isAuthenticated;
            this.Name = name;
            this.UserId = userId;
            this.DisplayName = displayName;
            this.AuthenticationType = "CapitecAuth";
        }

        public string AuthenticationType
        {
            get;
            protected set;
        }

        public bool IsAuthenticated
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public Guid UserId
        {
            get;
            protected set;
        }

        public string DisplayName
        {
            get;
            protected set;
        }
    }
}
using SAHL.Core.Identity;
using SAHL.Core.Services.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.Web.Services
{
    public class HttpCommandAuthoriser : IHttpCommandAuthoriser
    {
        private ConcurrentDictionary<Type, AuthAttributeData> commandsToAuthenticate;
        private IHostContext hostContext;

        public HttpCommandAuthoriser(IHostContext hostContext)
        {
            this.hostContext = hostContext;
            this.commandsToAuthenticate = new ConcurrentDictionary<Type, AuthAttributeData>();
        }

        public AuthToken AuthoriseCommand(SAHL.Core.Services.IServiceCommand command)
        {
            
            Type commandType = command.GetType();
            if (!this.commandsToAuthenticate.ContainsKey(commandType))
            {
                AuthorisedCommandAttribute attr = commandType.GetCustomAttributes(typeof(AuthorisedCommandAttribute), false).OfType<AuthorisedCommandAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    this.commandsToAuthenticate.TryAdd(commandType, new AuthAttributeData(true, attr.SplitRoles));
                }
                else
                {
                    this.commandsToAuthenticate.TryAdd(commandType, new AuthAttributeData(false, new string[] { }));
                }
            }

            AuthAttributeData authData = this.commandsToAuthenticate[commandType];
            var authToken = new AuthToken();

            if (authData.RequiresAuthorisation)
            {
                IPrincipal principal = this.hostContext.GetUser();
                if (!principal.Identity.IsAuthenticated)
                {
                    authToken.Authenticated = false;
                }

                if (authData.Roles != null && authData.Roles.Length > 0)
                {
                    authToken.Authorised = authData.Roles.Any(principal.IsInRole);
                }
            }

            return authToken;
        }

        public class AuthAttributeData
        {
            public AuthAttributeData(bool requiresAuthorisation, string[] roles)
            {
                this.Roles = roles;
                this.RequiresAuthorisation = requiresAuthorisation;
            }

            public string[] Roles { get; protected set; }

            public bool RequiresAuthorisation { get; protected set; }
        }

        public class AuthToken
        {
            public AuthToken()
            {
                this.Authenticated = true;
                this.Authorised = true;
            }

            public bool Authenticated { get; set; }

            public bool Authorised { get; set; }

            public bool IsAuthorised()
            {
                return Authenticated && Authorised;
            }
        }
    }
}
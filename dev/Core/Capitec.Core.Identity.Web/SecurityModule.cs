using System.Diagnostics;
using StructureMap;
using System;
using System.Linq;
using System.Web;

namespace Capitec.Core.Identity.Web
{
    public class SecurityModule : IHttpModule, ISecurityModule
    {
        private const string capitecAuthHeader = "CAPITEC-AUTH";

        // DO NOT MAKE THIS A PRIVATE SETTER BECAUSE SONARQUBE SAYS SO, StructureMap is setting this property on startup
        public IAuthenticationManager AuthenticationManager { get; set; }

        public void Dispose()
        {
            this.AuthenticationManager = null;
        }

        public void Init(HttpApplication context)
        {
            ObjectFactory.BuildUp(this);
            context.AuthenticateRequest += AuthenticateRequestOnContext;
        }

        private void AuthenticateRequestOnContext(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            if (context.User != null)
            {
                return;
            }

            // look for our custom auth header
            if (!context.Request.Headers.AllKeys.Contains(capitecAuthHeader, StringComparer.OrdinalIgnoreCase))
            {
                return;
            }
            // get the value and compare it to the users token
            var authenticationToken = context.Request.Headers[capitecAuthHeader];

            if (authenticationToken.Length > 0)
            {
                this.AuthenticationManager.Authenticate(Guid.Parse(authenticationToken));
            }
        }
    }
}
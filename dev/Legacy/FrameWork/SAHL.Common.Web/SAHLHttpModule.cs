using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Web;

using SAHL.Common.Authentication;
using SAHL.Common.BusinessModel;
using SAHL.Common.Security;
using SAHL.Common.CacheData;
using SAHL.Common.Logging;

namespace SAHL.Common.Web
{
    /// <summary>
    /// Custom HTTPModule implementation.  This allows us to convert the standard principal into 
    /// a custom <see cref="SAHLPrincipal"/> type, on which we can store (and cache) relevant information.
    /// </summary>
    public class SAHLHttpModule : IHttpModule
    {

        /// <summary>
        /// IHttpModule implementation.
        /// </summary>
        /// <param name="application"></param>
        public void Init(HttpApplication application)
        {
            application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
            application.EndRequest += application_EndRequest;
        }

        void application_EndRequest(object sender, EventArgs e)
        {
            Logger.ThreadContext.Clear();
            Metrics.ThreadContext.Clear();
        }

        void application_AuthenticateRequest(object sender, EventArgs e)
        {
            SAHLHttpApplication application = (SAHLHttpApplication)sender;
            HttpContext context = application.Context;
            HttpRequest request = context.Request;

            if (request.IsAuthenticated)
            {
                // convert the standard principal into a SAHLPrincipal
                IIdentity identity = context.User.Identity;
                SAHLPrincipal principal = new SAHLPrincipal(identity);
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
                context.User = principal;

                // assign the aduser
                // principal.ADUser = spc.GetADUser(principal);
                HttpContext.Current.User = principal;
            }
            else
            {
                HttpContext.Current.User = new SAHLPrincipal(WindowsIdentity.GetCurrent());
            }


        }

        public void Dispose() { }
    }
}

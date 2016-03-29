using System;
using System.Web;
using System.Linq;
using System.Security.Principal;

using SAHL.Core.Identity;

namespace SAHL.Core.Web.Identity
{
    public class HttpHostContext : IHostContext
    {
        public virtual void IssueSecurityToken(Guid token)
        {
            // Intentionally left blank - No implementation required 
        }

        public void SetUser(IIdentity userIdentity, string[] roles)
        {
            HttpContext.Current.User = new GenericPrincipal(userIdentity, roles);
        }

        public virtual void RevokeSecurityToken()
        {
            // Intentionally left blank - No implementation required 
        }

        public IPrincipal GetUser()
        {
            return HttpContext.Current == null ? null : HttpContext.Current.User;
        }

        public string GetContextValue(string contextValueKey, string keyPrefix)
        {
            return HttpContext.Current.Request.Headers[keyPrefix + contextValueKey] ?? null;
        }

        public string[] GetKeysWithPrefix(string keyPrefix)
        {
            return HttpContext.Current != null 
                        ? HttpContext.Current.Request.Headers.Keys.OfType<string>().Where(x => x.StartsWith(keyPrefix)).Select(x => x.Replace(keyPrefix, "")).ToArray() 
                        : new string[] { };
        }

        public string[] GetKeys()
        {
            return HttpContext.Current != null 
                        ? HttpContext.Current.Request.Headers.Keys.OfType<string>().ToArray() 
                        : new string[] { };
        }

        public virtual string GetApplicationPath()
        {
            return HttpContext.Current != null
                ? HttpContext.Current.Request.ApplicationPath
                : null;
        }

        public virtual Uri GetCurrentRequestUrl()
        {
            return HttpContext.Current != null
                ? HttpContext.Current.Request.Url
                : null;
        }
    }
}

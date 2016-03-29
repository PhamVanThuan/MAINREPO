using SAHL.Core.Identity;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Capitec.Core.Identity.Web
{
    public class HttpHostContext : IHostContext
    {
        public void IssueSecurityToken(Guid token)
        {
            HttpContext.Current.Response.AddHeader("CAPITEC-AUTH", token.ToString());
        }

        public void SetUser(IIdentity userIdentity, string[] roles)
        {
            HttpContext.Current.User = new GenericPrincipal(userIdentity, roles);
        }

        public void RevokeSecurityToken()
        {
            HttpContext.Current.Response.AddHeader("CAPITEC-AUTH", "");
        }

        public IPrincipal GetUser()
        {
            return HttpContext.Current == null ? null : HttpContext.Current.User;
        }

        public string GetContextValue(string contextValueKey, string keyPrefix)
        {
            if (HttpContext.Current.Request.Headers[keyPrefix + contextValueKey] != null)
            {
                return HttpContext.Current.Request.Headers[keyPrefix + contextValueKey].ToString();
            }
            return null;
        }

        public string[] GetKeysWithPrefix(string keyPrefix)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.Headers.Keys.OfType<string>().Where(x => x.StartsWith(keyPrefix)).Select(x => x.Replace(keyPrefix, "")).ToArray();
            }
            else
            {
                return new string[] { };
            }
        }

        public string[] GetKeys()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.Headers.Keys.OfType<string>().ToArray();
            }
            else
            {
                return new string[] { };
            }
        }
    }
}
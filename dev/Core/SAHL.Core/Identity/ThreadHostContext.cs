using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace SAHL.Core.Identity
{
    public class ThreadHostContext : IHostContext
    {
        private static readonly ThreadLocal<Dictionary<string, string>> localData = new ThreadLocal<Dictionary<string, string>>(() => new Dictionary<string, string>());

        /// <summary>
        /// Initialise the context
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <param name="roles"></param>
        /// <param name="contextValues">Any Dictionary[string, string] or list of key/value pairs</param>
        public void Initialise(IIdentity userIdentity, string[] roles, IEnumerable<KeyValuePair<string, string>> contextValues)
        {
            SetUser(userIdentity, roles);

            if (contextValues == null)
            {
                return;
            }

            foreach (var pair in contextValues)
            {
                localData.Value.Add(pair.Key, pair.Value);
            }
        }

        public void IssueSecurityToken(Guid token)
        {
            //left empty on purpose, as this is not meant for token based authentication
        }

        public void SetUser(IIdentity userIdentity, string[] roles)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(userIdentity, roles);
        }

        public IPrincipal GetUser()
        {
            return Thread.CurrentPrincipal;
        }

        public void RevokeSecurityToken()
        {
            //left empty on purpose, as this is not meant for token based authentication
        }

        public string GetContextValue(string contextValueKey)
        {
            throw new NotImplementedException();
        }

        public string GetContextValue(string contextValueKey, string keyPrefix)
        {
            return localData.Value.ContainsKey(keyPrefix + contextValueKey) ? localData.Value[keyPrefix + contextValueKey] : null;
        }

        public string[] GetKeysWithPrefix(string keyPrefix)
        {
            return GetAllKeys().Where(x => x.StartsWith(keyPrefix)).ToArray();
        }

        public string[] GetKeys()
        {
            return GetAllKeys();
        }

        private string[] GetAllKeys()
        {
            string[] keys = new string[localData.Value.Keys.Count];
            localData.Value.Keys.CopyTo(keys, 0);
            return keys;
        }
    }
}
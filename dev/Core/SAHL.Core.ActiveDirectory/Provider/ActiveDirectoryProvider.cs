using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Query;
using SAHL.Core.ActiveDirectory.Query.Results;
using SAHL.Core.Roles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Provider
{
    public class ActiveDirectoryProvider : IActiveDirectoryProvider, IRoleProvider
    {
        protected IActiveDirectoryQueryFactory QueryFactory { get; set; }

        protected IActiveDirectoryProviderCache ActiveDirectoryProviderCache { get; set; }

        protected ICredentials Credentials { get; set; }

        public ActiveDirectoryProvider(IActiveDirectoryQueryFactory queryFactory, IActiveDirectoryProviderCache activeDirectoryProviderCache, ICredentials credentials)
        {
            QueryFactory = queryFactory;
            ActiveDirectoryProviderCache = activeDirectoryProviderCache;
            Credentials = credentials;
        }

        public virtual SecurityIdentifier GetObjectSid(string distinguisedName)
        {
            if (string.IsNullOrWhiteSpace(distinguisedName))
            {
                return null;
            }

            SecurityIdentifier identifier;
            var found = ActiveDirectoryProviderCache.ObjectSidsByDistinguishedName.TryGetValue(distinguisedName, out identifier);
            if (found)
            {
                return identifier;
            }

            identifier = GetObjectSidViaQuery(distinguisedName);

            if (identifier != null)
            {
                ActiveDirectoryProviderCache.ObjectSidsByDistinguishedName.TryAdd(distinguisedName, identifier); //if false, then someone else probably added it after we checked the cache
            }

            return identifier;
        }

        protected virtual SecurityIdentifier GetObjectSidViaQuery(string distinguisedName)
        {
            using (var query = QueryFactory.Create(Credentials, string.Format(SearchTerms.DistinguishedName, distinguisedName)))
            {
                var searchResults = query.FindAll(Properties.ObjectSid);
                if (searchResults == null || searchResults.Count == 0)
                {
                    return null;
                }

                if (searchResults.Count > 1)
                {
                    ProcessAmbiguous(Properties.DistinguishedName, distinguisedName, searchResults);
                }

                var allValues = GetAllValues(searchResults);
                if (allValues == null || allValues.Count <= 0)
                {
                    return null;
                }

                var objectSidBytes = (byte[])GetObjectSidPropertyValues(allValues)[0]; //should only ever be 1
                return new SecurityIdentifier(objectSidBytes, 0);
            }
        }

        /// <summary>
        /// Helper wrapper to retrieve only the CommonName(role) from GetMemberOfInfo
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetRoles(string username)
        {
            return GetMemberOfInfo(username, (distinguishedName, securityIdentifier) => new MemberOfInfo(distinguishedName, securityIdentifier).CommonName, false);
        }

        public virtual IEnumerable<IMemberOfInfo> GetMemberOfInfo(string username, bool includeSecurityIdentifier = false)
        {
            return GetMemberOfInfo(username, (distinguishedName, securityIdentifier) => new MemberOfInfo(distinguishedName, securityIdentifier), includeSecurityIdentifier);
        }

        /// <summary>
        /// Returns the 'memberOf' property for the specified username
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="username"></param>
        /// <param name="resultTransformer">Results are retrieved as strings, the use resultTransformer to provide a function to transform the string into the result you require</param>
        /// <param name="includeSecurityIdentifier">Attempt to resolve the objectSid by checking the local cache, and failing that, by querying Active Directory</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetMemberOfInfo<T>(string username, Func<string, SecurityIdentifier, T> resultTransformer, bool includeSecurityIdentifier)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                yield break;
            }

            var usernameWithoutPrefix = UsernameHelpers.RemoveDomainPrefixIfAny(username);

            using (var query = QueryFactory.Create(Credentials, string.Format(SearchTerms.SamAccountName, usernameWithoutPrefix)))
            {
                var searchResults = query.FindAll(Properties.MemberOf);
                if (searchResults == null || searchResults.Count == 0)
                {
                    yield break;
                }

                if (searchResults.Count > 1)
                {
                    //this should never execute, as long as we're providing the username from the IIdentity.Name
                    ProcessAmbiguous(Properties.SamAccountName, usernameWithoutPrefix, searchResults);
                }

                var allValues = GetAllValues(searchResults);
                if (allValues == null)
                {
                    yield break;
                }

                var values = GetMemberOfPropertyValues(allValues);

                foreach (var result in values)
                {
                    var resultString = result.ToString();
                    SecurityIdentifier identifier = null;
                    if (includeSecurityIdentifier)
                    {
                        identifier = GetObjectSid(resultString);
                    }
                    yield return resultTransformer(resultString, identifier);
                }
            }
        }

        private static ResultPropertyValueCollection GetObjectSidPropertyValues(ResultPropertyCollection rawValues)
        {
            var values = (ResultPropertyValueCollection)rawValues
                .OfType<DictionaryEntry>()
                .Single(a => a.Key.ToString().Equals(Properties.ObjectSid, StringComparison.OrdinalIgnoreCase))
                .Value;
            return values;
        }

        private static ResultPropertyValueCollection GetMemberOfPropertyValues(ResultPropertyCollection rawValues)
        {
            var values = (ResultPropertyValueCollection)rawValues
                .OfType<DictionaryEntry>()
                .Single(a => a.Key.ToString().Equals(Properties.MemberOf, StringComparison.OrdinalIgnoreCase))
                .Value;
            return values;
        }

        private static ResultPropertyCollection GetAllValues(SearchResultCollection propertyValue)
        {
            var rawValues = propertyValue
                .OfType<SearchResult>()
                .Single()
                .Properties;
            return rawValues;
        }

        private static void ProcessAmbiguous(string parameterName, string parameterValue, SearchResultCollection propertyValue)
        {
            throw new InvalidOperationException(
                string.Format(
                    "The {0} that was provided ({1}) is ambiguous, (matches {2} results). Please provide a more specific {0}."
                    , string.IsNullOrWhiteSpace(parameterName) ? "parameter" : parameterName
                    , parameterValue ?? "null"
                    , propertyValue.Count));
        }
    }
}
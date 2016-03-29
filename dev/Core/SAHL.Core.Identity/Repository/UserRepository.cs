using SAHL.Core.Data;
using SAHL.Core.Identity.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SAHL.Core.Identity.Tests")]

namespace SAHL.Core.Identity
{
    public class UserRepository : IUserRepository
    {
        private const string LdapAdBaseQuery = "LDAP://DC=SAHL,DC=com";
        private const string LdapUserFilter = "(&(SAMAccountName={0}))";
        private IDbFactory dbFactory;

        public UserRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IUserDetails ADFindUser(IUserDetails userDetails)
        {
            var concreteDetails = userDetails as UserDetails;
            if (concreteDetails == null)
            {
                throw new ArgumentException("User Details not of expected UserDetails type");
            }

            var directoryEntry = new DirectoryEntry(LdapAdBaseQuery, "sahl\\Halouser", "Natal123");
            var directorySearcher = new DirectorySearcher(directoryEntry);

            directorySearcher.PropertiesToLoad.Add("displayname");
            directorySearcher.PropertiesToLoad.Add("mail");
            directorySearcher.PropertiesToLoad.Add("thumbnailPhoto");

            var adUserName = this.GetUserNameWithoutDomain(userDetails.FullADUsername);
            directorySearcher.Filter = string.Format(LdapUserFilter, adUserName);
            var userData = directorySearcher.FindOne();
            if (userData == null) { return null; }

            concreteDetails.DisplayName = userData.Properties["displayname"].Count > 0
                                                        ? userData.Properties["displayname"][0].ToString()
                                                        : string.Empty;
            concreteDetails.EmailAddress = userData.Properties["mail"].Count > 0
                                                        ? userData.Properties["mail"][0].ToString()
                                                        : string.Empty;
            concreteDetails.UserPhoto = userData.Properties["thumbnailPhoto"].Count > 0
                                                        ? userData.Properties["thumbnailPhoto"][0] as byte[]
                                                        : null;
            concreteDetails.UserName = adUserName;

            int domainSeperatorIndex = userDetails.FullADUsername.IndexOf("\\");
            concreteDetails.Domain = domainSeperatorIndex > 0 ? userDetails.FullADUsername.Substring(0, domainSeperatorIndex) : "";

            return concreteDetails;
        }

        public IUserDetails FindUserRoles(IUserDetails userDetails)
        {
            var concreteDetails = userDetails as UserDetails;
            if (concreteDetails == null) { throw new ArgumentException("User Details not of expected UserDetails type"); }

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var getUserRoleStatement = new GetUserRoleStatement(userDetails.FullADUsername);
                concreteDetails.UserRoles = db.Select<UserRole>(getUserRoleStatement);
            }

            return concreteDetails;
        }

        private string GetUserNameWithoutDomain(string userName)
        {
            if (!userName.Contains("\\"))
            {
                return userName;
            }
            var start = userName.LastIndexOf("\\");
            var adName = userName.Substring(start + 1, userName.Length - start - 1);
            return adName;
        }

        public IEnumerable<string> GetUserCapabilitiesForOrganisationStructure(int userOrganisationStructureKey)
        {
            IEnumerable<string> capabilites = new List<string>();
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var getUserCapabilitiesStatement = new GetUserCapabilitiesForOrganisationStructure(userOrganisationStructureKey);
                capabilites = db.Select<string>(getUserCapabilitiesStatement);
            }
            return capabilites;
        }


        public IEnumerable<OrganisationStructureCapability> GetUserCapabilities(string username)
        {
            IEnumerable<OrganisationStructureCapability> capabilites = null;
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var getUserCapabilitiesStatement = new GetUserCapabilitiesStatement(username);
                capabilites = db.Select<OrganisationStructureCapability>(getUserCapabilitiesStatement);
            }
            return capabilites;
        }
    }
}
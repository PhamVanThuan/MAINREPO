using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;
using System.DirectoryServices;
using System.Security.Principal;

namespace SAHL.Services.DecisionTreeDesign.QueryHandlers
{
    public class GetAuthenticatedUserDetailsQueryHandler : IServiceQueryHandler<GetAuthenticatedUserDetailsQuery>
    {
        private const string LdapAdBaseQuery = "LDAP://DC=SAHL,DC=com";
        private const string LdapUserFilter = "(&(SAMAccountName={0}))";
        private IHostContext hostContext;

        public GetAuthenticatedUserDetailsQueryHandler(IHostContext hostContext)
        {
            this.hostContext = hostContext;
        }

        public ISystemMessageCollection HandleQuery(GetAuthenticatedUserDetailsQuery query)
        {
            ISystemMessageCollection messges = SystemMessageCollection.Empty();

            IPrincipal authenticatedUser = this.hostContext.GetUser();

            var username = "";
            var displayName = "";
            var emailAddress = "";
            bool superUser = false;
            if (authenticatedUser.Identity is WindowsIdentity)
            {
                var directoryEntry = new DirectoryEntry(LdapAdBaseQuery, "sahl\\Halouser", "Natal123");
                var directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.PropertiesToLoad.Add("displayname");
                directorySearcher.PropertiesToLoad.Add("mail");

                int start = authenticatedUser.Identity.Name.LastIndexOf("\\");
                username = authenticatedUser.Identity.Name.Substring(start + 1, authenticatedUser.Identity.Name.Length - start - 1);

                directorySearcher.Filter = string.Format(LdapUserFilter, username);
                var userData = directorySearcher.FindOne();

                displayName = userData.Properties["displayname"][0].ToString();
                emailAddress = userData.Properties["mail"][0].ToString();
                
                WindowsIdentity identity = authenticatedUser.Identity as WindowsIdentity;
                foreach (var groupId in identity.Groups)
                {
                    try
                    {
                        var groupName = groupId.Translate(typeof(NTAccount));
                        if (groupName.ToString() == "SAHL\\IT Developers")
                        {
                            superUser = true;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        //do nothing
                    }
                }
            }

            GetAuthenticatedUserDetailsResult authenticatedUserDetails = new GetAuthenticatedUserDetailsResult(username, displayName, emailAddress.ToLower(),superUser);

            query.Result = new ServiceQueryResult<GetAuthenticatedUserDetailsResult>(new GetAuthenticatedUserDetailsResult[] { authenticatedUserDetails });

            return messges;
        }
    }
}
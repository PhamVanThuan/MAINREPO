using Automation.DataModels;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Services.Contracts;
using Automation.DataAccess.DataHelper;
using System.Reflection;
using Common.Constants;
using NUnit.Framework;
using BuildingBlocks.Timers;
using System.DirectoryServices.AccountManagement;

namespace BuildingBlocks.Services
{
    public sealed class ActiveDirectoryService : _2AMDataHelper, IActiveDirectoryService
    {
        public List<string> GetAllLockedTestUsers()
        {
            var usersLocked = new List<string>();
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "SAHL");
            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, "Domain Users");

            if (grp == null)
            {
                throw new ApplicationException("We did not find that group in that domain, perhaps the group resides in a different domain?");
            }

            IList<string> members = new List<String>();

            var memebers = grp.GetMembers(true);
            foreach (Principal p in memebers)
            {
                var isTestUser = typeof(TestUsers).GetFields()
                                    .Select(x => x.GetValue(new TestUsers()).ToString().Replace(@"SAHL\", ""))
                                    .Where(x => x == p.Name).Count() > 0;
                if (isTestUser)
                {
                    var user = UserPrincipal.FindByIdentity(p.Context, IdentityType.UserPrincipalName, p.UserPrincipalName);
                    if (user.IsAccountLockedOut())
                        usersLocked.Add(p.Name); //You can add more attributes, samaccountname, UPN, DN, object type, etc... 
                    user.Dispose();
                }
            }
            grp.Dispose();
            ctx.Dispose();

            return usersLocked;
        }
    }
}
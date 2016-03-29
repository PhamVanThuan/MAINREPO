using SAHL.Core.Roles;
using System.Collections.Generic;

namespace Capitec.Core.Identity
{
    public class RoleProvider : IRoleProvider
    {
        private IUserDataManager DataManager { get; set; }

        public RoleProvider(IUserDataManager dataManager)
        {
            DataManager = dataManager;
        }

        public IEnumerable<string> GetRoles(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                yield break;
            }
            var userDetails = DataManager.GetUserFromUsername(username);
            if (userDetails == null)
            {
                yield break;
            }
            foreach (var item in DataManager.GetRolesFromUser(userDetails.Id))
            {
                yield return item.Name;
            }
        }
    }
}
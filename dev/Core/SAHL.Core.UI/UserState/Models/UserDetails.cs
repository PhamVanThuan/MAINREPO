using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SAHL.Core.UI.UserState.Models
{
    public class UserDetails : IUserDetails, IUserPrincipal
    {
        private List<UserRole> userRoles;

        public UserDetails(string adUserName, string displayName, string emailAddress, Image userPhoto)
        {
            this.DisplayName = displayName;
            this.EmailAddress = emailAddress;
            this.UserPhoto = userPhoto;
            this.AdUserName = adUserName;
            this.userRoles = new List<UserRole>();
        }

        public string AdUserName { get; protected set; }

        public string DisplayName { get; protected set; }

        public string EmailAddress { get; protected set; }

        public Image UserPhoto { get; protected set; }

        public IEnumerable<UserRole> UserRoles { get { return this.userRoles; } }

        public void UpdatePhoto(Image photoToUpdate)
        {
            this.UserPhoto = photoToUpdate;
        }

        public void AddUserRole(string organisationArea, string roleName, string url)
        {
            this.userRoles.Add(new UserRole(organisationArea, roleName, url));
            if (this.userRoles.Count == 1)
            {
                this.ActiveRole = this.userRoles[0];
            }
        }

        public void ChangeActiveRole(string organisationArea, string roleName)
        {
            // first check that the role we are changing to exists in the users profile
            UserRole userRole = this.userRoles.Where(x => x.OrganisationArea.Equals(organisationArea, StringComparison.CurrentCultureIgnoreCase)
                                                       && x.RoleName.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();
            if (userRole != null)
            {
                this.ActiveRole = userRole;
            }
        }

        [JsonProperty]
        public UserRole ActiveRole { get; protected set; }
    }
}
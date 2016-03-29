namespace SAHL.Core.UI.Data.Models
{
    public class UserRole
    {
        public UserRole(string organisationArea, string roleName)
        {
            this.OrganisationArea = organisationArea;
            this.RoleName = roleName;
        }

        public string OrganisationArea { get; protected set; }

        public string RoleName { get; protected set; }
    }
}
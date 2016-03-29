using SAHL.Core.UI.Elements;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.UserState.Models
{
    public class UserRole : IUrlElement
    {
        public UserRole(string organisationArea, string roleName, string url)
        {
            this.OrganisationArea = organisationArea;
            this.RoleName = roleName;
            this.UrlAction = UrlAction.LinkNavigation;
            this.Url = url;
        }

        public string OrganisationArea { get; protected set; }

        public string RoleName { get; protected set; }

        public string Url { get; protected set; }

        public UrlAction UrlAction { get; protected set; }
    }
}
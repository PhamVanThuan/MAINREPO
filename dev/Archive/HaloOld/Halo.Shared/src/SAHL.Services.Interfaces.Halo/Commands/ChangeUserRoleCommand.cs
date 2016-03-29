using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class ChangeUserRoleCommand : ServiceCommand, IHaloServiceCommand
    {
        public ChangeUserRoleCommand(string userName, string organisationArea, string roleName)
        {
            this.UserName = userName;
            this.OrganisationArea = organisationArea;
            this.RoleName = roleName;
        }

        public string UserName { get; set; }

        public string OrganisationArea { get; set; }

        public string RoleName { get; set; }
    }
}
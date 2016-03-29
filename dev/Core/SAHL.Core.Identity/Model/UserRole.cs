namespace SAHL.Core.Identity
{
    public class UserRole : IUserRole
    {
        public int? UserOrganisationStructureKey { get; set; }

        public string OrganisationArea { get; set; }

        public string RoleName { get; set; }
    }
}
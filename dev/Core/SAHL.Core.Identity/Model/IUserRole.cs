namespace SAHL.Core.Identity
{
    public interface IUserRole
    {
        int? UserOrganisationStructureKey { get; }

        string OrganisationArea { get; }

        string RoleName { get; }
    }
}
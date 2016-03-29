namespace SAHL.Core.UI.UserState.Models
{
    public interface IUserPrincipal
    {
        string AdUserName { get; }

        string EmailAddress { get; }

        UserRole ActiveRole { get; }
    }
}
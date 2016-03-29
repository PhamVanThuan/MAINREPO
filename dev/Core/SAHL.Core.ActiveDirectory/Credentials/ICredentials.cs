namespace SAHL.Core.ActiveDirectory.Credentials
{
    public interface ICredentials
    {
        string Username { get; }

        string Password { get; }
    }
}
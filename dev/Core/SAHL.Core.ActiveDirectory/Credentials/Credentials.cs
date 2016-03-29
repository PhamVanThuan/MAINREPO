namespace SAHL.Core.ActiveDirectory.Credentials
{
    public class Credentials : ICredentials
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
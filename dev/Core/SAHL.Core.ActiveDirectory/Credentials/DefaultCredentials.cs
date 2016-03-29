namespace SAHL.Core.ActiveDirectory.Credentials
{
    public class DefaultCredentials : ICredentials
    {
        public string Username { get { return "sahl\\halouser"; } }

        public string Password { get { return "Natal123"; } }
    }
}
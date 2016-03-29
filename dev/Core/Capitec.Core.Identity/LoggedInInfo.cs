namespace Capitec.Core.Identity
{
    public class LoggedInInfo
    {
        public LoggedInInfo(string userId, string token, string username, string displayName, string[] roles)
        {
            this.UserId = userId;
            this.Token = token;
            this.Username = username;
            this.DisplayName = displayName;
            this.Roles = roles;
        }

        public string UserId { get; protected set; }

        public string Token { get; protected set; }

        public string Username { get; protected set; }

        public string DisplayName { get; protected set; }

        public string[] Roles { get; protected set; }
    }
}
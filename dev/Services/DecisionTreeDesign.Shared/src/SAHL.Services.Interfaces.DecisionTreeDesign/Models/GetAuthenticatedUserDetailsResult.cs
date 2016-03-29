namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetAuthenticatedUserDetailsResult
    {
        public GetAuthenticatedUserDetailsResult(string username, string displayName, string emailAddress, bool isSuperUser)
        {
            this.Username = username;
            this.DisplayName = displayName;
            this.EmailAddress = emailAddress;
            this.IsSuperUser = isSuperUser;
        }

        public string Username { get; protected set; }

        public string DisplayName { get; protected set; }

        public string EmailAddress { get; protected set; }

        public bool IsSuperUser { get; protected set; }
    }
}
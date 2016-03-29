using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetUserFromAuthTokenQueryResult
    {
        public GetUserFromAuthTokenQueryResult(Guid id, string username, string firstName, string lastName, string roles)
        {
            this.Id = id;
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Roles = roles;
        }

        public Guid Id { get; protected set; }

        public string Username { get; protected set; }

        public string FirstName { get; protected set; }

        public string LastName { get; protected set; }

        public string Roles { get; protected set; }

        public string[] GetRoles()
        {
            return this.Roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
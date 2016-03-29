using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetUserQueryResult
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public bool IsActive { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BranchName { get; set; }

        public Guid BranchId { get; set; }

        public string Roles { get; set; }
    }
}
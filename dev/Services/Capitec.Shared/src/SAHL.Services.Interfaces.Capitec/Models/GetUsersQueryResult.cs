using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetUsersQueryResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string IsActive { get; set; }

        public string BranchName { get; set; }

        public string LastActivity { get; set; }
    }
}
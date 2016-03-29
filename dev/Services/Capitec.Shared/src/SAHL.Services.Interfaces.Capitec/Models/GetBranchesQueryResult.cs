using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetBranchesQueryResult
    {
        public Guid Id { get; set; }

        public string BranchName { get; set; }

        public string SuburbName { get; set; }

        public string ProvinceName { get; set; }

        public string IsActive { get; set; }

        public string NumberOfUsers { get; set; }
    }
}
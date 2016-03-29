using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetBranchQueryResult
    {
        public Guid Id { get; set; }

        public string BranchName { get; set; }

        public string BranchCode { get; set; }

        public bool IsActive { get; set; }

        public Guid SuburbId { get; set; }

        public string SuburbName { get; set; }
    }
}
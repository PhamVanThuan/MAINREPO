using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetSuburbsQueryResult
    {
        public Guid Id { get; set; }

        public string SuburbName { get; set; }

        public string ProvinceName { get; set; }

        public int NumberOfBranches { get; set; }
    }
}
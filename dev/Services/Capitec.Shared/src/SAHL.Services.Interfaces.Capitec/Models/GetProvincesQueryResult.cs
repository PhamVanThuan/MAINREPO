using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetProvincesQueryResult
    {
        public Guid Id { get; set; }

        public string ProvinceName { get; set; }

        public int NumberOfBranches { get; set; }

        public string CountryName { get; set; }
    }
}
using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetCitiesQueryResult
    {
        public Guid Id { get; set; }

        public string CityName { get; set; }

        public int NumberOfBranches { get; set; }
    }
}
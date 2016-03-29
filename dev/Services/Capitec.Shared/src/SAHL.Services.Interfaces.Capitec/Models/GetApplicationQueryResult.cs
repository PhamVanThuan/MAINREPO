using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetApplicationQueryResult
    {
        public Guid Id { get; set; }

        public DateTime ApplicationDate { get; set; }

        public Guid UserId { get; set; }

        public string ApplicationNumber { get; set; }
    }
}
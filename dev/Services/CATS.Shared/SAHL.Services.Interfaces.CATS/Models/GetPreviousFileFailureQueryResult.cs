using System;

namespace SAHL.Services.Interfaces.CATS.Models
{
    public class GetPreviousFileFailureQueryResult
    {
        public int CATSPaymentBathKey { get; set; }
        public string CATSFileName { get; set; }
        public DateTime BatchCreationDate { get; set; }
        public DateTime? BatchProcessDate { get; set; }
        public int CATSPaymentBatchStatusKey { get; set; }
    }
}

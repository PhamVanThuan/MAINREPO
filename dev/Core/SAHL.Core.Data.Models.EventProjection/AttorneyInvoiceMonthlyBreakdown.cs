using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class AttorneyInvoiceMonthlyBreakdownDataModel :  IDataModel
    {
        public AttorneyInvoiceMonthlyBreakdownDataModel(Guid attorneyId, string attorneyName, decimal capitalised, decimal? paidBySPV, decimal? debtReview, decimal? total, decimal avgRValuePerInvoice, decimal avgRValuePerAccount, int paid, int rejected, int unprocessed, int processed, int accountsPaid)
        {
            this.AttorneyId = attorneyId;
            this.AttorneyName = attorneyName;
            this.Capitalised = capitalised;
            this.PaidBySPV = paidBySPV;
            this.DebtReview = debtReview;
            this.Total = total;
            this.AvgRValuePerInvoice = avgRValuePerInvoice;
            this.AvgRValuePerAccount = avgRValuePerAccount;
            this.Paid = paid;
            this.Rejected = rejected;
            this.Unprocessed = unprocessed;
            this.Processed = processed;
            this.AccountsPaid = accountsPaid;
		
        }		

        public Guid AttorneyId { get; set; }

        public string AttorneyName { get; set; }

        public decimal Capitalised { get; set; }

        public decimal? PaidBySPV { get; set; }

        public decimal? DebtReview { get; set; }

        public decimal? Total { get; set; }

        public decimal AvgRValuePerInvoice { get; set; }

        public decimal AvgRValuePerAccount { get; set; }

        public int Paid { get; set; }

        public int Rejected { get; set; }

        public int Unprocessed { get; set; }

        public int Processed { get; set; }

        public int AccountsPaid { get; set; }
    }
}
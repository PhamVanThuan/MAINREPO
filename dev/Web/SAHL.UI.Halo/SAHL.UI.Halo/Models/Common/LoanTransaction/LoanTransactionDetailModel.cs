using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Common.LoanTransaction
{
    public class LoanTransactionDetailModel : IHaloTileModel
    {
        public int FinancialTransactionKey
        { get; set; }
        public DateTime InsertDate
        { get; set; }
        public DateTime EffectiveDate
        { get; set; }
        public decimal InterestRate
        { get; set; }
        public decimal Amount
        { get; set; }
        public decimal Balance
        { get; set; }
        public string Reference
        { get; set; }
        public string UserID
        { get; set; }
        public decimal AccountBalance
        { get; set; }
        public string TransactionTypeDescription
        { get; set; }
        public string TransactionTypeHTMLColour
        { get; set; }
        public string FinancialService
        { get; set; }
        public string TransactionGroup
        { get; set; }
    }
}
using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class NewPurchaseApplicationLoanDetailDataModel :  IDataModel
    {
        public NewPurchaseApplicationLoanDetailDataModel(decimal deposit, decimal purchasePrice)
        {
            this.Deposit = deposit;
            this.PurchasePrice = purchasePrice;
		
        }

        public NewPurchaseApplicationLoanDetailDataModel(Guid id, decimal deposit, decimal purchasePrice)
        {
            this.Id = id;
            this.Deposit = deposit;
            this.PurchasePrice = purchasePrice;
		
        }		

        public Guid Id { get; set; }

        public decimal Deposit { get; set; }

        public decimal PurchasePrice { get; set; }
    }
}
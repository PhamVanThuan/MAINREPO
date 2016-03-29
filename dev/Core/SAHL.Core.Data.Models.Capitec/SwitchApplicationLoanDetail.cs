using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class SwitchApplicationLoanDetailDataModel :  IDataModel
    {
        public SwitchApplicationLoanDetailDataModel(decimal cashRequired, decimal currentBalance, decimal estimatedMarketValueOfTheHome, decimal interimInterest)
        {
            this.CashRequired = cashRequired;
            this.CurrentBalance = currentBalance;
            this.EstimatedMarketValueOfTheHome = estimatedMarketValueOfTheHome;
            this.InterimInterest = interimInterest;
		
        }

        public SwitchApplicationLoanDetailDataModel(Guid id, decimal cashRequired, decimal currentBalance, decimal estimatedMarketValueOfTheHome, decimal interimInterest)
        {
            this.Id = id;
            this.CashRequired = cashRequired;
            this.CurrentBalance = currentBalance;
            this.EstimatedMarketValueOfTheHome = estimatedMarketValueOfTheHome;
            this.InterimInterest = interimInterest;
		
        }		

        public Guid Id { get; set; }

        public decimal CashRequired { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal EstimatedMarketValueOfTheHome { get; set; }

        public decimal InterimInterest { get; set; }
    }
}
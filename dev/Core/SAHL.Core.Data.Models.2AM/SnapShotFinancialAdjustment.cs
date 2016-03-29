using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SnapShotFinancialAdjustmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SnapShotFinancialAdjustmentDataModel(int? snapShotFinancialServiceKey, int? accountKey, int? financialAdjustmentKey, int? financialServiceKey, int? financialAdjustmentSourceKey, int? financialAdjustmentTypeKey, int? financialAdjustmentStatusKey, DateTime? fromDate, DateTime? endDate, DateTime? cancellationDate, int? cancellationReasonKey, int? transactionTypeKey, decimal? fRARate, decimal? iRAAdjustment, decimal? rPAReversalPercentage, decimal? dPADifferentialAdjustment, int? dPABalanceTypeKey, decimal? amount)
        {
            this.SnapShotFinancialServiceKey = snapShotFinancialServiceKey;
            this.AccountKey = accountKey;
            this.FinancialAdjustmentKey = financialAdjustmentKey;
            this.FinancialServiceKey = financialServiceKey;
            this.FinancialAdjustmentSourceKey = financialAdjustmentSourceKey;
            this.FinancialAdjustmentTypeKey = financialAdjustmentTypeKey;
            this.FinancialAdjustmentStatusKey = financialAdjustmentStatusKey;
            this.FromDate = fromDate;
            this.EndDate = endDate;
            this.CancellationDate = cancellationDate;
            this.CancellationReasonKey = cancellationReasonKey;
            this.TransactionTypeKey = transactionTypeKey;
            this.FRARate = fRARate;
            this.IRAAdjustment = iRAAdjustment;
            this.RPAReversalPercentage = rPAReversalPercentage;
            this.DPADifferentialAdjustment = dPADifferentialAdjustment;
            this.DPABalanceTypeKey = dPABalanceTypeKey;
            this.Amount = amount;
		
        }
		[JsonConstructor]
        public SnapShotFinancialAdjustmentDataModel(int snapShotFinancialAdjustmentKey, int? snapShotFinancialServiceKey, int? accountKey, int? financialAdjustmentKey, int? financialServiceKey, int? financialAdjustmentSourceKey, int? financialAdjustmentTypeKey, int? financialAdjustmentStatusKey, DateTime? fromDate, DateTime? endDate, DateTime? cancellationDate, int? cancellationReasonKey, int? transactionTypeKey, decimal? fRARate, decimal? iRAAdjustment, decimal? rPAReversalPercentage, decimal? dPADifferentialAdjustment, int? dPABalanceTypeKey, decimal? amount)
        {
            this.SnapShotFinancialAdjustmentKey = snapShotFinancialAdjustmentKey;
            this.SnapShotFinancialServiceKey = snapShotFinancialServiceKey;
            this.AccountKey = accountKey;
            this.FinancialAdjustmentKey = financialAdjustmentKey;
            this.FinancialServiceKey = financialServiceKey;
            this.FinancialAdjustmentSourceKey = financialAdjustmentSourceKey;
            this.FinancialAdjustmentTypeKey = financialAdjustmentTypeKey;
            this.FinancialAdjustmentStatusKey = financialAdjustmentStatusKey;
            this.FromDate = fromDate;
            this.EndDate = endDate;
            this.CancellationDate = cancellationDate;
            this.CancellationReasonKey = cancellationReasonKey;
            this.TransactionTypeKey = transactionTypeKey;
            this.FRARate = fRARate;
            this.IRAAdjustment = iRAAdjustment;
            this.RPAReversalPercentage = rPAReversalPercentage;
            this.DPADifferentialAdjustment = dPADifferentialAdjustment;
            this.DPABalanceTypeKey = dPABalanceTypeKey;
            this.Amount = amount;
		
        }		

        public int SnapShotFinancialAdjustmentKey { get; set; }

        public int? SnapShotFinancialServiceKey { get; set; }

        public int? AccountKey { get; set; }

        public int? FinancialAdjustmentKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public int? FinancialAdjustmentSourceKey { get; set; }

        public int? FinancialAdjustmentTypeKey { get; set; }

        public int? FinancialAdjustmentStatusKey { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CancellationDate { get; set; }

        public int? CancellationReasonKey { get; set; }

        public int? TransactionTypeKey { get; set; }

        public decimal? FRARate { get; set; }

        public decimal? IRAAdjustment { get; set; }

        public decimal? RPAReversalPercentage { get; set; }

        public decimal? DPADifferentialAdjustment { get; set; }

        public int? DPABalanceTypeKey { get; set; }

        public decimal? Amount { get; set; }

        public void SetKey(int key)
        {
            this.SnapShotFinancialAdjustmentKey =  key;
        }
    }
}
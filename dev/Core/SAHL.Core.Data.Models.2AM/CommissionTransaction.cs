using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CommissionTransactionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CommissionTransactionDataModel(int? financialServiceKey, int financialServiceTypeKey, double commissionCalcAmount, double? commissionAmount, decimal? commissionFactor, string commissionType, double? kickerCalcAmount, double? kickerAmount, DateTime transactionDate, int? batchTypeKey, DateTime? batchRunDate, int aDUserKey)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.CommissionCalcAmount = commissionCalcAmount;
            this.CommissionAmount = commissionAmount;
            this.CommissionFactor = commissionFactor;
            this.CommissionType = commissionType;
            this.KickerCalcAmount = kickerCalcAmount;
            this.KickerAmount = kickerAmount;
            this.TransactionDate = transactionDate;
            this.BatchTypeKey = batchTypeKey;
            this.BatchRunDate = batchRunDate;
            this.ADUserKey = aDUserKey;
		
        }
		[JsonConstructor]
        public CommissionTransactionDataModel(int commissionTransactionKey, int? financialServiceKey, int financialServiceTypeKey, double commissionCalcAmount, double? commissionAmount, decimal? commissionFactor, string commissionType, double? kickerCalcAmount, double? kickerAmount, DateTime transactionDate, int? batchTypeKey, DateTime? batchRunDate, int aDUserKey)
        {
            this.CommissionTransactionKey = commissionTransactionKey;
            this.FinancialServiceKey = financialServiceKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.CommissionCalcAmount = commissionCalcAmount;
            this.CommissionAmount = commissionAmount;
            this.CommissionFactor = commissionFactor;
            this.CommissionType = commissionType;
            this.KickerCalcAmount = kickerCalcAmount;
            this.KickerAmount = kickerAmount;
            this.TransactionDate = transactionDate;
            this.BatchTypeKey = batchTypeKey;
            this.BatchRunDate = batchRunDate;
            this.ADUserKey = aDUserKey;
		
        }		

        public int CommissionTransactionKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public int FinancialServiceTypeKey { get; set; }

        public double CommissionCalcAmount { get; set; }

        public double? CommissionAmount { get; set; }

        public decimal? CommissionFactor { get; set; }

        public string CommissionType { get; set; }

        public double? KickerCalcAmount { get; set; }

        public double? KickerAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public int? BatchTypeKey { get; set; }

        public DateTime? BatchRunDate { get; set; }

        public int ADUserKey { get; set; }

        public void SetKey(int key)
        {
            this.CommissionTransactionKey =  key;
        }
    }
}